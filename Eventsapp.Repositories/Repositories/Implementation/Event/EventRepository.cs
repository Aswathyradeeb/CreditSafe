using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace eventsapp.Repositories
{
    public class EventRepository : RepositoryBase<Event, int> , IEventRepository
    {
        public EventRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}
