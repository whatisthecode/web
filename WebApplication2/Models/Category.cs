using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Category : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 id { get; set; }

        public String code { get; set; }

        public String name { get; set; }

        [ForeignKey("CategoryType")]
        [Column("type_id")]
        public Int16 typeId { get; set; }

        [ForeignKey("parent")]
        [Column("parent_id")]
        public Int16? parentId { get; set; }

        public Category parent { get; set; }

        public ICollection<Category> subCategories { get; set; }

        public CategoryType CategoryType { get; set; }

        public Category(string code, string name)
        {
            this.code = code;
            this.name = name;
        }
        public Category()
        {

        }
    }
}