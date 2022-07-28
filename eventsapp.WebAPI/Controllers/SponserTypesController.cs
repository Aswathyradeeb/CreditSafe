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
    [RoutePrefix("api/SponserTypes")]
    public class SponserTypesController : ApiController
    {
        private readonly ISponserTypeRepository _sponserTypeRepository;

        public SponserTypesController(ISponserTypeRepository sponserTypeRepository)
        {
            this._sponserTypeRepository = sponserTypeRepository;
        }

        // GET: api/SponserTypes 
        public async Task<IHttpActionResult> GetSponserTypes()
        {
            try
            {
                var _sponserTypes = await this._sponserTypeRepository.GetAllAsync();
                var sponserTypeDtos = MapperHelper.Map<List<SponserTypeDto>>(_sponserTypes);
                return Ok(sponserTypeDtos);
            }
            catch
            {
                return InternalServerError();
            }
        }

    
   
    }
}