using AppName_API.Models.Responses.Authentication;
using IATMS.Configurations;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace IATMS.Components
{
    public class LDAP
    {

        public static Res_Profile GetUserProfile(string? oa_user = null, string? fname = null, string? lname = null)
        {
            Res_Profile res = new();
            try
            {
                string path = AppSettings.LdapPath;
                // ใช้ตัวแปรจาก AppSettings หรือใส่ตรงๆ ตามที่คุณเทสสำเร็จ
                PrincipalContext _context = new(ContextType.Domain, "172.20.10.2", "DC=iatms,DC=local", "ldapuser", "@Int1234");
                UserPrincipal _up = new(_context);

                // ตั้งค่า Filter ตามพารามิเตอร์ที่ส่งมา
                if (!string.IsNullOrEmpty(oa_user)) _up.SamAccountName = oa_user;
                    if (!string.IsNullOrEmpty(fname)) _up.GivenName = fname;
                    if (!string.IsNullOrEmpty(lname)) _up.Surname = lname;

                using (PrincipalSearcher _search = new PrincipalSearcher(_up))
                {
                    var searchResult = _search.FindOne();
                    if (searchResult != null)
                    {
                        UserPrincipal usrPrn = UserPrincipal.FindByIdentity(_context, IdentityType.Sid, searchResult.Sid.Value);
                        if (usrPrn != null)
                        {
                            var directoryEntry = (DirectoryEntry)usrPrn.GetUnderlyingObject();

                            // Mapping ตาม Model Res_Profile ของคุณ
                            res.oa_user = GetValue(directoryEntry, "sAMAccountName"); // nattapol.prai
                            res.Name_en = GetValue(directoryEntry, "displayName");    // Nattapol Prairuenrom
                            res.Name_th = GetValue(directoryEntry, "description");    // ณัฐพล ไพรื่นรมย์
                            res.division_code = GetValue(directoryEntry, "department"); // Enterprise Resource Management
                            res.email = GetValue(directoryEntry, "mail");              // nattapol.prai@ku.th

                            // ฟิลด์เหล่านี้ LDAP ไม่มีค่าตรงๆ แนะนำให้ Admin เลือกเองในหน้าจอ หรือ Default ไว้
                            res.Team = "";
                            res.Work_Place = "";
                            res.role_id = "";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LDAP Error: " + ex.Message);
            }
            return res;
        }

        // ฟังก์ชันดึงค่า Attribute ป้องกัน Error กรณีไม่มีค่านั้น
        private static string GetValue(DirectoryEntry de, string propertyName)
        {
            if (de.Properties.Contains(propertyName))
            {
                return de.Properties[propertyName].Value?.ToString() ?? "";
            }
            return "";
        }
    }
}
