using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class IclaimInvoiceCutList
    {
        public long IdInvhd { get; set; }
        public int CutListNo { get; set; }
        public string CutListName { get; set; }
        public double? CutListPrice { get; set; }

    }
}
