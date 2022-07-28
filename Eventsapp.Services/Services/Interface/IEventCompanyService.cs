using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.ReturnObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface IEventCompanyService
    {
        Task<EventCompanyDto> CreateEventCompany(EventCompanyDto eventCompany, string connString);
        Task<EventCompanyDto> UpdateEventCompany(EventCompanyDto eventCompany);
    }
}
