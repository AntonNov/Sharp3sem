using System.ComponentModel.DataAnnotations;
using ConfigurationManager;

namespace DataAccess.Options
{
    internal class ConnectionStrings
    {
        [Required]
        [Name("ConnectionStringToAdventureWorksLT2019")]
        public string ConnectionStringToAdventureWorksLT2019 { get; set; }// = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=AdventureWorksLT2019;Data Source=DESKTOP-TMEKROF\SQLEXPRESS";
        [Required]
        [Name("SourceDirectory")]
        public string ConnectionStringToErrorLogs { get; set; }// = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=ErrorLogs;Data Source=DESKTOP-TMEKROF\SQLEXPRESS";
    }
}