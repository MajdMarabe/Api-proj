using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.utils
{
    public class RoleSeedData : ISeedData
    {
        private readonly RoleManager<IdentityRole> _roleManger;
        public RoleSeedData(RoleManager<IdentityRole> roleManger) {
            _roleManger = roleManger;
        }
        public async Task SeedData()
        {
            String[] roles = ["Admin", "User", "SuperAdmin"];

            if (!await _roleManger.Roles.AnyAsync())
            {
                foreach (var role in roles)
                {
                   await  _roleManger.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
