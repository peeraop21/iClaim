using DataAccess.EFCore.DigitalClaimModels;
using DataAccess.EFCore.RvpOfficeModels;
using Microsoft.EntityFrameworkCore;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IApprovalService
    {

        Task<ClaimViewModel> GetApprovalByAccNo(string accNo);
    }


    public class ApprovalService : IApprovalService
    {
        private readonly DigitalclaimContext digitalclaimContext;
        private readonly RvpofficeContext rvpofficeContext;
        
        public ApprovalService(DigitalclaimContext digitalclaimContext, RvpofficeContext rvpofficeContext)
        {
            this.digitalclaimContext = digitalclaimContext;
            this.rvpofficeContext = rvpofficeContext;
        }


        public async Task<DataAccess.EFCore.DigitalClaimModels.HosApproval> AddAsync(DataAccess.EFCore.DigitalClaimModels.HosApproval hosApproval)
        {
            /*var query = await rvpofficeContext.HosApproval.Where(w => w.AccNo == hosApproval.AccNo && w.VictimNo == hosApproval.VictimNo).Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.Pt4id }).LastOrDefaultAsync();*/
            
            hosApproval.AppNo = hosApproval.AppNo + 1;
            await digitalclaimContext.HosApproval.AddAsync(hosApproval);

            return hosApproval;
        }

        public async Task<ClaimViewModel> GetApprovalByAccNo(string accNo)
        {
            var query = await rvpofficeContext.HosApproval.Where(w => w.AccNo == accNo).Select(s => new { s.AccNo, s.VictimNo, s.AppNo, s.ClaimNo, s.Pt4id }).OrderByDescending(o => o.AppNo).Take(1).FirstOrDefaultAsync();
            var claimVwModel = new ClaimViewModel();
            if (query != null)
            {
                
                claimVwModel.AccNo = query.AccNo;
                claimVwModel.VictimNo = query.VictimNo;
                claimVwModel.AppNo = query.AppNo;
                claimVwModel.ClaimNo = query.ClaimNo;
                claimVwModel.Pt4id = query.Pt4id;
            }
            
            return claimVwModel;
        }


    }
}
