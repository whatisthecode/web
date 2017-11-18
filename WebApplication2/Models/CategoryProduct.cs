﻿using System;
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
        [Column("category_id")]
        public Int16 categoryId { get; set; }

        [ForeignKey("Product")]
        [Column("product_id")]
        public Int16 productId { get; set; }

        public Category Category { get; set; }

        public Product Product { get; set; }

        public CategoryProduct()
        {

        }
        public CategoryProduct(Int16 categoryId, Int16 productId)
        {
            this.categoryId = categoryId;
            this.productId = productId;
        }
    }
}