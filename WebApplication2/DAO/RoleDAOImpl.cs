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
        public RoleDAOImpl() : base()
        {

        }
        public void DeleteRole(String roleId)
        {
            IQueryable<ApplicationUser> roleUsers = DatabaseFactory.context.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId));
            ApplicationRole role = Service._roleManager.FindById(roleId);

            foreach (ApplicationUser user in roleUsers)
            {
                Service._userManager.RemoveFromRole(user.Id, role.Name);
            }
            Service._roleManager.Delete(role);
            DatabaseFactory.context.SaveChanges();
        }
    }
}