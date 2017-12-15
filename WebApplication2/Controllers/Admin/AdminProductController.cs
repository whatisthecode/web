using LaptopWebsite.Models.Mapping;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
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
        [MVCAuthorize(Roles = "VIEW_PRODUCT")]
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

        [MVCAuthorize(Roles = "VIEW_PRODUCT, VIEW_PRODUCT_ATTRIBUTE, VIEW_CATEGORY_PRODUCT, VIEW_IMAGE")]
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
        [MVCAuthorize(Roles = "CREATE_PRODUCT")]
        public ActionResult Create()
        {
            ProductDetail product = new ProductDetail();
            ViewBag.categories = Service.categoryDAO.getCategory();
            return View(product);
        }

        [MVCAuthorize(Roles = "VIEW_PRODUCT")]
        [HttpPost]
        public ActionResult Create(ProductDetail product)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["currentUser"]);

            CreateProductModel createProductModel = new CreateProductModel();
            createProductModel.code = product.code;
            createProductModel.name = product.name;
            createProductModel.shortDescription = product.shortDescription;
            createProductModel.longDescription = product.longDescription;
            List<Int16> categories = new List<Int16>();
            foreach (var category in product.choseCategories)
            {
                categories.Add(category.categoryId);
            }
            createProductModel.categories = categories.ToArray<Int16>();
            JObject attributes = new JObject();
            foreach (var attribute in product.attributes)
            {
                attributes.Add(attribute.key, attribute.value);
            }
            createProductModel.attributes = attributes;
            List<JObject> thumbnails = new List<JObject>();
            foreach (var thumbnail in product.thumbnails)
            {
                JObject temp = new JObject();
                temp.Add("url", thumbnail.url);
                thumbnails.Add(temp);
            }
            createProductModel.thumbnails = thumbnails;
            List<JObject> details = new List<JObject>();
            foreach (var detail in product.details)
            {
                JObject temp = new JObject();
                temp.Add("url", detail.url);
                details.Add(temp);
            }
            createProductModel.details = details;

            HttpContent httpContent = new ObjectContent<CreateProductModel>(createProductModel, new JsonMediaTypeFormatter());
            var response = httpClient.PostAsync("api/product", httpContent).Result;
            if (response.IsSuccessStatusCode)
            {
                Product content = ((JObject)response.Content.ReadAsAsync<Response>().Result.results).ToObject<Product>();
                return RedirectToAction("Detail", "AdminProduct", new { id = content.id});
            }
            else
            {
                ViewBag.categories = Service.categoryDAO.getCategory();
                return View(product);
            }
        }
    }
}