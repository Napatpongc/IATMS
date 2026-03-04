namespace IATMS.Models.Responses.AttendanceHistory
{
    public class Res_AttendanceHistory
    {
        public DateOnly attDate { get; set; }

        public string oauser { get; set; }

        public string fullName { get; set; }

        public string team{ get; set; }
        public string ciTime { get; set; }
        public string ciCorrectTime { get; set; }
        public string ciAddress { get; set; }
        public string ciCorrectZone { get; set; }
        public string ciReason { get; set; }
        public string coTime { get; set; }
        public string coCorrectTime { get; set; }
        public string coAddress { get; set; }
        public string coCorrectZone { get; set; }
        public string coReason { get; set; }
    }
}
