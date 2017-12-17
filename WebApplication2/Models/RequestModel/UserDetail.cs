using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.RequestModel
{
    public class UserDetail
    {
        public String Id { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public String Password { get; set; }
        public Int16 status { get; set; }
        public String firstName { get; set; }
        public String lastName { get; set; }
        public String fullName
        {
            get
            {
                return this.firstName + " " + this.lastName;
            }
            private set
            {
            }
        }
        public DateTime dob {
            get
            {
                String temp = this.dobDisplay.Split('/')[2] + "/" + this.dobDisplay.Split('/')[1] + "/" + this.dobDisplay.Split('/')[0];
                return DateTime.Parse(temp);
            }
            private set
            {

            }
        }
        public String dobDisplay { get; set; }
        public String identityNumber { get; set; }
        public String groupName { get; set; }
        public DateTime createdAt { get; set; }
        public Boolean isLogin { get; set; }

        public UserDetail() { }
    }
}