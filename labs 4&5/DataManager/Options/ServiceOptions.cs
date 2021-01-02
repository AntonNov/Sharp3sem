using System.ComponentModel.DataAnnotations;
using ConfigurationManager;

namespace DataManager.Options
{
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
    }
}