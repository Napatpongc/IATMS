using AppName_API.Components.Authorization;
using IATMS.Components;
using IATMS.contextDB;
using IATMS.Models.Authentications;
using IATMS.Models.Payloads;
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


    public class HolidayController : ControllerBase
    {
        private readonly TokenValidationParameters _tokenValidationParameters;
        public HolidayController(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }
        [HttpGet("getHolidays")]
        public async Task<IActionResult> getHolidays([FromQuery] getHolidays q)
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
                if (!AccessRole.IsAuthorize(info.username, menu: "menu_admin"))
                {
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + " : " + ex.Message);
            }
            try
            {
                var result = ConDB.GetHolidays(q.isActive, q.yearSearch);
            return Ok(result);
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

        [HttpPost("postHolidays")]
        public async Task<IActionResult> postHolidays([FromBody] postHolidays Payload)
        {
            AccessTokenProps info;
            try
            {
                info = JwtToken.AccessTokenValidation(Request, _tokenValidationParameters);
            }
            catch
            {
                return Unauthorized();
            }
            try
            {
                if (!AccessRole.IsAuthorize(info.username, menu: "menu_admin"))
                {
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + " : " + ex.Message);
            }
            try
            {
                await ConDB.PostHolidays(Payload.holidayDate,Payload.holidayName,Payload.isActive,Payload.isEdit, info.username);
                return Ok(new { Success = true });

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


        [HttpGet("getHolidayYears")]
        public async Task<IActionResult> getHolidayYears()
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
                if (!AccessRole.IsAuthorize(info.username, menu: "menu_setup"))
                {
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + " : " + ex.Message);
            }
            try
            {
                var result = ConDB.GetHolidayYears();
            return Ok(result);
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
