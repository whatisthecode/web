using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Invoice_Edit : Form
    {
        public string invoiceId;

        public Invoice_Edit(string invoiceId)
        {
            InitializeComponent();
            this.invoiceId = invoiceId;
        }

        private void Invoice_Edit_Load(object sender, EventArgs e)
        {
            
        }
    }
}
