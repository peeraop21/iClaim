using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.AccidentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    
    public class VictimtViewModel
    {

        public string AccNo { get; set; }
        public int VictimNo { get; set; }
        public string Prefix { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Sex { get; set; }
        public short? Age { get; set; }
        public string IdCardNo { get; set; }
        public string HomeId { get; set; }
        public string Moo { get; set; }
        public string Soi { get; set; }
        public string Road { get; set; }
        public string Tumbol { get; set; }
        public string TumbolName { get; set; }
        public string District { get; set; }
        public string DistrictName { get; set; }
        public string Province { get; set; }
        public string ProvinceName { get; set; }
        public string Zipcode { get; set; }
        public string AccHomeId { get; set; }
        public string AccMoo { get; set; }
        public string AccSoi { get; set; }
        public string AccRoad { get; set; }
        public string AccTumbol { get; set; }
        public string AccTumbolName { get; set; }
        public string AccDistrict { get; set; }
        public string AccDistrictName { get; set; }
        public string AccProvince { get; set; }
        public string AccProvinceName { get; set; }
        public string AccZipcode { get; set; }
        public string TelNo { get; set; }
        public string VictimIs { get; set; }
        public string VictimType { get; set; }
        public string DetailBroken { get; set; }
    }
}
