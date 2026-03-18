using AppName_API.Components.Authorization;
using IATMS.Components;
using IATMS.contextDB;
using IATMS.Models.Authentications;
using IATMS.Models.Payloads;
using IATMS.Models.Payloads.AttendanceChange;
using IATMS.Models.Payloads.CICO;
using IATMS.Models.Responses.AtendanceChange;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IATMS.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AttendanceChangeController : ControllerBase
    {
        private readonly TokenValidationParameters _tokenValidationParameters;
        public AttendanceChangeController(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }
        [HttpGet("getAttChange")]
        public async Task<IActionResult> getAttChange([FromQuery] Pay_AttendanceChange search)
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
                if (!AccessRole.IsAuthorize(info.username, menu: "menu_attendance", function: "func_cico"))
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
                var result = await ConDB.getAttChange(info.username,search.startDate,search.endDate,search.dropdown);
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

        [HttpGet("getModalAtt")]
        public async Task<IActionResult> getModalAtt([FromQuery] Pay_ModalAttendanceChange search)
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
                if (!AccessRole.IsAuthorize(info.username, menu: "menu_attendance", function: "func_cico"))
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
                var result = await ConDB.getModalAttChange(info.username, search.Date);
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
        [HttpPost("getHolidayOnly")]
        public async Task<IActionResult> getHolidayOnly([FromQuery] getHolidays search)
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
                if (!AccessRole.IsAuthorize(info.username, menu: "menu_attendance", function: "func_cico"))
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
                var holidays = ConDB.GetHolidayOnly(search.isActive, search.yearSearch);

                if (holidays == null || holidays.Count == 0)
                {
                    return NotFound("No holidays found for the specified year.");
                }

                return Ok(holidays);
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

        [HttpDelete("deleteAttChange")]
        public async Task<IActionResult> deleteAttChange([FromQuery] Pay_ModalAttendanceChange payload)
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
                if (!AccessRole.IsAuthorize(info.username, menu: "menu_attendance", function: "func_cico"))
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
                await ConDB.DeleteAttChange(info.username, payload.Date);
                return Ok("Success for delete");

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


    }
}
