namespace IATMS.Models.Response.Compensation
{
    public class Res_Compensation
    {
        // เดือน-ปี เช่น "01/2025"
        public string monthYear { get; set; }

        public string oaUser { get; set; }
        public string fullName { get; set; }
        public string team { get; set; }

        // จำนวนชั่วโมงทำงาน (ชั่วโมง)
        public decimal workHours { get; set; }

        // จำนวนเงิน (บาท)
        public decimal amount { get; set; }
    }

    public class Res_MonthYear
    {
        // เดือน-ปี เช่น "01/2025"
        public string monthYear { get; set; }
    }
}
