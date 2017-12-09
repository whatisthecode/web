using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class UserGroupDAOImpl : BaseImpl<ApplicationUserGroup, Int16>, UserGroupDAO, IDisposable
    {

        public UserGroupDAOImpl() : base()
        {

        }

        public void deleteUserGroup(String userId, Int16 groupId)
        {
            ApplicationUserGroup userGroup = this.getUserGroupById(userId, groupId);
            base.getContext().userGroups.Remove(userGroup);
        }

        public void ClearUserGroups(String userId)
        {
            //TO DO
            ApplicationUser user = base.getContext().Users.Find(userId);
            user.groups.Clear();
            base.getContext().SaveChanges();
        }

        public void dispose()
        {
            base.Dispose();
        }

        public IEnumerable<ApplicationUserGroup> getUserGroup()
        {
            return base.get();
        }

        public ApplicationUserGroup getUserGroupById(String userId, Int16 groupId)
        {
            ApplicationUserGroup userGroup = base.get().Where(ug => ug.userId == userId && ug.groupId == groupId).FirstOrDefault();
            return userGroup;
        }

        public void AddUserToGroup(ApplicationUserGroup userGroup)
        {
            Group group = Service.groupDAO.getGroupById(userGroup.groupId);

            ApplicationUser user = Service._userManager.FindById(userGroup.userId);
            ICollection<ApplicationRoleGroup> roleGroups = Service.groupRoleManagerDAO.getGroupRoles(userGroup.groupId);

            foreach (ApplicationRoleGroup roleGroup in roleGroups)
            {
                ApplicationRole role = Service._roleManager.FindById(roleGroup.roleId);
                Service._userManager.AddToRole(userGroup.userId, role.Name);
            }

            base.insert(userGroup);
        }

        public void saveUserGroup()
        {
            base.save();
        }

        public void updateUserGroup(ApplicationUserGroup userGroup)
        {
            base.update(userGroup);
        }
    }
}