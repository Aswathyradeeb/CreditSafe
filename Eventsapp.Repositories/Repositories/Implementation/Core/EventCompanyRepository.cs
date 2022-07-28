using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace eventsapp.Repositories
{
    public class EventCompanyRepository : RepositoryBase<EventCompany, int> , IEventCompanyRepository
    {
        public EventCompanyRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}
