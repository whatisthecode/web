using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2
{
    public class CustomViewEngine : WebFormViewEngine
    {
        public CustomViewEngine()
        {
            var viewLocations = new[] {
                "~/Views/Client/{1}/{0}.cshtml",
                "~/Views/Client/{1}/{0}.ascx",
                "~/Views/Shared/{0}.aspx",
                "~/Views/Shared/{0}.ascx",
                "~/AnotherPath/Views/{0}.ascx"
            };

            this.PartialViewLocationFormats = viewLocations;
            this.ViewLocationFormats = viewLocations;
        }
    }
}