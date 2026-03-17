using AppName_API.Components.Authorization;
using IATMS.Components;
using IATMS.Configurations;
using IATMS.Models.Authentications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IATMS.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class VersionController : ControllerBase
    {
        private readonly TokenValidationParameters _tokenValidationParameters;

        public VersionController(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }
        [HttpGet("getVersion")]
        public async Task<IActionResult> version()
        {
            AccessTokenProps info;
            try
            {
                info = JwtToken.AccessTokenValidation(Request, _tokenValidationParameters);
            }
            catch (Exception) { return Unauthorized(); }
            try
            {
                if (!AccessRole.IsAuthorize(info.username, menu: "menu_attendance", function: "func_approve"))
                {
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + " : " + ex.Message);
            }



            return Ok(AppSettings.Version);

        }
    }
}
