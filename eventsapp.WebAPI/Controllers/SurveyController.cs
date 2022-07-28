using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EventsApp.Domain.Entities;
using Eventsapp.Repositories;
using System.Threading.Tasks;
using EventsApp.Domain.DTOs;
using Microsoft.AspNet.Identity;
using Eventsapp.Services;
using EventsApp.Domain.DTOs.Core;

namespace eventsapp.WebAPI.Controllers
{
    [RoutePrefix("api/Survey")]
    public class SurveysController : ApiController
    {
        private readonly ISurveyService SurveyService;
        private readonly ISurveyResultService resultService;
        private ICurrentUser user;



        public SurveysController(ISurveyResultService resultService,ISurveyRepository SurveyRepo, IUserRepository userRepo , ICurrentUser user, ISurveyService Surveyservice)
        {
            this.SurveyService = Surveyservice;
            this.resultService = resultService;
            this.user = user;
        }
        [Route("GetSurvey")]
        public IHttpActionResult GetSurvey(int eventId)
        {
            try
            {

                var pollsDtos = this.SurveyService.GetSurvey(eventId);
                return Ok(pollsDtos);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

        }

        // GET: api/Surveys
        [Route("GetSurveys")]
        public async Task<IHttpActionResult> GetSurveys()
        {
            try
            {
                var returnValue = await this.SurveyService.GetSurveys(this.user.UserInfo.Id);
                return Ok(returnValue);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

        }

        // POST: api/Surveys
        [Route("CreateSurvey")]
        public IHttpActionResult CreateSurvey(SurveyDto SurveyDto)
        {
            try
            {
                SurveyDto.CreatedOn = System.DateTime.Now;
                SurveyService.CreateSurvey(SurveyDto);
                return Ok("True");
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [Route("UpdateSurvey")]
        public IHttpActionResult UpdateSurvey(SurveyDto SurveyDto)
        {
            try
            {
                Survey questEntity = MapperHelper.Map<Survey>(SurveyDto);
                this.SurveyService.UpdateSurvey(SurveyDto);
                return Ok("True");
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [Route("CreateSurveyResult")]
        public IHttpActionResult CreateSurveyResult([FromBody]int SurveyOptionId)
        {
            try
            { 
                SurveyResult SurveyResult = new SurveyResult() { SurveyOptionId = SurveyOptionId , UserId = this.user.UserInfo.Id };
                this.resultService.Vote(SurveyResult);
                return Json(new { result = "True" });
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [Route("GetSurveyResult")]
        public  IHttpActionResult GetSurveyResult(int SurveyId)
        {
            try
            {
                try
                { 

                    var surveyResults = SurveyService.GetSurveyResult(SurveyId, this.user.UserInfo.Id);

                    if (surveyResults != null)
                    {
                        var surveyResultsDto = MapperHelper.Map<SurveyResultDto>(surveyResults);
                        return Json(new { resultOptionId = surveyResultsDto.SurveyOptionId });
                    }
                    else
                    {
                        return Json(new { data = "no survey for this user" });
                    }
                }

                catch (Exception ex)
                {
                    return InternalServerError();
                }
            }

            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [Route("DeleteSurvey")]
        [HttpGet]
        public async Task<IHttpActionResult> DeleteSurvey(int id)
        {
            try
            {
                var data = await this.SurveyService.DeleteSurvey(id);
                return Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        //Get Survey added by speaker 
        [Route("GetAgendaSurveys/{eventId}/{agendaId}")]
        public async  Task<ResponseType<List<SpeakerQuestions>>> GetAgendaSurveys(int eventId, int agendaId)
        {
            try
            {
                if (eventId == 0 && agendaId == 0)
                    return ResponseType<List<SpeakerQuestions>>.PerformError<List<SpeakerQuestions>>("InputNotPassed","No Input is passed to process the request","");
                var returnValue = await this.SurveyService.GetSurveyFromSpeaker(eventId,agendaId);
                if(returnValue.Count>0)
                    return ResponseType<List<SpeakerQuestions>>.PerformSuccessed<List<SpeakerQuestions>>(returnValue);
                else
                    return ResponseType<List<SpeakerQuestions>>.PerformError<List<SpeakerQuestions>>("NoDataFound", "No Response exists for Event id: "+ eventId +" and Agenda Id: "+ agendaId, "");

            }
            catch (Exception ex)
            {
                return ResponseType<List<SpeakerQuestions>>.PerformError<List<SpeakerQuestions>>("Exception", "Exception has occured while fetching survey for Event Id : " + eventId + " and Agenda Id: " + agendaId + " Exception is : "+ ex.Message, "");
            }  
        }
        ////Get Survey added by speaker 
        [Route("AddSpeakerQuestion")]
        public async Task<ResponseType<List<SpeakerQuestions>>> AddSpeakerQuestion(List<AddSpeakerQuestionDTO> speakerQuestions)
        {
            try
            {
                if (speakerQuestions.Count== 0 || speakerQuestions==null)
                    return ResponseType<List<SpeakerQuestions>>.PerformError<List<SpeakerQuestions>>("InputNotPassed", "No Input is passed to process the request", "");
                var returnValue = await this.SurveyService.AddSpeakerQuestion(speakerQuestions);
                if (returnValue.Count > 0)
                    return ResponseType<List<SpeakerQuestions>>.PerformSuccessed<List<SpeakerQuestions>>(returnValue);
                else
                    return ResponseType<List<SpeakerQuestions>>.PerformError<List<SpeakerQuestions>>("NoDataFound", "No Data exists for Agenda Id: " + speakerQuestions.FirstOrDefault().AgendaId, "");
            }
            catch (Exception ex)
            {
                return ResponseType<List<SpeakerQuestions>>.PerformError<List<SpeakerQuestions>>("Exception", "Exception has occured while fetching survey for and Agenda Id: " + speakerQuestions.FirstOrDefault().AgendaId + " Exception is : " + ex.Message, "");
            }
        }


    }
}