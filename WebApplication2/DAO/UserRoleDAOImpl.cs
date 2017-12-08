using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class UserRoleDAOImpl : UserRoleDAO
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationRole> _roleManager;
        public UserRoleDAOImpl()
        {
            this._userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new DBContext()));
            this._roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(new DBContext()));
        }
        public void ClearUserRoles(String userId)
        {

            ApplicationUser user = this._userManager.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();
            currentRoles.AddRange(user.Roles);
            foreach (IdentityUserRole role in currentRoles)
            {
                var roleName = this._roleManager.FindById(role.RoleId);
                this._userManager.RemoveFromRole(userId, roleName.ToString());
            }
        }
    }
}