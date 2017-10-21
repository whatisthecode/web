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
    public class UserController : ApiController
    {
        private UserDAO userdao;
        public UserController()
        {
            this.userdao = new UserDAOImpl();
        }
        [Route("api/user/{id}")]
        [HttpGet]
        public IHttpActionResult getuser(Int16 id)
        {
            User usertemp = this.userdao.getUserById(id);
            Response response = new Response();
            if (usertemp == null)
            {
                response.code = "404";
                response.status = "Tên đăng nhập không tìm thấy.";

                return Content<Response>(HttpStatusCode.NotFound, response);
                
            }
            response.code = "200";
            response.status = "Tên đăng nhập cần tìm: ";
            response.results = usertemp;
            return Content<Response>(HttpStatusCode.OK, response);
        }
    }
}
