using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.ReturnObjects;
using EventsApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface ISpeakerService
    {
        Task<PersonDto> CreateSpeaker(PersonDto _speaker, string connString);
        Task<PersonDto> GetSpeakerId(int id);
        Task<ReturnSpeakerDto> GetAllSpeakers(FilterParams filterParams, int page, int pageSize);
        Task<PersonDto> UpdateSpeaker(PersonDto _speaker);
        Task<List<PersonDto>> GetSpeakers();
        Task<List<EventPersonDto>> GetSpeakersByEventId(int EventId);
        Task<string> DeleteSpeaker(int id);
        Task<PersonDto> GetSpeakerUser(int userid);
        Task<SpeakerRatingDto> SubmitSpeakerRating(SpeakerRatingDto SpeakerRating);
    }
}
