using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class UserInfo : Base
    {
        public Int16 id { get; set; }

        public Boolean status { get; set; }

        public String firstName { get; set; }

        public String lastName { get; set; }

        public DateTime? dob { get; set; }

        [Column("identity_number")]
        public String identityNumber { get; set; }

        public virtual ICollection<Invoice> buyerInvoices { get; set; }

        public virtual ICollection<Invoice> salerInvoices { get; set; }

        public virtual ICollection<Product> products { get; set; }

        public UserInfo(bool status, string firstName, string lastName, DateTime dob, string identityNumber)
        {
            this.status = status;
            this.firstName = firstName;
            this.lastName = lastName;
            this.dob = dob;
            this.identityNumber = identityNumber;
        }

        public UserInfo()
        {
            this.status = false;
        }
    }
}