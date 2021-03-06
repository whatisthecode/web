﻿using Microsoft.AspNet.Identity.EntityFramework;
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 id { get; set; }

        public Int16 status { get; set; }

        [Column("first_name")]
        public String firstName { get; set; }

        [Column("last_name")]
        public String lastName { get; set; }

        [Column("dob", TypeName = "date")]
        public DateTime? dob { get; set; }

        [Column("identity_number")]
        public String identityNumber { get; set; }

        public Int16 postAmount { get; set; }

        public UserInfo(Int16 status, string firstName, string lastName, DateTime dob, string identityNumber)
        {
            this.status = status;
            this.firstName = firstName;
            this.lastName = lastName;
            this.dob = dob;
            this.identityNumber = identityNumber;
        }

        public UserInfo()
        {
            this.status = 0;
        }
    }
}