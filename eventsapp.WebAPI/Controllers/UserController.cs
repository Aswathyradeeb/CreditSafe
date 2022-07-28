using eventsapp.WebAPI.Models;
using Eventsapp.Services;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.Core;
using EventsApp.Domain.DTOs.ReturnObjects;
using EventsApp.Domain.DTOs.Subscription;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace eventsapp.WebAPI.Controllers
{
    //[Authorize] 
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IKeyedRepository<PreferredLanguage, int> preferredLanguageRepository;

        public UserController(IUserService userService, IKeyedRepository<PreferredLanguage, int> _preferredLanguageRepository)
        {
            _userService = userService;
            preferredLanguageRepository = _preferredLanguageRepository;
        }

        [Route("Get")]
        public async Task<IHttpActionResult> Get(int userid = 0)
        {
            try
            {
                if (userid == 0)
                {
                    var userId = User.Identity.GetUserId<int>();
                    var user = await _userService.GetByUserId(userId);
                    return Ok(user);
                }
                else
                {
                    var user = await _userService.GetByUserId(userid);
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("VerifyVoucher")]
        public async Task<IHttpActionResult> VerifyUser(int userId = 0,int eventId = 0)
        {
            try
            {
               
                    var user = await _userService.VerifyVoucher(userId, eventId);
                    return Ok(user);
                
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Authorize(Roles = "Head Of Department,Head Of Section,Confirmity Engineer,System Administrator")]
        [Route("GetById")]
        public async Task<IHttpActionResult> GetById(int userId)
        {
            try
            {
                var users = await _userService.GetByUserId(userId);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("UserLock")]
        public async Task<IHttpActionResult> UserLock(int userId, bool isLock)
        {
            try
            {
                var users = await _userService.UserLock(userId, isLock);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("UpdateUserProfile")]
        public async Task<IHttpActionResult> UpdateUserProfile(UserDto model)
        {
            try
            {
                await _userService.UpdateUserProfile(model);
                var result = await _userService.GetByUserId(model.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        [Route("SendEmailBatch")]
        [Authorize(Roles = "Head Of Department,Head Of Section,Confirmity Engineer,System Administrator")]
        public async Task<IHttpActionResult> SendEmailBatch(JObject jsonData)
        {
            try
            {
                dynamic json = jsonData;
                EmailViewModel model = (json.email as JObject)?.ToObject<EmailViewModel>();
                FilterParams filterParams = (json.filterParams as JObject)?.ToObject<FilterParams>();
                string searchtext = json.searchtext as string;

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Ok(await _userService.SendEmailBatch(filterParams, searchtext, model.Subject, model.Body));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("SendSMSBatch")]
        [Authorize(Roles = "Head Of Department,Head Of Section,Confirmity Engineer,System Administrator")]
        public async Task<IHttpActionResult> SendSMSBatch(JObject jsonData)
        {
            try
            {
                dynamic json = jsonData;
                SMSViewModel model = (json.sms as JObject).ToObject<SMSViewModel>();
                FilterParams filterParams = (json.filterParams as JObject).ToObject<FilterParams>();
                bool isEmployee = json.isEmployee;
                string searchtext = json.searchtext as string;

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Ok(await _userService.SendSMSBatch(filterParams, searchtext, model.SMS));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("SendEmail")]
        [Authorize(Roles = "Head Of Department,Head Of Section,Confirmity Engineer,System Administrator")]
        public async Task<IHttpActionResult> SendEmail(SendEmailViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    await Request.GetOwinContext().GetUserManager<ApplicationUserManager>().SendEmailAsync(model.UserId, model.Subject, model.Body);
                    UserActionsTakenDto action = new UserActionsTakenDto();
                    action.Note = model.Body;
                    action.UserId = model.UserId;
                    await _userService.SendEmailUserAction(action);

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("SendSMS")]
        [Authorize(Roles = "Head Of Department,Head Of Section,Confirmity Engineer,System Administrator")]
        public async Task<IHttpActionResult> SendSMS(SendSMSViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var message = new IdentityMessage
                {
                    Destination = model.PhoneNumber,
                    Body = model.SMS
                };
                await Request.GetOwinContext().GetUserManager<ApplicationUserManager>().SmsService.SendAsync(message);
                UserActionsTakenDto action = new UserActionsTakenDto();
                action.Note = model.SMS;
                action.UserId = model.UserId;
                await _userService.SendSMSUserAction(action);

                return Ok();
            }
        }

        // [Authorize(Roles = "Head Of Department,Head Of Section,Confirmity Engineer,System Administrator, Third Party Engineer")]
        [HttpPost]
        [Route("GetUsers")]
        public async Task<IHttpActionResult> GetUsers(RequestFilter request)
        {
            try
            {
                IEnumerable<UserDto> userProfiles =
                    await _userService.GetPagedUsers(request.FilterParams, request.Page, request.PageSize,
                    (request.SortDirection == "asc"), request.Searchtext, request.SortBy);

                var pagedRecord = new PagedList
                {
                    TotalRecords = await _userService.GetNumbersOfRecords(request.FilterParams, request.Searchtext),
                    Content = userProfiles.ToList(),
                    CurrentPage = request.Page,
                    PageSize = request.PageSize
                };

                return Ok(pagedRecord);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("GetUserSubscriptions")]
        public async Task<IHttpActionResult> GetUserSubscriptions(RequestFilter request)
        {
            try
            {
                IEnumerable<UserSubscriptionDto> UserSubscriptions =
                    await _userService.GetUserSubscriptions(request.FilterParams, request.Page, request.PageSize,
                    (request.SortDirection == "asc"), request.Searchtext, request.SortBy);

                var pagedRecord = new PagedList
                {
                    TotalRecords = await _userService.GetSubscriptionNumbersOfRecords(request.FilterParams, request.Searchtext),
                    Content = UserSubscriptions.ToList(),
                    CurrentPage = request.Page,
                    PageSize = request.PageSize
                };

                return Ok(pagedRecord);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("CreateSubscription")]
        public async Task<IHttpActionResult> CreateSubscription(int eventPackageId)
        {
            try
            {
                var responseMsg = await _userService.CreateSubscription(eventPackageId);
                var response = new ReturnResponseDto { message = responseMsg.ToString(), success = true };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("AddCompany")]
        public async Task<IHttpActionResult> AddCompany(string companyCode)
        {
            try
            {
                var responseMsg = await _userService.AddCompany(companyCode);
                var response = new ReturnResponseDto { message = responseMsg.ToString(), success = true };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("SetUserToken")]
        public async Task<ResponseType<string>> SetUserToken(UserTokenModel tokenModel)
        {
            try
            {
                if (tokenModel == null)
                    return ResponseType<string>.PerformError<string>("Invalid data passed", "Cannot set user token");
                if (!tokenModel.UserId.HasValue)
                    return ResponseType<string>.PerformError<string>("Invalid data passed", "User Id should must be provided for setting token value");
                var result = await this._userService.SetUserToken(tokenModel.UserId.Value, tokenModel.Token);
                return ResponseType<string>.PerformSuccessed<string>(result);
            }
            catch (Exception ex)
            {
                return ResponseType<string>.PerformError<string>("Exception", "Exception has occured: " + ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserbyToken()
        {
            try
            {
                ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    var userId = claimsIdentity.Claims.Select(x => new { type = x.Type, value = x.Value }).Where(x => x.type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Select(s => s.value).FirstOrDefault();
                    if (userId != null)
                    {
                        var userData = await this._userService.GetByUserId(Convert.ToInt32(userId));
                        return Ok(userData);
                    }
                    else
                    {
                        return BadRequest("The user token has expired");
                    }
                }
                else
                {
                    return BadRequest("The user token has expired");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("UserPreferredLanguages")]
        public async Task<IHttpActionResult> UserPreferredLanguages(int userId)
        {
            try
            {
                var preferredLanguages = await this.preferredLanguageRepository.QueryAsync(x => x.UserId == userId);
                var preferredLanguagesDto = MapperHelper.Map<List<PreferredLanguageDto>>(preferredLanguages);
                return Ok(preferredLanguagesDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}