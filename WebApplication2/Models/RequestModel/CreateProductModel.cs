using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.RequestModel
{
    public class CreateProductModel
    {
        public Boolean status { get; set; }
        public String code { get; set; }
        public String name { get; set; }
        public String shortDescription { get; set; }
        public String longDescription { get; set; }
        public Int16 createdBy { get; set; }
        public Int16[] categoryId { get; set; }
        public CreateProductModel()
        {
        }
    }
}