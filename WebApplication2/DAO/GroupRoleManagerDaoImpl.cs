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
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private GroupDAO groupDAO;
        private UserGroupDAO userGroupDAO;

        public GroupRoleManagerDAOImpl() : base()
        {
            this._roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(new DBContext()));
            this._userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new DBContext()));
            this.groupDAO = new GroupDAOImpl();
            this.userGroupDAO = new UserGroupDAOImpl();
        }

        public void AddRoleToGroup(Int16 groupId, string roleName)
        {
            Group group = this.groupDAO.getGroupById(groupId);
            ApplicationRole role = _roleManager.FindByName(roleName);

            var newGroupRole = new ApplicationRoleGroup
            {
                groupId = group.id,
                roleId = role.Id
            };

            //make sure the groupRole is not already present
            if (!group.roles.Contains(newGroupRole))
            {
                base.insert(newGroupRole);
                base.save();
            }

            //Add all of the users in this group to the new role:
            IQueryable<ApplicationUser> groupUsers = base.getContext().Users.Where(u => u.groups.Any(g => g.groupId.Equals(group.id)));
            foreach (ApplicationUser user in groupUsers)
            {
                if (!(_userManager.IsInRole(user.Id, roleName)))
                {
                    _userManager.AddToRole(user.Id, role.Name);
                }
            }
        }

        public void ClearGroupRoles(Int16 groupId)
        {
            Group group = this.groupDAO.getGroupById(groupId);
            IQueryable<ApplicationUser> users = base.getContext().Users.Where(u => u.groups.Any(g => g.groupId.Equals(group.id)));

            foreach (ApplicationRoleGroup role in group.roles)
            {
                string currentRoleId = role.roleId;
                foreach (ApplicationUser user in users)
                {
                    int groupsWithRole = user.groups.Count(g => g.Group.roles.Any(r => r.roleId == currentRoleId));

                    if (groupsWithRole == 1)
                    {
                        _userManager.RemoveFromRole(user.Id, role.ApplicationRole.Name);
                    }
                }
                base.getContext().roleGroups.Remove(role);
                base.save();
            }
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