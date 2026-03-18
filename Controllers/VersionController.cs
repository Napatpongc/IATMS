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
        [AllowAnonymous]
        [HttpGet("getVersion")]
        public IActionResult GetVersion()
        {
            return Ok(AppSettings.Version);
        }
    }
}