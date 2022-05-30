using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class Address
    {
        public string HouseNo { get; set; }
        public string Moo { get; set; }
        public string Soi { get; set; }
        public string Road { get; set; }
        public string SubDistrict { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
        public DateTime? LastDate { get; set; }
    }
}
