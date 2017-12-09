using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class ApplicationRoleGroup : Base
    {
        [Key, Column(Order = 0)]
        public string roleId { get; set; }
        [Key, Column(Order = 1)]
        public Int16 groupId { get; set; }

        public virtual ApplicationRole role { get; set; }
        public virtual Group group { get; set; }
    }
}