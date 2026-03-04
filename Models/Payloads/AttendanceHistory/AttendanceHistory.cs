namespace IATMS.Models.Payloads.AttendanceHistory
{
    public class Pay_AttendanceHistory
    {
        // วันที่ (Start Date - End Date)
        public DateOnly? start_date { get; set; }
        public DateOnly? end_date { get; set; }

        // Team dropdown
        public string? team { get; set; } // หรือ team_code แล้วแต่ระบบ

        // ช่องค้นหา "ชื่อ-นามสกุลหรือ OA User"
        public string? search_text { get; set; }

        // สถานะเวลา (มี 2 dropdown: Check-In / Check-Out)
        public string? ci_time_status { get; set; }
        public string? co_time_status { get; set; }

        // สถานะตำแหน่ง (มี 2 dropdown: Check-In / Check-Out)
        public string? ci_location_status { get; set; }
        public string? co_location_status { get; set; }
    }
}
