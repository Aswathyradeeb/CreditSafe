using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.ReturnObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface IEventPersonService
    {
        Task<EventPersonDto> CreateEventPerson(EventPersonDto eventPerson, string connString);
        Task<EventPersonDto> UpdateEventPerson(EventPersonDto eventPerson);
    }
}
