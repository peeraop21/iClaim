

using System.Collections.Generic;

namespace Core.Models
{

    public class ApprovalReq
    {
        public string AccNo { get; set; }
        public int VictimNo { get; set; }
        public int AppNo { get; set; }
        public string BranchId { get; set; }
        public double? SumMoney { get; set; }
        public string ClaimNo { get; set; }
        public string UserIdLine { get; set; }
        public List<Bill> BillsData { get; set; }
        public BankInput BankData { get; set; }
        public VictimDetail VictimData { get; set; }
        public string RefCodeOtp { get; set; }
        public bool? IsEverAuthorize { get; set; }
        public double? EverAuthorizeMoney { get; set; }
        public string EverAuthorizeHosId { get; set; }
        public int RightsType { get; set; }




    }
}
