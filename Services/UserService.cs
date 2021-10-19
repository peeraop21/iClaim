using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.AccidentModels;
using Microsoft.EntityFrameworkCore;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Services
{
    public interface IUserService
    {
        Task<DirectPolicyKycViewModel> GetUser(string userToken);
        Task<bool> CheckRegister(string userToken);
        Task<object> AddAsync(DirectPolicyKycViewModel model);
        Task<int> GetLastKyc();
    }


    public class UserService : IUserService
    {
        private readonly RvpaccidentContext rvpAccidentContext;
        private readonly IpolicyContext ipolicyContext;
        private readonly IMapper _mapper;

        public UserService(RvpaccidentContext rvpAccidentContext, IpolicyContext ipolicyContext, IMapper _mapper)
        {
            this.rvpAccidentContext = rvpAccidentContext;
            this.ipolicyContext = ipolicyContext;
            this._mapper = _mapper;
        }

        public async Task<DirectPolicyKycViewModel> GetUser(string userToken)
        {
            var query = await ipolicyContext.DirectPolicyKyc.Where(w => w.LineId == userToken).
                Select(s => new { s.LineId, s.Prefix, s.Fname, s.Lname, s.IdcardNo, s.MobileNo, s.Kycno}).FirstOrDefaultAsync();
            if (query == null)
            {
                return null;
            }
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

        public async Task<bool> CheckRegister(string userToken)
        {
            var query = await ipolicyContext.DirectPolicyKyc.Where(w => w.LineId == userToken).Select(s => s.Kycno).FirstOrDefaultAsync();
            if (query <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }           
        }

        public async Task<object> AddAsync(DirectPolicyKycViewModel model)
        {        
            try
            {
                var dataDirectPolicy = _mapper.Map<DirectPolicyKyc>(model);
                await ipolicyContext.DirectPolicyKyc.AddAsync(dataDirectPolicy);
                await ipolicyContext.SaveChangesAsync();
                return dataDirectPolicy;
            }
            catch(Exception e)
            {
                return e;
            }                                
        }
        public async Task<int> GetLastKyc()
        {            
            return await ipolicyContext.DirectPolicyKyc.OrderByDescending(o => o.Kycno).Select(s => s.Kycno).Take(1).FirstOrDefaultAsync();
        }

     
    }
}
