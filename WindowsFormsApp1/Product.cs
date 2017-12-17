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

namespace WindowsFormsApp1
{
    public partial class Product : Form
    {
        static Uri baseUrl = new Uri("http://localhost:54962/");
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
            Hide();
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
        private void Product_Load(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();
            var json = wc.DownloadString(baseUrl+ "api/products/?pageIndex=1&pageSize=10&order=name");
            JObject googleSearch = JObject.Parse(json);
            IList<JToken> results = googleSearch["results"]["items"].Children().ToList();
            IList<SearchResult> searchResults = new List<SearchResult>();
            foreach (JToken result in results)
            {
                SearchResult searchResult = JsonConvert.DeserializeObject<SearchResult>(result.ToString());
                searchResults.Add(searchResult);
            }
            productView.DataSource = searchResults;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            var response = httpClient.GetAsync("api/category").Result;
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
                }
                cateFilter.DataSource = cateResults;
                cateFilter.DisplayMember = "name";
                cateFilter.ValueMember = "id";
            }

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
                        Console.WriteLine(response);
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("ok babe");
                            var resulted = response.Content.ReadAsStringAsync().Result;
                            Console.WriteLine(resulted);
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
    }
}
