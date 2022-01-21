using DataAccess.EFCore.AccidentModels;
using DataAccess.EFCore.ClaimDataModels;
using DataAccess.EFCore.DigitalClaimModels;
using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.RvpOfficeModels;
using DataAccess.EFCore.RvpSystemModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IWarmupService
    {
        Task<string> GetDataForWarmup();
    }
    public class WarmupService : IWarmupService
    {
        private readonly IpolicyContext ipolicyContext;
        private readonly RvpofficeContext rvpOfficeContext;
        private readonly ClaimDataContext claimDataContext;
        private readonly DigitalclaimContext digitalclaimContext;
        private readonly RvpSystemContext rvpSystemContext;

        public WarmupService(IpolicyContext ipolicyContext, RvpofficeContext rvpOfficeContext, ClaimDataContext claimDataContext, DigitalclaimContext digitalclaimContext, RvpSystemContext rvpSystemContext)
        {

            this.ipolicyContext = ipolicyContext;
            this.rvpOfficeContext = rvpOfficeContext;
            this.claimDataContext = claimDataContext;
            this.digitalclaimContext = digitalclaimContext;
            this.rvpSystemContext = rvpSystemContext;
        }

        public async Task<string> GetDataForWarmup()
        {
            var warmIpolicyContext = await ipolicyContext.DirectPolicyKyc.Select(s => s.Kycno).FirstOrDefaultAsync();
            var warmRvpOfficeContext = await rvpOfficeContext.HosApproval.Select(s => s.AccNo).FirstOrDefaultAsync();
            var warmClaimDataContext = await claimDataContext.Approvalregis.Select(s => s.AccNo).FirstOrDefaultAsync();
            var warmDigitalclaimContext = await digitalclaimContext.IclaimMasterTypes.Select(s => s.TypeId).FirstOrDefaultAsync();
            var warmRvpSystemContext = await rvpSystemContext.EDocDetail.Select(s => s.DocumentId).FirstOrDefaultAsync();
            return "Warm Up!";
        }
    }
}
