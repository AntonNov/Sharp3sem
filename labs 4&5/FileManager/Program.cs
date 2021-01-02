using System;
using System.IO;
using System.ServiceProcess;

namespace FileManager
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new FileManager()
            };
            try
            {
                ServiceBase.Run(ServicesToRun);
            }
            catch (Exception e)
            {
                using (var writer = new StreamWriter(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templog.txt"), 
                    true))
                {
                    writer.WriteLine(e.Message);
                }
            }
        }
    }
}
