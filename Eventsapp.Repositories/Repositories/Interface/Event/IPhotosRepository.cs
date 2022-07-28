using EventsApp.Domain.Entities;
using EventsApp.Framework;

namespace Eventsapp.Repositories
{
    public interface IPhotoRepository : IKeyedRepository<Photo, int>
    {
    }
}
