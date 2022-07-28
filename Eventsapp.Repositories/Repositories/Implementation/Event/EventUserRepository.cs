using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace EventsApp.Repositories
{
    public class EventUserRepository : RepositoryBase<EventUser, int> , IEventUserRepository
    {
        public EventUserRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}
