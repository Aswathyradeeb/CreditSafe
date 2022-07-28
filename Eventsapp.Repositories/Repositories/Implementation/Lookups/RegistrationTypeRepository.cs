using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace EventsApp.Repositories
{
    public class RegistrationTypeRepository : RepositoryBase<RegistrationType, int> , IRegistrationTypeRepository
    {
        public RegistrationTypeRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}
