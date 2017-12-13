using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.RequestModel
{
    public class FromUri
    {
        public class PageRequest
        {
            public Int16 pageSize { get; set; }
            public Int16 pageIndex { get; set; }
            public string order { get; set; }
            public bool ascending { get; set; }
        }

        public class AdminPageRequest
        {
            public Int16 pageSize { get; set; }
            public Int16 pageIndex { get; set; }
            public string order { get; set; }
            public bool ascending { get; set; }
            public Int16 userId { get; set; }
        }
    }
}