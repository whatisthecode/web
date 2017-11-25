using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Group : Base
    {
        public Group() { }


        public Group(string name) : this()
        {
            this.roles = new List<ApplicationRoleGroup>();
            this.name = name;
        }


        [Key]
        [Required]
        public virtual Int16 id { get; set; }

        public virtual string name { get; set; }
        public virtual ICollection<ApplicationRoleGroup> roles { get; set; }
    }
}