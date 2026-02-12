namespace IATMS.Configurations
{
    public class AppSettings
    {
        private static readonly IConfiguration app_setting = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        public static string AccessSecretKey { get; } = app_setting["Jwt:AccessSecret"];
        public static string RefreshSecretKey { get; } = app_setting["Jwt:RefreshSecret"];
        public static int AccessLiftTime { get; } = int.Parse(app_setting["Jwt:AccessLiftTime"]);
        public static int RefreshLiftTime { get; } = int.Parse(app_setting["Jwt:RefreshLiftTime"]);
        public static string DatabaseConnectionString { get; } = app_setting.GetConnectionString(app_setting["Env:db"]);
        public static string LdapServer { get; } = app_setting["Ldap:Server"];
        public static string LdapPort { get; } = app_setting["Ldap:Port"];
        public static string LdapPath => $"LDAP://{LdapServer}:{LdapPort}";

        public static string LdapSearchUser = app_setting["LdapSearchUser"];
        public static string LdapSearchPassword = app_setting["LdapSearchPassword"];
        public static string LdapName = app_setting["LdapName"];
    }
}
