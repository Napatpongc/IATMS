namespace IATMS.Models.Payloads.AttendanceApproval
{
    public class AttendanceApproval
    {
        public string? Name { get; set; }
        public string? Team { get; set; }
    }
    public class post_AttendanceApproval
    {
        public string? oa_user { get; set; }
        public DateOnly at_date { get; set; }    
        public bool isApprove { get; set; }

        public DateTime? ci_time_new { get; set; }
        public string? ci_location_new { get; set; }
        public string? ci_address_new { get; set; }

        public DateTime? co_time_new { get; set; }
        public string? co_location_new { get; set; }
        public string? co_address_new { get; set; }

        public string? rejectReason { get; set; }
    }
}
