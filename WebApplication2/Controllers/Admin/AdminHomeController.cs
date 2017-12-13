using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.CustomAttribute;

namespace WebApplication2.Controllers.Admin
{
    [MVCAuthorize]
    public class AdminHomeController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}