using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace EventsApp.Repositories
{
    public class SponserTypeRepository : RepositoryBase<SponserType, int> , ISponserTypeRepository
    {
        public SponserTypeRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}
