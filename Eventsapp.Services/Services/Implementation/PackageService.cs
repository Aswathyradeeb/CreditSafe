using Eventsapp.Repositories.Repositories.Interface.Core;
using Eventsapp.Services.Services.Interface;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.ReturnObjects;
using EventsApp.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository packageRepository;


        public PackageService(IPackageRepository packageRepository)
        {
            this.packageRepository = packageRepository;
        }

        public bool CreatePackage(PackageDto model)
        {
            var objMap = MapperHelper.Map<Package>(model);
            objMap.SponserType = null;
            objMap.Event = null;
            objMap.EventCompanies = null; 
            this.packageRepository.Insert(objMap);
            return true;
        }

        public async Task<ReturnPackageDto> GetAll(int page, int pageSize)
        { 
            var packages = await this.packageRepository.GetAllAsync(); 
            var lst = MapperHelper.Map<List<PackageDto>>(packages.Skip((page - 1) * pageSize).Take(pageSize).ToList()); 
            return new ReturnPackageDto { Packages = lst, PackagesCount = lst.Count() }; 
        }

        public async Task<bool> UpdatePackage(PackageDto model)
        {
            var package =await this.packageRepository.GetAsync(model.Id);
            package.Benefits = model.Benefits;
            package.Cost = model.Cost; 
            package.SponsorTypeId = model.SponsorTypeId;
            return true;

        }
    }
}
