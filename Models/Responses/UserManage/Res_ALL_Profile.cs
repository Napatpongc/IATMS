namespace IATMS.Models.Responses.User_Manage
{
    public class Res_ALL_Profile
    {
        public string? oa_user { get; set; }
        public string? first_name_th { get; set; }
        public string? last_name_th { get; set; }
        public string? first_name_en { get; set; }
        public string? last_name_en { get; set; }
        public string? email { get; set; }

        public string? role { get; set; }

        public string? team { get; set; }

        public string? division { get; set; }

        public string? work_place { get; set; }

        public bool is_active { get; set; }
    }
}