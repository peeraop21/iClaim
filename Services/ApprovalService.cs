﻿using DataAccess.EFCore.DigitalClaimModels;
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
        Task<List<ApprovalregisViewModel>> GetApproval(string accNo, int rightsType);
        Task<ClaimViewModel> GetApprovalByClaimNo(string claimNo, short victimNo, short regNo, int rightsType);
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
            dataApprovalStatusState.AppNo = hosApproval.AppNo + 1;
            dataApprovalStatusState.StateNo = 1;
            dataApprovalStatusState.InsertDate = DateTime.Now;
            dataApprovalStatusState.Status = 1;
            await digitalclaimContext.ApprovalStatusState.AddAsync(dataApprovalStatusState);
            await digitalclaimContext.SaveChangesAsync();

            return hosApproval;
        }

        public async Task<ClaimViewModel> GetApprovalByAccNo(string accNo)
        {
            var query = await rvpofficeContext.HosApproval.Where(w => w.AccNo == accNo).Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.Pt4id}).OrderByDescending(o => o.AppNo).Take(1).FirstOrDefaultAsync();
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
        public async Task<List<ApprovalregisViewModel>> GetApproval(string accNo, int rightsType)
        {

            var approvalVwMdList = new List<ApprovalregisViewModel>();
            var query = await claimDataContext.Approvalregis.Where(w => w.AccNo == accNo && w.Pt4 != null && w.Pt4 != "Compensate").Select(s => new { s.CrClaimno, s.VVictimno, s.ApRegno, s.ApRegdate, s.ApPaytype, s.ApMoney, s.AccNo, s.ApTotal, s.DailyReceiveno, s.Pt4, s.ApStatus }).ToListAsync();
            
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
                var predicateAnd = PredicateBuilder.False<DataAccess.EFCore.RvpOfficeModels.HosApproval>();
                predicateAnd = predicateAnd.And(f => f.ServiceMoney == 0 && f.ServiceMoney == null);
                predicateAnd = predicateAnd.And(f => f.CureMoney == 0 && f.CureMoney == null);
                predicateAnd = predicateAnd.And(f => f.SumMoney > 0);
                var predicateOr = PredicateBuilder.True<DataAccess.EFCore.RvpOfficeModels.HosApproval>();
                predicateOr = predicateOr.Or(f => f.CureMoney > 0);
                predicateOr = predicateOr.Or(f => f.ServiceMoney > 0);
                predicateOr = predicateOr.Or(predicateAnd);
                var predicate = PredicateBuilder.True<DataAccess.EFCore.RvpOfficeModels.HosApproval>();
                predicate = predicate.And(f => f.ClaimNo == claimNo);
                predicate = predicate.And(f => f.VictimNoClaim == victimNo);
                predicate = predicate.And(f => f.RegNoClaim == regNo);
                predicate = predicate.And(f => f.PayMore != "Y");
                predicate = predicate.And(f => f.PayMore != "B");
                predicate = predicate.And(predicateOr);


                var curemoneyQuery = await rvpofficeContext.HosApproval.Where(predicate)
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
               
                var predicate = PredicateBuilder.False<DataAccess.EFCore.RvpOfficeModels.HosApproval>();
                predicate = predicate.And(f => f.ClaimNo == claimNo);
                predicate = predicate.And(f => f.VictimNoClaim == victimNo);
                predicate = predicate.And(f => f.RegNoClaim == regNo);
                predicate = predicate.And(f => f.PayMore != "Y");
                predicate = predicate.And(f => f.PayMore != "B");
                predicate = predicate.And(f => f.CrippledMoney > 0);


                var crippledQuery = await rvpofficeContext.HosApproval.Where(predicate)
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
            var query = await rvpofficeContext.HosApproval.Where(w => w.ClaimNo == claimNo && w.VictimNoClaim == victimNo && w.RegNoClaim == regNo && w.PayMore != "Y" && w.PayMore != "B").Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.VictimNoClaim, s.RegNoClaim, s.Pt4id, s.MedicineMoney, s.PlasticMoney, s.ServiceMoney, s.RoomMoney, s.VeihcleMoney, s.CureMoney, s.DeadMoney, s.HygieneMoney, s.CrippledMoney, s.SumMoney, s.BlindCrippled, s.UnHearCrippled, s.DeafCrippled, s.LostSexualCrippled, s.LostOrganCrippled, s.LostMindCrippled, s.CrippledPermanent, s.OtherCrippled, s.CrippledComment, s.PayMore }).OrderByDescending(o => o.AppNo).Take(1).FirstOrDefaultAsync();
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
                if (claimVwModel.CrippledMoney > 0 && claimVwModel.CureMoney == 0)
                {
                    claimVwModel.SumCrippledMoney = query.CrippledMoney;
                } 
                if (claimVwModel.MedicineMoney != null && claimVwModel.PlasticMoney != null)
                {
                    claimVwModel.SumCureMoney = query.MedicineMoney + query.PlasticMoney + query.ServiceMoney + query.RoomMoney + query.VeihcleMoney;
                }
                else if (claimVwModel.CureMoney > 0)
                {
                    claimVwModel.SumCureMoney = query.CureMoney;
                }

            }
            return claimVwModel;
        }
        public async Task<List<HosApprovalViewModel>> GetHosApprovalsAsync(string accNo, int victimNo)
        {
            var query = await digitalclaimContext.HosApproval.Where(w => w.AccNo == accNo && w.VictimNo == victimNo).Select(s => new { s.AccNo, s.AppNo, s.RegDate, }).ToListAsync();
            var vwHosAppList = new List<HosApprovalViewModel>();
            if (query == null)
            {
                return vwHosAppList;
            }
            foreach (var hosApp in query) {
                var vwHosApp = new HosApprovalViewModel();
                vwHosApp.AccNo = hosApp.AccNo;
                vwHosApp.StringAccNo = hosApp.AccNo.Replace("/", "-");
                vwHosApp.AppNo = hosApp.AppNo;
                vwHosApp.RegDate = hosApp.RegDate;
                vwHosApp.StringRegDate = hosApp.RegDate.ToString().Replace("T", " ");
                var queryStatus = await digitalclaimContext.ApprovalStatusState.Join(digitalclaimContext.ApprovalStatus, appss => appss.Status, apps => apps.StatusId, (appss, apps) => new { appStatusStateJoinAppStatus = appss, statusName = apps.StatusName })
                    .Where(w => w.appStatusStateJoinAppStatus.AccNo == hosApp.AccNo && w.appStatusStateJoinAppStatus.AppNo == hosApp.AppNo).OrderByDescending(o => o.appStatusStateJoinAppStatus.InsertDate).Take(1).Select(s => new { s.statusName, s.appStatusStateJoinAppStatus.Status }).FirstOrDefaultAsync();
                vwHosApp.AppStatusName = queryStatus.statusName;
                vwHosApp.AppStatus = await GetApprovalStatus(hosApp.AccNo, hosApp.AppNo);
                vwHosAppList.Add(vwHosApp);
            }

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


        public async Task<List<ApprovalStatusViewModel>> GetApprovalStatus(string accNo, int appNo)
        {          
            var query = await digitalclaimContext.ApprovalStatus.ToListAsync();
            var appStatusState = await digitalclaimContext.ApprovalStatusState.Where(w => w.AccNo == accNo && w.AppNo == appNo).OrderByDescending(o => o.InsertDate).Take(1).FirstOrDefaultAsync();
            var appStatusVwModelList = new List<ApprovalStatusViewModel>();
            foreach(var status in query)
            {
                var appStatusVwModel = new ApprovalStatusViewModel();
                appStatusVwModel.StatusId = status.StatusId;
                appStatusVwModel.StatusName = status.StatusName;
                if(status.StatusId <= appStatusState.Status )
                {
                    appStatusVwModel.Active = true;
                }
                else
                {
                    appStatusVwModel.Active = false;
                }
                appStatusVwModelList.Add(appStatusVwModel);

            }
            return appStatusVwModelList;
        }

        

    }

}
