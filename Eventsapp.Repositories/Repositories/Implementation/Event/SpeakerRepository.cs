using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace EventsApp.Repositories
{
    public class SpeakerRepository : RepositoryBase<Person, int> , ISpeakerRepository
    {
        public SpeakerRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}
