using eventsapp.WebAPI.Models;
using eventsapp.WebAPI.Results;
using Eventsapp.Services;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.Core;
using EventsApp.Domain.DTOs.ReturnObjects;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace eventsapp.WebAPI.Controllers
{
    //[Authorize]
    [RoutePrefix("api/EventUser")]
    public class EventUserController : ApiController
    {
        private readonly IEventUserService eventUserService;

        private ICurrentUser user;

        public EventUserController(IEventUserService eventUserService, ICurrentUser user)
        {
            this.eventUserService = eventUserService;
            this.user = user;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetAttendeeTemplate")]
        public async Task<IHttpActionResult> GetAttendeeTemplate()
        {
            byte[] dataBytes = await eventUserService.GetExcelTemplate();
            var dataStream = new MemoryStream(dataBytes);
            return new CustomHttpResult(dataStream, Request, "EmailParticipantSample.xlsx");
        }

        [Authorize]
        [Route("update")]
        public IHttpActionResult update(EventDto eventDto)
        {
            try
            {
                string userId = User.Identity.GetUserId();

                var user = this.eventUserService.Update(eventDto, this.user.UserInfo.Id);
                return Ok();
            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }

        [Authorize]
        [Route("delete")]
        [HttpPost]
        public IHttpActionResult delete(EventDto eventDto)
        {
            try
            {
                string userId = User.Identity.GetUserId();

                var user = this.eventUserService.delete(eventDto, this.user.UserInfo.Id);
                return Ok();
            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }

        [Route("GetEventUsers")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEventUsersByEventId(int eventId = 0)
        {
            try
            {
                var data = await this.eventUserService.GetEventUsersByEventId(eventId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("GetEventUsersbyAttendeeID")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEventUsersbyAttendeeID(int attId = 0)
        {
            try
            {
                var data = await this.eventUserService.GetEventUsersbyAttendeeID(attId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("RegisteredUserAttended")]
        [HttpPost]
        public async Task<IHttpActionResult> RegisteredUserAttended(EventUserDto RegisteredUser)
        {

            try
            {
                var data = await this.eventUserService.RegisteredUserAttended(RegisteredUser);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("QRVisitorAttendance")]
        [HttpGet]
        public async Task<IHttpActionResult> QRVisitorAttendance(int UserId, int EventId, int AgendaId, string lang)
        {

            try
            {
                var data = await this.eventUserService.QRVisitorAttendance(UserId, EventId, AgendaId, lang);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("EventUserAdd")]
        [HttpPost]
        public async Task<IHttpActionResult> EventUserAdd(EventRegisterModel model)
        {
            EventUserDto RegisteredUser = new EventUserDto();
            RegisteredUser.UserId = model.userid;
            RegisteredUser.EventId = model.eventid;
            RegisteredUser.RegistrationTypeId = model.registrationTypeId;
            RegisteredUser.CreatedOn = System.DateTime.Now;

            try
            {
                var data = await this.eventUserService.AddEventUser(RegisteredUser, model.packageId, model.StandLocation, model.StandNumber);
                var response = new ReturnResponseDto { message = data.ToString(), success = true };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("AddEventUser")]
        [HttpPost]
        public async Task<IHttpActionResult> AddEventUser(int eventId, List<UserEventDTO> myList)
        {
            try
            {
                if (myList.Count > 0)
                {
                    myList.ForEach(x => x.EventId = eventId);
                    var result = await eventUserService.GenerateEventUsers(myList, ConfigurationManager.AppSettings["connectionString"].ToString());
                    if (result != null && result.Count > 0)
                        return Ok(result);
                    else
                        return Ok(false);
                }
                return Ok("data is not passed");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("GetAttendeeHistory/{userId}")]
        public async Task<ResponseType<List<AttendeeHistory>>> GetAttendeeHistory([FromUri]int userId = 0)
        {
            try
            {
                if (userId == 0)
                    return ResponseType<List<AttendeeHistory>>.PerformError<List<AttendeeHistory>>("UserIdNotPassed", "Please provide valid User Id for Get Attendee History Request ... request time : " + DateTime.Now.ToShortDateString());

                var attendeeData = await eventUserService.GetAttendeeHistories(userId, ConfigurationManager.AppSettings["ConnectionString"].ToString());
                if (attendeeData == null || attendeeData.Count() == 0)
                    return ResponseType<List<AttendeeHistory>>.PerformError<List<AttendeeHistory>>("EmptyResponse", "No History Data found for user" + userId + "  at" + DateTime.Now.ToShortDateString());
                else
                    return ResponseType<List<AttendeeHistory>>.PerformSuccessed<List<AttendeeHistory>>(attendeeData);
            }
            catch (Exception ex)
            {
                return ResponseType<List<AttendeeHistory>>.PerformError<List<AttendeeHistory>>("Exception", "Some error has occurred while getting Favorite events : " + ex.Message + " ... Request Time: " + DateTime.Now.ToShortDateString());
            }
        }

        [HttpGet]
        [Route("UpdateAttendance")]
        public async Task<IHttpActionResult> UpdateAttendance(int EventId, int UserId, int IsAttended)
        {
            try
            {
                var data = await this.eventUserService.UpdateAttendance(EventId, UserId, IsAttended);
                if (data.Count > 0)
                {
                    return Ok(data);
                }
                else
                {
                    return Ok(false);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("GetEventUsers")]
        public async Task<IHttpActionResult> GetEventUsers(PagedRequestFilter request)
        {
            try
            {
                if (request.SortDirection == null)
                {
                    request.SortDirection = "desc";
                }
                if (request.SortBy == null)
                {
                    request.SortBy = "CreatedOn";
                }
                var items = await this.eventUserService.GetPagedEventUsers(request.Page, request.PageSize,
                 (request.SortDirection == "asc" ? true : false), request.Searchtext, request.SortBy, request.EventId);

                var pagedRecord = new PagedList();
                pagedRecord.TotalRecords = await this.eventUserService.GetNumbersOfPagedEventUsers(request.Searchtext, request.EventId);
                pagedRecord.Content = items;
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
        [Route("GetEventAttendee")]
        public async Task<IHttpActionResult> GetEventAttendee(int UserId, int EventId, int AgendaId)
        {
            try
            {
                var data = await this.eventUserService.GetEventAttendee(EventId, UserId, AgendaId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
