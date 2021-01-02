using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using FileManager.OptionModels;
using ConfigurationManager;

namespace FileManager
{
    public class FileManager : ServiceBase
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.ServiceName = "FileManager";
        }

        Logger logger;
        private FileSystemWatcher fileWatcher;

        public FileManager()
        {
            fileWatcher = new FileSystemWatcher(Options.PathOptions.SourceDirectory);
            fileWatcher.Created += OnCreated;
            var cOptionsManager = new OptionsManager();
            Options.PathOptions = cOptionsManager.GetConfiguredOptionsModel<PathOptions>();
            Options.ServiceOptions = cOptionsManager.GetConfiguredOptionsModel<ServiceOptions>();

            InitializeComponent();

            this.CanStop = Options.ServiceOptions.CanStop;
            this.CanPauseAndContinue = Options.ServiceOptions.CanPauseAndContinue;
            this.AutoLog = Options.ServiceOptions.AutoLog;
        }

        protected override void OnStart(string[] args)
        {
            logger = new Logger(fileWatcher);
            var loggerThread = new Thread(new ThreadStart(logger.Start));
            loggerThread.Start();
        }

        protected override void OnStop()
        {
            logger.Stop();
            Thread.Sleep(1000);
        }
        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(async state =>
            {
                //GC.Collect(2);
                string pathInTargetDirectory = await SendToTargetDirectory(e.FullPath);
                var archivator = new Archivator();
                archivator.UnZip(e.FullPath, pathInTargetDirectory);
            });
        }

        private async Task<string> SendToTargetDirectory(string oldPath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var file = File.ReadAllBytes(oldPath);

                    var encryption = new Encryption();
                    var cryptedFile = encryption.Crypt(file);

                    var archivator = new Archivator();
                    string newPath = encryption.GetCryptedFilePath(
                        archivator.GetFilePathInTargetDirectoryByTime(new FileInfo(oldPath), true)) + ".gz";

                    string tempPath = Path.Combine(
                        "C:\\Users\\quasar\\source\\repos\\Service1\\bin\\Debug\\",
                        "temp123");

                    new FileStream(tempPath, FileMode.Create)
                        .WriteTo(cryptedFile)
                        .CompressFromFile(newPath)
                        .Dispose();

                    return newPath;
                }
                catch (Exception e)
                {
                    using (var writer = new StreamWriter(Options.PathOptions.Templog, true))
                    {
                        writer.WriteLine(e.Message);
                        writer.Flush();
                    }
                }

                return "";
            });
        }
    }
}