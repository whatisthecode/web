using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Product : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 id { get; set; }
        public String code { get; set; }
        public String name { get; set; }
        [Column("short_description")]
        public String shortDescription { get; set; }
        [Column("long_description")]
        public String longDescription { get; set; }
        [ForeignKey("User")]
        [Column("created_by")]
        public Int16 createdBy { get; set; }
        public User User { get; set; }
        public ICollection<ProductAttribute> attributes { get; set; }
        public Product(string code, string name, string shortDescription, string longDescription, short createdBy)
        {
            this.code = code;
            this.name = name;
            this.shortDescription = shortDescription;
            this.longDescription = longDescription;
            this.createdBy = createdBy;
        }

        public Product()
        {

        }
    }
}