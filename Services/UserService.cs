using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.AccidentModels;
using Microsoft.EntityFrameworkCore;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserService
    {
        Task<DirectPolicyKycViewModel> GetUser(string userToken);

    }


    public class UserService : IUserService
    {
        private readonly RvpaccidentContext rvpAccidentContext;
        private readonly IpolicyContext ipolicyContext;

        public UserService(RvpaccidentContext rvpAccidentContext, IpolicyContext ipolicyContext)
        {
            this.rvpAccidentContext = rvpAccidentContext;
            this.ipolicyContext = ipolicyContext;
        }

        public async Task<DirectPolicyKycViewModel> GetUser(string userToken)
        {
            var query = await ipolicyContext.DirectPolicyKyc.Where(w => w.LineId == userToken).
                Select(s => new { s.LineId, s.Prefix, s.Fname, s.Lname, s.IdcardNo, s.MobileNo, s.Kycno}).FirstOrDefaultAsync();

            var directPolicyKyc = new DirectPolicyKycViewModel();
            directPolicyKyc.Kycno = query.Kycno;
            directPolicyKyc.LineId = query.LineId;
            directPolicyKyc.Prefix = query.Prefix;
            directPolicyKyc.Fname = query.Fname;
            directPolicyKyc.Lname = query.Lname;
            directPolicyKyc.IdcardNo = query.IdcardNo;
            directPolicyKyc.MobileNo = query.MobileNo;


            return directPolicyKyc;
        }

     
    }
}
