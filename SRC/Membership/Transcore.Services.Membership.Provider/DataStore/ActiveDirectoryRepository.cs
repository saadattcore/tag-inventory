using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.Services.Membership.IdentityModels;

namespace Transcore.Services.Membership.DataStore
{
    public class ActiveDirectoryRepository : IRepository
    {
        public Task<bool> CheckPassword(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateUser(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> GetUser(string username)
        {
            throw new NotImplementedException();
        }

    }
}
