using DataAccess.EFCore.DigitalClaimModels;
using DataAccess.EFCore.RvpOfficeModels;
using Microsoft.EntityFrameworkCore;
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
using Microsoft.AspNetCore.Http;
using Services.Models;

namespace Services
{
    public interface IApprovalService
    {
        Task<List<ApprovalregisDetail>> GetApprovalRegis(string accNo, int victimNo, int rightsType);
        Task<Claim> GetApprovalByClaimNo(string claimNo, short victimNo, short regNo, int rightsType);
        Task<Claim> GetApprovalByAccNo(string accNo, int? victimNo);
        Task<List<ResultAddApproval>> AddAsync(DataAccess.EFCore.DigitalClaimModels.IclaimApproval iclaimApproval, InputBank inputBank, Victim victim, Invoicehd[] invoicehd, string userLineId);
        Task<string> UpdateAsync(string accNo, int victimNo, int reqNo, string userIdLine, UpdateBank bankModel, UpdateInvoice[] invModel);
        Task<List<IclaimRequests>> GetIClaimApprovalAsync(string accNo, int victimNo);
        Task<InputBank> GetIClaimBankAccountAsync(string accNo, int victimNo, int reqNo);
        Task<InputBank> GetLastIClaimBankAccountAsync(string accNo, int victimNo);
        Task<double?> GetRightsBalance(string accNo, int? victimNo, string rightsType);
        Task<string> UpdateApprovalStatusAsync(string accNo, int victimNo, int reqNo, string status, bool isHasInvCancel);
        Task<List<InvoiceHeader>> GetInvoicehdAsync(string accNo, int victimNo, int reqNo, int status);
        Task<IclaimCheckDocuments> GetDocumentCheck(string accNo, int victimNo, int appNo);
        Task<ConfirmMoney> GetDataForConfirmMoney(string accNo, int victimNo, int reqNo);
        //Task<string> GeneratePT(string accN);
        Task<object> GetHistoryInvoicedt(string accNo, int victimNo, int appNo);
        Task<ApprovalDetail> GetApprovalDetail(string accNo, int victimNo, int reqNo, string userIdCard);
        Task<ApprovalPDF> GetApprovalDataForGenPDF(string accNo, int victimNo, int reqNo);
        Task<CheckDuplicateInvoice[]> CheckDuplicateInvoice(CheckDuplicateInvoice[] invoicehds);
        Task<string> CanselApprovalAsync(string accNo, int victimNo, int reqNo, string userIdLine);
    }


    public class ApprovalService : IApprovalService
    {
        private readonly DigitalclaimContext digitalclaimContext;
        private readonly RvpofficeContext rvpofficeContext;
        private readonly ClaimDataContext claimDataContext;
        private readonly IAttachmentService attachmentService;
        private readonly RvpSystemContext rvpSystemContext;
        private readonly IpolicyContext ipolicyContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly DigitalclaimContextProcedures storeProc;

        public ApprovalService(DigitalclaimContext digitalclaimContext, RvpofficeContext rvpofficeContext, ClaimDataContext claimDataContext, IAttachmentService attachmentService, RvpSystemContext rvpSystemContext, IpolicyContext ipolicyContext, IHttpContextAccessor httpContextAccessor , DigitalclaimContextProcedures storeProc)
        {
            this.digitalclaimContext = digitalclaimContext;
            this.rvpofficeContext = rvpofficeContext;
            this.claimDataContext = claimDataContext;
            this.attachmentService = attachmentService;
            this.rvpSystemContext = rvpSystemContext;
            this.ipolicyContext = ipolicyContext;
            this.httpContextAccessor = httpContextAccessor;
            this.storeProc = storeProc;


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
        public async Task<List<ResultAddApproval>> AddAsync(DataAccess.EFCore.DigitalClaimModels.IclaimApproval iclaimApproval, InputBank inputBank, Victim victim, Invoicehd[] invoicehd, string userLineId)
        {
            var lastAppNo = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == iclaimApproval.AccNo && w.VictimNo == iclaimApproval.VictimNo).Select(s => s.ReqNo).OrderByDescending(o => o).FirstOrDefaultAsync();
            if (lastAppNo == null)
            {
                lastAppNo = 0;
            }
            var dataIcliamApproval = new DataAccess.EFCore.DigitalClaimModels.IclaimApproval();
            dataIcliamApproval.AccNo = iclaimApproval.AccNo;
            dataIcliamApproval.VictimNo = iclaimApproval.VictimNo;
            dataIcliamApproval.ReqNo = lastAppNo + 1;
            dataIcliamApproval.BranchId = iclaimApproval.BranchId.Trim();
            dataIcliamApproval.Status = 1;
            dataIcliamApproval.SumReqMoney = iclaimApproval.SumReqMoney;
            dataIcliamApproval.CureMoney = iclaimApproval.SumReqMoney;
            dataIcliamApproval.InsertDate = DateTime.Now;
            dataIcliamApproval.LastUpdate = DateTime.Now;
            dataIcliamApproval.LineId = userLineId;
            dataIcliamApproval.RefCodeOtp = iclaimApproval.RefCodeOtp;
            if (iclaimApproval.IsEverAuthorize == true)
            {
                dataIcliamApproval.IsEverAuthorize = iclaimApproval.IsEverAuthorize;
                dataIcliamApproval.EverAuthorizeMoney = iclaimApproval.EverAuthorizeMoney;
                dataIcliamApproval.EverAuthorizeHosId = iclaimApproval.EverAuthorizeHosId;
            }
            else if (iclaimApproval.IsEverAuthorize == false)
            {
                dataIcliamApproval.IsEverAuthorize = iclaimApproval.IsEverAuthorize;
                dataIcliamApproval.EverAuthorizeMoney = null;
                dataIcliamApproval.EverAuthorizeHosId = null;
            }
            await digitalclaimContext.IclaimApproval.AddAsync(dataIcliamApproval);  

            var dataBankAccount = new IclaimBankAccount();
            dataBankAccount.AccNo = iclaimApproval.AccNo;
            dataBankAccount.VictimNo = (short)iclaimApproval.VictimNo;
            dataBankAccount.ReqNo = (short)(lastAppNo + 1);
            dataBankAccount.AccountNo = inputBank.accountNumber;
            dataBankAccount.AccountName = inputBank.accountName;
            dataBankAccount.BankId = inputBank.bankId;
            dataBankAccount.InsertDate = DateTime.Now;
            dataBankAccount.RecordBy = userLineId;
            await digitalclaimContext.IclaimBankAccount.AddAsync(dataBankAccount);

            var dataIclaimApprovalStatusState = new IclaimApprovalState();
            dataIclaimApprovalStatusState.AccNo = iclaimApproval.AccNo;
            dataIclaimApprovalStatusState.ReqNo = lastAppNo + 1;
            dataIclaimApprovalStatusState.VictimNo = iclaimApproval.VictimNo;
            dataIclaimApprovalStatusState.StateNo = 1;
            dataIclaimApprovalStatusState.OldStatus = null;
            dataIclaimApprovalStatusState.NewStatus = 1;
            dataIclaimApprovalStatusState.InsertDate = DateTime.Now;
            dataIclaimApprovalStatusState.RecordBy = userLineId;
            await digitalclaimContext.IclaimApprovalState.AddAsync(dataIclaimApprovalStatusState);

            var idInvhd = await rvpofficeContext.Invoicehd.Select(s => s.IdInvhd).OrderByDescending(o => o).FirstOrDefaultAsync();
            List<ResultAddApproval> result = new List<ResultAddApproval>();
            for (int i = 0; i < invoicehd.Length; i++)
            {
                var dataResult = new ResultAddApproval();
                dataResult.IdInvhd = idInvhd + i + 1;
                dataResult.AccNo = iclaimApproval.AccNo;
                dataResult.VictimNo = iclaimApproval.VictimNo;
                dataResult.IclaimAppNo = lastAppNo + 1;
                result.Add(dataResult);

                var dataInvoicehd = new Invoicehd();
                dataInvoicehd.IdInvhd = idInvhd + i + 1;
                dataInvoicehd.AccNo = iclaimApproval.AccNo;
                dataInvoicehd.VictimNo = iclaimApproval.VictimNo;
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

                await rvpofficeContext.Invoicehd.AddAsync(dataInvoicehd);

                var dataInvStatus = new IclaimInvoiceStatus();
                dataInvStatus.IdInvhd = idInvhd + i + 1;
                dataInvStatus.AccNo = iclaimApproval.AccNo;
                dataInvStatus.VictimNo = iclaimApproval.VictimNo;
                dataInvStatus.ReqNo = lastAppNo + 1;
                dataInvStatus.Status = 1;
                dataInvStatus.InsertDate = DateTime.Now;
                dataInvStatus.LastUpdateDate = DateTime.Now;
                dataInvStatus.RecordBy = userLineId;
                dataInvStatus.ReqMoney = invoicehd[i].Suminv;
                await digitalclaimContext.IclaimInvoiceStatus.AddAsync(dataInvStatus);

                //var dataInvStatusState = new IclaimInvoiceStatusState();
                //dataInvStatusState.IdInvhd = idInvhd + i + 1;
                //dataInvStatusState.AccNo = iclaimApproval.AccNo;
                //dataInvStatusState.VictimNo = iclaimApproval.VictimNo;
                //dataInvStatusState.ReqNo = lastAppNo + 1;
                //dataInvStatusState.StateNo = 1;
                //dataInvStatusState.OldStatus = null;
                //dataInvStatusState.NewStatus = 1;
                //dataInvStatusState.InsertDate = DateTime.Now;
                //dataInvStatusState.RecordBy = userLineId;
                //await digitalclaimContext.IclaimInvoiceStatusState.AddAsync(dataInvStatusState);

            }

            await rvpofficeContext.SaveChangesAsync();
            await digitalclaimContext.SaveChangesAsync();

            return result;
        }
        public async Task<string> UpdateAsync(string accNo, int victimNo, int reqNo, string userIdLine, UpdateBank bankModel, UpdateInvoice[] invModel)
        {
            var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var iclaimBankAccount = await digitalclaimContext.IclaimBankAccount.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo).FirstOrDefaultAsync();
            var maxRunNoBankAccountLog = await digitalclaimContext.IclaimBankAccountLog.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo).Select(s => s.RunNo).OrderByDescending(o => o).FirstOrDefaultAsync();
            var iclaimBankAccountLog = new IclaimBankAccountLog();
            iclaimBankAccountLog.AccNo = iclaimBankAccount.AccNo;
            iclaimBankAccountLog.VictimNo = iclaimBankAccount.VictimNo;
            iclaimBankAccountLog.ReqNo = iclaimBankAccount.ReqNo;
            iclaimBankAccountLog.RunNo = maxRunNoBankAccountLog + 1;
            iclaimBankAccountLog.AccountNo = iclaimBankAccount.AccountNo;
            iclaimBankAccountLog.AccountName = iclaimBankAccount.AccountName;
            iclaimBankAccountLog.BankId = iclaimBankAccount.BankId;
            iclaimBankAccountLog.InsertDate = DateTime.Now;
            iclaimBankAccountLog.Ip = ip;
            await digitalclaimContext.IclaimBankAccountLog.AddAsync(iclaimBankAccountLog); // เก็บ log บัญชีธนาคาร

            iclaimBankAccount.AccountName = bankModel.accountName;
            iclaimBankAccount.AccountNo = bankModel.accountNumber;
            iclaimBankAccount.BankId = bankModel.bankId;

            bool isHasInvCancel = false;
            var lastInvStatusState = await digitalclaimContext.IclaimInvoiceStatusLog.Where(w => w.AccNo == accNo && w.ReqNo == reqNo && w.VictimNo == victimNo).Select(s => new { s.StateNo, s.NewStatus }).OrderByDescending(o => o.StateNo).FirstOrDefaultAsync();
            for (int i = 0; i < invModel.Length; i++)
            {
                var invhd = await rvpofficeContext.Invoicehd.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.IdInvhd == invModel[i].billNo).FirstOrDefaultAsync();
                await storeProc.sp_InsertInvoiceLogFromIClaimAsync("InvoiceHd",invhd.IdInvhd, invhd.IdInvdt, invhd.AccNo, invhd.VictimNo, userIdLine, ip, "แก้ไขใบแจ้งหนี้ iclaim");               



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

                var iclaimInvStatus = await digitalclaimContext.IclaimInvoiceStatus.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.IdInvhd == invModel[i].billNo).FirstOrDefaultAsync();
                var iclaimInvoiceStatusLog = new IclaimInvoiceStatusLog();
                if (invModel[i].isCancel) //ยกเลิกใบเสร็จ
                {                  
                    iclaimInvoiceStatusLog.IdInvhd = invModel[i].billNo;
                    iclaimInvoiceStatusLog.AccNo = accNo;
                    iclaimInvoiceStatusLog.VictimNo = victimNo;
                    iclaimInvoiceStatusLog.ReqNo = reqNo;
                    iclaimInvoiceStatusLog.StateNo = lastInvStatusState.StateNo + 1;
                    iclaimInvoiceStatusLog.OldStatus = lastInvStatusState.NewStatus;
                    iclaimInvoiceStatusLog.NewStatus = 0;
                    iclaimInvoiceStatusLog.InsertDate = DateTime.Now;
                    iclaimInvoiceStatusLog.InvDocComment = iclaimInvStatus.InvDocComment;
                    iclaimInvoiceStatusLog.RecordBy = userIdLine;
                    iclaimInvoiceStatusLog.InvCommentTypeId = iclaimInvStatus.InvCommentTypeId;
                    iclaimInvoiceStatusLog.Ip = ip;
                    await digitalclaimContext.IclaimInvoiceStatusLog.AddAsync(iclaimInvoiceStatusLog);

                    isHasInvCancel = true;
                    iclaimInvStatus.Status = 0;
                    iclaimInvStatus.InvDocComment = null;
                    iclaimInvStatus.InvCommentTypeId = null;
                    iclaimInvStatus.LastUpdateDate = DateTime.Now;

                }
                else
                {
                    iclaimInvoiceStatusLog.IdInvhd = invModel[i].billNo;
                    iclaimInvoiceStatusLog.AccNo = accNo;
                    iclaimInvoiceStatusLog.VictimNo = victimNo;
                    iclaimInvoiceStatusLog.ReqNo = reqNo;
                    iclaimInvoiceStatusLog.StateNo = lastInvStatusState.StateNo + 1;
                    iclaimInvoiceStatusLog.OldStatus = lastInvStatusState.NewStatus;
                    iclaimInvoiceStatusLog.NewStatus = 1;
                    iclaimInvoiceStatusLog.InsertDate = DateTime.Now;
                    iclaimInvoiceStatusLog.InvDocComment = iclaimInvStatus.InvDocComment;
                    iclaimInvoiceStatusLog.RecordBy = userIdLine;
                    iclaimInvoiceStatusLog.InvCommentTypeId = iclaimInvStatus.InvCommentTypeId;
                    iclaimInvoiceStatusLog.Ip  = ip;
                    await digitalclaimContext.IclaimInvoiceStatusLog.AddAsync(iclaimInvoiceStatusLog);

                    iclaimInvStatus.Status = 1;
                    iclaimInvStatus.InvDocComment = null;
                    iclaimInvStatus.InvCommentTypeId = null;
                    iclaimInvStatus.LastUpdateDate = DateTime.Now;
                }
                
            }
            await rvpofficeContext.SaveChangesAsync();
            await digitalclaimContext.SaveChangesAsync();
            await UpdateApprovalStatusAsync(accNo, victimNo, reqNo, "EditDocument", isHasInvCancel);

            return null;
        }

        public async Task<Claim> GetApprovalByAccNo(string accNo, int? victimNo)
        {
            var query = await rvpofficeContext.HosApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo).Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.Pt4id }).OrderByDescending(o => o.AppNo).Take(1).FirstOrDefaultAsync();
            var claimVwModel = new Claim();
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
        public async Task<List<ApprovalregisDetail>> GetApprovalRegis(string accNo, int victimNo, int rightsType)
        {

            var approvalRegisList = await claimDataContext.Approvalregis.Where(w => w.AccNo == accNo && w.AccVictimNo == victimNo && w.Pt4 != null && w.Pt4 != "Compensate" /*&& (w.ApStatus == "P" || w.ApStatus == "A")*/)
                .Select(s => new ApprovalregisDetail {
                    CrClaimno = s.CrClaimno,
                    VVictimno = s.VVictimno,
                    ApRegno = s.ApRegno,
                    ApRegdate = s.ApRegdate,
                    StringApRegdate = s.ApRegdate.Value.ToString("dd/MM/yyyy เวลา HH:mm น."),
                    ApPaytype = s.ApPaytype,
                    ApMoney = s.ApMoney,
                    AccNo = s.AccNo,
                    AccAppNo = (int)s.AccAppNo,
                    AccVictimNo = (int)s.AccVictimNo,
                    ApTotal = s.ApTotal,
                    Pt4 = s.Pt4,
                    StringPt4 = s.Pt4.ToString().Replace("/", "-"),
                    ApStatus = s.ApStatus 
                }).ToListAsync();           
            return approvalRegisList.OrderByDescending(o => o.ApRegdate).ToList();
        }

        public async Task<Claim> GetApprovalByClaimNo(string claimNo, short victimNo, short regNo, int rightsType)
        {

            var claimVwModel = new Claim();

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

        public async Task<object> GetHistoryInvoicedt(string accNo, int victimNo, int appNo)
        {
            var invdt = await rvpofficeContext.Invoicehd.Join(rvpofficeContext.Hospital, invhd => invhd.Hosid, hos => hos.Hospitalid, (invhd, hos) => new { Invhd = invhd, HospitalName = hos.Hospitalname })
                .Where(w => w.Invhd.AccNo == accNo && w.Invhd.VictimNo == victimNo && w.Invhd.AppNo == appNo)
                .Select(s => new { s.Invhd.IdInvdt, s.Invhd.Hostype, s.HospitalName, s.Invhd.Takendate, s.Invhd.Takentime, s.Invhd.Suminv }).ToListAsync();

            if (invdt.Count > 0)
            {
                var invhdHosTypeP = invdt.Where(w => w.Hostype != "G").Select(s => new { s.IdInvdt, s.HospitalName, s.Takendate, s.Takentime, s.Suminv }).ToList();
                var invhdHosTypeG = invdt.Where(w => w.Hostype == "G").Select(s => new { s.IdInvdt, s.HospitalName, s.Takendate, s.Takentime, s.Suminv }).ToList();
                if (invhdHosTypeP.Count > 0 && invhdHosTypeG.Count > 0)
                {
                    var invdtItemsP = await rvpofficeContext.Invoicedt
                    .Join(rvpofficeContext.Particulars, invdt => invdt.Treatid, par => par.Id, (invdt, par) => new { invdtID = invdt.IdInvdt, listNo = invdt.Listno, treatName = par.Name, money = invdt.Price })
                    .Where(w => invhdHosTypeP.Select(s => s.IdInvdt).Contains(w.invdtID)).ToListAsync();
                    var invdtItemsG = await rvpofficeContext.Invoicedt
                    .Join(rvpofficeContext.Particulars3, invdt => invdt.Treatid, par => par.Id, (invdt, par) => new { invdtID = invdt.IdInvdt, listNo = invdt.Listno, treatName = par.Name, money = invdt.Price })
                    .Where(w => invhdHosTypeG.Select(s => s.IdInvdt).Contains(w.invdtID)).ToListAsync();
                    var mergeInvdtList = invdtItemsP.Concat(invdtItemsG).ToList();
                    var mergeHospital = invhdHosTypeP.Concat(invhdHosTypeG).ToList();

                    return mergeInvdtList.GroupBy(item => item.invdtID,
                    (key, group) => new {
                        invdtId = key,
                        hospital = mergeHospital.Where(w => w.IdInvdt == key).Select(s => s.HospitalName).FirstOrDefault(),
                        takenDate = mergeHospital.Where(w => w.IdInvdt == key).Select(s => s.Takendate.Value.ToString("dd/MM/yyyy")).FirstOrDefault(),
                        takenTime = mergeHospital.Where(w => w.IdInvdt == key).Select(s => s.Takentime).FirstOrDefault(),
                        sumMoney = mergeHospital.Where(w => w.IdInvdt == key).Select(s => s.Suminv).FirstOrDefault(),
                        Items = group.Select(s => new { s.listNo, s.treatName, s.money }).OrderBy(o => o.listNo).ToList()
                    }).ToList();
                }
                else if (invhdHosTypeP.Count > 0 && invhdHosTypeG.Count == 0)
                {
                    var invdtItemsP = await rvpofficeContext.Invoicedt
                    .Join(rvpofficeContext.Particulars, invdt => invdt.Treatid, par => par.Id, (invdt, par) => new { invdtID = invdt.IdInvdt, listNo = invdt.Listno, treatName = par.Name, money = invdt.Price })
                    .Where(w => invhdHosTypeP.Select(s => s.IdInvdt).Contains(w.invdtID)).ToListAsync();

                    return invdtItemsP.GroupBy(item => item.invdtID,
                    (key, group) => new {
                        invdtId = key,
                        hospital = invhdHosTypeP.Where(w => w.IdInvdt == key).Select(s => s.HospitalName).FirstOrDefault(),
                        takenDate = invhdHosTypeP.Where(w => w.IdInvdt == key).Select(s => s.Takendate.Value.ToString("dd/MM/yyyy")).FirstOrDefault(),
                        takenTime = invhdHosTypeP.Where(w => w.IdInvdt == key).Select(s => s.Takentime).FirstOrDefault(),
                        sumMoney = invhdHosTypeP.Where(w => w.IdInvdt == key).Select(s => s.Suminv).FirstOrDefault(),
                        Items = group.Select(s => new { s.listNo, s.treatName, s.money }).OrderBy(o => o.listNo).ToList()
                    }).ToList();
                }
                else if (invhdHosTypeG.Count > 0 && invhdHosTypeP.Count == 0)
                {
                    var invdtItemsG = await rvpofficeContext.Invoicedt
                    .Join(rvpofficeContext.Particulars3, invdt => invdt.Treatid, par => par.Id, (invdt, par) => new { invdtID = invdt.IdInvdt, listNo = invdt.Listno, treatName = par.Name, money = invdt.Price })
                    .Where(w => invhdHosTypeG.Select(s => s.IdInvdt).Contains(w.invdtID)).ToListAsync();

                    return invdtItemsG.GroupBy(item => item.invdtID,
                    (key, group) => new {
                        invdtId = key, hospital = invhdHosTypeG.Where(w => w.IdInvdt == key).Select(s => s.HospitalName).FirstOrDefault(),
                        takenDate = invhdHosTypeG.Where(w => w.IdInvdt == key).Select(s => s.Takendate.Value.ToString("dd/MM/yyyy")).FirstOrDefault(),
                        takenTime = invhdHosTypeG.Where(w => w.IdInvdt == key).Select(s => s.Takentime).FirstOrDefault(),
                        sumMoney = invhdHosTypeG.Where(w => w.IdInvdt == key).Select(s => s.Suminv).FirstOrDefault(),
                        Items = group.Select(s => new { s.listNo, s.treatName, s.money }).OrderBy(o => o.listNo).ToList()
                    }).ToList();
                }
            }

            return null;
        }
        public async Task<List<IclaimRequests>> GetIClaimApprovalAsync(string accNo, int victimNo)
        {
            var iclaimApps = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo).Select(s => new { s.AccNo, s.VictimNo, s.ReqNo, s.InsertDate, s.SumReqMoney, s.Status }).ToListAsync();
            var reqNoIclaimInvList = await digitalclaimContext.IclaimInvoiceStatus.Where(w => w.AccNo == accNo && w.VictimNo == victimNo).Select(s => s.ReqNo).ToListAsync();
            var groupIclaimInvByReqNo = reqNoIclaimInvList.ToLookup(g => g).Select(s => new { ReqNo = s.Key, invhdCount = s.Count() });
            var iclaimAppList = new List<IclaimRequests>();
            if (iclaimApps == null)
            {
                return iclaimAppList;
            }
            for(int i = 0;i< iclaimApps.Count;i++)
            {
                var iclaimData = new IclaimRequests();
                iclaimData.AccNo = iclaimApps[i].AccNo;
                iclaimData.StringAccNo = iclaimApps[i].AccNo.Replace("/", "-");
                iclaimData.AppNo = iclaimApps[i].ReqNo;
                iclaimData.InsertDate = iclaimApps[i].InsertDate;
                iclaimData.SumMoney = iclaimApps[i].SumReqMoney;
                iclaimData.Status = iclaimApps[i].Status;
                iclaimData.StringRegDate = iclaimApps[i].InsertDate.Value.ToString("dd/MM/yyyy HH:mm");
                iclaimData.AppStatus = await GetApprovalStatus(iclaimApps[i].AccNo, iclaimApps[i].VictimNo, iclaimApps[i].ReqNo);
                iclaimData.AppStatusName = iclaimData.AppStatus.Where(w => w.Active == true).Select(s => new { s.StatusName, s.StatusId }).OrderByDescending(o => o.StatusId).Select(s => s.StatusName).FirstOrDefault();
                iclaimData.IclaimInvCount = groupIclaimInvByReqNo.Where(w => w.ReqNo == iclaimApps[i].ReqNo).Select(s => s.invhdCount).FirstOrDefault();
                iclaimAppList.Add(iclaimData);
            }

            return iclaimAppList.OrderByDescending(o => o.AppNo).ToList();
        }

        private async Task<List<ApprovalStatus>> GetApprovalStatus(string accNo, int victimNo, int appNo)
        {
            var query = await digitalclaimContext.IclaimMasterTypes.Where(w => w.ParentTypeId == 50 && w.IsActive).ToListAsync();
            var appStatus = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == appNo).FirstOrDefaultAsync();
            var appStatusState = await digitalclaimContext.IclaimApprovalState.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == appNo).ToListAsync();

            var appStatusVwModelList = new List<ApprovalStatus>();
            for (int i = 0; i < query.Count; i++)
            {
                var appStatusVwModel = new ApprovalStatus();
                appStatusVwModel.StatusId = query[i].TypeId;
                appStatusVwModel.StatusName = query[i].TypeNameIclaim;
                appStatusVwModel.StatusDate = appStatusState.Where(w => w.NewStatus == query[i].TypeId).Select(s => s.InsertDate.Value.Date.ToString("dd/MM/yyyy")).FirstOrDefault();
                appStatusVwModel.StatusTime = appStatusState.Where(w => w.NewStatus == query[i].TypeId).Select(s => s.InsertDate.Value.ToString("HH:mm")).FirstOrDefault();
                if (query[i].TypeId <= appStatus.Status)
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

            return appStatusVwModelList;
        }

        public async Task<InputBank> GetIClaimBankAccountAsync(string accNo, int victimNo, int reqNo)
        {
            return await digitalclaimContext.IclaimBankAccount.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo)
                .Select(s => new InputBank { accountNumber = s.AccountNo, accountName = s.AccountName, accountBankName = s.BankId, appNo = s.ReqNo }).FirstOrDefaultAsync();
        }
        public async Task<InputBank> GetLastIClaimBankAccountAsync(string accNo, int victimNo)
        {
            return await digitalclaimContext.IclaimBankAccount.Where(w => w.AccNo == accNo && w.VictimNo == victimNo)
                .Select(s => new InputBank
                {
                    accountNumber = s.AccountNo,
                    accountName = s.AccountName,
                    accountBankName = s.BankId,
                    appNo = s.ReqNo

                }).OrderByDescending(o => o.appNo).FirstOrDefaultAsync();
        }




        public async Task<double?> GetRightsBalance(string accNo, int? victimNo, string rightsType)
        {
            double? rightsBalance = 0;
            if (rightsType == "CureRights")
            {
                double? cureRights = 30000;
                var claimNo = await claimDataContext.Approvalregis.Where(w => w.AccNo == accNo && w.AccVictimNo == victimNo /*&& (w.ApStatus == "P" || w.ApStatus == "A")*/)
                    .Select(s => s.CrClaimno).FirstOrDefaultAsync();
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
                var claimNo = await claimDataContext.Approvalregis.Where(w => w.AccNo == accNo && w.AccVictimNo == victimNo /*&& w.ApStatus == "P"*/).Select(s => s.CrClaimno).FirstOrDefaultAsync();
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

        public async Task<string> UpdateApprovalStatusAsync(string accNo, int victimNo, int reqNo, string status, bool isHasInvCancel)
        {
            var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            if (status == "ConfirmMoney")
            {
                var approvalStatus = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo).FirstOrDefaultAsync();
                if (approvalStatus.Status != 4)
                {
                    return "Error";
                }
                approvalStatus.LastUpdate = DateTime.Now;
                approvalStatus.Status = 5;

                var lastStatusState = await digitalclaimContext.IclaimApprovalState.Where(w => w.AccNo == accNo && w.ReqNo == reqNo && w.VictimNo == victimNo).Select(s => new { s.StateNo, s.NewStatus }).OrderByDescending(o => o.StateNo).FirstOrDefaultAsync();
                var dataIclaimApprovalState = new IclaimApprovalState();
                dataIclaimApprovalState.AccNo = accNo;
                dataIclaimApprovalState.ReqNo = reqNo;
                dataIclaimApprovalState.VictimNo = victimNo;
                dataIclaimApprovalState.StateNo = lastStatusState.StateNo + 1;
                dataIclaimApprovalState.OldStatus = lastStatusState.NewStatus;
                dataIclaimApprovalState.NewStatus = 5;
                dataIclaimApprovalState.InsertDate = DateTime.Now;
                await digitalclaimContext.IclaimApprovalState.AddAsync(dataIclaimApprovalState);

                var invoiceStatus = await digitalclaimContext.IclaimInvoiceStatus.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo).ToListAsync();

                var invoicehd = await rvpofficeContext.Invoicehd.Where(w => invoiceStatus.Select(s => s.IdInvhd).Contains(w.IdInvhd)).ToListAsync();
                var invoicedtVerify = await digitalclaimContext.IclaimInvoiceDtVerify.Where(w => invoicehd.Select(s => s.IdInvdt).Contains(w.IdInvdt)).ToListAsync();
                for (int i = 0; i < invoicehd.Count; i++)
                {
                    await storeProc.sp_InsertInvoiceLogFromIClaimAsync("InvoiceHd", invoicehd[i].IdInvhd, invoicehd[i].IdInvdt, accNo, victimNo, invoiceStatus[0].RecordBy, ip, "ผสภ ยืนยันจำนวนเงิน iClaim");
                    var invoicedt = await rvpofficeContext.Invoicedt.Where(w => w.IdInvdt == invoicehd[i].IdInvdt).ToListAsync();
                    await storeProc.sp_InsertInvoiceLogFromIClaimAsync("InvoiceDt", invoicehd[i].IdInvhd, invoicehd[i].IdInvdt, accNo, victimNo, invoiceStatus[0].RecordBy, ip, null);
                    for (int j = 0; j < invoicedt.Count; j++)
                    {
                        invoicedt[j].Price = invoicedtVerify.Where(w => w.IdInvdt == invoicedt[j].IdInvdt && w.Listno == invoicedt[j].Listno && w.Treatid == invoicedt[j].Treatid).Select(s => s.PaidPrice).FirstOrDefault();
                    }
                    invoicehd[i].Suminv = invoicedt.Select(s => s.Price).Sum();
                }
                for (int i = 0; i < invoiceStatus.Count; i++)
                {
                    var lastInvoiceStatusState = await digitalclaimContext.IclaimInvoiceStatusLog
                        .Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo && w.IdInvhd == invoiceStatus[i].IdInvhd)
                        .Select(s => new { s.StateNo, s.NewStatus }).OrderByDescending(o => o.StateNo).FirstOrDefaultAsync();
                    var dataIclaimInvoiceStatusLog = new IclaimInvoiceStatusLog();
                    dataIclaimInvoiceStatusLog.IdInvhd = invoiceStatus[i].IdInvhd;
                    dataIclaimInvoiceStatusLog.AccNo = invoiceStatus[i].AccNo;
                    dataIclaimInvoiceStatusLog.VictimNo = invoiceStatus[i].VictimNo;
                    dataIclaimInvoiceStatusLog.ReqNo = invoiceStatus[i].ReqNo;
                    dataIclaimInvoiceStatusLog.StateNo = lastInvoiceStatusState.StateNo + 1;
                    dataIclaimInvoiceStatusLog.OldStatus = lastInvoiceStatusState.NewStatus;
                    dataIclaimInvoiceStatusLog.NewStatus = 5;
                    dataIclaimInvoiceStatusLog.InsertDate = DateTime.Now;
                    dataIclaimInvoiceStatusLog.InvDocComment = invoiceStatus[i].InvDocComment;
                    dataIclaimInvoiceStatusLog.RecordBy = invoiceStatus[i].RecordBy;
                    dataIclaimInvoiceStatusLog.InvCommentTypeId = invoiceStatus[i].InvCommentTypeId;
                    dataIclaimInvoiceStatusLog.Ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                    await digitalclaimContext.IclaimInvoiceStatusLog.AddAsync(dataIclaimInvoiceStatusLog);

                    invoiceStatus[i].Status = 5;
                    invoiceStatus[i].InvConfirmMoneyComment = null;
                    invoiceStatus[i].InvDocComment = null;
                    invoiceStatus[i].LastUpdateDate = DateTime.Now;                   
                }



                await rvpofficeContext.SaveChangesAsync();
                await digitalclaimContext.SaveChangesAsync();
                return "Success";
            }
            else if (status == "EditDocument")
            {
                var approvalStatus = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo).FirstOrDefaultAsync();
                if (approvalStatus.Status != 2)
                {
                    return "Error";
                }

                if (isHasInvCancel)
                {
                    var newSumMoneyReq = await digitalclaimContext.IclaimInvoiceStatus.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo && w.Status != 0).Select(s => s.ReqMoney).SumAsync();
                    approvalStatus.LastUpdate = DateTime.Now;
                    approvalStatus.Status = 1;
                    approvalStatus.SumReqMoney = newSumMoneyReq;
                    approvalStatus.CureMoney = newSumMoneyReq;

                    var lastStatusState = await digitalclaimContext.IclaimApprovalState.Where(w => w.AccNo == accNo && w.ReqNo == reqNo && w.VictimNo == victimNo).Select(s => new { s.StateNo, s.NewStatus }).OrderByDescending(o => o.StateNo).FirstOrDefaultAsync();
                    var dataHosApprovalStatusState = new IclaimApprovalState();
                    dataHosApprovalStatusState.AccNo = accNo;
                    dataHosApprovalStatusState.ReqNo = reqNo;
                    dataHosApprovalStatusState.VictimNo = victimNo;
                    dataHosApprovalStatusState.StateNo = lastStatusState.StateNo + 1;
                    dataHosApprovalStatusState.OldStatus = lastStatusState.NewStatus;
                    dataHosApprovalStatusState.NewStatus = 1;
                    dataHosApprovalStatusState.InsertDate = DateTime.Now;
                    await digitalclaimContext.IclaimApprovalState.AddAsync(dataHosApprovalStatusState);
                }
                else
                {
                    approvalStatus.LastUpdate = DateTime.Now;
                    approvalStatus.Status = 1;

                    var lastStatusState = await digitalclaimContext.IclaimApprovalState.Where(w => w.AccNo == accNo && w.ReqNo == reqNo && w.VictimNo == victimNo).Select(s => new { s.StateNo, s.NewStatus }).OrderByDescending(o => o.StateNo).FirstOrDefaultAsync();
                    var dataHosApprovalStatusState = new IclaimApprovalState();
                    dataHosApprovalStatusState.AccNo = accNo;
                    dataHosApprovalStatusState.ReqNo = reqNo;
                    dataHosApprovalStatusState.VictimNo = victimNo;
                    dataHosApprovalStatusState.StateNo = lastStatusState.StateNo + 1;
                    dataHosApprovalStatusState.OldStatus = lastStatusState.NewStatus;
                    dataHosApprovalStatusState.NewStatus = 1;
                    dataHosApprovalStatusState.InsertDate = DateTime.Now;
                    await digitalclaimContext.IclaimApprovalState.AddAsync(dataHosApprovalStatusState);
                }


                await digitalclaimContext.SaveChangesAsync();
                return "Success";
            }
            else if (status == "CanselApproval"){
                var approvalStatus = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo).FirstOrDefaultAsync();
                if (approvalStatus.Status != 2)
                {
                    return "Error";
                }
                approvalStatus.LastUpdate = DateTime.Now;
                approvalStatus.Status = 0;

                var lastStatusState = await digitalclaimContext.IclaimApprovalState.Where(w => w.AccNo == accNo && w.ReqNo == reqNo && w.VictimNo == victimNo).Select(s => new { s.StateNo, s.NewStatus }).OrderByDescending(o => o.StateNo).FirstOrDefaultAsync();
                var dataHosApprovalStatusState = new IclaimApprovalState();
                dataHosApprovalStatusState.AccNo = accNo;
                dataHosApprovalStatusState.ReqNo = reqNo;
                dataHosApprovalStatusState.VictimNo = victimNo;
                dataHosApprovalStatusState.StateNo = lastStatusState.StateNo + 1;
                dataHosApprovalStatusState.OldStatus = lastStatusState.NewStatus;
                dataHosApprovalStatusState.NewStatus = 0;
                dataHosApprovalStatusState.InsertDate = DateTime.Now;
                await digitalclaimContext.IclaimApprovalState.AddAsync(dataHosApprovalStatusState);

                await digitalclaimContext.SaveChangesAsync();
                return "Success";
            }
            return "Error";
        }

        public async Task<List<InvoiceHeader>> GetInvoicehdAsync(string accNo, int victimNo, int reqNo, int status)
        {
            var idInvhdList = new List<long>();
            if (status == 0)
            {
                var invStatusList = await digitalclaimContext.IclaimInvoiceStatus.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo && w.Status != status)
                    .Select(s => new { s.IdInvhd, s.ReqMoney }).ToListAsync();
                var query = await rvpofficeContext.Invoicehd.Where(w => invStatusList.Select(s => s.IdInvhd).Contains(w.IdInvhd)).ToListAsync();

                List<InvoiceHeader> invList = new List<InvoiceHeader>();
                for (int i = 0; i < query.Count(); i++)
                {
                    InvoiceHeader inv = new InvoiceHeader();
                    var refId = query[i].IdInvhd + "|" + accNo + "|" + victimNo;
                    inv.IdInvhd = query[i].IdInvhd;
                    inv.Mainconsider = query[i].Mainconsider;
                    inv.VictimType = query[i].VictimType;
                    inv.Hosid = query[i].Hosid;
                    inv.BookNo = query[i].BookNo;
                    inv.ReceiptNo = query[i].ReceiptNo;
                    inv.Suminv = invStatusList.Where(w => w.IdInvhd == query[i].IdInvhd).Select(s => s.ReqMoney).FirstOrDefault();
                    inv.Takentime = query[i].Takentime;
                    inv.Dispensetime = query[i].Dispensetime;
                    inv.WoundedName = await rvpofficeContext.Mcwounded.Where(w => w.WoundedId == Convert.ToInt16(query[i].Mainconsider)).Select(s => s.WoundedName).FirstOrDefaultAsync();
                    inv.HospitalName = await rvpofficeContext.Hospital.Where(w => w.Hospitalid == query[i].Hosid).Select(s => s.Hospitalname).FirstOrDefaultAsync();
                    inv.StringTakendate = (query[i].Takendate != null) ? query[i].Takendate.Value.Date.ToString("dd/MM/yyyy") : null;
                    inv.StringDispensedate = (query[i].Dispensedate != null) ? query[i].Dispensedate.Value.Date.ToString("dd/MM/yyyy") : null;
                    var maxRunningNo = await rvpSystemContext.EDocDetail
                        .Where(w => w.SystemId == "02" && w.TemplateId == "03" && w.DocumentId == "01" && w.RefId.StartsWith(refId))
                        .MaxAsync(m => m.RunningNo);
                    var edocDetail = await rvpSystemContext.EDocDetail.Where(w => w.SystemId == "02" && w.TemplateId == "03" && w.DocumentId == "01" && w.RefId.StartsWith(refId) && w.RunningNo == maxRunningNo)
                        .Select(s => new EDocDetail() { Paths = s.Paths, CreateDate = s.CreateDate, RunningNo = s.RunningNo }).OrderByDescending(o => o.RunningNo).ToListAsync();
                    List<string> base64Image = new List<string>();
                    for (int j = 0; j < edocDetail.Count();j++)
                    {
                        var file = await attachmentService.DownloadFileFromECM(edocDetail[j].Paths);
                        base64Image.Add(file);
                    }
                    inv.Base64Image = base64Image;
                    invList.Add(inv);
                }
                return invList;
            }
            else if (status == 2) //หาใบเสร็จที่ตรวจแล้วไม่ผ่าน
            {
                var invStatusList = await digitalclaimContext.IclaimInvoiceStatus.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo && w.Status == status).Select(s => new { s.IdInvhd, s.InvCommentTypeId, s.InvDocComment }).ToListAsync();
                var query = await rvpofficeContext.Invoicehd.Where(w => invStatusList.Select(s => s.IdInvhd).Contains(w.IdInvhd)).ToListAsync();
                

                List<InvoiceHeader> invNotPassList = new List<InvoiceHeader>();
                for (int i = 0; i < query.Count(); i++)
                {
                    InvoiceHeader invNotPass = new InvoiceHeader();
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
                    invNotPass.InvNotPassTypeId = invStatusList.Where(w => w.IdInvhd == query[i].IdInvhd).Select(s => s.InvCommentTypeId).FirstOrDefault();
                    invNotPass.InvNotPassComment = invStatusList.Where(w => w.IdInvhd == query[i].IdInvhd).Select(s => s.InvDocComment).FirstOrDefault();
                    invNotPassList.Add(invNotPass);
                }
                return invNotPassList;
            }


            return null;

        }

        public async Task<IclaimCheckDocuments> GetDocumentCheck(string accNo, int victimNo, int reqNo)
        {
            return await digitalclaimContext.IclaimCheckDocuments.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo).FirstOrDefaultAsync();
        }

        public async Task<ConfirmMoney> GetDataForConfirmMoney(string accNo, int victimNo, int reqNo)
        {


            var iclaimApproval = await digitalclaimContext.IclaimApproval
                .Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo && w.Status == 4).Select(s => new { s.SumReqMoney, s.SumPayMoney, s.InsertDate }).FirstOrDefaultAsync();
            var car = await rvpofficeContext.HosCarAccident.Where(w => w.AccNo == accNo).Select(s => new { s.FoundCarLicense, s.FoundChassisNo, s.FoundPolicyNo }).FirstOrDefaultAsync();
            var iclaimInvoiceStatusList = await digitalclaimContext.IclaimInvoiceStatus.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo).Select(s => new { s.IdInvhd, s.ReqMoney, s.PayMoney }).ToListAsync();

            CarHasPolicy carHasPolicy = new CarHasPolicy();
            carHasPolicy.FoundCarLicense = car.FoundCarLicense;
            carHasPolicy.FoundChassisNo = car.FoundChassisNo;
            carHasPolicy.FoundPolicyNo = car.FoundPolicyNo;

            ConfirmMoney confirmMoney = new ConfirmMoney();
            confirmMoney.ReqNo = reqNo;
            confirmMoney.ReqDate = iclaimApproval.InsertDate.Value.Date.ToString("dd/MM/yyyy");
            confirmMoney.ReqTime = iclaimApproval.InsertDate.Value.ToString("HH:mm");
            confirmMoney.SumReqMoney = (iclaimApproval.SumReqMoney != null) ? (double)iclaimApproval.SumReqMoney : 0;
            confirmMoney.SumPayMoney = (iclaimApproval.SumPayMoney != null) ? (double)iclaimApproval.SumPayMoney : 0;
            confirmMoney.Car = carHasPolicy;
            confirmMoney.BankAccount = await GetIClaimBankAccountAsync(accNo, victimNo, reqNo);

            //var invoicehds = await rvpofficeContext.Invoicehd.Where(w => iclaimInvoiceStatusList.Select(s => s.IdInvhd).ToList().Contains(w.IdInvhd)).ToListAsync();
            var invoicehds = await rvpofficeContext.Invoicehd
                .Join(rvpofficeContext.Hospital, invhd => invhd.Hosid, hos => hos.Hospitalid, (invhd, hos) => new { Invhd = invhd, HospitalName = hos.Hospitalname })
                .Where(w => iclaimInvoiceStatusList.Select(s => s.IdInvhd).ToList().Contains(w.Invhd.IdInvhd))
                .Select(s => new InvoiceHeader {
                    IdInvhd = s.Invhd.IdInvhd,
                    IdInvdt = s.Invhd.IdInvdt,
                    Hostype = s.Invhd.Hostype,
                    HospitalName = s.HospitalName,
                    Takendate = s.Invhd.Takendate,
                    Takentime = s.Invhd.Takentime,
                    Suminv = s.Invhd.Suminv,
                    BookNo = s.Invhd.BookNo,
                    ReceiptNo = s.Invhd.ReceiptNo
                }).ToListAsync();
            
            var invdtVerify = await GetIclaimInvoicedtVerify(invoicehds);
            var invCutList = await digitalclaimContext.IclaimInvoiceCutLists
                .Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo)
                .Select(s => new IclaimInvoiceCutList { IdInvhd = s.IdInvhd, CutListNo = s.CutListNo, CutListName = s.CutListName, CutListPrice = s.CutListPrice })
                .ToListAsync();
            List<InvoiceStatus> invStatusList = new List<InvoiceStatus>();
            for (int i = 0; i < iclaimInvoiceStatusList.Count; i++)
            {
                var invhd = invoicehds.Where(w => w.IdInvhd == iclaimInvoiceStatusList[i].IdInvhd).Select(s => new { s.BookNo, s.ReceiptNo }).FirstOrDefault();
                var idInvdt = invoicehds.Where(w => w.IdInvhd == iclaimInvoiceStatusList[i].IdInvhd).Select(s => s.IdInvdt).FirstOrDefault();
                InvoiceStatus invStatus = new InvoiceStatus();
                var refId = iclaimInvoiceStatusList[i].IdInvhd + "|" + accNo + "|" + victimNo;
                invStatus.IdInvhd = iclaimInvoiceStatusList[i].IdInvhd;
                invStatus.BookNo = invhd.BookNo;
                invStatus.ReceiptNo = invhd.ReceiptNo;
                invStatus.ReqMoney = iclaimInvoiceStatusList[i].ReqMoney;
                invStatus.PayMoney = iclaimInvoiceStatusList[i].PayMoney;
                invStatus.InvoicedtVerify = invdtVerify.Where(w => w.IdInvdt == idInvdt).FirstOrDefault();
                invStatus.InvoiceCutList = invCutList.Where(w => w.IdInvhd == iclaimInvoiceStatusList[i].IdInvhd).ToList();
                var maxRunningNo = await rvpSystemContext.EDocDetail
                        .Where(w => w.SystemId == "02" && w.TemplateId == "03" && w.DocumentId == "01" && w.RefId.StartsWith(refId))
                        .MaxAsync(m => m.RunningNo);
                var edocDetail = await rvpSystemContext.EDocDetail.Where(w => w.SystemId == "02" && w.TemplateId == "03" && w.DocumentId == "01" && w.RefId.StartsWith(refId) && w.RunningNo == maxRunningNo)
                    .Select(s => new EDocDetail() { Paths = s.Paths, CreateDate = s.CreateDate, RunningNo = s.RunningNo })
                    .OrderByDescending(o => o.RunningNo).ToListAsync();
                for (int j = 0; j < edocDetail.Count; j++)
                {
                    invStatus.Base64Image.Add(await attachmentService.DownloadFileFromECM(edocDetail[j].Paths));
                }
                    
                invStatusList.Add(invStatus);
            }
            confirmMoney.InvoiceList = invStatusList;

            return confirmMoney;
        }
        private async Task<List<IclaimInvoicedtVerify>> GetIclaimInvoicedtVerify(List<InvoiceHeader> invoicehds)
        {
            var iclaimInvdtVerify = await digitalclaimContext.IclaimInvoiceDtVerify.Where(w => invoicehds.Select(s => s.IdInvdt).Contains(w.IdInvdt)).ToListAsync();                     
            if (invoicehds.Count > 0)
            {
                var invhdHosTypeP = invoicehds.Where(w => w.Hostype != "G").Select(s => new { s.IdInvdt, s.HospitalName, s.Takendate, s.Takentime, s.Suminv }).ToList();
                var invhdHosTypeG = invoicehds.Where(w => w.Hostype == "G").Select(s => new { s.IdInvdt, s.HospitalName, s.Takendate, s.Takentime, s.Suminv }).ToList();
                if (invhdHosTypeP.Count > 0 && invhdHosTypeG.Count > 0)
                {
                    var invdtItemsP = await rvpofficeContext.Invoicedt
                    .Join(rvpofficeContext.Particulars, invdt => invdt.Treatid, par => par.Id, (invdt, par) => new { invdtID = invdt.IdInvdt, listNo = invdt.Listno, treatName = par.Name, money = invdt.Price })
                    .Where(w => invhdHosTypeP.Select(s => s.IdInvdt).Contains(w.invdtID)).ToListAsync();
                    var invdtItemsPJoinInvdtVerify = invdtItemsP
                        .Join(iclaimInvdtVerify, invdt => (invdt.invdtID, invdt.listNo), invdtver => (invdtver.IdInvdt, invdtver.Listno), (invdt, invdtver) => new { invDt = invdt, reqMoney = invdtver.InvPrice, paidMoney = invdtver.PaidPrice })
                        .Select(s => new { invdtID = s.invDt.invdtID, listNo = s.invDt.listNo, treatName = s.invDt.treatName, reqMoney = s.reqMoney, paidMoney = s.paidMoney })
                        .ToList();

                    var invdtItemsG = await rvpofficeContext.Invoicedt
                    .Join(rvpofficeContext.Particulars3, invdt => invdt.Treatid, par => par.Id, (invdt, par) => new { invdtID = invdt.IdInvdt, listNo = invdt.Listno, treatName = par.Name, money = invdt.Price })
                    .Where(w => invhdHosTypeG.Select(s => s.IdInvdt).Contains(w.invdtID))
                    .ToListAsync();
                    var invdtItemsGJoinInvdtVerify = invdtItemsG
                        .Join(iclaimInvdtVerify, invdt => (invdt.invdtID, invdt.listNo), invdtver => (invdtver.IdInvdt, invdtver.Listno), (invdt, invdtver) => new { invDt = invdt, reqMoney = invdtver.InvPrice, paidMoney = invdtver.PaidPrice })
                        .Select(s => new { invdtID = s.invDt.invdtID, listNo = s.invDt.listNo, treatName = s.invDt.treatName, reqMoney = s.reqMoney, paidMoney = s.paidMoney })
                        .ToList();

                    var mergeInvdtList = invdtItemsPJoinInvdtVerify.Concat(invdtItemsGJoinInvdtVerify).ToList();
                    var mergeHospital = invhdHosTypeP.Concat(invhdHosTypeG).ToList();

                    return mergeInvdtList.GroupBy(item => item.invdtID,
                    (key, group) => new IclaimInvoicedtVerify
                    {
                        IdInvdt = key,
                        Hospital = invhdHosTypeG.Where(w => w.IdInvdt == key).Select(s => s.HospitalName).FirstOrDefault(),
                        TakenDate = invhdHosTypeG.Where(w => w.IdInvdt == key).Select(s => s.Takendate.Value.ToString("dd/MM/yyyy")).FirstOrDefault(),
                        TakenTime = invhdHosTypeG.Where(w => w.IdInvdt == key).Select(s => s.Takentime).FirstOrDefault(),
                        SumMoney = invhdHosTypeG.Where(w => w.IdInvdt == key).Select(s => s.Suminv).FirstOrDefault(),
                        Items = group.Select(s => new { s.listNo, s.treatName, s.reqMoney, s.paidMoney }).OrderBy(o => o.listNo).ToList()
                    }).ToList();
                }
                else if (invhdHosTypeP.Count > 0 && invhdHosTypeG.Count == 0)
                {
                    var invdtItemsP = await rvpofficeContext.Invoicedt
                    .Join(rvpofficeContext.Particulars, invdt => invdt.Treatid, par => par.Id, (invdt, par) => new { invdtID = invdt.IdInvdt, listNo = invdt.Listno, treatName = par.Name, money = invdt.Price })
                    .Where(w => invhdHosTypeP.Select(s => s.IdInvdt).Contains(w.invdtID)).ToListAsync();

                    var invdtItemsPJoinInvdtVerify = invdtItemsP
                        .Join(iclaimInvdtVerify, invdt => (invdt.invdtID, invdt.listNo), invdtver => (invdtver.IdInvdt, invdtver.Listno), (invdt, invdtver) => new { invDt = invdt, reqMoney = invdtver.InvPrice, paidMoney = invdtver.PaidPrice })
                        .Select(s => new { invdtID = s.invDt.invdtID, listNo = s.invDt.listNo, treatName = s.invDt.treatName, reqMoney = s.reqMoney, paidMoney = s.paidMoney })
                        .ToList();
                    return invdtItemsPJoinInvdtVerify.GroupBy(item => item.invdtID,
                   (key, group) => new IclaimInvoicedtVerify
                   {
                       IdInvdt = key,
                       Hospital = invhdHosTypeG.Where(w => w.IdInvdt == key).Select(s => s.HospitalName).FirstOrDefault(),
                       TakenDate = invhdHosTypeG.Where(w => w.IdInvdt == key).Select(s => s.Takendate.Value.ToString("dd/MM/yyyy")).FirstOrDefault(),
                       TakenTime = invhdHosTypeG.Where(w => w.IdInvdt == key).Select(s => s.Takentime).FirstOrDefault(),
                       SumMoney = invhdHosTypeG.Where(w => w.IdInvdt == key).Select(s => s.Suminv).FirstOrDefault(),
                       Items = group.Select(s => new { s.listNo, s.treatName, s.reqMoney, s.paidMoney }).OrderBy(o => o.listNo).ToList()
                   }).ToList();
                }
                else if (invhdHosTypeG.Count > 0 && invhdHosTypeP.Count == 0)
                {
                    var invdtItemsG = await rvpofficeContext.Invoicedt
                    .Join(rvpofficeContext.Particulars3, invdt => invdt.Treatid, par => par.Id, (invdt, par) => new { invdtID = invdt.IdInvdt, listNo = invdt.Listno, treatName = par.Name, money = invdt.Price })
                    .Where(w => invhdHosTypeG.Select(s => s.IdInvdt).Contains(w.invdtID))
                    .ToListAsync();
                    var invdtItemsGJoinInvdtVerify = invdtItemsG
                        .Join(iclaimInvdtVerify, invdt => ( invdt.invdtID, invdt.listNo ), invdtver => ( invdtver.IdInvdt, invdtver.Listno ), (invdt, invdtver) => new { invDt = invdt, reqMoney = invdtver.InvPrice, paidMoney = invdtver.PaidPrice })
                        .Select(s => new { invdtID = s.invDt.invdtID, listNo = s.invDt.listNo, treatName = s.invDt.treatName, reqMoney = s.reqMoney, paidMoney = s.paidMoney })
                        .ToList();

                    return invdtItemsGJoinInvdtVerify.GroupBy(item => item.invdtID,
                    (key, group) => new IclaimInvoicedtVerify
                    {
                        IdInvdt = key,
                        Hospital = invhdHosTypeG.Where(w => w.IdInvdt == key).Select(s => s.HospitalName).FirstOrDefault(),
                        TakenDate = invhdHosTypeG.Where(w => w.IdInvdt == key).Select(s => s.Takendate.Value.ToString("dd/MM/yyyy")).FirstOrDefault(),
                        TakenTime = invhdHosTypeG.Where(w => w.IdInvdt == key).Select(s => s.Takentime).FirstOrDefault(),
                        SumMoney = invhdHosTypeG.Where(w => w.IdInvdt == key).Select(s => s.Suminv).FirstOrDefault(),
                        Items = group.Select(s => new { s.listNo, s.treatName, s.reqMoney, s.paidMoney }).OrderBy(o => o.listNo).ToList()
                    }).ToList();
                }
            }

            return null;
        }

        public async Task<ApprovalDetail> GetApprovalDetail(string accNo, int victimNo, int reqNo, string userIdCard)
        {
            var victim = new Victim();
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
            var kyc = await ipolicyContext.DirectPolicyKyc.Where(w => w.IdcardNo == userIdCard && w.Status == "Y").OrderByDescending(o => o.Kycno).FirstOrDefaultAsync();

            if (accVic == null) return null;
            victim.IdCardNo = kyc.IdcardNo;
            victim.Fname = accVic.Fname;
            victim.Lname = accVic.Lname;
            victim.Prefix = accVic.Prefix;
            victim.Age = accVic.Age;
            victim.Sex = accVic.Sex;
            victim.TelNo = kyc.MobileNo;

            victim.HomeId = accVic.HomeId;
            victim.Moo = accVic.Moo;
            victim.Soi = accVic.Soi;
            victim.Road = accVic.Road;
            victim.Tumbol = accVic.TumbolId;
            victim.TumbolName = accVic.TumbolName;
            victim.District = accVic.AmphurId;
            victim.DistrictName = accVic.AmphurName;
            victim.Province = accVic.ChangwatShort;
            victim.ProvinceName = accVic.ChangwatName;
            victim.Zipcode = accVic.Zipcode;

            //ข้อมูลรถ
            var car = await rvpofficeContext.HosCarAccident.Where(w => w.AccNo == accNo).Select(s => new { s.FoundCarLicense, s.FoundChassisNo, s.FoundPolicyNo }).FirstOrDefaultAsync();
            CarHasPolicy carHasPolicy = new CarHasPolicy();
            carHasPolicy.FoundCarLicense = car.FoundCarLicense;
            carHasPolicy.FoundChassisNo = car.FoundChassisNo;
            carHasPolicy.FoundPolicyNo = car.FoundPolicyNo;

            var iclaimApproval = await digitalclaimContext.IclaimApproval
                .Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo).Select(s => new { s.SumReqMoney, s.InsertDate }).FirstOrDefaultAsync();

            ApprovalDetail approvalDetail = new ApprovalDetail();
            approvalDetail.ReqNo = reqNo;
            approvalDetail.ReqDate = iclaimApproval.InsertDate.Value.Date.ToString("dd/MM/yyyy");
            approvalDetail.ReqTime = iclaimApproval.InsertDate.Value.ToString("HH:mm");
            approvalDetail.SumReqMoney = (double)iclaimApproval.SumReqMoney;
            approvalDetail.Victim = victim;
            approvalDetail.Car = carHasPolicy;
            approvalDetail.Invoicehds = await GetInvoicehdAsync(accNo, victimNo, reqNo, 0);
            approvalDetail.BankAccount = await GetIClaimBankAccountAsync(accNo, victimNo, reqNo);
            approvalDetail.BankAccount.bankId = approvalDetail.BankAccount.accountBankName;
            approvalDetail.BankAccount.accountBankName = await rvpofficeContext.BankNames.Where(w => w.BankCode != null && w.BankCode == approvalDetail.BankAccount.bankId).Select(s => s.Name).FirstOrDefaultAsync();

            return approvalDetail;
        }

        public async Task<ApprovalPDF> GetApprovalDataForGenPDF(string accNo, int victimNo, int reqNo)
        {
            var iclaimIdInvhd = await digitalclaimContext.IclaimInvoiceStatus.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo).Select(s => s.IdInvhd).ToListAsync();
            var hosIdList = await rvpofficeContext.Invoicehd.Where(w => w.IdInvhd == iclaimIdInvhd[0]).Select(s => s.Hosid).ToListAsync();
            var invhd = await rvpofficeContext.Invoicehd.Where(w => w.IdInvhd == iclaimIdInvhd[0]).Select(s => new { s.AppNo, s.RecordDate }).FirstOrDefaultAsync();
            var query = await rvpofficeContext.HosApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.AppNo == invhd.AppNo).FirstOrDefaultAsync();
            var iclaimApp = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo).Select(s => new { s.SumReqMoney, s.RefCodeOtp, s.IsEverAuthorize, s.EverAuthorizeHosId, s.EverAuthorizeMoney}).FirstOrDefaultAsync();

            ApprovalPDF approvalPDF = new ApprovalPDF();
            if (query == null)
            {
                approvalPDF.CureMoney = (double)iclaimApp.SumReqMoney;
                approvalPDF.TextCureMoney = ThaiBahtText(iclaimApp.SumReqMoney.ToString());
                approvalPDF.IdInvhd = string.Join(", ", iclaimIdInvhd);
                approvalPDF.InvCount = iclaimIdInvhd.Count;
                approvalPDF.RecordDay = invhd.RecordDate.Value.ToString("dd");
                approvalPDF.RecordMonth = await GetMonthName(invhd.RecordDate.Value.ToString("MM"));
                approvalPDF.RecordYear = (int.Parse(invhd.RecordDate.Value.ToString("yyyy")) + 543).ToString();
                approvalPDF.OtpSign = iclaimApp.RefCodeOtp;
                approvalPDF.HosId = string.Join(", ", hosIdList);
                approvalPDF.IsEverAuthorize = iclaimApp.IsEverAuthorize;
                approvalPDF.EverAuthorizeHosId = iclaimApp.EverAuthorizeHosId;
                approvalPDF.EverAuthorizeMoney = iclaimApp.EverAuthorizeMoney;
                return approvalPDF;
            }
            approvalPDF.ClaimNo = query.ClaimNo;
            approvalPDF.IdInvhd = string.Join(", ", iclaimIdInvhd);
            approvalPDF.InvCount = iclaimIdInvhd.Count;
            approvalPDF.CureMoney = (double)query.CureMoney;
            approvalPDF.TextCureMoney = ThaiBahtText(query.CureMoney.ToString());
            approvalPDF.RecordDay = invhd.RecordDate.Value.ToString("dd");
            approvalPDF.RecordMonth = await GetMonthName(invhd.RecordDate.Value.ToString("MM"));
            approvalPDF.RecordYear = (int.Parse(invhd.RecordDate.Value.ToString("yyyy")) + 543).ToString();
            approvalPDF.OtpSign = iclaimApp.RefCodeOtp;
            approvalPDF.HosId = string.Join(", ", hosIdList);
            approvalPDF.IsEverAuthorize = iclaimApp.IsEverAuthorize;
            approvalPDF.EverAuthorizeHosId = iclaimApp.EverAuthorizeHosId;
            approvalPDF.EverAuthorizeMoney = iclaimApp.EverAuthorizeMoney;
            //approvalPDF.ReqDate = query.InsertDate.Value.ToString("dd/MM/yyyy");
            //approvalPDF.TimeDate = query.InsertDate.Value.ToString("HH:mm");
            return approvalPDF;
        }

        private async Task<string> GetMonthName(string monthNo)
        {
            if (monthNo == "01") return "มกราคม";
            else if (monthNo == "02") return "กุมภาพันธ์";
            else if (monthNo == "03") return "มีนาคาม";
            else if (monthNo == "04") return "เมษายน";
            else if (monthNo == "05") return "พฤษภาคม";
            else if (monthNo == "06") return "มิถุนายน";
            else if (monthNo == "07") return "กรกฎาคม";
            else if (monthNo == "08") return "สิงหาคม";
            else if (monthNo == "09") return "กันยายน";
            else if (monthNo == "10") return "ตุลาคม";
            else if (monthNo == "11") return "พฤศจิกายน";
            else if (monthNo == "12") return "ธันวาคม";

            return null;
        }

        private string ThaiBahtText(string strNumber, bool IsTrillion = false)
        {
            string BahtText = "";
            string strTrillion = "";
            string[] strThaiNumber = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
            string[] strThaiPos = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };

            decimal decNumber = 0;
            decimal.TryParse(strNumber, out decNumber);

            if (decNumber == 0)
            {
                return "ศูนย์บาทถ้วน";
            }

            strNumber = decNumber.ToString("0.00");
            string strInteger = strNumber.Split('.')[0];
            string strSatang = strNumber.Split('.')[1];

            if (strInteger.Length > 13)
                throw new Exception("รองรับตัวเลขได้เพียง ล้านล้าน เท่านั้น!");

            bool _IsTrillion = strInteger.Length > 7;
            if (_IsTrillion)
            {
                strTrillion = strInteger.Substring(0, strInteger.Length - 6);
                BahtText = ThaiBahtText(strTrillion, _IsTrillion);
                strInteger = strInteger.Substring(strTrillion.Length);
            }

            int strLength = strInteger.Length;
            for (int i = 0; i < strInteger.Length; i++)
            {
                string number = strInteger.Substring(i, 1);
                if (number != "0")
                {
                    if (i == strLength - 1 && number == "1" && strLength != 1)
                    {
                        BahtText += "เอ็ด";
                    }
                    else if (i == strLength - 2 && number == "2" && strLength != 1)
                    {
                        BahtText += "ยี่";
                    }
                    else if (i != strLength - 2 || number != "1")
                    {
                        BahtText += strThaiNumber[int.Parse(number)];
                    }

                    BahtText += strThaiPos[(strLength - i) - 1];
                }
            }

            if (IsTrillion)
            {
                return BahtText + "ล้าน";
            }

            if (strInteger != "0")
            {
                BahtText += "บาท";
            }

            if (strSatang == "00")
            {
                BahtText += "ถ้วน";
            }
            else
            {
                strLength = strSatang.Length;
                for (int i = 0; i < strSatang.Length; i++)
                {
                    string number = strSatang.Substring(i, 1);
                    if (number != "0")
                    {
                        if (i == strLength - 1 && number == "1" && strSatang[0].ToString() != "0")
                        {
                            BahtText += "เอ็ด";
                        }
                        else if (i == strLength - 2 && number == "2" && strSatang[0].ToString() != "0")
                        {
                            BahtText += "ยี่";
                        }
                        else if (i != strLength - 2 || number != "1")
                        {
                            BahtText += strThaiNumber[int.Parse(number)];
                        }

                        BahtText += strThaiPos[(strLength - i) - 1];
                    }
                }

                BahtText += "สตางค์";
            }

            return BahtText;
        }

        public async Task<CheckDuplicateInvoice[]> CheckDuplicateInvoice(CheckDuplicateInvoice[] invoicehds)
        {
            if (invoicehds[0].ReqNo > 0) //คำร้องที่มีอยู่
            {
                foreach (var inv in invoicehds.Where(w => w.IsCansel == false))
                {
                    var iClaimInvId = await digitalclaimContext.IclaimInvoiceStatus.Where(w => w.IdInvhd == inv.BillId).Select(s => s.IdInvhd).FirstOrDefaultAsync();
                    var query = await rvpofficeContext.Invoicehd.Where(w => w.IdInvhd != iClaimInvId && w.BookNo == inv.BookNo && w.ReceiptNo == inv.ReceiptNo && w.Hosid == inv.HosId).FirstOrDefaultAsync();
                    if (query != null)
                    {
                        inv.IsDuplicate = true;
                    }
                    else
                    {
                        inv.IsDuplicate = false;
                    }

                }
            }
            else //คำร้องใหม่
            {
                foreach (var inv in invoicehds)
                {
                    var query = await rvpofficeContext.Invoicehd.Where(w => w.BookNo == inv.BookNo && w.ReceiptNo == inv.ReceiptNo && w.Hosid == inv.HosId).FirstOrDefaultAsync();
                    if (query != null)
                    {
                        inv.IsDuplicate = true;
                    }
                }
            }

            return invoicehds;
        }
        public async Task<string> CanselApprovalAsync(string accNo, int victimNo, int reqNo, string userIdLine)
        {
            var iclaimApproval = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo).FirstOrDefaultAsync();
            var iclaimInvStatusList = await digitalclaimContext.IclaimInvoiceStatus.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo ).ToListAsync();
            var lastInvoiceStatusState = await digitalclaimContext.IclaimInvoiceStatusLog
                        .Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.ReqNo == reqNo)
                        .Select(s => new { s.IdInvhd, s.StateNo, s.NewStatus }).ToListAsync();
            for (int i = 0; i < iclaimInvStatusList.Count(); i++)
            {
                var dataIclaimInvoiceStatusLog = new IclaimInvoiceStatusLog();
                dataIclaimInvoiceStatusLog.IdInvhd = iclaimInvStatusList[i].IdInvhd;
                dataIclaimInvoiceStatusLog.AccNo = iclaimInvStatusList[i].AccNo;
                dataIclaimInvoiceStatusLog.VictimNo = iclaimInvStatusList[i].VictimNo;
                dataIclaimInvoiceStatusLog.ReqNo = iclaimInvStatusList[i].ReqNo;
                dataIclaimInvoiceStatusLog.StateNo = lastInvoiceStatusState.Where(w => w.IdInvhd == iclaimInvStatusList[i].IdInvhd).Select(s => s.StateNo).OrderByDescending(o => o).FirstOrDefault() + 1;
                dataIclaimInvoiceStatusLog.NewStatus = 0;
                dataIclaimInvoiceStatusLog.InsertDate = DateTime.Now;
                dataIclaimInvoiceStatusLog.InvDocComment = iclaimInvStatusList[i].InvDocComment;
                dataIclaimInvoiceStatusLog.InvCommentTypeId = iclaimInvStatusList[i].InvCommentTypeId;
                dataIclaimInvoiceStatusLog.RecordBy = userIdLine;
                dataIclaimInvoiceStatusLog.OldStatus = lastInvoiceStatusState.Where(w => w.IdInvhd == iclaimInvStatusList[i].IdInvhd && w.StateNo == (dataIclaimInvoiceStatusLog.StateNo - 1)).Select(s => s.NewStatus).FirstOrDefault();
                dataIclaimInvoiceStatusLog.Ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                await digitalclaimContext.IclaimInvoiceStatusLog.AddAsync(dataIclaimInvoiceStatusLog);

                iclaimInvStatusList[i].Status = 0;
                iclaimInvStatusList[i].InvCommentTypeId = null;
                iclaimInvStatusList[i].InvDocComment = null;
                iclaimInvStatusList[i].LastUpdateDate = DateTime.Now;
                             
            }
            await digitalclaimContext.SaveChangesAsync();
            await UpdateApprovalStatusAsync(accNo, victimNo, reqNo, "CanselApproval", false);
            
            return "Success";
        }



       
    }
}
