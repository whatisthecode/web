using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models.Mapping;
using WebApplication2.Models.RequestModel;

namespace WebApplication2.Controllers.Admin
{
    public class AdminInfoController : Controller
    {
        public static Uri baseUrl = new Uri("http://localhost:54962/");
        // GET: AdminInfo
        public ActionResult Index()
        {
            String accessToken = Session["currentUser"].ToString();

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            var response = httpClient.GetAsync("api/user/info").Result;

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsAsync<Response>().Result;
                UserDetail userDetail = ((JObject)content.results).ToObject<UserDetail>();
                return View(userDetail);

            }
            return RedirectToAction("NotFound", "Error");
        }

        public ActionResult BuyingPost()
        {
            return View();
        }
    }
}