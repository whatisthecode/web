using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaptopWebsite.Models.Mapping
{
    public class Response
    {
        public string code { get; set; }
        public string status { get; set; }
        public Object results { get; set; }
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
        public int total { get; set; }

        public Response()
        {
        }

        public Response(string code, string status, object results, int pageSize, int pageIndex, int total)
        {
            this.code = code;
            this.status = status;
            this.results = results;
            this.pageSize = pageSize;
            this.pageIndex = pageIndex;
            this.total = total;
        }
    }
}