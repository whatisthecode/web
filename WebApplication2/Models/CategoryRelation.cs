using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class CategoryRelation : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 id { get; set; }

        [Column("parent_id")]
        [Index(IsUnique = true)]
        [ForeignKey("Parent")]
        public Int16 parentId { get; set; }

        [Column("child_id")]
        [Index(IsUnique = true)]
        [ForeignKey("Child")]
        public Int16 childId { get; set; }

        public Category Parent { get; set; }
        public Category Child { get; set; }
    }
}