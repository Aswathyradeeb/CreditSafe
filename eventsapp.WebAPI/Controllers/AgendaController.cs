using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web;
using System.Data.Entity.Validation;
using Eventsapp.Services;
using System.Threading.Tasks;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Eventsapp.Repositories;
using eventsapp.WebAPI.Models;

namespace agendasapp.WebAPI.Controllers
{
   // [Authorize]
    [RoutePrefix("api/Agenda")]
    public class AgendaController : ApiController
    {
        private IAgendumService agendaService;
        private readonly IAgendumRepository agendaRepository;

        public AgendaController(IAgendumService agendaService, IAgendumRepository _agendaRepository)
        {
            this.agendaService = agendaService;
            this.agendaRepository = _agendaRepository;
        }
        [Route("GetAgendaByEventId")]
        [ResponseType(typeof(Agendum))]
        [HttpPost]
        public async Task<IHttpActionResult> GetAgenda(EventLightDto EventId)
        {
            try
            {
                var agendaObj = await agendaService.GetAgendaByEventId(EventId.Id);
                return Ok(agendaObj);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("GetAgendaEvent")]
        [ResponseType(typeof(Agendum))]
        [HttpGet]
        public async Task<IHttpActionResult> GetAgendaEvent(int EventId)
        {
            try
            {
                var agendaObj = await agendaService.GetAgendaEvent(EventId);
                return Ok(agendaObj);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [Route("GetAgendaByEventId")]
        [ResponseType(typeof(Agendum))]
        [HttpGet]
        public async Task<IHttpActionResult> GetAgendaById(int EventId)
        {
            try
            {
                var agendaObj = await agendaService.GetAgendaByEventId(EventId);
                return Ok(agendaObj);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}