using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WebApplication2;
using static WebApplication2.Models.AccountViewModels;
using static WebApplication2.Models.AccountBindingModels;
using WebApplication2.Results;
using WebApplication2.Providers;
using WebApplication2.Models;
using WebApplication2.DAO;
using WebApplication2.Models.Mapping;
using System.Net;
using System.Linq;

namespace WebAPI_NG_TokenbasedAuth.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private GroupRoleManagerDAO groupRoleManagerDAO;
        private UserInfoDAO userInfoDao;

        public AccountController()
        {
            userInfoDao = new UserInfoDAOImpl();
            this.groupRoleManagerDAO = new GroupRoleManagerDAOImp();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationRoleManager roleManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
            RoleManager = roleManager;
        }
        
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public IHttpActionResult GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
            UserInfo userInfo = new UserInfo();
            var user = UserManager.FindById(User.Identity.GetUserId());
            CurrentUserInfoLogin currentUserInfoLogin = new CurrentUserInfoLogin();
            currentUserInfoLogin.dob = user.userInfo.dob;
            currentUserInfoLogin.lastName = user.userInfo.lastName;
            currentUserInfoLogin.firstName = user.userInfo.firstName;
            currentUserInfoLogin.status = user.userInfo.status;
            currentUserInfoLogin.id = user.userInfo.id;
            currentUserInfoLogin.identityNumber = user.userInfo.identityNumber;
            //Get User Roles
            var currentRoles = new List<IdentityUserRole>();
            List<string> roles = new List<string>();
            currentRoles.AddRange(user.Roles);
            for(int i = 0; i < currentRoles.Count; i++)
            {
                var roleId = currentRoles[i].RoleId;
                var role = RoleManager.FindById(roleId);
                roles.Add(role.Name.ToString());
            }
            currentUserInfoLogin.roles = roles;

            UserInfoViewModel userInfoModel = new UserInfoViewModel();
            userInfoModel._id = user.Id;
            userInfoModel.Email = User.Identity.GetUserName();
            userInfoModel.HasRegistered = externalLogin == null;
            userInfoModel.LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null;
            userInfoModel.userInfo = currentUserInfoLogin;
            Response response = new Response();
            response.code = "200";
            response.status = "Success";
            response.results = userInfoModel;
            return Content<Response>(HttpStatusCode.OK, response);
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            Response response = new Response();
            if (!ModelState.IsValid)
            {
                response.code = "400";
                response.status = "Mật khẩu không hợp lệ";
                response.results = ModelState;
                return Content<Response>(HttpStatusCode.BadRequest, response);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                response.code = "500";
                response.status = "Internal Error Can't Change New Password";
                response.results = result;
                return Content<Response>(HttpStatusCode.InternalServerError, response);
            }

            response.code = "200";
            response.status = "Đổi mật khẩu thành công";
            return Content<Response>(HttpStatusCode.OK, response);
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            Response response = new Response();
            if (!ModelState.IsValid)
            {
                response.code = "400";
                response.status = "Mật khẩu không hợp lệ";
                response.results = ModelState;
                return Content<Response>(HttpStatusCode.BadRequest, response);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                response.code = "500";
                response.status = "Internal Error Can't Set New Password";
                response.results = result;
                return Content<Response>(HttpStatusCode.InternalServerError, response);
            }

            response.code = "200";
            response.status = "Đặt lại mật khẩu thành công";
            return Content<Response>(HttpStatusCode.OK, response);
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                
                 ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]//khong can dang nhap
        [Route("Register")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> Register([FromBody]RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Response response = new Response();

            UserInfo checkUser = userInfoDao.checkExist("identityNumber", model.identityNumber);
            if (checkUser != null)
            {
                response.code = "409";
                response.status = "Số chứng minh nhân dân đã được đăng ký";
                return Content<Response>(HttpStatusCode.Conflict, response);
            }

            UserInfo createUserInfo = new UserInfo();
            createUserInfo.firstName = model.firstName;
            createUserInfo.lastName = model.lastName;
            createUserInfo.identityNumber = model.identityNumber;
            createUserInfo.dob = model.dob;
            
            var identityUser = new ApplicationUser() { UserName = model.Email, Email = model.Email};
            identityUser.userInfo = createUserInfo;
            IdentityResult result = await UserManager.CreateAsync(identityUser, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            else
            {
                foreach (var group in model.groups)
                {
                    Group gr = groupRoleManagerDAO.findByName(group);
                    groupRoleManagerDAO.AddUserToGroup(identityUser.Id, gr.id);
                }
                string code = await this.UserManager.GenerateEmailConfirmationTokenAsync(identityUser.Id);
                var callbackUrl = new Uri(Url.Link("ConfirmEmail", new { userId = identityUser.Id, code = code }));
                await this.UserManager.SendEmailAsync(identityUser.Id,"Xác thực tài khoản của bạn","Vui lòng nhấn vào link sau: <a href=\""+ callbackUrl + "\">link</a>");
                var message = "Chúng tôi đã gửi email xác thực tài khoản vào mail " + identityUser.Email + " . Vui lòng kiểm tra email để xác thực.";
                response.code = "201";
                response.status = "Đăng ký thành công";
                response.results = message;
                return Content<Response>(HttpStatusCode.Created, response);
            }
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result); 
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion

        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmail")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId, string code)
        {
            Response response = new Response();
            if (userId == null || code == null)
            {
                response.code = "422";
                response.status = "Missing Required fields";
                return Content<Response>(HttpStatusCode.BadRequest, response);
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return Redirect("http://localhost:54962/account/confirm");
            }

            response.code = "500";
            response.status = "Không thể xác thực tài khoản";
            return Content<Response>(HttpStatusCode.BadRequest, response);
        }

        // POST: /Account/ForgotPassword
        [AllowAnonymous]
        [HttpPost]
        [Route("ForgotPassword", Name = "ForgotPasswordRoute")]
        public async Task<IHttpActionResult> ForgotPassword([FromBody]RegisterExternalBindingModel registerExternalBindingModel)
        {
            Response response = new Response();
            var user = await UserManager.FindByEmailAsync(registerExternalBindingModel.Email);
            if(user == null)
            {
                response.code = "404";
                response.status = "Email không tồn tại";
                Content<Response>(HttpStatusCode.NotFound, response);
            }

            var userConfirmed = await UserManager.IsEmailConfirmedAsync(user.Id);
            if(!userConfirmed)
            {
                response.code = "403";
                response.status = "Email chưa được xác thực";
                Content<Response>(HttpStatusCode.Forbidden, response);
            }
 
            string code = await this.UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = new Uri(Url.Link("ForgotPassword", new { userId = user.Id, code = code }));
            await this.UserManager.SendEmailAsync(user.Id, "Lấy lại mật khẩu", "Vui lòng nhấn vào link sau: <a href=\"" + callbackUrl + "\">link</a>");
            var message = "Chúng tôi đã gửi email lấy lại mật khẩu tài khoản vào mail " + user.Email + " . Vui lòng kiểm tra email để xác thực.";
            response.code = "200";
            response.status = "Thành công";
            response.results = message;
            return Content<Response>(HttpStatusCode.OK, response);
        }

        // GET: /Account/ForgotPasswordConfirm
        [AllowAnonymous]
        [HttpGet]
        [Route("ForgotPassword", Name = "ForgotPassword")]
        public async Task<IHttpActionResult> ForgotPasswordEmail(string userId, string code)
        {
            Response response = new Response();
            if (userId == null || code == null)
            {
                response.code = "422";
                response.status = "Missing Required fields";
                return Content<Response>(HttpStatusCode.BadRequest, response);
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return Redirect("http://www.google.com");
            }

            response.code = "500";
            response.status = "Không thể xác thực tài khoản";
            return Content<Response>(HttpStatusCode.BadRequest, response);
        }

        /**
         *  Update UserInfo
         *  Method PUT: /api/account/{id}
         *  Body : {
         *      "dob" : "",
         *      "identityNumber" : "",
         *      "firstName" : "",
         *      "lastName" : "",
         *      "roles" : []
         *  }
         *  Update cái gì thì truyền params đó vào
         */
        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> updateAccountInfo([FromUri]string id, [FromBody]CurrentUserInfoLogin currentUserInfoLogin)
        {
            Response response = new Response();
            if(id == null)
            {
                response.code = "400";
                response.status = "Missing Required fields";
                return Content<Response>(HttpStatusCode.BadRequest, response);
            }

            var flag = 0;
            var user = UserManager.FindById(id);
            var userInfoId = user.userInfo.id;
            UserInfo userInfo = new UserInfo();
            userInfo = userInfoDao.getUserInfo(userInfoId);
            if(currentUserInfoLogin.identityNumber != null)
            {
                userInfo.identityNumber = currentUserInfoLogin.identityNumber;
                flag++;
            }
            if(currentUserInfoLogin.firstName != null)
            {
                userInfo.firstName = currentUserInfoLogin.firstName;
                flag++;
            }
            if(currentUserInfoLogin.lastName != null)
            {
                userInfo.lastName = currentUserInfoLogin.lastName;
                flag++;
            }
            if(currentUserInfoLogin.dob != null)
            {
                userInfo.dob = currentUserInfoLogin.dob;
            }
            if(flag > 0)
            {
                userInfoDao.updateUserInfo(userInfo);
                userInfoDao.saveUserinfo();
            }
            
            if(currentUserInfoLogin.roles != null && currentUserInfoLogin.roles.Count > 0)
            {
                var currentRoles = new List<IdentityUserRole>();

                currentRoles.AddRange(user.Roles);
                foreach (var role in currentRoles)
                {
                    var roleName = RoleManager.FindById(role.RoleId);
                    var result = UserManager.RemoveFromRole(id, roleName.Name.ToString());
                }
                foreach (var role in currentUserInfoLogin.roles)
                {
                    var result = await UserManager.AddToRoleAsync(id, role);
                }
            }
            response.code = "200";
            response.status = "Success";
            return Content<Response>(HttpStatusCode.OK, response);
        }
    }
}
