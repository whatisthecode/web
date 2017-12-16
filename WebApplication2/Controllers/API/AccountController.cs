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
using Microsoft.Owin.Testing;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Text;
using WebApplication2.CustomAttribute;

namespace WebAPI_NG_TokenbasedAuth.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";

        public AccountController()
        {

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
                return (ApplicationUserManager)Service._userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                Service._userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return (ApplicationRoleManager)Service._roleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                Service._roleManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [Route("UserInfo")]
        public async Task<IHttpActionResult> GetUserInfoAsync()
        {
            Response response = new Response();
            String accessToken = HttpContext.Current.Request.Headers.Get("Authorization").Replace("Bearer ","");
            if (accessToken != null)
            {
                Token token = Service.tokenDAO.getByAccessToken(accessToken);
                DateTime currentDate = DateTime.Now;
                DateTime expiresDate = token.expires;
                int result = DateTime.Compare(currentDate, expiresDate);
                if (result <= 0)
                {
                    UserInfo userInfo = new UserInfo();
                    var user = await Service._userManager.FindByEmailAsync(token.userName);
                    user.UserInfo = Service.userInfoDAO.getUserInfo(user.userInfoId);
                    CurrentUserInfoLogin currentUserInfoLogin = new CurrentUserInfoLogin();

                    user.groups = Service.userGroupDAO.getUserGroupByUser(user.Id).ToList();

                    currentUserInfoLogin.dob = user.UserInfo.dob;
                    currentUserInfoLogin.lastName = user.UserInfo.lastName;
                    currentUserInfoLogin.firstName = user.UserInfo.firstName;
                    currentUserInfoLogin.status = user.UserInfo.status;
                    currentUserInfoLogin.id = user.UserInfo.id;
                    currentUserInfoLogin.identityNumber = user.UserInfo.identityNumber;
                    currentUserInfoLogin.groups = user.groups.ToList();

                    UserInfoViewModel userInfoModel = new UserInfoViewModel();
                    userInfoModel._id = user.Id;
                    userInfoModel.Email = User.Identity.GetUserName();
                    userInfoModel.userInfo = currentUserInfoLogin;
                    response.code = "200";
                    response.status = "Success";
                    response.results = userInfoModel;
                    return Content<Response>(HttpStatusCode.OK, response);
                }
                else
                {
                    Service.tokenDAO.delete(token.id);     //remove token from database
                    Service.tokenDAO.save();
                    response.status = "Phiên đăng nhập của bạn đã hết hạn, vui lòng đăng nhập lại";
                    response.code = "401";
                    response.results = "";
                    return Content<Response>(HttpStatusCode.OK, response);
                }
            }
            else
            {
                response.status = "Phiên đăng nhập của bạn đã hết hạn, vui lòng đăng nhập lại";
                response.code = "401";
                response.results = "";
                return Content<Response>(HttpStatusCode.OK, response);
            }
        }

        // POST api/Account/Logout
        [HttpPost]
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            String accessToken = HttpContext.Current.Request.Headers.Get("Authorization").ToString().Replace("Bearer ","");
            Token token = Service.tokenDAO.getByAccessToken(accessToken);
            DateTime currentDate = DateTime.Now;
            DateTime expiresDate = token.expires;
            int compare = DateTime.Compare(currentDate, expiresDate);
            if(compare < 0)
            {
                token.isLogin = false;
                Service.tokenDAO.update(token);
                Service.tokenDAO.save();
            }
            else
            {
                Service.tokenDAO.delete(token.id);
                Service.tokenDAO.save();
            }
            return Ok();
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
        [AllowAnonymous]
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

            IdentityUser user = await UserManager.FindByEmailAsync(model.email);
            if (user.Id != null)
            {
                IdentityResult result = await UserManager.AddPasswordAsync(user.Id, model.newPassword);
                if (!result.Succeeded)
                {
                    response.code = "500";
                    response.status = "Internal Error Can't Set New Password";
                    response.results = result;
                    return Content<Response>(HttpStatusCode.InternalServerError, response);
                }
            }
            else
            {
                response.code = "404";
                response.status = "Email không tồn tại";
                return Content<Response>(HttpStatusCode.NotFound, response);
            }


            response.code = "200";
            response.status = "Đặt lại mật khẩu thành công";
            return Content<Response>(HttpStatusCode.OK, response);
        }

        // POST api/Account/Register
        [Route("Register")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> Register([FromBody]RegisterBindingModel model)
        {

            Response response = new Response();

            UserInfo checkUser = Service.userInfoDAO.checkExist("identityNumber", model.identityNumber);
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

            var identityUser = new ApplicationUser() { UserName = model.Email, Email = model.Email , PhoneNumber = model.PhoneNumber};
            identityUser.UserInfo = createUserInfo;
            IdentityResult result = await UserManager.CreateAsync(identityUser, model.Password);

            if (!result.Succeeded)
            {
                var message = "Đăng ký thất bại. Xin vui lòng đăng ky lại!";
                response.code = "400";
                response.status = "Đăng ký thất bại";
                response.results = message;
                return Content<Response>(HttpStatusCode.BadRequest, response);
            }
            else
            {
                if(model.groupName == "Merchant")
                {
                    model.groups.Add("Customer");
                    model.groups.Add("Merchant");
                }
                else
                {
                    model.groups.Add(model.groupName);
                }
                foreach (var group in model.groups)
                {
                    Group gr = Service.groupDAO.getGroupByName(group);
                    ApplicationUserGroup userGroup = new ApplicationUserGroup();
                    userGroup.groupId = gr.id;
                    userGroup.userId = identityUser.Id;
                    Service.userGroupDAO.AddUserToGroup(userGroup);
                    Service.userGroupDAO.saveUserGroup();
                }
                string code = await this.UserManager.GenerateEmailConfirmationTokenAsync(identityUser.Id);
                var callbackUrl = new Uri(Url.Link("ConfirmEmail", new { userId = identityUser.Id, code = code }));
                await this.UserManager.SendEmailAsync(identityUser.Id, "Xác thực tài khoản của bạn", "Vui lòng nhấn vào link sau: <a href=\"" + callbackUrl + "\">link</a>");
                var message = "Chúng tôi đã gửi email xác thực tài khoản vào mail " + identityUser.Email + " . Vui lòng kiểm tra email để xác thực.";
                response.code = "201";
                response.status = "Đăng ký thành công. " + message;
                response.results = identityUser;
                return Content<Response>(HttpStatusCode.Created, response);
            }
        }

        #region Helpers

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
            if (user == null)
            {
                response.code = "404";
                response.status = "Email không tồn tại";
                Content<Response>(HttpStatusCode.NotFound, response);
            }

            var userConfirmed = await UserManager.IsEmailConfirmedAsync(user.Id);
            if (!userConfirmed)
            {
                response.code = "403";
                response.status = "Email chưa được xác thực";
                Content<Response>(HttpStatusCode.Forbidden, response);
            }

            string code = await this.UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = new Uri(Url.Link("ForgotPassword", new { userId = user.Id, code = code }));
            await this.UserManager.SendEmailAsync(user.Id, "Lấy lại mật khẩu", "Vui lòng nhấn vào link sau: <a href=\"" + callbackUrl + "\">link</a>");
            var message = "Chúng tôi đã gửi email lấy lại mật khẩu tài khoản vào mail " + user.Email + " . Vui lòng kiểm tra email để đặt lại mật khẩu mới.";
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
            var result = await UserManager.FindByIdAsync(userId);
            return Redirect("http://localhost:54962/account/set-password");
        }

        /**
         *  Update UserInfo
         *  Method PUT: /api/account/{id}
         *  Body : {
         *      "dob" : "",
         *      "identityNumber" : "",
         *      "firstName" : "",
         *      "lastName" : ""
         *  }
         *  Update cái gì thì truyền params đó vào
         */
        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult updateAccountInfo([FromUri]string id, [FromBody]CurrentUserInfoLogin currentUserInfoLogin)
        {
            Response response = new Response();
            if (id == null)
            {
                response.code = "400";
                response.status = "Missing Required fields";
                return Content<Response>(HttpStatusCode.BadRequest, response);
            }

            var flag = 0;
            var user = UserManager.FindById(id);
            var userInfoId = user.UserInfo.id;
            UserInfo userInfo = new UserInfo();
            userInfo = Service.userInfoDAO.getUserInfo(userInfoId);
            if (currentUserInfoLogin.identityNumber != null)
            {
                userInfo.identityNumber = currentUserInfoLogin.identityNumber;
                flag++;
            }
            if (currentUserInfoLogin.firstName != null)
            {
                userInfo.firstName = currentUserInfoLogin.firstName;
                flag++;
            }
            if (currentUserInfoLogin.lastName != null)
            {
                userInfo.lastName = currentUserInfoLogin.lastName;
                flag++;
            }
            if (currentUserInfoLogin.dob != null)
            {
                userInfo.dob = currentUserInfoLogin.dob;
            }
            if (flag > 0)
            {
                Service.userInfoDAO.updateUserInfo(userInfo);
                Service.userInfoDAO.saveUserinfo();
            }
            response.code = "200";
            response.status = "Success";
            return Content<Response>(HttpStatusCode.OK, response);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IHttpActionResult> LoginUser(LoginModel loginModel)
        {
            Response response = new Response();
            if (loginModel == null)
            {
                return this.BadRequest("Invalid user data");
            }
            ApplicationUser identityUser = Service._userManager.FindByEmailAsync(loginModel.email).Result;
            if (identityUser != null)    //validate username
            {
                bool result = UserManager.CheckPassword(identityUser, loginModel.password); //validate user password
                if (result == true)
                {
                    Token validateToken = Service.tokenDAO.getByUsername(loginModel.email); //user have logined yet?
                    if (validateToken == null)      //User dont have token
                    {
                        var request = HttpContext.Current.Request;
                        var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + "Token";
                        using (var client = new HttpClient())
                        {
                            var requestParams = new List<KeyValuePair<string, string>>
                            {
                                new KeyValuePair<string, string>("grant_type", "password"),
                                new KeyValuePair<string, string>("username", loginModel.email),
                                new KeyValuePair<string, string>("password", loginModel.password)
                            };
                            var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                            var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
                            if (tokenServiceResponse.StatusCode == HttpStatusCode.OK)
                            {
                                var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                                var jsSerializer = new JavaScriptSerializer();
                                var responseData = jsSerializer.Deserialize<Dictionary<string, string>>(responseString);

                                Token token = new Token();
                                token.accessToken = responseData["access_token"];
                                token.userName = responseData["userName"];
                                token.expiresIn = int.Parse(responseData["expires_in"]);
                                token.expires = DateTime.Parse(responseData[".expires"]);
                                token.tokenType = responseData["token_type"];
                                token.issued = DateTime.Parse(responseData[".issued"]);
                                token.isLogin = true;

                                Service.tokenDAO.insert(token);
                                Service.tokenDAO.save();

                                response.status = "Đăng nhập thành công";
                                response.code = "200";
                                response.results = token;
                                return Content<Response>(HttpStatusCode.OK, response);
                            }
                        }
                    }
                    else //User have token
                    {
                        DateTime currentDate = DateTime.Now;
                        DateTime expiresDate = validateToken.expires;
                        int compare = DateTime.Compare(currentDate, expiresDate);
                        if (!validateToken.isLogin)
                        {
                            if (compare < 0)      //Token hasn't expired
                            {
                                Token token = Service.tokenDAO.getById(validateToken.id);
                                token.isLogin = true;
                                Service.tokenDAO.update(token);
                                Service.tokenDAO.save();
                                response.status = "Đăng nhập thành công";
                                response.code = "200";
                                response.results = token;
                                return Content<Response>(HttpStatusCode.OK, response);
                            }
                            else        //Token expired
                            {
                                Service.tokenDAO.delete(validateToken.id);     //remove token from database
                                Service.tokenDAO.save();
                                response.status = "Phiên đăng nhập của bạn đã hết hạn, vui lòng đăng nhập lại";
                                response.code = "401";
                                response.results = "";
                                return Content<Response>(HttpStatusCode.OK, response);
                            }
                        }
                        else
                        {
                            if(compare <= 0)
                            {
                                Token token = Service.tokenDAO.getById(validateToken.id);
                                response.status = "Đăng nhập thành công";
                                response.code = "200";
                                response.results = token;
                                return Content<Response>(HttpStatusCode.OK, response);
                            }
                            else
                            {
                                Service.tokenDAO.delete(validateToken.id);     //remove token from database
                                Service.tokenDAO.save();
                                response.status = "Phiên đăng nhập của bạn đã hết hạn, vui lòng đăng nhập lại";
                                response.code = "401";
                                response.results = "";
                                return Content<Response>(HttpStatusCode.OK, response);
                            }
                        }

                    }
                }   //wrong password
                else
                {
                    response.code = "404";
                    response.status = "Sai Mật khẩu";
                    response.results = "";
                    return Content<Response>(HttpStatusCode.BadRequest, response);
                }
            }
            else  //wrong username
            {
                response.code = "404";
                response.status = "Tài khoản không tồn tại";
                response.results = "";
                return Content<Response>(HttpStatusCode.BadRequest, response);
            }
            return Ok();
        }
    }
}
