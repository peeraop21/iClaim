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

    }


    public class AccidentService : IAccidentService
    {
        private readonly RvpaccidentContext rvpAccidentContext;
        private readonly IpolicyContext ipolicyContext;
        private readonly RvpofficeContext rvpOfficeContext;

        public AccidentService(RvpaccidentContext rvpAccidentContext, IpolicyContext ipolicyContext, RvpofficeContext rvpOfficeContext)
        {
            this.rvpAccidentContext = rvpAccidentContext;
            this.ipolicyContext = ipolicyContext;
            this.rvpOfficeContext = rvpOfficeContext;
        }

        public async Task<List<AccidentViewModel>> GetAccident(string userToken)
        {
            var userIdCard = await ipolicyContext.DirectPolicyKyc.Where(w => w.LineId == userToken).Select(s => s.IdcardNo).FirstOrDefaultAsync();           
            var accLineNo = GetLineAccNo(userIdCard);
            var accHosNo = GetHosAccNo(userIdCard);
            var accLineList = await rvpAccidentContext.TbAccidentMasterLine.Where(w => accLineNo.Contains(w.EaAccNo)).Select(s  => new {s.EaAccNo,s.EaAccDate }).ToListAsync();
            var accHosList = await rvpOfficeContext.HosAccident.Where(w => accHosNo.Contains(w.AccNo)).Select(s => new { s.AccNo, s.DateAcc }).ToListAsync();
            var accViewModelList = new List<AccidentViewModel>();
            foreach (var acc in accLineList)
            {
                var accVwModel = new AccidentViewModel();
                accVwModel.AccNo = acc.EaAccNo;
                accVwModel.ClaimNo = await rvpOfficeContext.HosApproval.Where(w => w.AccNo == acc.EaAccNo).Select(s => s.ClaimNo).FirstOrDefaultAsync();
                accVwModel.StringAccNo = acc.EaAccNo.ToString().Replace("/", "-");
                accVwModel.AccDate = acc.EaAccDate ?? DateTime.Now;                
                accVwModel.StringAccDate = accVwModel.AccDate.ToString().Replace("12:00:00 AM", " ");
                accVwModel.Car = await rvpAccidentContext.TbAccidentMasterLineCar.Where(w => w.EaAccNo == acc.EaAccNo).Select(s => s.EaCarLicense).ToListAsync();
                accVwModel.Channel = "LINE";
                accViewModelList.Add(accVwModel);
            }
            foreach (var acc in accHosList)
            {
                var accVwModel = new AccidentViewModel();
                accVwModel.AccNo = acc.AccNo;
                accVwModel.ClaimNo = await rvpOfficeContext.HosApproval.Where(w => w.AccNo == acc.AccNo).Select(s => s.ClaimNo).FirstOrDefaultAsync();
                accVwModel.StringAccNo = acc.AccNo.ToString().Replace("/", "-");
                accVwModel.AccDate = acc.DateAcc ?? DateTime.Now;
                accVwModel.StringAccDate = accVwModel.AccDate.ToString().Replace("12:00:00 AM", " ");
                accVwModel.Car = await rvpOfficeContext.HosCarAccident.Where(w => w.AccNo == acc.AccNo).Select(s => s.CarLicense).ToListAsync();               
                accVwModel.Channel = "HOS";
                accViewModelList.Add(accVwModel);
            }
            return accViewModelList.OrderByDescending(o => o.AccDate).ThenByDescending(o => o.AccNo).ToList();
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
