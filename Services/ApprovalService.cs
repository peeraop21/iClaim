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
using DataAccess.EFCore.ClaimDataModels;

namespace Services
{
    public interface IApprovalService
    {
        Task<List<ApprovalregisViewModel>> GetApproval(string accNo);
        Task<ClaimViewModel> GetApprovalByClaimNo(string claimNo, short victimNo, short regNo, string accNo);
        Task<ClaimViewModel> GetApprovalByAccNo(string accNo);
        Task<DataAccess.EFCore.DigitalClaimModels.HosApproval> AddAsync(DataAccess.EFCore.DigitalClaimModels.HosApproval hosApproval, InputBankViewModel inputBank, VictimtViewModel victim);
        Task<List<HosApprovalViewModel>> GetHosApprovalsAsync(string accNo, int victimNo);
        Task<List<InputBankViewModel>> GetHosDocumentReceiveAsync(string accNo, int victimNo, int appNo);
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
        public async Task<DataAccess.EFCore.DigitalClaimModels.HosApproval> AddAsync(DataAccess.EFCore.DigitalClaimModels.HosApproval hosApproval, InputBankViewModel inputBank, VictimtViewModel victim)
        {
            /*var query = await rvpofficeContext.HosApproval.Where(w => w.AccNo == hosApproval.AccNo && w.VictimNo == hosApproval.VictimNo).Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.Pt4id }).LastOrDefaultAsync();*/
            var dataHosApproval = new DataAccess.EFCore.DigitalClaimModels.HosApproval();
            dataHosApproval.AccNo = hosApproval.AccNo;
            dataHosApproval.VictimNo = hosApproval.VictimNo;
            dataHosApproval.AppNo = hosApproval.AppNo + 1;
            dataHosApproval.SumMoney = hosApproval.SumMoney;
            dataHosApproval.CureMoney = hosApproval.SumMoney;
            dataHosApproval.RegDate = DateTime.Now;
            dataHosApproval.RevPrefix = victim.Prefix;
            dataHosApproval.RevFname = victim.Fname;
            dataHosApproval.RevLname = victim.Lname;
            dataHosApproval.RevRelate = "000";
            dataHosApproval.RecPrefix = victim.Prefix;
            dataHosApproval.RecFname = victim.Fname;
            dataHosApproval.RecLname = victim.Lname;
            dataHosApproval.RecRelate = "000";
            dataHosApproval.RecSocNo = victim.DrvSocNo;
            if (!string.IsNullOrEmpty(hosApproval.ClaimNo))
            {
                dataHosApproval.ClaimNo = hosApproval.ClaimNo;
            }
            else
            {
                dataHosApproval.ClaimNo = null; //เคลมใหม่รอเลข running
            }
            await digitalclaimContext.HosApproval.AddAsync(dataHosApproval);


            var dataHosDocumentReceive = new HosDocumentReceive();
            dataHosDocumentReceive.AccNo = hosApproval.AccNo;
            dataHosDocumentReceive.VictimNo = (short)hosApproval.VictimNo;
            dataHosDocumentReceive.Appno = (short)(hosApproval.AppNo + 1);
            dataHosDocumentReceive.PaymentType = "D";
            dataHosDocumentReceive.AccountNo = inputBank.accountNumber;
            dataHosDocumentReceive.AccountName = inputBank.accountName;
            dataHosDocumentReceive.BankId = inputBank.accountBankName;
            await digitalclaimContext.HosDocumentReceive.AddAsync(dataHosDocumentReceive);
            

            var dataApprovalStatusState = new ApprovalStatusState();
            dataApprovalStatusState.AccNo = hosApproval.AccNo;
            dataApprovalStatusState.VictimNo = hosApproval.VictimNo;
            dataApprovalStatusState.AppNo = hosApproval.AppNo;
            dataApprovalStatusState.InsertDate = DateTime.Now;
            dataApprovalStatusState.Status = 1;
            await digitalclaimContext.ApprovalStatusState.AddAsync(dataApprovalStatusState);
            await digitalclaimContext.SaveChangesAsync();

            return hosApproval;
        }

        public async Task<ClaimViewModel> GetApprovalByAccNo(string accNo)
        {
            var query = await rvpofficeContext.HosApproval.Where(w => w.AccNo == accNo).Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.Pt4id, s.MedicineMoney, s.PlasticMoney, s.ServiceMoney, s.RoomMoney, s.VeihcleMoney, s.CureMoney, s.DeadMoney, s.HygieneMoney, s.CrippledMoney, s.SumMoney, s.BlindCrippled, s.UnHearCrippled, s.DeafCrippled, s.LostSexualCrippled, s.LostOrganCrippled, s.LostMindCrippled, s.CrippledPermanent, s.OtherCrippled, s.CrippledComment, s.PayMore }).OrderByDescending(o => o.AppNo).Take(1).FirstOrDefaultAsync();
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
                claimVwModel.PayMore = query.PayMore;


            }

            return claimVwModel;
        }
        public async Task<List<ApprovalregisViewModel>> GetApproval(string accNo)
        {

            var approvalVwMdList = new List<ApprovalregisViewModel>();
            var query = await claimDataContext.Approvalregis.Where(w => w.AccNo == accNo && w.Pt4 != null && w.Pt4 != "Compensate").Select(s => new { s.CrClaimno, s.VVictimno, s.ApRegno, s.ApRegdate, s.ApPaytype, s.ApMoney, s.AccNo, s.ApTotal, s.DailyReceiveno, s.Pt4, s.ApStatus }).ToListAsync();

           
            if(query != null)
            {
                foreach (var acc in query)
                {
                    var approvalVwModel = new ApprovalregisViewModel();

                    approvalVwModel.CrClaimno = acc.CrClaimno;
                    approvalVwModel.StringCrClaimno = acc.CrClaimno.ToString().Replace("/", "-");
                    approvalVwModel.VVictimno = acc.VVictimno;
                    approvalVwModel.ApRegno = acc.ApRegno;
                    approvalVwModel.ApRegdate = acc.ApRegdate ?? DateTime.Now;
                    approvalVwModel.StringApRegdate = acc.ApRegdate.ToString().Replace("12:00:00 AM", " ");
                    approvalVwModel.ApPaytype = acc.ApPaytype;
                    approvalVwModel.ApMoney = acc.ApMoney;
                    approvalVwModel.AccNo = acc.AccNo;
                    approvalVwModel.ApTotal = acc.ApTotal;
                    approvalVwModel.DailyReceiveno = acc.DailyReceiveno;
                    
                    approvalVwModel.Pt4 = acc.Pt4;
                    approvalVwModel.StringPt4 = approvalVwModel.Pt4.ToString().Replace("/", "-");
                    approvalVwModel.SubPt4 = approvalVwModel.Pt4.Substring(0, 3).Replace("บต", "pt");
                    
                    approvalVwModel.ApStatus = acc.ApStatus;
                    approvalVwModel.Claim = await GetApprovalByClaimNo(acc.CrClaimno, acc.VVictimno, acc.ApRegno, acc.AccNo);
                    approvalVwMdList.Add(approvalVwModel);

                }
            }
            return approvalVwMdList.OrderByDescending(o => o.ApRegdate).ToList();
        }

        public async Task<ClaimViewModel> GetApprovalByClaimNo(string claimNo, short victimNo, short regNo, string accNo)
        {
            var query = await rvpofficeContext.HosApproval.Where(w => w.ClaimNo == claimNo && w.VictimNoClaim == victimNo && w.RegNoClaim == regNo && w.AccNo == accNo && w.PayMore != "Y" &&  w.PayMore != "B").Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.VictimNoClaim, s.RegNoClaim, s.Pt4id, s.MedicineMoney, s.PlasticMoney, s.ServiceMoney, s.RoomMoney, s.VeihcleMoney, s.CureMoney, s.DeadMoney, s.HygieneMoney, s.CrippledMoney, s.SumMoney, s.BlindCrippled, s.UnHearCrippled, s.DeafCrippled, s.LostSexualCrippled, s.LostOrganCrippled, s.LostMindCrippled, s.CrippledPermanent, s.OtherCrippled, s.CrippledComment, s.PayMore }).OrderByDescending(o => o.AppNo).Take(1).FirstOrDefaultAsync();
            var claimVwModel = new ClaimViewModel();
            if (query != null)
            {

                claimVwModel.AccNo = query.AccNo;
                claimVwModel.VictimNo = query.VictimNo;
                claimVwModel.AppNo = query.AppNo;
                claimVwModel.ClaimNo = query.ClaimNo;
                claimVwModel.VictimNoClaim = query.VictimNoClaim;
                claimVwModel.RegNoClaim = query.RegNoClaim;
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
                claimVwModel.PayMore = query.PayMore;
            }
            return claimVwModel;
        }
        public async Task<List<HosApprovalViewModel>> GetHosApprovalsAsync(string accNo, int victimNo)
        {
            var query = await digitalclaimContext.HosApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo).Select(s => new { s.AccNo, s.AppNo, s.RegDate, }).FirstOrDefaultAsync();
            var vwHosAppList = new List<HosApprovalViewModel>();
            if (query == null)
            {
                return vwHosAppList;
            }

            var vwHosApp = new HosApprovalViewModel();
            vwHosApp.AccNo = query.AccNo;
            vwHosApp.StringAccNo = query.AccNo.Replace("/", "-");
            vwHosApp.AppNo = query.AppNo;
            vwHosApp.RegDate = query.RegDate;
            vwHosApp.StringRegDate = query.RegDate.ToString().Replace("T", " ");
            vwHosAppList.Add(vwHosApp);

            return vwHosAppList;
        }

        public async Task<List<InputBankViewModel>> GetHosDocumentReceiveAsync(string accNo, int victimNo, int appNo)
        {
            var query = await digitalclaimContext.HosDocumentReceive.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.Appno == appNo).Select(s => new { s.AccountNo, s.AccountName, s.BankId }).FirstOrDefaultAsync();
            var inputBankViewModelsList = new List<InputBankViewModel>();
            if (query == null)
            {
                return inputBankViewModelsList;
            }
            var inputBankViewModel = new InputBankViewModel();
            inputBankViewModel.accountNumber = query.AccountNo;
            inputBankViewModel.accountName = query.AccountName;
            inputBankViewModel.accountBankName = query.BankId;
            inputBankViewModelsList.Add(inputBankViewModel);
            return inputBankViewModelsList;
        }
    }

}
