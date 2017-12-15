using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;

namespace WebApplication2.CustomAttribute
{
    public class MVCAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {

        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {

            if (filterContext.HttpContext.Session["currentUser"] == null)
            {
                if (filterContext.HttpContext.Request.Path != "/login")
                {
                    String returnUrl = filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.Url.AbsoluteUri);
                    filterContext.Result = new RedirectResult("/login?returnUrl=" + returnUrl);
                }
            }
            else
            {
                if (filterContext.HttpContext.Request.Path == "/login")
                {
                    filterContext.Result = new RedirectResult("/dashboard");
                }
                else
                {
                    var accessToken = filterContext.HttpContext.Session["currentUser"].ToString();
                    Token token = Service.tokenDAO.getByAccessToken(accessToken);
                    if (token == null)
                    {
                        filterContext.HttpContext.Session["currentUser"] = null;
                        filterContext.HttpContext.Session["username"] = null;
                        filterContext.HttpContext.Session["sidebar"] = null;
                        String returnUrl = filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.Url.AbsoluteUri);
                        filterContext.Result = new RedirectResult("/login?returnUrl=" + returnUrl);
                    }
                    else
                    {
                        if (!token.isLogin)
                        {
                            filterContext.HttpContext.Session["currentUser"] = null;
                            filterContext.HttpContext.Session["username"] = null;
                            filterContext.HttpContext.Session["sidebar"] = null;
                            String returnUrl = filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.Url.AbsoluteUri);
                            filterContext.Result = new RedirectResult("/login?returnUrl=" + returnUrl);
                        }
                        else
                        {
                            DateTime currentDate = DateTime.Now;
                            DateTime expiresDate = token.expires;
                            int compare = DateTime.Compare(currentDate, expiresDate);
                            if (compare >= 0)
                            {
                                filterContext.HttpContext.Session["currentUser"] = null;
                                filterContext.HttpContext.Session["username"] = null;
                                filterContext.HttpContext.Session["sidebar"] = null;
                                String returnUrl = filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.Url.AbsoluteUri);
                                filterContext.Result = new RedirectResult("/login?returnUrl=" + returnUrl);
                            }
                            else
                            {
                                ApplicationUser user = Service._userManager.FindByEmailAsync(token.userName).Result;
                                ICollection<IdentityUserRole> currentUserRoles = user.Roles;
                                Boolean isPassed = false;
                                if (!String.IsNullOrEmpty(Roles))
                                {
                                    foreach (IdentityUserRole userRole in currentUserRoles)
                                    {
                                        String roleId = userRole.RoleId;
                                        String roleName = Service._roleManager.FindByIdAsync(roleId).Result.Name;
                                        if (Roles.Contains(roleName))
                                        {
                                            isPassed = true;
                                            break;
                                        }
                                    }
                                    if (!isPassed)
                                    {
                                        filterContext.Result = new RedirectResult("/forbidden");
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }
    }

    public class APIAuthorize : System.Web.Http.AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {

            //var authentication = filterContext.HttpContext.GetOwinContext().Authentication;
            if (actionContext.Request.Headers.Authorization == null)
            {
                if (actionContext.Request.RequestUri.AbsolutePath != "/api/account/login")
                {
                    Response response = new Response();
                    response.status = "Lỗi xác thực người dùng!";
                    response.code = "401";
                    response.results = null;
                    actionContext.Response = actionContext.Request.CreateResponse<Response>(System.Net.HttpStatusCode.Unauthorized, response, new JsonMediaTypeFormatter());

                }
            }
            else
            {
                var accessToken = actionContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                Token token = Service.tokenDAO.getByAccessToken(accessToken);
                if (token == null)
                {
                    Response response = new Response();
                    response.status = "Phiên đăng nhập của bạn đã hết hạn!";
                    response.code = "401";
                    response.results = null;
                    actionContext.Response = actionContext.Request.CreateResponse<Response>(System.Net.HttpStatusCode.Unauthorized, response, new JsonMediaTypeFormatter());
                }
                else
                {
                    if (!token.isLogin)
                    {
                        Response response = new Response();
                        response.status = "Phiên đăng nhập của bạn đã hết hạn!";
                        response.code = "401";
                        response.results = null;
                        actionContext.Response = actionContext.Request.CreateResponse<Response>(System.Net.HttpStatusCode.Unauthorized, response, new JsonMediaTypeFormatter());
                    }
                    else
                    {
                        DateTime currentDate = DateTime.Now;
                        DateTime expiresDate = token.expires;
                        int compare = DateTime.Compare(currentDate, expiresDate);
                        if (compare >= 0)
                        {
                            Service.tokenDAO.delete(token.id);
                            Service.tokenDAO.save();
                            Response response = new Response();
                            response.status = "Phiên đăng nhập của bạn đã hết hạn!";
                            response.code = "401";
                            response.results = null;
                            actionContext.Response = actionContext.Request.CreateResponse<Response>(System.Net.HttpStatusCode.Unauthorized, response, new JsonMediaTypeFormatter());
                        }
                        else
                        {
                            ApplicationUser user = Service._userManager.FindByEmailAsync(token.userName).Result;
                            ICollection<IdentityUserRole> currentUserRoles = user.Roles;
                            Boolean isPassed = false;
                            if (!String.IsNullOrEmpty(Roles))
                            {
                                foreach (IdentityUserRole userRole in currentUserRoles)
                                {
                                    String roleId = userRole.RoleId;
                                    String roleName = Service._roleManager.FindByIdAsync(roleId).Result.Name;
                                    if (Roles.Contains(roleName))
                                    {
                                        isPassed = true;
                                        break;
                                    }
                                }
                                if (!isPassed)
                                {
                                    Response response = new Response();
                                    response.status = "Bạn không có phân quyền cho chức năng này!";
                                    response.code = "403";
                                    response.results = null;
                                    actionContext.Response = actionContext.Request.CreateResponse<Response>(System.Net.HttpStatusCode.Forbidden, response, new JsonMediaTypeFormatter());
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}