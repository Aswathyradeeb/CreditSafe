using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.ReturnObjects;
using EventsApp.Domain.DTOs.Subscription;
using EventsApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface IEventService
    {
        Task<List<EventLightDto>> GetEvents();
        Task<string> getAdminMail();
        EventDto CreateEvent(EventSingleDto _event);
        Task<EventDto> GetEventId(int applicationId);
        Task<EventRegitrationDto> GetEventRegitration(int applicationId);
        Task<ReturnEventsDto> GetAllEvents(FilterParams filterParams, int page, int pageSize);
        Task<ReturnEventsDto> GetEventsByCompanyName(string compName, FilterParams filterParams, int page, int pageSize);
        Task<ReturnEventsDto> GetEventsByCompanyId(int userId, FilterParams filterParams, int page, int pageSize);
        Task<EventDto> UpdateEvent(EventDto _eventDto);
        string DeleteEvent(int eventId);
        Task<EventUser> RegisterEventUser(EventRegistrationDto RegistrationModel);
        Task<EventUserDto> IsRegisterEvent(EventRegistrationDto RegistrationModel);
        Task<List<EventLightDto>> GetEventsByUserId(int userId);
        Task<EventsGroupingByWeek> GetEventsGroupingByWeek(string companyCode);
        AttendeeQuestionDto Addquestion(AttendeeQuestionDto _addQuestion);
        Task<List<AttendeeQuestionDto>> GetQuestions(int eventid, int speakerid);
        Task<AttendeeQuestionDto> UpdateAnswer(AttendeeQuestionDto _qstnDto);
        Task<List<UserSubscriptionSingleDto>> GetPaidPackages(int userId);
        int checkSubsription(int userId, int subscriptionId, int eventId);
    }
}
