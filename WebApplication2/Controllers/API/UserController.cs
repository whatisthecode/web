using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication2.CustomAttribute;
using WebApplication2.DAO;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;
using static WebApplication2.Models.RequestModel.FromUri;

namespace WebApplication2.Controllers.API
{
    public class UserController : ApiController
    {
        public UserController()
        {
        }

        [APIAuthorize(Roles = "VIEW_USER")]
        [Route("api/user/{id}")]
        [HttpGet]
        public IHttpActionResult getuser(Int16 id)
        {
            return null;
        }
        [APIAuthorize(Roles = "VIEW_USER")]
        [Route("api/users")]
        [HttpGet]
        public IHttpActionResult getusers([FromUri] PageRequest pageRequest)
        {
            String accessToken = HttpContext.Current.Request.Headers.Get("Authorization").Replace("Bearer ", "");
            Token token = Service.tokenDAO.getByAccessToken(accessToken);
            ApplicationUser user = Service._userManager.FindByEmailAsync(token.userName).Result;
            Boolean isAdmin = false;
            Response response = new Response();
            foreach(var group in user.groups)
            {
                if(group.Group.name == "Admin" || group.Group.name == "SuperAdmin")
                {
                    isAdmin = true;
                }
            }
            if (isAdmin)
            {
                PagedResult<UserInfo> pv = Service.userInfoDAO.AdminPageView(user.userInfoId, pageRequest.pageIndex, pageRequest.pageSize, pageRequest.order);
                response.code = "200";
                response.status = "Danh sách người dùng :";
                response.results = pv;
                return Content<Response>(HttpStatusCode.OK, response);
            }
            else
            {
                response.status = "Bạn không có phân quyền cho chức năng này";
                response.code = "403";
                response.results = null;
                return Content<Response>(HttpStatusCode.Forbidden, response);
            }
        }
    }
}
