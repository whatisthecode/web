using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication2.DAO;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;
using WebApplication2.Models.RequestModel;

namespace WebApplication2.Controllers.API
{
    [Authorize]
    public class ProductController : ApiController
    {
        private ProductDAO productDao;
        private UserInfoDAO userInfoDAO;
        private CategoryDAO categoryDAO;
        private CategoryProductDAO categoryProductDAO;

        public ProductController()
        {
            this.productDao = new ProductDAOImpl();
            this.userInfoDAO = new UserInfoDAOImpl();
            this.categoryDAO = new CategoryDAOImpl();
            this.categoryProductDAO = new CategoryProductDAOImpl();
        }
       
        [Route("api/product")]
        [HttpPost]
        public IHttpActionResult insertNewProduct([FromBody]CreateProductModel createProductModel)
        {
            CategoryProduct catepro = new CategoryProduct();
            Response response = new Response();
            Product productcheck = this.productDao.checkexist(createProductModel.code);
            UserInfo user = this.userInfoDAO.getUserInfo(createProductModel.createdBy);
            if(createProductModel.categoryId.Length < 1)
            {
                response.code = "400";
                response.status = "Missing require fields";
                return Content<Response>(HttpStatusCode.BadRequest, response);
            }
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
                //create Product object to insert data
                Product product = new Product();
                product.code = createProductModel.code;
                product.shortDescription = createProductModel.shortDescription;
                product.longDescription = createProductModel.longDescription;
                product.name = createProductModel.name;
                product.createdBy = createProductModel.createdBy;
                this.productDao.insertProduct(product);
                this.productDao.save();

                //insert data to category product
                Product newProduct = productDao.checkexist(product.code);
                for(int i = 0; i < createProductModel.categoryId.Length; i++)
                {
                    catepro.categoryId = createProductModel.categoryId[i];
                    catepro.productId = newProduct.id;
                }

                response.code = "201";
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
        public IHttpActionResult updateProduct([FromBody]Product product, Int16 id)
        {
            Response response = new Response();
            Product productcheck = this.productDao.getProduct(id);

            if (productcheck == null)
            {
                response.code = "404";
                response.status = "Sản phẩm cần cập nhật không có trong danh sách";
                return Content<Response>(HttpStatusCode.NotFound, response);

            }
            else
            {

                response.code = "200";
                response.status = "Cập nhật sản phẩm thành công";

                productcheck.name = product.name;
                productcheck.shortDescription = product.shortDescription;
                productcheck.longDescription = product.longDescription;
                productcheck.updatedAt = product.updatedAt;
                this.productDao.updateProduct(productcheck);
                this.productDao.save();
                return Content<Response>(HttpStatusCode.OK, response);
            }

        }
        
        [HttpDelete]
        [Route("api/product/{id}")]
        public IHttpActionResult deleteProduct(Int16 iddel)
        { 
            Response response = new Response();
            Product productcheck = this.productDao.getProduct(iddel);
            if (productcheck == null)
            {
                response.code = "404";
                response.status = "không tồn tại sản phẩm cần xóa";
                return Content<Response>(HttpStatusCode.NotFound,response);
            }
            else
            {
                this.productDao.deleteProduct(iddel);
                response.code = "200";
                response.status = "Xóa thành công";
                return Content<Response>(HttpStatusCode.OK, response);
            }

        }

    }
}
