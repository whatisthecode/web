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
        private UserInfoDAO userdao;
        public UserController()
        {
            this.userdao = new UserInfoDAOImpl();
        }
        [Route("api/user/{id}")]
        [HttpGet]
        public IHttpActionResult getuser(Int16 id)
        {
            return null;
        }
        [Route("api/user/{id}")]
        [HttpGet]
        public IHttpActionResult getusers()
        {
            return null;
        }
    }
}
