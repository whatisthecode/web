using LaptopWebsite.Models.Mapping;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApplication2.CustomAttribute;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;
using WebApplication2.Models.RequestModel;

namespace WebApplication2.Controllers.Admin
{
    public class AdminProductController : Controller
    {
        static Uri baseUrl = new Uri("http://localhost:54962/");

        // GET: Product
        [MVCAuthorize(Roles = "VIEW_PRODUCT" )]
        public ActionResult Index(String pageSize, String pageIndex)
        {
            String accessToken = "Bearer " + Session["currentUser"].ToString();
            String uri = "/api/products/?pageSize=" + pageSize + "&pageIndex=" + pageIndex + "&order=name";
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            httpClient.DefaultRequestHeaders.Add("Authorization", accessToken);

            var response = httpClient.GetAsync(uri).Result;

            if (response.IsSuccessStatusCode)
            {
                Response content = response.Content.ReadAsAsync<Response>().Result;
                PagedResult<Product> pageResult = ((JObject)content.results).ToObject<PagedResult<Product>>();
                ICollection<Product> products = new List<Product>();
                if (pageResult.items.Count > 0)
                    products = pageResult.items;
                return View(products);
            }

            return View();
        }
        [MVCAuthorize(Roles = "VIEW_PRODUCT")]
        public ActionResult Detail(Int16 id)
        {
            String accessToken = "Bearer " + Session["currentUser"].ToString();
            String uri = "/api/product/" + id;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;

            var response = httpClient.GetAsync(uri).Result;
            if (response.IsSuccessStatusCode)
            {
                Response content = response.Content.ReadAsAsync<Response>().Result;
                ProductDetail product = ((JObject)content.results).ToObject<ProductDetail>();
                ViewBag.categories = Service.categoryDAO.getCategory();               
                return View(product);
            }
            return View();
        }
    }
}