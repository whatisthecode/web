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
    public class ProductController : ApiController
    {
        private ProductDAO productDao;

        public ProductController()
        {
            this.productDao = new ProductDAOImpl();
        }

        [Route("api/product")]
        [HttpPost]
         public IHttpActionResult insertNewProduct([FromBody]Product product)
         {
             Response response = new Response();
             Product productcheck = this.productDao.checkexist(product);
            if (productcheck != null)
            {
                response.code = "409";
                response.status = "Trùng sản phẩm, xin nhập lại";
                return Content<Response>(HttpStatusCode.Conflict, response);

            }
            else
            {
                this.productDao.insertProduct(product);
                this.productDao.save();
                response.code = "200";
                response.results = "Thêm sản phẩm thành công";
                return Content<Response>(HttpStatusCode.OK, response);

            }
         }
    }
}
