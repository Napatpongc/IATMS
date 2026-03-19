namespace IATMS.Models.Responses.DashBoard
{
    public class BaseDashboardResponse
    {
        // ปรับชื่อให้ตรงกับ SQL (menu_intern, menu_teamled, ...)
        public bool menu_intern { get; set; }
        public bool menu_teamled { get; set; }
        public bool menu_admin { get; set; }
        public bool menu_manager { get; set; }
        
    }

    public class StaffDashboardResponse : BaseDashboardResponse
    {
        public string check_in { get; set; }
        public string check_out { get; set; }
        public string ci_address { get; set; }
        public string co_address { get; set; }
        public TimeOnly working_hour { get; set; }
        public int approve { get; set; }
        public int pending{ get; set; }
        public int reject { get; set; }
        public string displaydate { get; set; }
    }

    public class ManagementDashboardResponse : BaseDashboardResponse
    {
        public string check_in_summary { get; set; }
        public string ci_late_count { get; set; }
        public string ci_outside_count { get; set; }
        public string co_summary { get; set; }
        public string co_early_count { get; set; }
        public string co_outside_count { get; set; }
        public int pending_requests { get; set; }
        public string displaydate { get; set; }
    }
    public class AdminDashboardResponse : BaseDashboardResponse
    {

    }
}