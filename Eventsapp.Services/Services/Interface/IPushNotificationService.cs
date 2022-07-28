using EventsApp.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface IPushNotificationService
    {

        void sendNotification(NotifyIOSDto notifyIOSDto, string[] token);
        void sendNotificationAndroid(NotifyIOSDto notifyIOSDto);

    }
}
