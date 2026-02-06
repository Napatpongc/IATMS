using System.ComponentModel.DataAnnotations;

namespace IATMS.Models.Payloads
{
    public class ListOfValues
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Maximum length")]
        public string fieldName { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Maximum length")]
        public string code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Maximum length")]
        public string description { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Maximum length")]
        public string condition { get; set; }
        
        public int orderIndex { get; set; }
        
        public bool isActive { get; set; }

    }

    public class searchLov
    {
        [MaxLength(255, ErrorMessage = "Maximum length")]
        public string? keyword { get; set; }


    }
}
