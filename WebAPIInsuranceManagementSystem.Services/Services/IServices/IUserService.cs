using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIInsuranceManagementSystem.DataAccess.Models;
using WebAPIInsuranceManagementSystem.Services.DTOs;

namespace WebAPIInsuranceManagementSystem.Services.Services.IServices
{
    public interface IUserService
    {
        public Task<int> BuyPolicy(UserPolicyDTO userPolicyDTO);

        public Task<List<MyPolicyDTO>> GetBoughtPolicies(int userId);

        public Task<List<UserDTO>> GetApprovedAgents();


    }

}
