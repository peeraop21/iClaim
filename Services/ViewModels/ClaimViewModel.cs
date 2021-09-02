
using System;


namespace Services.ViewModels
{
    
    public class ClaimViewModel
    {

        public string AccNo { get; set; }
        public int VictimNo { get; set; }
        public int AppNo { get; set; }        
        public double? SumMoney { get; set; }
        public string Pt4id { get; set; }
        public string ClaimNo { get; set; }

        public double? MedicineMoney { get; set; }
        public double? PlasticMoney { get; set; }
        public double? ServiceMoney { get; set; }
        public double? RoomMoney { get; set; }
        public double? VeihcleMoney { get; set; }
        

        public double? CureMoney { get; set; }
        public double? DeadMoney { get; set; }
        public double? HygieneMoney { get; set; }
        public double? CrippledMoney { get; set; }

    }
}
