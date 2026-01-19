using IATMS.Models.Payloads;
using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;

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

                    return Ok(new
                    {
                        status = "success",
                        message = "correct",
                        user = Payload.username
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



        //static string GetDomainFromSignIn(string Login, string Password)
        //{
        //    string ldapHost = "localhost";
        //    int ldapPort = 389;


        //    //if (Login == "admin" && Password == "admin")
        //    //{
        //    //    return "correct";
        //    //}
        //    //else
        //    //    return "uncorrect";
        //}

    }
}
