using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication2.DAO;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;
using static WebApplication2.Models.Mapping.CategoryViewModels;

namespace WebApplication2.Controllers.API
{
    public class CategoryTypeController : ApiController
    {
            
        public CategoryTypeController()
        {

        }

        [Route("api/category-type/{id}")]
        [HttpGet]
        public IHttpActionResult getCategoryType(short id)
        {
            Response response = new Response();
            IEnumerable<Category> categories = Service.categoryDAO.getCategoryByTypeId(id);
            response.code = "200";
            response.status = "Thành công";
            response.results = categories;
            return Content<Response>(HttpStatusCode.OK, response);
        }
    }
}
