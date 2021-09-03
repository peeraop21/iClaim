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
            var query = await rvpofficeContext.HosApproval.Where(w => w.AccNo == accNo).Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.Pt4id, s.MedicineMoney, s.PlasticMoney, s.ServiceMoney, s.RoomMoney, s.VeihcleMoney, s.CureMoney, s.DeadMoney, s.HygieneMoney, s.CrippledMoney, s.SumMoney, s.BlindCrippled, s.UnHearCrippled, s.DeafCrippled, s.LostSexualCrippled, s.LostOrganCrippled, s.LostMindCrippled, s.CrippledPermanent, s.OtherCrippled, s.CrippledComment }).OrderByDescending(o => o.AppNo).Take(1).FirstOrDefaultAsync();
            var claimVwModel = new ClaimViewModel();
            if (query != null)
            {

                claimVwModel.AccNo = query.AccNo;
                claimVwModel.VictimNo = query.VictimNo;
                claimVwModel.AppNo = query.AppNo;
                claimVwModel.ClaimNo = query.ClaimNo;
                claimVwModel.Pt4id = query.Pt4id;
                claimVwModel.MedicineMoney = query.MedicineMoney;
                claimVwModel.PlasticMoney = query.PlasticMoney;
                claimVwModel.ServiceMoney = query.ServiceMoney;
                claimVwModel.RoomMoney = query.RoomMoney;
                claimVwModel.VeihcleMoney = query.VeihcleMoney;
                claimVwModel.CureMoney = query.CureMoney;
                claimVwModel.DeadMoney = query.DeadMoney;
                claimVwModel.HygieneMoney = query.HygieneMoney;
                claimVwModel.CrippledMoney = query.CrippledMoney;
                claimVwModel.SumMoney = query.SumMoney;
                claimVwModel.BlindCrippled = query.BlindCrippled;
                claimVwModel.UnHearCrippled = query.UnHearCrippled;
                claimVwModel.DeafCrippled = query.DeafCrippled;
                claimVwModel.LostSexualCrippled = query.LostSexualCrippled;
                claimVwModel.LostOrganCrippled = query.LostOrganCrippled;
                claimVwModel.LostMindCrippled = query.LostMindCrippled;
                claimVwModel.CrippledPermanent = query.CrippledPermanent;
                claimVwModel.OtherCrippled = query.OtherCrippled;
                claimVwModel.CrippledComment = query.CrippledComment;
                
                
            }

            return claimVwModel;
        }
        public async Task<List<ApprovalregisViewModel>> GetApproval(string accNo)
        {

            var approvalVwMdList = new List<ApprovalregisViewModel>();
            var query = await claimDataContext.Approvalregis.Where(w => w.AccNo == accNo).Select(s => new { s.CrClaimno, s.VVictimno, s.ApRegno, s.ApRegdate, s.ApPaytype, s.ApMoney, s.AccNo, s.ApTotal, s.DailyReceiveno, s.Pt4, s.ApStatus }).FirstOrDefaultAsync();
            var approvalVwModel = new ApprovalregisViewModel();
            if (query != null)
            {
                approvalVwModel.CrClaimno = query.CrClaimno;
            approvalVwModel.StringCrClaimno = query.CrClaimno.ToString().Replace("/", "-");
            approvalVwModel.VVictimno = query.VVictimno;
            approvalVwModel.ApRegno = query.ApRegno;
            approvalVwModel.ApRegdate = query.ApRegdate.Value.Date;
            approvalVwModel.StringApRegdate = approvalVwModel.ApRegdate.ToString().Replace("12:00:00 AM", " ");
            approvalVwModel.ApPaytype = query.ApPaytype;
            approvalVwModel.ApMoney = query.ApMoney;
            approvalVwModel.AccNo = query.AccNo;
            approvalVwModel.ApTotal = query.ApTotal;
            approvalVwModel.DailyReceiveno = query.DailyReceiveno;
            approvalVwModel.Pt4 = query.Pt4;
            approvalVwModel.StringPt4 = approvalVwModel.Pt4.ToString().Replace("/", "-");
            approvalVwModel.SubPt4 = approvalVwModel.Pt4.Substring(0, 3).Replace("บต", "pt");
            approvalVwModel.ApStatus = query.ApStatus;
            approvalVwMdList.Add(approvalVwModel);
            }
            
            return approvalVwMdList;


        }
    }


   


}
