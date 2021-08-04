using DataAccess.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAccidentService
    {
        Task<List<TbAccidentMasterLine>> GetAccident();
    }


    public class AccidentService : IAccidentService
    {
        private readonly RvpAccidentContext rvpAccidentContext;

        public AccidentService(RvpAccidentContext rvpAccidentContext)
        {
            this.rvpAccidentContext = rvpAccidentContext;
        }

        public async Task<List<TbAccidentMasterLine>> GetAccident()
        {
            var accNo = await rvpAccidentContext.TbAccidentMasterLineVictim.Where(w => w.EaIdCardVictim == "1100201513657" && w.EaAccno != null).Select(s => s.EaAccno).ToListAsync();
            var query = await rvpAccidentContext.TbAccidentMasterLine.Where(w => accNo.Contains(w.EaAccNo)).ToListAsync();
            return query;
        }
    }
}
