﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using WebApplication2.DAO;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;

namespace WebApplication2.Controllers.API
{
    public class CategoryController : ApiController
    {
        private CategoryDAO categoryDao;

        public CategoryController()
        {
            this.categoryDao = new CategoryDAOImpl();
        }

        [Route("api/category")]
        [HttpPost]
        public IHttpActionResult insert([FromBody]Category category)
        {
            Response response = new Response();
            if (this.categoryDao.checkExist(category) != null)
            {
                response = new Response("409", "Loại sản phẩm này đã tồn tại", null);
                return Content<Response>(HttpStatusCode.Conflict, response);
            }
            else
            {
                this.categoryDao.insertCategory(category);
                this.categoryDao.saveCategory();
                response = new Response("201", "Loại sản phẩm đã được thêm", category);
                return Content<Response>(HttpStatusCode.Created, response);
            }
        }

        [Route("api/category")]
        [HttpPut]
        public IHttpActionResult update([FromBody]Category category)
        {
            Category category1 = this.categoryDao.checkExist(category);
            Response response = new Response();
            if (category1 != null && category1.id != category.id)
            {
                response = new Response("409", "Loại sản phẩm đã tồn tại", null);
                return Content<Response>(HttpStatusCode.NotFound, response);
            }
            else if (this.categoryDao.getCategoryById(category.id) == null)
            {
                response = new Response("404", "Loại sản phẩm không tồn tại", null);
                return Content<Response>(HttpStatusCode.NotFound, response);
            }
            else
            {
                category1 = this.categoryDao.getCategoryById(category.id);
                category1.code = category.code;
                category1.name = category.name;
                this.categoryDao.updateCategory(category1);
                this.categoryDao.saveCategory();
                response = new Response("200", "Cập nhật loại sản phẩm thành công", category);
                return Content<Response>(HttpStatusCode.OK, response);
            }
        }

        [Route("api/category/{id}")]
        [HttpDelete]
        public IHttpActionResult delete(short id)
        {
            Category cate = this.categoryDao.getCategoryById(id);
            Response response = new Response();
            if (cate == null)
            {
                response.code = "404";
                response.status = "Category not exist";
                return Content<Response>(HttpStatusCode.NotFound, response);
            }

            this.categoryDao.deleteCategory(id);
            response.code = "200";
            response.status = "Xóa loại sản phẩm thành công";
            return Content<Response>(HttpStatusCode.OK, response);
        }

    }
}
