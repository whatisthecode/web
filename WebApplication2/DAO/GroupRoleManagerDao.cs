using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    interface GroupRoleManagerDao
    {
        bool RoleExists(string name);

        IdentityResult createRole(string name, string description = "");

        IdentityResult AddUserToRole(string userId, string roleName);

        void ClearUserRoles(string userId);

        void RemoveFromRole(string userId, string roleName);

        void DeleteRole(string roleId);

        void CreateGroup(string groupName);

        bool GroupNameExists(string groupName);

        void ClearUserGroups(string userId);

        void AddUserToGroup(string userId, Int16 groupId);

        void ClearGroupRoles(Int16 groupId);

        void AddRoleToGroup(Int16 groupId, string roleName);

        void DeleteGroup(Int16 groupId);

        Group findByName(string name);

        List<Group> GetAllGroup();

        void removeUserRole();
    }
}