using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using WebApplication2.Models;
using WebApplication2.Models.RequestModel;
using Newtonsoft.Json.Linq;
using WebApplication2.Models.Mapping;
using Newtonsoft.Json;

namespace WindowsFormsApp1
{
    public partial class Product_Add : Form
    {
        static Uri baseUrl = new Uri("http://localhost:54962/");

        public Product_Add()
        {
            InitializeComponent();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
        public class SearchResult
        {
            public string id { get; set; }
            public string name { get; set; }
        }
        private void Product_Add_Load(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            var response = httpClient.GetAsync("api/category").Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                JObject obj = JObject.Parse(result);
                IList<JToken> results = obj["results"].Children().ToList();
                IList<SearchResult> searchResults = new List<SearchResult>();
                foreach (JToken item in results)
                {
                    SearchResult searchResult = JsonConvert.DeserializeObject<SearchResult>(item.ToString());
                    searchResults.Add(searchResult);
                }
                cbCategories.DataSource = searchResults;
                cbCategories.DisplayMember = "name";
                cbCategories.ValueMember = "id";
            }
        }

        public Int16 selectedCategories { get; set; }
        private void cbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedCategories = cbCategories.SelectedIndex;

        }
        public Int16 selectedStatus { get; set; }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedStatus = comboBox1.SelectedIndex;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Int16 status = selectedStatus;
            Console.WriteLine(status);
            List<Int16> categories = new List<Int16>();
            categories.Add(selectedCategories);
            var token = Login.LoginInfo.token;
            var userId = Login.LoginInfo.id;
            string code = txtCode.Text;
            string name = txtName.Text;
            string shortDes = txtShort.Text;
            string longDes = txtLong.Text;
            string price = txtPrice.Text;
            string amount = txtAmount.Text;
            string discount = txtDiscount.Text;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = baseUrl;
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                dynamic attributes = new JObject();
                attributes.price = price;
                attributes.amount = amount;
                attributes.discount = discount;
                attributes.color = "red";
                CreateProductModel cr = new CreateProductModel();
                cr.code = code;
                cr.name = name;
                cr.shortDescription = shortDes;
                cr.longDescription = longDes;
                cr.createdBy = userId;
                cr.attributes = attributes;
                cr.categories = categories.ToArray();
                Console.WriteLine(selectedCategories);
                HttpContent httpContent = new ObjectContent<CreateProductModel>(cr, new JsonMediaTypeFormatter());
                var response = httpClient.PostAsync("api/product", httpContent).Result;
                Console.WriteLine(response);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var s = JsonConvert.DeserializeObject(result);
                    Console.WriteLine(result);
                }
                else
                {
                    Console.WriteLine("Fail");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Fail quá fail");
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        OpenFileDialog ofd = new OpenFileDialog();
        private void btnThumbnail_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedFile = ofd.FileName;
                txtThumbnail.Text = ofd.FileName;
            }
        }
    }
}
