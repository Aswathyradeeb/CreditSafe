using EventsApp.Domain.Entities;
using EventsApp.Framework;

namespace Eventsapp.Repositories
{
    public interface ICityRepository : IKeyedRepository<City, int>
    {
    }
}
