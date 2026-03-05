namespace IATMS.Models.Responses.WorkHourHistory
{
    public class Res_HourHistory
    {
        public string oa_user { get; set; }
        public string full_name { get; set; }
        public string team_code { get; set; }
        public string at_date { get; set; }
        public TimeOnly WorkingHour { get; set; }
    }
}