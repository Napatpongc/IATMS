namespace IATMS.Models.Payloads.Leave
{
    public class searchLeave
    {
        public string? username { get; set; }
        public DateOnly? startDate { get; set; }
        public DateOnly? endDate { get; set; }
        public string? status { get; set; } 
    }
    public class Pay_Leave
    {
        public string oa_user { get; set; }
        public string type_leave { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
        public string reason { get; set; }
    }
}