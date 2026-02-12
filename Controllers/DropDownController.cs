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
    public class DropDownController : ControllerBase
    {
        private readonly TokenValidationParameters _tokenValidationParameters;
        public DropDownController(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }

        [HttpGet("getDropdown")]
        public async Task<IActionResult> GetDropdown([FromQuery] Req_Dropdown req)
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
                // ตรวจสอบว่ามีค่าส่งมาใน req.type หรือไม่ (ดึงจาก ?type=...)
                if (req == null || string.IsNullOrEmpty(req.type))
                {
                    return BadRequest(new { message = "Type is required in Query String (?type=...)" });
                }

                var result = await ConDB.GetDropdownList(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }

    
}
