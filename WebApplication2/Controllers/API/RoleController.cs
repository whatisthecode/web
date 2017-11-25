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
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        private GroupRoleManagerDao groupRoleManagerDao;

        public RoleController()
        {
            this.groupRoleManagerDao = new GroupRoleManagerDaoImp();
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
                return _roleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
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
            var result = groupRoleManagerDao.createRole("Admin", "Global Access");
            Response response = new Response();
            if (!result.Succeeded)
            {
                response.code = "500";
                response.status = "Cant create role";
                return Content<Response>(HttpStatusCode.InternalServerError, response);
            }
            groupRoleManagerDao.createRole("CanEditUser", "Add, modify, and delete Users");
            groupRoleManagerDao.createRole("CanEditGroup", "Add, modify, and delete Groups");
            groupRoleManagerDao.createRole("CanEditRole", "Add, modify, and delete roles");
            groupRoleManagerDao.createRole("User", "Restricted to business domain activity");

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
                groupRoleManagerDao.CreateGroup(groupName);
            }
            Response response = new Response("201", "Created", "");
            return Content<Response>(HttpStatusCode.Created, response);
        }

        [HttpPost]
        [Route("add/role-to-group")]
        public IHttpActionResult AddRoleToGroup()
        {
            string[] _superAdminRoleNames =
                new string[] { "Admin", "CanEditUser", "CanEditGroup", "CanEditRole", "User" };
            string[] _groupAdminRoleNames =
                new string[] { "CanEditUser", "CanEditGroup", "User" };
            string[] _userAdminRoleNames =
                new string[] { "CanEditUser", "User" };
            string[] _userRoleNames =
                new string[] { "User" };

            // Add the Super-Admin Roles to the Super-Admin Group:
            Group superAdmins = groupRoleManagerDao.findByName("SuperAdmins");
            foreach (string name in _superAdminRoleNames)
            {
                groupRoleManagerDao.AddRoleToGroup(superAdmins.id, name);
            }

            // Add the Group-Admin Roles to the Group-Admin Group:
            Group groupAdmins = groupRoleManagerDao.findByName("GroupAdmins");
            foreach (string name in _groupAdminRoleNames)
            {
                groupRoleManagerDao.AddRoleToGroup(groupAdmins.id, name);
            }

            // Add the User-Admin Roles to the User-Admin Group:
            Group userAdmins = groupRoleManagerDao.findByName("UserAdmins");
            foreach (string name in _userAdminRoleNames)
            {
                groupRoleManagerDao.AddRoleToGroup(userAdmins.id, name);
            }

            // Add the User Roles to the Users Group:
            Group users = groupRoleManagerDao.findByName("Users");
            foreach (string name in _userRoleNames)
            {
                groupRoleManagerDao.AddRoleToGroup(users.id, name);
            }

            Response response = new Response("201", "Created", "");
            return Content<Response>(HttpStatusCode.Created, response);
        }

        [HttpPost]
        [Route("add/user-to-group")]
        public async Task<IHttpActionResult> AddUserToGroup([FromBody]RoleViewModel roleViewModel)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(roleViewModel.username);
            foreach (var group in roleViewModel.groups)
            {
                Group gr = groupRoleManagerDao.findByName(group);
                groupRoleManagerDao.AddUserToGroup(user.Id, gr.id);
            }
            Response response = new Response("200", "created", "");
            return Content<Response>(HttpStatusCode.Created, response);
        }
    }
}
