using AppName_API.Models.Responses.Authentication;
using Azure.Core;
using IATMS.Components;
using IATMS.Configurations;
using IATMS.Models.Authentications;
using IATMS.Models.Responses;
using IATMS.Models.Responses.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Runtime;
using IATMS.Models.Payloads;
namespace IATMS.contextDB
{
    public class ConDB
    {
        //DBNull.Value
        public static readonly int Timeout = 300;
        public static readonly string connectionString = AppSettings.DatabaseConnectionString;
        public static Res_Login GetSigninUserProfile(string username)
        {
            Res_Login result = new();
            result.role = new Res_Role();
            result.profile = new Res_Profile();
            try
            {
                using var con = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("dbo.getProfile", con);

                cmd.CommandTimeout = Timeout;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@OA_User", SqlDbType.VarChar, 50).Value = username;

                con.Open();
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    // --- 1. Mapping Profile (ตาม Res_Profile) ---
                    result.profile.oa_user = rd["oa_user"]?.ToString();
                    result.profile.Name_en = rd["Name_en"]?.ToString();
                    result.profile.Name_th = rd["Name_th"]?.ToString();
                    result.profile.division_code = rd["division_code"]?.ToString();
                    result.profile.Team = rd["Team"]?.ToString();
                    result.profile.Work_Place = rd["Work_Place"]?.ToString();
                    result.profile.email = rd["email"]?.ToString();
                    result.profile.role_id = rd["role_id"]?.ToString();


                    // --- 2. Mapping Role & Menus (ตาม Res_Role) ---
                    result.role.menu_attendance = rd["menu_attendance"] != DBNull.Value && Convert.ToBoolean(rd["menu_attendance"]);
                    result.role.menu_report = rd["menu_report"] != DBNull.Value && Convert.ToBoolean(rd["menu_report"]);
                    result.role.menu_admin = rd["menu_admin"] != DBNull.Value && Convert.ToBoolean(rd["menu_admin"]);
                    result.role.menu_setup = rd["menu_setup"] != DBNull.Value && Convert.ToBoolean(rd["menu_setup"]);

                    // --- 3. Mapping Functions (ตาม Res_Role) ---
                    result.role.func_approve = rd["func_approve"] != DBNull.Value && Convert.ToBoolean(rd["func_approve"]);
                    result.role.func_cico = rd["func_cico"] != DBNull.Value && Convert.ToBoolean(rd["func_cico"]);
                    result.role.func_rp_attendance = rd["func_rp_attendance"] != DBNull.Value && Convert.ToBoolean(rd["func_rp_attendance"]);
                    result.role.func_rp_work_hours = rd["func_rp_work_hours"] != DBNull.Value && Convert.ToBoolean(rd["func_rp_work_hours"]);
                    result.role.func_rp_compensation = rd["func_rp_compensation"] != DBNull.Value && Convert.ToBoolean(rd["func_rp_compensation"]);

                    // --- 4. Mapping Menu Spares (1-5) ---
                    result.role.menu_spare1 = rd["menu_spare1"] != DBNull.Value && Convert.ToBoolean(rd["menu_spare1"]);
                    result.role.menu_spare2 = rd["menu_spare2"] != DBNull.Value && Convert.ToBoolean(rd["menu_spare2"]);
                    result.role.menu_spare3 = rd["menu_spare3"] != DBNull.Value && Convert.ToBoolean(rd["menu_spare3"]);
                    result.role.menu_spare4 = rd["menu_spare4"] != DBNull.Value && Convert.ToBoolean(rd["menu_spare4"]);
                    result.role.menu_spare5 = rd["menu_spare5"] != DBNull.Value && Convert.ToBoolean(rd["menu_spare5"]);

                    // --- 5. Mapping Function Spares (1-10) ---
                    result.role.func_spare1 = rd["func_spare1"] != DBNull.Value && Convert.ToBoolean(rd["func_spare1"]);
                    result.role.func_spare2 = rd["func_spare2"] != DBNull.Value && Convert.ToBoolean(rd["func_spare2"]);
                    result.role.func_spare3 = rd["func_spare3"] != DBNull.Value && Convert.ToBoolean(rd["func_spare3"]);
                    result.role.func_spare4 = rd["func_spare4"] != DBNull.Value && Convert.ToBoolean(rd["func_spare4"]);
                    result.role.func_spare5 = rd["func_spare5"] != DBNull.Value && Convert.ToBoolean(rd["func_spare5"]);
                    result.role.func_spare6 = rd["func_spare6"] != DBNull.Value && Convert.ToBoolean(rd["func_spare6"]);
                    result.role.func_spare7 = rd["func_spare7"] != DBNull.Value && Convert.ToBoolean(rd["func_spare7"]);
                    result.role.func_spare8 = rd["func_spare8"] != DBNull.Value && Convert.ToBoolean(rd["func_spare8"]);
                    result.role.func_spare9 = rd["func_spare9"] != DBNull.Value && Convert.ToBoolean(rd["func_spare9"]);
                    result.role.func_spare10 = rd["func_spare10"] != DBNull.Value && Convert.ToBoolean(rd["func_spare10"]);
                }
                rd.Close();
                cmd.Dispose();
                con.Close();

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<int> refreshTokenValidate(string username, string refresh_token)
        {
            int result = 0;
            try
            {
                using var conn = new SqlConnection(connectionString);
                var cmd = new SqlCommand("dbo.checkRefreshToken", conn);
                cmd.CommandTimeout = Timeout;
                cmd.CommandType = CommandType.StoredProcedure;

                // parameters
                cmd.Parameters.Add("@OA_User", SqlDbType.VarChar, 50).Value = username;
                cmd.Parameters.Add("@TokenRefresh", SqlDbType.VarChar, -1).Value = refresh_token;

                conn.Open();
                var rd = await cmd.ExecuteReaderAsync();
                while (rd.Read())
                {
                    result = Convert.ToInt32(rd["ExpireToken"]);
                }
                rd.Close();
                cmd.Dispose();
                conn.Close();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
        public static async Task tokenRefresh(string username, string refresh_token, DateTime expire_date)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                var cmd = new SqlCommand("dbo.postRefreshToken", conn);
                cmd.CommandTimeout = Timeout;
                cmd.CommandType = CommandType.StoredProcedure;

                // parameters
                cmd.Parameters.Add("@OA_User", SqlDbType.VarChar, 50).Value = username;
                cmd.Parameters.Add("@TokenRefresh", SqlDbType.VarChar, -1).Value = refresh_token;
                cmd.Parameters.Add("@TokenExpire", SqlDbType.DateTime).Value = expire_date;

                conn.Open();
                await cmd.ExecuteNonQueryAsync();
                cmd.Dispose();
                conn.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<Res_AllRole> GetRoles()
        {
            List<Res_AllRole> result = new();
            try
            {
                using var con = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("dbo.getRoles", con);

                cmd.CommandTimeout = Timeout;
                cmd.CommandType = CommandType.StoredProcedure;


                con.Open();
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    var item = new Res_AllRole();

                    item.role_id = rd["role_id"]?.ToString();
                    item.description = rd["description"]?.ToString();
                    item.role_level = rd["role_level"] != DBNull.Value ? Convert.ToInt32(rd["role_level"]) : 0;

                    item.menu_attendance = rd["menu_attendance"] != DBNull.Value && Convert.ToBoolean(rd["menu_attendance"]);
                    item.menu_report = rd["menu_report"] != DBNull.Value && Convert.ToBoolean(rd["menu_report"]);
                    item.menu_admin = rd["menu_admin"] != DBNull.Value && Convert.ToBoolean(rd["menu_admin"]);
                    item.menu_setup = rd["menu_setup"] != DBNull.Value && Convert.ToBoolean(rd["menu_setup"]);

                    item.func_approve = rd["func_approve"] != DBNull.Value && Convert.ToBoolean(rd["func_approve"]);
                    item.func_cico = rd["func_cico"] != DBNull.Value && Convert.ToBoolean(rd["func_cico"]);
                    item.func_rp_attendance = rd["func_rp_attendance"] != DBNull.Value && Convert.ToBoolean(rd["func_rp_attendance"]);
                    item.func_rp_work_hours = rd["func_rp_work_hours"] != DBNull.Value && Convert.ToBoolean(rd["func_rp_work_hours"]);
                    item.func_rp_compensation = rd["func_rp_compensation"] != DBNull.Value && Convert.ToBoolean(rd["func_rp_compensation"]);

                    item.is_active = rd["is_active"] != DBNull.Value && Convert.ToBoolean(rd["is_active"]);

                    item.menu_spare1 = rd["menu_spare1"] != DBNull.Value ? Convert.ToBoolean(rd["menu_spare1"]) : (bool?)null;
                    item.func_spare1 = rd["func_spare1"] != DBNull.Value ? Convert.ToBoolean(rd["func_spare1"]) : (bool?)null;
                    result.Add(item);
                }
                rd.Close();
                cmd.Dispose();
                con.Close();

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static async Task<bool> PostRole(Pay_Role data)
        {
            try
            {
                using var con = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("dbo.postRole", con);

                cmd.CommandTimeout = Timeout;
                cmd.CommandType = CommandType.StoredProcedure;

                // --- 1. ข้อมูลหลัก (Main Data) ---
                cmd.Parameters.Add("@role_id", SqlDbType.VarChar, 50).Value = data.role_id;
                cmd.Parameters.Add("@description", SqlDbType.VarChar, 255).Value = (object)data.description ?? DBNull.Value;
                cmd.Parameters.Add("@role_level", SqlDbType.Int).Value = data.role_level;

                // --- 2. การจัดการสิทธิ์เมนู (Menu Permissions) ---
                cmd.Parameters.Add("@menu_attendance", SqlDbType.Bit).Value = data.menu_attendance;
                cmd.Parameters.Add("@menu_report", SqlDbType.Bit).Value = data.menu_report;
                cmd.Parameters.Add("@menu_admin", SqlDbType.Bit).Value = data.menu_admin;
                cmd.Parameters.Add("@menu_setup", SqlDbType.Bit).Value = data.menu_setup;

                // --- 3. การจัดการฟังก์ชัน (Function Permissions) ---
                cmd.Parameters.Add("@func_approve", SqlDbType.Bit).Value = data.func_approve;
                cmd.Parameters.Add("@func_cico", SqlDbType.Bit).Value = data.func_cico;
                cmd.Parameters.Add("@func_rp_attendance", SqlDbType.Bit).Value = data.func_rp_attendance;
                cmd.Parameters.Add("@func_rp_work_hours", SqlDbType.Bit).Value = data.func_rp_work_hours;
                cmd.Parameters.Add("@func_rp_compensation", SqlDbType.Bit).Value = data.func_rp_compensation;

                // --- 4. สถานะและผู้บันทึก (Status & Audit) ---
                cmd.Parameters.Add("@is_active", SqlDbType.Bit).Value = data.is_active;
                cmd.Parameters.Add("@username", SqlDbType.VarChar, 50).Value = data.username;

                // --- 5. ข้อมูลสำรอง (Spare Fields) ---
                // ใช้ (object)?? DBNull.Value เพื่อรองรับค่า Nullable จาก Payload
                cmd.Parameters.Add("@menu_spare1", SqlDbType.Bit).Value = (object)data.menu_spare1 ?? DBNull.Value;
                cmd.Parameters.Add("@menu_spare2", SqlDbType.Bit).Value = (object)data.menu_spare2 ?? DBNull.Value;
                cmd.Parameters.Add("@menu_spare3", SqlDbType.Bit).Value = (object)data.menu_spare3 ?? DBNull.Value;
                cmd.Parameters.Add("@menu_spare4", SqlDbType.Bit).Value = (object)data.menu_spare4 ?? DBNull.Value;
                cmd.Parameters.Add("@menu_spare5", SqlDbType.Bit).Value = (object)data.menu_spare5 ?? DBNull.Value;

                cmd.Parameters.Add("@func_spare1", SqlDbType.Bit).Value = (object)data.func_spare1 ?? DBNull.Value;
                cmd.Parameters.Add("@func_spare2", SqlDbType.Bit).Value = (object)data.func_spare2 ?? DBNull.Value;
                cmd.Parameters.Add("@func_spare3", SqlDbType.Bit).Value = (object)data.func_spare3 ?? DBNull.Value;
                cmd.Parameters.Add("@func_spare4", SqlDbType.Bit).Value = (object)data.func_spare4 ?? DBNull.Value;
                cmd.Parameters.Add("@func_spare5", SqlDbType.Bit).Value = (object)data.func_spare5 ?? DBNull.Value;
                cmd.Parameters.Add("@func_spare6", SqlDbType.Bit).Value = (object)data.func_spare6 ?? DBNull.Value;
                cmd.Parameters.Add("@func_spare7", SqlDbType.Bit).Value = (object)data.func_spare7 ?? DBNull.Value;
                cmd.Parameters.Add("@func_spare8", SqlDbType.Bit).Value = (object)data.func_spare8 ?? DBNull.Value;
                cmd.Parameters.Add("@func_spare9", SqlDbType.Bit).Value = (object)data.func_spare9 ?? DBNull.Value;
                cmd.Parameters.Add("@func_spare10", SqlDbType.Bit).Value = (object)data.func_spare10 ?? DBNull.Value;

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync(); // ใช้สำหรับ SP ที่ไม่มีการ Return ข้อมูลกลับ

                con.Close();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
