using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class RoleDAOImpl : RoleDAO
    {
        private readonly RoleManager<ApplicationRole> _roleManager = null;

        public RoleDAOImpl() : base() {
            _roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new DBContext()));
        }

        public async Task<Boolean> deleteRoleAsync(ApplicationRole applicationRole)
        {
            var result = await _roleManager.DeleteAsync(applicationRole);
            if (result.Succeeded)
                return true;
            else
                return false;
        }
    }
}