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

namespace Services
{
    public interface IMasterService
    {
        Task<List<BankNames>> GetBank();
        Task<List<ChangwatViewModel>> GetChangwat();
        Task<GenAddressViewModel> GetIdAddress(string changwat, string amphur, string tumbol);
        Task<JJJ> GetWoundeds();
        Task<List<string>> GetPrefixesAsync();
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
            return await rvpofficeContext.BankNames.Where(w => w.BankCode != null).ToListAsync();
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

        // ยังไม่สำบูรณ์ต้องแก้ insert ลง db แล้ว ลงแค่ proviceId
        public async Task<GenAddressViewModel> GetIdAddress(string changwat, string amphur, string tumbol)
        {
            var changwatShortname = await rvpofficeContext.Changwat.Where(w => w.Changwatname.Contains(changwat)).Select(s => s.Changwatshortname).FirstOrDefaultAsync();
            var amphurId = await rvpofficeContext.Amphur.Where(w => w.Amphurname.Contains(amphur) && w.Changwatshortname == changwatShortname).Select(s => s.Amphurid).FirstOrDefaultAsync();
            var query = await rvpofficeContext.Tumbol.Where(w => w.Tumbolname.Contains(tumbol) && w.Amphurid == amphurId && w.Changwatshortname == changwatShortname).Select(s => new { s.Tumbolid, s.Amphurid, s.Provinceid, s.Zipcode }).FirstOrDefaultAsync();
            //var query = await rvpofficeContext.Tumbol
            //        .Join(rvpofficeContext.Amphur, tja => tja.Amphurid, amp => amp.Amphurid, (tja, amp) => new { tumbolJoinAmphur = tja, amphurName = amp.Amphurname, amphurId = amp.Amphurid })
            //        .Join(rvpofficeContext.Changwat, tjchw => tjchw.tumbolJoinAmphur.Changwatshortname, chw => chw.Changwatshortname, (tjchw, chw) => new { tumbolJoinChangwat = tjchw, changwatName = chw.Changwatname, changwatShort = chw.Changwatshortname })
            //        .Where(w => w.tumbolJoinChangwat.tumbolJoinAmphur.Tumbolname.Contains(tumbol) && w.tumbolJoinChangwat.amphurName == amphur && w.changwatName == changwat)
            //        .Select(s => new { s.tumbolJoinChangwat.tumbolJoinAmphur.Tumbolid, s.tumbolJoinChangwat.amphurId, s.changwatShort , s.tumbolJoinChangwat.tumbolJoinAmphur.Zipcode}).FirstOrDefaultAsync();
            GenAddressViewModel genAddress = new GenAddressViewModel();
            genAddress.ProvinceId = (query != null) ? query.Provinceid : null;
            genAddress.DistrictId = (query != null) ? query.Amphurid : amphurId;
            genAddress.SubDistrictId = (query != null) ? query.Tumbolid : null;
            genAddress.ZipCode = (query != null) ? query.Zipcode : null;
            return genAddress;
        }

        public class JJJ
        {
            public List<wound> WoundedList { get; set; }
            public List<string> Organ { get; set; }
        }
        public class wound
        {
            public short WoundedId { get; set; }
            public string Wounded { get; set; }

            public string Organ { get; set; }
        }
        public async Task<JJJ> GetWoundeds()
        {

            JJJ jjj = new JJJ();
            var qurey = await rvpofficeContext.Mcwounded.Where(w => w.Organ != "มป" && w.Organ != "หัวเข่า").Select(s => new { s.WoundedId, s.WoundedName, s.Organ}).ToListAsync();
            var organ = qurey.GroupBy(x => x.Organ).Select(s => s.Key).ToList();
            var lookup = qurey.ToLookup(x => x.Organ);
            List<wound> WoundedList = new List<wound>();
            foreach (var l in qurey)
            {
                wound w = new wound();
                w.WoundedId = l.WoundedId;
                w.Wounded = l.WoundedName;
                w.Organ = l.Organ;
                WoundedList.Add(w);
            }
            
            jjj.WoundedList = WoundedList;
            
            jjj.Organ = organ;                  
            
            

            
            return jjj;
        }
        public async Task<List<string>> GetPrefixesAsync()
        {
            var query = await rvpofficeContext.Prefix.Where(w => w.Sex != null).Select(s => new { s.Titlename, s.Seq}).OrderBy(o => o.Seq).ToListAsync();
            return query.Select(s => s.Titlename).ToList();
        }
        
    }
}
