using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class InvoiceDetail : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 id { get; set; }

        [ForeignKey("Invoice")]
        [Index(IsUnique = true)]
        public Int16 invoice { get; set; }

        [ForeignKey("Product")]
        [Index(IsUnique = true)]
        public Int16 product { get; set; }

        public Int16 amount { get; set; }

        public Int16 price { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Double subTotal
        {
            get
            {
                return this.price * this.amount;
            }
            private set
            {

            }
        }

        public Invoice Invoice { get; set; }

        public Product Product { get; set; }

        public InvoiceDetail(short invoice, short product, short amount, short price, double subTotal)
        {
            this.invoice = invoice;
            this.product = product;
            this.amount = amount;
            this.price = price;
            this.subTotal = subTotal;
        }
    }
}