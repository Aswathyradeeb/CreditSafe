using Eventsapp.Repositories;
using EventsApp.Domain.Entities;
using EventsApp.Framework;


namespace eventsapp.Repositories
{
    public class PhotoRepository : RepositoryBase<Photo, int> , IPhotoRepository
    {
        public PhotoRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }
    }
}
