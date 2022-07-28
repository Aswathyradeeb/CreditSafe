using EventsApp.Domain.Entities;
using EventsApp.Domain.Enums;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventsapp.Services.Schedular
{
    public class VoucherJobShcedular : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            eventsappEntities entity = new eventsappEntities();
            List<User> UserLst = entity.Users.Where(u => u.RegistrationTypeId == (int?)RegistrationTypeEnum.Athlete || 
            u.RegistrationTypeId == (int?)RegistrationTypeEnum.Official ||
            u.RegistrationTypeId == (int?)RegistrationTypeEnum.Guest).ToList();
            foreach (var item in UserLst)
            {
                item.UsedDrinkVouchers = 0;
                item.UsedFoodVouchers = 0;
            }
            entity.SaveChangesAsync();
        }
    }
}
