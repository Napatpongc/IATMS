namespace IATMS.Models.Payloads
{
    public class Pay_Role
    {
        public string role_id { get; set; }
        public string description { get; set; }
        public int role_level { get; set; }

        public bool menu_attendance { get; set; }
        public bool menu_report { get; set; }
        public bool menu_admin { get; set; }
        public bool menu_setup { get; set; }

        public bool func_approve { get; set; }
        public bool func_cico { get; set; }
        public bool func_rp_attendance { get; set; }
        public bool func_rp_work_hours { get; set; }
        public bool func_rp_compensation { get; set; }

        public bool is_active { get; set; }
        public string username { get; set; } 
        public bool? menu_spare1 { get; set; }
        public bool? menu_spare2 { get; set; }
        public bool? menu_spare3 { get; set; }
        public bool? menu_spare4 { get; set; }
        public bool? menu_spare5 { get; set; }

        public bool? func_spare1 { get; set; }
        public bool? func_spare2 { get; set; }
        public bool? func_spare3 { get; set; }
        public bool? func_spare4 { get; set; }
        public bool? func_spare5 { get; set; }
        public bool? func_spare6 { get; set; }
        public bool? func_spare7 { get; set; }
        public bool? func_spare8 { get; set; }
        public bool? func_spare9 { get; set; }
        public bool? func_spare10 { get; set; }
    }

}
