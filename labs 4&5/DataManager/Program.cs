using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DataManager
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new DataManager()
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
