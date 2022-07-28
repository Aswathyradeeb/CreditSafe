﻿using EventsApp.Domain.Entities;
using EventsApp.Framework;

namespace Eventsapp.Repositories
{
    public interface INotificationsRepository : IKeyedRepository<IOSDevice, int>
    {
    }
}
