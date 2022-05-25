using DataAccess.EFCore.RvpOfficeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{

    public class Province
    {
        public string Changwatshortname { get; set; }
        public string Changwatname { get; set; }
        public string Branchid { get; set; }
        public string ProvinceId { get; set; }

    }
}