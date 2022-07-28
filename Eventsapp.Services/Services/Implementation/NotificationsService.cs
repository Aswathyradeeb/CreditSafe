using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Eventsapp.Repositories;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using System.Web;
using System;
using Eventsapp.Services.Push_Notifications;

namespace Eventsapp.Services
{
    public class NotificationsService : INotificationsService
    {
        public readonly INotificationsRepository notificationRepo;
        private readonly IKeyedRepository<PushNotification, int> pushNotification;



        public NotificationsService(INotificationsRepository notificationRepo)
        {
            this.notificationRepo = notificationRepo;
        }

        public void RegisterIOS(IOSDevice _notification)
        {
            notificationRepo.Insert(_notification);
        }

        public async Task<IOSDevice> GetIOSUsers(int userId)
        {
            eventsappEntities entities = new eventsappEntities();
            List<IOSDevice> iosUser = await notificationRepo.QueryAsync(x => x.UserId == userId);
            IOSDevice ios = iosUser.FirstOrDefault();
            //entities.IOSDevice.Attach(ios);
            //var ff = entities.IOSDevices.Remove(ios);
            entities.SaveChanges();
            return iosUser.FirstOrDefault();
        }
        public async Task<string> SendPushNotification(PushNotification data)
        {
            string result = string.Empty;
            foreach (var recipient in data.Recipients)
            {
                if (!recipient.IsSent.Value || !recipient.IsSent.HasValue)
                {
                    result = NotificationConnector.InitiateFireBase(recipient.ReferenceConnectionId, data.Title, data.Message);
                    recipient.IsSent = result.ToLower().Contains("success") ? true : false;
                }
            }
            pushNotification.Insert(data);
            return result;
        }

    }
}
