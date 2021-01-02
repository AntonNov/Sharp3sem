using System.ComponentModel.DataAnnotations;
using ConfigurationManager;

namespace FileManager.OptionModels
{
    public class Shit
    {
        public bool shit { get; set; }// = true;
    }
    public class ServiceOptions
    {
        [Required]
        [Name("CanStop")]
        public bool CanStop { get; set; }// = true;
        [Required]
        [Name("CanPauseAndContinue")]
        public bool CanPauseAndContinue { get; set; }// = true;
        [Required]
        [Name("AutoLog")]
        public bool AutoLog { get; set; }// = true;
        [Required]
        [Name("Shit")]
        public Shit Shit { get; set; }
    }
}