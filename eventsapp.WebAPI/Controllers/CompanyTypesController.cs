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

namespace eventsapp.WebAPI.Controllers
{
    [RoutePrefix("api/CompanyTypes")]
    public class CompanyTypesController : ApiController
    {

        private readonly ICompanyTypeRepository _eventTypeRepository;

        public CompanyTypesController(ICompanyTypeRepository eventTypeRepository)
        {
            this._eventTypeRepository = eventTypeRepository;
        }

        // GET: api/CompanyTypes 
        public async Task<IHttpActionResult> GetCompanyTypes()
        {
            try
            {
                var _eventTypes = await this._eventTypeRepository.GetAllAsync();
                var eventTypeDtos = MapperHelper.Map<List<CompanyTypeDto>>(_eventTypes);
                return Ok(eventTypeDtos);
            }
            catch
            {
                return InternalServerError();
            }
        }


    }
}