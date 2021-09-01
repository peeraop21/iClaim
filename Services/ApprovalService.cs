using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.AccidentModels;
using Microsoft.EntityFrameworkCore;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.EFCore.RvpOfficeModels;
using DataAccess.EFCore.ClaimDataModels;

namespace Services
{
    public interface IApprovalService
    {
        Task<List<ApprovalregisViewModel>> GetApproval(string accNo);
    }


    public class ApprovalService : IApprovalService
    {
        private readonly ClaimDataContext claimDataContext;
        public ApprovalService(ClaimDataContext claimDataContext)
        {
            this.claimDataContext = claimDataContext;
        }

        public async Task<List<ApprovalregisViewModel>> GetApproval(string accNo)
        {

            var approvalVwMdList = new List<ApprovalregisViewModel>();
            var query = await claimDataContext.Approvalregis.Where(w => w.AccNo == accNo).Select(s => new { s.CrClaimno, s.VVictimno, s.ApRegno, s.ApRegdate, s.ApEntrydate, s.ApMoney, s.ApRevprefix, s.ApRevfname, s.ApRevlname, s.ApApproveby, s.ApRecprefix, s.ApRecfname, s.ApReclname, s.ApRecid, s.ApMoneydate, s.AccNo, s.ApTotal, s.DailyReceiveno, s.Pt4, s.ApStatus }).FirstOrDefaultAsync();
            var approvalVwModel = new ApprovalregisViewModel();
            if (query == null)
            {
                return null;
            }
            approvalVwModel.CrClaimno = query.CrClaimno;
            approvalVwModel.StringCrClaimno = query.CrClaimno.ToString().Replace("/", "-");
            approvalVwModel.VVictimno = query.VVictimno;
            approvalVwModel.ApRegno = query.ApRegno;
            approvalVwModel.ApRegdate = query.ApRegdate.Value.Date;
            approvalVwModel.StringApRegdate = approvalVwModel.ApRegdate.ToString().Replace("12:00:00 AM", " ");
            approvalVwModel.ApEntrydate = query.ApEntrydate;
            approvalVwModel.ApMoney = query.ApMoney;
            approvalVwModel.ApRevprefix = query.ApRevprefix;
            approvalVwModel.ApRevfname = query.ApRevfname;
            approvalVwModel.ApRevlname = query.ApRevlname;
            approvalVwModel.ApApproveby = query.ApApproveby;
            approvalVwModel.ApRecprefix = query.ApRecprefix;
            approvalVwModel.ApRecfname = query.ApRecfname;
            approvalVwModel.ApReclname = query.ApReclname;
            approvalVwModel.ApRecid = query.ApRecid;
            approvalVwModel.ApMoneydate = query.ApMoneydate;
            approvalVwModel.AccNo = query.AccNo;
            approvalVwModel.ApTotal = query.ApTotal;
            approvalVwModel.DailyReceiveno = query.DailyReceiveno;
            approvalVwModel.Pt4 = query.Pt4;
            approvalVwModel.ApStatus = query.ApStatus;
            approvalVwMdList.Add(approvalVwModel);
            return approvalVwMdList;
          
            
        }

  
    }
}
