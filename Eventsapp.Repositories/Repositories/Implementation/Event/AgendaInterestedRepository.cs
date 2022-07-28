using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace eventsapp.Repositories
{
    public class AgendaInterestedRepository : RepositoryBase<InterestedAgenda, int> , IAgendaInterestedRepository
    {
        public AgendaInterestedRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}
