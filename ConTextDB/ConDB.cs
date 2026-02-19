using AppName_API.Models.Responses.Authentication;
using Azure.Core;
using IATMS.Components;
using IATMS.Configurations;
using IATMS.Models.Authentications;
using IATMS.Models.Payloads;
using IATMS.Models.Payloads.CICO;
using IATMS.Models.Payloads.Leave;
using IATMS.Models.Payloads.UserManage;
using IATMS.Models.Responses;
using IATMS.Models.Responses.CheckinCheckout;
using IATMS.Models.Responses.DropDown;
using IATMS.Models.Responses.Holidays;
using IATMS.Models.Responses.Leave;
using IATMS.Models.Responses.Role;
using IATMS.Models.Responses.User_Manage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Runtime;
using static IATMS.Models.Responses.CheckinCheckout.getButton;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static System.Runtime.InteropServices.JavaScript.JSType;
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
                //        using var con = new SqlConnection(connectionString);
                //        using var cmd = new SqlCommand("dbo.getProfile", con);
                //        using var con = new SqlConnection(connectionString);
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

                // --- 1. ข้อมูลหลัก ---
                cmd.Parameters.Add("@role_id", SqlDbType.VarChar, 50).Value = data.role_id;

                cmd.Parameters.Add("@description", SqlDbType.VarChar, 255).Value = (object)data.description ?? DBNull.Value;

                cmd.Parameters.Add("@role_level", SqlDbType.Int).Value = data.role_level;

                // --- 2. การจัดการสิทธิ์เมนู ---
                cmd.Parameters.Add("@menu_attendance", SqlDbType.Bit).Value = data.menu_attendance;

                cmd.Parameters.Add("@menu_report", SqlDbType.Bit).Value = data.menu_report;
                cmd.Parameters.Add("@menu_admin", SqlDbType.Bit).Value = data.menu_admin;
                cmd.Parameters.Add("@menu_setup", SqlDbType.Bit).Value = data.menu_setup;

                // --- 3. การจัดการฟังก์ชัน ---
                cmd.Parameters.Add("@func_approve", SqlDbType.Bit).Value = data.func_approve;

                cmd.Parameters.Add("@func_cico", SqlDbType.Bit).Value = data.func_cico;

                cmd.Parameters.Add("@func_rp_attendance", SqlDbType.Bit).Value = data.func_rp_attendance;

                cmd.Parameters.Add("@func_rp_work_hours", SqlDbType.Bit).Value = data.func_rp_work_hours;

                cmd.Parameters.Add("@func_rp_compensation", SqlDbType.Bit).Value = data.func_rp_compensation;

                // --- 4. สถานะและผู้บันทึก ---
                cmd.Parameters.Add("@is_active", SqlDbType.Bit).Value = data.is_active;
                cmd.Parameters.Add("@username", SqlDbType.VarChar, 50).Value = data.username;

                // --- 5. ข้อมูลสำรอง (Spare Fields) ---
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
                await cmd.ExecuteNonQueryAsync();
                con.Close();

                return true;

            }

            catch (Exception)

            {

                throw;

            }

        }
        public static async Task PostListofvalues(string fieldName, string code, string description, string condition, int orderIndex, bool isActive, string username)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("dbo.postLov", conn);
                cmd.CommandTimeout = Timeout;
                cmd.CommandType = CommandType.StoredProcedure;

                // parameters
                cmd.Parameters.Add("@field_name", SqlDbType.VarChar, 50).Value = fieldName;
                cmd.Parameters.Add("@code", SqlDbType.VarChar, 50).Value = code;
                cmd.Parameters.Add("@description", SqlDbType.VarChar, 255).Value = description;
                cmd.Parameters.Add("@condition", SqlDbType.VarChar, 255).Value = condition;
                cmd.Parameters.Add("@order_index", SqlDbType.Int).Value = orderIndex;
                cmd.Parameters.Add("@is_active", SqlDbType.Bit).Value = isActive;
                cmd.Parameters.Add("@username", SqlDbType.VarChar, 50).Value = username;

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                conn.Close();

            }
            catch (Exception)

            {

                throw;

            }


        }






        public static List<Res_Lov> GetListofvalues(string keyword)
        {

            var results = new List<Res_Lov>();
            try
            {
                using var con = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("dbo.getLov", con);

                cmd.CommandTimeout = Timeout;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@keyword", SqlDbType.VarChar, 255).Value = keyword;

                con.Open();
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    var item = new Res_Lov();

                    item.fieldName = rd["field_name"]?.ToString();
                    item.code = rd["code"]?.ToString();
                    item.description = rd["description"]?.ToString();
                    item.condition = rd["condition"]?.ToString();
                    item.orderIndex = rd["order_index"] == DBNull.Value ? 0 : Convert.ToInt32(rd["order_index"]);
                    item.isActive = Convert.ToInt32(rd["is_active"]) == 1;

                    results.Add(item);
                }
                rd.Close();
                cmd.Dispose();
                con.Close();

                return results;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static List<Res_Holidays> GetHolidays(bool isActive, int yearSearch)
        {
            var results = new List<Res_Holidays>();
            try
            {
                using var con = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("dbo.getHolidays", con);

                cmd.CommandTimeout = Timeout;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@is_active", SqlDbType.Bit).Value = isActive;
                cmd.Parameters.Add("@yearSearch", SqlDbType.Int).Value = yearSearch;


                con.Open();
                var rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    var item = new Res_Holidays();

                    var dt = rd.GetDateTime(rd.GetOrdinal("holiday_date"));
                    item.holidayDate = DateOnly.FromDateTime(dt);
                    item.holidayName = rd["holiday_name"]?.ToString();
                    item.isActive = Convert.ToInt32(rd["is_active"]) == 1;

                    results.Add(item);
                }
                rd.Close();
                cmd.Dispose();
                con.Close();

                return results;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public static async Task PostHolidays(DateOnly holydayDate, string holydayName, bool isActive, string username)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("dbo.postHolidays", conn);
                cmd.CommandTimeout = Timeout;
                cmd.CommandType = CommandType.StoredProcedure;

                // parameters
                cmd.Parameters.Add("@holidayDate", SqlDbType.Date).Value = holydayDate;
                cmd.Parameters.Add("@holidayName", SqlDbType.VarChar, 100).Value = holydayName;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = isActive;
                cmd.Parameters.Add("@username", SqlDbType.VarChar, 50).Value = username;

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                conn.Close();

            }
            catch (Exception)
            {
                throw;
            }

        }



        public static List<Res_HolidayYears> GetHolidayYears()
        {
            var results = new List<Res_HolidayYears>();
            try
            {
                using var con = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("dbo.getHolidayYears", con)
                {
                    CommandTimeout = Timeout,
                    CommandType = CommandType.StoredProcedure
                };

                con.Open();

                using var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    var item = new Res_HolidayYears();
                    // ชื่อคอลัมน์ต้องตรงกับที่ SP SELECT ออกมา: minYear, maxYear
                    item.Year = rd["year"] == DBNull.Value ? 0 : Convert.ToInt32(rd["year"]);
                    results.Add(item);
                }
                rd.Close();
                cmd.Dispose();
                con.Close();


            }
            catch (Exception ex)
            {
                throw;
            }


            return results;
        }


        public static List<Res_ALL_Profile> GetUserManage(string keyword)
        {
            List<Res_ALL_Profile> results = new();
            try
            {
                using var con = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("dbo.getUserManage", con);

                cmd.CommandTimeout = Timeout;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Keyword", SqlDbType.VarChar, 100).Value = (object)keyword ?? DBNull.Value;

                con.Open();
                using var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    var item = new Res_ALL_Profile
                    {
                        oa_user = rd["oa_user"]?.ToString(),
                        first_name_th = rd["first_name_th"]?.ToString(),
                        last_name_th = rd["last_name_th"]?.ToString(),
                        first_name_en = rd["first_name_en"]?.ToString(),
                        last_name_en = rd["last_name_en"]?.ToString(),
                        email = rd["email"]?.ToString(),

                        role = rd["role"]?.ToString(),
                        team = rd["team"]?.ToString(),
                        division = rd["division"]?.ToString(),
                        work_place = rd["work_place"]?.ToString(),

                        is_active = rd["is_active"] != DBNull.Value && Convert.ToBoolean(rd["is_active"])
                    };

                    results.Add(item);
                }

                return results;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static async Task<bool> PostUserProfile(Pay_UserManage data)
        {
            try
            {
                using var con = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("dbo.postUserProfile", con);

                cmd.CommandTimeout = Timeout;
                cmd.CommandType = CommandType.StoredProcedure;

                // Binding Parameters ให้ตรงกับชื่อและลำดับใน Stored Procedure
                cmd.Parameters.Add("@oa_user", SqlDbType.VarChar, 50).Value = (object)data.oa_user ?? DBNull.Value;
                cmd.Parameters.Add("@first_name_th", SqlDbType.NVarChar, 100).Value = (object)data.first_name_th ?? DBNull.Value;
                cmd.Parameters.Add("@last_name_th", SqlDbType.NVarChar, 100).Value = (object)data.last_name_th ?? DBNull.Value;
                cmd.Parameters.Add("@first_name_en", SqlDbType.VarChar, 100).Value = (object)data.first_name_en ?? DBNull.Value;
                cmd.Parameters.Add("@last_name_en", SqlDbType.VarChar, 100).Value = (object)data.last_name_en ?? DBNull.Value;
                cmd.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = (object)data.email ?? DBNull.Value;
                cmd.Parameters.Add("@division_code", SqlDbType.VarChar, 50).Value = (object)data.division_code ?? DBNull.Value;
                cmd.Parameters.Add("@role_id", SqlDbType.VarChar, 50).Value = (object)data.role_id ?? DBNull.Value;
                cmd.Parameters.Add("@team_code", SqlDbType.VarChar, 50).Value = (object)data.team_code ?? DBNull.Value;
                cmd.Parameters.Add("@work_place", SqlDbType.VarChar, 50).Value = (object)data.work_place ?? DBNull.Value;
                cmd.Parameters.Add("@is_active", SqlDbType.Bit).Value = data.is_active;
                cmd.Parameters.Add("@username", SqlDbType.VarChar, 50).Value = (object)data.username ?? "System";
                cmd.Parameters.Add("@mode", SqlDbType.VarChar, 10).Value = data.mode;

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                con.Close();

                return true;
            }
            catch (Exception ex)
            {
                // สามารถทำ Log Error ได้ที่นี่
                throw new Exception("Error at PostUserProfile: " + ex.Message);
            }
        }
        public static async Task<List<Res_Dropdown>> GetDropdownList(Req_Dropdown type)
        {
            List<Res_Dropdown> results = new();
            try
            {
                using var con = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("dbo.getDropDown", con);

                cmd.CommandTimeout = Timeout;
                cmd.CommandType = CommandType.StoredProcedure;

                // ดึงค่าจาก Property .type ใน Object มาใช้
                cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 50).Value = (object)type.type ?? DBNull.Value;

                await con.OpenAsync();
                using var rd = await cmd.ExecuteReaderAsync();

                while (await rd.ReadAsync())
                {
                    results.Add(new Res_Dropdown
                    {
                        value = rd["value"]?.ToString(),
                        label = rd["label"]?.ToString()
                    });
                }
                return results;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in GetDropdownList: {ex.Message}");
            }
        }

        public static async Task <getButton> GetButton (string username)
        {
            var results = new getButton();
            try
            {
                using var con = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("dbo.getButtonCICO", con);

                cmd.CommandTimeout = Timeout;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@oa_user", SqlDbType.VarChar, 50).Value = username;

                await con.OpenAsync();
                using var rd = await cmd.ExecuteReaderAsync();

                while (await rd.ReadAsync())
                {
                    results.oaUser = rd["oa_user"]?.ToString();
                    var dt = rd.GetDateTime(rd.GetOrdinal("at_date"));
                    results.attDate = DateOnly.FromDateTime(dt);
                    results.canCi = Convert.ToInt32(rd["can_ci"]) == 1;
                    results.canCo = Convert.ToInt32(rd["can_co"]) == 1;
                    results.ciThreshold = rd["ci_threshold_time"]?.ToString();
                    results.coThreshold = rd["co_threshold_time"]?.ToString();
                    results.wpCondition = rd["wp_condition"]?.ToString();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return results;
        }

        public static async Task<List<getCICO>> GetCico(string username, int mode)
        {
            var results = new List<getCICO>();

            using var con = new SqlConnection(connectionString);
            using var cmd = new SqlCommand("dbo.getCICO", con)
            {
                CommandTimeout = Timeout,
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("@oa_user", SqlDbType.VarChar, 50).Value = username;

            // ใส่บรรทัดนี้ต่อเมื่อ SP มี @mode จริงเท่านั้น
            cmd.Parameters.Add("@mode", SqlDbType.Int).Value = mode;

            await con.OpenAsync();
            using var rd = await cmd.ExecuteReaderAsync();

            // เปลี่ยน "at_date" ให้ตรงกับชื่อคอลัมน์ที่ SP ส่งออกมาจริง
            int iDate = rd.GetOrdinal("date");

            while (await rd.ReadAsync())
            {
                var dt = rd.GetDateTime(iDate);

                var item = new getCICO
                {
                    attDate = DateOnly.FromDateTime(dt),
                    ciTime = rd["ci_time"]?.ToString(),
                    ciCorrectTime = rd["ci_correct_time"]?.ToString(),
                    ciAddress = rd["ci_address"]?.ToString(),
                    ciCorrectZone = rd["ci_correct_zone"]?.ToString(),
                    ciReason = rd["ci_reason"]?.ToString(),
                    coTime = rd["co_time"]?.ToString(),
                    coCorrectTime = rd["co_correct_time"]?.ToString(),
                    coAddress = rd["co_address"]?.ToString(),
                    coCorrectZone = rd["co_correct_zone"]?.ToString(),
                    coReason = rd["co_reason"]?.ToString(),
                    isNomal = Convert.ToInt32(rd["is_normal"]) == 1
                };

                results.Add(item);
            }

            return results;
        }
        public static async Task<bool> PostCICO(Pay_CICO data)
        {
            try
            {
                using var con = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("dbo.postCICO", con);

                cmd.CommandTimeout = Timeout; // ใช้ค่า Timeout ที่คุณตั้งไว้
                cmd.CommandType = CommandType.StoredProcedure;

                // Binding Parameters ให้ตรงกับ Stored Procedure [dbo].[postCICO]
                cmd.Parameters.Add("@oa_user", SqlDbType.VarChar, 50).Value = (object)data.oa_user ?? DBNull.Value;

                // สำหรับ Location และ Address แนะนำใช้ NVarChar เพื่อรองรับภาษาไทย
                cmd.Parameters.Add("@location", SqlDbType.NVarChar, 100).Value = (object)data.location ?? DBNull.Value;
                cmd.Parameters.Add("@address", SqlDbType.NVarChar, 255).Value = (object)data.address ?? DBNull.Value;

                cmd.Parameters.Add("@mac_address", SqlDbType.VarChar, 50).Value = (object)data.mac_address ?? DBNull.Value;

                // ถ้า reason เป็นค่าว่าง ให้ส่ง "" ตาม Logic ของคุณ
                cmd.Parameters.Add("@reason", SqlDbType.NVarChar, 255).Value = (object)data.reason ?? DBNull.Value;

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                con.Close();

                return true;
            }
            catch (Exception ex)
            {
                // แนะนำให้ Log ex.Message ไว้ดูยามจำเป็นครับ
                throw new Exception("Error at PostCICO: " + ex.Message);
            }
        }
        public static async Task<List<Res_Leave>> GetLeaveRequest(string username, DateOnly? startDate = null, DateOnly? endDate = null, string status = null)
        {
            var results = new List<Res_Leave>();
            try
            {
                using var con = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("dbo.getLeaveRequest", con);

                cmd.CommandTimeout = Timeout;
                cmd.CommandType = CommandType.StoredProcedure;

                // รับค่าทีละตัวและจัดการ DBNull หากไม่ได้ส่งค่ามา
                cmd.Parameters.Add("@oa_user", SqlDbType.VarChar, 50).Value = (object)username ?? DBNull.Value;
                cmd.Parameters.Add("@startDate", SqlDbType.Date).Value = (object)startDate ?? DBNull.Value;
                cmd.Parameters.Add("@endDate", SqlDbType.Date).Value = (object)endDate ?? DBNull.Value;
                cmd.Parameters.Add("@status", SqlDbType.VarChar, 50).Value = (object)status ?? DBNull.Value;

                await con.OpenAsync();
                using var rd = await cmd.ExecuteReaderAsync();
                while (await rd.ReadAsync())
                {
                    results.Add(new Res_Leave
                    {
                        oa_user = rd["oa_user"]?.ToString(),
                        // ดึงจากคอลัมน์ภาษาอังกฤษที่แก้ใหม่ใน SQL
                        type_leave = rd["type_leave_display"]?.ToString(),
                        start_date = rd["start_date"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(rd["start_date"])) : null,
                        end_date = rd["end_date"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(rd["end_date"])) : null,

                        // รองรับค่า NULL สำหรับการลาเต็มวัน
                        start_time = rd["start_time"] != DBNull.Value ? Convert.ToDateTime(rd["start_time"]) : null,
                        end_time = rd["end_time"] != DBNull.Value ? Convert.ToDateTime(rd["end_time"]) : null,

                        reason = rd["reason"]?.ToString(),
                        status_request = rd["status_display"]?.ToString(),
                        total_hours = Convert.ToDecimal(rd["total_hours"]),
                        approve_by = rd["approve_by"]?.ToString(),
                        approve_date = rd["approve_date"] != DBNull.Value ? Convert.ToDateTime(rd["approve_date"]) : null,
                        created_date = Convert.ToDateTime(rd["created_date"])
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error at GetLeaveRequest: " + ex.Message);
            }
            return results;
        }

        public static async Task<bool> PostLeaveRequest(Pay_Leave data)
        {
            try
            {
                using var con = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("dbo.postLeaveRequest", con);

                cmd.CommandTimeout = Timeout;
                cmd.CommandType = CommandType.StoredProcedure;

                // Binding Parameters (ตามลำดับใน Stored Procedure)
                cmd.Parameters.Add("@oa_user", SqlDbType.VarChar, 50).Value = (object)data.oa_user ?? DBNull.Value;
                cmd.Parameters.Add("@type_leave", SqlDbType.VarChar, 50).Value = (object)data.type_leave ?? DBNull.Value;
                cmd.Parameters.Add("@start_date", SqlDbType.Date).Value = data.start_date;
                cmd.Parameters.Add("@end_date", SqlDbType.Date).Value = data.end_date;

                // เวลาเริ่มต้น/สิ้นสุด (อนุญาตให้เป็น NULL ถ้าลาเต็มวัน)
                cmd.Parameters.Add("@start_time", SqlDbType.DateTime).Value = (object)data.start_time ?? DBNull.Value;
                cmd.Parameters.Add("@end_time", SqlDbType.DateTime).Value = (object)data.end_time ?? DBNull.Value;

                // ใช้ NVarChar(-1) สำหรับ varchar(max) รองรับเหตุผลยาวๆ และภาษาไทย
                cmd.Parameters.Add("@reason", SqlDbType.NVarChar, -1).Value = (object)data.reason ?? DBNull.Value;

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                con.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error at PostLeaveRequest: " + ex.Message);
            }
        }
    }

}






