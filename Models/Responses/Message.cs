namespace IATMS.Models.Responses
{
    public class Message
    {
        public int code { get; set; }
        public string message { get; set; }
    }
    public class Error
    {
        public List<Message> errors { get; set; } = new();
    }
}
