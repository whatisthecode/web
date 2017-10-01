using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class User : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 id { get; set; }
        public String username { get; set; }
        public String password { get; set; }
        public String email { get; set; }
        public DateTime dob { get; set; }
        [Column("identity_number")]
        public String identityNumber { get; set; }
        [ForeignKey("UserType")]
        [Column("type")]
        public Int16 type { get; set; }
        public UserType UserType { get; set; }
        public ICollection<Invoice> invoices { get; set; }
        public User(string username, string password, string email, DateTime dob, string identityNumber, short type)
        {
            this.username = username;
            this.password = password;
            this.email = email;
            this.dob = dob;
            this.identityNumber = identityNumber;
            this.type = type;
        }
    }
}