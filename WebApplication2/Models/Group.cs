using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public virtual Int16 id { get; set; }

        public virtual string name { get; set; }
        public virtual ICollection<ApplicationRoleGroup> roles { get; set; }
    }
}