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
            if (string.IsNullOrWhiteSpace(oa_user) &&
                string.IsNullOrWhiteSpace(fname) &&
                string.IsNullOrWhiteSpace(lname))
            {
                return res; // คืนค่า Res_Profile เปล่าๆ กลับไป (หรือจะ Return null ก็ได้แล้วแต่คุณออกแบบ)
            }
            try
            {
                string path = AppSettings.LdapPath;
                // ใช้ตัวแปรจาก AppSettings หรือใส่ตรงๆ ตามที่คุณเทสสำเร็จ
                PrincipalContext _context = new(ContextType.Domain,"192.168.1.101", "DC=erm,DC=local", "admin", "@Password123");
                UserPrincipal _up = new(_context);

                // ตั้งค่า Filter ตามพารามิเตอร์ที่ส่งมา
                if (!string.IsNullOrWhiteSpace(oa_user)) _up.SamAccountName = oa_user.Trim();
                if (!string.IsNullOrWhiteSpace(fname)) _up.GivenName = fname.Trim();
                if (!string.IsNullOrWhiteSpace(lname)) _up.Surname = lname.Trim();

                using (PrincipalSearcher _search = new PrincipalSearcher(_up))
                {
                    var searchResult = _search.FindOne();
                    if (searchResult != null)
                    {
                        UserPrincipal usrPrn = UserPrincipal.FindByIdentity(_context, IdentityType.Sid, searchResult.Sid.Value);
                        if (usrPrn != null)
                        {
                            var directoryEntry = (DirectoryEntry)usrPrn.GetUnderlyingObject();

                            string ad_oaUser = GetValue(directoryEntry, "sAMAccountName");
                            string ad_fname = GetValue(directoryEntry, "givenName");
                            string ad_lname = GetValue(directoryEntry, "sn");

                            if (!string.IsNullOrWhiteSpace(oa_user) &&
                            !string.Equals(ad_oaUser, oa_user, StringComparison.OrdinalIgnoreCase))
                                return null;

                            if (!string.IsNullOrWhiteSpace(fname) &&
                                !string.Equals(ad_fname, fname, StringComparison.OrdinalIgnoreCase))
                                return null;

                            if (!string.IsNullOrWhiteSpace(lname) &&
                                !string.Equals(ad_lname, lname, StringComparison.OrdinalIgnoreCase))
                                return null;


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
