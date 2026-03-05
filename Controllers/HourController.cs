using IATMS.Components;
using IATMS.contextDB;
using IATMS.Models.Authentications;
using IATMS.Models.Payloads.Compensation;
using IATMS.Models.Payloads.WorkHourHistory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IATMS.Controllers
{

    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HourController : Controller
    {
        private readonly TokenValidationParameters _tokenValidationParameters;
        public HourController(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }
        [HttpGet("getHourHistory")]
        public async Task<IActionResult> getHourHistory([FromQuery] Pay_HourHistory payload)
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
                var result = await ConDB.getHourHistory(info.username, payload);
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
