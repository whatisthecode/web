using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.RequestModel
{

    public class CreateProductModel
    {
        public static String[] required = { "name", "code", "createdBy", "categories",
            "attributes.price", "attributes.amount", "attributes.discount", "attributes.color"};

        public Int16 status { get; set; }
        public String code { get; set; }
        public String name { get; set; }
        public String shortDescription { get; set; }
        public String longDescription { get; set; }
        public Int16 createdBy { get; set; }

        public Int16[] categories { get; set; }

        public JObject attributes { get; set; }

        public List<JObject> thumbnails { get; set; }
        public List<JObject> details { get; set; }

        public CreateProductModel()
        {
        }
    }
}