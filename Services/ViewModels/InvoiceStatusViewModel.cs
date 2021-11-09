using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class InvoiceStatusViewModel
    {
        public long IdInvhd { get; set; }
        public string Base64Image { get; set; }
        public string BookNo { get; set; }
        public string ReceiptNo { get; set; }
        public double? ReqMoney { get; set; }
        public double? PayMoney { get; set; }
    }
}
