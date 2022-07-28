using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace EventsApp.Repositories
{
    public class UserRepository : RepositoryBase<User, int> , IUserRepository
    {
        public UserRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}
