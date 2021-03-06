﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.RequestModel
{
    public class UserGeneral
    {
        public String Id { get; set; }
        public String email { get; set; }
        public Int16 status { get; set; }
        public DateTime createdAt { get; set; }
        public String fullname { get; set; }
        public Boolean isLogin { get; set; }
        public String groupName { get; set; }

        public UserGeneral()
        {

        }

        public UserGeneral(String Id, String email, String fullname, String groupName, Int16 status, Boolean isLogin, DateTime createdAt)
        {
            this.Id = Id;
            this.email = email;
            this.fullname = fullname;
            this.groupName = groupName;
            this.status = status;
            this.isLogin = isLogin;
            this.createdAt = createdAt;
        }
    }
}