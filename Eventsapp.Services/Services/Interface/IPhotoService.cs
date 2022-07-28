using EventsApp.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface IPhotoService
    {
        List<PhotoDto> CreatePhoto(List<PhotoDto> _photo);
        Task<List<PhotoDto>> GetAllPhotos();
        Task<List<PhotoReverseDto>> GetPhotosByEventId(int eventId);

    }
}
