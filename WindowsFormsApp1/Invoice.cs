using LaptopWebsite.Models.Mapping;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Invoice : Form
    {
        static Uri baseUrl = new Uri("http://localhost:54962/");
        string token = Login.LoginInfo.token;

        public Invoice()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public class InvoiceData
        {
            public int id { get; set; }
            public string code { get; set; }
            public float total { get; set; }
        }
        public class InvoiceDetail
        {
            public int status { get; set; }
        }
        public void getInvoiceStatus(int invoiceId)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = httpClient.GetAsync("api/invoice/getby/" + invoiceId).Result;
            if (response.IsSuccessStatusCode)
            {
                WebApplication2.Models.Mapping.Response content = response.Content.ReadAsAsync<WebApplication2.Models.Mapping.Response>().Result;
                InvoiceDetail invoice = ((JObject)content.results).ToObject<InvoiceDetail>();
            }
        }
        public void getInvoiceList()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = httpClient.GetAsync("api/invoice?pageIndex=1&pageSize=10").Result;
            if (response.IsSuccessStatusCode)
            {
                WebApplication2.Models.Mapping.Response content = response.Content.ReadAsAsync<WebApplication2.Models.Mapping.Response>().Result;
                PagedResult<WebApplication2.Models.RequestModel.InvoiceView.SalerInvoice> pageResult = ((JObject)content.results).ToObject<PagedResult<WebApplication2.Models.RequestModel.InvoiceView.SalerInvoice>>();
                ICollection<WebApplication2.Models.RequestModel.InvoiceView.SalerInvoice> invoice = new List<WebApplication2.Models.RequestModel.InvoiceView.SalerInvoice>();
                IList<WebApplication2.Models.Invoice> searchResults = new List<WebApplication2.Models.Invoice>();
                DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
                foreach (WebApplication2.Models.RequestModel.InvoiceView.SalerInvoice item in pageResult.items)
                {
                    WebApplication2.Models.Invoice iv = item.invoice;
                    searchResults.Add(iv);
                }
                invoiceGV.DataSource = searchResults;
            }
        }
        private void Invoice_Load(object sender, EventArgs e)
        {
            this.getInvoiceList();
            Dictionary<Int32, String> status = new Dictionary<int, string>();
            status.Add(-1, "Đã bị hủy");
            status.Add(0, "Đang xử lý");
            status.Add(1, "Đang giao");
            status.Add(2, "Đã hoàn tất");
            cbStatus.DataSource = new BindingSource(status, null);
            cbStatus.DisplayMember = "Value";
            cbStatus.ValueMember = "Key";
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show(cbStatus.SelectedItem.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {

            int statusInDB = int.Parse(invoiceGV.SelectedRows[0].Cells[3].Value.ToString());
            int invoiceId = int.Parse(invoiceGV.SelectedRows[0].Cells[0].Value.ToString());
            int statusUpdate = int.Parse(selectedStatus.ToString());
            if (statusInDB == 1 && statusUpdate == 0)
            {
                MessageBox.Show("Đang giao không thể trở về đang xử lý");
            }
            else if (statusInDB == -1)
            {
                MessageBox.Show("Hóa đơn đã bị hủy không thể cập nhật trạng thái");
            }
            else if (statusInDB == 2)
            {
                MessageBox.Show("Hoá đơn đã hoàn thành không thể cập nhật trạng thái");
            }
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = baseUrl;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            JObject data = new JObject();
            data.Add("status", statusUpdate);
            HttpContent httpContent = new ObjectContent<JObject>(data, new JsonMediaTypeFormatter());
            var response = httpClient.PutAsync("api/invoice/"+ invoiceId,httpContent).Result;
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Update invoice successful");
                invoiceGV.Update();
                invoiceGV.Refresh();
            }
        }

        private void invoiceGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void invoiceGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var invoiceId = invoiceGV.SelectedRows[0].Cells[0].Value.ToString();
            var invoiceStatus = invoiceGV.SelectedRows[0].Cells[3].Value.ToString();
            switch (invoiceStatus)
            {
                case "-1":
                    invoiceStatus = "Đã bị Hủy";
                    break;
                case "0":
                    invoiceStatus = "Đang xử lý";
                    break;
                case "1":
                    invoiceStatus = "Đang giao hàng";
                    break;
                case "2":
                    invoiceStatus = "Đã giao";
                    break;
            }
            cbStatus.Text = invoiceStatus;
        }
       
        public object selectedStatus { get; set; }
        private void cbStatus_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            selectedStatus = cbStatus.SelectedValue;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.getInvoiceList();
        }
    }
}
