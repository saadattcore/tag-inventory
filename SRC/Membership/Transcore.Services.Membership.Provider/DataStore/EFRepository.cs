using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.Services.Membership.Models;
using Transcore.Services.Membership.IdentityModels;
using Microsoft.Data.SqlClient;

namespace Transcore.Services.Membership.DataStore
{
    public class EFRepository : IRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public EFRepository(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }
        public async Task<bool> CreateUser(ApplicationUser user)
        {
            var r = await _userManager.CreateAsync(user, user.Password);

            if (!r.Succeeded)
            {
                string errors = string.Join(",", r.Errors.Select(x => x.Description));

                throw new Exception(errors);
            }

            return r.Succeeded;

           
        }

        public async Task<ApplicationUser> GetUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return user;
        }

        public async Task<bool> CheckPassword(ApplicationUser user)
        {
            return await _userManager.CheckPasswordAsync(user, user.Password);
        }
    }
}
