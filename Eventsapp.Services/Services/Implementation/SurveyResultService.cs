using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Eventsapp.Repositories;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using System.Web;
using System;
using Eventsapp.Services;

namespace Eventsapp.Services
{
    public class SurveyResultService : ISurveyResultService
    { 
        private readonly ISurveyResultsRepository SurveyResultRepository;
        private readonly ISurveyRepository SurveyRepository;


        public SurveyResultService(ISurveyResultsRepository SurveyResultRepository, ISurveyRepository SurveyRepository)
        {
            this.SurveyResultRepository = SurveyResultRepository;
            this.SurveyRepository = SurveyRepository;
        }

        public void Vote(SurveyResult SurveyResult)
        {
            var survey =  this.SurveyRepository.Query(x => x.SurveyOptions.FirstOrDefault(y => y.Id == SurveyResult.SurveyOptionId) != null).FirstOrDefault();
            SurveyResult surveyResultObj=  this.SurveyResultRepository.Query(a => a.UserId == SurveyResult.UserId && a.SurveyOption.SurveyId == survey.Id).FirstOrDefault() ;
            if (surveyResultObj != null)
            {
                surveyResultObj.SurveyOptionId = SurveyResult.SurveyOptionId;
            }
            else
            {
                SurveyResult.CreatedOn = DateTime.Now;
                this.SurveyResultRepository.Insert(SurveyResult);
            }
        }
    }
}
