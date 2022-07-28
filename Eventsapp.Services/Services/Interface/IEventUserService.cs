using EventsApp.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface IEventUserService
    {
        EventDto Update(EventDto _eventUser, int userId);
        EventDto delete(EventDto _eventUser, int userId);
        Task<List<EventAttendeesDto>> GetEventUsersByEventId(int eventId);
        Task<EventUserDto> GetEventUsersbyAttendeeID(int attId);
        Task<byte[]> GetExcelTemplate();
        Task<EventUserDto> RegisteredUserAttended(EventUserDto RegisteredUser);
        Task<string> QRVisitorAttendance(int UserId, int EventId, int AgendaId, string lang);
        Task<string> AddEventUser(EventUserDto RegisteredUser, int? packageId, string StandLocation, string StandNumber);
        Task<List<EventUserDto>> GenerateEventUsers(List<UserEventDTO> myList, string connString);
        Task<List<AttendeeHistory>> GetAttendeeHistories(int userId, string connString);
        Task<List<EventUserDto>> UpdateAttendance(int EventId, int UserId, int IsAttended);
        Task<List<EventAttendeesDto>> GetPagedEventUsers(int pageIndex, int pageCount, bool ascending, string searchtext, string sortBy, int EventId);
        Task<int> GetNumbersOfPagedEventUsers(string searchtext, int EventId);
        Task<EventUserDto> GetEventAttendee(int EventId, int UserId, int AgendaId);
    }
}
