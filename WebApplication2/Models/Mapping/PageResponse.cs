using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.Mapping
{
    public class PageResponse
    {
        public string code { get; set; }
        public string status { get; set; }
        public Object results { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }

        public PageResponse()
        {
        }

        public PageResponse(string code, string status, object results, int pageIndex, int PageSize, int total)
        {
            this.code = code;
            this.status = status;
            this.results = results;
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
            this.total = total;
        }
    }
}