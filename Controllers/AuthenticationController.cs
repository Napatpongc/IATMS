using IATMS.Models.Payloads;
using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices.Protocols;
using System.Net;

namespace IATMS.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost("signin")]
        public IActionResult Login([FromBody] Pay_signin input)
        {

            using var conn = new LdapConnection(new LdapDirectoryIdentifier("localhost", 389),new NetworkCredential($"cn={input.username},ou=users,dc=example,dc=org", input.password),AuthType.Basic);


            try
            {
                conn.Bind();
                return Ok(new { success = true, message = "LDAP bind success" });
            }
            catch (LdapException)
            {
                return Unauthorized(new { success = false, message = "Invalid username or password" });
            }
        }
    }
}
