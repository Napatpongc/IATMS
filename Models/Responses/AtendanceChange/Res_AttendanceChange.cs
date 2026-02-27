namespace IATMS.Models.Responses.AtendanceChange
{
    public class Res_AttChange
    {
        public string action { get; set; }          
        public DateOnly attDate { get; set; }        

        public string changeStatus { get; set; }     
        public string requestReason { get; set; }
        public string rejectReason { get; set; }



        public string ciTime { get; set; }          
        public string ciLocation { get; set; }
        public string ciLatlong { get; set; }
        public string ciReason { get; set; }         

       
        public string ciTimeNew { get; set; }        
        public string ciLocationNew { get; set; }    

       
        public string coTime { get; set; }           
        public string coLocation { get; set; }
        public string coLatlong { get; set; }
        public string coReason { get; set; }         

    
        public string coTimeNew { get; set; }        
        public string coLocationNew { get; set; }     
    }
    public class Res_ModalAttChange
    {
        public DateOnly attDate { get; set; }
        public string ciTime { get; set; }
        public string ciCorrectTime { get; set; }
        public string ciAddress { get; set; }
        public string ciLatlong { get; set; }
        public string ciCorrectZone { get; set; }
        public string ciReason { get; set; }
        public string coTime { get; set; }
        public string coCorrectTime { get; set; }
        public string coAddress { get; set; }
        public string coLatlong { get; set; }
        public string coCorrectZone { get; set; }
        public string coReason { get; set; }
        public string requestReason { get; set; }
        public string rejectReason { get; set; }

    }
}

