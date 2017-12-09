using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class ApplicationRoleGroup : Base
    {
        [Key, Column("role_id", Order = 0)]
        [ForeignKey("ApplicationRole")]
        public String roleId { get; set; }

        [Key, Column("group_id", Order = 1)]
        [ForeignKey("Group")]
        public Int16 groupId { get; set; }

        public virtual ApplicationRole ApplicationRole { get; set; }

        public virtual Group Group { get; set; }
    }
}