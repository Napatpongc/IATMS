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
using Microsoft.IdentityModel.Tokens;

namespace IATMS.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ProfileController : ControllerBase
    {
        private readonly TokenValidationParameters _tokenValidationParameters;
        public ProfileController(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
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

            Res_Only_Profile result = new Res_Only_Profile();

            Res_Login profileRole = ConDB.GetSigninUserProfile(info.username);

            result.role = profileRole.role;
            result.profile = profileRole.profile;


            return Ok(result);

        }

    }
}
