using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.Mapping
{
    public class Response
    {
        public string code { get; set; }
        public string status { get; set; }
        public Array results { get; set; }

        public Response()
        {
        }
        public Response(string code, string status, Array results)
        {
            this.code = code;
            this.status = status;
            this.results = results;
        }
    }
}