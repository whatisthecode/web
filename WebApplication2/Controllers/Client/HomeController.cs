﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.DAO;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Base
    {
        private CategoryDAO categoryDAO;
        public HomeController()
        {
            this.categoryDAO = new CategoryDAOImpl(); 
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}