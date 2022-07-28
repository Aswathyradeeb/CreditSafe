using Eventsapp.Repositories;
using Eventsapp.Services.Services.Interface;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Eventsapp.Services.Services.Implementation
{
    public class PollingService : IPollingService
    {
        private readonly IKeyedRepository<AttendeeQuestion, int> qstnRepository;
        private readonly IKeyedRepository<Survey, int> surveyRepo;
        private readonly IEventRepository eventRepository;

        public PollingService(IEventRepository eventRepository, IKeyedRepository<AttendeeQuestion, int> qstnRepository, ICurrentUser user, IKeyedRepository<Survey, int> surveyRepo)
        {
            this.eventRepository = eventRepository;
            this.qstnRepository = qstnRepository;
            this.surveyRepo = surveyRepo;
        }
        public async Task<Dictionary<int, List<AttendeeQuestionDto>>> GetPollingQuestions(string type, int id)
        {
            try
            {
                var events = await this.eventRepository.GetAllAsync();
                Dictionary<int, List<AttendeeQuestionDto>> allQuestions = new Dictionary<int, List<AttendeeQuestionDto>>();
                if (type.ToLower() == "speaker")
                {
                    var speakerEvents = events.Where(u => u.EventPersons != null && u.EventPersons.Count > 0).Select(x => x.EventPersons).FirstOrDefault();
                    var mySpeakerEvent = speakerEvents != null && speakerEvents.Count > 0 ? speakerEvents.Where(e => e.PersonId == id) : null;
                    if (mySpeakerEvent != null)
                    {
                        foreach (var item in mySpeakerEvent)
                        {
                            var questions = await qstnRepository.QueryAsync(x => x.SpeakerId == id && x.EventId == item.EventId);
                            var mappedQuestion = MapperHelper.Map<List<AttendeeQuestionDto>>(questions);
                            if (allQuestions.ContainsKey(item.EventId))
                                allQuestions[item.EventId] = mappedQuestion;
                            else
                                allQuestions.Add(item.EventId, mappedQuestion);
                        }
                    }
                }
                else
                {
                    var userEvents = events.Where(u => u.EventUsers != null && u.EventUsers.Count > 0).Select(x => x.EventUsers).FirstOrDefault();
                    var myUserEvent = userEvents != null && userEvents.Count > 0 ? userEvents.Where(e => e.UserId == id) : null;
                    if (myUserEvent != null)
                    {
                        foreach (var item in myUserEvent)
                        {
                            var questions = await qstnRepository.QueryAsync(x => x.SpeakerId == id && x.EventId == item.EventId);
                            var mappedQuestion = MapperHelper.Map<List<AttendeeQuestionDto>>(questions);
                            if (allQuestions.ContainsKey(item.EventId))
                                allQuestions[item.EventId] = mappedQuestion;
                            else
                                allQuestions.Add(item.EventId, mappedQuestion);
                        }
                    }
                }
                return allQuestions;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<List<GuestQuestion>> GetGuestQuestion(int agendaId, int userId)
        {
            List<GuestQuestion> questionData = new List<GuestQuestion>();

            var questions = await qstnRepository.QueryAsync(x => (userId == 0 || x.UserId.Value == userId) && x.AgendaId.Value == agendaId);
            if (questions != null && questions.Count > 0)
            {
                foreach (var item in questions)
                    questionData.Add(new GuestQuestion()
                    {
                        Id = item.Id,
                        QuestionAr = item.QuestionAr,
                        QuestionEn = item.QuestionEn,
                        AgendaId = item.AgendaId,
                        EventId = item.EventId,
                        SpeakerId = item.SpeakerId,
                        UserId = item.UserId,
                        Answered = item.Answered,
                        CreatedOn = item.CreatedOn
                    });
            }
            return questionData;
        }
        public async Task<bool> SubmitPoll(AttendeeQuestionDto myQuestions)
        {
            try
            {
                myQuestions.CreatedOn = DateTime.Now;
                var mappedItem = MapperHelper.Map<AttendeeQuestion>(myQuestions);
                qstnRepository.Insert(mappedItem);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<List<AttendeeQuestionDto>> SubmitPollResponse(int questionId, int userId)
        {
            var userQuestion = await qstnRepository.QueryAsync(x => x.UserId.Value == userId && x.Id == questionId);
            if (userQuestion.Count > 0)
            {
                userQuestion.FirstOrDefault().Answered = true;
                qstnRepository.Update(userQuestion.FirstOrDefault());
                return new List<AttendeeQuestionDto>();
            }
            else
                return null;
        }
        public KeyValuePair<bool, string> SubmitSurvey(string connectionString, List<SurveyResponse> response)
        {
            DateTime? surveyTime = null;
            List<SurveyResultDataTable> recipientDT = new List<SurveyResultDataTable>();
            SurveyResultDataTable rcpt;
            bool speakerQuesResponse = false;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                foreach (var item in response)
                {
                    rcpt = new SurveyResultDataTable();
                    speakerQuesResponse = item.AgendaId.HasValue ? (item.AgendaId.Value > 0 ? true : false) : false;
                    var survey = this.surveyRepo.Query(x => x.Id == item.SurveyId);
                    surveyTime = survey.Count() > 0 ? (survey.FirstOrDefault().ResponseAlotTime.HasValue ? survey.FirstOrDefault().CreatedOn.Value.AddMinutes(survey.FirstOrDefault().ResponseAlotTime.Value) : DateTime.Now) : new Nullable<DateTime>();

                    rcpt.AgendaId = item.AgendaId.HasValue ? item.AgendaId.Value : new Nullable<int>();
                    rcpt.SurveyId = item.SurveyId;
                    rcpt.SurveyOptionId = item.SurveyOptionId;
                    rcpt.EventId = item.EventId;
                    rcpt.UserId = item.UserId;
                    rcpt.CreatedOn = DateTime.Now;
                    if (surveyTime.HasValue)
                    {
                        if (DateTime.Compare(surveyTime.Value, DateTime.Now) <= 0)
                            recipientDT.Add(rcpt);
                    }
                    else
                        recipientDT.Add(rcpt);
                }
                if (!surveyTime.HasValue || DateTime.Compare(surveyTime.Value, DateTime.Now) <= 0)
                {
                    DataTable tvp = ToDataTable<SurveyResultDataTable>(recipientDT);
                    using (conn)
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("[dbo].[InsertBulkResponses]", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvparam = cmd.Parameters.AddWithValue("@List", tvp);
                        // these next lines are important to map the C# DataTable object to the correct SQL User Defined Type
                        tvparam.SqlDbType = SqlDbType.Structured;
                        tvparam.TypeName = "[Event].[SurveyResultType]";
                        // execute query, consume results, etc. here
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    return new KeyValuePair<bool, string>(true, "data submitted successfully");
                }
                else
                {
                    return new KeyValuePair<bool, string>(false, "The time to submit speaker question response is now over, please try next time");
                }
            }
            catch (Exception ex)
            {
                return new KeyValuePair<bool, string>(false, ex.Message);
            }
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
