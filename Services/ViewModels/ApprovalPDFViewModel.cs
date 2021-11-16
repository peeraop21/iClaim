using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class ApprovalPDFViewModel
    {
        public double CureMoney { get; set; }
        public string TextCureMoney { get; set; }
        public string IdInvhd { get; set; }
        public int InvCount { get; set; }
        public string RecordDay { get; set; }
        public string RecordMonth { get; set; }
        public string RecordYear { get; set; }
        public string OtpSign { get; set; }

    }
}
