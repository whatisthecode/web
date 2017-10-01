using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class CategoryProduct : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 id { get; set; }
        [ForeignKey("Category")]
        public Int16 category { get; set; }
        [ForeignKey("Product")]
        public Int16 product { get; set; }
        public Category Category { get; set; }
        public Product Product { get; set; }
        public CategoryProduct(Int16 category, Int16 product)
        {
            this.category = category;
            this.product = product;
        }
    }
}