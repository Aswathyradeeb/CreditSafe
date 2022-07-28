using Eventsapp.Services;
using EventsApp.Domain.DTOs;
using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace eventsapp.WebAPI.Controllers
{
    [RoutePrefix("api/EventCompany")]
    public class EventCompanyController : ApiController
    {
        private readonly IEventCompanyService _eventCompanyServices;
        private readonly IUserService _userService;
        private ICurrentUser user;
        private IEventService eventService;
        public EventCompanyController(IEventCompanyService eventCompanyServices, IUserService _userService, ICurrentUser user, IEventService eventService)
        {
            this._eventCompanyServices = eventCompanyServices;
            this._userService = _userService;
            this.user = user;
            this.eventService = eventService;
        }

        [HttpPost]
        [Route("CreateEventCompany")]
        public async Task<IHttpActionResult> CreateEventCompany(EventCompanyDto eventCompany)
        {
            try
            {
                var _eventcompany = this._eventCompanyServices.CreateEventCompany(eventCompany, ConfigurationManager.AppSettings["connectionString"].ToString());
                var user = await _userService.GetByUserId(this.user.UserInfo.Id);
                var eventObj = await eventService.GetEventId(eventCompany.EventId);
                
                try
                {
                    string body = await createRegisterEmailBody(user.FirstName + " " + user.LastName, eventCompany.Company.NameEn, System.DateTime.Now.ToString(), eventObj.NameEn,eventObj.DescriptionEn, eventObj.EventAddresses.ToString(), "eventCompany-notification.html");
                    var result = await SendEmail("Company Registered as Sponsor/Exhibitor", body, user.Email);

                    body = await createRegisterEmailBody(eventCompany.Company.NameEn, eventCompany.Company.NameEn, System.DateTime.Now.ToString(), eventObj.NameEn, eventObj.DescriptionEn, eventObj.EventAddresses.ToString(),"eventCompany-notification.html");
                    result = await SendEmail("Company Registered as Sponsor/Exhibitor", body, eventCompany.Company.Email);
                }
                catch (Exception ex)
                {
                    EventsApp.Framework.ExceptionHandling.ExceptionHandlingManager.HandleException(ex,
                       EventsApp.Framework.ExceptionHandling.ExceptionHandlingPolicies.ServiceLayer);
                }
                return Ok(_eventcompany);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("UpdateEventCompany")]
        public async Task<IHttpActionResult> UpdateEventCompany(EventCompanyDto eventCompany)
        {
            try
            {
                var _eventcompany = await this._eventCompanyServices.UpdateEventCompany(eventCompany);
                return Ok(_eventcompany);
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

        private async Task<string> createRegisterEmailBody(string Name, string CompName, string startdate, string eventname,string eventDesc,string eventloc, string path)
        {
            string body = string.Empty;
            //Generating a token
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/" + path)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("[FullNAME]", Name);
            body = body.Replace("[CompName]", CompName);
            body = body.Replace("[RegDate]", startdate); 
            body = body.Replace("[EventName]", eventname);
            body = body.Replace("[EventDetails]", eventDesc);
                 
            return body; 
        }
        #endregion
    }
}