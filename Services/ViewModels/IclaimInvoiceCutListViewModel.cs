using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class IclaimInvoiceCutListViewModel
    {
        public long IdInvhd { get; set; }
        public int CutListNo { get; set; }
        public string CutListName { get; set; }
        public double? CutListPrice { get; set; }

    }
}
