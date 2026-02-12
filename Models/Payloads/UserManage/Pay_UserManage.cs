namespace IATMS.Models.Payloads.UserManage
{
    public class Pay_UserManage
    {
        public string? oa_user { get; set; }
        public string? first_name_th { get; set; }
        public string? last_name_th { get; set; }
        public string? first_name_en { get; set; }
        public string? last_name_en { get; set; }
        public string? email { get; set; }
        public string? division_code { get; set; }
        public string? role_id { get; set; }
        public string? team_code { get; set; }
        public string? work_place { get; set; }
        public bool is_active { get; set; }
        public string? username { get; set; }
        public string? mode { get; set; }
    }
}