using System;
using System.Collections.Generic;

namespace WebApplication2.Models
{
    public class AccountViewModels
    {
        // Models returned by AccountController actions.

        public class ExternalLoginViewModel
        {
            public string Name { get; set; }

            public string Url { get; set; }

            public string State { get; set; }
        }

        public class ManageInfoViewModel
        {
            public string LocalLoginProvider { get; set; }

            public string Email { get; set; }

            public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

            public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
        }

        public class UserInfoViewModel
        {
            public string _id { get; set; }
            public string Email { get; set; }

            public bool HasRegistered { get; set; }

            public string LoginProvider { get; set; }

            public object userInfo { get; set; }
        }

        public class UserLoginInfoViewModel
        {
            public string LoginProvider { get; set; }

            public string ProviderKey { get; set; }
        }

        public class CurrentUserInfoLogin
        {
            public Int16? id { get; set; }
            public Boolean status { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public DateTime? dob { get; set; }
            public string identityNumber { get; set; }
            public List<ApplicationUserGroup> groups { get; set; }
        }
    }
}