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

        }
    }
}
