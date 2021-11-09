using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class ApprovalDetailViewModel
    {
        public int ReqNo { get; set; }
        public string ReqDate { get; set; }
        public string ReqTime { get; set; }
        public double SumReqMoney { get; set; }
        public VictimtViewModel Victim { get; set; }
        public CarViewModel Car { get; set; }
        public List<InvoicehdViewModel> Invoicehds { get; set; }
        public InputBankViewModel BankAccount { get; set; }
    }
}
