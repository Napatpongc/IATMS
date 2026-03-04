namespace IATMS.Models.Payloads.Compensation
{
    public class Pay_Compensation
    {
        // ช่อง "ชื่อ-นามสกุลหรือ OA User"
        public string? search_text { get; set; }

        // Team dropdown (เก็บเป็น team_code หรือชื่อทีม แล้วแต่ DB)
        public string? team { get; set; }

        // วันที่
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
    }
}
