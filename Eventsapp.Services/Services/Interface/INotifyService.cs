using EventsApp.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface INotifyService
    {
        void UpdateNotification(bool IsRead, int id);
        Task<List<NotifyIOSDto>> GetNotifictions(string userId);
    }
}
