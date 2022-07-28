using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface INotificationsService
    {
       void RegisterIOS(IOSDevice _notification);
       Task<IOSDevice> GetIOSUsers(int userId);
       Task<string> SendPushNotification(PushNotification data);
    }
}
