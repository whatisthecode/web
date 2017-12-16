using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.RequestModel
{
    public class InvoiceView
    {
        public class ViewInvoiceModel
        {
            public List<InvoiceProducts> products { get; set; }
            public Int16 buyer { get; set; }
            public Double total { get; set; }
            public ViewInvoiceModel()
            {

            }
        }

        public class InvoiceProducts
        {
            public Int16 productId { get; set; }
            public Int16 amount { get; set; }
            public double price { get; set; }
            public string name { get; set; }

            public InvoiceProducts()
            {

            }
        }

        public class SalerInvoice
        {
            public Invoice invoice { get; set; }

            public List<InvoiceDetail> details { get; set; }

            public SalerInvoice()
            {
                
            }
        }
    }
}