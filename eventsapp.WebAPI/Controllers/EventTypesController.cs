using Eventsapp.Repositories;
using EventsApp.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace eventsapp.WebAPI.Controllers
{
    [AllowAnonymous]
    public class EventTypesController : ApiController
    {
        private readonly IEventTypeRepository _eventTypeRepository;

        public EventTypesController(IEventTypeRepository eventTypeRepository)
        {
            this._eventTypeRepository = eventTypeRepository;
        }

        // GET: api/EventTypes 
        public async Task<IHttpActionResult> GetEventTypes()
        {
            try
            {
                var _eventTypes = await this._eventTypeRepository.GetAllAsync();
                var eventTypeDtos = MapperHelper.Map<List<EventTypeDto>>(_eventTypes);
                return Ok(eventTypeDtos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}