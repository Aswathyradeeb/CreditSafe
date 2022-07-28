using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Eventsapp.Repositories;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using System.Web;
using System;
using EventsApp.Domain.Enums;

namespace Eventsapp.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly ISurveyRepository SurveyRepository;
        private readonly ISurveyResultsRepository SurveyResultRepository;
        private readonly IUserRepository userRepo;

        public SurveyService(ISurveyRepository SurveyRepositoryss, IUserRepository userRepo, ISurveyResultsRepository SurveyResultRepo)
        {
            this.SurveyRepository = SurveyRepositoryss;
            this.userRepo = userRepo;
            SurveyResultRepository = SurveyResultRepo;
        }

        public void CreateSurvey(SurveyDto SurveyDto)
        {
            Survey questEntity = MapperHelper.Map<Survey>(SurveyDto);
            questEntity.Event = null;
            questEntity.Agendum = null;
            questEntity.AgendaId = questEntity.AgendaId != 0 ? questEntity.AgendaId : (int?)null;
            this.SurveyRepository.Insert(questEntity);
        }

        public void UpdateSurvey(SurveyDto _Survey)
        {
            var SurveyEntity = this.SurveyRepository.Get(_Survey.Id);
            var SurveyResultEntity = this.SurveyResultRepository.GetAll();
            var Survey = MapperHelper.Map<Survey>(_Survey);

            SurveyEntity.NameAr = Survey.NameAr;
            SurveyEntity.AgendaId = Survey.AgendaId;
            SurveyEntity.NameEn = Survey.NameEn;
            SurveyEntity.EventId = Survey.EventId;
            SurveyEntity.FromSpeaker = Survey.FromSpeaker;
            SurveyEntity.ResponseAlotTime = Survey.ResponseAlotTime;

            List<SurveyOption> deletedSurveyOption = SurveyEntity.SurveyOptions.ToList();
            foreach (SurveyOption item in deletedSurveyOption)
            {
                SurveyEntity.SurveyOptions.Remove(item);
            }

            List<SurveyOption> addedSurveyOption = Survey.SurveyOptions.ToList();
            foreach (SurveyOption item in addedSurveyOption)
            {
                SurveyEntity.SurveyOptions.Add(item);
            }



            List<SurveyOption> updatedSurveyOptions = Survey.SurveyOptions.Where(x => SurveyEntity.SurveyOptions.Where(y => y.Id == x.Id).FirstOrDefault() == null).ToList();
            foreach (SurveyOption item in updatedSurveyOptions)
            {
                SurveyOption updatedSurveyOption = Survey.SurveyOptions.Where(y => y.Id == item.Id).FirstOrDefault();
                item.NameAr = updatedSurveyOption.NameAr;
                item.NameEn = updatedSurveyOption.NameEn;
                item.OrderNumber = updatedSurveyOption.OrderNumber;
                item.SurveyId = updatedSurveyOption.SurveyId;
            }

        }

        public async Task<string> DeleteSurvey(int surveyid)
        {
            var survey = SurveyRepository.Query(x => x.Id == surveyid).FirstOrDefault();
            survey.DeletedOn = DateTime.Now;
            return "Deleted Successfully";
        }
        public async Task<List<SurveyResultDto>> GetSurveyResult(int surveyId, int userId)
        {
            var surveyResults = await SurveyResultRepository.QueryAsync(x => x.SurveyOption.SurveyId == surveyId);
            var surveyResults1 = surveyResults.Where(x => x.UserId == userId).FirstOrDefault();
            var surveysDtos = MapperHelper.Map<List<SurveyResultDto>>(surveyResults1);
            return surveysDtos;
        }

        public async Task<List<SurveyDto>> GetSurveys(int userId)
        {
            var userObj = this.userRepo.Query(a => a.Id == userId).FirstOrDefault();
            bool isSuperAdmin = userObj.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0;
            List<Survey> surveys = SurveyRepository.Query(x => x.DeletedOn == null && x.Event.DeletedOn == null && (isSuperAdmin == true || x.Event.CreatedBy == userId || x.Event.EventUsers.Count(y => y.UserId == userId) > 0)).ToList();
            var surveysDtos = MapperHelper.Map<List<SurveyDto>>(surveys.ToList());
            foreach (var survey in surveysDtos)
            {
                var surveyResult = SurveyResultRepository.Query(a => a.UserId == userId && a.SurveyOption.SurveyId == survey.Id).FirstOrDefault();
                if (surveyResult != null)
                {
                    var surveyOption = survey.SurveyOptions.FirstOrDefault(a => a.Id == surveyResult.SurveyOptionId);
                    surveyOption.selectedId = surveyResult.SurveyOptionId;
                }
            }
            return surveysDtos;
        }

        public async Task<List<SurveyDto>> GetSurvey(int eventId)
        {
            IEnumerable<Survey> surveys = SurveyRepository.Query(x => x.EventId == eventId && x.Event.DeletedOn == null && x.DeletedOn == null);
            var surveysDtos = MapperHelper.Map<List<SurveyDto>>(surveys.Where(x => x.DeletedOn == null).ToList());
            if (surveys.Count() != 0)
            {
                //var surveyResults =  SurveyResultRepository.GetAll();
                foreach (var item in surveys)
                {
                    decimal totalResults = item.SurveyOptions.Where(x => x.SurveyResults.Count != 0).LongCount();
                    foreach (var surveyoption in item.SurveyOptions)
                    {
                        if (totalResults > 0)
                        {
                            var surveyOptionDto = surveysDtos.FirstOrDefault(a => a.Id == item.Id).SurveyOptions.FirstOrDefault(a => a.Id == surveyoption.Id);
                            surveyOptionDto.Progress = Convert.ToDecimal(surveyoption.SurveyResults.Count) / totalResults * 100;
                        }
                    }
                }
            }
            return surveysDtos;
        }
        public async Task<List<SpeakerQuestions>> GetSurveyFromSpeaker(int eventId, int agendaId)
        {
            List<SurveyDto> surveysDtos = new List<SurveyDto>();
            List<SpeakerQuestions> survey = new List<SpeakerQuestions>();
            IEnumerable<Survey> surveys;
            if (eventId > 0)
                surveys = await SurveyRepository.QueryAsync(x => x.EventId == eventId && x.Event.DeletedOn == null && x.DeletedOn == null);
            else
                surveys = await SurveyRepository.QueryAsync(x => x.DeletedOn == null);
            if (surveys != null)
            {
                var allSurvey = surveys.Where(x => x.AgendaId.HasValue).ToList();
                if (allSurvey.Count > 0)
                {
                    var mySurveys = allSurvey.Where(x => x.DeletedOn == null && x.AgendaId.Value == agendaId && x.FromSpeaker != null && x.FromSpeaker == true).ToList();
                    foreach (var x in mySurveys)
                    {
                        survey.Add(new SpeakerQuestions()
                        {
                            Id = x.Id,
                            AgendaId = x.Agendum.Id,
                            TitleAr = x.Agendum.TitleAr,
                            TitleEn = x.Agendum.TitleEn,
                            NameAr = x.NameAr,
                            NameEn = x.NameEn,
                            Date = x.Agendum.StartDate,
                            FromTime = x.Agendum.FromTime,
                            ToTime = x.Agendum.ToTime,
                            SurveyOptions = x.SurveyOptions.Count > 0 ? MapperHelper.Map<List<SurveyOptionDto>>(x.SurveyOptions) : null,
                            ResponseAlotTime = x.ResponseAlotTime
                        });
                    }
                }
            }
            return survey;
        }
        public async Task<List<SpeakerQuestions>> AddSpeakerQuestion(List<AddSpeakerQuestionDTO> data)
        {
            foreach (var question in data)
            {
                var Survey = new Survey()
                {
                    NameAr = question.NameAr,
                    NameEn = question.NameEn,
                    AgendaId = question.AgendaId,
                    FromSpeaker = true,
                    CreatedOn = DateTime.Now,
                    EventId = question.EventId,
                    DeletedOn = null,
                    ResponseAlotTime = question.ResponseAlotTime,
                    SurveyOptions = MapperHelper.Map<List<SurveyOption>>(question.SurveyOptions)
                };
                this.SurveyRepository.Insert(Survey);
            }
            var speakerQuestions = await GetSurveyFromSpeaker(0, data.FirstOrDefault().AgendaId);
            return speakerQuestions;
        }
    }
}
