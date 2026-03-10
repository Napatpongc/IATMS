using System.ComponentModel.DataAnnotations;

namespace IATMS.Models.Payloads.AttendanceHistory
{
    public class Pay_AttendanceHistory
    {
        public DateOnly? start_date { get; set; }
        public DateOnly? end_date { get; set; }

        public string? team { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? search_text { get; set; }

        public string? ci_time_status { get; set; }
        public string? co_time_status { get; set; }

        public string? ci_location_status { get; set; }
        public string? co_location_status { get; set; }
    }
}