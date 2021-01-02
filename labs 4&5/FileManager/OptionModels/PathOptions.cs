using System.ComponentModel.DataAnnotations;
using ConfigurationManager;

namespace FileManager.OptionModels
{
    public class PathOptions
    {
        [Required]
        [Name("Root")]
        public string Root { get; set; }// = @"C:\Users\quasar\source\repos\Service1\bin\Debug\";
        [Required]
        [Name("Templog")]
        public string Templog { get; set; }// = @"C:\Users\quasar\source\repos\Service1\bin\Debug\templog.txt";
        [Required]
        [Name("SourceDirectory")]
        public string SourceDirectory { get; set; }// = @"C:\Users\quasar\source\repos\Service1\bin\Debug\SourceDirectory";
        [Required]
        [Name("TargetDirectory")]
        public string TargetDirectory { get; set; }// = @"C:\Users\quasar\source\repos\Service1\bin\Debug\TargetDirectory";
    }
}