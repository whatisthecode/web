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

        private void Product_Add_Load(object sender, EventArgs e)
        {
            ComboBox comboBox1 = new ComboBox();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Int16 status;

            string selectedItem = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            if(selectedItem == "Invisible")
            {
                status = 0;
            }
            else
            {
                status = 1;
            }
            var token = Login.LoginInfo.token;
            var userId = Login.LoginInfo.id;
            string code = txtCode.Text;
            string name = txtName.Text;
            string shortDes = txtShort.Text;
            string longDes = txtLong.Text;
            string price = txtPrice.Text;
            string amount = txtAmount.Text;
            string discount = txtDiscount.Text;
            short[] ct = new short[1];           
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
                cr.categories = ct;
                cr.attributes = attributes;
                HttpContent httpContent = new ObjectContent<CreateProductModel>(cr, new JsonMediaTypeFormatter());
                var response = httpClient.PostAsync("api/product", httpContent).Result;
                
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var s = JsonConvert.DeserializeObject(result);
                    Console.WriteLine(s);
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
    }
}
