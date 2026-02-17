using AppName_API.Models.Responses.Authentication;
using IATMS.Models.Responses;
using System.Collections.Generic;

namespace IATMS.Components
{
    public static class MockData
    {
        public class DummyUser
        {
            public string username { get; set; }
            public string password { get; set; }
            public Res_Profile result { get; set; }
            public Res_Role role { get; set; } = new Res_Role();
        }

        public static readonly List<DummyUser> Users = new()
        {

            new DummyUser
            {
                username = "nattapol.prai",
                password = "@Int1234",
                result = new Res_Profile
                {
                    oa_user = "nattapol.prai",
                    Name_en = "nattapol",
                    Name_th = "สมชาย แซ่ลี้",
                    division_code = "Human Resources",
                    Team = "Recruitment",
                    Work_Place = "Head Office",
                    email = "somchai.s@company.com",
                    role_id = "HR"
                }
            },
            new DummyUser
            {
                username = "napattarapong.c",
                password = "1234",
                result = new Res_Profile
                {
                    oa_user = "napattarapong.c",
                    Name_en = "Napattarapong",
                    Name_th = "ณภัทรพงศ์ แช่มช้อย",
                    division_code = "Enterprise Resource Management",
                    Team = "IT Development",
                    Work_Place = "Head Office",
                    email = "nattapol.prai@ku.th",
                    role_id = "ADMIN"
                }
            },
            new DummyUser
            {
                username = "vipa.kong",
                password = "1234",
                result = new Res_Profile
                {
                    oa_user = "vipa.kong",
                    Name_en = "Vipa Kongdee",
                    Name_th = "วิภา คงดี",
                    division_code = "Finance & Accounting",
                    Team = "Accounting",
                    Work_Place = "Branch 01",
                    email = "vipa.k@company.com",
                    role_id = "USER"
                }
            },
            new DummyUser
            {
                username = "manit.rak",
                password = "1234",
                result = new Res_Profile
                {
                    oa_user = "manit.rak",
                    Name_en = "Manit Rakthai",
                    Name_th = "มานิต รักไทย",
                    division_code = "Operations",
                    Team = "Logistics",
                    Work_Place = "Warehouse A",
                    email = "manit.r@company.com",
                    role_id = "MANAGER"
                }
            },
            new DummyUser
            {
                username = "sarah.win",
                password = "1234",
                result = new Res_Profile
                {
                    oa_user = "sarah.win",
                    Name_en = "Sarah Winchester",
                    Name_th = "ซาร่า วินเชสเตอร",
                    division_code = "Marketing",
                    Team = "Digital Marketing",
                    Work_Place = "Head Office",
                    email = "sarah.w@company.com",
                    role_id = "USER"
                }
            }
        };
    }
}