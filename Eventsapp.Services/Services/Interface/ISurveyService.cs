using EventsApp.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface ISurveyService
    {
        void UpdateSurvey(SurveyDto _Survey);
        void CreateSurvey(SurveyDto surveyDto);
        Task<List<SurveyDto>> GetSurveys(int userId);
        Task<List<SurveyDto>> GetSurvey(int eventId);
        Task<List<SurveyResultDto>> GetSurveyResult(int surveyId, int userId);
        Task<string> DeleteSurvey(int surveyid);
        Task<List<SpeakerQuestions>> GetSurveyFromSpeaker(int eventId, int agendaId);
        Task<List<SpeakerQuestions>> AddSpeakerQuestion(List<AddSpeakerQuestionDTO> data);
    }
}
