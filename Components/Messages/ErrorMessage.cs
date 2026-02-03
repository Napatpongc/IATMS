using AppName_API.Models.Responses;
using IATMS.Models.Responses;

namespace AppName_API.Components.Messages
{
    public static class ErrorMessage
    {
        public static Message GetMessage(int code, int sql_code = 0)
        {
            string message = string.Empty;

            switch (code)
            {
                #region common
               case 0:
                    message = "กำลังทำอยู่ครับ";
                    break;
                case 1001:
                    message = "Payload invalid.";
                    break;
                case 1002:
                    message = "Keyword is not empty.";
                    break;
                case 1003:
                    message = "ไม่สามารถลบได้ เนื่องจากมีการใช้ข้อมูลนี้อยู่";
                    break;
                case 1004:
                    message = "SQL Server Error Code: " + sql_code;
                    break;
                case 1005:
                    message = "ไม่สามารถแก้ไขข้อมูลได้";
                    break;

                #endregion

                #region UserController
                case 3001:
                    message = "ไม่พบ OA User ที่ระบุ";
                    break;
                case 3002:
                    message = "ไม่พบ Employee ID ที่ระบุ";
                    break;
                #endregion

                #region AuthenticationController
                case 2001:
                    message = "Username/Password invalid.";
                    break;
                case 2002:
                    message = "ไม่มี Username นี้ในระบบ กรุณาติดต่อผู้ดูแลระบบ";
                    break;
                #endregion

                default:
                    message = "Message Code: " + code;
                    break;
            }

            return new()
            {
                message = message,
                code = code
            };
        }
    }
}
