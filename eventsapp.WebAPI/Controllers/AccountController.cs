using eventsapp.WebAPI.Models;
using Eventsapp.Repositories;
using Eventsapp.Services;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using EventsApp.Domain.Enums;
using EventsApp.Framework;
using EventsApp.Framework.EmailsSender;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace eventsapp.WebAPI
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private INotificationsService notificationService;
        private ICurrentUser user;
        private IMailSender mailSender;
        private readonly IKeyedRepository<UserSubscription, int> _userSubscriptionRepository;
        private readonly IKeyedRepository<PreferredLanguage, int> _preferredLanguageRepository;
        private readonly IKeyedRepository<EventUser, int> _eventUserRepository;
        private readonly IKeyedRepository<UserCompany, int> _userCompanyRepository;
        private readonly IKeyedRepository<Event, int> _eventRepository;
        private readonly IUserService _userService;
        private readonly IKeyedRepository<AthleteVoucher, int> _athleteVouchersRepository;
        private readonly IKeyedRepository<GuestVoucher, int> _guestVouchersRepository;
        private readonly IKeyedRepository<User, int> _systemUserRepository;


        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountController(INotificationsService notifService, IMailSender mailSender,
            ICurrentUser user, IUserService _userService,
            IKeyedRepository<UserSubscription, int> userSubscriptionRepository,
            IKeyedRepository<EventUser, int> _eventUserRepository,
            IKeyedRepository<UserCompany, int> _userCompanyRepository,
            IKeyedRepository<Event, int> _eventRepository,
            IKeyedRepository<PreferredLanguage, int> _preferredLanguageRepository,
            IKeyedRepository<AthleteVoucher, int> _athleteVouchersRepository,
            IKeyedRepository<GuestVoucher, int> _guestVouchersRepository,
            IKeyedRepository<User, int> _systemUserRepository)
        {
            notificationService = notifService;
            this.mailSender = mailSender;
            this.user = user;
            this._userService = _userService;
            this._userSubscriptionRepository = userSubscriptionRepository;
            this._preferredLanguageRepository = _preferredLanguageRepository;
            this._eventUserRepository = _eventUserRepository;
            this._userCompanyRepository = _userCompanyRepository;
            this._eventRepository = _eventRepository;
            this._athleteVouchersRepository = _athleteVouchersRepository;
            this._guestVouchersRepository = _guestVouchersRepository;
            this._systemUserRepository = _systemUserRepository;
        }

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
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
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(OAuthDefaults.AuthenticationType);
            var iosUser = notificationService.GetIOSUsers(this.user.UserInfo.Id);
            return Json(new { result = "Successfuly Loged out" });
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(this.user.UserInfo.Id, model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(this.user.UserInfo.Id, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [Route("SetUserPassword")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> SetUserPassword(PasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(model.UserId, model.NewPassword);

            if (result.Succeeded)
            {
                var applicationUser = UserManager.FindById(model.UserId);
                applicationUser.EmailConfirmed = true;
                await UserManager.UpdateAsync(applicationUser);
            }
            return Ok(result.Succeeded ? "PasswordSet" : "Error");

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

            IdentityResult result = await UserManager.AddLoginAsync(this.user.UserInfo.Id,
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
                result = await UserManager.RemovePasswordAsync(this.user.UserInfo.Id);
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(this.user.UserInfo.Id,
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
                //  return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
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

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail")]
        public async System.Threading.Tasks.Task<string> ConfirmEmail(int userId, string code)
        {

            //userId = 13;
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "User Id Not Found";
            }
            code = HttpUtility.UrlDecode(code);
            code = code.Replace(" ", "+");
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            //TODO: Fixit .
            //if (result.Succeeded)
            //{
            //    // user.EmailConfirmationToken = null;
            //    await UserManager.UpdateAsync(user);
            //    return "ConfirmEmail";
            //}
            // user.EmailConfirmationToken = null;
            //return "Error";

            // Just by passing code verification for Now
            user.EmailConfirmed = true;
            await UserManager.UpdateAsync(user);
            return "ConfirmEmail";


        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                var user = await UserManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    //Generating a token
                    string resetToken = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var url = string.Format(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/#/page/resetPassword?userId={0}&code={1}", user.Id, HttpUtility.UrlEncode(resetToken));
                    await SendEmail("Reset your password", "Please reset your password.<div class=\"block-div\"> <a href=\"" + url + "\" class=\"btn-primary-brown\">Click Here</a></div>", user.FirstName, user.Email);

                    return Ok();
                }
                else
                {
                    return BadRequest("User not found");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPasswordWithToken")]
        public async Task<IHttpActionResult> ResetPasswordWithToken(ResetPasswordWithTokenViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Code = HttpUtility.UrlDecode(model.Code);
                model.Code = model.Code.Replace(" ", "+");
                model.UserId = model.UserId;

                //reset the password
                var result = await UserManager.ResetPasswordAsync(model.UserId, model.Code, model.Password);

                return Ok(result.Succeeded ? "PasswordReset" : "Error");
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("ResetPasswordAdmin")]
        public async Task<IHttpActionResult> ResetPasswordAdmin(ResetPasswordAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                string resetToken = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                //reset the password
                var result = await UserManager.ResetPasswordAsync(model.UserId, resetToken, model.NewPassword);

                return Ok(result.Succeeded ? "PasswordReset" : "Error");
            }

            return BadRequest(ModelState);
        }

        //[HttpPost]
        //[Route("ConfigureAdmin")]
        //public async Task<IHttpActionResult> ResetPasswordAdmin(ConfigureAdminModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByEmailAsync(model.Email);
        //        user.NoOfEvents = model.NoOfEvents;
        //        user.IsApproved = model.IsApproved; 
        //        var result = UserManager.Update(user); 
        //        return Ok(result.Succeeded ? "Success" : "Error");
        //    }

        //    return BadRequest(ModelState);
        //}


        [HttpPost]
        [Route("VerifyPhoneNumber")]
        public async Task<IHttpActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!UserManager.IsPhoneNumberConfirmed(this.user.UserInfo.Id))
            {
                var result = await UserManager.ChangePhoneNumberAsync(this.user.UserInfo.Id, UserManager.GetPhoneNumber(this.user.UserInfo.Id), model.Code);
                return Ok(result.Succeeded ? "PhoneVerified" : "Failed to verify phone");
            }
            else
            {
                return Ok("Phone Number is already verified");
            }
        }

        [HttpPost]
        [Route("ResendPhoneNumberCode")]
        public async Task<IHttpActionResult> ResendPhoneNumberCode(VerifyPhoneNumberViewModel model)
        {
            string phoneNumber = UserManager.GetPhoneNumber(this.user.UserInfo.Id);
            string code = await UserManager.GenerateChangePhoneNumberTokenAsync(this.user.UserInfo.Id, UserManager.GetPhoneNumber(this.user.UserInfo.Id));
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = phoneNumber,
                    Body = "Your security code is: " + code
                };
                // Send token
                await UserManager.SmsService.SendAsync(message);
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ResendConfirmEmail")]
        public async Task<IHttpActionResult> ResendConfirmEmail(ResetPasswordViewModel model)
        {
            var user = await UserManager.FindByEmailAsync(model.Email);
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            string url = string.Format(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/#/page/confirmEmail?userId={0}&code={1}", user.Id, HttpUtility.UrlEncode(code));
            await SendEmail("Confirm your account", "Please confirm your account.  <div class=\"block-div\"> <a href=\"" + url + "\" class=\"btn-primary-brown\"  >click here</a></div>", user.FirstName, user.Email);

            return Ok();
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            //MessageService messageService = new MessageService();
            //var smsResult = await messageService.SendMessage("SMS Testing From SBA", "SBIF Event", "923364404470, 971582590580");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (!String.IsNullOrEmpty(model.Response))
            //{
            //    string EncodedResponse = model.Response;
            //    bool IsCaptchaValid = (ReCaptchaClass.Validate(EncodedResponse) == "true" ? true : false);

            //    if (!IsCaptchaValid)
            //    {
            //        return BadRequest(ModelState);
            //    }
            //}
            if (model.RegistrationTypeId == (int?)RegistrationTypeEnum.Guest)
            {
                var athlete = await UserManager.FindByIdAsync((int)model.GuestOf);
                model.EventId = (int)athlete.EventId;

            }

            var user = new ApplicationUser()
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Photo = model.Photo,
                CreatedOn = DateTime.Now,
                IsActive = true,
                EmailConfirmed = true,
                EventId = model.EventId != 0 ? model.EventId : (int?)null,
                RegistrationTypeId = model.RegistrationTypeId != 0 ? model.RegistrationTypeId : (int?)RegistrationTypeEnum.Guest,
                AssignedDrinkVouchers = model.AssignedDrinkVouchers,
                AssignedFoodVouchers = model.AssignedFoodVouchers,
                GuestOf = model.GuestOf,
                Relation = model.Relation
            };
            if (model.RegistrationTypeId == (int?)RegistrationTypeEnum.Athlete || model.RegistrationTypeId == (int?)RegistrationTypeEnum.Guest || model.RegistrationTypeId == (int?)RegistrationTypeEnum.Official)
            {
                var data = this._systemUserRepository.Query(m => m.RegistrationTypeId == (int?)RegistrationTypeEnum.Athlete || m.RegistrationTypeId == (int?)RegistrationTypeEnum.Guest || m.RegistrationTypeId == (int?)RegistrationTypeEnum.Official)
                     .OrderByDescending(m => m.UserCode).FirstOrDefault();
                if (data.UserCode == null)
                    user.UserCode = 00001;
                else
                    user.UserCode = data.UserCode + 1;
            }
            var QRCode = "";
            var owinContext = Request.GetOwinContext();
            var manager = owinContext.GetUserManager<ApplicationUserManager>();
            var isEmailTaken = await UserManager.FindByEmailAsync(model.Email);

            if (isEmailTaken == null)
            {
                // New Athlete's System Registration 
                try
                {
                    IdentityResult identityResult = new IdentityResult();
                    // var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());

                    if (model.UserRole == "Admin")
                    {
                        identityResult = await UserManager.CreateAsync(user, model.Password);   // Add Admin User Account
                        var result1 = UserManager.AddToRole(user.Id, model.UserRole);      // Add Admin Role
                    }
                    else
                    {
                        identityResult = await UserManager.CreateAsync(user, model.Password);   // Add User Account
                        var result1 = UserManager.AddToRole(user.Id, "User");
                        // URL Should be /#/app/verifyVoucher/1037/57
                        //var userData = model.FirstName + "|" + model.PhoneNumber + "|" + model.Email + "|" + user.Id;
                        string FrontEndUrl = System.Configuration.ConfigurationManager.AppSettings["FrontEndUrl"];
                        var userData = FrontEndUrl + "/#/app/verifyVoucher/" + user.Id + "/" + model.EventId;

                        if (model.RegistrationTypeId == (int?)RegistrationTypeEnum.Athlete)
                        {
                            // QRCode = GetQRBase64(userData, Brushes.White, Brushes.Red,user.UserCode.ToString());
                            QRCode = GetQrcode(userData, 200, null, 0, Brushes.White, Brushes.Red, user.UserCode.ToString().PadLeft(5, '0'));
                            user.QrImage = QRCode;
                            await UserManager.UpdateAsync(user);
                        }
                        else if (model.RegistrationTypeId == (int?)RegistrationTypeEnum.Guest)
                        {
                            // QRCode = GetQRBase64(userData, "Blue");
                            QRCode = GetQrcode(userData, 200, null, 0, Brushes.White, Brushes.Blue, user.UserCode.ToString().PadLeft(5, '0'));
                            user.QrImage = QRCode;

                            await UserManager.UpdateAsync(user);
                        }
                        else if (model.RegistrationTypeId == (int?)RegistrationTypeEnum.Official)
                        {
                            // QRCode = GetQRBase64(userData, "Green");
                            QRCode = GetQrcode(userData, 200, null, 0, Brushes.White, Brushes.Green, user.UserCode.ToString().PadLeft(5, '0'));
                            user.QrImage = QRCode;

                            await UserManager.UpdateAsync(user);
                        }
                        if (model.EventId != 0)
                        {
                            var eventUserObj = new EventUser
                            {
                                UserId = user.Id,
                                EventId = model.EventId,
                                CreatedOn = System.DateTime.Now,
                                QRCode = QRCode

                            };
                            this._eventUserRepository.Insert(eventUserObj);
                            this._eventUserRepository.Commit();
                        }
                        //if (model.AthleteVouchers.Count > 0)
                        //    foreach (var voucher in model.AthleteVouchers)
                        //    {
                        //        if (model.RegistrationTypeId == (int?)RegistrationTypeEnum.Athlete)
                        //        {
                        //            var athleteVoucherObj = new AthleteVoucher
                        //            {
                        //                UserId = user.Id,
                        //                VoucherId = voucher.VoucherId,
                        //                IsValid = true

                        //            };
                        //            this._athleteVouchersRepository.Insert(athleteVoucherObj);
                        //            this._athleteVouchersRepository.Commit();
                        //        }
                        //        else if (model.RegistrationTypeId == (int?)RegistrationTypeEnum.Guest)
                        //        {
                        //            var guestVoucherObj = new GuestVoucher
                        //            {
                        //                userId = user.Id,
                        //                voucherId = voucher.VoucherId,
                        //                sharedBy = this.user.UserInfo.Id

                        //            };
                        //            this._guestVouchersRepository.Insert(guestVoucherObj);
                        //            this._guestVouchersRepository.Commit();
                        //        }


                        //    }
                    }

                    try
                    {
                        if (model.RegistrationTypeId == (int?)RegistrationTypeEnum.Guest)
                        {
                            var body = await createRegisterEmailBody(model.FirstName + " " + model.LastName, model.Email, model.Password, model.CompanyCode, QRCode, "", "", "", "", "", "", "", "", "", "", "");
                            var result = await SendEmail("DDF Guest Registration", body, model.FirstName, model.Email);

                        }
                    }
                    catch (Exception ex)
                    {
                        EventsApp.Framework.ExceptionHandling.ExceptionHandlingManager.HandleException(ex,
                           EventsApp.Framework.ExceptionHandling.ExceptionHandlingPolicies.ServiceLayer);
                    }


                    if (identityResult != null && !identityResult.Succeeded)
                    {
                        return GetErrorResult(identityResult);
                    }
                }
                catch (Exception ex)
                {
                    EventsApp.Framework.ExceptionHandling.ExceptionHandlingManager.HandleException(ex,
                       EventsApp.Framework.ExceptionHandling.ExceptionHandlingPolicies.ServiceLayer);
                    return InternalServerError(ex);
                }
            }
            else
            {
                ModelState.AddModelError("InvalidDataExceptionBase", "EmailTaken");
                return BadRequest(ModelState);
            }

            return Ok(new SimpleLoginData { UserId = user.Id, PhoneNumber = user.PhoneNumber });
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


        [Route("GetUsersById")]
        public async Task<IHttpActionResult> GetUsersById()
        {
            try
            {
                var user = await this._systemUserRepository.QueryAsync(x => x.Id == this.user.UserInfo.Id);
                var userDtos = MapperHelper.Map<List<UserDto>>(user);
                return Ok(userDtos);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }

        [AllowAnonymous]
        [Route("UpdateProfile")]
        public async Task<IHttpActionResult> UpdateProfile(UserDto model)
        {
            var owinContext = Request.GetOwinContext();

            var manager = owinContext.GetUserManager<ApplicationUserManager>();
            var regUser = await UserManager.FindByEmailAsync(model.Email);

            if (regUser != null)
            {
                try
                {
                    var user = UserManager.FindByName(model.Email);


                    user.UserName = model.Email;
                    user.Email = model.Email;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Photo = model.Photo;
                    //user.CreatedOn = DateTime.Now;
                    user.IsActive = (bool)model.IsActive;
                    user.EventId = model.EventId;
                    user.AssignedFoodVouchers = model.AssignedFoodVouchers;
                    user.AssignedDrinkVouchers = model.AssignedDrinkVouchers;
                    user.Relation = model.Relation;

                    IdentityResult identityResult = await UserManager.UpdateAsync(user);

                    if (user.RegistrationTypeId == (int?)RegistrationTypeEnum.Athlete)
                    {
                        var guestsList = (await this._systemUserRepository.QueryAsync(x => x.GuestOf == user.Id)).ToList();
                        foreach (var guest in guestsList)
                        {
                            guest.IsActive = user.IsActive;
                        }
                    }

                    foreach (var voucher in model.AthleteVouchers)
                    {
                        var isExisting = this._athleteVouchersRepository.Query(x => x.VoucherId == voucher.Id && x.UserId == user.Id);
                        if (isExisting == null)
                        {
                            var athleteVoucherObj = new AthleteVoucher
                            {
                                UserId = user.Id,
                                VoucherId = voucher.VoucherId,
                                IsValid = true

                            };
                            this._athleteVouchersRepository.Insert(athleteVoucherObj);
                            this._athleteVouchersRepository.Commit();

                        }

                    }
                    if (!identityResult.Succeeded)
                    {
                        return GetErrorResult(identityResult);
                    }

                    var EventUser = (await this._eventUserRepository.QueryAsync(u => u.UserId == model.Id && u.EventId == model.EventId)).FirstOrDefault();
                    if (EventUser == null)
                    {
                        var NewEventUser = new EventUser();
                        NewEventUser.UserId = model.Id;
                        NewEventUser.EventId = (int)model.EventId;
                        NewEventUser.QRCode = model.QrImage;
                        NewEventUser.RegistrationTypeId = (int)model.RegistrationTypeId;
                        NewEventUser.IsApproved = model.IsActive;
                        NewEventUser.CreatedOn = System.DateTime.Now;
                        this._eventUserRepository.Insert(NewEventUser);
                        this._eventUserRepository.Commit();
                    }
                    else
                    {
                        EventUser.QRCode = model.QrImage;
                        EventUser.RegistrationTypeId = (int)model.RegistrationTypeId;
                        EventUser.IsApproved = model.IsActive;
                        EventUser.LastModified = System.DateTime.Now;
                        this._eventUserRepository.Update(EventUser);
                        this._eventUserRepository.Commit();
                    }

                }
                catch (Exception ex)
                {
                    EventsApp.Framework.ExceptionHandling.ExceptionHandlingManager.HandleException(ex,
                       EventsApp.Framework.ExceptionHandling.ExceptionHandlingPolicies.ServiceLayer);
                    return InternalServerError(ex);
                }
            }
            else
            {
                ModelState.AddModelError("InvalidDataExceptionBase", "UpdateFailed");//resourceManager.GetResource("EmailTaken")m
                return BadRequest(ModelState);
            }

            return Ok(new SimpleLoginData { UserId = model.Id, PhoneNumber = model.PhoneNumber });
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

        [DataContract]
        private class SimpleLoginData
        {
            [DataMember]
            public int UserId { get; set; }
            [DataMember]
            public string PhoneNumber { get; set; }
        }

        #endregion

        private async Task<bool> SendEmail(string subject, string body, string toName, string toEmail)
        {
            try
            {
                string SMTPServer = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"];
                int SMTPPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTPPort"]);
                string SMTPSSL = System.Configuration.ConfigurationManager.AppSettings["SMTPSSL"];
                string EmailAddress = System.Configuration.ConfigurationManager.AppSettings["EmailAddress"];
                string EmailPassword = System.Configuration.ConfigurationManager.AppSettings["EmailPassword"];

                var responseValue = await mailSender.SendEmailAsync("DDF Events", EmailAddress, toName, toEmail, subject, body, true, null, null, null);
                return responseValue;
            }
            catch (Exception ex)
            {
                EventsApp.Framework.ExceptionHandling.ExceptionHandlingManager.HandleException(ex,
                   EventsApp.Framework.ExceptionHandling.ExceptionHandlingPolicies.ServiceLayer);
                string error = ex.Message;
                return false;
            }
        }

        private async Task<string> createRegisterEmailBody(string Name, string Username, string Password, string CompanyCode, string QRCode, string AndroidLink, string IOSLink, string body1, string body2, string dateAndTimeSlot, string direction, string QRCodeLabel, string TicketType, string Participant, string VisitorFullName, string DateTimeSlot)
        {
            string body = string.Empty;
            var user = await UserManager.FindByEmailAsync(Username);

            if (null != user)
            {
                //Generating a token
                //string code = UserManager.GenerateEmailConfirmationToken(user.Id);
                // string url = string.Format(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/#/page/confirmEmail?userId={0}&code={1}", user.Id, HttpUtility.UrlEncode(code));
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/email-guest-QR.html")))
                {
                    body = reader.ReadToEnd();
                }
                var virtualfilepath = "~/Uploads/Attachment/" + QRCode;
                var absulotePath = VirtualPathUtility.ToAbsolute(virtualfilepath);
                var physicalfilepath = HttpContext.Current.Server.MapPath(virtualfilepath);

                body = body.Replace("[EmailBody1]", "Dear " + Name + "<br />" + "You have been registered in DDF E-Voucher System.");
                body = body.Replace("[FullNAME]", Name);
                body = body.Replace("[FoodVouchers]", user.AssignedFoodVouchers.ToString());
                body = body.Replace("[DrinkVouchers]", user.AssignedDrinkVouchers.ToString());
                body = body.Replace("[QRCode]", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath);

            }
            return body;
        }

        private string GetQRBase64(string data, Brush bColor, Brush fColor, string userCode)
        {
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            QrCode qrCode = new QrCode();
            qrEncoder.TryEncode(data, out qrCode);
            int margin = 5;
            int textHeight = 20;

            var font = new Font(FontFamily.GenericMonospace, 12);
            var brush = new SolidBrush(Color.Black);
            var format = new StringFormat() { Alignment = StringAlignment.Center };

            GraphicsRenderer renderer = new GraphicsRenderer(new FixedCodeSize(125, QuietZoneModules.Zero), bColor, fColor);
            MemoryStream ms = new MemoryStream();



            renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
            var be = new Bitmap(ms);
            Graphics ge = Graphics.FromImage(be);
            var rect = new RectangleF(margin, be.Height - textHeight,
                              be.Width - 2 * margin, textHeight);
            ge.DrawString(userCode.ToString(), font, brush, rect, format);
            byte[] byteImage = ms.ToArray();
            var SigBase64 = Convert.ToBase64String(byteImage); //Get Base64
            //return "data:image/png;base64," + SigBase64;

            var fileName = Guid.NewGuid().ToString() + ".png";
            var virtualfilepath = "~/Uploads/Attachment/" + fileName;
            var absulotePath = VirtualPathUtility.ToAbsolute(virtualfilepath);
            var physicalfilepath = HttpContext.Current.Server.MapPath(virtualfilepath);

            File.WriteAllBytes(physicalfilepath, ms.ToArray());
            return fileName;
        }

        private string GetQrcode(string code, int eSize, Image img, int iSize, Brush bColor, Brush fColor, string userCode)
        {
            var qrcoder = new QrEncoder(ErrorCorrectionLevel.H);
            var qrCode = qrcoder.Encode(code);

            var render = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Four), fColor, bColor);

            using (Stream stream = new MemoryStream())
            {
                eSize = eSize > 0 ? eSize : 200;

                render.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream, new Point(10, 10));

                var be = new Bitmap(stream);
                be = ResizeImage(be, eSize, eSize);
                Graphics ge = Graphics.FromImage(be);

                var imageLogo = HttpContext.Current.Server.MapPath(VirtualPathUtility.ToAbsolute("~/Uploads/Attachment/Ddf.png"));

                img = Image.FromFile(imageLogo);
                if (img != null)
                {
                    iSize = iSize > 0 ? iSize : 50;
                    img = ResizeImage(img, iSize, iSize);

                    var bi = new Bitmap(iSize + 10, iSize + 10);
                    Graphics gi = Graphics.FromImage(bi);
                    gi.Clear(Color.White);
                    gi.DrawImage(img, 5, 5, iSize, iSize);

                    ge.DrawImage(bi, (eSize - iSize) / 2, (eSize - iSize) / 2, iSize, iSize);
                }
                MemoryStream ms = new MemoryStream();

                render.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);

                byte[] byteImage = ms.ToArray();

                //For adding text

                //int margin = 5;
                //int textHeight = 20;

                //var font = new Font(FontFamily.GenericMonospace, 12);
                //var brush = new SolidBrush(Color.Black);
                //var format = new StringFormat() { Alignment = StringAlignment.Center };
                //var txt = new Bitmap(ms);
                //Graphics gtxt = Graphics.FromImage(txt);
                //var rect = new RectangleF(margin, be.Height - textHeight,
                //                  be.Width - 2 * margin, textHeight);
                //gtxt.DrawString(userCode.ToString(), font, brush, rect, format);


                var SigBase64 = Convert.ToBase64String(byteImage); //Get Base64
                var fileName = Guid.NewGuid().ToString() + ".png";
                var virtualfilepath = "~/Uploads/Attachment/" + fileName;
                var absulotePath = VirtualPathUtility.ToAbsolute(virtualfilepath);
                var physicalfilepath = HttpContext.Current.Server.MapPath(virtualfilepath);

                File.WriteAllBytes(physicalfilepath, ms.ToArray());
                return fileName;

            }
        }

        private static Bitmap ResizeImage(Image sourceImage, int width, int height)
        {
            Graphics graphics = null;
            try
            {
                var b = new Bitmap(width, height);
                graphics = Graphics.FromImage(b);


                graphics.CompositingQuality = CompositingQuality.HighQuality;

                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphics.SmoothingMode = SmoothingMode.HighQuality;

                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.Clear(Color.Transparent);

                graphics.DrawImage(sourceImage, new Rectangle(0, 0, width, height),
                                   new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), GraphicsUnit.Pixel);
                return b;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (graphics != null)
                    graphics.Dispose();
            }
        }
    }
}
