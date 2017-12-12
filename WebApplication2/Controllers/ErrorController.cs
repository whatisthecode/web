using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult notFound()
        {
            return View();
        }

        public ActionResult forbidden()
        {
            return View();
        }
    }
}