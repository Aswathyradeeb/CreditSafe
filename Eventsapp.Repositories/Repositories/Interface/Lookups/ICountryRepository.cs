using EventsApp.Domain.Entities;
using EventsApp.Framework;

namespace Eventsapp.Repositories
{
    public interface ICountryRepository : IKeyedRepository<Country, int>
    {
    }
}
