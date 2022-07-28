using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace EventsApp.Repositories
{
    public class EventPersonRepository : RepositoryBase<EventPerson, int> , IEventPersonRepository
    {
        public EventPersonRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}
