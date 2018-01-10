using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using LaptopWebsite.Models.Mapping;

namespace WindowsFormsApp1
{
    public partial class Product : Form
    {
        static Uri baseUrl = new Uri("http://localhost:54962/");
        string token = Login.LoginInfo.token;

        public Product()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Product_Add pr = new Product_Add();
            pr.ShowDialog();
        }
        public class SearchResult
        {
            public string id { get; set; }
            public string status { get; set; }
            public string name  { get; set; }
            public string code { get; set; }
            public string shortDescription { get; set; }
            public string longDescription { get; set; }
        }
        public class CategoriesResult
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public void getProductList() {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = httpClient.GetAsync("api/products/?pageIndex=1&pageSize=10&order=name").Result;
            if (response.IsSuccessStatusCode)
            {
                WebApplication2.Models.Mapping.Response content = response.Content.ReadAsAsync<WebApplication2.Models.Mapping.Response>().Result;
                PagedResult<WebApplication2.Models.Product> pageResult = ((JObject)content.results).ToObject<PagedResult<WebApplication2.Models.Product>>();
                IList<WebApplication2.Models.Product> searchResults = new List<WebApplication2.Models.Product>();
                foreach (WebApplication2.Models.Product item in pageResult.items)
                {
                    WebApplication2.Models.Product pr = item;
                    searchResults.Add(pr);
                }
                productView.DataSource = searchResults;
            }

        }

        public void getCategoryList()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            var response = httpClient.GetAsync("api/category-type/2").Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                JObject obj = JObject.Parse(result);
                IList<JToken> cateresults = obj["results"].Children().ToList();
                IList<CategoriesResult> cateResults = new List<CategoriesResult>();
                foreach (JToken item in cateresults)
                {
                    CategoriesResult searchResult = JsonConvert.DeserializeObject<CategoriesResult>(item.ToString());
                    cateResults.Add(searchResult);
                    Console.WriteLine(searchResult);
                }
                cateFilter.DataSource = cateResults;
                cateFilter.DisplayMember = "name";
                cateFilter.ValueMember = "id";
            }
        }
        private void Product_Load(object sender, EventArgs e)
        {
            this.getProductList();
            this.getCategoryList();
        }

        private void productView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public string idProduct { get; set; }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(productView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Xin chọn 1 dòng");
            }
            else
            {
                idProduct = productView.SelectedRows[0].Cells[0].Value.ToString();
                Product_Edit pe = new Product_Edit(idProduct);
                pe.ShowDialog();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (productView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Xin chọn 1 dòng");
            }
            else
            {
                idProduct = productView.SelectedRows[0].Cells[0].Value.ToString();
                DialogResult result = MessageBox.Show("Do You Want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result.Equals(DialogResult.OK))
                {
                    try
                    {
                        var token = Login.LoginInfo.token;
                        HttpClient httpClient = new HttpClient();
                        httpClient.BaseAddress = baseUrl;
                        var response = httpClient.DeleteAsync("api/product/"+idProduct).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Delete successful");
                        }

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Cannot delete ! Please try again");
                    }
                }
                else
                {
                    MessageBox.Show("DEk OK");
                }
            }
        }

        public void getProductByCate(object idCate)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            var response = httpClient.GetAsync("api/category/"+idCate+ "/products?pageIndex=1&pageSize=10").Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                JObject temp = JObject.Parse(result);
                IList<JToken> results = temp["results"]["items"].Children().ToList();
                IList<SearchResult> searchResults = new List<SearchResult>();
                foreach (JToken item in results)
                {
                    SearchResult searchResult = JsonConvert.DeserializeObject<SearchResult>(item.ToString());
                    searchResults.Add(searchResult);
                }
                productView.DataSource = searchResults;
            }
        }
        public object SelectedValue { get; set; }

        private void cateFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedStatus = cateFilter.SelectedIndex;
            SelectedValue = cateFilter.SelectedValue;
            this.getProductByCate(SelectedValue);            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Manage mn = new Manage();
            mn.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.getProductList();
        }
    }
}
