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
        Task<List<BankNamesViewModel>> GetBank();
        Task<List<HospitalViewModel>> GetHospital();
        Task<List<ChangwatViewModel>> GetChangwat();
    }

    public class MasterService : IMasterService
    {
        private readonly RvpofficeContext rvpofficeContext;

        public MasterService(RvpofficeContext rvpofficeContext)
        {
            this.rvpofficeContext = rvpofficeContext;
        }
        public async Task<List<BankNamesViewModel>> GetBank()
        {
            var query = await rvpofficeContext.BankNames.Where(w => w.BankCode != null).Select(s => new { s.Bank, s.Name, s.Default0, s.BankCode }).ToListAsync();
            var bankViewModel = new List<BankNamesViewModel>();
            foreach (var bank in query)
            {
                var result = new BankNamesViewModel();
                result.Bank = bank.Bank;
                result.Name = bank.Name;
                result.Default0 = bank.Default0;
                result.BankCode = bank.BankCode;
                bankViewModel.Add(result);
            }

            return bankViewModel;
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

        public async Task<List<HospitalViewModel>> GetHospital()
        {

            var query = await rvpofficeContext.HospitalTable.Where(w => w.Hospitaltradename != "-,สต." && w.Hospitaltradename != "-,คลินิค" && w.Hospitaltradename != "-,รพ." && w.Hospitaltradename != null).Select(s => new { s.Hospitalid, s.Hospitaltradename, s.Changwatshortname, s.BranchId }).OrderBy(o => o.Hospitalid).ToListAsync();

            var hosViewModel = new List<HospitalViewModel>();
            foreach (var hos in query)
            {
                var result = new HospitalViewModel();
                result.Hospitalid = hos.Hospitalid;
                result.Hospitaltradename = hos.Hospitaltradename;
                result.Changwatshortname = hos.Changwatshortname;
                result.BranchId = hos.BranchId;
                hosViewModel.Add(result);
            }

            return hosViewModel;
        }
    }
}
