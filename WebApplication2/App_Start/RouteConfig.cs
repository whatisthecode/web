using System;
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
                name: "Default",
                url: "{*whatever}",
                defaults: new { type = "Client", controller = "Home", action = "Index" }
            );
        }
    }
}
