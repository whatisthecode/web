using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public enum BaseAttribute
    {
        price, color, amount, discount, size
    }
    public class ProductAttribute : Base
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 id { get; set; }

        [ForeignKey("Product")]
        [Column("product_id")]
        [Index("IX_productId_key", 1, IsUnique = true)]
        public Int16 productId { get; set; }

        [Index("IX_productId_key", 2, IsUnique = true)]
        [StringLength(128)]
        public String key { get; set; }

        [NotMapped]
        public BaseAttribute attributes { get; set; }

        public String value { get; set; }

        public Product Product { get; set; }

        public ProductAttribute(Int16 productId, String key, String value)
        {
            this.productId = productId;
            this.key = key;
            this.value = value;
        }
        public ProductAttribute()
        {
            
        }
    }
}