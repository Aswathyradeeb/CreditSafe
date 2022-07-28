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
using Eventsapp.Services.Services.Interface;

namespace eventsapp.WebAPI.Controllers
{
   // [Authorize]
    [RoutePrefix("api/InterestedAgenda")]
    public class InterestedAgendaController : ApiController
    {
        private IAgendaInterestedService agendaService;
        private readonly IAgendaInterestedRepository agendaInterestedRepository;
        private ICurrentUser user;
       

        public InterestedAgendaController(IAgendaInterestedService agendaService, IAgendaInterestedRepository _agendaRepository, ICurrentUser user)
        {
            this.agendaService = agendaService;
            this.agendaInterestedRepository = _agendaRepository;
            this.user = user;
        }


        [Route("SaveAgenda")]
        public IHttpActionResult RegisterEvent(InterestedAgendaDto _InterestedAgendaDto)
        {
            try
            { 
                _InterestedAgendaDto.UserId = this.user.UserInfo.Id;
                var agendaDto = agendaService.RegisterEventAgenda(_InterestedAgendaDto);
                return Ok(agendaDto);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }


    }
}