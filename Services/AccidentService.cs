using DataAccess.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAccidentService
    {
        Task<List<AccidentViewModel>> GetAccident();

    }


    public class AccidentService : IAccidentService
    {
        private readonly RvpAccidentContext rvpAccidentContext;

        public AccidentService(RvpAccidentContext rvpAccidentContext)
        {
            this.rvpAccidentContext = rvpAccidentContext;
        }

        public async Task<List<AccidentViewModel>> GetAccident()
        {
            var accNo = GetAccNo();
            var query = await rvpAccidentContext.TbAccidentMasterLine.Where(w => accNo.Contains(w.EaAccNo)).Select(s  => new {s.EaTmpId,s.EaAccNo,s.EaAccDate }).ToListAsync();
            /*var carList = await rvpAccidentContext.TbAccidentMasterLineCar.Where(w => accNo.Contains(w.EaAccNo)).ToListAsync();*/
            var accViewModel = new List<AccidentViewModel>();
            foreach (var acc in query)
            {
                var result = new AccidentViewModel();
                result.EaTmpId = acc.EaTmpId;
                result.EaAccNo = acc.EaAccNo;
                result.EaAccDate = acc.EaAccDate;
                var carList = await rvpAccidentContext.TbAccidentMasterLineCar.Where(w => acc.EaAccNo.Contains(w.EaAccNo)).ToListAsync();

                result.EaCar = carList;
                
                

                accViewModel.Add(result);
            }
            
            return accViewModel;
        }

        public List<string> GetAccNo()
        {
            var IdCard = "1100201513657";
            return rvpAccidentContext.TbAccidentMasterLineVictim.Where(w => w.EaIdCardVictim == IdCard && w.EaAccno != null).Select(s => s.EaAccno).ToList();
        }

        public async Task<List<TbAccidentMasterLineCar>> GetCar(List<string> accNo){           
            return  await rvpAccidentContext.TbAccidentMasterLineCar.Where(w => accNo.Contains(w.EaAccNo)).ToListAsync();
        }
    }
}
