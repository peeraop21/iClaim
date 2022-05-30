using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class InvoiceStatus
    {
        public long IdInvhd { get; set; }
        public List<string> Base64Image { get; set; }
        public string BookNo { get; set; }
        public string ReceiptNo { get; set; }
        public double? ReqMoney { get; set; }
        public double? PayMoney { get; set; }
        public IclaimInvoicedtVerify InvoicedtVerify { get; set; }
        public List<IclaimInvoiceCutList> InvoiceCutList { get; set; }
    }
}
