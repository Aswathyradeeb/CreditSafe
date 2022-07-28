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
    [RoutePrefix("api/EventPerson")]
    public class EventPersonController : ApiController
    {
        private readonly IEventPersonService _eventPersonServices;
        private readonly IUserService _userService;
        private ICurrentUser user;
        public EventPersonController(IEventPersonService eventPersonServices, IUserService _userService, ICurrentUser user)
        {
            this._eventPersonServices = eventPersonServices;
            this._userService = _userService;
            this.user = user;
        }

        [HttpPost]
        [Route("CreateEventPerson")]
        public async Task<IHttpActionResult> CreateEventPerson(EventPersonDto eventPerson)
        {
            try
            {
                var data = this._eventPersonServices.CreateEventPerson(eventPerson, ConfigurationManager.AppSettings["connectionString"].ToString());


               // var user = await _userService.GetByUserId(this.user.UserInfo.Id);
               //// var speaker = await _userService.GetByUserId(eventPerson.PersonId);
               
               // try
               // {
               //     string body = await createRegisterEmailBody(user.FirstName + " " + user.LastName,eventPerson.Person.NameEn,System.DateTime.Now.ToString(), "speaker-register.html");
               //     var result = await SendEmail("Speaker Registered", body, user.Email);

               //     body= await createRegisterEmailBody(eventPerson.Person.NameEn, eventPerson.Person.NameEn, System.DateTime.Now.ToString(),"speaker-notification.html");
               //      result = await SendEmail("Registered as Speaker", body, eventPerson.Person.Email);
               // }
               // catch (Exception ex)
               // {
               //     EventsApp.Framework.ExceptionHandling.ExceptionHandlingManager.HandleException(ex,
               //        EventsApp.Framework.ExceptionHandling.ExceptionHandlingPolicies.ServiceLayer);
               // }

                return Ok(data);



            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("UpdateEventPerson")]
        public async Task<IHttpActionResult> UpdateEventPerson(EventPersonDto eventPerson)
        {
            try
            {
                var data = await this._eventPersonServices.UpdateEventPerson(eventPerson);
                return Ok(data);
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
                    //mailMessage.From = new MailAddress("noreply@esma.gov.ae", "Emirates Authority for Standardization and Metrology");
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

        private async Task<string> createRegisterEmailBody(string Name, string EventName, string startdate,string path,string adminName="")
        {
            string body = string.Empty;
            //Generating a token
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/"+path)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("[FullNAME]", Name);
            body = body.Replace("[SpeakerName]", EventName);
            body = body.Replace("[startdate]", startdate);
            if (!string.IsNullOrEmpty(adminName))
            {
                body = body.Replace("[adminName]", adminName);
            }
            //return Ok();
            return body;
            // return Task.Delay(10000)
            //.ContinueWith(t => body); 
        }
        #endregion
    }
}