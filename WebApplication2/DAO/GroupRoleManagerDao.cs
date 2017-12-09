using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public interface GroupRoleManagerDAO
    {

        void ClearGroupRoles(Int16 groupId);

        void AddRoleToGroup(Int16 groupId, string roleName);

        ICollection<ApplicationRoleGroup> getGroupRoles(Int16 groupId);

    }
}