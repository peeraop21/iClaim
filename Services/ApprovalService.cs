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
        Task<List<ApprovalregisViewModel>> GetApproval(string accNo);
        Task<ClaimViewModel> GetApprovalByAccNo(string accNo);
        Task<DataAccess.EFCore.DigitalClaimModels.HosApproval> AddAsync(DataAccess.EFCore.DigitalClaimModels.HosApproval hosApproval, InputBankViewModel inputBank);
    }


    public class ApprovalService : IApprovalService
    {
        private readonly DigitalclaimContext digitalclaimContext;
        private readonly RvpofficeContext rvpofficeContext;
        private readonly ClaimDataContext claimDataContext;

        public ApprovalService(DigitalclaimContext digitalclaimContext, RvpofficeContext rvpofficeContext, ClaimDataContext claimDataContext)
        {
            this.digitalclaimContext = digitalclaimContext;
            this.rvpofficeContext = rvpofficeContext;
            this.claimDataContext = claimDataContext;
        }
        public async Task<DataAccess.EFCore.DigitalClaimModels.HosApproval> AddAsync(DataAccess.EFCore.DigitalClaimModels.HosApproval hosApproval, InputBankViewModel inputBank)
        {
            /*var query = await rvpofficeContext.HosApproval.Where(w => w.AccNo == hosApproval.AccNo && w.VictimNo == hosApproval.VictimNo).Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.Pt4id }).LastOrDefaultAsync();*/
            var dataHosApproval = new DataAccess.EFCore.DigitalClaimModels.HosApproval();
            dataHosApproval.AccNo = hosApproval.AccNo;
            dataHosApproval.VictimNo = hosApproval.VictimNo;
            dataHosApproval.AppNo = hosApproval.AppNo + 1;
            dataHosApproval.SumMoney = hosApproval.SumMoney;
            dataHosApproval.CureMoney = hosApproval.SumMoney;
            dataHosApproval.RegDate = DateTime.Now;
            if (!string.IsNullOrEmpty(hosApproval.ClaimNo))
            {
                dataHosApproval.ClaimNo = hosApproval.ClaimNo;
            }
            else
            {
                dataHosApproval.ClaimNo = null;
            }         
            await digitalclaimContext.HosApproval.AddAsync(dataHosApproval);


            var dataHosDocumentReceive = new HosDocumentReceive();
            dataHosDocumentReceive.AccNo = hosApproval.AccNo;
            dataHosDocumentReceive.VictimNo = (short)hosApproval.VictimNo;
            dataHosDocumentReceive.Appno = (short)(hosApproval.AppNo + 1);
            dataHosDocumentReceive.AccountNo = inputBank.accountNumber;
            dataHosDocumentReceive.AccountName = inputBank.accountName;
            dataHosDocumentReceive.BankId = inputBank.accountBankName;
            await digitalclaimContext.HosDocumentReceive.AddAsync(dataHosDocumentReceive);
            await digitalclaimContext.SaveChangesAsync();

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
