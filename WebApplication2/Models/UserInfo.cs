using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class UserInfo : Base
    {
        public Int16 id { get; set; }
        public Boolean? status { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime? dob { get; set; }
        [Column("identity_number")]
        public String identityNumber { get; set; }
        [Column("type")]
        public Int16 type { get; set; }
        public UserType UserType { get; set; }
        public virtual ICollection<Invoice> invoices { get; set; }
        public virtual ICollection<Product> products { get; set; }

        public UserInfo(bool status, string firstName, string lastName, DateTime dob, string identityNumber, short type)
        {
            this.status = status;
            this.firstName = firstName;
            this.lastName = lastName;
            this.dob = dob;
            this.identityNumber = identityNumber;
            this.type = type;
        }

        public UserInfo()
        {

        }
    }
}