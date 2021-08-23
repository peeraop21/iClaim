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
    public interface IHospitalService
    {
        Task<List<HospitalViewModel>> GetHospital();
    }
    public class HospitalService : IHospitalService
    {
        private readonly RvpofficeContext rvpofficeContext;
        public HospitalService(RvpofficeContext rvpofficeContext)
        {
            this.rvpofficeContext = rvpofficeContext;
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
