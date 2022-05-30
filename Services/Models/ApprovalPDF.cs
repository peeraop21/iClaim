using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class ApprovalPDF
    {
        public string ClaimNo { get; set; }
        public double CureMoney { get; set; }
        public string TextCureMoney { get; set; }
        public string IdInvhd { get; set; }
        public int InvCount { get; set; }
        public string RecordDay { get; set; }
        public string RecordMonth { get; set; }
        public string RecordYear { get; set; }
        public string OtpSign { get; set; }
        public string HosId { get; set; }
        public bool? IsEverAuthorize { get; set; }
        public double? EverAuthorizeMoney { get; set; }
        public string EverAuthorizeHosId { get; set; }


    }
}
