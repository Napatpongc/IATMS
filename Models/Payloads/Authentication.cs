using System.ComponentModel.DataAnnotations;

namespace IATMS.Models.Payloads
{
    public class Pay_signin 
    {
        [Required]
        [MaxLength(100,ErrorMessage ="Maximum length")]
        public string username { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Maximum length")]
        public string password { get; set; }
    }
}
