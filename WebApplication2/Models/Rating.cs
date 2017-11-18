using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Rating
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 id { get; set; }

        [Column("user_id")]
        [ForeignKey("UserInfo")]
        [Index(IsUnique = true)]
        public Int16 userId { get; set; }

        [Index(IsUnique = true)]
        public String type { get; set; }

        public Double point { get; set; }

        public UserInfo UserInfo { get; set; }
    }
}