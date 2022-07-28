using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using System.Linq;

namespace EventsApp.Repositories
{
    public class ConfigurationsRepository : RepositoryBase<Configuration, int>, IConfigurationsRepository
    {
        public ConfigurationsRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }


        public string GetByKey(string key)
        {
            return GetAll().First(x => x.Key == key).Value;
        }
    }
}
