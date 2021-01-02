using System;
using System.IO;
using System.Threading;
using FileManager.OptionModels;

namespace FileManager
{
    class Logger
    {
        FileSystemWatcher watcher;
        object obj = new object();
        bool enabled = true;
        public Logger(FileSystemWatcher fileWatcher)
        {
            watcher = fileWatcher;
            watcher.Deleted += OnDeleted;
            watcher.Created += OnCreated;
            watcher.Changed += OnChanged;
            watcher.Renamed += OnRenamed;
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
            while (enabled)
            {
                Thread.Sleep(1000);
            }
        }
        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            enabled = false;
        }
        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            string fileEvent = "переименован в " + e.FullPath;
            string filePath = e.OldFullPath;
            RecordEntry(fileEvent, filePath);
        }
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "изменен";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }
        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "создан";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }
        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            //OnSmth(e.FullPath);
            string fileEvent = "удален";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }

        private void RecordEntry(string fileEvent, string filePath)
        {
            lock (obj)
            {
                using (StreamWriter writer = new StreamWriter(Options.PathOptions.Templog, true))
                {
                    writer.WriteLine(String.Format("{0} файл {1} был {2}",
                        DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath, fileEvent));
                    writer.Flush();
                }
            }
        }
    }
}