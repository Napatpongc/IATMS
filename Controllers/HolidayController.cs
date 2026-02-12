using IATMS.Components;
using IATMS.contextDB;
using IATMS.Models.Authentications;
using IATMS.Models.Payloads;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Data.SqlClient;



namespace IATMS.Controllers
{
    [Route("api")]
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
            var result = ConDB.GetHolidays(q.isActive, q.yearSearch);
            return Ok(result);

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
                await ConDB.PostHolidays(Payload.holidayDate,Payload.holidayName,Payload.isActive, info.username);
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

        [HttpGet("getHolidayYearRange")]
        public async Task<IActionResult> getHolidayYearRange()
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
            var result = ConDB.GetHolidayYearRange();
            return Ok(result);

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
            var result = ConDB.GetHolidayYears();
            return Ok(result);

        }
    }


}
