using DataAccess.EFCore.RvpOfficeModels;
using Microsoft.EntityFrameworkCore;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IChangwatService
    {
        Task<List<ChangwatViewModel>> GetChangwat();
    }
    public class ChangwatService : IChangwatService
    {
        private readonly RvpofficeContext rvpofficeContext;

        public ChangwatService(RvpofficeContext rvpofficeContext)
        {
            this.rvpofficeContext = rvpofficeContext;
        }
        public async Task<List<ChangwatViewModel>> GetChangwat()
        {
            var query = await rvpofficeContext.Changwat.Where(w => w.Branchid != null).Select(s => new { s.Changwatshortname, s.Changwatname, s.Branchid }).ToListAsync();
            var chwViewModel = new List<ChangwatViewModel>();
            foreach (var bank in query)
            {
                var result = new ChangwatViewModel();
                result.Changwatshortname = bank.Changwatshortname;
                result.Changwatname = bank.Changwatname;
                result.Branchid = bank.Branchid;
                chwViewModel.Add(result);
            }

            return chwViewModel;
        }
    }
    
}
