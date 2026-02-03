using AppName_API.Models.Responses.Authentication;

namespace IATMS.Models.Responses
{
    public class Res_Only_Profile 
    {
        public Res_Role role { get; set; }
        public Res_Profile profile { get; set; }
    }
}
