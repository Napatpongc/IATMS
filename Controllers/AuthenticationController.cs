using IATMS.Models.Payloads;
using IATMS.Models.Responses; //res
using Microsoft.AspNetCore.Authentication.JwtBearer; //jwt
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens; //Token
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;
using IATMS.Components; //class jwt
using IATMS.Configurations; //appsetting.cs

namespace IATMS.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost("SignIn")]
        public async Task<IActionResult> Login([FromBody] Pay_signin Payload)
        {
            try
            {
                string ldapIp = "10.125.123.87";
                string Path = $"LDAP://{ldapIp}:389";

                string UserName = Payload.username;
                string PassWord = Payload.password;

                using (var entry = new DirectoryEntry(Path, UserName, PassWord, AuthenticationTypes.Secure))
                {
                    var forceBind = entry.NativeObject;

                    // declare object Res_Login
                    Res_Login result = new Res_Login();

                    // generate AC Token
                    string guid = Guid.NewGuid().ToString();
                    var Lifetem_Access = System.DateTime.Now.AddMinutes(AppSettings.AccessLiftTime);
                    result.token = JwtToken.GenerateToken(Payload.username, AppSettings.AccessSecretKey, guid, Lifetem_Access);
                    var token = result.token;

                    // generate RF Token
                    DateTime refresh_expire = System.DateTime.Now.AddHours(AppSettings.RefreshLiftTime);
                    result.refresh_token = JwtToken.GenerateToken(Payload.username, AppSettings.RefreshSecretKey, guid, refresh_expire);
                    var token_refresh = result.refresh_token;

                    return Ok(new
                    {
                        status = "success",
                        message = "correct",
                        user = Payload.username,
                        token,
                        token_refresh
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Uncorrect: " + ex.Message);
                return Unauthorized(new
                {
                    status = "error",
                    message = "uncorrect",
                    details = ex.Message
                });
            }
           
        }
    }
}
