using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication2.DAO;
using WebApplication2.Models;
using WebApplication2.Models.IdentityModels;
using WebApplication2.Models.Mapping;

namespace WebApplication2.Controllers.API
{
    [RoutePrefix("api")]
    public class RoleController : ApiController
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationRole> _roleManager;

        private GroupRoleManagerDAO groupRoleManagerDao;
        private GroupDAO groupDAO;
        private UserGroupDAO userGroupDAO;

        public RoleController()
        {
            this.groupRoleManagerDao = new GroupRoleManagerDAOImpl();
            this._roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(new DBContext()));
            this._userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new DBContext()));
            this.groupDAO = new GroupDAOImpl();
            this.userGroupDAO = new UserGroupDAOImpl();
        }

        public RoleController(ApplicationUserManager userManager, ApplicationRoleManager roleManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
            RoleManager = roleManager;
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return (ApplicationRoleManager)_roleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                this._roleManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return (ApplicationUserManager)_userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        [HttpPost]
        [Route("role/create")]
        public IHttpActionResult create()
        {
            var result = this._roleManager.CreateAsync(new ApplicationRole("Admin", "Global Access"));
            Response response = new Response();
            if (!result.Result.Succeeded)
            {
                response.code = "500";
                response.status = "Cant create role";
                return Content<Response>(HttpStatusCode.InternalServerError, response);
            }
            this._roleManager.CreateAsync(new ApplicationRole("CanEditUser", "Add, modify, and delete Users"));
            this._roleManager.CreateAsync(new ApplicationRole("CanEditGroup", "Add, modify, and delete Groups"));
            this._roleManager.CreateAsync(new ApplicationRole("CanEditRole", "Add, modify, and delete roles"));
            this._roleManager.CreateAsync(new ApplicationRole("User", "Restricted to business domain activity"));
            this._roleManager.CreateAsync(new ApplicationRole("Merchant", ""));

            response.code = "201";
            response.status = "Create Success";
            return Content<Response>(HttpStatusCode.Created, response);
        }

        [HttpPost]
        [Route("group/create")]
        public IHttpActionResult AddGroups()
        {
            string[] _initialGroupNames = new string[] { "SuperAdmins", "GroupAdmins", "UserAdmins", "Users" };
            foreach (var groupName in _initialGroupNames)
            {
                Group group = new Group();
                group.name = groupName;
                this.groupDAO.insertGroup(group);
                this.groupDAO.saveGroup();
            }
            Response response = new Response("201", "Created", "");
            return Content<Response>(HttpStatusCode.Created, response);
        }

        [HttpPost]
        [Route("add/role-to-group")]
        public IHttpActionResult AddRoleToGroup()
        {
            string[] _superAdminRoleNames =
                new string[] { "Admin", "CanEditUser", "CanEditGroup", "CanEditRole", "User", "Merchant" };
            string[] _groupAdminRoleNames =
                new string[] { "CanEditUser", "CanEditGroup", "User" };
            string[] _userAdminRoleNames =
                new string[] { "CanEditUser", "User" };
            string[] _userRoleNames =
                new string[] { "User" };

            // Add the Super-Admin Roles to the Super-Admin Group:
            Group superAdmins = this.groupDAO.getGroupByName("SuperAdmins");
            foreach (string name in _superAdminRoleNames)
            {
                groupRoleManagerDao.AddRoleToGroup(superAdmins.id, name);
            }

            // Add the Group-Admin Roles to the Group-Admin Group:
            Group groupAdmins = this.groupDAO.getGroupByName("GroupAdmins");
            foreach (string name in _groupAdminRoleNames)
            {
                groupRoleManagerDao.AddRoleToGroup(groupAdmins.id, name);
            }

            // Add the User-Admin Roles to the User-Admin Group:
            Group userAdmins = this.groupDAO.getGroupByName("UserAdmins");
            foreach (string name in _userAdminRoleNames)
            {
                groupRoleManagerDao.AddRoleToGroup(userAdmins.id, name);
            }

            // Add the User Roles to the Users Group:
            Group users = this.groupDAO.getGroupByName("Users");
            foreach (string name in _userRoleNames)
            {
                groupRoleManagerDao.AddRoleToGroup(users.id, name);
            }

            Response response = new Response("201", "Created", "");
            return Content<Response>(HttpStatusCode.Created, response);
        }

        [HttpPost]
        [Route("group/add/user-to-group")]
        public async Task<IHttpActionResult> AddUserToGroup([FromBody]RoleViewModel roleViewModel)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(roleViewModel.username);
            foreach (var group in roleViewModel.groups)
            {
                Group gr = this.groupDAO.getGroupByName(group);
                ApplicationUserGroup userGroup = new ApplicationUserGroup();
                userGroup.groupId = gr.id;
                userGroup.userId = user.Id;
                this.userGroupDAO.AddUserToGroup(userGroup);
                this.userGroupDAO.saveUserGroup();
            }
            Response response = new Response("200", "created", "");
            return Content<Response>(HttpStatusCode.Created, response);
        }

        [HttpDelete]
        [Route("delete-user-group/{userId}")]
        public IHttpActionResult DeleteUserGroup([FromUri]string userId)
        {
            this.userGroupDAO.ClearUserGroups(userId);
            Response response = new Response("200", "Clear user groups success", "");
            return Content<Response>(HttpStatusCode.OK, response);
        }
    }
}
