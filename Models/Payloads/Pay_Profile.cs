using System.ComponentModel.DataAnnotations;

namespace IATMS.Models.Payloads
{
    public class Pay_Profile
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Maximum length")]
        public string username { get; set; }
    }
}
