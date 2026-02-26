namespace IATMS.Models.Payloads.AttendanceChange
{
    public class Pay_AttendanceChange
    {
        public DateOnly? startDate { get; set; }
        public DateOnly? endDate { get; set; }
        public string? dropdown { get; set; }
    }
    public class Pay_ModalAttendanceChange
    {
       public string? username { get; set; }

        public DateOnly Date { get; set; }  
    }

    public class Pay_AttendanceChange_post
    {
        public DateOnly at_date { get; set; }
        public DateTime? ci_time_old { get; set; }
        public DateTime? ci_time_new { get; set; }
        public string? ci_location_old { get; set; }
        public string? ci_location_new { get; set; }
        public string? ci_address_old { get; set; }
        public string? ci_address_new { get; set; }
        public string? ci_request_reason { get; set; }   // varchar(100)
        public DateTime? co_time_old { get; set; }
        public DateTime? co_time_new { get; set; }
        public string? co_location_old { get; set; }
        public string? co_location_new { get; set; }
        public string? co_address_old { get; set; }
        public string? co_address_new { get; set; }
        public string? co_request_reason { get; set; }   // varchar(100)
        public string? request_reason { get; set; }      // varchar(100)
    }
}
