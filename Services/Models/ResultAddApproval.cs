using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class ResultAddApproval
    {
        public long IdInvhd { get; set; }
        public string AccNo { get; set; }
        public int? VictimNo { get; set; }
        public int? IclaimAppNo { get; set; }
    }
}
