using DataAccess.EFCore.BankNamesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{

    public class HospitalViewModel
    {
        public string Hospitalid { get; set; }
        public string Hospitalname { get; set; }
        public string Changwatshortname { get; set; }
        public string Hospitaltradename { get; set; }
        public string BranchId { get; set; }
        public string BranchShortName { get; set; }
        public string ChangwatShortName { get; set; }

    }
}