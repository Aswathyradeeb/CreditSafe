using EventsApp.Domain.Entities;
using EventsApp.Framework;

namespace Eventsapp.Repositories
{
    public interface IEventTypeRepository : IKeyedRepository<EventType, int>
    {
    }
}
