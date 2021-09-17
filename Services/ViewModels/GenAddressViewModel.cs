using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.AccidentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    
    public class GenAddressViewModel
    {

        public string? ProvinceId { get; set; }
        public string? DistrictId { get; set; }
        public string? SubDistrictId { get; set; }
        public string? ZipCode { get; set; }
    }
}
