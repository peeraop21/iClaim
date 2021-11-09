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
using LinqKit;
using DataAccess.EFCore.RvpSystemModels;
using DataAccess.EFCore.iPolicyModels;

namespace Services
{
    public interface IApprovalService
    {
        Task<List<ApprovalregisViewModel>> GetApprovalRegis(string accNo, int victimNo, int rightsType);
        Task<ClaimViewModel> GetApprovalByClaimNo(string claimNo, short victimNo, short regNo, int rightsType);
        Task<ClaimViewModel> GetApprovalByAccNo(string accNo, int? victimNo);
        Task<List<ResultApprovalPostViewModel>> AddAsync(IclaimApproval iclaimApproval, InputBankViewModel inputBank, VictimtViewModel victim, Invoicehd[] invoicehd, string userLineId);
        Task<string> UpdateAsync(string accNo, int victimNo, int appNo, UpdateBankViewModel bankModel, UpdateInvoiceViewModel[] invModel);
        Task<List<IcliamApprovalViewModel>> GetIClaimApprovalAsync(string accNo, int victimNo);
        Task<InputBankViewModel> GetIClaimBankAccountAsync(string accNo, int victimNo, int reqNo);
        Task<List<InputBankViewModel>> GetLastIClaimBankAccountAsync(string accNo, int victimNo);
        Task<double?> GetRightsBalance(string accNo, int? victimNo, string rightsType);
        Task<string> UpdateApprovalStatusAsync(string accNo, int victimNo, int appNo, string status);
        Task<List<InvoicehdViewModel>> GetInvoicehdAsync(string accNo, int victimNo, int reqNo, int status);
        Task<IclaimCheckDocuments> GetDocumentCheck(string accNo, int victimNo, int appNo);
        Task<ConfirmMoneyViewModel> GetDataForConfirmMoney(string accNo, int victimNo, int reqNo);
        //Task<string> GeneratePT(string accN);
        Task<object> GetHistoryInvoicedt(string pt4);
        Task<ApprovalDetailViewModel> GetApprovalDetail(string accNo, int victimNo, int reqNo, string userIdCard);
        Task<ApprovalPDFViewModel> GetApprovalDataForGenPDF(string accNo, int victimNo, int reqNo);
    }


    public class ApprovalService : IApprovalService
    {
        private readonly DigitalclaimContext digitalclaimContext;
        private readonly RvpofficeContext rvpofficeContext;
        private readonly ClaimDataContext claimDataContext;
        private readonly IAttachmentService attachmentService;
        private readonly RvpSystemContext rvpSystemContext;
        private readonly IpolicyContext ipolicyContext;

        public ApprovalService(DigitalclaimContext digitalclaimContext, RvpofficeContext rvpofficeContext, ClaimDataContext claimDataContext, IAttachmentService attachmentService, RvpSystemContext rvpSystemContext, IpolicyContext ipolicyContext)
        {
            this.digitalclaimContext = digitalclaimContext;
            this.rvpofficeContext = rvpofficeContext;
            this.claimDataContext = claimDataContext;
            this.attachmentService = attachmentService;
            this.rvpSystemContext = rvpSystemContext;
            this.ipolicyContext = ipolicyContext;


        }
        //public async Task<string> GeneratePT(string accNo)
        //{
        //    var branchId = await rvpofficeContext.HosAccident
        //        .Join(rvpofficeContext.Branch, acc => acc.AccProv, branch => branch.ChangwatShortName, (acc, branch) => new { accJoinBranch = acc, branchId = branch.Branchid, branchOff = branch.Branchoff, canRegis = branch.CanRegis, chwShortname = branch.ChangwatShortName })
        //        .Where(w => w.accJoinBranch.AccNo == accNo && w.branchOff == "1" && w.canRegis == "Y" && !string.IsNullOrEmpty(w.chwShortname))
        //        .Select(s => s.branchId)
        //        .FirstOrDefaultAsync();
        //    var ptType = "บต3";
        //    var year = (DateTime.Now.Year + 543).ToString();
        //    var lastTwoYear = year.Substring(year.Length - 2);
        //    var conditionQuery = ptType + "/" + branchId + "/";
        //    var templatPt = ptType + "/" + branchId + "/" + lastTwoYear;
        //    var lastPt4id = await rvpofficeContext.HosPt4.Where(w => w.Pt4id.StartsWith(conditionQuery)).Select(s => s.Pt4id).OrderByDescending(o => o).Take(1).FirstOrDefaultAsync();
        //    var runNo = int.Parse(lastPt4id.Substring(lastPt4id.Length - 4)) + 1;
        //    var leadZeroRunNo = runNo.ToString().PadLeft(4, '0');
        //    var pt4id = templatPt + "/" + leadZeroRunNo;
        //    //var query = await digitalclaimContext.HosApproval.Where
        //    return pt4id;
        //}
        public async Task<List<ResultApprovalPostViewModel>> AddAsync(IclaimApproval iclaimApproval, InputBankViewModel inputBank, VictimtViewModel victim, Invoicehd[] invoicehd, string userLineId)
        {
            /*var query = await rvpofficeContext.HosApproval.Where(w => w.AccNo == hosApproval.AccNo && w.VictimNo == hosApproval.VictimNo).Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.Pt4id }).LastOrDefaultAsync();*/
            var lastAppNo = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == iclaimApproval.AccNo && w.VictimNo == iclaimApproval.VictimNo).Select(s => s.ReqNo).OrderByDescending(o => o).FirstOrDefaultAsync();
            if (lastAppNo == null)
            {
                lastAppNo = 0;
            }
            var dataIcliamApproval = new IclaimApproval();
            dataIcliamApproval.AccNo = iclaimApproval.AccNo;
            dataIcliamApproval.VictimNo = iclaimApproval.VictimNo;
            dataIcliamApproval.ReqNo = lastAppNo + 1;
            dataIcliamApproval.Status = 1;
            dataIcliamApproval.SumReqMoney = iclaimApproval.SumReqMoney;
            dataIcliamApproval.CureMoney = iclaimApproval.SumReqMoney;
            dataIcliamApproval.InsertDate = DateTime.Now;
            dataIcliamApproval.LastUpdate = DateTime.Now;
            await digitalclaimContext.IclaimApproval.AddAsync(dataIcliamApproval);

            //var runNo = await digitalclaimContext.HosDocumentReceive.Where(w => w.AccNo == iclaimApproval.AccNo && w.VictimNo == (short)iclaimApproval.VictimNo && w.Appno == (short)dataIcliamApproval.IclaimAppNo).Select(s => s.RunNo).OrderByDescending(o => o).FirstOrDefaultAsync();
            //var dataHosDocumentReceive = new HosDocumentReceive();
            //dataHosDocumentReceive.AccNo = iclaimApproval.AccNo;
            //dataHosDocumentReceive.VictimNo = (short)iclaimApproval.VictimNo;
            //dataHosDocumentReceive.Appno = (short)(lastAppNo+1);
            //dataHosDocumentReceive.RunNo = (runNo == null || runNo  == 0) ? (short)1 : (short)(runNo + 1);
            //dataHosDocumentReceive.PaymentType = "D";
            //dataHosDocumentReceive.AccountNo = inputBank.accountNumber;
            //dataHosDocumentReceive.AccountName = inputBank.accountName;
            //dataHosDocumentReceive.BankId = inputBank.bankId;
            //await digitalclaimContext.HosDocumentReceive.AddAsync(dataHosDocumentReceive);            

            var dataBankAccount = new IclaimBankAccount();
            dataBankAccount.AccNo = iclaimApproval.AccNo;
            dataBankAccount.VictimNo = (short)iclaimApproval.VictimNo;
            dataBankAccount.ReqNo = (short)(lastAppNo + 1);
            dataBankAccount.RunNo = 1;
            dataBankAccount.AccountNo = inputBank.accountNumber;
            dataBankAccount.AccountName = inputBank.accountName;
            dataBankAccount.BankId = inputBank.bankId;
            await digitalclaimContext.IclaimBankAccount.AddAsync(dataBankAccount);

            var dataIclaimApprovalStatusState = new IclaimApprovalState();
            dataIclaimApprovalStatusState.AccNo = iclaimApproval.AccNo;
            dataIclaimApprovalStatusState.ReqNo = lastAppNo + 1;
            dataIclaimApprovalStatusState.VictimNo = iclaimApproval.VictimNo;
            dataIclaimApprovalStatusState.StateNo = 1;
            dataIclaimApprovalStatusState.OldStatus = null;
            dataIclaimApprovalStatusState.NewStatus = 1;
            dataIclaimApprovalStatusState.InsertDate = DateTime.Now;
            await digitalclaimContext.IclaimApprovalState.AddAsync(dataIclaimApprovalStatusState);

            var idInvhd = await rvpofficeContext.Invoicehd.Select(s => s.IdInvhd).OrderByDescending(o => o).FirstOrDefaultAsync();
            List<ResultApprovalPostViewModel> result = new List<ResultApprovalPostViewModel>();
            for (int i = 0; i < invoicehd.Length; i++)
            {
                var dataResult = new ResultApprovalPostViewModel();
                dataResult.IdInvhd = idInvhd + i + 1;
                dataResult.AccNo = iclaimApproval.AccNo;
                dataResult.VictimNo = iclaimApproval.VictimNo;
                dataResult.IclaimAppNo = lastAppNo + 1;
                result.Add(dataResult);

                var dataInvoicehd = new Invoicehd();
                dataInvoicehd.IdInvhd = idInvhd + i + 1;
                dataInvoicehd.AccNo = iclaimApproval.AccNo;
                dataInvoicehd.VictimNo = iclaimApproval.VictimNo;
                //dataInvoicehd.AppNo = null  /*lastAppNo + 1*/;
                dataInvoicehd.Victimname = victim.Prefix + victim.Fname + " " + victim.Lname;
                dataInvoicehd.Mainconsider = invoicehd[i].Mainconsider;
                dataInvoicehd.Takendate = invoicehd[i].Takendate;
                dataInvoicehd.Takentime = invoicehd[i].Takentime;
                dataInvoicehd.Dispensedate = invoicehd[i].Dispensedate;
                dataInvoicehd.Dispensetime = invoicehd[i].Dispensetime;
                TimeSpan calDate = (TimeSpan)(invoicehd[i].Dispensedate - invoicehd[i].Takendate);
                dataInvoicehd.Daybed = (int?)calDate.TotalDays;
                dataInvoicehd.BookNo = invoicehd[i].BookNo;
                dataInvoicehd.ReceiptNo = invoicehd[i].ReceiptNo;
                dataInvoicehd.Suminv = invoicehd[i].Suminv;
                dataInvoicehd.VictimType = invoicehd[i].VictimType;
                dataInvoicehd.Hosid = invoicehd[i].Hosid;
                dataInvoicehd.RecordBy = "LineID";
                await rvpofficeContext.Invoicehd.AddAsync(dataInvoicehd);

                var dataInvStatus = new IclaimInvoiceStatus();
                dataInvStatus.IdInvhd = idInvhd + i + 1;
                dataInvStatus.AccNo = iclaimApproval.AccNo;
                dataInvStatus.VictimNo = iclaimApproval.VictimNo;
                dataInvStatus.ReqNo = lastAppNo + 1;
                dataInvStatus.Status = 1;
                dataInvStatus.InsertDate = DateTime.Now;
                dataInvStatus.LastUpdateDate = DateTime.Now;
                dataInvStatus.RecordBy = "LineID";
                dataInvStatus.ReqMoney = invoicehd[i].Suminv;
                await digitalclaimContext.IclaimInvoiceStatus.AddAsync(dataInvStatus);

                var dataInvStatusState = new IclaimInvoiceStatusState();
                dataInvStatusState.IdInvhd = idInvhd + i + 1;
                dataInvStatusState.AccNo = iclaimApproval.AccNo;
                dataInvStatusState.VictimNo = iclaimApproval.VictimNo;
                dataInvStatusState.ReqNo = lastAppNo + 1;
                dataInvStatusState.StateNo = 1;
                dataInvStatusState.OldStatus = null;
                dataInvStatusState.NewStatus = 1;
                dataInvStatusState.InsertDate = DateTime.Now;
                dataInvStatusState.RecordBy = "LineID";
                await digitalclaimContext.IclaimInvoiceStatusState.AddAsync(dataInvStatusState);

            }

            await rvpofficeContext.SaveChangesAsync();
            await digitalclaimContext.SaveChangesAsync();

            return result;
        }
        public async Task<string> UpdateAsync(string accNo, int victimNo, int appNo, UpdateBankViewModel bankModel, UpdateInvoiceViewModel[] invModel)
        {
            var hosDocumentReceive = await digitalclaimContext.IclaimBankAccount.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == appNo).FirstOrDefaultAsync();
            hosDocumentReceive.AccountName = bankModel.accountName;
            hosDocumentReceive.AccountNo = bankModel.accountNumber;
            hosDocumentReceive.BankId = bankModel.bankId;

            for (int i = 0; i < invModel.Length; i++)
            {
                var invhd = await rvpofficeContext.Invoicehd.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.IdInvhd == invModel[i].billNo).FirstOrDefaultAsync();
                invhd.Hosid = invModel[i].selectHospitalId;
                invhd.Takendate = DateTime.ParseExact(invModel[i].hospitalized_date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                invhd.Takentime = invModel[i].hospitalized_time.Replace(":", ".");
                invhd.Dispensedate = DateTime.ParseExact(invModel[i].out_hospital_date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                invhd.Dispensetime = invModel[i].out_hospital_time.Replace(":", ".");
                TimeSpan calDate = (TimeSpan)(invhd.Dispensedate - invhd.Takendate);
                invhd.Daybed = (int?)calDate.TotalDays;
                invhd.Mainconsider = invModel[i].injuriId.ToString();
                invhd.Suminv = double.Parse(invModel[i].money);
                invhd.BookNo = invModel[i].bookNo;
                invhd.ReceiptNo = invModel[i].bill_no;
                invhd.VictimType = invModel[i].typePatient;
            }
            await digitalclaimContext.SaveChangesAsync();
            await UpdateApprovalStatusAsync(accNo, victimNo, appNo, "EditDocument");

            return null;
        }

        public async Task<ClaimViewModel> GetApprovalByAccNo(string accNo, int? victimNo)
        {
            var query = await rvpofficeContext.HosApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo).Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.Pt4id }).OrderByDescending(o => o.AppNo).Take(1).FirstOrDefaultAsync();
            //var query = await rvpofficeContext.HosApproval.Where(w => w.AccNo == accNo).Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.Pt4id, s.MedicineMoney, s.PlasticMoney, s.ServiceMoney, s.RoomMoney, s.VeihcleMoney, s.CureMoney, s.DeadMoney, s.HygieneMoney, s.CrippledMoney, s.SumMoney, s.BlindCrippled, s.UnHearCrippled, s.DeafCrippled, s.LostSexualCrippled, s.LostOrganCrippled, s.LostMindCrippled, s.CrippledPermanent, s.OtherCrippled, s.CrippledComment, s.PayMore }).OrderByDescending(o => o.AppNo).Take(1).FirstOrDefaultAsync();
            var claimVwModel = new ClaimViewModel();
            if (query != null)
            {

                claimVwModel.AccNo = query.AccNo;
                claimVwModel.VictimNo = query.VictimNo;
                claimVwModel.AppNo = query.AppNo;
                claimVwModel.ClaimNo = query.ClaimNo;
                claimVwModel.Pt4id = query.Pt4id;
                /*claimVwModel.MedicineMoney = query.MedicineMoney;
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
                claimVwModel.PayMore = query.PayMore;*/


            }

            return claimVwModel;
        }
        public async Task<List<ApprovalregisViewModel>> GetApprovalRegis(string accNo, int victimNo, int rightsType)
        {

            var approvalVwMdList = new List<ApprovalregisViewModel>();
            var query = await claimDataContext.Approvalregis.Where(w => w.AccNo == accNo && w.AccVictimNo == victimNo && w.Pt4 != null && w.Pt4 != "Compensate" && (w.ApStatus == "P" || w.ApStatus == "A")).Select(s => new { s.CrClaimno, s.VVictimno, s.ApRegno, s.ApRegdate, s.ApPaytype, s.ApMoney, s.AccNo, s.ApTotal, s.DailyReceiveno, s.Pt4, s.ApStatus }).ToListAsync();

            if (query != null)
            {
                foreach (var acc in query)
                {
                    var claimVwModel = new ClaimViewModel();
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
                    approvalVwModel.PtDetail = await GetHistoryInvoicedt(acc.Pt4);
                    approvalVwModel.ApStatus = acc.ApStatus;
                    approvalVwModel.Claim = await GetApprovalByClaimNo(acc.CrClaimno, acc.VVictimno, acc.ApRegno, rightsType);

                    approvalVwMdList.Add(approvalVwModel);

                }
            }
            return approvalVwMdList.OrderByDescending(o => o.ApRegdate).ToList();
        }

        public async Task<ClaimViewModel> GetApprovalByClaimNo(string claimNo, short victimNo, short regNo, int rightsType)
        {

            var claimVwModel = new ClaimViewModel();

            if (rightsType == 1)
            {
                var curemoneyQuery = await rvpofficeContext.HosApproval
                    .Where(w => w.ClaimNo == claimNo && w.VictimNoClaim == victimNo && w.RegNoClaim == regNo && w.PayMore != "Y" && w.PayMore != "B" && (w.CrippledMoney == 0 || w.CrippledMoney == null))
                    .Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.VictimNoClaim, s.RegNoClaim, s.Pt4id, s.MedicineMoney, s.PlasticMoney, s.ServiceMoney, s.RoomMoney, s.VeihcleMoney, s.CureMoney, s.DeadMoney, s.HygieneMoney, s.CrippledMoney, s.SumMoney, s.BlindCrippled, s.UnHearCrippled, s.DeafCrippled, s.LostSexualCrippled, s.LostOrganCrippled, s.LostMindCrippled, s.CrippledPermanent, s.OtherCrippled, s.CrippledComment, s.PayMore })
                    .FirstOrDefaultAsync();
                if (curemoneyQuery == null)
                {
                    return claimVwModel;
                }
                claimVwModel.AccNo = curemoneyQuery.AccNo;
                claimVwModel.VictimNo = curemoneyQuery.VictimNo;
                claimVwModel.AppNo = curemoneyQuery.AppNo;
                claimVwModel.ClaimNo = curemoneyQuery.ClaimNo;
                claimVwModel.VictimNoClaim = curemoneyQuery.VictimNoClaim;
                claimVwModel.RegNoClaim = curemoneyQuery.RegNoClaim;
                claimVwModel.Pt4id = curemoneyQuery.Pt4id;
                claimVwModel.MedicineMoney = curemoneyQuery.MedicineMoney;
                claimVwModel.PlasticMoney = curemoneyQuery.PlasticMoney;
                claimVwModel.ServiceMoney = curemoneyQuery.ServiceMoney;
                claimVwModel.RoomMoney = curemoneyQuery.RoomMoney;
                claimVwModel.VeihcleMoney = curemoneyQuery.VeihcleMoney;
                claimVwModel.CureMoney = curemoneyQuery.CureMoney;
                claimVwModel.DeadMoney = curemoneyQuery.DeadMoney;
                claimVwModel.HygieneMoney = curemoneyQuery.HygieneMoney;
                claimVwModel.CrippledMoney = curemoneyQuery.CrippledMoney;
                claimVwModel.SumMoney = curemoneyQuery.SumMoney;
                claimVwModel.BlindCrippled = curemoneyQuery.BlindCrippled;
                claimVwModel.UnHearCrippled = curemoneyQuery.UnHearCrippled;
                claimVwModel.DeafCrippled = curemoneyQuery.DeafCrippled;
                claimVwModel.LostSexualCrippled = curemoneyQuery.LostSexualCrippled;
                claimVwModel.LostOrganCrippled = curemoneyQuery.LostOrganCrippled;
                claimVwModel.LostMindCrippled = curemoneyQuery.LostMindCrippled;
                claimVwModel.CrippledPermanent = curemoneyQuery.CrippledPermanent;
                claimVwModel.OtherCrippled = curemoneyQuery.OtherCrippled;
                claimVwModel.CrippledComment = curemoneyQuery.CrippledComment;
                claimVwModel.PayMore = curemoneyQuery.PayMore;
                if (curemoneyQuery.CrippledMoney > 0 && curemoneyQuery.CureMoney == 0)
                {
                    claimVwModel.SumCrippledMoney = curemoneyQuery.CrippledMoney;
                }
                if (curemoneyQuery.MedicineMoney != null && curemoneyQuery.PlasticMoney != null)
                {
                    claimVwModel.SumCureMoney = curemoneyQuery.MedicineMoney + curemoneyQuery.PlasticMoney + curemoneyQuery.ServiceMoney + curemoneyQuery.RoomMoney + curemoneyQuery.VeihcleMoney;
                }
                else if (curemoneyQuery.CureMoney > 0)
                {
                    claimVwModel.SumCureMoney = curemoneyQuery.CureMoney;
                }
                return claimVwModel;
            }
            else if (rightsType == 2)
            {
                var crippledQuery = await rvpofficeContext.HosApproval
                    .Where(w => w.ClaimNo == claimNo && w.VictimNoClaim == victimNo && w.RegNoClaim == regNo && w.PayMore != "Y" && w.PayMore != "B" && w.CrippledMoney > 0)
                    .Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.VictimNoClaim, s.RegNoClaim, s.Pt4id, s.MedicineMoney, s.PlasticMoney, s.ServiceMoney, s.RoomMoney, s.VeihcleMoney, s.CureMoney, s.DeadMoney, s.HygieneMoney, s.CrippledMoney, s.SumMoney, s.BlindCrippled, s.UnHearCrippled, s.DeafCrippled, s.LostSexualCrippled, s.LostOrganCrippled, s.LostMindCrippled, s.CrippledPermanent, s.OtherCrippled, s.CrippledComment, s.PayMore })
                    .FirstOrDefaultAsync();
                if (crippledQuery == null)
                {
                    return claimVwModel;
                }
                claimVwModel.AccNo = crippledQuery.AccNo;
                claimVwModel.VictimNo = crippledQuery.VictimNo;
                claimVwModel.AppNo = crippledQuery.AppNo;
                claimVwModel.ClaimNo = crippledQuery.ClaimNo;
                claimVwModel.VictimNoClaim = crippledQuery.VictimNoClaim;
                claimVwModel.RegNoClaim = crippledQuery.RegNoClaim;
                claimVwModel.Pt4id = crippledQuery.Pt4id;
                claimVwModel.MedicineMoney = crippledQuery.MedicineMoney;
                claimVwModel.PlasticMoney = crippledQuery.PlasticMoney;
                claimVwModel.ServiceMoney = crippledQuery.ServiceMoney;
                claimVwModel.RoomMoney = crippledQuery.RoomMoney;
                claimVwModel.VeihcleMoney = crippledQuery.VeihcleMoney;
                claimVwModel.CureMoney = crippledQuery.CureMoney;
                claimVwModel.DeadMoney = crippledQuery.DeadMoney;
                claimVwModel.HygieneMoney = crippledQuery.HygieneMoney;
                claimVwModel.CrippledMoney = crippledQuery.CrippledMoney;
                claimVwModel.SumMoney = crippledQuery.SumMoney;
                claimVwModel.BlindCrippled = crippledQuery.BlindCrippled;
                claimVwModel.UnHearCrippled = crippledQuery.UnHearCrippled;
                claimVwModel.DeafCrippled = crippledQuery.DeafCrippled;
                claimVwModel.LostSexualCrippled = crippledQuery.LostSexualCrippled;
                claimVwModel.LostOrganCrippled = crippledQuery.LostOrganCrippled;
                claimVwModel.LostMindCrippled = crippledQuery.LostMindCrippled;
                claimVwModel.CrippledPermanent = crippledQuery.CrippledPermanent;
                claimVwModel.OtherCrippled = crippledQuery.OtherCrippled;
                claimVwModel.CrippledComment = crippledQuery.CrippledComment;
                claimVwModel.PayMore = crippledQuery.PayMore;
                if (crippledQuery.CrippledMoney > 0 && crippledQuery.CureMoney == 0)
                {
                    claimVwModel.SumCrippledMoney = crippledQuery.CrippledMoney;
                }
                if (crippledQuery.MedicineMoney != null && crippledQuery.PlasticMoney != null)
                {
                    claimVwModel.SumCureMoney = crippledQuery.MedicineMoney + crippledQuery.PlasticMoney + crippledQuery.ServiceMoney + crippledQuery.RoomMoney + crippledQuery.VeihcleMoney;
                }
                else if (crippledQuery.CureMoney > 0)
                {
                    claimVwModel.SumCureMoney = crippledQuery.CureMoney;
                }
                return claimVwModel;
            }
            else if (rightsType == 5)
            {
                var query = await rvpofficeContext.HosApproval
                    .Join(rvpofficeContext.Invoicehd,
                    hosApp => new { accNo = hosApp.AccNo, victimNo = hosApp.VictimNo, appNo = hosApp.AppNo },
                    invhd => new { accNo = invhd.AccNo, victimNo = (int)invhd.VictimNo, appNo = (int)invhd.AppNo },
                    (hosApp, invhd) => new { hosAppJoinInvhd = hosApp, invhd = invhd })
                    .Join(rvpofficeContext.Invoicedt,
                    invhd => invhd.invhd.IdInvdt, invdt => invdt.IdInvdt, (invhd, invdt) => new { invhdJoinInvdt = invhd, invdt = invdt })
                    .Where(w => w.invhdJoinInvdt.hosAppJoinInvhd.ClaimNo == claimNo && w.invhdJoinInvdt.hosAppJoinInvhd.VictimNoClaim == victimNo &&
                    w.invhdJoinInvdt.hosAppJoinInvhd.RegNoClaim == regNo && w.invhdJoinInvdt.hosAppJoinInvhd.PayMore != "Y" && w.invhdJoinInvdt.hosAppJoinInvhd.PayMore != "B"

                    )
                    .Select(s => new { s.invhdJoinInvdt.hosAppJoinInvhd.Pt4id })
                    .ToListAsync();
            }

            return claimVwModel;
        }

        public async Task<object> GetHistoryInvoicedt(string pt4)
        {
            //var query = await rvpofficeContext.Invoicehd
            //    .Join(rvpofficeContext.HosApproval,
            //    invhd => new { accNo = invhd.AccNo, victimNo = (int)invhd.VictimNo, appNo = (int)invhd.AppNo },
            //    hosApp => new { accNo = hosApp.AccNo, victimNo = hosApp.VictimNo, appNo = hosApp.AppNo },
            //    (invhd, hosApp) => new { hosAppJoinInvhd = hosApp, invhd = invhd })
            //    .Where(w => w.hosAppJoinInvhd.Pt4id == pt4).Select(s => s.invhd.IdInvhd).ToListAsync();
            var q = await rvpofficeContext.HosApproval.Where(w => w.Pt4id == pt4).Select(s => new { s.AccNo, s.VictimNo, s.AppNo }).FirstOrDefaultAsync();
            var query = await rvpofficeContext.Invoicehd.Where(w => w.AccNo == q.AccNo && w.VictimNo == q.VictimNo && w.AppNo == q.AppNo).Select(s => s.IdInvdt).ToListAsync();
            var qq = await rvpofficeContext.Invoicedt.Where(w => query.Contains(w.IdInvdt)).ToListAsync();
            //var query1 = await rvpofficeContext.HosApproval
            //    .Join(rvpofficeContext.Invoicehd,
            //    hosApp => new { accNo = hosApp.AccNo, victimNo = hosApp.VictimNo, appNo = hosApp.AppNo },
            //    invhd => new { accNo = invhd.AccNo, victimNo = (int)invhd.VictimNo, appNo = (int)invhd.AppNo },
            //    (hosApp, invhd) => new { hosAppJoinInvhd = hosApp, invhd = invhd })
            //    .Where(w => w.hosAppJoinInvhd.Pt4id == pt4).ToListAsync();

            return qq;
        }
        public async Task<List<IcliamApprovalViewModel>> GetIClaimApprovalAsync(string accNo, int victimNo)
        {
            var query = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo).Select(s => new { s.AccNo, s.VictimNo, s.ReqNo, s.InsertDate, s.SumReqMoney, s.Status }).ToListAsync();
            var vwIclaimAppList = new List<IcliamApprovalViewModel>();
            if (query == null)
            {
                return vwIclaimAppList;
            }
            foreach (var iclaimApp in query)
            {
                var vwIclaimApp = new IcliamApprovalViewModel();
                vwIclaimApp.AccNo = iclaimApp.AccNo;
                vwIclaimApp.StringAccNo = iclaimApp.AccNo.Replace("/", "-");
                vwIclaimApp.AppNo = iclaimApp.ReqNo;
                vwIclaimApp.InsertDate = iclaimApp.InsertDate;
                vwIclaimApp.SumMoney = iclaimApp.SumReqMoney;
                vwIclaimApp.Status = iclaimApp.Status;
                vwIclaimApp.StringRegDate = iclaimApp.InsertDate.Value.ToString("dd/MM/yyyy HH:mm");
                //var queryStatus = await digitalclaimContext.IclaimApproval
                //    .Join(digitalclaimContext.IclaimApprovalStatus, appss => appss.Status, apps => apps.StatusId, (appss, apps) => new { appStatusStateJoinAppStatus = appss, statusName = apps.StatusNameIclaim })
                //    .Where(w => w.appStatusStateJoinAppStatus.AccNo == hosApp.AccNo && w.appStatusStateJoinAppStatus.AppNo == hosApp.AppNo && w.appStatusStateJoinAppStatus.VictimNo == hosApp.VictimNo).Select(s => new { s.statusName, s.appStatusStateJoinAppStatus.Status }).FirstOrDefaultAsync();

                vwIclaimApp.AppStatus = await GetApprovalStatus(iclaimApp.AccNo, iclaimApp.VictimNo, iclaimApp.ReqNo);
                vwIclaimApp.AppStatusName = vwIclaimApp.AppStatus.Where(w => w.Active == true).Select(s => new { s.StatusName, s.StatusId }).OrderByDescending(o => o.StatusId).Select(s => s.StatusName).FirstOrDefault();
                vwIclaimAppList.Add(vwIclaimApp);
            }

            return vwIclaimAppList;
        }

        private async Task<List<ApprovalStatusViewModel>> GetApprovalStatus(string accNo, int victimNo, int appNo)
        {
            var query = await digitalclaimContext.IclaimApprovalStatus.ToListAsync();
            var appStatus = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == appNo).FirstOrDefaultAsync();
            var appStatusState = await digitalclaimContext.IclaimApprovalState.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == appNo).ToListAsync();

            var appStatusVwModelList = new List<ApprovalStatusViewModel>();
            for (int i = 0; i < query.Count; i++)
            {
                var appStatusVwModel = new ApprovalStatusViewModel();
                appStatusVwModel.StatusId = query[i].StatusId;
                appStatusVwModel.StatusName = query[i].StatusNameIclaim;
                appStatusVwModel.StatusDate = appStatusState.Where(w => w.NewStatus == query[i].StatusId).Select(s => s.InsertDate.Value.Date.ToString("dd/MM/yyyy")).FirstOrDefault();
                appStatusVwModel.StatusTime = appStatusState.Where(w => w.NewStatus == query[i].StatusId).Select(s => s.InsertDate.Value.ToString("HH:mm")).FirstOrDefault();
                if (query[i].StatusId <= appStatus.Status)
                {
                    appStatusVwModel.Active = true;
                }
                else
                {
                    appStatusVwModel.Active = false;
                }
                if (!string.IsNullOrEmpty(appStatusVwModel.StatusName))
                {
                    appStatusVwModelList.Add(appStatusVwModel);
                }

            }
            //foreach (var status in query)
            //{
            //    var appStatusVwModel = new ApprovalStatusViewModel();
            //    appStatusVwModel.StatusId = status.StatusId;
            //    appStatusVwModel.StatusName = status.StatusNameIclaim;
            //    appStatusVwModel.StatusDate = appStatusState.Where(w => w.NewStatus == status.StatusId).Select(s => s.InsertDate.Value.Date.ToString("dd/MM/yyyy")).FirstOrDefault();
            //    appStatusVwModel.StatusTime = appStatusState.Where(w => w.NewStatus == status.StatusId).Select(s => s.InsertDate.Value.ToString("HH:mm")).FirstOrDefault();
            //    if (status.StatusId <= appStatus.Status)
            //    {
            //        appStatusVwModel.Active = true;
            //    }
            //    else
            //    {
            //        appStatusVwModel.Active = false;
            //    }
            //    appStatusVwModelList.Add(appStatusVwModel);

            //}
            return appStatusVwModelList;
        }

        public async Task<InputBankViewModel> GetIClaimBankAccountAsync(string accNo, int victimNo, int reqNo)
        {
            var query = await digitalclaimContext.IclaimBankAccount.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo).Select(s => new { s.AccountNo, s.AccountName, s.BankId, s.ReqNo }).FirstOrDefaultAsync();
            //var bankName = await rvpofficeContext.BankNames.Where(w => w.BankCode != null && w.BankCode == query.BankId).Select(s =>  s.Name).FirstOrDefaultAsync();
            var inputBankViewModel = new InputBankViewModel();
            if (query == null)
            {
                return inputBankViewModel;
            }
            
            //inputBankViewModel.accountNumber = query.AccountNo;
            //inputBankViewModel.accountName = query.AccountName;
            //inputBankViewModel.accountBankName = bankName;
            //inputBankViewModel.bankId = query.BankId;
            inputBankViewModel.accountNumber = query.AccountNo;
            inputBankViewModel.accountName = query.AccountName;
            inputBankViewModel.accountBankName = query.BankId;
            inputBankViewModel.appNo = query.ReqNo;
            
            return inputBankViewModel;
        }
        public async Task<List<InputBankViewModel>> GetLastIClaimBankAccountAsync(string accNo, int victimNo)
        {
            var query = await digitalclaimContext.IclaimBankAccount.Where(w => w.AccNo == accNo && w.VictimNo == victimNo).Select(s => new { s.AccountNo, s.AccountName, s.BankId, s.ReqNo }).OrderByDescending(o => o.ReqNo).FirstOrDefaultAsync();
            var inputBankViewModelsList = new List<InputBankViewModel>();
            if (query == null)
            {
                return inputBankViewModelsList;
            }
            var inputBankViewModel = new InputBankViewModel();
            inputBankViewModel.accountNumber = query.AccountNo;
            inputBankViewModel.accountName = query.AccountName;
            inputBankViewModel.accountBankName = query.BankId;
            inputBankViewModel.appNo = query.ReqNo;
            inputBankViewModelsList.Add(inputBankViewModel);
            return inputBankViewModelsList;
        }




        public async Task<double?> GetRightsBalance(string accNo, int? victimNo, string rightsType)
        {
            double? rightsBalance = 0;
            if (rightsType == "CureRights")
            {
                double? cureRights = 30000;
                var claimNo = await claimDataContext.Approvalregis.Where(w => w.AccNo == accNo && w.AccVictimNo == victimNo && (w.ApStatus == "P" || w.ApStatus == "A")).Select(s => s.CrClaimno).FirstOrDefaultAsync();
                if (claimNo == null)
                {
                    return cureRights;
                }
                var cureMoneyQuery = await rvpofficeContext.HosApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ClaimNo == claimNo && w.PayMore != "Y" && w.PayMore != "B" && (w.CrippledMoney == 0 || w.CrippledMoney == null))
                .Select(s => s.SumMoney)
                .ToListAsync();
                if (cureMoneyQuery == null)
                {
                    return cureRights;
                }
                rightsBalance = cureRights - cureMoneyQuery.Sum();
            }
            else if (rightsType == "CrippledRights")
            {
                double? crippledRights = 35000;
                var claimNo = await claimDataContext.Approvalregis.Where(w => w.AccNo == accNo && w.AccVictimNo == victimNo && w.ApStatus == "P").Select(s => s.CrClaimno).FirstOrDefaultAsync();
                if (claimNo == null)
                {
                    return crippledRights;
                }
                var crippledMoneyQuery = await rvpofficeContext.HosApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ClaimNo == claimNo && w.PayMore != "Y" && w.PayMore != "B" && w.CrippledMoney > 0)
                .Select(s => s.CrippledMoney)
                .ToListAsync();
                if (crippledMoneyQuery == null)
                {
                    return crippledRights;
                }
                rightsBalance = crippledRights - crippledMoneyQuery.Sum();
            }


            return rightsBalance;
        }

        public async Task<string> UpdateApprovalStatusAsync(string accNo, int victimNo, int appNo, string status)
        {
            if (status == "ConfirmMoney")
            {
                var approvalStatus = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == appNo).FirstOrDefaultAsync();
                if (approvalStatus.Status != 4)
                {
                    return "Error";
                }
                approvalStatus.LastUpdate = DateTime.Now;
                approvalStatus.Status = 5;

                var lastStatusState = await digitalclaimContext.IclaimApprovalState.Where(w => w.AccNo == accNo && w.ReqNo == appNo && w.VictimNo == victimNo).Select(s => new { s.StateNo, s.NewStatus }).OrderByDescending(o => o.StateNo).FirstOrDefaultAsync();
                var dataIclaimApprovalState = new IclaimApprovalState();
                dataIclaimApprovalState.AccNo = accNo;
                dataIclaimApprovalState.ReqNo = appNo;
                dataIclaimApprovalState.VictimNo = victimNo;
                dataIclaimApprovalState.StateNo = lastStatusState.StateNo + 1;
                dataIclaimApprovalState.OldStatus = lastStatusState.NewStatus;
                dataIclaimApprovalState.NewStatus = 5;
                dataIclaimApprovalState.InsertDate = DateTime.Now;
                await digitalclaimContext.IclaimApprovalState.AddAsync(dataIclaimApprovalState);

                var invoiceStatus = await digitalclaimContext.IclaimInvoiceStatus.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == appNo).ToListAsync();
                for(int i = 0; i < invoiceStatus.Count; i++)
                {
                    invoiceStatus[i].Status = 5;
                    invoiceStatus[i].InvConfirmMoneyComment = null;
                    invoiceStatus[i].InvDocComment = null;
                    invoiceStatus[i].LastUpdateDate = DateTime.Now;

                    var lastInvoiceStatusState = await digitalclaimContext.IclaimInvoiceStatusState
                        .Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == appNo && w.IdInvhd == invoiceStatus[i].IdInvhd)
                        .Select(s => new { s.StateNo, s.NewStatus }).OrderByDescending(o => o.StateNo).FirstOrDefaultAsync();
                    var dataIclaimInvoiceStatusState = new IclaimInvoiceStatusState();
                    dataIclaimInvoiceStatusState.IdInvhd = invoiceStatus[i].IdInvhd;
                    dataIclaimInvoiceStatusState.AccNo = invoiceStatus[i].AccNo;
                    dataIclaimInvoiceStatusState.VictimNo = invoiceStatus[i].VictimNo;
                    dataIclaimInvoiceStatusState.ReqNo = invoiceStatus[i].ReqNo;
                    dataIclaimInvoiceStatusState.StateNo = lastInvoiceStatusState.StateNo + 1;
                    dataIclaimInvoiceStatusState.OldStatus = lastInvoiceStatusState.NewStatus;
                    dataIclaimInvoiceStatusState.NewStatus = 5;
                    dataIclaimInvoiceStatusState.InsertDate = DateTime.Now;
                    dataIclaimInvoiceStatusState.InvDocComment = null;
                    dataIclaimInvoiceStatusState.RecordBy = invoiceStatus[i].RecordBy;
                    await digitalclaimContext.IclaimInvoiceStatusState.AddAsync(dataIclaimInvoiceStatusState);
                }

                
                

                await digitalclaimContext.SaveChangesAsync();
                return "Success";
            }
            else if (status == "EditDocument")
            {
                var approvalStatus = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == appNo).FirstOrDefaultAsync();
                if (approvalStatus.Status != 2)
                {
                    return "Error";
                }
                approvalStatus.LastUpdate = DateTime.Now;
                approvalStatus.Status = 1;

                var lastStatusState = await digitalclaimContext.IclaimApprovalState.Where(w => w.AccNo == accNo && w.ReqNo == appNo && w.VictimNo == victimNo).Select(s => new { s.StateNo, s.NewStatus }).OrderByDescending(o => o.StateNo).FirstOrDefaultAsync();
                var dataHosApprovalStatusState = new IclaimApprovalState();
                dataHosApprovalStatusState.AccNo = accNo;
                dataHosApprovalStatusState.ReqNo = appNo;
                dataHosApprovalStatusState.VictimNo = victimNo;
                dataHosApprovalStatusState.StateNo = lastStatusState.StateNo + 1;
                dataHosApprovalStatusState.OldStatus = lastStatusState.NewStatus;
                dataHosApprovalStatusState.NewStatus = 1;
                dataHosApprovalStatusState.InsertDate = DateTime.Now;
                await digitalclaimContext.IclaimApprovalState.AddAsync(dataHosApprovalStatusState);

                await digitalclaimContext.SaveChangesAsync();
            }
            return "Error";
        }

        public async Task<List<InvoicehdViewModel>> GetInvoicehdAsync(string accNo, int victimNo, int reqNo, int status)
        {
            var idInvhdList = new List<long>();
            if (status == 0)
            {
                idInvhdList = await digitalclaimContext.IclaimInvoiceStatus.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo).Select(s => s.IdInvhd).ToListAsync();
                var query = await rvpofficeContext.Invoicehd.Where(w => idInvhdList.Contains(w.IdInvhd)).ToListAsync();

                List<InvoicehdViewModel> invList = new List<InvoicehdViewModel>();
                for (int i = 0; i < query.Count(); i++)
                {
                    InvoicehdViewModel inv = new InvoicehdViewModel();
                    var refId = query[i].IdInvhd + "|" + accNo + "|" + victimNo;
                    inv.IdInvhd = query[i].IdInvhd;
                    inv.Mainconsider = query[i].Mainconsider;
                    inv.VictimType = query[i].VictimType;
                    inv.Hosid = query[i].Hosid;
                    inv.BookNo = query[i].BookNo;
                    inv.ReceiptNo = query[i].ReceiptNo;
                    inv.Suminv = query[i].Suminv;
                    inv.Takentime = query[i].Takentime;
                    inv.Dispensetime = query[i].Dispensetime;
                    inv.WoundedName = await rvpofficeContext.Mcwounded.Where(w => w.WoundedId == Convert.ToInt16(query[i].Mainconsider)).Select(s => s.WoundedName).FirstOrDefaultAsync();
                    inv.HospitalName = await rvpofficeContext.Hospital.Where(w => w.Hospitalid == query[i].Hosid).Select(s => s.Hospitalname).FirstOrDefaultAsync();
                    inv.StringTakendate = (query[i].Takendate != null) ? query[i].Takendate.Value.Date.ToString("dd/MM/yyyy") : null;
                    inv.StringDispensedate = (query[i].Dispensedate != null) ? query[i].Dispensedate.Value.Date.ToString("dd/MM/yyyy") : null;
                    var edocDetail = await rvpSystemContext.EDocDetail.Where(w => w.SystemId == "02" && w.TemplateId == "03" && w.DocumentId == "01" && w.RefId.StartsWith(refId)).Select(s => new { s.Paths, s.CreateDate, s.RunningNo }).OrderByDescending(o => o.RunningNo).FirstOrDefaultAsync();
                    inv.Base64Image = await attachmentService.DownloadFileFromECM(edocDetail.Paths);
                    //invNotPass.InvoiceStatus = qurey[i].invoiceStatus.ToString();
                    invList.Add(inv);
                }
                return invList;
            }
            else {
                idInvhdList = await digitalclaimContext.IclaimInvoiceStatus.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo && w.Status == status).Select(s => s.IdInvhd).ToListAsync();
                var query = await rvpofficeContext.Invoicehd.Where(w => idInvhdList.Contains(w.IdInvhd)).ToListAsync();

                List<InvoicehdViewModel> invNotPassList = new List<InvoicehdViewModel>();
                for (int i = 0; i < query.Count(); i++)
                {
                    InvoicehdViewModel invNotPass = new InvoicehdViewModel();
                    invNotPass.AccNo = query[i].AccNo;
                    invNotPass.VictimNo = query[i].VictimNo;
                    invNotPass.AppNo = query[i].AppNo;
                    invNotPass.IdInvhd = query[i].IdInvhd;
                    invNotPass.Mainconsider = query[i].Mainconsider;
                    invNotPass.VictimType = query[i].VictimType;
                    invNotPass.Hosid = query[i].Hosid;
                    invNotPass.BookNo = query[i].BookNo;
                    invNotPass.ReceiptNo = query[i].ReceiptNo;
                    invNotPass.Suminv = query[i].Suminv;
                    invNotPass.Takendate = query[i].Takendate;
                    invNotPass.Takentime = query[i].Takentime;
                    invNotPass.Dispensedate = query[i].Dispensedate;
                    invNotPass.Dispensetime = query[i].Dispensetime;
                    //invNotPass.InvoiceStatus = qurey[i].invoiceStatus.ToString();
                    invNotPassList.Add(invNotPass);
                }
                return invNotPassList;
            }

            

           
        }

        public async Task<IclaimCheckDocuments> GetDocumentCheck(string accNo, int victimNo, int reqNo)
        {
            return await digitalclaimContext.IclaimCheckDocuments.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo).FirstOrDefaultAsync();
        }

        public async Task<ConfirmMoneyViewModel> GetDataForConfirmMoney(string accNo, int victimNo, int reqNo)
        {
            

            var iclaimApproval = await digitalclaimContext.IclaimApproval
                .Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo && w.Status == 4).Select(s => new { s.SumReqMoney, s.SumPayMoney, s.InsertDate }).FirstOrDefaultAsync();
            var car = await rvpofficeContext.HosCarAccident.Where(w => w.AccNo == accNo).Select(s => new { s.FoundCarLicense, s.FoundChassisNo, s.FoundPolicyNo }).FirstOrDefaultAsync();
            var idInvhdList = await digitalclaimContext.IclaimInvoiceStatus.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo).Select(s => new { s.IdInvhd, s.ReqMoney, s.PayMoney }).ToListAsync();
           
            CarViewModel carViewModel = new CarViewModel();
            carViewModel.FoundCarLicense = car.FoundCarLicense;
            carViewModel.FoundChassisNo = car.FoundChassisNo;
            carViewModel.FoundPolicyNo = car.FoundPolicyNo;

            ConfirmMoneyViewModel confirmMoney = new ConfirmMoneyViewModel();
            confirmMoney.ReqNo = reqNo;
            confirmMoney.ReqDate = iclaimApproval.InsertDate.Value.Date.ToString("dd/MM/yyyy");
            confirmMoney.ReqTime = iclaimApproval.InsertDate.Value.ToString("HH:mm");
            confirmMoney.SumReqMoney = (iclaimApproval.SumReqMoney != null) ? (double)iclaimApproval.SumReqMoney : 0;
            confirmMoney.SumPayMoney = (iclaimApproval.SumPayMoney != null) ? (double)iclaimApproval.SumPayMoney : 0;
            confirmMoney.Car = carViewModel;
            confirmMoney.BankAccount = await GetIClaimBankAccountAsync(accNo, victimNo, reqNo);
            

            List<InvoiceStatusViewModel> invStatusList = new List<InvoiceStatusViewModel>();
            for (int i = 0; i < idInvhdList.Count; i++)
            {
                var invhd = await rvpofficeContext.Invoicehd.Where(w => w.IdInvhd == idInvhdList[i].IdInvhd).Select(s => new { s.BookNo, s.ReceiptNo }).FirstOrDefaultAsync();
                InvoiceStatusViewModel invStatus = new InvoiceStatusViewModel();
                var refId = idInvhdList[i].IdInvhd + "|" + accNo + "|" + victimNo;
                invStatus.IdInvhd = idInvhdList[i].IdInvhd;
                invStatus.BookNo = invhd.BookNo;
                invStatus.ReceiptNo = invhd.ReceiptNo;
                invStatus.ReqMoney = idInvhdList[i].ReqMoney;
                invStatus.PayMoney = idInvhdList[i].PayMoney;               
                var edocDetail = await rvpSystemContext.EDocDetail.Where(w => w.SystemId == "02" && w.TemplateId == "03" && w.DocumentId == "01" && w.RefId.StartsWith(refId)).Select(s => new { s.Paths, s.CreateDate, s.RunningNo }).OrderByDescending(o => o.RunningNo).FirstOrDefaultAsync();
                invStatus.Base64Image = await attachmentService.DownloadFileFromECM(edocDetail.Paths);
                invStatusList.Add(invStatus);
            }
            confirmMoney.InvoiceList = invStatusList;

            return confirmMoney;
        }

        public async Task<ApprovalDetailViewModel> GetApprovalDetail(string accNo, int victimNo, int reqNo, string userIdCard)
        {
            var victimViewModel = new VictimtViewModel();
            var accVic = await (from hs in rvpofficeContext.HosVicTimAccident
                                join ch in rvpofficeContext.Changwat on hs.Province equals ch.Changwatshortname into result1
                                from hschi in result1.DefaultIfEmpty()
                                join a in rvpofficeContext.Amphur on new { key1 = hs.Province, key2 = hs.District } equals new { key1 = a.Changwatshortname, key2 = a.Amphurid } into result2
                                from hsai in result2.DefaultIfEmpty()
                                join t in rvpofficeContext.Tumbol on new { key1 = hs.Province, key2 = hs.District, key3 = hs.Tumbol } equals new { key1 = t.Changwatshortname, key2 = t.Amphurid, key3 = t.Tumbolid } into result3
                                from hsti in result3.DefaultIfEmpty()
                                where (hs.AccNo == accNo && hs.DrvSocNo == userIdCard) || (hs.AccNo == accNo && hs.VictimNo == victimNo)
                                select new
                                {
                                    AccNo = hs.AccNo,
                                    VictimNo = hs.VictimNo,
                                    DrvSocNo = hs.DrvSocNo,
                                    Fname = hs.Fname,
                                    Lname = hs.Lname,
                                    Prefix = hs.Prefix,
                                    Age = hs.Age,
                                    Sex = hs.Sex,
                                    TelNo = hs.TelNo,
                                    HomeId = hs.HomeId,
                                    Moo = hs.Moo,
                                    Soi = hs.Soi,
                                    Road = hs.Road,
                                    Zipcode = hsti.Zipcode,
                                    TumbolId = hs.Tumbol,
                                    TumbolName = hsti.Tumbolname,
                                    AmphurId = hs.District,
                                    AmphurName = hsai.Amphurname,
                                    ChangwatShort = hs.Province,
                                    ChangwatName = hschi.Changwatname,
                                }).FirstOrDefaultAsync();
            var kyc = await ipolicyContext.DirectPolicyKyc.Where(w => w.IdcardNo == userIdCard).FirstOrDefaultAsync();
            var address = await (from t in rvpofficeContext.Tumbol
                                 join a in rvpofficeContext.Amphur on new { key1 = t.Amphurid, key2 = t.Provinceid } equals new { key1 = a.Amphurid, key2 = a.Provinceid } into result1
                                 from ta in result1.DefaultIfEmpty()
                                 join ch in rvpofficeContext.Changwat on t.Changwatshortname equals ch.Changwatshortname into result2
                                 from tch in result2.DefaultIfEmpty()
                                 where (t.Tumbolid == kyc.HomeTumbolId && t.Amphurid == kyc.HomeCityId && t.Provinceid == kyc.HomeProvinceId)
                                 select new
                                 {
                                     Zipcode = t.Zipcode,
                                     TumbolId = t.Tumbolid,
                                     TumbolName = t.Tumbolname,
                                     AmphurId = ta.Amphurid,
                                     AmphurName = ta.Amphurname,
                                     ChangwatShort = tch.Changwatshortname,
                                     ChangwatName = tch.Changwatname
                                 }).FirstOrDefaultAsync();
            if (accVic == null) return null;
            victimViewModel.IdCardNo = kyc.IdcardNo;
            victimViewModel.Fname = kyc.Fname;
            victimViewModel.Lname = kyc.Lname;
            victimViewModel.Prefix = kyc.Prefix;
            victimViewModel.Age = accVic.Age;
            victimViewModel.Sex = accVic.Sex;
            victimViewModel.TelNo = kyc.MobileNo;
            victimViewModel.HomeId = kyc.HomeHouseNo;
            victimViewModel.Moo = kyc.HomeHmo;
            victimViewModel.Soi = kyc.HomeSoi;
            victimViewModel.Road = kyc.HomeRoad;
            victimViewModel.Tumbol = address.TumbolId;
            victimViewModel.TumbolName = address.TumbolName;
            victimViewModel.District = address.AmphurId;
            victimViewModel.DistrictName = address.AmphurName;
            victimViewModel.Province = address.ChangwatShort;
            victimViewModel.ProvinceName = address.ChangwatName;
            victimViewModel.Zipcode = address.Zipcode;
            victimViewModel.AccHomeId = accVic.HomeId;
            victimViewModel.AccMoo = accVic.Moo;
            victimViewModel.AccSoi = accVic.Soi;
            victimViewModel.AccRoad = accVic.Road;
            victimViewModel.AccTumbol = accVic.TumbolId;
            victimViewModel.AccTumbolName = accVic.TumbolName;
            victimViewModel.AccDistrict = accVic.AmphurId;
            victimViewModel.AccDistrictName = accVic.AmphurName;
            victimViewModel.AccProvince = accVic.ChangwatShort;
            victimViewModel.AccProvinceName = accVic.ChangwatName;
            victimViewModel.AccZipcode = accVic.Zipcode;

            //ข้อมูลรถ
            var car = await rvpofficeContext.HosCarAccident.Where(w => w.AccNo == accNo).Select(s => new { s.FoundCarLicense, s.FoundChassisNo, s.FoundPolicyNo }).FirstOrDefaultAsync();
            CarViewModel carViewModel = new CarViewModel();
            carViewModel.FoundCarLicense = car.FoundCarLicense;
            carViewModel.FoundChassisNo = car.FoundChassisNo;
            carViewModel.FoundPolicyNo = car.FoundPolicyNo;

            var iclaimApproval = await digitalclaimContext.IclaimApproval
                .Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo).Select(s => new { s.SumReqMoney, s.InsertDate }).FirstOrDefaultAsync();            

            ApprovalDetailViewModel approvalDetailViewModel = new ApprovalDetailViewModel();
            approvalDetailViewModel.ReqNo = reqNo;
            approvalDetailViewModel.ReqDate = iclaimApproval.InsertDate.Value.Date.ToString("dd/MM/yyyy");
            approvalDetailViewModel.ReqTime = iclaimApproval.InsertDate.Value.ToString("HH:mm");
            approvalDetailViewModel.SumReqMoney = (double)iclaimApproval.SumReqMoney;
            approvalDetailViewModel.Victim = victimViewModel;
            approvalDetailViewModel.Car = carViewModel;
            approvalDetailViewModel.Invoicehds = await GetInvoicehdAsync(accNo, victimNo, reqNo, 0);
            approvalDetailViewModel.BankAccount = await GetIClaimBankAccountAsync(accNo, victimNo, reqNo);
            approvalDetailViewModel.BankAccount.bankId = approvalDetailViewModel.BankAccount.accountBankName;
            approvalDetailViewModel.BankAccount.accountBankName = await rvpofficeContext.BankNames.Where(w => w.BankCode != null && w.BankCode == approvalDetailViewModel.BankAccount.bankId).Select(s => s.Name).FirstOrDefaultAsync();

            return approvalDetailViewModel;
        }

        public async  Task<ApprovalPDFViewModel> GetApprovalDataForGenPDF(string accNo, int victimNo, int reqNo)
        {
            var query = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo).Select(s => new { s.SumReqMoney, s.InsertDate }).FirstOrDefaultAsync();
            ApprovalPDFViewModel approvalPDF = new ApprovalPDFViewModel();
            approvalPDF.SumReqMoney = (double)query.SumReqMoney;
            approvalPDF.ReqDate = query.InsertDate.Value.ToString("dd/MM/yyyy");
            approvalPDF.TimeDate = query.InsertDate.Value.ToString("HH:mm");
            return approvalPDF;
        }
    }
}
