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
using EventsApp.Domain.DTOs;

namespace eventsapp.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/SendNotifications")]
    public class NotifyController : ApiController
    {
        //public INotificationsRepository notificationRepo;
        //public INotificationsService notificationsService;
        private readonly INotifyService notifyService;
        private ICurrentUser user;


        public NotifyController(INotifyService notifyService, ICurrentUser user)
        {
            //this.notificationRepo = notificationRepo;
            //this.notificationsService=  notificationsService;
            this.notifyService = notifyService;
            this.user = user;
        }

        [Route("GetNotifictions")]
        public async Task<IHttpActionResult> GetNotifictions()
        {
            try
            {
                string userId = User.Identity.GetUserId();
                var notificationDtos = await this.notifyService.GetNotifictions(userId);
                return Ok(notificationDtos);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

        }


        [Route("MakAsRead")]
        public IHttpActionResult MakAsRead(NotificationPostDto notif)
        {
            try
            {
                notifyService.UpdateNotification(true, notif.NotificationId);
                return Json(new { result = "Success" });
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [Route("RegisterIOS")]
        public IHttpActionResult RegisterIOS(IOSDevice _notification)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                _notification.UserId = this.user.UserInfo.Id;
                //notifyService.RegisterIOS(_notification);
                return Ok(_notification);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

    }
}