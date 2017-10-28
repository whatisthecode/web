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
        private UserDAO userDao;
        public ProductController()
        {
            this.productDao = new ProductDAOImpl();
            this.userDao = new UserDAOImpl();
        }

        [Route("api/product")]
        [HttpPost]
        public IHttpActionResult insertNewProduct([FromBody]Product product)
        {
            Response response = new Response();
            Product productcheck = this.productDao.checkexist(product);
            User user = this.userDao.getUserById(product.createdBy);

            if (user == null)
            {
                response.code = "404";
                response.status = "Người dùng hiện tại chưa có quyền thêm sản phẩm";
                return Content<Response>(HttpStatusCode.NotFound, response);

            }
            else if (productcheck != null)
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
        [Route("api/product/{id}")]
        [HttpGet]
        public IHttpActionResult getProductWithConditions(short id)
        {
            Response response = new Response();
            Product productflag = this.productDao.getProduct(id);
            if (productflag == null)
            {
                response.code = "404";
                response.status = "Sản phẩm cần tìm không có trong danh sách";
                return Content<Response>(HttpStatusCode.NotFound, response);
            }
            else
            {
                response.code = "200";
                response.status = "Sản phẩm cần tìm là: ";
                response.results = productflag;
                return Content<Response>(HttpStatusCode.OK, response);
            }

        }
        [Route("api/product/")]
        [HttpGet]
        public IHttpActionResult getProductslist()
        {
            Response response = new Response();
            IEnumerable<Product> productlist = this.productDao.getProducts().ToList();
            response.code = "200";
            response.status = "Danh sách sản phẩm hiện tại: ";
            response.results = productlist;
            return Content<Response>(HttpStatusCode.OK, response);

        }
        [Route("api/product/{id}")]
        [HttpPut]
        public IHttpActionResult updateProduct([FromBody]Product product)
        {
            Response response = new Response();
            Product productcheck = this.productDao.checkexist(product);
            if (productcheck != null)
            {
                response.code = "404";
                response.status = "Sản phẩm cần cập nhật không có trong danh sách";
                return Content<Response>(HttpStatusCode.NotFound, response);

            }
            else
            {

                response.code = "200";
                response.status = "Cập nhật sản phẩm thành công";
                Product productcheck2 = this.productDao.getProduct(product.id);
                productcheck2.name = product.name;
                productcheck2.shortDescription = product.shortDescription;
                productcheck2.longDescription = product.longDescription;
                productcheck2.updatedAt = product.updatedAt;
                this.productDao.updateProduct(productcheck);
                this.productDao.save();
                return Content<Response>(HttpStatusCode.OK, response);
            }

        }


    }
}
