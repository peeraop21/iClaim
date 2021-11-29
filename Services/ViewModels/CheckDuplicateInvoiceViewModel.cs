using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class CheckDuplicateInvoiceViewModel
    {
        public string AccNo { get; set; }
        public int VictimNo { get; set; }
        public int ReqNo { get; set; }
        public int BillId { get; set; }
        public string BookNo { get; set; }
        public string ReceiptNo { get; set; }
        public string HosId { get; set; }
        public bool IsDuplicate { get; set; }
    }
}
