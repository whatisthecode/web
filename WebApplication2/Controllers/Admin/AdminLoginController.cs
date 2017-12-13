using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Mvc;
using WebApplication2.CustomAttribute;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;
using static WebApplication2.Models.AccountViewModels;

namespace WebApplication2.Controllers.Admin
{
    public class AdminLoginController : Controller
    {
        static Uri baseUrl = new Uri("http://localhost:54962/");
        public AdminLoginController()
        {
        }
        // GET: Default
        [MVCAuthorize]
        public ActionResult Index(String returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Index(String email, String password, String returnUrl)
        {
            ViewBag.email = email;
            ViewBag.password = password;
            ViewBag.returnUrl = returnUrl;
            if (email == null || password == null)
            {
                return View();
            }
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            LoginModel loginModel = new LoginModel();
            loginModel.email = email;
            loginModel.password = password;
            HttpContent httpContent = new ObjectContent<LoginModel>(loginModel, new JsonMediaTypeFormatter());

            var reponse = httpClient.PostAsync("api/account/login", httpContent).Result;
            if (reponse.IsSuccessStatusCode)
            {
                var contents = ((JObject)reponse.Content.ReadAsAsync<Response>().Result.results).ToObject<Token>();
                Session["currentUser"] = contents.accessToken;

                HttpClient httpClient1 = new HttpClient();
                httpClient1.BaseAddress = baseUrl;
                httpClient1.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["currentUser"]);

                var response2 = httpClient1.GetAsync("api/account/userinfo").Result;
                if (response2.IsSuccessStatusCode)
                {
                    var content2 = ((JObject)response2.Content.ReadAsAsync<Response>().Result.results).ToObject<UserInfoViewModel>();
                    CurrentUserInfoLogin userInfo = ((JObject)content2.userInfo).ToObject<CurrentUserInfoLogin>();
                    Session["username"] = userInfo.firstName + " " + userInfo.lastName;

                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "AdminHome");
                }
                else
                {
                    ViewBag.error = "Sai tên đăng nhập hoặc mật khẩu";
                    return View();
                }

            }
            return View();
        }

        public ActionResult Logout()
        {
            String accessToken = Session["currentUser"].ToString();
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var reponse = httpClient.PostAsync("api/account/logout", null).Result;
            if (reponse.IsSuccessStatusCode)
            {
                Session["currentUser"] = null;
                Session["username"] = null;
            }
            return RedirectToAction("Index", "AdminLogin");
        }
    }
}