using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using WebApplication2.DAO;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;
using WebApplication2.Models.RequestModel;
using static WebApplication2.Models.RequestModel.FromUri;

namespace WebApplication2.Controllers.API
{
    [Authorize]
    public class CategoryController : ApiController
    {

        public CategoryController()
        {

        }

        [Route("api/category")]
        [HttpPost]
        public IHttpActionResult insert([FromBody]Category category)
        {

            Response response = new Response();
            if (category.typeId <0 || category.typeId>3)
            {
                response.code = "202";
                response.status = ("Vui lòng chọn loại sản phẩm");
                return Content<Response>(HttpStatusCode.Conflict, response);

            }
            else if(Service.categoryDAO.checkExist(category) != null)
            {
                response = new Response("409", "Loại sản phẩm này đã tồn tại", null);
                return Content<Response>(HttpStatusCode.Conflict, response);
            }
            else
            {
                Service.categoryDAO.insertCategory(category);
                Service.categoryDAO.saveCategory();
                response = new Response("201", "Loại sản phẩm đã được thêm", category);
                return Content<Response>(HttpStatusCode.Created, response);
            }
        }

        [Route("api/category")]
        [HttpPut]
        public IHttpActionResult update([FromBody]Category category)
        {
            Category category1 = Service.categoryDAO.checkExist(category);
            Response response = new Response();
            if (Service.categoryDAO.getCategoryById(category.id) == null)
            {
                response = new Response("404", "Loại sản phẩm không tồn tại", null);
                return Content<Response>(HttpStatusCode.NotFound, response);
            }
            else if (category1 != null && category1.id != category.id)
            {
                response = new Response("409", "Loại sản phẩm đã tồn tại", null);
                return Content<Response>(HttpStatusCode.NotFound, response);
            }
            if (category1.typeId < 0 || category1.typeId > 3)
            {
                response.code = "202";
                response.status = ("Vui lòng chọn loại sản phẩm");
                return Content<Response>(HttpStatusCode.Conflict, response);

            }

            else
            {
                category1 = Service.categoryDAO.getCategoryById(category.id);
                category1.code = category.code;
                category1.name = category.name;
                Service.categoryDAO.updateCategory(category1);
                Service.categoryDAO.saveCategory();
                response = new Response("200", "Cập nhật loại sản phẩm thành công", category);
                return Content<Response>(HttpStatusCode.OK, response);
            }
        }

        [Route("api/category/{id}")]
        [HttpDelete]
        public IHttpActionResult delete(short id)
        {
            Category cate = Service.categoryDAO.getCategoryById(id);
            Response response = new Response();
            if (cate == null)
            {
                response.code = "404";
                response.status = "Category not exist";
                return Content<Response>(HttpStatusCode.NotFound, response);
            }

            Service.categoryDAO.deleteCategory(id);
            response.code = "200";
            response.status = "Xóa loại sản phẩm thành công";
            return Content<Response>(HttpStatusCode.OK, response);
        }

        [Route("api/category/{id}")]
        [HttpGet]
        public IHttpActionResult getCategory(short id)
        {
            Category categorytemp = Service.categoryDAO.getCategoryById(id);
            Response response = new Response();
            if (categorytemp == null)
            {
                response.code = "404";
                response.status = ("Không tìm thấy loại sản phẩm");
                return Content<Response>(HttpStatusCode.NotFound, response);
            }

            response.code = "200";
            response.status = "Loại sản phẩm cần tìm";
            response.results = categorytemp;
            return Content<Response>(HttpStatusCode.OK, response);

        }

        [Authorize(Roles = "VIEW_CATEGORY")]
        [Route("api/category/")]
        [HttpGet]
        public IHttpActionResult getCategories()
        {
            IEnumerable<Category> categories = Service.categoryDAO.getCategory();
            Response response = new Response();
            response.code = "200";
            response.status = "Thành công";
            response.results = categories.ToList();
            return Content<Response>(HttpStatusCode.OK, response);

        }

        [Route("api/category/{categoryId}/products/")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult getProductByCategory(Int16 categoryId, [FromUri] PageRequest pageRequest)
        {
            Response response = new Response();

            PagedResult<Product> pageResults = Service.categoryProductDAO.pageView(categoryId, pageRequest.pageIndex, pageRequest.pageSize, pageRequest.order, pageRequest.ascending);

            response.code = "200";
            response.status = "Thành công";
            response.results = pageResults;
            return Content<Response>(HttpStatusCode.OK, response); ;
        }

        [Route("api/category/{codeName}")]
        public IHttpActionResult getCategoryByCode([FromUri]string codeName)
        {
            Response response = new Response();
            if (codeName == "" || codeName == null)
            {
                response.code = "400";
                response.status = "Thiếu mã loại sản phẩm";
                return Content<Response>(HttpStatusCode.BadRequest, response);
            }

            return null;
        }
    }
}
