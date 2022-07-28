using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace EventsApp.Repositories
{
    public class CountryRepository : RepositoryBase<Country, int> , ICountryRepository
    {
        public CountryRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}
