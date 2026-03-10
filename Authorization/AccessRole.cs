
using IATMS.contextDB;
using System.Data;

namespace AppName_API.Components.Authorization
{
    public static class AccessRole
    {
        public static bool IsAuthorize(string username, string menu = "", string function = "")
        {
            try
            {
                var profile = ConDB.GetSigninUserProfile(username);
                if (profile?.role == null) return false;

                var role = profile.role;
                bool isMenuOk = true;     // ตั้งต้นเป็น true ถ้าไม่ส่ง menu มาก็ให้ผ่าน
                bool isFunctionOk = true; // ตั้งต้นเป็น true ถ้าไม่ส่ง function มาก็ให้ผ่าน

                // 1. ตรวจสอบสิทธิ์ Menu (ถ้ามีการส่งค่ามา)
                if (!string.IsNullOrEmpty(menu))
                {
                    if (menu == "menu_attendance") isMenuOk = role.menu_attendance;
                    else if (menu == "menu_report") isMenuOk = role.menu_report;
                    else if (menu == "menu_admin") isMenuOk = role.menu_admin;
                    else if (menu == "menu_setup") isMenuOk = role.menu_setup;
                    else if (menu == "menu_spare1") isMenuOk = role.menu_spare1;
                    else if (menu == "menu_spare2") isMenuOk = role.menu_spare2;
                    else if (menu == "menu_spare3") isMenuOk = role.menu_spare3;
                    else if (menu == "menu_spare4") isMenuOk = role.menu_spare4;
                    else if (menu == "menu_spare5") isMenuOk = role.menu_spare5;
                    else isMenuOk = false; // ถ้าส่งชื่อเมนูที่ไม่มีในระบบมา ให้ false
                }

                // 2. ตรวจสอบสิทธิ์ Function (ถ้ามีการส่งค่ามา)
                if (!string.IsNullOrEmpty(function))
                {
                    if (function == "func_approve") isFunctionOk = role.func_approve;
                    else if (function == "func_cico") isFunctionOk = role.func_cico;
                    else if (function == "func_rp_attendance") isFunctionOk = role.func_rp_attendance;
                    else if (function == "func_rp_work_hours") isFunctionOk = role.func_rp_work_hours;
                    else if (function == "func_rp_compensation") isFunctionOk = role.func_rp_compensation;
                    else if (function == "func_spare1") isFunctionOk = role.func_spare1;
                    else if (function == "func_spare2") isFunctionOk = role.func_spare2;
                    else if (function == "func_spare3") isFunctionOk = role.func_spare3;
                    else if (function == "func_spare4") isFunctionOk = role.func_spare4;
                    else if (function == "func_spare5") isFunctionOk = role.func_spare5;
                    else if (function == "func_spare6") isFunctionOk = role.func_spare6;
                    else if (function == "func_spare7") isFunctionOk = role.func_spare7;
                    else if (function == "func_spare8") isFunctionOk = role.func_spare8;
                    else if (function == "func_spare9") isFunctionOk = role.func_spare9;
                    else if (function == "func_spare10") isFunctionOk = role.func_spare10;
                    else isFunctionOk = false;
                }

                // 3. สรุปผล: ต้องเป็น true ทั้งคู่ (ถ้าส่งมาอย่างเดียว อีกอันจะเป็น true โดยปริยาย)
                if (string.IsNullOrEmpty(menu) && string.IsNullOrEmpty(function)) return false;

                return isMenuOk && isFunctionOk;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
