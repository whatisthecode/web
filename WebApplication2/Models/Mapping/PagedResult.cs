using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaptopWebsite.Models.Mapping
{
    public class PagedResult<T>
    {
        public IList<T> results { get; set; }
        public int currentPage { get; set; }
        public int pageCount {get;set;}
        public int pageSize { get; set; }
        public int rowCount { get; set; }
    }
}