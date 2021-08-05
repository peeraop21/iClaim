using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAccidentService
    {
        Task<List<AccidentViewModel>> GetAccident(string userToken);

    }


    public class AccidentService : IAccidentService
    {
        private readonly RvpAccidentContext rvpAccidentContext;
        private readonly IpolicyContext ipolicyContext;

        public AccidentService(RvpAccidentContext rvpAccidentContext, IpolicyContext ipolicyContext)
        {
            this.rvpAccidentContext = rvpAccidentContext;
            this.ipolicyContext = ipolicyContext;
        }

        public async Task<List<AccidentViewModel>> GetAccident(string userToken)
        {
            var userIdCard = await ipolicyContext.DirectPolicyKyc.Where(w => w.LineId == userToken).Select(s => s.IdcardNo).FirstOrDefaultAsync();           
            var accNo = GetAccNo(userIdCard);
            var query = await rvpAccidentContext.TbAccidentMasterLine.Where(w => accNo.Contains(w.EaAccNo)).Select(s  => new {s.EaTmpId,s.EaAccNo,s.EaAccDate }).OrderByDescending(o => o.EaAccDate).ToListAsync();
            /*var carList = await rvpAccidentContext.TbAccidentMasterLineCar.Where(w => accNo.Contains(w.EaAccNo)).ToListAsync();*/
            var accViewModel = new List<AccidentViewModel>();
            foreach (var acc in query)
            {
                var result = new AccidentViewModel();
                result.EaTmpId = acc.EaTmpId;
                result.EaAccNo = acc.EaAccNo;
                result.EaAccDate = acc.EaAccDate ?? DateTime.Now;                
                result.EaCar = await rvpAccidentContext.TbAccidentMasterLineCar.Where(w => acc.EaAccNo.Contains(w.EaAccNo)).ToListAsync();
                accViewModel.Add(result);
            }
            
            return accViewModel;
        }

        public List<string> GetAccNo(string userIdCard)
        {          
            return rvpAccidentContext.TbAccidentMasterLineVictim.Where(w => w.EaIdCardVictim == userIdCard && w.EaAccno != null).Select(s => s.EaAccno).ToList();
        }

        
    }
}
