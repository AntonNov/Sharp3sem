using System.ComponentModel.DataAnnotations;
using ConfigurationManager;

namespace DataManager.Options
{
    public class PathOptions
    {
        [Required]
        [Name("Templog")]
        public string Templog { get; set; }// = @"C:\Users\quasar\source\repos\lab4\bin\Debug\templog.txt";
        [Required]
        [Name("SourceDirectory")]
        public string SourceDirectory { get; set; }// = @"C:\Users\quasar\source\repos\lab4\bin\Debug\SourceDirectory";
    }
}