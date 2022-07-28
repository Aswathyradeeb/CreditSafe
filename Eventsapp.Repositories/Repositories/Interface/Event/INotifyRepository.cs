using EventsApp.Domain.Entities;
using EventsApp.Framework;

namespace Eventsapp.Repositories
{
    public interface INotifyRepository : IKeyedRepository<Notification, int>
    {
    }
}
