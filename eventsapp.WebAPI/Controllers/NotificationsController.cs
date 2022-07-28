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
using Eventsapp.Repositories;
using System.Threading.Tasks;
using Eventsapp.Services;
using EventsApp.Domain.Entities;
using Microsoft.AspNet.Identity;
using EventsApp.Domain.DTOs.Core;

namespace eventsapp.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Notifications")]
    public class NotificationsController : ApiController
    {
        public INotificationsRepository notificationRepo;
        public INotificationsService notificationsService;

        public NotificationsController(INotificationsRepository notificationRepo, INotificationsService notificationsService)
        {
            this.notificationRepo = notificationRepo;
            this.notificationsService = notificationsService;
        }

        [Route("RegisterIOS")]
        public IHttpActionResult RegisterIOS(IOSDevice _notification)
        {
            try
            {
                //_notification.UserId = this.user.UserInfo.Id;
                notificationsService.RegisterIOS(_notification);
                return Ok(_notification);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        //For Any type of notification
        [HttpPost]
        [Route("PushNotification")]
        public async Task<ResponseType<string>> PushNotification(PushNotification data)
        {
            try
            {
                if (data == null)
                    return ResponseType<string>.PerformError<string>("DataNotProvided", "Data is not provided for push Notification", "");
                var result =await notificationsService.SendPushNotification(data);
                return result != string.Empty ? ResponseType<string>.PerformSuccessed<string>(result) :   ResponseType<string>.PerformError<string>("NotSend", "Could not send Notification", "");
            }
           
            catch(Exception ex)
            {
                return ResponseType<string>.PerformError<string>("Exception", "Some error occured in Push Notification... "+ex.Message, "");
            }

        }

    }
}