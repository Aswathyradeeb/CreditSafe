using EventsApp.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventsapp.Services.Services.Interface
{
    public interface IPollingService
    {
        Task<bool> SubmitPoll(AttendeeQuestionDto myQuestions);
        Task<Dictionary<int, List<AttendeeQuestionDto>>> GetPollingQuestions(string type, int id);
        KeyValuePair<bool, string> SubmitSurvey(string connectionString, List<SurveyResponse> response);
        Task<List<AttendeeQuestionDto>> SubmitPollResponse(int questionId, int userId);
        Task<List<GuestQuestion>> GetGuestQuestion(int agendaId, int userId);
    }
}
