using Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class ConfirmMoney
    {
        public int ReqNo { get; set; }
        public string ReqDate { get; set; }
        public string ReqTime { get; set; }
        public double SumReqMoney { get; set; }
        public double SumPayMoney { get; set; }
        public CarHasPolicy Car { get; set; }
        public InputBank BankAccount { get; set; }
        public List<InvoiceStatus> InvoiceList { get; set; }
    }
}
