using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
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
            var response = httpClient.GetAsync("api/category-type/2").Result;
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

        public Int16 selectedCategories { get; set; }
        private void cbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public object selectedStatus { get; set; }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           selectedStatus = comboBox1.SelectedValue;
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
                Dictionary<Int32, String> status = new Dictionary<int, string>();
                status.Add(0, "Invisible");
                status.Add(1, "Available");
                comboBox1.DataSource = new BindingSource(status, null);
                comboBox1.DisplayMember = "Value";
                comboBox1.ValueMember = "Key";
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
           
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            Int16 status = Int16.Parse(selectedStatus.ToString());
            List<Int16> categories = new List<Int16>();
            categories.Add(selectedCategories);
            List<JObject> thumbnails = new List<JObject>();
            JObject tempThumbnail = new JObject();
            tempThumbnail.Add("url", selectedThumbnail);
            thumbnails.Add(tempThumbnail);
            List<JObject> details = new List<JObject>();
            JObject tempDetail = new JObject();
            tempDetail.Add("url", selectedDetail);
            details.Add(tempDetail);
            var token = Login.LoginInfo.token;
            var userId = Login.LoginInfo.id;
            string name = txtName.Text;
            string shortDes = txtShort.Text;
            string longDes = txtLong.Text;
            string price = txtPrice.Text;
            string amount = txtAmount.Text;
            string discount = txtDiscount.Text;
            string color = txtColor.Text;
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
                attributes.color = color;
                CreateProductModel cr = new CreateProductModel();
                cr.status = status;
                cr.name = name;
                cr.shortDescription = shortDes;
                cr.longDescription = longDes;
                cr.createdBy = userId;
                cr.attributes = attributes;
                cr.categories = categories.ToArray();
                cr.thumbnails = thumbnails;
                cr.details = details;
                HttpContent httpContent = new ObjectContent<CreateProductModel>(cr, new JsonMediaTypeFormatter());
                var response = httpClient.PutAsync("api/product/"+idProduct, httpContent).Result;
                var statusCode = response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Update successful");
                    this.Hide();
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        OpenFileDialog ofd_thumbnail = new OpenFileDialog();
        private JToken selectedThumbnail;
        private void btnThumbnail_Click_1(object sender, EventArgs e)
        {
            if (ofd_thumbnail.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedThumbnail = ofd_thumbnail.FileName;
                txtThumbnail.Text = ofd_thumbnail.FileName;
            }
        }

        OpenFileDialog ofd_detail = new OpenFileDialog();
        private JToken selectedDetail;
        private void btnDetail_Click_1(object sender, EventArgs e)
        {
            if (ofd_detail.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedDetail = ofd_detail.FileName;
                txtDetail.Text = ofd_detail.FileName;
            }
        }
    }
}
