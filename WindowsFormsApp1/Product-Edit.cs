using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebApplication2.Models.Mapping;
using WebApplication2.Models.RequestModel;

namespace WindowsFormsApp1
{
    public partial class Product_Edit : Form
    {
        public string idProduct;
        static Uri baseUrl = new Uri("http://localhost:54962/");
        public Product_Edit(string idProduct)
        {
            InitializeComponent();
            this.idProduct = idProduct;
        }
        public void GetCategories()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            var response = httpClient.GetAsync("api/category").Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                JObject obj = JObject.Parse(result);
                IList<JToken> cateresults = obj["results"].Children().ToList();
                IList<Product.CategoriesResult> cateResults = new List<Product.CategoriesResult>();
                foreach (JToken item in cateresults)
                {
                    Product.CategoriesResult searchResult = JsonConvert.DeserializeObject<Product.CategoriesResult>(item.ToString());
                    cateResults.Add(searchResult);
                }
                cbCategories.DataSource = cateResults;
                cbCategories.DisplayMember = "name";
                cbCategories.ValueMember = "id";
            }
        }
        private void Product_Edit_Load(object sender, EventArgs e)
        {
            var token = Login.LoginInfo.token;
            String uri = "/api/product/" + idProduct;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            var response = httpClient.GetAsync(uri).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                Response content = response.Content.ReadAsAsync<Response>().Result;
                ProductDetail product = ((JObject)content.results).ToObject<ProductDetail>();
                txtCode.Text = product.code;
                txtName.Text = product.name;
                txtShort.Text = product.shortDescription;
                txtLong.Text = product.longDescription;
                this.GetCategories();
                foreach ( var item in product.attributes)
                {
                    if(item.key == "price")
                    {
                        txtPrice.Text = item.value;
                    }
                    if(item.key == "amount")
                    {
                        txtAmount.Text = item.value;
                    }
                    if(item.key == "discount")
                    {
                        txtDiscount.Text = item.value;
                    }
                    if(item.key == "color")
                    {
                        txtColor.Text = item.value;
                    }
                }
            }

        }
    }
}
