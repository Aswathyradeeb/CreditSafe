using Eventsapp.Services.Services.Interface;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;

namespace eventsapp.WebAPI.Controllers
{
    [RoutePrefix("api/Polling")]
    public class PollingController : ApiController
    {
        private IPollingService pollingService;
        public PollingController(IPollingService pollingService)
        {
            this.pollingService = pollingService;
        }
        [Route("GetQuestions/{type}/{id}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetQuestions(string type, int id)
        {
            try
            {
                var questions = await this.pollingService.GetPollingQuestions(type, id);
                if (questions == null)
                    return Ok("An exception has occured while retrieving questions for " + type + "-" + id + " at" + DateTime.Now.ToShortDateString());
                if (questions.Count == 0)
                    return Ok("No question found for " + type + "-" + id + " at" + DateTime.Now.ToShortDateString());
                else
                    return Ok(questions);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
        [HttpGet]
        [Route("GetGuestQuestion/{agendaId}/{userId}")]
        public async Task<ResponseType<List<GuestQuestion>>> GetGuestQuestion(int agendaId, int userId)
        {
            try
            {
                if (agendaId > 0)
                {

                    var result = await this.pollingService.GetGuestQuestion(agendaId, userId);
                    if (result != null && result.Count > 0)
                        return ResponseType<List<GuestQuestion>>.PerformSuccessed<List<GuestQuestion>>(result);
                    else
                        return ResponseType<List<GuestQuestion>>.PerformError<List<GuestQuestion>>("NoResponseFound", "No Data exist for Agenda: " + agendaId + " and User: " + userId, "");
                }
                else
                {
                    return ResponseType<List<GuestQuestion>>.PerformError<List<GuestQuestion>>("NoInputPassed", "No Data has been passed to get question", "");
                }
            }
            catch (Exception ex)
            {
                return ResponseType<List<GuestQuestion>>.PerformError<List<GuestQuestion>>("Exception", "Exception has occurred in Get Guest Question Request : " + ex.Message, "");
            }

        }
        [HttpPost]
        [Route("UserPollQuestion")]
        public async Task<ResponseType<bool>> UserPollQuestion(AttendeeQuestionDto userQuestion)
        {
            try
            {
                if (userQuestion == null)
                    return ResponseType<bool>.PerformError<bool>("NoInputPassed", "No Data has been passed to poll question", "");
                if (userQuestion.QuestionEn == null || userQuestion.QuestionAr == null || userQuestion.UserId == null)
                    return ResponseType<bool>.PerformError<bool>("ParameterEmpty", "Question Name and User Id cannot be empty, please pass the parameter", "");
                var returnValue = await this.pollingService.SubmitPoll(userQuestion);
                if (returnValue == false)
                    return ResponseType<bool>.PerformError<bool>("NotSubmitted", "Some issue occurred cannot submit question to Poll");
                else
                    return ResponseType<bool>.PerformSuccessed<bool>(returnValue);
            }
            catch (Exception ex)
            {
                return ResponseType<bool>.PerformError<bool>("Exception", "Exception has occurred in UserPollQuestion Request : " + ex.Message, "");
            }
        }
        [HttpPost]
        [Route("SubmitPollResponse/{questionId}/{userId}")]
        public async Task<ResponseType<Dictionary<int, List<AttendeeQuestionDto>>>> SubmitPollResponse(int questionId, int userId)
        {
            try
            {
                if (questionId == 0)
                    return ResponseType<Dictionary<int, List<AttendeeQuestionDto>>>.PerformError<Dictionary<int, List<AttendeeQuestionDto>>>("questionIdNotExist", "Question Id cannot be zero", "");
                if (userId == 0)
                    return ResponseType<Dictionary<int, List<AttendeeQuestionDto>>>.PerformError<Dictionary<int, List<AttendeeQuestionDto>>>("userIdNotExist", "User Id cannot be zero", "");

                var returnValue = await this.pollingService.SubmitPollResponse(questionId, userId);
                var result = await this.pollingService.GetPollingQuestions("user",
                    userId);
                return ResponseType<Dictionary<int, List<AttendeeQuestionDto>>>.PerformSuccessed<Dictionary<int, List<AttendeeQuestionDto>>>(result);
            }
            catch (Exception ex)
            {
                return ResponseType<Dictionary<int, List<AttendeeQuestionDto>>>.PerformError<Dictionary<int, List<AttendeeQuestionDto>>>("Exception", "Exception has occurred in UserPollQuestion Request : " + ex.Message, "");
            }
        }
        [HttpPost]
        [Route("SubmitSurvey")]
        public async Task<ResponseType<string>> SubmitSurvey([FromBody] List<SurveyResponse> responses)
        {
            try
            {
                if (responses != null && responses.Count > 0)
                {

                    var result = this.pollingService.SubmitSurvey(ConfigurationManager.AppSettings["connectionstring"].ToString(), responses);
                    if (result.Key)
                        return ResponseType<string>.PerformSuccessed<string>(result.Value);
                    else
                        return ResponseType<string>.PerformError<string>("Error", result.Value, "");
                }
                else
                {
                    return ResponseType<string>.PerformError<string>("Error", "Cannot submit blank responses please provide valid data : " + DateTime.Now.ToString(), "");
                }
            }
            catch (Exception ex)
            {
                return ResponseType<string>.PerformError<string>("Error", "Exception occured " + DateTime.Now.ToString() + ": " + ex.Message, "");
            }

        }
    }
}
