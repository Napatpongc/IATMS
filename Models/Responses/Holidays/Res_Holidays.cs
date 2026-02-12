namespace IATMS.Models.Responses.Holidays
{
    public class Res_Holidays
    {
        public DateOnly holidayDate { get; set; }
        public string holidayName { get; set; }
        public bool isActive { get; set; }
    }

    public class Res_HolidayYearRange
    {
        public int maxYear { get; set; }
        public int minYear { get; set; }
    }
    
     public class Res_HolidayYears
    {
        public int Year { get; set; }
    }
}
