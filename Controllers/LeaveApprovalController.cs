using IATMS.Components;
using IATMS.contextDB;
using IATMS.Models.Authentications;
using IATMS.Models.Payloads.Leave;
using IATMS.Models.Payloads.LeaveApproval;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IATMS.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LeaveApprovalController : Controller
    {
        private readonly TokenValidationParameters _tokenValidationParameters;

        public LeaveApprovalController(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }
        [HttpGet("getLeaveApproval")]
        public async Task<IActionResult> getLeave([FromQuery] Search_Leave_Approval payload)
        {
            AccessTokenProps info;
            try
            {
                info = JwtToken.AccessTokenValidation(Request, _tokenValidationParameters);
            }
            catch (Exception) { return Unauthorized(); }

            try
            {
                var result = await ConDB.GetLeaveApproval(info?.username, payload.Search, payload.Team);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("postLeaveApproval")]
        public async Task<IActionResult> postLeave([FromBody] Pay_LeaveApproval payload)
        {
            AccessTokenProps info;
            try
            {
                info = JwtToken.AccessTokenValidation(Request, _tokenValidationParameters);
            }
            catch (Exception) { return Unauthorized(); }

            try
            {
                // ส่ง username ของ Admin และ Payload ไปที่ ConDB
                var success = await ConDB.PostLeaveApproval(info.username, payload);
                return success ? Ok(new { message = "Success" }) : BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
