using IATMS.Components;
using IATMS.contextDB;
using IATMS.Models.Authentications;
using IATMS.Models.Payloads;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IATMS.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DashBoardController : Controller
    {
        private readonly TokenValidationParameters _tokenValidationParameters;
        public DashBoardController(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }

        [HttpGet("getHomeDashboard")]
        public async Task<IActionResult> GetHomeDashboard()
        {
            AccessTokenProps info;
            try
            {
                // ตรวจสอบ Token เหมือนเดิม
                info = JwtToken.AccessTokenValidation(Request, _tokenValidationParameters);
            }
            catch (Exception)
            {
                return Unauthorized();
            }

            try
            {
                // ส่ง Username (oa_user) ที่ได้จาก Token เข้าไปใน ConDB
                // สมมติว่าใน ConDB มี Method ชื่อ GetHomeDashboardData ที่รับค่า oa_user
                var result = await ConDB.GetHomeDashboard(info.username);

                if (result == null)
                {
                    return NotFound(new { message = "No dashboard data found for this user." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
