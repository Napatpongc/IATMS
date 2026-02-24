namespace IATMS.Models.Responses.Leave
{
    public class Res_Leave
    {
        public string oa_user { get; set; }
        public string type_leave { get; set; }
        public DateOnly? start_date { get; set; }
        public DateOnly? end_date { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
        public string reason { get; set; }
        public string reject_reason { get; set; }
        public string status_request { get; set; }
        public decimal total_minute { get; set; }
    }
}