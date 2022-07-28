using eventsapp.WebAPI.Models;
using eventsapp.WebAPI.Models;
using Eventsapp.Repositories;
using Eventsapp.Services;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.ReturnObjects;
using EventsApp.Domain.Entities;
using EventsApp.Domain.Enums;
using EventsApp.Framework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;

namespace eventsapp.WebAPI.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Event")]
    public class EventsController : ApiController
    {
        private IEventService eventService;
        private ICurrentUser user;
        private readonly IEventRepository eventRepository;
        private readonly IUserRepository userRepository;
        private readonly IEventUserRepository eventUserRepository;
        private readonly IUserService _userService;
       
        public EventsController(IEventService eventService, IEventRepository _eventRepository, IUserRepository userRepository, ICurrentUser user, IUserService _userService)
        {
            this.eventService = eventService;
            this.eventRepository = _eventRepository;
            
            this.userRepository = userRepository;
            this.user = user;
            this._userService = _userService;
        }

        [Route("GetEvents")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetEvents()
        {
            try
            {
                var eventDtos = await this.eventService.GetEvents();
                return Ok(eventDtos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [AllowAnonymous]
        [Route("GetEventsGroupingByWeek")]
        public async Task<IHttpActionResult> GetEventsGroupingByWeek(string companyCode)
        {
            try
            {
                var eventDtos = await this.eventService.GetEventsGroupingByWeek(companyCode);
                return Ok(eventDtos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("GetEventsByCompany")]
        [HttpPost]
        public async Task<IHttpActionResult> GetEventsByCompany(string CompName, RequestFilter request)
        {
            try
            {
                var returnValue = await this.eventService.GetEventsByCompanyName(CompName, request.FilterParams, request.Page, request.PageSize);
                var pagedRecord = new PagedList
                {
                    TotalRecords = returnValue.eventsCount,
                    Content = returnValue.events,
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
        [Route("GetEventsByCompanyId")]
        [HttpPost]
        public async Task<IHttpActionResult> GetEventsByCompanyId(RequestFilter request)
        {
            try
            {
                var returnValue = await this.eventService.GetEventsByCompanyId(this.user.UserInfo.Id, request.FilterParams, request.Page, request.PageSize);
                var pagedRecord = new PagedList
                {
                    TotalRecords = returnValue.eventsCount,
                    Content = returnValue.events,
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

        [Route("GetEventsLookUp")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetEventsLookUp()
        {
            try
            {
                bool isAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.Administrator) > 0;
                bool isSuperAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0;
                bool isUser = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.User) > 0;

                var eventDtos = MapperHelper.Map<List<EventLightDto>>(this.eventRepository.Query(x => isSuperAdmin == true ||
               (isAdmin == true && x.CreatedBy == this.user.UserInfo.Id) ||
               (isUser == true && x.EventUsers.Count(a => a.UserId == this.user.UserInfo.Id) > 0)));
                return Ok(eventDtos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [Route("GetAllEvents")]
        [HttpPost]
        public async Task<IHttpActionResult> GetEvents(RequestFilter request)
        {
            try
            {
                var returnValue = await this.eventService.GetAllEvents(request.FilterParams, request.Page, request.PageSize);
                var pagedRecord = new PagedList
                {
                    TotalRecords = returnValue.eventsCount,
                    Content = returnValue.events,
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

        [Route("GetUsersByEventId")]
        public async Task<IHttpActionResult> GetRegisterdUsersByEvent(int eventId)
        {
            try
            {
                var user = await this.userRepository.QueryAsync(x => x.EventUsers.Where(y => y.EventId == eventId).Any());
                var userDtos = MapperHelper.Map<List<UserDto>>(user);
                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [Route("GetEventsByUserId")]
        public async Task<IHttpActionResult> GetEventsByUserId()
        {
            try
            {
                var eventDtos = await this.eventService.GetEventsByUserId(this.user.UserInfo.Id);
                return Ok(eventDtos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [AllowAnonymous]
        [Route("GetEventById")]
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> GetEvent(int id)
        {
            try
            {
                var eventObj = await eventService.GetEventId(id);
                return Ok(eventObj);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [AllowAnonymous]
        [Route("GetEventRegitration")]
        public async Task<IHttpActionResult> GetEventRegitration(int id)
        {
            try
            {
                var eventObj = await eventService.GetEventRegitration(id);
                return Ok(eventObj);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("RegisterByEventIdUserId")]
        public async Task<IHttpActionResult> RegisterByEventIdUserId(EventRegistrationDto RegistrationModel)
        {
            try
            {
                RegistrationModel.UserId = this.user.UserInfo.Id;

                var registeredEventUserDto = await this.eventService.IsRegisterEvent(RegistrationModel);
                if (registeredEventUserDto != null)
                {
                    IDictionary<string, string> response = new Dictionary<string, string>()
                    {
                        { "message",  "You are already registered as " + registeredEventUserDto.RegistrationType.NameEn }
                    };
                    return Ok(response);
                }
                var eventUserDto = this.eventService.RegisterEventUser(RegistrationModel);
                return Ok(eventUserDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //[HttpPost]
        //[Route("RegisterByEventIdUserId")]
        //public async Task<IHttpActionResult> RegisterByEventIdUserId()
        //{
        //    string speakerResumeNewName = String.Empty;
        //    string speakerResumeFullPathWithNewName = String.Empty;
        //    var uploadFolderName = "Event-Users-Attachment";

        //    var virtualfilepath = "~/Uploads/" + uploadFolderName + "/";
        //    var absulotePath = VirtualPathUtility.ToAbsolute(virtualfilepath);
        //    var physicalFilePathToUploadFile = HttpContext.Current.Server.MapPath(virtualfilepath);

        //    try
        //    {
        //        // Check if the request contains multipart/form-data.  
        //        if (!Request.Content.IsMimeMultipartContent())
        //        {
        //            throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //        }
        //        EventUserDto eventUserDto = new EventUserDto();
        //        var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());

        //        //access form data  
        //        NameValueCollection formData = provider.FormData;

        //        eventUserDto.EventId = Int32.Parse(formData["EventId"]);
        //        eventUserDto.RegistrationTypeId = Int32.Parse(formData["RegistrationTypeId"]);
        //        eventUserDto.UserId = User.Identity.GetUserId();


        //        //check user is registered to thie event
        //        var registeredEventUserDto = await this.eventService.IsRegisterEvent(eventUserDto);
        //        if (registeredEventUserDto != null)
        //        {
        //            IDictionary<string, string> response = new Dictionary<string, string>()
        //            {
        //                { "message",  "You are already registered as " + registeredEventUserDto.RegistrationType.NameEn }
        //            };
        //            return Ok(response);
        //        }

        //        //access files  
        //        IList<HttpContent> speakerResumes = provider.Files;

        //        if (speakerResumes.Count > 0)
        //        {
        //            HttpContent speakerResume = speakerResumes[0];
        //            var vfileName = Guid.NewGuid().ToString();
        //            var speakerResumeOriginalName = speakerResume.Headers.ContentDisposition.FileName.Trim('\"');
        //            speakerResumeNewName = vfileName + speakerResumeOriginalName;
        //            eventUserDto.DocumentName = speakerResumeNewName;

        //            speakerResumeFullPathWithNewName = physicalFilePathToUploadFile + speakerResumeNewName;

        //            Stream input = await speakerResume.ReadAsStreamAsync();

        //            //Deletion exists file  
        //            if (File.Exists(speakerResumeFullPathWithNewName))
        //            {
        //                File.Delete(speakerResumeFullPathWithNewName);
        //            }

        //            //Directory.CreateDirectory(@directoryName);  
        //            using (Stream file = File.OpenWrite(speakerResumeFullPathWithNewName))
        //            {
        //                input.CopyTo(file);
        //                //close file  
        //                file.Close();
        //            }
        //        }

        //        // registering user to event
        //        var eventDto = this.eventService.RegisterEvent(eventUserDto);
        //        return Ok(eventDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Deletion if something went wrong
        //        if (File.Exists(speakerResumeFullPathWithNewName))
        //        {
        //            File.Delete(speakerResumeFullPathWithNewName);
        //        }
        //        return InternalServerError(ex);
        //    }
        //}

        [Route("AttendEvent")]
        public IHttpActionResult AttendEvent(EventUserDto _eventUserDto)
        {
            try
            {

                EventUser eventUserEntity = this.eventUserRepository.Get(_eventUserDto.EventId);
                EventUser _eventUser = MapperHelper.Map<EventUser>(_eventUserDto);

                eventUserEntity.IsAttended = _eventUser.IsAttended;
                return Ok(MapperHelper.Map<EventUserDto>(eventUserEntity));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("DeleteEvent")]
        [HttpPost]
        public async Task<IHttpActionResult> DeleteEvent(int eventId)
        {
            try
            {
                var data = eventService.DeleteEvent(eventId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("UpdateEvent")]
        //[ResponseType(typeof(void))]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateEvent(EventDto eventDto)
        {
            try
            {
               
                var data = await eventService.UpdateEvent(eventDto);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("CreateEvent")]
        [ResponseType(typeof(Event))]
        [HttpPost]
        public async Task<IHttpActionResult> CreateEvent(EventSingleDto @event)
        {
            try
            {
                @event.CreatedBy = this.user.UserInfo.Id;
                @event.CreatedOn = DateTime.Now;
                var eventDto = eventService.CreateEvent(@event);
                List<Event> events = this.eventRepository.GetAll().ToList();
                Event evento = events.LastOrDefault();
                EventDto _eventDto = MapperHelper.Map<EventDto>(evento);

                var user = await _userService.GetByUserId(this.user.UserInfo.Id);
                string date = @event.StartDate.ToString();
                try
                {
                    string body = await createRegisterEmailBody(user.FirstName + " " + user.LastName, @event.NameEn, date, "event-register.html");
                    var result = await SendEmail("Event Registered", body, user.Email);
                }
                catch (Exception ex)
                {
                    EventsApp.Framework.ExceptionHandling.ExceptionHandlingManager.HandleException(ex,
                       EventsApp.Framework.ExceptionHandling.ExceptionHandlingPolicies.ServiceLayer);
                }
                return Ok(_eventDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [Route("Questions")]
        [HttpPost]
        public async Task<IHttpActionResult> Questions(AttendeeQuestionDto _AttendeeQuestionDto)
        {
            try
            {
                // _AttendeeQuestionDto.UserId = 1;
                // _AttendeeQuestionDto.UserId = this.user.UserInfo.Id;
                var qstnDto = eventService.Addquestion(_AttendeeQuestionDto);
                string adminemail =await eventService.getAdminMail();
                var user = await _userService.GetByUserId(this.user.UserInfo.Id);
                var eventObj = await eventService.GetEventId(Convert.ToInt32(_AttendeeQuestionDto.EventId));
                try
                {
                    string body = await createRegisterEmailBody(user.FirstName + " " + user.LastName, eventObj.NameEn,_AttendeeQuestionDto.QuestionEn,"Question-notification.html");
                    var result = await SendEmail("Query about Event", body, adminemail);
                }
                catch (Exception ex)
                {
                    EventsApp.Framework.ExceptionHandling.ExceptionHandlingManager.HandleException(ex,
                       EventsApp.Framework.ExceptionHandling.ExceptionHandlingPolicies.ServiceLayer);
                }
                return Ok(qstnDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("GetQuestions")]
        [HttpGet]
        public async Task<IHttpActionResult> GetQuestions(int eventid = 0, int speakerid = 0)
        {
            try
            {
                var questionsDtos = await this.eventService.GetQuestions(eventid, speakerid);
                return Ok(questionsDtos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("UpdateAnswer")]
        [ResponseType(typeof(void))]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateAnswer(AttendeeQuestionDto qstnDto)
        {
            try
            {
                var data = await eventService.UpdateAnswer(qstnDto);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("GetPaidPackages")]    
        [HttpGet]
        public async Task<IHttpActionResult> GetPaidPackages()
        {
            try
            {
                var data = await eventService.GetPaidPackages(this.user.UserInfo.Id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [Route("checkSubsription")]
        [HttpGet]
        public async Task<IHttpActionResult> checkSubsription(int id,int eventId)
        {
            try
            {
                var data =  eventService.checkSubsription(this.user.UserInfo.Id,id,eventId);
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

        private async Task<string> createRegisterEmailBody(string Name, string EventName, string startdate,string path)
        {
            string body = string.Empty;
             
                 using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/"+path)))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("[FullNAME]", Name);
                body = body.Replace("[Eventname]", EventName);
                body = body.Replace("[startdate]", startdate);              
         
            return body;
             
        }
        #endregion

    }


}