
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Eventsapp.Repositories;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using System.Web;
using System;
using EventsApp.Domain.DTOs.ReturnObjects;

namespace Eventsapp.Services
{
    public class AgendaInterestedService : IAgendaInterestedService
    {
        private readonly IAgendaInterestedRepository agendaRepository;
         
        

        public AgendaInterestedService(IAgendaInterestedRepository agendaRepository )
        {
            this.agendaRepository = agendaRepository;
           
        }

      
        public InterestedAgendaDto RegisterEventAgenda(InterestedAgendaDto _InterestedAgenda)
        {
            InterestedAgenda agendaMap = MapperHelper.Map<InterestedAgenda>(_InterestedAgenda);
            this.agendaRepository.Insert(agendaMap);
            InterestedAgendaDto _eventAgendaDto = MapperHelper.Map<InterestedAgendaDto>(agendaMap);
            return _eventAgendaDto;
        }

        
    }
}
