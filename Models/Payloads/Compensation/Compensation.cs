namespace IATMS.Models.Payloads.Compensation
{
    public class Pay_Compensation
    {
        public string? search_text { get; set; }
        public string? team { get; set; }

        // NEW: "2026-02"
        public string? start_month_year { get; set; }
        public string? end_month_year { get; set; }
    }

    public class Pay_MonthYearCompensation
    {
        public string? team { get; set; }

    }
}

