using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.Services.Membership.Models;
using Transcore.Services.Membership.IdentityModels;

namespace Transcore.Services.Membership.DataStore
{
    public interface IRepository
    {
        Task<bool> CreateUser(ApplicationUser user);
        Task<ApplicationUser> GetUser(string username);
        Task<bool> CheckPassword(ApplicationUser user);
    }
}
