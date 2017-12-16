using LaptopWebsite.Models.Mapping;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using WebApplication2.CustomAttribute;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;
using WebApplication2.Models.RequestModel;
using static WebApplication2.Models.AccountBindingModels;

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
                PagedResult<UserGeneral> pageResult = ((JObject)content.results).ToObject<PagedResult<UserGeneral>>();
                ICollection<UserGeneral> users = new List<UserGeneral>();
                if (pageResult.items.Count > 0)
                    users = pageResult.items;
                return View(users);
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("Forbidden", "Error");
            }
            return RedirectToAction("NotFound", "Error");
        }

        public ActionResult Detail(String id)
        {
            ViewBag.groups = Service.groupDAO.getGroup();
            String accessToken = Session["currentUser"].ToString();
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            var response = httpClient.GetAsync("api/user/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsAsync<Response>().Result;
                UserDetail userDetail = ((JObject)content.results).ToObject<UserDetail>();
                return View(userDetail);
            }
            return RedirectToAction("NotFound","Error");
        }
        [HttpPost]
        public ActionResult Detail(RegisterBindingModel createUser)
        {
            return null;
        }

        public ActionResult Create()
        {
            RegisterBindingModel createUser = new RegisterBindingModel();
            ViewBag.now = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            ViewBag.groups = Service.groupDAO.getGroup();
            return View(createUser);
        }

        [HttpPost]
        public ActionResult Create(RegisterBindingModel createUser)
        {
            ViewBag.now = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            ViewBag.groups = Service.groupDAO.getGroup();
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpContent httpContent = new ObjectContent<RegisterBindingModel>(createUser, new JsonMediaTypeFormatter());

            var response = httpClient.PostAsync("api/account/register", httpContent).Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsAsync<Response>().Result;
                ApplicationUser user = ((JObject)content.results).ToObject<ApplicationUser>();
                return RedirectToAction("Detail", "AdminUser", new { id = user.Id });
            }
            return View(createUser);
        }
    }
}