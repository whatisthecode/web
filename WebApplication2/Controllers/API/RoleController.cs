using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication2.Models;
using WebApplication2.Models.IdentityModels;
using WebApplication2.Models.Mapping;

namespace WebApplication2.Controllers.API
{
    [RoutePrefix("api/role")]
    public class RoleController : ApiController
    {
        private ApplicationRoleManager _roleManager;

        public RoleController() { }
        public RoleController(ApplicationRoleManager roleManager)
        {
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

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> createAsync(RoleViewModel model)
        {
            var role = new ApplicationRole() { Name = model.name };
            var result = await RoleManager.CreateAsync(role);
            Response response = new Response();
            if(!result.Succeeded)
            {
                response.code = "500";
                response.status = "Cant create role";
                return Content<Response>(HttpStatusCode.InternalServerError, response);
            }

            response.code = "201";
            response.status = "Create Success";
            return Content<Response>(HttpStatusCode.Created, response);
        }
    }
}
