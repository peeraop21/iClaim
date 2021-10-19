

using System.Collections.Generic;

namespace Core.ViewModels
{

    public class ApprovalViewModel
    {
        public string AccNo { get; set; }
        public int VictimNo { get; set; }
        public int AppNo { get; set; }
        public double? SumMoney { get; set; }
        public string ClaimNo { get; set; }
        public string UserIdLine { get; set; }
        public List<BillViewModel> BillsData { get; set; }
        public BankViewModel BankData { get; set; }
        public VictimViewModel VictimData { get; set; }

    }
}
