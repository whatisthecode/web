using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers.Admin
{
    public class AdminLoginController : Controller
    {
        static Uri baseUrl = new Uri("http://localhost:54962/");
        public AdminLoginController()
        {
        }
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(String email, String password)
        {
            ViewBag.email = email;
            ViewBag.password = password;
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

            return RedirectToAction("Index", "AdminHome");
            
        }
    }
}