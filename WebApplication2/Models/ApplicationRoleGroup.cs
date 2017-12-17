using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class ApplicationRoleGroup : Base
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 id { get; set; }

        [Column("role_id", Order = 0)]
        [ForeignKey("ApplicationRole")]
        [Index("IX_roleId_groupId", 1, IsUnique = true)]
        public String roleId { get; set; }

        [Column("group_id", Order = 1)]
        [ForeignKey("Group")]
        [Index("IX_roleId_groupId", 2, IsUnique = true)]
        public Int16 groupId { get; set; }

        public ApplicationRole ApplicationRole { get; set; }

        public Group Group { get; set; }

        public ApplicationRoleGroup()
        {

        }
    }
}