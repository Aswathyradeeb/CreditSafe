using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.ReturnObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface IAgendaInterestedService
    {


        InterestedAgendaDto RegisterEventAgenda(InterestedAgendaDto _InterestedAgendaDto);
        
    }
}
