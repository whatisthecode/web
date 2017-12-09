using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class ApplicationUserGroup : Base
    {
        public ApplicationUserGroup() : base()
        {

        }

        public ApplicationUserGroup(String userId, Int16 groupId)
        {
            this.userId = userId;
            this.groupId = groupId;
        }
        [Key, Column("user_id", Order = 0)]
        [ForeignKey("ApplicationUser")]
        public String userId { get; set; }

        [Key, Column("group_id", Order = 1)]
        [ForeignKey("Group")]
        public Int16 groupId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Group Group { get; set; }
    }
}