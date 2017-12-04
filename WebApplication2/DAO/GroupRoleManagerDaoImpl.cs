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
    public class GroupRoleManagerDAOImpl : BaseImpl<Group, Int16>, GroupRoleManagerDAO, IDisposable
    {
        private readonly RoleManager<ApplicationRole> _roleManager = new RoleManager<ApplicationRole>(
            new RoleStore<ApplicationRole>(new DBContext()));

        private readonly UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(new DBContext()));

        public GroupRoleManagerDAOImpl() : base()
        {

        }

        public void AddRoleToGroup(Int16 groupId, string roleName)
        {
            Group group = base.getById(groupId);
            ApplicationRole role = _roleManager.FindByName(roleName);

            var newGroupRole = new ApplicationRoleGroup
            {
                groupId = group.id,
                roleId = role.Id
            };

            //make sure the groupRole is not already present
            if (!group.roles.Contains(newGroupRole))
            {
                group.roles.Add(newGroupRole);
                base.getContext().SaveChanges();
            }

            //Add all of the users in this group to the new role:
            IQueryable<ApplicationUser> groupUsers = base.getContext().Users.Where(u => u.groups.Any(g => g.groupId.Equals(group.id)));
            foreach (ApplicationUser user in groupUsers)
            {
                if (!(_userManager.IsInRole(user.Id, roleName)))
                {
                    AddUserToRole(user.Id, role.Name);
                }
            }
        }

        public void AddUserToGroup(string userId, Int16 groupId)
        {
            Group group = base.getById(groupId);
            ApplicationUser user = _userManager.FindById(userId);

            var userGroup = new ApplicationUserGroup
            {
                groupId = group.id,
                userId = user.Id
            };

            foreach (ApplicationRoleGroup role in group.roles)
            {
                ApplicationRole roleName;
                if(role.role == null)
                {
                    roleName = _roleManager.FindById(role.roleId);
                    _userManager.AddToRole(userId, roleName.Name);
                }
                else
                {
                    _userManager.AddToRole(userId, role.role.Name);
                }
                
            }
            user.groups.Add(userGroup);
            base.getContext().SaveChanges();
        }

        public IdentityResult AddUserToRole(string userId, string roleName)
        {
            return _userManager.AddToRole(userId, roleName);
        }

        public void ClearGroupRoles(Int16 groupId)
        {
            Group group = base.getById(groupId);
            IQueryable<ApplicationUser> groupsUsers = base.getContext().Users.Where(u => u.groups.Any(g => g.groupId.Equals(group.id)));

            foreach (ApplicationRoleGroup role in group.roles)
            {
                string currentRoleId = role.roleId;
                foreach (ApplicationUser user in groupsUsers)
                {
                    int groupsWithRole = user.groups.Count(g => g.group.roles.Any(r => r.roleId == currentRoleId));

                    if (groupsWithRole == 1)
                    {
                        RemoveFromRole(user.Id, role.role.Name);
                    }
                }
            }
            group.roles.Clear();
            base.getContext().SaveChanges();
        }

        public void ClearUserGroups(string userId)
        {
            ClearUserRoles(userId);
            ApplicationUser user = base.getContext().Users.Find(userId);
            user.groups.Clear();
            base.getContext().SaveChanges();
        }

        public void ClearUserRoles(string userId)
        {
            ApplicationUser user = _userManager.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();

            currentRoles.AddRange(user.Roles);
            foreach (IdentityUserRole role in currentRoles)
            {
                var roleName = _roleManager.FindById(role.RoleId);
                _userManager.RemoveFromRole(userId, roleName.ToString());
            }
        }

        public void CreateGroup(string groupName)
        {
            if (GroupNameExists(groupName))
            {
                throw new GroupExistsException(
                    "A group by that name already exists in the database. Please choose another name.");
            }

            var newGroup = new Group(groupName);
            base.insert(newGroup);
            base.save();
        }

        public IdentityResult createRole(string name, string description = "")
        {
            return _roleManager.Create(new ApplicationRole(name, description));
        }

        public void DeleteGroup(Int16 groupId)
        {
            Group group = base.getById(groupId);

            ClearGroupRoles(groupId);
            base.getContext().groups.Remove(group);
            base.getContext().SaveChanges();
        }

        public void DeleteRole(string roleId)
        {
            IQueryable<ApplicationUser> roleUsers = base.getContext().Users.Where(u => u.Roles.Any(r => r.RoleId == roleId));
            ApplicationRole role = _roleManager.FindById(roleId);

            foreach (ApplicationUser user in roleUsers)
            {
                RemoveFromRole(user.Id, role.Name);
            }
            base.getContext().Roles.Remove(role);
            base.getContext().SaveChanges();
        }

        public Group findByName(string name)
        {
            return base.getContext().groups.First(g => g.name == name);
        }

        public List<Group> GetAllGroup()
        {
            return base.get().ToList();
        }

        public bool GroupNameExists(string groupName)
        {
            return base.getContext().groups.Any(gr => gr.name == groupName);
        }

        public void RemoveFromRole(string userId, string roleName)
        {
            _userManager.RemoveFromRole(userId, roleName);
        }

        public void removeUserRole()
        {
            throw new NotImplementedException();
        }

        public bool RoleExists(string name)
        {
            return _roleManager.RoleExists(name);
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