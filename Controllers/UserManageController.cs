using AppName_API.Components.Messages;
using AppName_API.Models.Responses.Authentication;
using IATMS.Components;
using IATMS.Configurations;
using IATMS.contextDB;
using IATMS.Models.Authentications;
using IATMS.Models.Payloads;
using IATMS.Models.Payloads.UserManage;
using IATMS.Models.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.AccessControl;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IATMS.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserManageController : ControllerBase
    {
        private readonly TokenValidationParameters _tokenValidationParameters;
        public UserManageController(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }
        [HttpGet("getUserManage")]
        public async Task<IActionResult> getAllProfile([FromQuery] searchLov q)
        {
            AccessTokenProps info;
            try
            {
                info = JwtToken.AccessTokenValidation(Request, _tokenValidationParameters);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }

            var k = (q?.keyword ?? "").Trim();
            var result = ConDB.GetUserManage(k);
            return Ok(result);

        }
        [HttpPost("postUserManage")]
        public async Task<IActionResult> postUserManage([FromBody] Pay_UserManage payload)
        {
            AccessTokenProps info;
            try
            {
                info = JwtToken.AccessTokenValidation(Request, _tokenValidationParameters);
            }
            catch (Exception)
            {
                return Unauthorized();
            }

            try
            {
                payload.username = info.username;

                // เรียกใช้ฟังก์ชันใน ConDB เพื่อบันทึกข้อมูล
                var isSuccess = await ConDB.PostUserProfile(payload);

                if (isSuccess)
                {
                    return Ok(new { res_code = 200, message = "บันทึกข้อมูลสำเร็จ" });
                }
                else
                {
                    return BadRequest(new { res_code = 400, message = "ไม่สามารถบันทึกข้อมูลได้" });
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { res_code = 500, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { res_code = 500, message = ex.Message });
            }
        }

        

        [HttpGet]
        [HttpGet("find")]
        public async Task<IActionResult> FindUser(string? oa_user, string? fname, string? lname)
        {
            // 1. Validation Logic เหมือนเดิม...

            // 3. Execute Search with Fallback
            Res_Profile profile = null;

            try
            {
                profile = LDAP.GetUserProfile(oa_user, fname, lname);
            }
            catch { /* LDAP Error */ }

            // ถ้า LDAP ไม่เจอ ให้หาใน MockData.result
            if (profile == null || string.IsNullOrWhiteSpace(profile.oa_user))
            {
                var dummy = MockData.Users.FirstOrDefault(u =>
                    (!string.IsNullOrEmpty(oa_user) && u.result.oa_user.Equals(oa_user, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(fname) && u.result.Name_en.Contains(fname, StringComparison.OrdinalIgnoreCase))
                );

                if (dummy != null)
                {
                    profile = dummy.result;
                }
            }

            if (profile == null || string.IsNullOrWhiteSpace(profile.oa_user))
            {
                return BadRequest(ErrorMessage.GetMessage(4001));
            }

            return Ok(new { res_code = 200, result = profile });
        }



    }
}
