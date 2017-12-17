using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WebApplication2.Models
{
    public class AccountBindingModels
    {
        // Models used as parameters to AccountController actions.

        public class AddExternalLoginBindingModel
        {
            [Required]
            [Display(Name = "External access token")]
            public string ExternalAccessToken { get; set; }
        }

        public class ChangePasswordBindingModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public class RegisterBindingModel
        {
            [Required]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [StringLength(12, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Confirm Phone Number")]
            public string ConfirmPhoneNumber { get; set; }

            public DateTime dob {
                get
                {
                    String temp = this.dobDisplay.Split('/')[2] + "/" + this.dobDisplay.Split('/')[1] + "/" + this.dobDisplay.Split('/')[0];
                    return DateTime.Parse(temp);
                }
                private set { }
            }

            public String dobDisplay { get; set; }

            [Required]
            public string firstName { get; set; }
            [Required]
            public string lastName { get; set; }
            [Required]
            public string identityNumber { get; set; }
            [Required]
            public List<string> groups { get; set; }

            public String groupName { get; set; }

            public RegisterBindingModel()
            {

            }
        }
    }

    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string newPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string confirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        public LoginModel()
        {

        }
    }
}