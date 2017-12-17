using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 id { get; set; }

        public String url { get; set; }
        [ForeignKey("Product")]
        [Column("product_id")]
        public Int16 productId { get; set; }

        public String type { get; set; }

        public Product Product { get; set; }

        public Image()
        {
            
        }
        public Image(String url, Int16 productId)
        {
            this.url = url;
            this.productId = productId;
        }
    }
}