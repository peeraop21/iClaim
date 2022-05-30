using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.AccidentModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Services.Models;

namespace Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdLine(string userToken);
        Task<bool> CheckRegisterByIdLine(string userToken);
        Task<object> AddAsync(User model);
        Task<int> GetLastKyc();
        Task<bool> CheckUserAccessToken(string systemName, string userToken);
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

        public async Task<User> GetUserByIdLine(string userToken)
        {
            return await ipolicyContext.DirectPolicyKyc.Where(w => w.LineId == userToken && w.Status == "Y").
                Select(s => new User { Kycno = s.Kycno, LineId = s.LineId, Prefix = s.Prefix, Fname = s.Fname, Lname = s.Lname, IdcardNo = s.IdcardNo, MobileNo = s.MobileNo, StringDateofBirth = s.DateofBirth.Value.ToString("dd/MM/yyyy") })
                .OrderByDescending(o => o.Kycno)
                .Take(1)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CheckRegisterByIdLine(string userToken)
        {
            var query = await ipolicyContext.DirectPolicyKyc.Where(w => w.LineId == userToken && w.Status == "Y").Select(s => s.Kycno).OrderByDescending(o => o).FirstOrDefaultAsync();
            if (query <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }           
        }

        public async Task<object> AddAsync(User model)
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
        public async Task<bool> CheckUserAccessToken(string systemName, string userToken)
        {
            if(systemName == "eClaim")
            {
                return true;
            }
            var userId = await ipolicyContext.DirectPolicyKyc.Where(w => w.LineId == userToken && w.Status == "Y").Select(s => s.LineId).OrderByDescending(o => o).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(userId))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

     
    }
}
