using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication2.CustomAttribute;
using WebApplication2.DAO;
using WebApplication2.Models;
using WebApplication2.Models.Mapping;

namespace WebApplication2.Controllers.API
{
    public class UserController : ApiController
    {
        public UserController()
        {
        }

        [APIAuthorize(Roles = "VIEW_USER")]
        [Route("api/user/{id}")]
        [HttpGet]
        public IHttpActionResult getuser(Int16 id)
        {
            return null;
        }
        [APIAuthorize(Roles = "VIEW_USER")]
        [Route("api/user")]
        [HttpGet]
        public IHttpActionResult getusers()
        {
            String accessToken = HttpContext.Current.Request.Headers.Get("Authorization").Replace("Bearer ", "");
            return null;
        }
    }
}
