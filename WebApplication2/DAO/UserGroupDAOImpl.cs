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
        private GroupDAO groupDAO;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserGroupDAOImpl() : base()
        {
            this.groupDAO = new GroupDAOImpl();
            this._userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new DBContext()));
            this._roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(new DBContext()));
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
            Group group = this.groupDAO.getGroupById(userGroup.groupId);

            ApplicationUser user = _userManager.FindById(userGroup.userId);

            foreach (ApplicationRoleGroup roleGroup in group.roles)
            {
                ApplicationRole role = this._roleManager.FindById(roleGroup.roleId);
                _userManager.AddToRole(userGroup.userId, role.Name);
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