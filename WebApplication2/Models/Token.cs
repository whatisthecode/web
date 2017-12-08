using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Token : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 id { get; set; }

        [Column("access_token")]
        public string accessToken { get; set; }

        [Column("token_type")]
        public string tokenType { get; set; }

        [Column("expires_in")]
        public int expiresIn { get; set; }

        [Column("user_name")]
        public string userName { get; set; }

        [Column(".issued", TypeName = "datetime2")]
        public DateTime issued { get; set; }
    
        [Column(".expires", TypeName = "datetime2")]
        public DateTime expires { get; set; }

        public Token()
        {

        }
    }
}