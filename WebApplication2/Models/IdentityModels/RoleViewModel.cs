using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.IdentityModels
{
    public class RoleViewModel
    {
        public RoleViewModel() { }

        public List<string> groups { get; set;}
        public string username { get; set; }
    }
}