namespace IATMS.Models.Responses.DashBoard
{
    public class BaseDashboardResponse
    {
        public bool MenuIntern { get; set; }
        public bool MenuTeamled { get; set; }
        public bool MenuAdmin { get; set; }
        public bool MenuManager { get; set; }
        public string Displaydate { get; set; }
    }

    public class StaffDashboardResponse : BaseDashboardResponse
    {
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }

        public string CiAddress { get; set; }
        public string CoAddress { get; set; }

        public int WorkingMinutes { get; set; }
        public int ApproveLeave { get; set; }
        public int PendingLeave { get; set; }
        public int RejectLeave { get; set; }
    }

    public class ManagementDashboardResponse : BaseDashboardResponse
    {
        public string CheckInSummary { get; set; }
        public string CiLateCount { get; set; }
        public string CiOutsideCount { get; set; }

        public string CoSummary { get; set; }
        public string CoEarlyCount { get; set; }
        public string CoOutsideCount { get; set; }

        public int PendingRequests { get; set; }
    }

    public class AdminDashboardResponse : BaseDashboardResponse
    {

    }
}