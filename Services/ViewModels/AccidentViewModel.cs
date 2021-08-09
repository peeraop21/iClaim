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
        public string EaTmpId { get; set; }
        public string EaAccNo { get; set; }
        public DateTime EaAccDate { get; set; }
        public string stringAccDate { get; set; }
        public List<TbAccidentMasterLineCar> EaCar { get; set; }
        /*public string EaCarLicense { get; set; }
        public string EaPrefixVictim { get; set; }
        public string EaFnameVictim { get; set; }
        public string EaLnameVictim { get; set; }
        public List<string> EaIdCardVictim { get; set; }
        public string EaPhoneNumber { get; set; }
        public string TokenId { get; set; }*/
    }
}
