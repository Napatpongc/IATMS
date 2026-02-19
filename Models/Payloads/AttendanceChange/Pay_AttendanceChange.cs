namespace IATMS.Models.Payloads.AttendanceChange
{
    public class Pay_AttendanceChange
    {
        public DateOnly? startDate { get; set; }
        public DateOnly? endDate { get; set; }
        public string? dropdown { get; set; }
    }
}
