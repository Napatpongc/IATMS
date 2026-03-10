using AppName_API.Components.Authorization;
using AppName_API.Models.Responses.Authentication;
using IATMS.Components;
using IATMS.contextDB;
using IATMS.Models.Authentications;
using IATMS.Models.Payloads;
using IATMS.Models.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;


namespace IATMS.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ListOfValuesController : ControllerBase
    {
        private readonly TokenValidationParameters _tokenValidationParameters;
        public ListOfValuesController(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }
        [HttpGet("getLov")]
        public async Task<IActionResult> getListofvalue([FromQuery] searchLov q)
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
            try { 
            var k = (q?.keyword ?? "").Trim();   
            var result = ConDB.GetListofvalues(k);
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

        [HttpPost("postLov")]
        public async Task<IActionResult> postListofvalue([FromBody] ListOfValues Payload)
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
                await ConDB.PostListofvalues(Payload.fieldName, Payload.code, Payload.description, Payload.condition, Payload.orderIndex, Payload.isActive, info.username);
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


    }


}
