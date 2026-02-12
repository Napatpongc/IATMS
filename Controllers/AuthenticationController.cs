using AppName_API.Components.Messages;
using AppName_API.Models.Responses.Authentication;
using IATMS.Components; //class jwt
using IATMS.Configurations; //appsetting.cs
using IATMS.contextDB;
using IATMS.Models.Authentications;
using IATMS.Models.Payloads;
using IATMS.Models.Responses; //res
using Microsoft.AspNetCore.Authentication.JwtBearer; //jwt
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens; //Token
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;

namespace IATMS.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private const string return_msg = "Return-";
        private readonly ILogger<AuthenticationController> _logger;
        private const string exception_msg = "Exception: An error occurred with the message: ";
        public AuthenticationController(ILogger<AuthenticationController> logger)
        {
            _logger = logger;
        }

       


        [HttpPost("SignIn")]
        public async Task<IActionResult> Login([FromBody] Pay_signin Payload)
        {

            string Path = AppSettings.LdapPath;

            string UserName = Payload.username;
            string PassWord = Payload.password;
            bool isAuthenticated = false;
            //ldap
            try
            {
                using (var entry = new DirectoryEntry(Path, UserName, PassWord, AuthenticationTypes.Secure))
                {
                    var forceBind = entry.NativeObject;
                    isAuthenticated = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"LDAP failed, trying MockData: {ex.Message}");
            }
            Res_Role userRole = null;
            Res_Profile userProfile = null;

            if (isAuthenticated)
            {
                // กรณีผ่าน LDAP: ดึงข้อมูลจาก DB จริง
                Res_Login dbProfile = ConDB.GetSigninUserProfile(UserName);
                userRole = dbProfile.role;
                userProfile = dbProfile.profile;
            }
            else
            {
                // กรณี LDAP พลาด: ค้นหาใน MockData
                var dummy = MockData.Users.FirstOrDefault(u =>
                    u.username.Equals(UserName, StringComparison.OrdinalIgnoreCase) &&
                    u.password == PassWord);

                if (dummy != null)
                {
                    userRole = dummy.role;
                    userProfile = dummy.result; // ใช้ .result ตามที่แจ้งมา
                }
            }

            // ตรวจสอบว่ามี User หรือไม่
            if (userProfile == null || string.IsNullOrEmpty(userProfile.oa_user))
            {
                return Unauthorized(new { status = "error", message = "Invalid username or password." });
            }

            // declare object Res_Login
            Res_Login result = new Res_Login();
            // generate AC Token
            string guid = Guid.NewGuid().ToString();
            var Lifetem_Access = System.DateTime.Now.AddMinutes(AppSettings.AccessLiftTime);
            result.token = JwtToken.GenerateToken(UserName, AppSettings.AccessSecretKey, guid, Lifetem_Access);

            // generate RF Token
            DateTime refresh_expire = System.DateTime.Now.AddMinutes(AppSettings.RefreshLiftTime);
            result.refresh_token = JwtToken.GenerateToken(UserName, AppSettings.RefreshSecretKey, guid, refresh_expire);

            Res_Login profileRole = ConDB.GetSigninUserProfile(UserName);
            if (string.IsNullOrEmpty(profileRole.profile.oa_user))
            {
                Message msg = ErrorMessage.GetMessage(2002);

                return BadRequest(msg);
            }

            result.role = profileRole.role;
            result.profile = profileRole.profile;

            try
            {
                await ConDB.tokenRefresh(Payload.username, result.refresh_token, refresh_expire);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, exception_msg + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + " : " + ex.Message);
            }

            return Ok(result);

        }

       
        [HttpPost("refresh_token")]
        public async Task<IActionResult> refresh_token([FromBody] RefreshToken Payload)
        {
            if (!ModelState.IsValid || Payload == null)
            {
                _logger.LogInformation(return_msg, "Invalid Refresh token.");
                return Unauthorized();
            }

            Payload.refresh_token = Payload.refresh_token.Trim();

            if (string.IsNullOrWhiteSpace(Payload.refresh_token))
            {
                _logger.LogInformation(return_msg, "Invalid Refresh token.");
                return Unauthorized();
            }

            // get token
            AccessTokenProps info;
            try
            {
                info = JwtToken.RefreshTokenValidation(Payload.refresh_token, AppSettings.RefreshSecretKey);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, exception_msg + ex.Message);
                return Unauthorized();
            }

            // validate expire token
            if (await ConDB.refreshTokenValidate(info.username, Payload.refresh_token) != 1)
            {
                _logger.LogInformation(return_msg + "refresh token expired.");
                return Unauthorized();
            }

            // generate new token
            Res_RefreshToken result = new();
            result.token = JwtToken.GenerateToken(info.username, AppSettings.AccessSecretKey, info.guid, System.DateTime.Now.AddMinutes(AppSettings.AccessLiftTime));

            DateTime refresh_expire = System.DateTime.Now.AddHours(AppSettings.RefreshLiftTime);
            result.refresh_token = JwtToken.GenerateToken(info.username, AppSettings.RefreshSecretKey, info.guid, refresh_expire);

            try
            {
                await ConDB.tokenRefresh(info.username, result.refresh_token, refresh_expire);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, exception_msg + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + " : " + ex.Message);
            }

            return Ok(result);

        }
        


    }

}
