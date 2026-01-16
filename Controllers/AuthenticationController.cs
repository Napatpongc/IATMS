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
        public async Task<IActionResult> Login([FromBody] Pay_signin Payload)
        {
            //// เปลี่ยน Domain เป็นของ มก.
            //string ldapPath = "LDAP://nontri.ku.ac.th";
            //string username = "b6530300155"; // หรือ username นนทรีของคุณ
            //string password = "@FOur14822346";

            //try
            //{
            //    // สำหรับ KU แนะนำให้ลองใช้ AuthenticationTypes.Secure ก่อน
            //    using (DirectoryEntry entry = new DirectoryEntry(ldapPath, username, password, AuthenticationTypes.Secure))
            //    {
            //        // บรรทัดนี้จะพยายาม Binding กับ Server ถ้าผ่านคือ Login สำเร็จ
            //        object nativeObject = entry.NativeObject;
            //        Console.WriteLine("KU Login Success!");
            //        return Ok(nativeObject);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Login Failed: " + ex.Message);
            //    return BadRequest();
            //}
            //}

            try
            {
                string Path = "LDAP://localhost:389";
                string UserName = Payload.username;
                string PassWord = Payload.password;

                using (var entry = new DirectoryEntry(Path, UserName, PassWord, AuthenticationTypes.None))
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
