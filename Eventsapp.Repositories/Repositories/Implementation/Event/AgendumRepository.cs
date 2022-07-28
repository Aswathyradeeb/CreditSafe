using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace eventsapp.Repositories
{
    public class AgendumRepository : RepositoryBase<Agendum, int> , IAgendumRepository
    {
        public AgendumRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}
