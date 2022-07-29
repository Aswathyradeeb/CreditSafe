using eventsapp.WebAPI.Models;
using Eventsapp.Services;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace EventsApp.WebAPI.Controllers
{
    [RoutePrefix("api/City")]
    public class CityController : ApiController
    {
        private readonly ICityService _cityServices;
        private readonly IKeyedRepository<City, int> _cityRepository;
        

        public CityController(ICityService _cityServices,
            IKeyedRepository<City, int> _cityRepository)
        {
            this._cityServices = _cityServices;
            this._cityRepository = _cityRepository;
        }

        [Route("GetCountries")]
        [HttpPost]
        public async Task<IHttpActionResult> GetCountries(RequestFilter request)
        {
            try
            {
                var returnValue = await this._cityServices.GetAllCountries(request.FilterParams, request.Page, request.PageSize);
                var pagedRecord = new PagedList
                {
                    TotalRecords = returnValue.countryCount,
                    Content = returnValue.countries,
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


        [Route("GetCities")]
        [HttpPost]
        public async Task<IHttpActionResult> GetCities(RequestFilter request)
        {
            try
            {
                var returnValue = await this._cityServices.GetAllCities(request.FilterParams, request.Page, request.PageSize, request.Searchtext);
                var pagedRecord = new PagedList
                {
                    TotalRecords = returnValue.cityCount,
                    Content = returnValue.cities,
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



        [Route("GetCityWeather")]
        [HttpPost]
        public async Task<IHttpActionResult> GetCityWeather(RequestFilter request)
        {
            try
            {
                var returnValue = await this._cityServices.GetAllCityWeather(request.FilterParams, request.Page, request.PageSize, request.Searchtext);
                var pagedRecord = new PagedList
                {
                    TotalRecords = returnValue.cityCount,
                    Content = returnValue.cities,
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
        [Route("CreateUpdateCity")]
        public async Task<IHttpActionResult> CreateUpdateCity(CityDto city)
        {
            try
            {
                var _city = await this._cityServices.CreateUpdateCity(city);               
                return Ok(_city);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

   

        [HttpGet]
        [Route("DeleteCity")]
        public async Task<IHttpActionResult> DeleteCity(int id)
        {
            try
            {
                var data = await this._cityServices.DeleteCity(id);
                return Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        
    }
}