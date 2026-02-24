using IATMS.Components;
using IATMS.contextDB;
using IATMS.Models.Authentications;
using IATMS.Models.Payloads;
using IATMS.Models.Payloads.Leave;
using IATMS.Models.Responses.Leave;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IATMS.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LeaveController : ControllerBase
    {
        private readonly TokenValidationParameters _tokenValidationParameters;

        public LeaveController(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }

        [HttpGet("getLeave")]
        public async Task<IActionResult> getLeave([FromQuery] searchLeave q)
        {
            AccessTokenProps info;
            try
            {
                info = JwtToken.AccessTokenValidation(Request, _tokenValidationParameters);
            }
            catch (Exception) { return Unauthorized(); }

            try
            {
                var result = await ConDB.GetLeaveRequest(info?.username, q.startDate, q.endDate, q.status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("postLeave")]
        public async Task<IActionResult> postLeave([FromBody] Pay_Leave payload)
        {
            AccessTokenProps info;
            try
            {
                info = JwtToken.AccessTokenValidation(Request, _tokenValidationParameters);
            }
            catch (Exception) { return Unauthorized(); }
            
            try
            {
                var success = await ConDB.PostLeaveRequest(payload);
                return success ? Ok(new { message = "Success" }) : BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("deleteLeave")]
        public async Task<IActionResult> DeleteLeaveRequest([FromQuery] Pay_Delete_Leave payload)
        {
            AccessTokenProps info;
            try
            {
                info = JwtToken.AccessTokenValidation(Request, _tokenValidationParameters);
            }
            catch (Exception) { return Unauthorized(); }

            try
            {
                var success = await ConDB.DeleteLeaveRequest(info?.username , payload);
                return success ? Ok(new { message = "Success" }) : BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}