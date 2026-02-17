using IATMS.Components;
using IATMS.contextDB;
using IATMS.Models.Authentications;
using IATMS.Models.Payloads;
using IATMS.Models.Payloads.CICO;
using IATMS.Models.Responses.CheckinCheckout;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace IATMS.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CheckinCheckoutController : ControllerBase
    {
        private readonly TokenValidationParameters _tokenValidationParameters;
        public CheckinCheckoutController(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }
        [HttpGet("getButton")]
        public async Task<IActionResult> getButton ()
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
                var result = await ConDB.GetButton(info.username);
                return Ok(result);

            }catch (Exception ex)
            {
                return Unauthorized(new
                {
                    res_code = 401,
                    message = "สิทธิ์การเข้าใช้งานไม่ถูกต้องหรือหมดอายุ"
                });

            }
            
            

        }

        [HttpGet("getCICO")]
        public async Task<IActionResult> getCICO([FromQuery] Pay_ButtomCICO q)
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
                var result = await ConDB.GetCico(info.username,q.mode);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return Unauthorized(new
                {
                    res_code = 401,
                    message = "สิทธิ์การเข้าใช้งานไม่ถูกต้องหรือหมดอายุ"
                });

            }
        }
        [HttpPost("postCICO")]
        public async Task<IActionResult> postCICO([FromBody] Pay_CICO data)
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
                if (data == null)
                {
                    return BadRequest(new { message = "ข้อมูลไม่ถูกต้อง" });
                }

                // บังคับใช้ username จาก Token (oa_user) เพื่อความปลอดภัย 
                // หรือจะใช้ data.oa_user ที่ส่งมาจาก Frontend ก็ได้ถ้า logic ออกแบบไว้แบบนั้น
                data.oa_user = info.username;

                // เรียกใช้ ConDB 
                var success = await ConDB.PostCICO(data);   

                if (success)
                {
                    return Ok(new
                    {
                        res_code = 200,
                        message = "บันทึกข้อมูลสำเร็จ"
                    });
                }
                else
                {
                    return BadRequest(new { message = "ไม่สามารถบันทึกข้อมูลได้" });
                }
            }
            catch (Exception ex)
            {
                // กรณีเกิด Error ในระดับ Database หรือ Code
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    res_code = 500,
                    message = "เกิดข้อผิดพลาดภายในระบบ: " + ex.Message
                });
            }
        }
    }
}
