namespace IATMS.Configurations
{
    public class AppSettings
    {
        private static readonly IConfiguration app_setting = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        public static string AccessSecretKey { get; } = app_setting["Jwt:AccessSecret"];
        public static string RefreshSecretKey { get; } = app_setting["Jwt:RefreshSecret"];
        public static int AccessLiftTime { get; } = int.Parse(app_setting["Jwt:AccessLiftTime"]);
        public static int RefreshLiftTime { get; } = int.Parse(app_setting["Jwt:RefreshLiftTime"]);
    }
}
