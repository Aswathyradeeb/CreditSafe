using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web;
using EventsApp.Domain.Entities;
using EventsApp.Domain.DTOs;
using Eventsapp.Services;
using Eventsapp.Repositories;
using System.Threading.Tasks;
using eventsapp.WebAPI.Models;
using System.IO;
using System.Net.Mail;
using System.Configuration;

namespace eventsapp.WebAPI.Controllers
{
    [RoutePrefix("api/Speaker")]
    public class SpeakersController : ApiController
    {
        private ISpeakerService speakerService;
        private readonly IUserService _userService;
        private ICurrentUser user;
        public SpeakersController(ISpeakerService speakerService, IUserService _userService, ICurrentUser user)
        {
            this.speakerService = speakerService;
            this._userService = _userService;
            this.user = user;
        }

        // GET: api/Speakers
        [Route("GetSpeakers")]
        public async Task<IHttpActionResult> GetPeople()
        {
            try
            {
                var speakerDtos = MapperHelper.Map<List<PersonDto>>(await this.speakerService.GetSpeakers());
                return Ok(speakerDtos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("GetSpeakersByEventId")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSpeakersByEventId(int EventId)
        {
            try
            {
                var speakerDtos = await this.speakerService.GetSpeakersByEventId(EventId);
                return Ok(speakerDtos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [Route("GetAllSpeakers")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAllSpeakers(RequestFilter request)
        {
            try
            {
                var returnValue = await this.speakerService.GetAllSpeakers(request.FilterParams, request.Page, request.PageSize);
                var pagedRecord = new PagedList();
                pagedRecord.TotalRecords = returnValue.speakersCount;
                pagedRecord.Content = returnValue.speakers;
                pagedRecord.CurrentPage = request.Page;
                pagedRecord.PageSize = request.PageSize;
                return Ok(pagedRecord);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        //GET: api/Speakers/5
        [Route("GetSpeakerById")]
        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> GetPerson(int id)
        {
            try
            {
                var speakerObj = await speakerService.GetSpeakerId(id);
                return Ok(speakerObj);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [Route("UpdateSpeaker")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateSpeaker(PersonDto person)
        {
            try
            {
                var speakerDto = await speakerService.UpdateSpeaker(person);
                return Ok(speakerDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //POST: api/Speakers
        [Route("CreateSpeaker")]
        [ResponseType(typeof(Person))]
        [HttpPost]
        public async Task<IHttpActionResult> PostPerson(PersonDto person)
        {
            try
            {
                person.CreatedBy = this.user.UserInfo.Id;
                var speakerDto = await speakerService.CreateSpeaker(person, ConfigurationManager.AppSettings["connectionString"].ToString());

                var user = await _userService.GetByUserId(this.user.UserInfo.Id);
                try
                {
                    string body = await createRegisterEmailBody(user.FirstName + " " + user.LastName, "Speaker", person.NameEn, System.DateTime.Now.ToString(), "speaker-register.html");
                    var result = await SendEmail("Speaker Registered", body, user.Email);

                    body = await createRegisterEmailBody(person.NameEn, person.NameEn, System.DateTime.Now.ToString(), "speaker-notification.html", user.FirstName, "Speaker");
                    result = await SendEmail("Registered as Speaker", body, person.Email);
                }
                catch (Exception ex)
                {
                    EventsApp.Framework.ExceptionHandling.ExceptionHandlingManager.HandleException(ex,
                       EventsApp.Framework.ExceptionHandling.ExceptionHandlingPolicies.ServiceLayer);
                }




                return Ok(speakerDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("DeleteSpeaker")]
        public async Task<IHttpActionResult> DeleteSpeaker(int id)
        {
            try
            {
                var data = await this.speakerService.DeleteSpeaker(id);
                return Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("GetSpeakerByUser")]
        public async Task<IHttpActionResult> GetSpeakerByUser(int uid)
        {
            try
            {
                var _speaker = this.speakerService.GetSpeakerUser(uid);
                return Ok(_speaker);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("SubmitSpeakerRating")]
        public async Task<IHttpActionResult> SubmitSpeakerRating(SpeakerRatingDto SpeakerRating)
        {
            try
            {
                var data =  await this.speakerService.SubmitSpeakerRating(SpeakerRating);
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

        private async Task<string> createRegisterEmailBody(string Name, string Eventname, string SpeakerName, string startdate, string path, string adminName = "", string RegType = "")
        {
            string body = string.Empty;
            //Generating a token
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/" + path)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("[FullNAME]", Name);
            body = body.Replace("[Name]", SpeakerName);
            body = body.Replace("[startdate]", startdate);
            if (!string.IsNullOrEmpty(adminName))
            {
                body = body.Replace("[adminName]", adminName);
            }
            if (!string.IsNullOrEmpty(Eventname))
            {
                body = body.Replace("[Eventname]", Eventname);
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