using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.AccidentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{

    public class DirectPolicyKycViewModel
    {
        public int Kycno { get; set; }
        public string IdcardNo { get; set; }
        public string IdcardLaserCode { get; set; }
        public string Prefix { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public DateTime? DateofBirth { get; set; }      
        public string Gender { get; set; }
        public string CurrentHouseNo { get; set; }
        public string CurrentHmo { get; set; }
        public string CurrentSoi { get; set; }
        public string CurrentBuildingName { get; set; }
        public string CurrentRoad { get; set; }
        public string CurrentTumbolId { get; set; }
        public string CurrentCityId { get; set; }
        public string CurrentProvinceId { get; set; }
        public string CurrentZipcode { get; set; }
        public string CurrentAddrIsHomeAddr { get; set; }
        public string HomeHouseNo { get; set; }
        public string HomeHmo { get; set; }
        public string HomeSoi { get; set; }
        public string HomeBuildingName { get; set; }
        public string HomeRoad { get; set; }
        public string HomeTumbolId { get; set; }
        public string HomeCityId { get; set; }
        public string HomeProvinceId { get; set; }
        public string HomeZipcode { get; set; }
        public DateTime? InsertDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string LastUpdateIp { get; set; }
        public string HashData { get; set; }
        public string LineId { get; set; }
        public string Status { get; set; }
        public string StringDateofBirth { get; set; }
        public string Base64IdCard { get; set; }
        public string Base64Face { get; set; }

    }
}
