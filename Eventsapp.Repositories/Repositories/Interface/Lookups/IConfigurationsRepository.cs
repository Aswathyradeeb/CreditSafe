using EventsApp.Domain.Entities;
using EventsApp.Framework;

namespace Eventsapp.Repositories
{
    public interface IConfigurationsRepository : IKeyedRepository<Configuration, int>
    {
        string GetByKey(string key);
    }
}
