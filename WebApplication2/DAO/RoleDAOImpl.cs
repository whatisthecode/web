using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class RoleDAOImpl : RoleDAO
    {
        private DBContext context;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationRole> _roleManager;
        public RoleDAOImpl()
        {
            this.context = DatabaseFactory.context;
            this._userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new DBContext()));
            this._roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(new DBContext()));
        }
        public void DeleteRole(String roleId)
        {
            IQueryable<ApplicationUser> roleUsers = this.context.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId));
            ApplicationRole role = this._roleManager.FindById(roleId);

            foreach (ApplicationUser user in roleUsers)
            {
                this._userManager.RemoveFromRole(user.Id, role.Name);
            }
            this._roleManager.Delete(role);
            this.context.SaveChanges();
        }
    }
}