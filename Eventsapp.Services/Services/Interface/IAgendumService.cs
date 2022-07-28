using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.ReturnObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface IAgendumService
    {

        Task<IEnumerable<AgendumDto>> GetAgendaByEventId(int EventId);
        Task<List<AgendumLightDto>> GetAgendaEvent(int EventId);

    }
}
