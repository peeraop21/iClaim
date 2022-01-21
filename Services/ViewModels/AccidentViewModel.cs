using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.AccidentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    
    public class AccidentViewModel
    {
        public string AccNo { get; set; }     
        public int? VictimNo { get; set; }
        public string BranchId { get; set; }
        public ClaimViewModel LastClaim { get; set; }
        public string StringAccNo { get; set; }
        public DateTime AccDate { get; set; }
        public string StringAccDate { get; set; }
        public string StringAccDateSearch { get; set; }
        public string AccNature { get; set; }
        public string PlaceAcc { get; set; }
        public string ProvAcc { get; set; }
        public List<VictimtViewModel> Victim { get; set; }
        public List<string> Car { get; set; }
        public string Channel { get; set; }
        public double? CureRightsBalance { get; set; }
        public double? CrippledRightsBalance { get; set; }
        public int CountHosApp { get; set; }
        public int CountNotify { get; set; }

        /*public string EaCarLicense { get; set; }
        public string EaPrefixVictim { get; set; }
        public string EaFnameVictim { get; set; }
        public string EaLnameVictim { get; set; }
        public List<string> EaIdCardVictim { get; set; }
        public string EaPhoneNumber { get; set; }
        public string TokenId { get; set; }*/
    }
}
