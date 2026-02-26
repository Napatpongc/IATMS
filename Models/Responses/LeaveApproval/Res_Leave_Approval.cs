namespace IATMS.Models.Responses.LeaveApproval
{
    public class Res_Leave_Approval
    {
        public string oa_user { get; set; }
        public string full_name { get; set; }
        public string team { get; set; }

        public string type_leave_display { get; set; }
        public DateOnly? start_date { get; set; }
        public DateOnly? end_date { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }

        public string status_display { get; set; }
        public string reason { get; set; }
        public string reject_reason { get; set; }

        public int? total_minute { get; set; }
    }
}