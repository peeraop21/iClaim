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

namespace Services
{
    public interface IAccidentService
    {
        Task<List<AccidentViewModel>> GetAccident(string userToken);
        Task<List<VictimtViewModel>> GetAccidentVictim(string accNo, string channal, string userIdCard);
        Task<List<CarViewModel>> GetAccidentCar(string accNo, string channal);

    }


    public class AccidentService : IAccidentService
    {
        private readonly RvpaccidentContext rvpAccidentContext;
        private readonly IpolicyContext ipolicyContext;
        private readonly RvpofficeContext rvpOfficeContext;
        private readonly IApprovalService approvalService;

        public AccidentService(RvpaccidentContext rvpAccidentContext, IpolicyContext ipolicyContext, RvpofficeContext rvpOfficeContext, IApprovalService approvalService)
        {
            this.rvpAccidentContext = rvpAccidentContext;
            this.ipolicyContext = ipolicyContext;
            this.rvpOfficeContext = rvpOfficeContext;
            this.approvalService = approvalService;
        }

        public async Task<List<AccidentViewModel>> GetAccident(string userToken)
        {
            var userIdCard = await ipolicyContext.DirectPolicyKyc.Where(w => w.LineId == userToken).Select(s => s.IdcardNo).FirstOrDefaultAsync(); /*"3149900145384";*/
            var accLineNo = GetLineAccNo(userIdCard);
            var accHosNo = GetHosAccNo(userIdCard);
            var mockQueryHos = await rvpOfficeContext.HosAccident
                .Join(rvpOfficeContext.HosVicTimAccident, accVic => accVic.AccNo, vic => vic.AccNo, (accVic, vic) => new { accJoinVictim = accVic, victimNo = vic.VictimNo, victimIdCard = vic.DrvSocNo })
                .Where(w => w.victimIdCard == userIdCard).Select(s => new { s.accJoinVictim.AccNo, s.accJoinVictim.DateAcc, s.accJoinVictim.AccPlace }).ToListAsync();
            var accLineList = await rvpAccidentContext.TbAccidentMasterLine.Where(w => accLineNo.Contains(w.EaAccNo)).Select(s => new { s.EaAccNo, s.EaAccDate, s.EaAccPlace }).ToListAsync();
            var accHosList = await rvpOfficeContext.HosAccident.Where(w => accHosNo.Contains(w.AccNo)).Select(s => new { s.AccNo, s.DateAcc, s.AccPlace }).ToListAsync();
            var accViewModelList = new List<AccidentViewModel>();
            foreach (var acc in accLineList)
            {

                var accVwModel = new AccidentViewModel();
                var approvalVwModel = new ApprovalregisViewModel();
                //double[] aa = { (double)approvalVwModel.ApMoney };

                accVwModel.AccNo = acc.EaAccNo;
                accVwModel.LastClaim = await approvalService.GetApprovalByAccNo(acc.EaAccNo);
                accVwModel.StringAccNo = acc.EaAccNo.ToString().Replace("/", "-");
                accVwModel.AccDate = acc.EaAccDate ?? DateTime.Now;
                accVwModel.StringAccDate = accVwModel.AccDate.ToString().Replace("12:00:00 AM", " ");
                accVwModel.PlaceAcc = acc.EaAccPlace;
                accVwModel.Car = await rvpAccidentContext.TbAccidentMasterLineCar.Where(w => w.EaAccNo == acc.EaAccNo).Select(s => s.EaCarLicense).ToListAsync();
                accVwModel.Channel = "LINE";
                accVwModel.Rights = null;
                accViewModelList.Add(accVwModel);
            }
            foreach (var acc in accHosList)
            {
                
                var accVwModel = new AccidentViewModel();
                accVwModel.AccNo = acc.AccNo;
                accVwModel.LastClaim = await approvalService.GetApprovalByAccNo(acc.AccNo);
                accVwModel.StringAccNo = acc.AccNo.ToString().Replace("/", "-");
                accVwModel.AccDate = acc.DateAcc ?? DateTime.Now;
                accVwModel.StringAccDate = accVwModel.AccDate.ToString().Replace("12:00:00 AM", " ");
                accVwModel.PlaceAcc = acc.AccPlace;
                accVwModel.Car = await rvpOfficeContext.HosCarAccident.Where(w => w.AccNo == acc.AccNo).Select(s => s.CarLicense).ToListAsync();
                accVwModel.Channel = "HOSPITAL";
                accVwModel.Rights = null;
                accViewModelList.Add(accVwModel);
            }
            return accViewModelList.OrderByDescending(o => o.AccDate).ThenByDescending(o => o.AccNo).ToList();
        }

        public async Task<List<VictimtViewModel>> GetAccidentVictim(string accNo, string channal, string userIdCard)
        {
            var vicViewModelList = new List<VictimtViewModel>();
            if (channal == "LINE")
            {                
                var vicVwModel = new VictimtViewModel();
                var vic = await rvpAccidentContext.TbAccidentMasterLineVictim.Where(w => w.EaIdCardVictim == userIdCard && w.EaAccno == accNo).Select(s => new { s.EaPrefixVictim, s.EaFnameVictim, s.EaLnameVictim, s.EaSexVictim, s.EaAgeVictim, s.EaPhoneNumber }).FirstOrDefaultAsync();
                vicVwModel.Fname = vic.EaFnameVictim;
                vicVwModel.Lname = vic.EaLnameVictim;
                vicVwModel.Prefix = vic.EaPrefixVictim;
                vicVwModel.Age = vic.EaAgeVictim;
                vicVwModel.Sex = vic.EaSexVictim;
                vicVwModel.TelNo = vic.EaPhoneNumber;
                vicViewModelList.Add(vicVwModel);
            }
            else if (channal == "HOSPITAL")
            {
                
                var vicVwModel = new VictimtViewModel();
                /*var vic = await rvpOfficeContext.HosVicTimAccident
                    .Join(rvpOfficeContext.Tumbol, vicT => vicT.Tumbol, t => t.Tumbolid, (vicT, t) => new {victimJoinTumbol = vicT, tumbolName = t.Tumbolname, tumbolId = t.Tumbolid})
                    .Join(rvpOfficeContext.Amphur, vicA => vicA.victimJoinTumbol.District, amp => amp.Amphurid, (vicA, amp) => new {victimJoinAmphur = vicA, amphurName = amp.Amphurname, amphurId = amp.Amphurid})
                    .Join(rvpOfficeContext.Changwat, vicC => vicC.victimJoinAmphur.victimJoinTumbol.Province, chw => chw.Changwatshortname, (vicC, chw) => new {victimJoinChangwat = vicC, changwatName = chw.Changwatname, changwatShort = chw.Changwatshortname})
                    .Where(w => w.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.DrvSocNo == userIdCard
                    && w.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.AccNo == accNo
                    *//*&& w.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Tumbol == w.victimJoinChangwat.victimJoinAmphur.tumbolId
                    && w.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.District == w.victimJoinChangwat.amphurId
                    && w.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Province == w.changwatShort*//*)
                    .Select(s => new {
                        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.AccNo,
                        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.VictimNo,
                        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.DrvSocNo,
                        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Fname,
                        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Lname,
                        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Prefix,
                        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Age,
                        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Sex,
                        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.TelNo,
                        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.HomeId,
                        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Moo,
                        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Soi,
                        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Road,
                        s.victimJoinChangwat.victimJoinAmphur.tumbolId,
                        s.victimJoinChangwat.victimJoinAmphur.tumbolName,
                        s.victimJoinChangwat.amphurId,
                        s.victimJoinChangwat.amphurName,
                        s.changwatShort,
                        s.changwatName,
                        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Zipcode
                    }).FirstOrDefaultAsync();*/
                var vic = await rvpOfficeContext.HosVicTimAccident
                    .GroupJoin(rvpOfficeContext.Tumbol, jtum => jtum.Tumbol, t => t.Tumbolid, (jtum, t) => new { victimJoinTumbol = jtum, tumbolTb = t }).SelectMany(x => x.tumbolTb.DefaultIfEmpty(), (x, t) => new { vicT = x.victimJoinTumbol, tumbolName = t.Tumbolname, tumbolId = t.Tumbolid })
                    .GroupJoin(rvpOfficeContext.Amphur, jamp => jamp.vicT.District, amp => amp.Amphurid, (jamp, amp) => new { victimJoinAmphur = jamp, amphurTb = amp }).SelectMany(x => x.amphurTb.DefaultIfEmpty(), (x, amp) => new { vicA = x.victimJoinAmphur, amphurName = amp.Amphurname, amphurId = amp.Amphurid })
                    .GroupJoin(rvpOfficeContext.Changwat, jchw => jchw.vicA.vicT.Province, chw => chw.Changwatshortname, (jchw, chw) => new { victimJoinChangwat = jchw, changwatTb = chw }).SelectMany(x => x.changwatTb.DefaultIfEmpty(), (x, chw) => new { vicC = x.victimJoinChangwat, changwatName = chw.Changwatname, changwatShort = chw.Changwatshortname })
                    .Where(w => w.vicC.vicA.vicT.DrvSocNo == userIdCard
                    && w.vicC.vicA.vicT.AccNo == accNo
                    /*  && w.vicC.vicA.vicT.Tumbol == w.victimJoinChangwat.victimJoinAmphur.tumbolId
                      && w.vicC.vicA.vicT.District == w.victimJoinChangwat.amphurId
                      && w.vicC.vicA.vicT.Province == w.changwatShort*/)
                    .Select(s => new {
                        s.vicC.vicA.vicT.AccNo,
                        s.vicC.vicA.vicT.VictimNo,
                        s.vicC.vicA.vicT.DrvSocNo,
                        s.vicC.vicA.vicT.Fname,
                        s.vicC.vicA.vicT.Lname,
                        s.vicC.vicA.vicT.Prefix,
                        s.vicC.vicA.vicT.Age,
                        s.vicC.vicA.vicT.Sex,
                        s.vicC.vicA.vicT.TelNo,
                        s.vicC.vicA.vicT.HomeId,
                        s.vicC.vicA.vicT.Moo,
                        s.vicC.vicA.vicT.Soi,
                        s.vicC.vicA.vicT.Road,
                        s.vicC.vicA.tumbolId,
                        s.vicC.vicA.tumbolName,
                        s.vicC.amphurId,
                        s.vicC.amphurName,
                        s.changwatShort,
                        s.changwatName,
                        s.vicC.vicA.vicT.Zipcode
                    })
                    .FirstOrDefaultAsync();
                if (vic == null)
                {
                    return vicViewModelList;
                }
                vicVwModel.AccNo = vic.AccNo;
                vicVwModel.VictimNo = vic.VictimNo;
                vicVwModel.DrvSocNo = vic.DrvSocNo;
                vicVwModel.Fname = vic.Fname;
                vicVwModel.Lname = vic.Lname;
                vicVwModel.Prefix = vic.Prefix;
                vicVwModel.Age = vic.Age;
                vicVwModel.Sex = vic.Sex;
                vicVwModel.TelNo = vic.TelNo;
                vicVwModel.HomeId = vic.HomeId;
                vicVwModel.Moo = vic.Moo;
                vicVwModel.Soi = vic.Soi;
                vicVwModel.Road = vic.Road;
                vicVwModel.Tumbol = vic.tumbolId;
                vicVwModel.TumbolName = vic.tumbolName;
                vicVwModel.District = vic.amphurId;
                vicVwModel.DistrictName = vic.amphurName;
                vicVwModel.Province = vic.changwatShort;
                vicVwModel.ProvinceName = vic.changwatName;
                vicVwModel.Zipcode = vic.Zipcode;

                vicViewModelList.Add(vicVwModel);
            }
            return vicViewModelList;
        }

        public async Task<List<CarViewModel>> GetAccidentCar(string accNo, string channal)
        {
            var carViewModelList = new List<CarViewModel>();
            if (channal == "LINE")
            {
                var query = await rvpAccidentContext.TbAccidentMasterLineCar.Where(w => w.EaAccNo == accNo).Select(s => new { s.EaCarFoundCarLicense, s.EaCarFoundChassisNo, s.EaCarFoundPolicyNo }).FirstOrDefaultAsync();
                var carVwModel = new CarViewModel();
                carVwModel.FoundCarLicense = query.EaCarFoundCarLicense;
                carVwModel.FoundChassisNo = query.EaCarFoundChassisNo;
                carVwModel.FoundPolicyNo = query.EaCarFoundPolicyNo;
                carViewModelList.Add(carVwModel);
            }
            else if (channal == "HOSPITAL")
            {               
                var query = await rvpOfficeContext.HosCarAccident.Where(w => w.AccNo == accNo).Select(s => new { s.FoundCarLicense, s.FoundChassisNo, s.FoundPolicyNo }).FirstOrDefaultAsync();
                var carVwModel = new CarViewModel();
                carVwModel.FoundCarLicense = query.FoundCarLicense;
                carVwModel.FoundChassisNo = query.FoundChassisNo;
                carVwModel.FoundPolicyNo = query.FoundPolicyNo;
                carViewModelList.Add(carVwModel);
            }           
            return carViewModelList;
        }
        private List<string> GetLineAccNo(string userIdCard)
        {          
            return rvpAccidentContext.TbAccidentMasterLineVictim.Where(w => w.EaIdCardVictim == userIdCard && w.EaAccno != null).Select(s => s.EaAccno).ToList();
        }

        private List<string> GetHosAccNo(string userIdCard)
        {
            return rvpOfficeContext.HosVicTimAccident.Where(w => w.DrvSocNo == userIdCard && w.AccNo != null).Select(s => s.AccNo).ToList();
        }


    }
}
