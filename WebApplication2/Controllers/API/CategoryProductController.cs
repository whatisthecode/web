using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication2.DAO;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;

namespace WebApplication2.Controllers.API
{
    public class CategoryProductController : ApiController
    {
            
        public CategoryProductController()
        {

        }

        public void insertCategoryProduct(CategoryProduct catepro)
        {
            Response response = new Response();
            response.code = "200";
            response.status = "Thành Công";
            Service.categoryProductDAO.insertCategoryProduct(catepro);
            Content<Response>(HttpStatusCode.OK, response);
        }

    }
}
