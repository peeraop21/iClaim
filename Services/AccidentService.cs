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
        Task<AccidentDetail> GetAccidentDetail(string accNo);
        Task<List<CarEpolicy>> GetEpoliciesByIdCardAsync(string idCardNo);
        Task<CarEpolicy> GetEpolicyByAccNoAsync(string accNo);
        Task<string> AddAsync(HosAccident hosAccident, HosCarAccident hosCarAccident, HosVicTimAccident hosVicTimAccident, string ip);
        Task<string> EditAsync(HosAccidentCheck hosAccidentCheck, string ip);
        Task<int> GetEpolicyCountAsync(string idCardNo);
        Task<Address> GetLastCurrentAddressByIdCardNo(string idCardNo);
        Task<AccidentCheckedDetail> GetAccidentCheckedDetailAsync(string accNo);
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
                var policyNos = await pvrContext.Epolicy.Where(w => w.Idcard == _kyc.IdcardNo && w.Status == "A" && w.Startdate <= DateTime.Now.Date && w.Enddate >= DateTime.Now)
                .Select(s => s.Policyno).ToListAsync();

                var accHosList = await rvpOfficeContext.HosAccident
                    .Join(rvpOfficeContext.HosCarAccident, acc => acc.AccNo, accCar => accCar.AccNo, (acc, accCar) => new {
                        acc = acc,
                        policyNo = accCar.FoundPolicyNo
                    })
                    .Join(rvpOfficeContext.HosVicTimAccident, accJoinAccCar => accJoinAccCar.acc.AccNo, accVic => accVic.AccNo, (accJoinAccCar, accVic) => new { 
                        accJoinCarJoinVic = accJoinAccCar.acc,
                        policyNo = accJoinAccCar.policyNo,
                        victimNo = accVic.VictimNo,
                        victimIdCard = accVic.DrvSocNo,
                        confirmed = accVic.Confirmed,
                        victimIs = accVic.VicTimIs
                    })
                    .Where(w => w.victimIdCard == _kyc.IdcardNo && (w.confirmed == "1" || w.confirmed == "3" || w.confirmed == "Y") && DateTime.Compare((DateTime)w.accJoinCarJoinVic.DateAcc, DateTime.Today.AddMonths(-12)) >= 0 && w.victimIs == "ผขป" /*&& policyNos.Contains(w.policyNo)*/)
                    .Select(s => new { s.accJoinCarJoinVic.AccNo, s.victimNo, s.accJoinCarJoinVic.DateAcc, s.accJoinCarJoinVic.AccPlace, s.accJoinCarJoinVic.AccProv, s.accJoinCarJoinVic.AccNature, s.accJoinCarJoinVic.TimeAcc, s.accJoinCarJoinVic.BranchId })
                    .ToListAsync();

                var accList = new List<Accident>();
                var accChecks = await  rvpOfficeContext.HosAccidentCheck.Where(w => accHosList.Select(s => s.AccNo).Contains(w.AccNo)).ToListAsync();

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
                    acc.LastStatus = accChecks.Where(w => w.AccNo == accHosList[i].AccNo).OrderBy(o => o.StateNo).Select(s => s.Status).LastOrDefault();
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
            return await rvpOfficeContext.HosCarAccident.Where(w => w.AccNo == accNo).Select(s => new CarHasPolicy { FoundCarLicense = s.FoundCarLicense, FoundChassisNo = s.FoundChassisNo, FoundPolicyNo = s.FoundPolicyNo }).FirstOrDefaultAsync();
        }

        public async Task<AccidentDetail> GetAccidentDetail(string accNo)
        {
            var query = await rvpOfficeContext.HosAccident
                .Join(rvpOfficeContext.Changwat, acc => acc.AccProv, ch => ch.Changwatshortname, (acc, ch) => new
                {
                    accJoinChangwat = new { acc.AccNo, acc.DateAcc, acc.TimeAcc, acc.AccPlace, acc.AccProv, acc.AccDist, acc.AccSubDist, acc.AccNature },
                    changwatName = ch.Changwatname
                }).
                Join(rvpOfficeContext.Amphur, acc => acc.accJoinChangwat.AccDist, amp => amp.Amphurid, (acc, amp) => new
                {
                    accJoinChangwatJoinAmphur = acc,
                    changwatName = acc.changwatName,
                    amphurName = amp.Amphurname,
                })
                .Join(rvpOfficeContext.Tumbol, acc => acc.accJoinChangwatJoinAmphur.accJoinChangwat.AccSubDist, tumb => tumb.Tumbolid, (acc, tumb) => new
                {
                    accJoinChangwatJoinAmphurJoinTumbol = acc,
                    changwatName = acc.changwatName,
                    amphurName = acc.amphurName,
                    tumbolName = tumb.Tumbolname
                })
                .Select(s => new AccidentDetail { 
                    AccNo = s.accJoinChangwatJoinAmphurJoinTumbol.accJoinChangwatJoinAmphur.accJoinChangwat.AccNo,
                    DateAccString = s.accJoinChangwatJoinAmphurJoinTumbol.accJoinChangwatJoinAmphur.accJoinChangwat.DateAcc.Value.Date.ToString("dd-MM-yyyy"),
                    DateAcc = s.accJoinChangwatJoinAmphurJoinTumbol.accJoinChangwatJoinAmphur.accJoinChangwat.DateAcc,
                    TimeAcc = s.accJoinChangwatJoinAmphurJoinTumbol.accJoinChangwatJoinAmphur.accJoinChangwat.TimeAcc,
                    AccPlace = s.accJoinChangwatJoinAmphurJoinTumbol.accJoinChangwatJoinAmphur.accJoinChangwat.AccPlace,
                    AccProv = s.changwatName,
                    AccDist = s.amphurName,
                    AccSubDist = s.tumbolName,
                    AccNature = s.accJoinChangwatJoinAmphurJoinTumbol.accJoinChangwatJoinAmphur.accJoinChangwat.AccNature
                })
                .Where(w => w.AccNo == accNo)
                .FirstOrDefaultAsync();

            return query;
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
        public async Task<CarEpolicy> GetEpolicyByAccNoAsync(string accNo)
        {
            var policyNo = await rvpOfficeContext.HosCarAccident.Where(w => w.AccNo == accNo).Select(s => s.FoundPolicyNo).FirstOrDefaultAsync();
            var car = await pvrContext.Epolicy.Where(w => w.Policyno == policyNo && w.Status == "A" && w.Startdate <= DateTime.Now.Date && w.Enddate >= DateTime.Now)
                .Select(s => new CarEpolicy
                {
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
                .FirstOrDefaultAsync();
            car.CarProvinceName = await rvpOfficeContext.Changwat.Where(w => w.Provinceid == car.CarProvince).Select(s => s.Changwatname).FirstOrDefaultAsync();
            return car;
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

            await rvpOfficeContext.SaveChangesAsync();

            return hosAccident.AccNo;
        }
        public async Task<string> EditAsync(HosAccidentCheck hosAccidentCheck, string ip)
        {
            var lastHosAccidentCheck = await rvpOfficeContext.HosAccidentCheck.Where(w => w.AccNo == hosAccidentCheck.AccNo).Select(s => new {s.StateNo, s.BranchId}).OrderBy(o => o.StateNo).LastOrDefaultAsync();

            hosAccidentCheck.StateNo = lastHosAccidentCheck.StateNo + 1;
            hosAccidentCheck.Ip = ip;
            hosAccidentCheck.BranchId = lastHosAccidentCheck.BranchId;
            await rvpOfficeContext.HosAccidentCheck.AddAsync(hosAccidentCheck);
            await rvpOfficeContext.SaveChangesAsync();

            return hosAccidentCheck.AccNo;
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

        public async Task<AccidentCheckedDetail> GetAccidentCheckedDetailAsync(string accNo)
        {
            var accChecked = await rvpOfficeContext.HosAccidentCheck.Where(w => w.AccNo == accNo && w.Status == 102).OrderBy(o => o.StateNo)
                .Select(s => new AccidentCheckedDetail
                {
                    AccNo = s.AccNo,
                    StateNo = s.StateNo,
                    Status = s.Status,
                    CheckDate = s.InsertDate,
                    AccNatureImgCheckType = s.AccNatureImgCheckType,
                    AccNutureImageCheckComment = s.AccNutureImageCheckComment,
                    AccCarImgCheckType = s.AccCarImgCheckType,
                    AccCarImgCheckComment = s.AccCarImgCheckComment,
                    AccVictimImgCheckType = s.AccVictimImgCheckType,
                    AccVictimImgCheckComment = s.AccVictimImgCheckComment,
                })
                .LastOrDefaultAsync();
            
            if(!string.IsNullOrEmpty(accChecked.AccNatureImgCheckType) && accChecked.AccNatureImgCheckType.Length >= 3)
            {
                var types = accChecked.AccNatureImgCheckType.Split("-").Select(Int32.Parse).ToList();
                var typeNames = await rvpOfficeContext.HosAccidentCheckTypes.Where(w => types.Contains(w.TypeId) && w.IsActive == true).Select(s => new { s.TypeId, s.TypeName}).OrderBy(o => o.TypeId).ToListAsync();
                List<AccidentCheckTypeName> accChk = new List<AccidentCheckTypeName>();
                for (int i =0; i< typeNames.Count; i++)
                {
                    AccidentCheckTypeName obj = new AccidentCheckTypeName();
                    obj.Id = typeNames[i].TypeId.ToString();
                    obj.Name = typeNames[i].TypeName;
                    accChk.Add(obj);
                }
                accChecked.AccNatureImgCheckTypeName = accChk;
            }
            if (!string.IsNullOrEmpty(accChecked.AccCarImgCheckType) && accChecked.AccCarImgCheckType.Length >= 3)
            {
                var types = accChecked.AccCarImgCheckType.Split("-").Select(Int32.Parse).ToList();
                var typeNames = await rvpOfficeContext.HosAccidentCheckTypes.Where(w => types.Contains(w.TypeId) && w.IsActive == true).Select(s => new { s.TypeId, s.TypeName }).OrderBy(o => o.TypeId).ToListAsync();
                List<AccidentCheckTypeName> accChk = new List<AccidentCheckTypeName>();
                for (int i = 0; i < typeNames.Count; i++)
                {
                    AccidentCheckTypeName obj = new AccidentCheckTypeName();
                    obj.Id = typeNames[i].TypeId.ToString();
                    obj.Name = typeNames[i].TypeName;
                    accChk.Add(obj);
                }
                accChecked.AccCarImgCheckTypeName = accChk;
            }
            if (!string.IsNullOrEmpty(accChecked.AccVictimImgCheckType) && accChecked.AccVictimImgCheckType.Length >= 3)
            {
                var types = accChecked.AccVictimImgCheckType.Split("-").Select(Int32.Parse).ToList();
                var typeNames = await rvpOfficeContext.HosAccidentCheckTypes.Where(w => types.Contains(w.TypeId) && w.IsActive == true).Select(s => new { s.TypeId, s.TypeName }).OrderBy(o => o.TypeId).ToListAsync();
                List<AccidentCheckTypeName> accChk = new List<AccidentCheckTypeName>();
                for (int i = 0; i < typeNames.Count; i++)
                {
                    AccidentCheckTypeName obj = new AccidentCheckTypeName();
                    obj.Id = typeNames[i].TypeId.ToString();
                    obj.Name = typeNames[i].TypeName;
                    accChk.Add(obj);
                }
                accChecked.AccVictimImgCheckTypeName = accChk;
            }
            return accChecked;
        }

        
    }
}
