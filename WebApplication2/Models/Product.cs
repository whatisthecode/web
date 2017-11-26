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

        public Int16 status { get; set; }

        public String code { get; set; }

        public String name { get; set; }

        [Column("short_description")]
        public String shortDescription { get; set; }

        [Column("long_description")]
        public String longDescription { get; set; }

        [Column("created_by")]
        public Int16 createdBy { get; set; }

        public virtual UserInfo userInfo { get; set; }

        public virtual ICollection<ProductAttribute> attributes { get; set; }

        public Product(string code, string name, Int16 status, string shortDescription, string longDescription, Int16 createdBy)
        {
            this.code = code;
            this.name = name;
            this.shortDescription = shortDescription;
            this.longDescription = longDescription;
            this.createdBy = createdBy;
            this.status = status;
        }
        public Product()
        {

        }
    }
}