using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.RequestModel
{
    public class CreateImageModel
    {
        public Int16 productId { get; set; }
        public String url { get; set; }
        public String type { get; set; }
    }
}