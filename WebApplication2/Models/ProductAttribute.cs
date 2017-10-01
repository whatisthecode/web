using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class ProductAttribute : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 id { get; set; }
        [ForeignKey("Product")]
        [Column("product")]
        [Index(IsUnique = true)]
        public Int16 product { get; set; }
        [Index(IsUnique = true)]
        [StringLength(128)]
        public String key { get; set; }
        public String value { get; set; }
        public Product Product { get; set; }
        public ProductAttribute(short product, string key, string value)
        {
            this.product = product;
            this.key = key;
            this.value = value;
        }
    }
}