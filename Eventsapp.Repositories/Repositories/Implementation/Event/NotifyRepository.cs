using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace eventsapp.Repositories
{
    public class NotifyRepository : RepositoryBase<Notification, int>, INotifyRepository
    {
        public NotifyRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}
