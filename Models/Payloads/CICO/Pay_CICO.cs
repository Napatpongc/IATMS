namespace IATMS.Models.Payloads.CICO
{
    public class Pay_ButtonCICO
    {
        public int mode { get; set; }

    }
    public class Pay_CICO
    {
        public string oa_user { get; set; }

        public string location { get; set; }

        public string address { get; set; }

        public string mac_address { get; set; }

        public string reason { get; set; }
    }
}
