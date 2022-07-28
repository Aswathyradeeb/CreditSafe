using EventsApp.Domain.Entities;
using EventsApp.Framework;

namespace Eventsapp.Repositories
{
    public interface IAgendumRepository : IKeyedRepository<Agendum, int>
    {
    }
}
