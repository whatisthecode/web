using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public interface UserGroupDAO
    {
        IEnumerable<ApplicationUserGroup> getUserGroup();
        IEnumerable<ApplicationUserGroup> getUserGroupByUser(String userId);
        ApplicationUserGroup getUserGroupById(String userId, Int16 groupId);
        void AddUserToGroup(ApplicationUserGroup userGroup);
        void deleteUserGroup(String userId, Int16 groupId);
        void ClearUserGroups(String userId);
        void updateUserGroup(ApplicationUserGroup userGroup);
        void dispose();
    }
}