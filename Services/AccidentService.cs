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
            var userIdCard = await ipolicyContext.DirectPolicyKyc.Where(w => w.LineId == userToken).Select(s => s.IdcardNo).FirstOrDefaultAsync();
            var accLineNo = GetLineAccNo(userIdCard);
            var accHosNo = GetHosAccNo(userIdCard);
            var accLineList = await rvpAccidentContext.TbAccidentMasterLine.Where(w => accLineNo.Contains(w.EaAccNo)).Select(s => new { s.EaAccNo, s.EaAccDate, s.EaAccPlace }).ToListAsync();
            var accHosList = await rvpOfficeContext.HosAccident.Where(w => accHosNo.Contains(w.AccNo)).Select(s => new { s.AccNo, s.DateAcc, s.AccPlace }).ToListAsync();
            var accViewModelList = new List<AccidentViewModel>();
            foreach (var acc in accLineList)
            {

                var accVwModel = new AccidentViewModel();

                

                accVwModel.AccNo = acc.EaAccNo;
                accVwModel.LastClaim = await approvalService.GetApprovalByAccNo(acc.EaAccNo);
                accVwModel.StringAccNo = acc.EaAccNo.ToString().Replace("/", "-");
                accVwModel.AccDate = acc.EaAccDate ?? DateTime.Now;
                accVwModel.StringAccDate = accVwModel.AccDate.ToString().Replace("12:00:00 AM", " ");
                accVwModel.PlaceAcc = acc.EaAccPlace;
                accVwModel.Car = await rvpAccidentContext.TbAccidentMasterLineCar.Where(w => w.EaAccNo == acc.EaAccNo).Select(s => s.EaCarLicense).ToListAsync();
                accVwModel.Channel = "LINE";
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
                var vic = await rvpOfficeContext.HosVicTimAccident.Where(w => w.DrvSocNo == userIdCard && w.AccNo == accNo).Select(s => new { s.Fname, s.Lname, s.Prefix, s.Age, s.Sex, s.TelNo }).FirstOrDefaultAsync();
                vicVwModel.Fname = vic.Fname;
                vicVwModel.Lname = vic.Lname;
                vicVwModel.Prefix = vic.Prefix;
                vicVwModel.Age = vic.Age;
                vicVwModel.Sex = vic.Sex;
                vicVwModel.TelNo = vic.TelNo;
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
