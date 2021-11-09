using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class ConfirmMoneyViewModel
    {
        public int ReqNo { get; set; }
        public string ReqDate { get; set; }
        public string ReqTime { get; set; }
        public double SumReqMoney { get; set; }
        public double SumPayMoney { get; set; }
        public CarViewModel Car { get; set; }
        public InputBankViewModel BankAccount { get; set; }
        public List<InvoiceStatusViewModel> InvoiceList { get; set; }
    }
}
