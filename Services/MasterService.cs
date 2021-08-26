using DataAccess.EFCore.RvpOfficeModels;
using Services.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.EFCore.iPolicyModels;

namespace Services
{
    public interface IMasterService
    {
        Task<List<BankNames>> GetBank();
        Task<List<ChangwatViewModel>> GetChangwat();
    }

    public class MasterService : IMasterService
    {
        private readonly RvpofficeContext rvpofficeContext;

        public MasterService(RvpofficeContext rvpofficeContext)
        {
            this.rvpofficeContext = rvpofficeContext;
        }
        public async Task<List<BankNames>> GetBank()
        {           
            return await rvpofficeContext.BankNames.Where(w => w.BankCode != null).ToListAsync(); ;
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
