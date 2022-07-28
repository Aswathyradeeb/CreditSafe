using EventsApp.Domain.Entities;
using EventsApp.Framework;

namespace Eventsapp.Repositories
{
    public interface IEventRepository : IKeyedRepository<Event, int>
    {
    }
}
