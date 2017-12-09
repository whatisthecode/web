using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using WebApplication2.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Runtime.Serialization;

namespace WebApplication2.DAO
{
    public class GroupRoleManagerDAOImpl : BaseImpl<ApplicationRoleGroup, Int16>, GroupRoleManagerDAO, IDisposable
    {

        public GroupRoleManagerDAOImpl() : base()
        {

        }

        public void AddRoleToGroup(Int16 groupId, string roleName)
        {
            Group group = Service.groupDAO.getGroupById(groupId);
            ApplicationRole role = Service._roleManager.FindByName(roleName);

            var newGroupRole = new ApplicationRoleGroup
            {
                groupId = group.id,
                roleId = role.Id
            };
            ICollection<ApplicationRoleGroup> roleGroups = this.getGroupRoles(groupId);
            //make sure the groupRole is not already present
            if (!roleGroups.Contains(newGroupRole))
            {
                base.insert(newGroupRole);
                base.save();
            }

            //Add all of the users in this group to the new role:
            IQueryable<ApplicationUser> groupUsers = base.getContext().Users.Where(u => u.groups.Any(g => g.groupId.Equals(group.id)));
            foreach (ApplicationUser user in groupUsers)
            {
                if (!(Service._userManager.IsInRole(user.Id, roleName)))
                {
                    Service._userManager.AddToRole(user.Id, role.Name);
                }
            }
        }

        public void ClearGroupRoles(Int16 groupId)
        {
            Group group = Service.groupDAO.getGroupById(groupId);
            IQueryable<ApplicationUser> users = base.getContext().Users.Where(u => u.groups.Any(g => g.groupId.Equals(group.id)));
            ICollection<ApplicationRoleGroup> roleGroups = this.getGroupRoles(groupId);

            foreach (ApplicationRoleGroup role in roleGroups)
            {
                string currentRoleId = role.roleId;
                foreach (ApplicationUser user in users)
                {
                    int groupsWithRole = roleGroups.Where(gr => gr.groupId == groupId && gr.roleId == currentRoleId).Count();

                    if (groupsWithRole == 1)
                    {
                        Service._userManager.RemoveFromRole(user.Id, role.ApplicationRole.Name);
                    }
                }
                base.getContext().roleGroups.Remove(role);
                base.save();
            }
        }

        public ICollection<ApplicationRoleGroup> getGroupRoles(Int16 groupId)
        {
            return base.get().Where(gr => gr.groupId == groupId).ToList();
        }

        [Serializable]
        public class GroupExistsException : Exception
        {
            public GroupExistsException()
            {
            }

            public GroupExistsException(string message) : base(message)
            {
            }

            public GroupExistsException(string message, Exception inner) : base(message, inner)
            {
            }

            protected GroupExistsException(
                SerializationInfo info,
                StreamingContext context) : base(info, context)
            {
            }
        }
    }
}