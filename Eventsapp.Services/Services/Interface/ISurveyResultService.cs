using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface ISurveyResultService
    {
        void Vote(SurveyResult surveyResult);
    }
}
