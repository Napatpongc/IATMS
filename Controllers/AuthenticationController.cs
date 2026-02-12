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

        private class DummyUser
        {
            public string username { get; set; }
            public string password { get; set; }
            public Res_Role role { get; set; }
            public Res_Profile profile { get; set; }
        }

        private static readonly List<DummyUser> _dummyUsers = new()
{
    new DummyUser
    {
        username = "admin01",
        password = "1234",
        role = new Res_Role
        {
            menu_attendance=true, menu_report=true, menu_admin=true, menu_setup=true,
            func_approve=true, func_cico=true, func_rp_attendance=true, func_rp_work_hours=true, func_rp_compensation=true,

            menu_spare1=false, menu_spare2=false, menu_spare3=false, menu_spare4=false, menu_spare5=false,
            func_spare1=false, func_spare2=false, func_spare3=false, func_spare4=false, func_spare5=false,
            func_spare6=false, func_spare7=false, func_spare8=false, func_spare9=false, func_spare10=false,
        },
        profile = new Res_Profile
        {
            oa_user = "OA001",
            Name_en = "Admin One",
            Name_th = "แอดมิน วัน",
            division_code = "D001",
            Team = "IT Core",
            Work_Place = "HQ",
            email = "admin01@company.com",
            role_id = "ADMIN"
        }
    },

    new DummyUser
    {
        username = "hr01",
        password = "1234",
        role = new Res_Role
        {
            menu_attendance=true, menu_report=true, menu_admin=false, menu_setup=true,
            func_approve=true, func_cico=false, func_rp_attendance=true, func_rp_work_hours=true, func_rp_compensation=true,

            menu_spare1=false, menu_spare2=false, menu_spare3=false, menu_spare4=false, menu_spare5=false,
            func_spare1=false, func_spare2=false, func_spare3=false, func_spare4=false, func_spare5=false,
            func_spare6=false, func_spare7=false, func_spare8=false, func_spare9=false, func_spare10=false,
        },
        profile = new Res_Profile
        {
            oa_user = "OA002",
            Name_en = "HR One",
            Name_th = "เอชอาร์ วัน",
            division_code = "D002",
            Team = "Human Resources",
            Work_Place = "HQ",
            email = "hr01@company.com",
            role_id = "HR"
        }
    },

    new DummyUser
    {
        username = "manager01",
        password = "1234",
        role = new Res_Role
        {
            menu_attendance=true, menu_report=true, menu_admin=false, menu_setup=false,
            func_approve=true, func_cico=true, func_rp_attendance=true, func_rp_work_hours=true, func_rp_compensation=false,

            menu_spare1=false, menu_spare2=false, menu_spare3=false, menu_spare4=false, menu_spare5=false,
            func_spare1=false, func_spare2=false, func_spare3=false, func_spare4=false, func_spare5=false,
            func_spare6=false, func_spare7=false, func_spare8=false, func_spare9=false, func_spare10=false,
        },
        profile = new Res_Profile
        {
            oa_user = "OA003",
            Name_en = "Manager One",
            Name_th = "ผู้จัดการ วัน",
            division_code = "D003",
            Team = "Operations",
            Work_Place = "Plant-1",
            email = "manager01@company.com",
            role_id = "MANAGER"
        }
    },

    new DummyUser
    {
        username = "user01",
        password = "1234",
        role = new Res_Role
        {
            menu_attendance=true, menu_report=false, menu_admin=false, menu_setup=false,
            func_approve=false, func_cico=true, func_rp_attendance=false, func_rp_work_hours=false, func_rp_compensation=false,

            menu_spare1=false, menu_spare2=false, menu_spare3=false, menu_spare4=false, menu_spare5=false,
            func_spare1=false, func_spare2=false, func_spare3=false, func_spare4=false, func_spare5=false,
            func_spare6=false, func_spare7=false, func_spare8=false, func_spare9=false, func_spare10=false,
        },
        profile = new Res_Profile
        {
            oa_user = "OA004",
            Name_en = "User One",
            Name_th = "ผู้ใช้ วัน",
            division_code = "D003",
            Team = "Operations",
            Work_Place = "Plant-1",
            email = "user01@company.com",
            role_id = "EMPLOYEE"
        }
    },

    new DummyUser
    {
        username = "report01",
        password = "1234",
        role = new Res_Role
        {
            menu_attendance=false, menu_report=true, menu_admin=false, menu_setup=false,
            func_approve=false, func_cico=false, func_rp_attendance=true, func_rp_work_hours=true, func_rp_compensation=false,

            menu_spare1=false, menu_spare2=false, menu_spare3=false, menu_spare4=false, menu_spare5=false,
            func_spare1=false, func_spare2=false, func_spare3=false, func_spare4=false, func_spare5=false,
            func_spare6=false, func_spare7=false, func_spare8=false, func_spare9=false, func_spare10=false,
        },
        profile = new Res_Profile
        {
            oa_user = "OA005",
            Name_en = "Report Viewer",
            Name_th = "ผู้ดูรายงาน",
            division_code = "D004",
            Team = "Finance",
            Work_Place = "HQ",
            email = "report01@company.com",
            role_id = "REPORT"
        }
    },
};

        [HttpPost("SignIn")]
        public async Task<IActionResult> Login([FromBody] Pay_signin Payload)
        {

            string Path = AppSettings.LdapPath;

            string UserName = Payload.username;
            string PassWord = Payload.password;

            //try
            //{
            //    using (var entry = new DirectoryEntry(Path, UserName, PassWord, AuthenticationTypes.Secure))
            //    {
            //        var forceBind = entry.NativeObject;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Uncorrect: " + ex.Message);
            //    return Unauthorized(new
            //    {
            //        status = "error",
            //        message = "uncorrect",
            //        details = ex.Message
            //    });
            //}

            // declare object Res_Login
            Res_Login result = new Res_Login();
            // generate AC Token
            string guid = Guid.NewGuid().ToString();
            var Lifetem_Access = System.DateTime.Now.AddSeconds(AppSettings.AccessLiftTime);
            result.token = JwtToken.GenerateToken(UserName, AppSettings.AccessSecretKey, guid, Lifetem_Access);

            // generate RF Token
            DateTime refresh_expire = System.DateTime.Now.AddSeconds(AppSettings.RefreshLiftTime);
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
        [HttpPost("user_profile_dummy")]
        public async Task<IActionResult> dummyUser([FromBody] Pay_dummy Payload)
        {
            if (Payload == null || string.IsNullOrWhiteSpace(Payload.username))
                return BadRequest(new { message = "username is required" });

            var username = Payload.username.Trim();

            var user = _dummyUsers.FirstOrDefault(u =>
                u.username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                // จะใช้ ErrorMessage.GetMessage(2002) ก็ได้ ถ้าคุณอยากให้ format เหมือนของเดิม
                // Message msg = ErrorMessage.GetMessage(2002);
                // return BadRequest(msg);

                return NotFound(new { message = "user not found" });
            }

            // สร้าง Res_Login เหมือน SignIn
            Res_Login result = new Res_Login();

            string guid = Guid.NewGuid().ToString();

            var accessExpire = DateTime.Now.AddHours(AppSettings.AccessLiftTime);
            result.token = JwtToken.GenerateToken(user.username, AppSettings.AccessSecretKey, guid, accessExpire);

            DateTime refresh_expire = DateTime.Now.AddHours(AppSettings.RefreshLiftTime);
            result.refresh_token = JwtToken.GenerateToken(user.username, AppSettings.RefreshSecretKey, guid, refresh_expire);

            result.role = user.role;
            result.profile = user.profile;

            //// (ถ้าคุณอยากเก็บ refresh token ลง DB ให้เหมือน SignIn)
            //try
            //{
            //    await ConDB.tokenRefresh(user.username, result.refresh_token, refresh_expire);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, exception_msg + ex.Message);
            //    return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + " : " + ex.Message);
            //}

            return Ok(result);
        }


    }
    
}
