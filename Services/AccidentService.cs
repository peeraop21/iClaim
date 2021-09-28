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
using DataAccess.EFCore.DigitalClaimModels;

namespace Services
{
    public interface IAccidentService
    {
        Task<List<AccidentViewModel>> GetAccident(string userToken);
        Task<VictimtViewModel> GetAccidentVictim(string accNo, string channal, string userIdCard,int victimNo);
        Task<CarViewModel> GetAccidentCar(string accNo, string channal);
        Task<AccidentPDFViewModel> GetAccidentForGenPDF(string accNo, int victimNo, int appNo);

    }


    public class AccidentService : IAccidentService
    {
        private readonly RvpaccidentContext rvpAccidentContext;
        private readonly IpolicyContext ipolicyContext;
        private readonly RvpofficeContext rvpOfficeContext;
        private readonly IApprovalService approvalService;
        private readonly ClaimDataContext claimDataContext;
        private readonly DigitalclaimContext digitalclaimContext;

        public AccidentService(RvpaccidentContext rvpAccidentContext, IpolicyContext ipolicyContext, RvpofficeContext rvpOfficeContext, IApprovalService approvalService, ClaimDataContext claimDataContext, DigitalclaimContext digitalclaimContext)
        {
            this.rvpAccidentContext = rvpAccidentContext;
            this.ipolicyContext = ipolicyContext;
            this.rvpOfficeContext = rvpOfficeContext;
            this.approvalService = approvalService;
            this.claimDataContext = claimDataContext;
            this.digitalclaimContext = digitalclaimContext;
        }

        public async Task<List<AccidentViewModel>> GetAccident(string userToken)
        {
            var userIdCard = await ipolicyContext.DirectPolicyKyc.Where(w => w.LineId == userToken).Select(s => s.IdcardNo).FirstOrDefaultAsync(); /*"3149900145384";*/
            //var accLineNo = GetLineAccNo(userIdCard);
            //var accHosNo = GetHosAccNo(userIdCard);
            //var mockQueryHos = await rvpOfficeContext.HosAccident
            //    .Join(rvpOfficeContext.HosVicTimAccident, accVic => accVic.AccNo, vic => vic.AccNo, (accVic, vic) => new { accJoinVictim = accVic, victimNo = vic.VictimNo, victimIdCard = vic.DrvSocNo })
            //    .Where(w => w.victimIdCard == userIdCard).Select(s => new { s.accJoinVictim.AccNo, s.victimNo, s.accJoinVictim.DateAcc, s.accJoinVictim.AccPlace }).ToListAsync();

            //var accLineList = await rvpAccidentContext.TbAccidentMasterLine.Where(w => accLineNo.Contains(w.EaAccNo)).Select(s => new { s.EaAccNo, s.EaAccDate, s.EaAccPlace }).ToListAsync();
            var accLineList = await rvpAccidentContext.TbAccidentMasterLine
                .Join(rvpAccidentContext.TbAccidentMasterLineVictim, accVic => accVic.EaAccNo, vic => vic.EaAccno, (accVic, vic) => new { accJoinVictim = accVic, victimNo = vic.EaVictimNo, victimIdCard = vic.EaIdCardVictim })
                .Where(w => w.victimIdCard == userIdCard).Select(s => new { s.accJoinVictim.EaAccNo, s.victimNo, s.accJoinVictim.EaAccDate, s.accJoinVictim.EaAccPlace }).ToListAsync();
            //var accHosList = await rvpOfficeContext.HosAccident.Where(w => accHosNo.Contains(w.AccNo)).Select(s => new { s.AccNo, s.DateAcc, s.AccPlace }).ToListAsync();
            var accHosList = await rvpOfficeContext.HosAccident
                .Join(rvpOfficeContext.HosVicTimAccident, accVic => accVic.AccNo, vic => vic.AccNo, (accVic, vic) => new { accJoinVictim = accVic, victimNo = vic.VictimNo, victimIdCard = vic.DrvSocNo })
                .Where(w => w.victimIdCard == userIdCard).Select(s => new { s.accJoinVictim.AccNo, s.victimNo, s.accJoinVictim.DateAcc, s.accJoinVictim.AccPlace }).ToListAsync();

            var accViewModelList = new List<AccidentViewModel>();
            //var sum = rvpOfficeContext.HosApproval.Where(w => w.ClaimNo == null).Sum(s => s.SumMoney);
            
            foreach (var acc in accLineList)
            {

                var accVwModel = new AccidentViewModel();
                accVwModel.AccNo = acc.EaAccNo;
                accVwModel.VictimNo = acc.victimNo;
                accVwModel.LastClaim = await approvalService.GetApprovalByAccNo(acc.EaAccNo, acc.victimNo);
                accVwModel.StringAccNo = acc.EaAccNo.ToString().Replace("/", "-");
                accVwModel.AccDate = acc.EaAccDate ?? DateTime.Now;
                accVwModel.StringAccDate = accVwModel.AccDate.ToString().Replace("12:00:00 AM", " ");
                accVwModel.PlaceAcc = acc.EaAccPlace;
                accVwModel.Car = await rvpAccidentContext.TbAccidentMasterLineCar.Where(w => w.EaAccNo == acc.EaAccNo).Select(s => s.EaCarLicense).ToListAsync();
                accVwModel.Channel = "LINE";
                accVwModel.CureRightsBalance = await approvalService.GetRightsBalance(acc.EaAccNo, acc.victimNo, "CureRights");
                accVwModel.CrippledRightsBalance = await approvalService.GetRightsBalance(acc.EaAccNo, acc.victimNo, "CrippledRights");
                accVwModel.CountHosApp = await digitalclaimContext.HosApproval.Where(w => w.AccNo == acc.EaAccNo).CountAsync();
                accViewModelList.Add(accVwModel);
            }
            foreach (var acc in accHosList)
            {
                
                var accVwModel = new AccidentViewModel();
                accVwModel.AccNo = acc.AccNo;
                accVwModel.VictimNo = acc.victimNo;
                accVwModel.LastClaim = await approvalService.GetApprovalByAccNo(acc.AccNo, acc.victimNo);
                accVwModel.StringAccNo = acc.AccNo.ToString().Replace("/", "-");
                accVwModel.AccDate = acc.DateAcc ?? DateTime.Now;
                accVwModel.StringAccDate = accVwModel.AccDate.ToString().Replace("12:00:00 AM", " ");
                accVwModel.PlaceAcc = acc.AccPlace;
                accVwModel.Car = await rvpOfficeContext.HosCarAccident.Where(w => w.AccNo == acc.AccNo).Select(s => s.CarLicense).ToListAsync();
                accVwModel.Channel = "HOSPITAL";                
                accVwModel.CureRightsBalance = await approvalService.GetRightsBalance(acc.AccNo, acc.victimNo, "CureRights");
                accVwModel.CrippledRightsBalance = await approvalService.GetRightsBalance(acc.AccNo, acc.victimNo, "CrippledRights");
                accVwModel.CountHosApp = await digitalclaimContext.HosApproval.Where(w => w.AccNo == acc.AccNo && w.VictimNo == acc.victimNo).CountAsync();
                accViewModelList.Add(accVwModel);
            }
            return accViewModelList.OrderByDescending(o => o.AccDate).ThenByDescending(o => o.AccNo).ToList();
        }

        public async Task<VictimtViewModel> GetAccidentVictim(string accNo, string channal, string userIdCard, int victimNo)
        {
            var vicVwModel = new VictimtViewModel();
            if (channal == "LINE")
            {                                
                var vic = await rvpAccidentContext.TbAccidentMasterLineVictim.Where(w => w.EaIdCardVictim == userIdCard && w.EaAccno == accNo).Select(s => new { s.EaPrefixVictim, s.EaFnameVictim, s.EaLnameVictim, s.EaSexVictim, s.EaAgeVictim, s.EaPhoneNumber }).FirstOrDefaultAsync();
                vicVwModel.Fname = vic.EaFnameVictim;
                vicVwModel.Lname = vic.EaLnameVictim;
                vicVwModel.Prefix = vic.EaPrefixVictim;
                vicVwModel.Age = vic.EaAgeVictim;
                vicVwModel.Sex = vic.EaSexVictim;
                vicVwModel.TelNo = vic.EaPhoneNumber;
            }
            else if (channal == "HOSPITAL")
            {

                var vic = await (from hs in rvpOfficeContext.HosVicTimAccident
                            join ch in rvpOfficeContext.Changwat on hs.Province equals ch.Changwatshortname into result1
                            from hschi in result1.DefaultIfEmpty()
                            join a in rvpOfficeContext.Amphur on new { key1 = hs.Province, key2 = hs.District} equals new { key1 = a.Changwatshortname, key2 = a.Amphurid}  into result2
                            from hsai in result2.DefaultIfEmpty()
                            join t in rvpOfficeContext.Tumbol on new { key1 = hs.Province, key2 = hs.District, key3 = hs.Tumbol } equals new { key1 = t.Changwatshortname, key2 = t.Amphurid, key3 = t.Tumbolid } into result3
                            from hsti in result3.DefaultIfEmpty()
                            where hs.AccNo == accNo && hs.DrvSocNo == userIdCard
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
                                ChangwatName = hschi.Changwatname
                            }).FirstOrDefaultAsync();
                            
                //var vic1 = await rvpOfficeContext.HosVicTimAccident
                //    .Join(rvpOfficeContext.Tumbol, vicT => vicT.Tumbol, t => t.Tumbolid, (vicT, t) => new { victimJoinTumbol = vicT, tumbolName = t.Tumbolname, tumbolId = t.Tumbolid })
                //    .Join(rvpOfficeContext.Amphur, vicA => vicA.victimJoinTumbol.District, amp => amp.Amphurid, (vicA, amp) => new { victimJoinAmphur = vicA, amphurName = amp.Amphurname, amphurId = amp.Amphurid })
                //    .Join(rvpOfficeContext.Changwat, vicC => vicC.victimJoinAmphur.victimJoinTumbol.Province, chw => chw.Changwatshortname, (vicC, chw) => new { victimJoinChangwat = vicC, changwatName = chw.Changwatname, changwatShort = chw.Changwatshortname })
                //    .Where(w => w.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.DrvSocNo == userIdCard
                //    && w.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.AccNo == accNo
                //    && w.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Tumbol == w.victimJoinChangwat.victimJoinAmphur.tumbolId
                //    && w.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.District == w.victimJoinChangwat.amphurId
                //    && w.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Province == w.changwatShort)
                //    .Select(s => new
                //    {
                //        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.AccNo,
                //        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.VictimNo,
                //        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.DrvSocNo,
                //        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Fname,
                //        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Lname,
                //        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Prefix,
                //        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Age,
                //        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Sex,
                //        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.TelNo,
                //        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.HomeId,
                //        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Moo,
                //        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Soi,
                //        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Road,
                //        s.victimJoinChangwat.victimJoinAmphur.tumbolId,
                //        s.victimJoinChangwat.victimJoinAmphur.tumbolName,
                //        s.victimJoinChangwat.amphurId,
                //        s.victimJoinChangwat.amphurName,
                //        s.changwatShort,
                //        s.changwatName,
                //        s.victimJoinChangwat.victimJoinAmphur.victimJoinTumbol.Zipcode
                //    }).FirstOrDefaultAsync();
                //var victest = await rvpOfficeContext.HosVicTimAccident
                //    .GroupJoin(rvpOfficeContext.Changwat, jchw => jchw.Province, chw => chw.Changwatshortname, (jchw, chw) => new { joinChangwat = jchw, changwat = chw }).SelectMany(x => x.changwat.DefaultIfEmpty(), (x, chw) => new { vicC = x.joinChangwat, changwatName = chw.Changwatname, changwatShort = chw.Changwatshortname })
                //    .GroupJoin(rvpOfficeContext.Amphur, jamp => jamp.vicC.District, amp => amp.Amphurid, (jamp, amp) => new { joinAmphur = jamp, amphur = amp }).SelectMany(x => x.amphur.DefaultIfEmpty(), (x, amp) => new { vicA = x.joinAmphur, amphurName = amp.Amphurname, amphurId = amp.Amphurid })
                //    .GroupJoin(rvpOfficeContext.Tumbol, jtum => jtum.vicA.vicC.Tumbol, t => t.Tumbolid, (jtum, t) => new { joinTumbol = jtum, tumbol = t }).SelectMany(x => x.tumbol.DefaultIfEmpty(), (x, t) => new { vicT = x.joinTumbol, tumbolName = t.Tumbolname, tumbolId = t.Tumbolid, zipcode = t.Zipcode })
                //    .Where(w => (w.vicT.vicA.vicC.DrvSocNo == userIdCard || w.vicT.vicA.vicC.VictimNo == victimNo)
                //    && w.vicT.vicA.vicC.AccNo == accNo)
                //    .Select(s => new {
                //        s.vicT.vicA.vicC.AccNo,
                //        s.vicT.vicA.vicC.VictimNo,
                //        s.vicT.vicA.vicC.DrvSocNo,
                //        s.vicT.vicA.vicC.Fname,
                //        s.vicT.vicA.vicC.Lname,
                //        s.vicT.vicA.vicC.Prefix,
                //        s.vicT.vicA.vicC.Age,
                //        s.vicT.vicA.vicC.Sex,
                //        s.vicT.vicA.vicC.TelNo,
                //        s.vicT.vicA.vicC.HomeId,
                //        s.vicT.vicA.vicC.Moo,
                //        s.vicT.vicA.vicC.Soi,
                //        s.vicT.vicA.vicC.Road,
                //        s.vicT.vicA.vicC.Tumbol,
                //        s.tumbolName,
                //        s.vicT.vicA.vicC.District,
                //        s.vicT.amphurName,
                //        s.vicT.vicA.vicC.Province,
                //        s.vicT.vicA.changwatShort,
                //        s.vicT.vicA.vicC.Zipcode,
                //        s.zipcode
                //    })
                //    .FirstOrDefaultAsync();
                //var vic = await rvpOfficeContext.HosVicTimAccident
                //    .GroupJoin(rvpOfficeContext.Tumbol, jtum => jtum.Tumbol, t => t.Tumbolid, (jtum, t) => new { victimJoinTumbol = jtum, tumbolTb = t }).SelectMany(x => x.tumbolTb.DefaultIfEmpty(), (x, t) => new { vicT = x.victimJoinTumbol, tumbolName = t.Tumbolname, tumbolId = t.Tumbolid })
                //    .GroupJoin(rvpOfficeContext.Amphur, jamp => jamp.vicT.District, amp => amp.Amphurid, (jamp, amp) => new { victimJoinAmphur = jamp, amphurTb = amp }).SelectMany(x => x.amphurTb.DefaultIfEmpty(), (x, amp) => new { vicA = x.victimJoinAmphur, amphurName = amp.Amphurname, amphurId = amp.Amphurid })
                //    .GroupJoin(rvpOfficeContext.Changwat, jchw => jchw.vicA.vicT.Province, chw => chw.Changwatshortname, (jchw, chw) => new { victimJoinChangwat = jchw, changwatTb = chw }).SelectMany(x => x.changwatTb.DefaultIfEmpty(), (x, chw) => new { vicC = x.victimJoinChangwat, changwatName = chw.Changwatname, changwatShort = chw.Changwatshortname })
                //    .Where(w => (w.vicC.vicA.vicT.DrvSocNo == userIdCard || w.vicC.vicA.vicT.VictimNo == victimNo)
                //    && w.vicC.vicA.vicT.AccNo == accNo
                //    && w.vicC.vicA.vicT.Tumbol == w.vicC.vicA.tumbolId
                //    && w.vicC.vicA.vicT.District == w.vicC.amphurId
                //    && w.vicC.vicA.vicT.Province == w.changwatShort)
                //    .Select(s => new {
                //        s.vicC.vicA.vicT.AccNo,
                //        s.vicC.vicA.vicT.VictimNo,
                //        s.vicC.vicA.vicT.DrvSocNo,
                //        s.vicC.vicA.vicT.Fname,
                //        s.vicC.vicA.vicT.Lname,
                //        s.vicC.vicA.vicT.Prefix,
                //        s.vicC.vicA.vicT.Age,
                //        s.vicC.vicA.vicT.Sex,
                //        s.vicC.vicA.vicT.TelNo,
                //        s.vicC.vicA.vicT.HomeId,
                //        s.vicC.vicA.vicT.Moo,
                //        s.vicC.vicA.vicT.Soi,
                //        s.vicC.vicA.vicT.Road,
                //        s.vicC.vicA.tumbolId,
                //        s.vicC.vicA.tumbolName,
                //        s.vicC.amphurId,
                //        s.vicC.amphurName,
                //        s.changwatShort,
                //        s.changwatName,
                //        s.vicC.vicA.vicT.Zipcode
                //    })
                //    .FirstOrDefaultAsync();
                if (vic == null)
                {
                    return vicVwModel;
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
                vicVwModel.Tumbol = vic.TumbolId;
                vicVwModel.TumbolName = vic.TumbolName;
                vicVwModel.District = vic.AmphurId;
                vicVwModel.DistrictName = vic.AmphurName;
                vicVwModel.Province = vic.ChangwatShort;
                vicVwModel.ProvinceName = vic.ChangwatName;
                vicVwModel.Zipcode = vic.Zipcode;


            }
            return vicVwModel;
        }

        public async Task<CarViewModel> GetAccidentCar(string accNo, string channal)
        {
            var carVwModel = new CarViewModel();
            if (channal == "LINE")
            {
                var query = await rvpAccidentContext.TbAccidentMasterLineCar.Where(w => w.EaAccNo == accNo).Select(s => new { s.EaCarFoundCarLicense, s.EaCarFoundChassisNo, s.EaCarFoundPolicyNo }).FirstOrDefaultAsync();
                carVwModel.FoundCarLicense = query.EaCarFoundCarLicense;
                carVwModel.FoundChassisNo = query.EaCarFoundChassisNo;
                carVwModel.FoundPolicyNo = query.EaCarFoundPolicyNo;
            }
            else if (channal == "HOSPITAL")
            {               
                var query = await rvpOfficeContext.HosCarAccident.Where(w => w.AccNo == accNo).Select(s => new { s.FoundCarLicense, s.FoundChassisNo, s.FoundPolicyNo }).FirstOrDefaultAsync();
                carVwModel.FoundCarLicense = query.FoundCarLicense;
                carVwModel.FoundChassisNo = query.FoundChassisNo;
                carVwModel.FoundPolicyNo = query.FoundPolicyNo;
            }           
            return carVwModel;
        }
        private List<string> GetLineAccNo(string userIdCard)
        {          
            return rvpAccidentContext.TbAccidentMasterLineVictim.Where(w => w.EaIdCardVictim == userIdCard && w.EaAccno != null).Select(s => s.EaAccno).ToList();
        }

        private List<string> GetHosAccNo(string userIdCard)
        {
            return rvpOfficeContext.HosVicTimAccident.Where(w => w.DrvSocNo == userIdCard && w.AccNo != null).Select(s => s.AccNo).ToList();
        }

        public async Task<AccidentPDFViewModel> GetAccidentForGenPDF(string accNo, int victimNo, int appNo)
        {
            var result = new AccidentPDFViewModel();
            var query = await rvpOfficeContext.HosAccident.Where(w => w.AccNo == accNo).Select(s => new {s.AccNo, s.DateAcc, s.TimeAcc, s.AccPlace }).FirstOrDefaultAsync();
            result.AccNo = query.AccNo;
            result.DateAccString = query.DateAcc.Value.Date.ToString("dd-MM-yyyy");
            result.DateAcc = query.DateAcc;
            result.TimeAcc = query.TimeAcc;
            result.AccPlace = query.AccPlace;
            return result;
        }

        

    }
}
