using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace EventsApp.Repositories
{
    public class CityRepository : RepositoryBase<City, int> , ICityRepository
    {
        public CityRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}
