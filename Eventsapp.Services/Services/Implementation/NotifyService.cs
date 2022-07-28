using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Eventsapp.Repositories;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using System.Web;
using System;

namespace Eventsapp.Services
{
    public class NotifyService : INotifyService
    {
        private readonly INotifyRepository notifyRepository;

        public NotifyService(INotifyRepository notifyRepository)
        {
            this.notifyRepository = notifyRepository;
        }

        public void UpdateNotification(bool IsRead, int id)
        {
            Notification notifyEntity = this.notifyRepository.Get(id);
            notifyEntity.IsRead = IsRead;
        }

        public async Task<List<NotifyIOSDto>> GetNotifictions(string userId)
        {
            var notifications = await this.notifyRepository.QueryAsync(x => x.IsRead == false);
            List<NotifyIOSDto> notifyIOSDto = new List<NotifyIOSDto>();

            foreach (var item in notifications)
            {
                notifyIOSDto.Add(new NotifyIOSDto()
                {
                    NameEn = item.Event.NameEn,
                    NameAr = item.Event.NameAr,
                    StartDate = item.Event.StartDate,
                    BannerPhoto = item.Event.BannerPhoto,
                    EventId = item.Event.Id,
                    IsRead = item.IsRead,
                    UserId = userId,
                    Id = item.Id
                });
            }
            return notifyIOSDto.OrderByDescending(x => x.StartDate).ToList();

        }
    }
}
