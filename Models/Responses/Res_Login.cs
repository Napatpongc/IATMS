using IATMS.Models.Responses;

namespace AppName_API.Models.Responses.Authentication
{
    public class Res_Login : Message
    {
        public string token { get; set; }
        public string refresh_token { get; set; }
        public Res_Role role { get; set; }
        public Res_Profile profile { get; set; }
    }
    public class Res_Role
    {
        public bool menu_attendance { get; set; }
        public bool menu_report { get; set; }
        public bool menu_admin { get; set; }
        public bool menu_setup { get; set; }

        public bool func_approve { get; set; }
        public bool func_cico { get; set; } 
        public bool func_rp_attendance { get; set; }
        public bool func_rp_work_hours { get; set; }
        public bool func_rp_compensation { get; set; }

        public bool menu_spare1 { get; set; }
        public bool menu_spare2 { get; set; }
        public bool menu_spare3 { get; set; }
        public bool menu_spare4 { get; set; }
        public bool menu_spare5 { get; set; }

        public bool func_spare1 { get; set; }
        public bool func_spare2 { get; set; }
        public bool func_spare3 { get; set; }
        public bool func_spare4 { get; set; }
        public bool func_spare5 { get; set; }
        public bool func_spare6 { get; set; }
        public bool func_spare7 { get; set; }
        public bool func_spare8 { get; set; }
        public bool func_spare9 { get; set; }
        public bool func_spare10 { get; set; }

    }

    public class Res_Profile
    {
        public string oa_user { get; set; }
        public string Name_en { get; set; }
        public string Name_th { get; set; }
        public string division_code { get; set; }
        public string Team { get; set; }
        public string Work_Place { get; set; }
        public string email { get; set; }
        public string role_id { get; set; }
    }
}
