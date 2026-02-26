namespace IATMS.Models.Responses.AttendanceApproval
{
    public class Res_AttendanceApproval
    {
        public DateOnly attDate { get; set; }

        public string changeStatus { get; set; }
        public string oaUser { get; set; }

        // แนะนำให้แก้ alias ใน SQL เป็น full_name / fullName จะ map ง่ายกว่า
        public string fullName { get; set; }
        public string team { get; set; }


        public string requestReason { get; set; }

        // Check-In (old/new)
        public string ciTimeOld { get; set; }
        public string ciAddressOld { get; set; }
        public string ciRequestReason { get; set; }

        public string ciTimeNew { get; set; }
        public string ciAddressNew { get; set; }

        // Check-Out (old/new)
        public string coTimeOld { get; set; }
        public string coAddressOld { get; set; }
        public string coRequestReason { get; set; }

        public string coTimeNew { get; set; }
        public string coAddressNew { get; set; }
    }

    public class Res_ModalAttApproval
    {
        public string? oa_user { get; set; }
        public DateOnly attDate { get; set; }

        // Check-In
        public string? ciTimeOld { get; set; }
        public string? ciTimeNew { get; set; }
        public string? ciLocationOld { get; set; }
        public string? ciLocationNew { get; set; }
        public string? ciAddressOld { get; set; }
        public string? ciAddressNew { get; set; }
        public string? ciRequestReason { get; set; }

        // Check-Out
        public string? coTimeOld { get; set; }
        public string? coTimeNew { get; set; }
        public string? coLocationOld { get; set; }
        public string? coLocationNew { get; set; }
        public string? coAddressOld { get; set; }
        public string? coAddressNew { get; set; }
        public string? coRequestReason { get; set; }

        public string? requestReason { get; set; }
    }
}
