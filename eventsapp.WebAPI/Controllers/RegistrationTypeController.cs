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
    [RoutePrefix("api/RegistrationType")]
    public class RegistrationTypeController : ApiController
    {
        private readonly IRegistrationTypeRepository _registrationTypeRepository;

        public RegistrationTypeController(IRegistrationTypeRepository registrationTypeRepository)
        {
            this._registrationTypeRepository = registrationTypeRepository;
        }
                                                                                                                            
        public async Task<IHttpActionResult> GetRegistrationTypes()
        {
            try
            {
                var data = await this._registrationTypeRepository.GetAllAsync();
                var result = MapperHelper.Map<List<RegistrationTypeDto>>(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}