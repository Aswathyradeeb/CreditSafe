using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace eventsapp.Repositories
{
    public class NotificationsRepository : RepositoryBase<IOSDevice, int> , INotificationsRepository
    {
        public NotificationsRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}
