using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class UserRole : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 id { get; set; }

        public String code { get; set; }

        public String name { get; set; }

        public ICollection<UserTypeRole> types { get; set; }

        public UserRole(string code, string name)
        {
            this.code = code;
            this.name = name;
        }
    }
}