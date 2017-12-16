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
using WebApplication2.Models.RequestModel;
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
        public IHttpActionResult getuser(String id)
        {
            Response response = new Response();
            UserDetail userDetail = Service.userInfoDAO.getUserDetail(id);
            if(userDetail != null)
            {
                response.code = "200";
                response.status = "Thông tin người dùng:";
                response.results = userDetail;
            }
            else
            {
                response.code = "404";
                response.status = "Không tìm thấy người dùng";
                response.results = null;
            }
            return Content<Response>(HttpStatusCode.OK, response);
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
                PagedResult<UserInfo> tempPv = Service.userInfoDAO.AdminPageView(user.userInfoId, pageRequest.pageIndex, pageRequest.pageSize, pageRequest.order);
                List<UserGeneral> users = new List<UserGeneral>();
                foreach(var item in tempPv.items)
                {
                    ApplicationUser appUser = DatabaseFactory.context.Users.Where(au => au.userInfoId == item.id).FirstOrDefault();
                    Token userToken = Service.tokenDAO.getByUsername(appUser.Email);
                    Boolean isLogin = false;
                    if (userToken != null)
                        isLogin = userToken.isLogin;
                    appUser.groups = Service.userGroupDAO.getUserGroupByUser(appUser.Id).ToList();
                    String groupName = "Customer";
                    foreach(var group in appUser.groups)
                    {
                        if(group.Group.name == "SuperAdmin")
                        {
                            groupName = "SuperAdmin";
                            break;
                        }
                        else if(group.Group.name == "Admin")
                        {
                            groupName = "Admin";
                            break;
                        }
                        else if(group.Group.name == "Merchant")
                        {
                            groupName = "Merchant";
                            break;
                        }
                    }
                    users.Add(new UserGeneral(appUser.Id,appUser.Email, item.firstName + " " + item.lastName, groupName, item.status, isLogin, item.createdAt));
                }
                PagedResult<UserGeneral> pv = new PagedResult<UserGeneral>();
                pv.currentPage = tempPv.currentPage;
                pv.pageCount = tempPv.pageCount;
                pv.pageSize = tempPv.pageSize;
                pv.rowCount = tempPv.rowCount;
                pv.items = users;

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
