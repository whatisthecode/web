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
        public Int16 userId { get; set; }

        public String type { get; set; }

        public Double point { get; set; }

        public UserInfo UserInfo { get; set; }

        public Rating()
        {
            this.point = 2.5;
        }
    }
}