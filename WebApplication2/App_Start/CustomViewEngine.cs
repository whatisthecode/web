using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2
{
    public class CustomViewEngine : RazorViewEngine
    {
        public CustomViewEngine() : base ()
        {
            var viewLocations = new[] {
                "~/Views/%1/{1}/{0}.cshtml",
                "~/Views/%1/Shared/{0}.cshtml",
                "~/Views/%1/{1}/{0}.aspx",
                "~/Views/%1/{1}/{0}.ascx",
            };

            this.PartialViewLocationFormats = viewLocations;
            this.ViewLocationFormats = viewLocations;
        }
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            var type = controllerContext.RouteData.Values["type"].ToString();
            if (type == "" || type != "Admin")
                type = "Client";
            else
                type = "Admin";
            return base.CreatePartialView(controllerContext, partialPath.Replace("%1", type));
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            var type = controllerContext.RouteData.Values["type"].ToString();
            if (type == "" || type !="Admin")
                type = "Client";
            else
                type = "Admin";
            return base.CreateView(controllerContext, viewPath.Replace("%1", type), masterPath);
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            var type = controllerContext.RouteData.Values["type"].ToString();
            if (type == "" || type != "Admin")
                type = "Client";
            else
                type = "Admin";
            return base.FileExists(controllerContext, virtualPath.Replace("%1", type));
        }
    }
}