using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.ReturnObjects;
using System.Threading.Tasks;

namespace Eventsapp.Services.Services.Interface
{
    public interface IPackageService
    {
        Task<ReturnPackageDto> GetAll(int page, int pageSize);
        bool CreatePackage(PackageDto model);
        Task<bool> UpdatePackage(PackageDto model);
    }
}
