using Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class ApprovalDetail
    {
        public int ReqNo { get; set; }
        public string ReqDate { get; set; }
        public string ReqTime { get; set; }
        public double SumReqMoney { get; set; }
        public Victim Victim { get; set; }
        public CarHasPolicy Car { get; set; }
        public List<InvoiceHeader> Invoicehds { get; set; }
        public InputBank BankAccount { get; set; }
    }
}
