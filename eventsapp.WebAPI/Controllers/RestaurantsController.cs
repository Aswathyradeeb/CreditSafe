using eventsapp.WebAPI.Models;
using Eventsapp.Services;
using Eventsapp.Services.Services.Interface;
using EventsApp.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace eventsapp.WebAPI
{
    [Authorize]
    [RoutePrefix("api/Restaurant")]
    public class RestaurantsController : ApiController
    {
        private IRestaurantService _companyServices;
        private ICurrentUser user;
        private readonly IUserService _userService;
        public RestaurantsController(IRestaurantService _companyServices,
            ICurrentUser user, IUserService _userService)
        {
            this._companyServices = _companyServices;
            this.user = user;
            this._userService = _userService;
        }

        [HttpPost]
        [Route("GetAllRestaurants")]
        public async Task<IHttpActionResult> GetAllRestaurants(RequestFilter request)
        {
            try
            {
                var returnValue = await this._companyServices.GetAllRestaurants(request.FilterParams, request.Page, request.PageSize);
                var pagedRecord = new PagedList();
                pagedRecord.TotalRecords = returnValue.companiesCount;
                pagedRecord.Content = returnValue.companies;
                pagedRecord.CurrentPage = request.Page;
                pagedRecord.PageSize = request.PageSize;
                return Ok(pagedRecord);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [Route("GetAllRestaurants")]
        public async Task<IHttpActionResult> GetAllRestaurants()
        {
            try
            {
                var _companies = await this._companyServices.GetAllRestaurants();
                return Ok(_companies);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("CreateRestaurant")]
        public async Task<IHttpActionResult> CreateRestaurant(CompanyDto compnay)
        {
            try
            {
                compnay.CreatedBy = this.user.UserInfo.Id.ToString();
                var _company = await this._companyServices.CreateRestaurant(compnay, ConfigurationManager.AppSettings["connectionString"].ToString());
               
                // var user = await _userService.GetByUserId(this.user.UserInfo.Id);
                // var speaker = await _userService.GetByUserId(eventPerson.PersonId);

                try
                {
                //    string body = await createRegisterEmailBody(user.FirstName + " " + user.LastName, "Company", compnay.NameEn, System.DateTime.Now.ToString(), "speaker-register.html");
                //    var result = await SendEmail("Company Registered", body, user.Email);

                //    body = await createRegisterEmailBody(compnay.NameEn, "", compnay.NameEn, System.DateTime.Now.ToString(), "speaker-notification.html", user.FirstName, "Sponsor/Exhibitor");
                //    result = await SendEmail("Company Registered", body, compnay.Email);
                }
                catch (Exception ex)
                {
                    EventsApp.Framework.ExceptionHandling.ExceptionHandlingManager.HandleException(ex,
                       EventsApp.Framework.ExceptionHandling.ExceptionHandlingPolicies.ServiceLayer);
                }


                return Ok(_company);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("UpdateRestaurant")]
        public async Task<IHttpActionResult> UpdateRestaurant(CompanyDto compnay)
        {
            try
            {
                var _company = await this._companyServices.UpdateRestaurant(compnay);
                return Ok(_company);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("DeleteRestaurant")]
        public async Task<IHttpActionResult> DeleteRestaurant(int id)
        {
            try
            {
                var data = await this._companyServices.DeleteRestaurant(id);
                return Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        #region Mail
        public async Task<bool> SendEmail(string subject, string body, string recipient)
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                try
                {
                    bool result = true;
                    mailMessage.From = new MailAddress("noreply@hifive.ae");
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.To.Add(recipient);

                    SmtpClient smtp = new SmtpClient("mail.hifive.ae", 25);
                    smtp.EnableSsl = Convert.ToBoolean("false");
                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential("noreply@hifive.ae", "Hifive@123");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Send(mailMessage);
                    return result;


                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    return false;
                }
            }
        }

        private async Task<string> createRegisterEmailBody(string Name, string eventName, string CompName, string startdate, string path, string adminName = "", string RegType = "")
        {
            string body = string.Empty;
            //Generating a token
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/" + path)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("[FullNAME]", Name);
            body = body.Replace("[Name]", CompName);
            body = body.Replace("[startdate]", startdate);
            if (!string.IsNullOrEmpty(adminName))
            {
                body = body.Replace("[adminName]", adminName);
            }
            if (!string.IsNullOrEmpty(eventName))
            {
                body = body.Replace("[Eventname]", eventName);
            }
            if (!string.IsNullOrEmpty(RegType))
            {
                body = body.Replace("[RegType]", RegType);
            }
            //return Ok();
            return body;
            // return Task.Delay(10000)
            //.ContinueWith(t => body); 
        }
        #endregion
    }
}
