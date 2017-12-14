using LaptopWebsite.Models.Mapping;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApplication2.CustomAttribute;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;

namespace WebApplication2.Controllers.Admin
{
    public class AdminUserController : Controller
    {
        static Uri baseUrl = new Uri("http://localhost:54962/");
        [MVCAuthorize(Roles = "VIEW_USER")]
        // GET: AdminUser
        public ActionResult Index(String pageSize, String pageIndex)
        {
            String accessToken = Session["currentUser"].ToString();
            String uri = "/api/users?pageSize=" + pageSize + "&pageIndex=" + pageIndex + "&order=id";
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            var response = httpClient.GetAsync(uri).Result;

            if (response.IsSuccessStatusCode)
            {
                Response content = response.Content.ReadAsAsync<Response>().Result;
                PagedResult<UserInfo> pageResult = ((JObject)content.results).ToObject<PagedResult<UserInfo>>();
                ICollection<UserInfo> userInfos = new List<UserInfo>();
                if (pageResult.items.Count > 0)
                    userInfos = pageResult.items;
                return View(userInfos);
            }
            else if(response.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("Forbidden", "Error");
            }
            return RedirectToAction("NotFound", "Error");
        }
    }
}