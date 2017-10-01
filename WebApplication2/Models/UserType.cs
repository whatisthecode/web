using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class UserType : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 id { get; set; }
        public String code { get; set; }
        public String name { get; set; }
        public ICollection<User> users { get; set; }
        public ICollection<UserTypeRole> roles { get; set; }
        public UserType() { }
        public UserType(string code, string name)
        {
            this.code = code;
            this.name = name;
            this.createdAt = DateTime.Now;
            this.updatedAt = DateTime.Now;
        }
    }
}