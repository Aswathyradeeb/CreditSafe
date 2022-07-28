using Eventsapp.Repositories;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.ReturnObjects;
using EventsApp.Domain.Entities;
using EventsApp.Domain.Enums;
using EventsApp.Framework;
using EventsApp.Framework.EmailsSender;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Eventsapp.Services
{
    public class SpeakerService : ISpeakerService
    {
        private readonly ISpeakerRepository speakerRepository;
        private readonly IEventPersonRepository eventSpeakerRepository;
        private readonly IKeyedRepository<User, int> _userRepository;
        private readonly IKeyedRepository<SpeakerRating, int> speakerRatingRepository;
        private readonly ICurrentUser user;
        private static readonly IMailSender _mailSender = IoC.Instance.Resolve<IMailSender>();


        public SpeakerService(ISpeakerRepository _speakerRepository, IEventPersonRepository _eventPersonRepository,
            IKeyedRepository<User, int> _userRepository, ICurrentUser user,
            IKeyedRepository<SpeakerRating, int> speakerRatingRepository)
        {
            this.speakerRepository = _speakerRepository;
            this.eventSpeakerRepository = _eventPersonRepository;
            this._userRepository = _userRepository;
            this.speakerRatingRepository = speakerRatingRepository;
            this.user = user;
        }

        public async Task<PersonDto> CreateSpeaker(PersonDto _speaker, string connString)
        {
            User userProfileObj = null;
            Person speakerMap = MapperHelper.Map<Person>(_speaker);
            speakerMap.Agenda = null;
            speakerMap.AttendeeQuestions = null;
            speakerMap.User = null;
            speakerMap.Users = null;
            this.speakerRepository.Insert(speakerMap);
            this.speakerRepository.Commit();
            int personId = speakerMap.Id;
            if (String.IsNullOrEmpty(_speaker.Email) == false)
            {
                userProfileObj = _userRepository.Query(a => a.Email == _speaker.Email).FirstOrDefault();
                if (userProfileObj != null && userProfileObj.Roles.Count(a => a.Id == (int)RolesEnum.User) > 0 && userProfileObj.PersonId != null)
                {
                    throw new Exception("Person Already Exists With This Email");
                }
                else if (userProfileObj == null)
                {
                    userProfileObj = new User();
                    userProfileObj.FirstName = _speaker.NameEn;
                    userProfileObj.LastName = "";
                    userProfileObj.IsActive = true;
                    userProfileObj.PhoneNumber = _speaker.Phone;
                    userProfileObj.Email = _speaker.Email;
                    userProfileObj.CreatedOn = DateTime.Now;
                    userProfileObj.EmailConfirmed = false;
                    userProfileObj.PhoneNumberConfirmed = false;
                    userProfileObj.CompanyId = null;
                    userProfileObj.PersonId = null;
                    userProfileObj.EventId = null;
                    userProfileObj.LockoutEnabled = false;
                    userProfileObj.LockoutEndDateUtc = null;
                    userProfileObj.RegistrationTypeId = Convert.ToInt32(RegistrationTypeEnum.Athlete);
                    userProfileObj.PasswordHash = null;
                    userProfileObj.SecurityStamp = new Guid().ToString();
                    userProfileObj.TwoFactorEnabled = false;
                    userProfileObj.PersonId = personId;
                    userProfileObj.UserName = _speaker.Email;
                    userProfileObj.UserCompanies = new List<UserCompany>();
                    userProfileObj.UserCompanies.Add(new UserCompany
                    {
                        CompanyId = this.user.UserInfo.CompanyId
                    });
                    this._userRepository.Insert(userProfileObj);
                    this._userRepository.Commit();
                    userProfileObj = this._userRepository.Query(a => a.Email == _speaker.Email).FirstOrDefault();
                    var url = string.Format(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/#/page/setUserPassword?userId={0}", userProfileObj.Id);
                    string registeremailBody = "<p> <strong>Congrats!</strong> You are successfully registered on Event Management System, please proceed with setting your password by clicking <a href=\"" + url + "\">here</a></p>";
                    await _mailSender.SendEmailAsync("Hifive", "noreply@hifive.ae", userProfileObj.FirstName + " " + userProfileObj.LastName, userProfileObj.Email, "User Registration", registeremailBody);
                    AddUserRole(userProfileObj.Id, connString);
                }
            }
            if (userProfileObj.Roles.Count(a => a.Id == (int)RolesEnum.User || a.Id == (int)RolesEnum.Administrator) > 0 && userProfileObj.PersonId == null)
                userProfileObj.PersonId = personId;
            PersonDto _speakerDto = MapperHelper.Map<PersonDto>(speakerMap);
            return _speakerDto;
        }

        public void AddUserRole(int userId, string connString)
        {
            var userObj = this._userRepository.Query(a => a.Id == userId).FirstOrDefault();
            if (userObj.Roles.Count == 0)
            {
                SqlConnection conn = new SqlConnection(connString);
                using (SqlCommand cmd = new SqlCommand("dbo.AddUserRole", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@RoleId", SqlDbType.NVarChar).Value = Convert.ToInt32(RolesEnum.User);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public async Task<PersonDto> GetSpeakerId(int speakerId)
        {
            var _speaker = (await this.speakerRepository.GetAsync(speakerId));
            var _person = MapperHelper.Map<PersonDto>(_speaker);
            return _person;
        }

        public async Task<ReturnSpeakerDto> GetAllSpeakers(FilterParams filterParams, int page, int pageSize)
        {
            bool isAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.Administrator) > 0;
            bool isSuperAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0;

            int[] eventIds = (filterParams.Events == null || filterParams.Events.Count() == 0) ? new int[0] : filterParams.Events.Select(x => x.Id).ToArray();
            bool emptyempty = (filterParams.Events == null || filterParams.Events.Count() == 0);
            Func<Person, bool> filterExt = a => (emptyempty || eventIds.Contains(a.Id)) && a.DeletedOn == null
            && (isSuperAdmin == true || (isAdmin == true && a.CreatedBy == this.user.UserInfo.Id));
            var speakers = (await this.speakerRepository.GetAllAsync()).Where(filterExt);
            var speakerDtoLst = MapperHelper.Map<List<PersonDto>>(speakers.Skip((page - 1) * pageSize).Take(pageSize).ToList());
            return new ReturnSpeakerDto { speakers = speakerDtoLst, speakersCount = speakers.Count() };

        }

        public async Task<List<PersonDto>> GetSpeakers()
        {
            bool isAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.Administrator) > 0;
            bool isSuperAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0;
            var speakers = await this.speakerRepository.QueryAsync(a => a.DeletedOn == null &&
            (isSuperAdmin == true || (isAdmin == true && a.CreatedBy == this.user.UserInfo.Id)
            || a.EventPersons.FirstOrDefault(b => b.Event.EventUsers.Count(c => c.UserId == this.user.UserInfo.Id) > 0) != null));
            var speakersDtos = MapperHelper.Map<List<PersonDto>>(speakers);
            return speakersDtos;
        }

        public async Task<List<EventPersonDto>> GetSpeakersByEventId(int EventId)
        {
            bool isAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.Administrator) > 0;
            bool isSuperAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0;
            var speakers = await this.eventSpeakerRepository.QueryAsync(x => x.EventId == EventId &&
            (isSuperAdmin == true || (isAdmin == true && x.Event.CreatedBy == this.user.UserInfo.Id)));
            var speakersDtos = MapperHelper.Map<List<EventPersonDto>>(speakers);
            return speakersDtos;
        }

        public async Task<PersonDto> UpdateSpeaker(PersonDto speaker)
        {
            var speakerObj = await this.speakerRepository.GetAsync(speaker.Id);
            speakerObj.TitleAr = speaker.TitleAr;
            speakerObj.TitleEn = speaker.TitleEn;
            speakerObj.NameEn = speaker.NameEn;
            speakerObj.NameAr = speaker.NameAr;
            speakerObj.DescriptionAr = speaker.DescriptionAr;
            speakerObj.DescriptionEn = speaker.DescriptionEn;
            speakerObj.Email = speaker.Email;
            speakerObj.Phone = speaker.Phone;
            speakerObj.Photo = speaker.Photo;
            speakerObj.DateOfBirth = speaker.DateOfBirth;
            speakerObj.Gender = speaker.Gender;
            speakerObj.Resume = speaker.Resume;
            return speaker;
        }

        public async Task<string> DeleteSpeaker(int id)
        {
            var speaker = await this.speakerRepository.GetAsync(id);
            speaker.DeletedOn = DateTime.Now;
            return "Deleted Successfully";
        }

        public async Task<PersonDto> GetSpeakerUser(int userid)
        {
            var speakerObj = speakerRepository.Query(c => c.DeletedOn == null).Where(c => c.Users.Count(a => a.Id == userid) > 0).FirstOrDefault();
            if (speakerObj != null)
            {
                var speakers = await this.speakerRepository.GetAsync(speakerObj.Id);
                var speakersDto = MapperHelper.Map<PersonDto>(speakers);
                return speakersDto;
            }
            return null;

        }

        public async Task<SpeakerRatingDto> SubmitSpeakerRating(SpeakerRatingDto SpeakerRating)
        {
            var SpeakerRatingEntity = MapperHelper.Map<SpeakerRating>(SpeakerRating);
            SpeakerRatingEntity.Event = null;
            SpeakerRatingEntity.Person = null;
            this.speakerRatingRepository.Insert(SpeakerRatingEntity);
            return SpeakerRating;
        }
    }
}
