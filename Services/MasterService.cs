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
using Newtonsoft.Json;
using static Services.MasterService;
using DataAccess.EFCore.DigitalClaimModels;
using Services.Models;

namespace Services
{
    public interface IMasterService
    {
        Task<List<BankNames>> GetBank();
        Task<List<Province>> GetChangwatsAsync();
        Task<List<District>> GetAmphursByChangwatnameAsync(string changwatshortname);
        Task<List<SubDistrict>> GetTumbolsByAmphurIdAsync(string changwatshortname, string amphurId);
        Task<ResultWound> GetWoundeds();
        Task<List<string>> GetPrefixesAsync();
        Task<object> GetTypesOfInvoiceNotPass();
        Task<object> GetTypesOfBankAccountNotPass();
    }

    public class MasterService : IMasterService
    {
        private readonly RvpofficeContext rvpofficeContext;
        private readonly DigitalclaimContext digitalclaimContext;

        public MasterService(RvpofficeContext rvpofficeContext, DigitalclaimContext digitalclaimContext)
        {
            this.rvpofficeContext = rvpofficeContext;
            this.digitalclaimContext = digitalclaimContext;
        }
        public async Task<List<BankNames>> GetBank()
        {
            return await rvpofficeContext.BankNames.Where(w => w.BankCode != null).ToListAsync();
        }

        public async Task<List<Province>> GetChangwatsAsync()
        {         
            return await rvpofficeContext.Changwat.Where(w => w.Branchid != null).Select(s => new Province { Changwatshortname = s.Changwatshortname, Changwatname = s.Changwatname, Branchid = s.Branchid, ProvinceId = s.Provinceid }).ToListAsync(); ;
        }
        public async Task<List<District>> GetAmphursByChangwatnameAsync(string changwatshortname)
        {
            return await rvpofficeContext.Amphur.Where(w => w.Changwatshortname == changwatshortname && !string.IsNullOrEmpty(w.Amphurname)).Select(s => new District { Amphurid = s.Amphurid, Amphurname = s.Amphurname }).ToListAsync();
        }

        public async Task<List<SubDistrict>> GetTumbolsByAmphurIdAsync(string changwatshortname, string amphurId)
        {
            return await rvpofficeContext.Tumbol.Where(w => w.Changwatshortname == changwatshortname && w.Amphurid == amphurId && w.Status != "C" && !string.IsNullOrEmpty(w.Tumbolname)).Select(s => new SubDistrict { Tumbolid = s.Tumbolid, Tumbolname = s.Tumbolname, Zipcode = s.Zipcode}).ToListAsync();
        }

       
        
        public async Task<ResultWound> GetWoundeds()
        {
            ResultWound result = new ResultWound();
            var mcWoundeds = await rvpofficeContext.Mcwounded.Where(w => w.Organ != "มป" && w.Organ != "หัวเข่า").Select(s => new { s.WoundedId, s.WoundedName, s.Organ}).ToListAsync();
            var organ = mcWoundeds.GroupBy(x => x.Organ).Select(s => s.Key).ToList();
            var lookup = mcWoundeds.ToLookup(x => x.Organ);
            List<Wound> WoundedList = new List<Wound>();

            for (int i = 0; i < mcWoundeds.Count; i++)
            {
                Wound w = new Wound();
                w.WoundedId = mcWoundeds[i].WoundedId;
                w.Wounded = mcWoundeds[i].WoundedName;
                w.Organ = mcWoundeds[i].Organ;
                WoundedList.Add(w);
            }
            result.WoundedList = WoundedList;
            result.Organ = organ;                  
            return result;
        }
        public async Task<List<string>> GetPrefixesAsync()
        {
            var query = await rvpofficeContext.Prefix.Where(w => w.Sex != null).Select(s => new { s.Titlename, s.Seq}).OrderBy(o => o.Seq).ToListAsync();
            return query.Select(s => s.Titlename).ToList();
        }

        public async Task<object> GetTypesOfInvoiceNotPass()
        {
            return await digitalclaimContext.IclaimMasterTypes.Where(w => w.ParentTypeId == 100 && w.IsActive).Select(s => new { s.TypeId, s.TypeName}).ToListAsync();
        }

        public async Task<object> GetTypesOfBankAccountNotPass()
        {
            return await digitalclaimContext.IclaimMasterTypes.Where(w => w.ParentTypeId == 200 && w.IsActive).Select(s => new { s.TypeId, s.TypeName }).ToListAsync();
        }

    }
}
