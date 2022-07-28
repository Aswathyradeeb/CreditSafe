using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace eventsapp.Repositories
{
    public class EventTypeRepository : RepositoryBase<EventType, int> , IEventTypeRepository
    {
        public EventTypeRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}
