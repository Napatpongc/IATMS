namespace IATMS.Models.Responses
{
    public class Res_Login : Message
    {
        public string token { get; set; }
        public string refresh_token { get; set; }
    }
}
