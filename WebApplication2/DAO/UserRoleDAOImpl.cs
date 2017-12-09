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
        public UserRoleDAOImpl() : base()
        {

        }
        public void ClearUserRoles(String userId)
        {

            ApplicationUser user = Service._userManager.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();
            currentRoles.AddRange(user.Roles);
            foreach (IdentityUserRole role in currentRoles)
            {
                var roleName = Service._roleManager.FindById(role.RoleId);
                Service._userManager.RemoveFromRole(userId, roleName.ToString());
            }
        }
    }
}