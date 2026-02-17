namespace IATMS.Models.Responses.CheckinCheckout
{
    public class getButton
    {
        public string oaUser { get; set; }
        public DateOnly attDate { get; set; }
        public bool canCi { get; set; }
        public bool canCo { get; set; }

    }

    public class getCICO
    {
        public DateOnly attDate { get; set; }
        public string ciTime { get; set; }
        public string ciCorrectTime { get; set; }
        public string ciCorrectZone { get; set; }
        public string ciReason { get; set; }
        public string coTime { get; set; }
        public string coCorrectTime { get; set; }
        public string coCorrectZone { get; set; }
        public string coReason { get; set; }
        public bool isNomal { get; set; }

    }
}
