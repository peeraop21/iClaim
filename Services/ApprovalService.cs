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

namespace Services
{
    public interface IApprovalService
    {
        Task<List<ApprovalregisViewModel>> GetApproval(string accNo, int victimNo, int rightsType);
        Task<ClaimViewModel> GetApprovalByClaimNo(string claimNo, short victimNo, short regNo, int rightsType);
        Task<ClaimViewModel> GetApprovalByAccNo(string accNo, int? victimNo);
        Task<List<DataAccess.EFCore.DigitalClaimModels.Invoicehd>> AddAsync(IclaimApproval iclaimApproval, InputBankViewModel inputBank, VictimtViewModel victim, DataAccess.EFCore.DigitalClaimModels.Invoicehd[] invoicehd);
        Task<string> UpdateAsync(string accNo, int victimNo, int appNo, UpdateBankViewModel bankModel, UpdateInvoiceViewModel[] invModel);
        Task<List<IcliamApprovalViewModel>> GetIClaimApprovalAsync(string accNo, int victimNo);
        Task<List<InputBankViewModel>> GetHosDocumentReceiveAsync(string accNo, int victimNo, int appNo);
        Task<List<InputBankViewModel>> GetLastHosDocumentReceiveAsync(string accNo, int victimNo);
        Task<double?> GetRightsBalance(string accNo, int? victimNo, string rightsType);
        Task<string> UpdateApprovalStatusAsync(string accNo, int victimNo, int appNo, string status);
        Task<List<InvoicehdNotPassViewModel>> GetInvoicehdAsync(string accNo, int victimNo, int appNo);
        Task<IclaimCheckDocuments> GetDocumentCheck(string accNo, int victimNo, int appNo);
        //Task<string> GeneratePT(string accN);

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
        public async Task<List<DataAccess.EFCore.DigitalClaimModels.Invoicehd>> AddAsync(IclaimApproval iclaimApproval, InputBankViewModel inputBank, VictimtViewModel victim, DataAccess.EFCore.DigitalClaimModels.Invoicehd[] invoicehd)
        {
            /*var query = await rvpofficeContext.HosApproval.Where(w => w.AccNo == hosApproval.AccNo && w.VictimNo == hosApproval.VictimNo).Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.Pt4id }).LastOrDefaultAsync();*/
            var lastAppNo = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == iclaimApproval.AccNo && w.VictimNo == iclaimApproval.VictimNo).Select(s => s.AppNo).OrderByDescending(o => o).FirstOrDefaultAsync();
            if(lastAppNo == null)
            {
                lastAppNo = 0;
            }
            var dataIcliamApproval = new IclaimApproval();
            dataIcliamApproval.AccNo = iclaimApproval.AccNo;
            dataIcliamApproval.VictimNo = iclaimApproval.VictimNo;
            dataIcliamApproval.AppNo = lastAppNo + 1;
            dataIcliamApproval.Status = 1;
            dataIcliamApproval.SumMoney = iclaimApproval.SumMoney;
            dataIcliamApproval.CureMoney = iclaimApproval.SumMoney;
            dataIcliamApproval.InsertDate = DateTime.Now;
            dataIcliamApproval.LastUpdate = DateTime.Now;
            dataIcliamApproval.RevPrefix = victim.Prefix;
            dataIcliamApproval.RevFname = victim.Fname;
            dataIcliamApproval.RevLname = victim.Lname;
            dataIcliamApproval.RevRelate = "000";
            dataIcliamApproval.RecPrefix = victim.Prefix;
            dataIcliamApproval.RecFname = victim.Fname;
            dataIcliamApproval.RecLname = victim.Lname;
            dataIcliamApproval.RecRelate = "000";
            dataIcliamApproval.RecSocNo = victim.DrvSocNo;

            
            await digitalclaimContext.IclaimApproval.AddAsync(dataIcliamApproval);

            var runNo = await digitalclaimContext.HosDocumentReceive.Where(w => w.AccNo == iclaimApproval.AccNo && w.VictimNo == (short)iclaimApproval.VictimNo && w.Appno == (short)dataIcliamApproval.AppNo).Select(s => s.RunNo).OrderByDescending(o => o).FirstOrDefaultAsync();
            var dataHosDocumentReceive = new HosDocumentReceive();
            dataHosDocumentReceive.AccNo = iclaimApproval.AccNo;
            dataHosDocumentReceive.VictimNo = (short)iclaimApproval.VictimNo;
            dataHosDocumentReceive.Appno = (short)(lastAppNo+1);
            dataHosDocumentReceive.RunNo = (runNo == null || runNo  == 0) ? (short)1 : (short)(runNo + 1);
            dataHosDocumentReceive.PaymentType = "D";
            dataHosDocumentReceive.AccountNo = inputBank.accountNumber;
            dataHosDocumentReceive.AccountName = inputBank.accountName;
            dataHosDocumentReceive.BankId = inputBank.bankId;
            await digitalclaimContext.HosDocumentReceive.AddAsync(dataHosDocumentReceive);            

            var dataIclaimApprovalStatusState = new IclaimApprovalStatusState();
            dataIclaimApprovalStatusState.AccNo = iclaimApproval.AccNo;
            dataIclaimApprovalStatusState.AppNo = lastAppNo + 1;
            dataIclaimApprovalStatusState.VictimNo = iclaimApproval.VictimNo;
            dataIclaimApprovalStatusState.StateNo = 1;
            dataIclaimApprovalStatusState.OldStatus = null;
            dataIclaimApprovalStatusState.NewStatus = 1;
            dataIclaimApprovalStatusState.InsertDate = DateTime.Now;
            await digitalclaimContext.IclaimApprovalStatusState.AddAsync(dataIclaimApprovalStatusState);

            var idInvhd = await digitalclaimContext.Invoicehd.Select(s => s.IdInvhd).OrderByDescending(o => o).FirstOrDefaultAsync();
            List <DataAccess.EFCore.DigitalClaimModels.Invoicehd> invoicehds = new List<DataAccess.EFCore.DigitalClaimModels.Invoicehd>();
            for (int i = 0; i < invoicehd.Length; i++)
            {
                var dataInvoicehd = new DataAccess.EFCore.DigitalClaimModels.Invoicehd();
                
                dataInvoicehd.IdInvhd = idInvhd + i + 1;
                dataInvoicehd.AccNo = iclaimApproval.AccNo;
                dataInvoicehd.VictimNo = iclaimApproval.VictimNo;
                dataInvoicehd.AppNo =  lastAppNo + 1;
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
                invoicehds.Add(dataInvoicehd);
                await digitalclaimContext.Invoicehd.AddAsync(dataInvoicehd);
            }

            //for (int i = 0; i < invoicehd.Length; i++)
            //{
            //    invoicehd[i].IdInvhd = idInvhd + 1;
            //    invoicehd[i].AccNo = hosApproval.AccNo;
            //    invoicehd[i].VictimNo = hosApproval.VictimNo;
            //    invoicehd[i].AppNo = hosApproval.AppNo + 1;
            //    var test = invoicehd[i].Takendate.Value.ToShortDateString();
            //    invoicehd[i].Takendate = null;
            //    await digitalclaimContext.Invoicehd.AddAsync(invoicehd[i]);
            //}


            await digitalclaimContext.SaveChangesAsync();

            return invoicehds;
        }
        public async Task<string> UpdateAsync(string accNo , int victimNo, int appNo, UpdateBankViewModel bankModel, UpdateInvoiceViewModel[] invModel)
        {
            var hosDocumentReceive = await digitalclaimContext.HosDocumentReceive.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.Appno == appNo).FirstOrDefaultAsync();
            hosDocumentReceive.AccountName = bankModel.accountName;
            hosDocumentReceive.AccountNo = bankModel.accountNumber;
            hosDocumentReceive.BankId = bankModel.bankId;

            for(int i = 0; i < invModel.Length; i++)
            {
                var invhd = await digitalclaimContext.Invoicehd.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.AppNo == appNo && w.IdInvhd == invModel[i].billNo).FirstOrDefaultAsync();
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
            var query = await rvpofficeContext.HosApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo).Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.Pt4id}).OrderByDescending(o => o.AppNo).Take(1).FirstOrDefaultAsync();
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
        public async Task<List<ApprovalregisViewModel>> GetApproval(string accNo, int victimNo, int rightsType)
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

            if(rightsType == 1)
            {              
                var curemoneyQuery = await rvpofficeContext.HosApproval.Where(w => w.ClaimNo == claimNo && w.VictimNoClaim == victimNo && w.RegNoClaim == regNo && w.PayMore != "Y" && w.PayMore != "B" && (w.CrippledMoney == 0 || w.CrippledMoney == null))
                    .Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.VictimNoClaim, s.RegNoClaim, s.Pt4id, s.MedicineMoney, s.PlasticMoney, s.ServiceMoney, s.RoomMoney, s.VeihcleMoney, s.CureMoney, s.DeadMoney, s.HygieneMoney, s.CrippledMoney, s.SumMoney, s.BlindCrippled, s.UnHearCrippled, s.DeafCrippled, s.LostSexualCrippled, s.LostOrganCrippled, s.LostMindCrippled, s.CrippledPermanent, s.OtherCrippled, s.CrippledComment, s.PayMore })
                    .FirstOrDefaultAsync();
                if(curemoneyQuery == null)
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
            } else if (rightsType == 2)
            {                  
                var crippledQuery = await rvpofficeContext.HosApproval.Where(w => w.ClaimNo == claimNo && w.VictimNoClaim == victimNo && w.RegNoClaim == regNo && w.PayMore != "Y" && w.PayMore != "B" && w.CrippledMoney > 0)
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
            
            return claimVwModel;
        }
        public async Task<List<IcliamApprovalViewModel>> GetIClaimApprovalAsync(string accNo, int victimNo)
        {
            var query = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo).Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.InsertDate,s.SumMoney, s.Status}).ToListAsync();
            var vwIclaimAppList = new List<IcliamApprovalViewModel>();
            if (query == null)
            {
                return vwIclaimAppList;
            }
            foreach (var iclaimApp in query) {
                var vwIclaimApp = new IcliamApprovalViewModel();
                vwIclaimApp.AccNo = iclaimApp.AccNo;
                vwIclaimApp.StringAccNo = iclaimApp.AccNo.Replace("/", "-");
                vwIclaimApp.AppNo = iclaimApp.AppNo;
                vwIclaimApp.InsertDate = iclaimApp.InsertDate;
                vwIclaimApp.SumMoney = iclaimApp.SumMoney;
                vwIclaimApp.Status = iclaimApp.Status;
                vwIclaimApp.StringRegDate = iclaimApp.InsertDate.Value.ToString("dd/MM/yyyy HH:mm");
                //var queryStatus = await digitalclaimContext.IclaimApproval
                //    .Join(digitalclaimContext.IclaimApprovalStatus, appss => appss.Status, apps => apps.StatusId, (appss, apps) => new { appStatusStateJoinAppStatus = appss, statusName = apps.StatusNameIclaim })
                //    .Where(w => w.appStatusStateJoinAppStatus.AccNo == hosApp.AccNo && w.appStatusStateJoinAppStatus.AppNo == hosApp.AppNo && w.appStatusStateJoinAppStatus.VictimNo == hosApp.VictimNo).Select(s => new { s.statusName, s.appStatusStateJoinAppStatus.Status }).FirstOrDefaultAsync();

                vwIclaimApp.AppStatus = await GetApprovalStatus(iclaimApp.AccNo, iclaimApp.VictimNo, iclaimApp.AppNo );
                vwIclaimApp.AppStatusName = vwIclaimApp.AppStatus.Where(w => w.Active == true).Select(s => new { s.StatusName , s.StatusId}).OrderByDescending(o => o.StatusId).Select(s => s.StatusName).FirstOrDefault();
                vwIclaimAppList.Add(vwIclaimApp);
            }

            return vwIclaimAppList;
        }

        private async Task<List<ApprovalStatusViewModel>> GetApprovalStatus(string accNo, int victimNo, int appNo)
        {
            var query = await digitalclaimContext.IclaimApprovalStatus.ToListAsync();
            var appStatus = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.AppNo == appNo).FirstOrDefaultAsync();
            var appStatusState = await digitalclaimContext.IclaimApprovalStatusState.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.AppNo == appNo).ToListAsync();
            
            var appStatusVwModelList = new List<ApprovalStatusViewModel>();
            for(int i = 0; i < query.Count; i++)
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

        public async Task<List<InputBankViewModel>> GetHosDocumentReceiveAsync(string accNo, int victimNo, int appNo)
        {
            var query = await digitalclaimContext.HosDocumentReceive.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.Appno == appNo && w.PaymentType == "D").Select(s => new { s.AccountNo, s.AccountName, s.BankId, s.Appno }).FirstOrDefaultAsync();
            //var bankName = await rvpofficeContext.BankNames.Where(w => w.BankCode != null && w.BankCode == query.BankId).Select(s =>  s.Name).FirstOrDefaultAsync();
            var inputBankViewModelsList = new List<InputBankViewModel>();
            if (query == null)
            {
                return inputBankViewModelsList;
            }
            var inputBankViewModel = new InputBankViewModel();
            //inputBankViewModel.accountNumber = query.AccountNo;
            //inputBankViewModel.accountName = query.AccountName;
            //inputBankViewModel.accountBankName = bankName;
            //inputBankViewModel.bankId = query.BankId;
            inputBankViewModel.accountNumber = query.AccountNo;
            inputBankViewModel.accountName = query.AccountName;
            inputBankViewModel.accountBankName = query.BankId;
            inputBankViewModel.appNo = query.Appno;
            inputBankViewModelsList.Add(inputBankViewModel);
            return inputBankViewModelsList;
        }
        public async Task<List<InputBankViewModel>> GetLastHosDocumentReceiveAsync(string accNo, int victimNo)
        {
            var query = await digitalclaimContext.HosDocumentReceive.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.PaymentType == "D").Select(s => new { s.AccountNo, s.AccountName, s.BankId, s.Appno}).OrderByDescending(o => o.Appno).FirstOrDefaultAsync();
            var inputBankViewModelsList = new List<InputBankViewModel>();
            if (query == null)
            {
                return inputBankViewModelsList;
            }
            var inputBankViewModel = new InputBankViewModel();
            inputBankViewModel.accountNumber = query.AccountNo;
            inputBankViewModel.accountName = query.AccountName;
            inputBankViewModel.accountBankName = query.BankId;
            inputBankViewModel.appNo = query.Appno;
            inputBankViewModelsList.Add(inputBankViewModel);
            return inputBankViewModelsList;
        }




        public async Task<double?> GetRightsBalance(string accNo, int? victimNo,string rightsType)
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
                var approvalStatus = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.AppNo == appNo).FirstOrDefaultAsync();
                if (approvalStatus.Status != 4)
                {
                    return "Error";
                }
                approvalStatus.LastUpdate = DateTime.Now;
                approvalStatus.Status = 5;

                var lastStatusState = await digitalclaimContext.IclaimApprovalStatusState.Where(w => w.AccNo == accNo && w.AppNo == appNo && w.VictimNo == victimNo).Select(s => new {s.StateNo, s.NewStatus}).OrderByDescending(o => o.StateNo).FirstOrDefaultAsync();
                var dataHosApprovalStatusState = new IclaimApprovalStatusState();
                dataHosApprovalStatusState.AccNo = accNo;
                dataHosApprovalStatusState.AppNo = appNo;
                dataHosApprovalStatusState.VictimNo = victimNo;
                dataHosApprovalStatusState.StateNo = lastStatusState.StateNo +1;
                dataHosApprovalStatusState.OldStatus = lastStatusState.NewStatus;
                dataHosApprovalStatusState.NewStatus = 5;
                dataHosApprovalStatusState.InsertDate = DateTime.Now;
                await digitalclaimContext.IclaimApprovalStatusState.AddAsync(dataHosApprovalStatusState);

                await digitalclaimContext.SaveChangesAsync();
                return "Success";
            }else if (status == "EditDocument")
            {
                var approvalStatus = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.AppNo == appNo).FirstOrDefaultAsync();
                if (approvalStatus.Status != 2)
                {
                    return "Error";
                }
                approvalStatus.LastUpdate = DateTime.Now;
                approvalStatus.Status = 1;

                var lastStatusState = await digitalclaimContext.IclaimApprovalStatusState.Where(w => w.AccNo == accNo && w.AppNo == appNo && w.VictimNo == victimNo).Select(s => new { s.StateNo, s.NewStatus }).OrderByDescending(o => o.StateNo).FirstOrDefaultAsync();
                var dataHosApprovalStatusState = new IclaimApprovalStatusState();
                dataHosApprovalStatusState.AccNo = accNo;
                dataHosApprovalStatusState.AppNo = appNo;
                dataHosApprovalStatusState.VictimNo = victimNo;
                dataHosApprovalStatusState.StateNo = lastStatusState.StateNo + 1;
                dataHosApprovalStatusState.OldStatus = lastStatusState.NewStatus;
                dataHosApprovalStatusState.NewStatus = 1;
                dataHosApprovalStatusState.InsertDate = DateTime.Now;
                await digitalclaimContext.IclaimApprovalStatusState.AddAsync(dataHosApprovalStatusState);

                await digitalclaimContext.SaveChangesAsync();
            }
            return "Error";
        }

        public async Task<List<InvoicehdNotPassViewModel>> GetInvoicehdAsync(string accNo, int victimNo, int appNo )
        {
            var qurey = await digitalclaimContext.Invoicehd
                .Join(digitalclaimContext.IclaimCheckInvoiceDocuments, invhd => invhd.IdInvhd, invhdchk => invhdchk.IdInvhd, (invhd, invhdchk) => new { invoicehdJoinInvoiceChk = invhd, invoiceStatus = invhdchk.InvoiceStatus })
                .Where(w => w.invoicehdJoinInvoiceChk.AccNo == accNo && w.invoicehdJoinInvoiceChk.VictimNo == victimNo && w.invoicehdJoinInvoiceChk.AppNo == appNo && w.invoiceStatus == "ไม่ผ่าน")
                .Select(s => new { s.invoicehdJoinInvoiceChk.AccNo, s.invoicehdJoinInvoiceChk.VictimNo, s.invoicehdJoinInvoiceChk.AppNo, s.invoicehdJoinInvoiceChk.Hosid,
                    s.invoicehdJoinInvoiceChk.Mainconsider, s.invoicehdJoinInvoiceChk.VictimType, s.invoicehdJoinInvoiceChk.ReceiptNo, s.invoicehdJoinInvoiceChk.Suminv, 
                    s.invoicehdJoinInvoiceChk.Takendate, s.invoicehdJoinInvoiceChk.Takentime, s.invoicehdJoinInvoiceChk.Dispensedate, s.invoicehdJoinInvoiceChk.Dispensetime,
                    s.invoiceStatus, s.invoicehdJoinInvoiceChk.IdInvhd, s.invoicehdJoinInvoiceChk.BookNo
                    
                }).ToListAsync();
            List<InvoicehdNotPassViewModel> invNotPassList = new List<InvoicehdNotPassViewModel>();
            for (int i = 0; i < qurey.Count(); i++)
            {
                InvoicehdNotPassViewModel invNotPass = new InvoicehdNotPassViewModel();
                invNotPass.AccNo = qurey[i].AccNo;
                invNotPass.VictimNo = qurey[i].VictimNo;
                invNotPass.AppNo = qurey[i].AppNo;
                invNotPass.IdInvhd = qurey[i].IdInvhd;
                invNotPass.Mainconsider = qurey[i].Mainconsider;
                invNotPass.VictimType = qurey[i].VictimType;
                invNotPass.Hosid = qurey[i].Hosid;
                invNotPass.BookNo = qurey[i].BookNo;
                invNotPass.ReceiptNo = qurey[i].ReceiptNo;
                invNotPass.Suminv = qurey[i].Suminv;
                invNotPass.Takendate = qurey[i].Takendate;
                invNotPass.Takentime = qurey[i].Takentime;
                invNotPass.Dispensedate = qurey[i].Dispensedate;
                invNotPass.Dispensetime = qurey[i].Dispensetime;
                invNotPass.InvoiceStatus = qurey[i].invoiceStatus;
                invNotPassList.Add(invNotPass);
            }
            
            return invNotPassList;
        }

        public async Task<IclaimCheckDocuments> GetDocumentCheck(string accNo, int victimNo, int appNo)
        {   
            return await digitalclaimContext.IclaimCheckDocuments.Where(w => w.AccNo == accNo && w.VictimNo == victimNo && w.AppNo == appNo).FirstOrDefaultAsync();
        }

    }

}
