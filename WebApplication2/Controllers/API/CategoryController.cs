using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication2.DAO;

namespace WebApplication2.Controllers.API
{
    public class CategoryController : ApiController
    {
        private CategoryDAO categoryDao;

        public CategoryController()
        {
            this.categoryDao = new CategoryDAOImpl();
        }

        public IHttpActionResult GetAll()
        {
            return Ok("OK");
        }

    }
}
