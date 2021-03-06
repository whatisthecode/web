﻿using Microsoft.AspNet.Identity;
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
            base.delete(userGroup.id);
        }

        public void ClearUserGroups(String userId)
        {
            ApplicationUser user = Service._userManager.FindById(userId);
            using (DBContext context = new DBContext())
            {
                user.groups.Clear();
                context.SaveChanges();
            }
        }

        public void dispose()
        {
            base.Dispose();
        }

        public IEnumerable<ApplicationUserGroup> getUserGroup()
        {
            return base.get();
        }

        public IEnumerable<ApplicationUserGroup> getUserGroupByUser(String userId)
        {
            var userGroups = base.get().Where(ug => ug.userId == userId).ToList();
            foreach(var userGroup in userGroups)
            {
                userGroup.Group = Service.groupDAO.getGroupById(userGroup.groupId);
            }
            return userGroups;
        }

        public ApplicationUserGroup getUserGroupById(String userId, Int16 groupId)
        {
            ApplicationUserGroup userGroup = base.get().Where(ug => ug.userId == userId && ug.groupId == groupId).FirstOrDefault();
            return userGroup;
        }

        public void AddUserToGroup(ApplicationUserGroup userGroup)
        {
        
            ApplicationUser user = Service._userManager.FindById(userGroup.userId);
            ICollection<ApplicationRoleGroup> roleGroups = Service.groupRoleManagerDAO.getGroupRoles(userGroup.groupId);

            foreach (ApplicationRoleGroup roleGroup in roleGroups)
            {
                ApplicationRole role = Service._roleManager.FindById(roleGroup.roleId);
                Service._userManager.AddToRole(userGroup.userId, role.Name);
            }

            base.insert(userGroup);
        }

        public void updateUserGroup(ApplicationUserGroup userGroup)
        {
            base.update(userGroup);
        }
    }
}