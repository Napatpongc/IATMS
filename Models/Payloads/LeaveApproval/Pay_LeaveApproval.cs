namespace IATMS.Models.Payloads.LeaveApproval
{
    public class Search_Leave_Approval
    {
        public string? Search { get; set; }
        public string? Team { get; set; }

    }
    public class Pay_LeaveApproval
    {
        public string oa_user { get; set; }
        public DateOnly start_date { get; set; }
        public DateOnly end_date { get; set; }
        public string action { get; set; }
        public string? reject_reason { get; set; }
    }
}
