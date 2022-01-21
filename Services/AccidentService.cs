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
            var accHosList = await rvpOfficeContext.HosAccident
                .Join(rvpOfficeContext.HosVicTimAccident, accVic => accVic.AccNo, vic => vic.AccNo, (accVic, vic) => new { accJoinVictim = accVic, victimNo = vic.VictimNo, victimIdCard = vic.DrvSocNo , confirmed = vic.Confirmed})
                .Where(w => w.victimIdCard == userIdCard && (w.confirmed == "1" || w.confirmed == "3" || w.confirmed == "Y"))
                .Select(s => new { s.accJoinVictim.AccNo, s.victimNo, s.accJoinVictim.DateAcc, s.accJoinVictim.AccPlace, s.accJoinVictim.AccProv, s.accJoinVictim.AccNature, s.accJoinVictim.TimeAcc, s.accJoinVictim.BranchId})
                .ToListAsync();

            var accViewModelList = new List<AccidentViewModel>();                    
            foreach (var acc in accHosList)
            {
                
                var accVwModel = new AccidentViewModel();
                accVwModel.AccNo = acc.AccNo;
                accVwModel.VictimNo = acc.victimNo;
                accVwModel.BranchId = acc.BranchId;
                accVwModel.LastClaim = await approvalService.GetApprovalByAccNo(acc.AccNo, acc.victimNo);
                accVwModel.StringAccNo = acc.AccNo.ToString().Replace("/", "-");
                accVwModel.AccDate = acc.DateAcc ?? DateTime.Now;
                accVwModel.StringAccDate = accVwModel.AccDate.ToString("dd/MM/yyyy") + " เวลา " + acc.TimeAcc.Replace(".",":") + " น.";
                accVwModel.AccNature = acc.AccNature;
                accVwModel.PlaceAcc = acc.AccPlace;
                accVwModel.ProvAcc = await rvpOfficeContext.Changwat.Where(w => w.Changwatshortname == acc.AccProv).Select(s => s.Changwatname).FirstOrDefaultAsync();
                accVwModel.Car = await rvpOfficeContext.HosCarAccident.Where(w => w.AccNo == acc.AccNo).Select(s => s.CarLicense).ToListAsync();
                accVwModel.Channel = "HOSPITAL";                
                accVwModel.CureRightsBalance = await approvalService.GetRightsBalance(acc.AccNo, acc.victimNo, "CureRights");
                accVwModel.CrippledRightsBalance = await approvalService.GetRightsBalance(acc.AccNo, acc.victimNo, "CrippledRights");
                accVwModel.CountHosApp = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == acc.AccNo && w.VictimNo == acc.victimNo).CountAsync();
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

                var accVic = await (from hs in rvpOfficeContext.HosVicTimAccident
                            join ch in rvpOfficeContext.Changwat on hs.Province equals ch.Changwatshortname into result1
                            from hschi in result1.DefaultIfEmpty()
                            join a in rvpOfficeContext.Amphur on new { key1 = hs.Province, key2 = hs.District} equals new { key1 = a.Changwatshortname, key2 = a.Amphurid}  into result2
                            from hsai in result2.DefaultIfEmpty()
                            join t in rvpOfficeContext.Tumbol on new { key1 = hs.Province, key2 = hs.District, key3 = hs.Tumbol } equals new { key1 = t.Changwatshortname, key2 = t.Amphurid, key3 = t.Tumbolid } into result3
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
                                VictimIs = hs.VicTimIs,
                                VictimType = hs.VictimType,
                                DetailBroken = hs.DetailBroken
                            }).FirstOrDefaultAsync();
                var kyc = await ipolicyContext.DirectPolicyKyc.Where(w => w.IdcardNo == userIdCard).FirstOrDefaultAsync();                
                //var address = await (from t in rvpOfficeContext.Tumbol
                //                 join a in rvpOfficeContext.Amphur on new { key1 = t.Amphurid, key2 = t.Provinceid } equals new { key1 = a.Amphurid, key2 = a.Provinceid } into result1
                //                 from ta in result1.DefaultIfEmpty()
                //                 join ch in rvpOfficeContext.Changwat on  t.Changwatshortname equals ch.Changwatshortname into result2
                //                 from tch in result2.DefaultIfEmpty()                                
                //                 where (t.Tumbolid == kyc.HomeTumbolId && t.Amphurid == kyc.HomeCityId && t.Provinceid == kyc.HomeProvinceId)
                //                 select new
                //                 {                                    
                //                     Zipcode = t.Zipcode,
                //                     TumbolId = t.Tumbolid,
                //                     TumbolName = t.Tumbolname,
                //                     AmphurId = ta.Amphurid,
                //                     AmphurName = ta.Amphurname,
                //                     ChangwatShort = tch.Changwatshortname,
                //                     ChangwatName = tch.Changwatname
                //                 }).FirstOrDefaultAsync();

                if (accVic == null)
                {
                    return vicVwModel;
                }

                vicVwModel.IdCardNo = kyc.IdcardNo;
                vicVwModel.Fname = kyc.Fname;
                vicVwModel.Lname = kyc.Lname;
                vicVwModel.Prefix = kyc.Prefix;
                vicVwModel.Age = accVic.Age;
                vicVwModel.Sex = accVic.Sex;
                vicVwModel.TelNo = kyc.MobileNo;

                vicVwModel.HomeId = accVic.HomeId;
                vicVwModel.Moo = accVic.Moo;
                vicVwModel.Soi = accVic.Soi;
                vicVwModel.Road = accVic.Road;
                vicVwModel.Tumbol = accVic.TumbolId;
                vicVwModel.TumbolName = accVic.TumbolName;
                vicVwModel.District = accVic.AmphurId;
                vicVwModel.DistrictName = accVic.AmphurName;
                vicVwModel.Province = accVic.ChangwatShort;
                vicVwModel.ProvinceName = accVic.ChangwatName;
                vicVwModel.Zipcode = accVic.Zipcode;
                vicVwModel.AccHomeId = accVic.HomeId;

                //vicVwModel.AccMoo = accVic.Moo;
                //vicVwModel.AccSoi = accVic.Soi;
                //vicVwModel.AccRoad = accVic.Road;
                //vicVwModel.AccTumbol = accVic.TumbolId;
                //vicVwModel.AccTumbolName = accVic.TumbolName;
                //vicVwModel.AccDistrict = accVic.AmphurId;
                //vicVwModel.AccDistrictName = accVic.AmphurName;
                //vicVwModel.AccProvince = accVic.ChangwatShort;
                //vicVwModel.AccProvinceName = accVic.ChangwatName;
                //vicVwModel.AccZipcode = accVic.Zipcode;

                vicVwModel.VictimIs = accVic.VictimIs;
                vicVwModel.VictimType = accVic.VictimType;
                vicVwModel.DetailBroken = accVic.DetailBroken;




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
            var query = await rvpOfficeContext.HosAccident.Where(w => w.AccNo == accNo).Select(s => new {s.AccNo, s.DateAcc, s.TimeAcc, s.AccPlace, s.AccProv }).FirstOrDefaultAsync();
            result.AccNo = query.AccNo;
            result.DateAccString = query.DateAcc.Value.Date.ToString("dd-MM-yyyy");
            result.DateAcc = query.DateAcc;
            result.TimeAcc = query.TimeAcc;
            result.AccPlace = query.AccPlace;
            result.AccProv = await rvpOfficeContext.Changwat.Where(w => w.Changwatshortname == query.AccProv).Select(s => s.Changwatname).FirstOrDefaultAsync();
            return result;
        }

        

    }
}
