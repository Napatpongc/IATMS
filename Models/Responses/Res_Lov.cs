namespace IATMS.Models.Responses
{
    public class Res_Lov : Message
    {
        public string fieldName { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string condition { get; set; }
        public int orderIndex { get; set; }
        public bool isActive { get; set; }

    }


}
