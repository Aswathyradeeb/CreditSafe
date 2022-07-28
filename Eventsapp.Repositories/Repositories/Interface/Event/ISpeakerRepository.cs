using EventsApp.Domain.Entities;
using EventsApp.Framework;

namespace Eventsapp.Repositories
{
    public interface ISpeakerRepository : IKeyedRepository<Person, int>
    {
    }
}
