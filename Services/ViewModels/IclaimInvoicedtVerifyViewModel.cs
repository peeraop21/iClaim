using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class IclaimInvoicedtVerifyViewModel
    {
        public long IdInvdt { get; set; }
        public string Hospital { get; set; }
        public string TakenDate { get; set; }
        public string TakenTime { get; set; }
        public double? SumMoney { get; set; }
        public object Items { get; set; }


    }
}
