using Eventsapp.Repositories;
using EventsApp.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace sponsersapp.WebAPI.Controllers
{
    [RoutePrefix("api/Country")]
    public class CountryController : ApiController
    {
        private readonly ICountryRepository countryRepository; 

        public CountryController(ICountryRepository _countryRepository)
        {
            this.countryRepository = _countryRepository;
        }

        [HttpGet]
        [Route("GetCountries")]
        public async Task<IHttpActionResult> GetCountries()
        {
            try
            {
                var countries = await this.countryRepository.GetAllAsync();
                var countriesList = MapperHelper.Map<List<CountryWithStateDto>>(countries);
                return Ok(countriesList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}