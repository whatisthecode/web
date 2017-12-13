﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using WebApplication2.Controllers.API;

namespace WebApplication2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Admin-Home",
                url: "dashboard",
                defaults: new { type = "Admin", controller = "AdminHome", action = "Index" }
            );
            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { type = "Admin", controller = "AdminLogin", action = "Index" }
            );
            routes.MapRoute(
                name: "Logout",
                url: "logout",
                defaults: new { type = "Admin", controller = "AdminLogin", action = "Logout" }
            );
            routes.MapRoute(
                name: "Create-Product",
                url: "dashboard/product/create",
                defaults: new { type = "Admin", controller = "AdminProduct", action = "Create" }
            );
            routes.MapRoute(
                name: "Product",
                url: "dashboard/products",
                defaults: new { type = "Admin", controller = "AdminProduct", action = "Index" }
            );
            routes.MapRoute(
                name: "Product-Detail",
                url: "dashboard/product/{id}",
                defaults: new { type = "Admin", controller = "AdminProduct", action = "Detail" }
            );
            routes.MapRoute(
                name: "Remove-Product",
                url: "dashboard/product/remove/{id}",
                defaults: new { type = "Admin", controller = "AdminProduct", action = "Remove" }
            );
            routes.MapRoute(
                name: "Not-Found",
                url: "notfound",
                defaults: new { type = "Admin", controller = "Error", action = "notfound" }
            );
            routes.MapRoute(
                name: "Forbidden",
                url: "forbidden",
                defaults: new { type = "Admin", controller = "Error", action = "forbidden" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{*whatever}",
                defaults: new { type = "Client", controller = "Home", action = "Index" }
            );
        }
    }
}
