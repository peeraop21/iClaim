﻿using DataAccess.EFCore.iPolicyModels;
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
        public string DrvSocNo { get; set; }
        public string HomeId { get; set; }
        public string Moo { get; set; }
        public string Soi { get; set; }
        public string Road { get; set; }
        public string Tumbol { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string Zipcode { get; set; }
        public string TelNo { get; set; }
    }
}