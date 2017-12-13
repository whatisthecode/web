using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.RequestModel
{
    public class ProductDetail
    {
        public Int16 id { get; set; }

        public Int16 status { get; set; }

        public String code { get; set; }

        public String name { get; set; }

        public String shortDescription { get; set; }

        public String longDescription { get; set; }

        public UserInfo userInfo { get; set; }

        public ICollection<ProductAttribute> attributes { get; set; }

        public ICollection<CategoryProduct> choseCategories { get; set; }

        public ICollection<Image> thumbnails { get; set; }

        public ICollection<Image> details { get; set; }
    }
}