using DataAccess.EFCore.BankNamesModels;
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
    public interface IBankNamesService
    {
        Task<List<BankNamesViewModel>> GetBank();
    }

    public class BankNamesService : IBankNamesService
    {
        private readonly RvpofficeContext rvpofficeContext;

        public BankNamesService(RvpofficeContext rvpofficeContext)
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
    }
}
