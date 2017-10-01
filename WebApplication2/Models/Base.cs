using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Base
    {
        [Column("created_at", TypeName = "datetime2")]
        public DateTime createdAt { get; set; }
        [Column("updated_at", TypeName = "datetime2")]
        public DateTime updatedAt { get; set; }
        public Base()
        {
            this.createdAt = DateTime.Now;
            this.updatedAt = DateTime.Now;
        }
    }
}