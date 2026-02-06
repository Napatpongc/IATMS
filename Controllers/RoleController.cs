using AppName_API.Models.Responses.Authentication;
using IATMS.Components;
using IATMS.contextDB;
using IATMS.Models.Authentications;
using IATMS.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using IATMS.Models.Payloads;
using Microsoft.Data.SqlClient;

namespace IATMS.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly TokenValidationParameters _tokenValidationParameters;
        public RoleController(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }

        [HttpGet("Role")]
        public async Task<IActionResult> Profile()
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
            try
            {
                var rolesList = ConDB.GetRoles();
                return Ok(rolesList);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpPost("PostRole")]
        public async Task<IActionResult> PostRole([FromBody] Pay_Role payload)
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
                // ใส่ชื่อผู้ทำรายการจาก Token ลงใน payload
                payload.username = info.username;

                // เรียกใช้ฟังก์ชัน PostRole ใน ConDB ที่ใช้ Store Procedure dbo.postRole
                bool isSuccess = await ConDB.PostRole(payload);

                if (isSuccess)
                {
                    return Ok(new
                    {
                        res_code = 200,
                        message = "Success"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        res_code = 500,
                        message = "ไม่สามารถบันทึกข้อมูลได้ กรุณาตรวจสอบ ID หรือข้อมูลอีกครั้ง"
                    });
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { res_code = 500, message = ex.Message });
            }
            catch (Exception ex)
            {
                // กรณี Error อื่นๆ
                return Unauthorized(new
                {
                    res_code = 401,
                    message = "สิทธิ์การเข้าใช้งานไม่ถูกต้องหรือหมดอายุ"
                });
            }
        }

    }
}
