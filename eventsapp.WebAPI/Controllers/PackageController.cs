using eventsapp.WebAPI.Models;
using Eventsapp.Repositories.Repositories.Interface.Core;
using Eventsapp.Services.Services.Interface;
using EventsApp.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace eventsapp.WebAPI.Controllers
{
    [RoutePrefix("api/Package")]
    public class PackageController : ApiController
    {
        private IPackageService packageService;
        private IPackageRepository packageRepository;

        public PackageController(IPackageService packageService , IPackageRepository packageRepository)
        {
            this.packageService = packageService;
            this.packageRepository = packageRepository;
        }

        [HttpPost]
        [Route("GetAll")]
        public async Task<IHttpActionResult> GetAll(RequestFilter request)
        {
            try
            {
                var returnValue = await this.packageService.GetAll(request.Page, request.PageSize);
                var pagedRecord = new PagedList
                {
                    TotalRecords = returnValue.PackagesCount,
                    Content = returnValue.Packages,
                    CurrentPage = request.Page,
                    PageSize = request.PageSize
                };
                return Ok(pagedRecord);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("CreatePackage")]
        public IHttpActionResult CreatePackage(PackageDto model)
        {
            try
            {
                var data = packageService.CreatePackage(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("UpdatePackage")]
        public async Task<IHttpActionResult> UpdatePackage(PackageDto model)
        {
            try
            {
                var data = await packageService.UpdatePackage(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("GetPackages")]
        public async Task<IHttpActionResult> GetPackages()
        {
            try
            {
                var data = await this.packageRepository.GetAllAsync();
                var list = MapperHelper.Map<List<PackageDto>>(data);
                return Ok(list);
            }
            catch
            {
                return InternalServerError();
            }
        }

    }
}