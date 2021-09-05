
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
        public short? VictimNoClaim { get; set; }
        public short? RegNoClaim { get; set; }

        public double? MedicineMoney { get; set; }
        public double? PlasticMoney { get; set; }
        public double? ServiceMoney { get; set; }
        public double? RoomMoney { get; set; }
        public double? VeihcleMoney { get; set; }
        

        public double? CureMoney { get; set; }
        public double? DeadMoney { get; set; }
        public double? HygieneMoney { get; set; }
        public double? CrippledMoney { get; set; }

        public string BlindCrippled { get; set; }
        public string UnHearCrippled { get; set; }
        public string DeafCrippled { get; set; }
        public string LostSexualCrippled { get; set; }
        public string LostOrganCrippled { get; set; }
        public string LostMindCrippled { get; set; }
        public string CrippledPermanent { get; set; }
        public string OtherCrippled { get; set; }
        public string CrippledComment { get; set; }

        public string PayMore { get; set; }


    }
}
