using IATMS.Components;
using IATMS.contextDB;
using IATMS.Models.Authentications;
using IATMS.Models.Payloads.AttendanceChange;
using IATMS.Models.Payloads.AttendanceHistory;
using IATMS.Models.Payloads.Compensation;
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
    public class CompensationController : ControllerBase
    {
        private readonly TokenValidationParameters _tokenValidationParameters;
        public CompensationController(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }
        [HttpGet("getCompensation")]
        public async Task<IActionResult> getCompensation([FromQuery] Pay_Compensation search)
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
                var result = await ConDB.getCompensation(info.username, search);
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
    }
}
