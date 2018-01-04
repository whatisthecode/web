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
                DataGridViewComboBoxColumn cb = new DataGridViewComboBoxColumn();
                cb.HeaderText = "StatusChange";
                cb.Name = "cbStatus";
                cb.Items.Add("-1");
                cb.Items.Add("0");
                cb.Items.Add("1");
                invoiceGV.Columns.Add(cb);
                this.invoiceGV.CellEndEdit += new DataGridViewCellEventHandler(invoiceGV_CellEndEdit);
                this.invoiceGV.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(invoiceGV_EditingControlShowing);
                invoiceGV.DataSource = searchResults;
            }
        }
        private void Invoice_Load(object sender, EventArgs e)
        {
            this.getInvoiceList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (invoiceGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Xin chọn 1 dòng");
            }
            else
            {
                var invoiceId = invoiceGV.SelectedRows[0].Cells[0].Value.ToString();
                MessageBox.Show(invoiceId);
            }
        }
        void invoiceGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (cbm != null)
            {
                cbm.SelectedIndexChanged -= new EventHandler(cbm_SelectedIndexChanged);
            }

        }
        ComboBox cbm;
        DataGridViewCell currentCell;
        void invoiceGV_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)

        {

            if (e.Control is ComboBox)

            {

                cbm = (ComboBox)e.Control;

                if (cbm != null)

                {

                    cbm.SelectedIndexChanged += new EventHandler(cbm_SelectedIndexChanged);
                }

                currentCell = this.invoiceGV.CurrentCell;

            }

        }



        void cbm_SelectedIndexChanged(object sender, EventArgs e)

        {
            var temp = cbm.SelectedValue;
            Console.WriteLine(temp);
            this.BeginInvoke(new MethodInvoker(EndEdit));
        }

        void EndEdit()
        {
            if (cbm != null)
            {
                DataRowView drv = cbm.SelectedItem as DataRowView;
                if (drv != null)
                {
                    this.invoiceGV[currentCell.ColumnIndex + 1, currentCell.RowIndex].Value = drv[2].ToString();
                    var temp = this.invoiceGV[currentCell.ColumnIndex + 1, currentCell.RowIndex].Value = drv[2].ToString();
                    Console.WriteLine(temp);
                    this.invoiceGV.EndEdit();
                }
            }
        }
    }
}
