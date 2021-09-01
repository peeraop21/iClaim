using DataAccess.EFCore.DigitalClaimModels;
using DataAccess.EFCore.RvpOfficeModels;
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

        Task<ClaimViewModel> GetApprovalByAccNo(string accNo);
    }


    public class ApprovalService : IApprovalService
    {
        private readonly DigitalclaimContext digitalclaimContext;
        private readonly RvpofficeContext rvpofficeContext;
        
        public ApprovalService(DigitalclaimContext digitalclaimContext, RvpofficeContext rvpofficeContext)
        {
            this.digitalclaimContext = digitalclaimContext;
            this.rvpofficeContext = rvpofficeContext;
        }
        Task<List<ApprovalregisViewModel>> GetApproval(string accNo);
    }


        public async Task<DataAccess.EFCore.DigitalClaimModels.HosApproval> AddAsync(DataAccess.EFCore.DigitalClaimModels.HosApproval hosApproval)
        {
            /*var query = await rvpofficeContext.HosApproval.Where(w => w.AccNo == hosApproval.AccNo && w.VictimNo == hosApproval.VictimNo).Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.Pt4id }).LastOrDefaultAsync();*/
            
            hosApproval.AppNo = hosApproval.AppNo + 1;
            await digitalclaimContext.HosApproval.AddAsync(hosApproval);

            return hosApproval;
        }

        public async Task<ClaimViewModel> GetApprovalByAccNo(string accNo)
        {
            var query = await rvpofficeContext.HosApproval.Where(w => w.AccNo == accNo).Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.Pt4id }).OrderByDescending(o => o.AppNo).Take(1).FirstOrDefaultAsync();
            var claimVwModel = new ClaimViewModel();
            if (query != null)
            {
                
                claimVwModel.AccNo = query.AccNo;
                claimVwModel.VictimNo = query.VictimNo;
                claimVwModel.AppNo = query.AppNo;
                claimVwModel.ClaimNo = query.ClaimNo;
                claimVwModel.Pt4id = query.Pt4id;
            }
            
            return claimVwModel;
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
