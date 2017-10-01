using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class UserTypeRole : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 id { get; set; }
        [ForeignKey("UserType")]
        [Column("type")]
        public Int16 type { get; set; }
        [ForeignKey("UserRole")]
        [Column("role")]
        public Int16 role { get; set; }
        public UserType UserType { get; set; }
        public UserRole UserRole { get; set; }
        public UserTypeRole(short type, short role)
        {
            this.type = type;
            this.role = role;
        }
    }
}