namespace IATMS.Models.Payloads.Leave
{
    public class searchLeave
    {
        public DateOnly? startDate { get; set; }
        public DateOnly? endDate { get; set; }
        public string? status { get; set; } 
    }
    public class Pay_Leave
    {
        public string type_leave { get; set; }
        public DateOnly start_date { get; set; }
        public DateOnly end_date { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
        public string reason { get; set; }
    }
    public class Pay_Delete_Leave
    {
        public DateOnly start_date { get; set; }
        public DateOnly end_date { get; set; }
    }
}