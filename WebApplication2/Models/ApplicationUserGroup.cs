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

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 id { get; set; }

        [Column("user_id")]
        [ForeignKey("ApplicationUser")]
        [Index("IX_userId_groupId", 1, IsUnique = true)]
        public String userId { get; set; }

        [Column("group_id")]
        [ForeignKey("Group")]
        [Index("IX_userId_groupId", 2, IsUnique = true)]
        public Int16 groupId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public Group Group { get; set; }
    }
}