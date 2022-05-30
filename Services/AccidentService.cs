using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.AccidentModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.EFCore.RvpOfficeModels;
using DataAccess.EFCore.ClaimDataModels;
using DataAccess.EFCore.DigitalClaimModels;
using DataAccess.EFCore.PVRModels;
using System.ComponentModel.DataAnnotations;
using Services.Models;
using Microsoft.AspNetCore.Http;

namespace Services
{
    public interface IAccidentService
    {
        Task<List<Accident>> GetAccidentByIdLine(string userToken);
        Task<Victim> GetAccidentVictim(string accNo,  string userIdCard,int victimNo);
        Task<CarHasPolicy> GetAccidentCar(string accNo);
        Task<AccidentPDF> GetAccidentForGenPDF(string accNo, int victimNo, int appNo);
        Task<List<CarEpolicy>> GetEpoliciesByIdCardAsync(string idCardNo);
        Task<string> AddAsync(HosAccident hosAccident, HosCarAccident hosCarAccident, HosVicTimAccident hosVicTimAccident, string ip);
        Task<int> GetEpolicyCountAsync(string idCardNo);
        Task<Address> GetLastCurrentAddressByIdCardNo(string idCardNo);
    }


    public class AccidentService : IAccidentService
    {
        private readonly RvpaccidentContext rvpAccidentContext;
        private readonly IpolicyContext ipolicyContext;
        private readonly RvpofficeContext rvpOfficeContext;
        private readonly IApprovalService approvalService;
        private readonly ClaimDataContext claimDataContext;
        private readonly DigitalclaimContext digitalclaimContext;
        private readonly PVRContext pvrContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AccidentService(RvpaccidentContext rvpAccidentContext, IpolicyContext ipolicyContext, RvpofficeContext rvpOfficeContext, IApprovalService approvalService, ClaimDataContext claimDataContext, DigitalclaimContext digitalclaimContext, PVRContext pvrContext, IHttpContextAccessor httpContextAccessor)
        {
            this.rvpAccidentContext = rvpAccidentContext;
            this.ipolicyContext = ipolicyContext;
            this.rvpOfficeContext = rvpOfficeContext;
            this.approvalService = approvalService;
            this.claimDataContext = claimDataContext;
            this.digitalclaimContext = digitalclaimContext;
            this.pvrContext = pvrContext;
        }

        public async Task<List<Accident>> GetAccidentByIdLine(string userToken)
        {
            if (!string.IsNullOrEmpty(userToken))
            {
                var _kyc = await ipolicyContext.DirectPolicyKyc.Where(w => w.LineId == userToken && w.Status == "Y").Select(s => new { s.IdcardNo, s.Kycno }).OrderByDescending(o => o.Kycno).FirstOrDefaultAsync(); /*"3149900145384";*/
                var accHosList = await rvpOfficeContext.HosAccident
                    .Join(rvpOfficeContext.HosVicTimAccident, accVic => accVic.AccNo, vic => vic.AccNo, (accVic, vic) => new { accJoinVictim = accVic, victimNo = vic.VictimNo, victimIdCard = vic.DrvSocNo, confirmed = vic.Confirmed })
                    .Where(w => w.victimIdCard == _kyc.IdcardNo && (w.confirmed == "1" || w.confirmed == "3" || w.confirmed == "Y") && DateTime.Compare((DateTime)w.accJoinVictim.DateAcc, DateTime.Today.AddMonths(-12)) >= 0)
                    .Select(s => new { s.accJoinVictim.AccNo, s.victimNo, s.accJoinVictim.DateAcc, s.accJoinVictim.AccPlace, s.accJoinVictim.AccProv, s.accJoinVictim.AccNature, s.accJoinVictim.TimeAcc, s.accJoinVictim.BranchId })
                    .ToListAsync();

                var accList = new List<Accident>();

                for (int i = 0; i < accHosList.Count; i++)
                {
                    var acc = new Accident();
                    acc.AccNo = accHosList[i].AccNo;
                    acc.VictimNo = accHosList[i].victimNo;
                    acc.BranchId = accHosList[i].BranchId;
                    acc.LastClaim = await approvalService.GetApprovalByAccNo(accHosList[i].AccNo, accHosList[i].victimNo);
                    acc.StringAccNo = accHosList[i].AccNo.ToString().Replace("/", "-");
                    acc.AccDate = accHosList[i].DateAcc ?? DateTime.Now;
                    acc.StringAccDate = acc.AccDate.ToString("dd/MM/yyyy") + " เวลา " + accHosList[i].TimeAcc.Replace(".", ":") + " น.";
                    acc.StringAccDateSearch = acc.AccDate.ToString("dd/MM/yyyy");
                    acc.AccNature = accHosList[i].AccNature;
                    acc.PlaceAcc = accHosList[i].AccPlace;
                    acc.ProvAcc = await rvpOfficeContext.Changwat.Where(w => w.Changwatshortname == accHosList[i].AccProv).Select(s => s.Changwatname).FirstOrDefaultAsync();
                    acc.Car = await rvpOfficeContext.HosCarAccident.Where(w => w.AccNo == accHosList[i].AccNo).Select(s => s.CarLicense).ToListAsync();
                    acc.CureRightsBalance = await approvalService.GetRightsBalance(accHosList[i].AccNo, accHosList[i].victimNo, "CureRights");
                    acc.CrippledRightsBalance = await approvalService.GetRightsBalance(accHosList[i].AccNo, accHosList[i].victimNo, "CrippledRights");
                    acc.CountHosApp = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == accHosList[i].AccNo && w.VictimNo == accHosList[i].victimNo).CountAsync();
                    acc.CountNotify = await digitalclaimContext.IclaimApproval.Where(w => w.AccNo == accHosList[i].AccNo && w.VictimNo == accHosList[i].victimNo && (w.Status == 2 || w.Status == 4)).CountAsync();
                    accList.Add(acc);
                }
                return accList.OrderByDescending(o => o.AccDate).ThenByDescending(o => o.AccNo).ToList();
            }
            else
            {
                return null;
            }
            
        }

        public async Task<Victim> GetAccidentVictim(string accNo, string userIdCard, int victimNo)
        {
            var victim = new Victim();
            var accVic = await (from hs in rvpOfficeContext.HosVicTimAccident
                                join ch in rvpOfficeContext.Changwat on hs.Province equals ch.Changwatshortname into result1
                                from hschi in result1.DefaultIfEmpty()
                                join a in rvpOfficeContext.Amphur on new { key1 = hs.Province, key2 = hs.District } equals new { key1 = a.Changwatshortname, key2 = a.Amphurid } into result2
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
            var kyc = await ipolicyContext.DirectPolicyKyc.Where(w => w.IdcardNo == userIdCard && w.Status == "Y").OrderByDescending(o => o.Kycno).FirstOrDefaultAsync();
            if (accVic == null)
            {
                return victim;
            }
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
            victim.AccHomeId = accVic.HomeId;
            victim.VictimIs = accVic.VictimIs;
            victim.VictimType = accVic.VictimType;
            victim.DetailBroken = accVic.DetailBroken;
            return victim;
        }

        public async Task<CarHasPolicy> GetAccidentCar(string accNo)
        {
            var carVwModel = new CarHasPolicy();
            var query = await rvpOfficeContext.HosCarAccident.Where(w => w.AccNo == accNo).Select(s => new { s.FoundCarLicense, s.FoundChassisNo, s.FoundPolicyNo }).FirstOrDefaultAsync();
            carVwModel.FoundCarLicense = query.FoundCarLicense;
            carVwModel.FoundChassisNo = query.FoundChassisNo;
            carVwModel.FoundPolicyNo = query.FoundPolicyNo;
            return carVwModel;
        }

        public async Task<AccidentPDF> GetAccidentForGenPDF(string accNo, int victimNo, int appNo)
        {
            var result = new AccidentPDF();
            var query = await rvpOfficeContext.HosAccident.Where(w => w.AccNo == accNo).Select(s => new {s.AccNo, s.DateAcc, s.TimeAcc, s.AccPlace, s.AccProv }).FirstOrDefaultAsync();
            result.AccNo = query.AccNo;
            result.DateAccString = query.DateAcc.Value.Date.ToString("dd-MM-yyyy");
            result.DateAcc = query.DateAcc;
            result.TimeAcc = query.TimeAcc;
            result.AccPlace = query.AccPlace;
            result.AccProv = await rvpOfficeContext.Changwat.Where(w => w.Changwatshortname == query.AccProv).Select(s => s.Changwatname).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<CarEpolicy>> GetEpoliciesByIdCardAsync(string idCardNo)
        {
            return await pvrContext.Epolicy.Where(w => w.Idcard == idCardNo && w.Status == "A" && w.Startdate <=  DateTime.Now.Date && w.Enddate >= DateTime.Now)
                .Select(s => new CarEpolicy {
                    PolicyNo = s.Policyno,
                    CarLicense = s.Carno,
                    CarProvince = s.Carchangwat,
                    CarTankNo = s.Cartankno,
                    EngineSize = s.Enginesize,
                    Marque = s.Marque,
                    StartDate = s.Startdate.Value.ToString("dd/MM/yyyy"), 
                    StartTime = s.Starttime,
                    EndDate = s.Enddate.Value.ToString("dd/MM/yyyy"),
                    EndTime = s.Starttime,
                    CarTypeId = s.Pcartype
                })
                .ToListAsync();
        }

        public async Task<string> AddAsync(HosAccident hosAccident, HosCarAccident hosCarAccident, HosVicTimAccident hosVicTimAccident, string ip)
        {
            hosAccident.AccNo = await rvpOfficeContext.HosAccident.Select(s => RvpofficeContext.fnGenerateAccNo(hosAccident.BranchId)).FirstOrDefaultAsync();
            hosAccident.HosAccNo = hosAccident.AccNo;
            hosAccident.UserId = hosVicTimAccident.UserId;
            hosAccident.UserIp = ip;           
            await rvpOfficeContext.HosAccident.AddAsync(hosAccident);

            hosCarAccident.UserId = hosVicTimAccident.UserId;
            hosCarAccident.AccNo = hosAccident.AccNo;
            hosCarAccident.BranchId = hosAccident.BranchId;
            await rvpOfficeContext.HosCarAccident.AddAsync(hosCarAccident);

            hosVicTimAccident.AccNo = hosAccident.AccNo;
            hosVicTimAccident.BranchId = hosAccident.BranchId;
            hosVicTimAccident.Sex = await rvpOfficeContext.Prefix.Where(w => w.Titlename == hosVicTimAccident.Prefix.Trim()).Select(s => s.Sex).FirstOrDefaultAsync();
            hosVicTimAccident.Age = await GetAgeAsync(hosVicTimAccident.BirthDate.Value);
            await rvpOfficeContext.HosVicTimAccident.AddAsync(hosVicTimAccident);

            HosAccidentCheck hosAccidentCheck = new HosAccidentCheck();
            hosAccidentCheck.AccNo = hosAccident.AccNo;
            hosAccidentCheck.StateNo = 1;
            hosAccidentCheck.Status = 101;
            hosAccidentCheck.InsertDate = DateTime.Now;
            hosAccidentCheck.RecordBy = hosVicTimAccident.UserId;
            hosAccidentCheck.Ip = ip;
            hosAccidentCheck.BranchId = hosAccident.BranchId;
            await rvpOfficeContext.HosAccidentCheck.AddAsync(hosAccidentCheck);
          

            return hosAccident.AccNo;
        }

        private async Task<short> GetAgeAsync(DateTime birthDay)
        {
            var age = DateTime.Now.Year - birthDay.Year;
            if (DateTime.Now < birthDay.AddYears(age))
                age--;
            return short.Parse(age.ToString());
        }

        public async Task<int> GetEpolicyCountAsync(string idCardNo)
        {
            return await pvrContext.Epolicy.Where(w => w.Idcard == idCardNo && w.Status == "A" && w.Startdate <= DateTime.Now.Date && w.Enddate >= DateTime.Now).CountAsync();
        }

        ///// สร้าง function สำหรับ query ข้อมูลที่อยู่ปัจจุบันจากรับแจ้งล่าสุด
        public async Task<Address> GetLastCurrentAddressByIdCardNo(string idCardNo)
        {
            return await rvpOfficeContext.HosVicTimAccident.Where(w => w.DrvSocNo == idCardNo).Select(s => new Address
            {
                HouseNo = s.HomeId,
                Moo = s.Moo,
                Soi = s.Soi,
                Road = s.Road,
                SubDistrict = s.Tumbol,
                District = s.District,
                Province = s.Province,
                LastDate = s.LastUpdate
            }).OrderByDescending(o => o.LastDate).FirstOrDefaultAsync();
        }

    }
}
