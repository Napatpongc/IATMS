using System.ComponentModel.DataAnnotations;

namespace IATMS.Models.Payloads
{
    public class getHolidays
    {
        public int yearSearch { get; set; } 

        public bool isActive { get; set; } 
    }

    public class postHolidays
    {
        public DateOnly holidayDate { get; set; }

        public string holidayName { get; set; }

        public bool isActive { get; set; }
    }
}
