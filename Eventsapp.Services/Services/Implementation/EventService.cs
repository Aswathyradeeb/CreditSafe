using Eventsapp.Repositories;
using Eventsapp.Services;
using Eventsapp.Services.Push_Notifications;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.ReturnObjects;
using EventsApp.Domain.DTOs.Subscription;
using EventsApp.Domain.Entities;
using EventsApp.Domain.Enums;
using EventsApp.Framework;
using EventsApp.Framework.EmailsSender;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EventsApp.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository eventRepository;
        //private readonly IAttendeeQuestionsRepository qstnRepository;
        private readonly IUserRepository userRepository;
        private readonly IEventPersonRepository EventPersonRepository;
        private readonly IKeyedRepository<Person, int> PersonRepository;
        private readonly IKeyedRepository<AttendeeQuestion, int> qstnRepository;
        private readonly IKeyedRepository<Company, int> CompanyRepository;
        private readonly IEventCompanyRepository EventCompanyRepository;
        private readonly IEventUserRepository eventUserRepository;
        private readonly IConfigurationsRepository configurationsRepository;
        private readonly ICurrentUser user;
        private readonly IKeyedRepository<UserSubscription, short> _userSubscriptionRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMailSender imailSender;
        public EventService(IEventRepository eventRepository,
            IUserRepository userRepository,
            IKeyedRepository<UserSubscription, short> _userSubscriptionRepository,
            IEventUserRepository eventUserRepository,
            IConfigurationsRepository configurationsRepository,
            IEventPersonRepository EventPersonRepository,
            IEventCompanyRepository EventCompanyRepository,
            IKeyedRepository<Person, int> PersonRepository,
            IKeyedRepository<Company, int> CompanyRepository,
             IKeyedRepository<AttendeeQuestion, int> qstnRepository, ICurrentUser user,
             ICompanyRepository _companyRepository,
             IMailSender _iMailSender

            )
        {
            this.eventRepository = eventRepository;
            this.userRepository = userRepository;
            this.eventUserRepository = eventUserRepository;
            this.configurationsRepository = configurationsRepository;
            this.EventPersonRepository = EventPersonRepository;
            this.EventCompanyRepository = EventCompanyRepository;
            this.PersonRepository = PersonRepository;
            this.CompanyRepository = CompanyRepository;
            this.qstnRepository = qstnRepository;
            this.user = user;
            this._companyRepository = _companyRepository;
            this._userSubscriptionRepository = _userSubscriptionRepository;
            this.imailSender = _iMailSender;
        }

        public EventDto CreateEvent(EventSingleDto _event)
        {
            Event eventMap = MapperHelper.Map<Event>(_event);
            eventMap.User = null;
            eventMap.User1 = null;
            eventMap.User2 = null;
            eventMap.Users = null;
            eventMap.EventType = null;
            eventMap.ParticipantsRegistrationType = null;
            eventMap.AttendeeQuestions = null;
            eventMap.CreatedBy = this.user.UserInfo.Id;
            eventMap.UserSubscriptionId = _event.UserSubscriptionId;
            eventMap.CreatedOn = DateTime.Now;
            eventMap.UniqueId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            this.eventRepository.Insert(eventMap);
            EventDto _eventDto = MapperHelper.Map<EventDto>(eventMap);
            return _eventDto;
        }

        public async Task<EventUser> RegisterEventUser(EventRegistrationDto RegistrationModel)
        {
            EventUser NewEventUser = new EventUser();
            NewEventUser.EventId = RegistrationModel.EventId;
            NewEventUser.UserId = RegistrationModel.UserId;
            NewEventUser.IsAttended = false;
            NewEventUser.RegistrationTypeId = RegistrationModel.RegistrationTypeId;
            NewEventUser.RegistrationType = null;
            NewEventUser.User = null;
            NewEventUser.Event = null;

            if (RegistrationModel.RegistrationTypeId == (int)RegistrationTypeEnum.Athlete ||
                RegistrationModel.RegistrationTypeId == (int)RegistrationTypeEnum.Athlete ||
                RegistrationModel.RegistrationTypeId == (int)RegistrationTypeEnum.Athlete)
            {
                var PersonProfile = (await this.PersonRepository.QueryAsync(x => x.Id == RegistrationModel.Person.Id && x.Email == RegistrationModel.Person.Email)).FirstOrDefault();

                if (RegistrationModel.Person != null)
                {
                    if (PersonProfile == null)
                    {
                        EventPerson person = new EventPerson();
                        person.Person = MapperHelper.Map<Person>(RegistrationModel.Person);
                        person.EventId = RegistrationModel.EventId;
                        person.Event = null;
                        this.EventPersonRepository.Insert(person);
                    }
                    else
                    {
                        PersonProfile.DateOfBirth = RegistrationModel.Person.DateOfBirth;
                        PersonProfile.DescriptionAr = RegistrationModel.Person.DescriptionAr;
                        PersonProfile.DescriptionEn = RegistrationModel.Person.DescriptionEn;
                        PersonProfile.Email = RegistrationModel.Person.Email;
                        PersonProfile.Gender = RegistrationModel.Person.Gender;
                        PersonProfile.NameAr = RegistrationModel.Person.NameAr;
                        PersonProfile.NameEn = RegistrationModel.Person.NameEn;
                        PersonProfile.Phone = RegistrationModel.Person.Phone;
                        PersonProfile.Photo = RegistrationModel.Person.Photo;
                        PersonProfile.Resume = RegistrationModel.Person.Resume;
                        PersonProfile.TitleAr = RegistrationModel.Person.TitleAr;
                        PersonProfile.TitleEn = RegistrationModel.Person.TitleEn;
                    }
                }
            }
            else if (RegistrationModel.RegistrationTypeId == (int)RegistrationTypeEnum.Exhibitor ||
                RegistrationModel.RegistrationTypeId == (int)RegistrationTypeEnum.Sponsor ||
                RegistrationModel.RegistrationTypeId == (int)RegistrationTypeEnum.Partner)
            {
                var CompanyProfile = (await this.CompanyRepository.QueryAsync(x => x.Id == RegistrationModel.Company.Id && x.Email == RegistrationModel.Company.Email)).FirstOrDefault();

                if (RegistrationModel.Company != null)
                {
                    if (CompanyProfile == null)
                    {
                        EventCompany company = new EventCompany();
                        company.Company = MapperHelper.Map<Company>(RegistrationModel.Company);
                        company.EventId = RegistrationModel.EventId;
                        company.Event = null;
                        this.EventCompanyRepository.Insert(company);
                    }
                    else
                    {
                        CompanyProfile.Email = RegistrationModel.Company.Email;
                        CompanyProfile.NameAr = RegistrationModel.Company.NameAr;
                        CompanyProfile.NameEn = RegistrationModel.Company.NameEn;
                        CompanyProfile.Phone = RegistrationModel.Company.Phone;
                        CompanyProfile.Photo = RegistrationModel.Company.PhotoFullPath;
                    }
                }
            }

            this.eventUserRepository.Insert(NewEventUser);
            return NewEventUser;
        }

        public async Task<EventUserDto> IsRegisterEvent(EventRegistrationDto RegistrationModel)
        {
            var _eventUser = (await this.eventUserRepository.QueryAsync(x => x.UserId == RegistrationModel.UserId && x.EventId == RegistrationModel.EventId && x.RegistrationTypeId == RegistrationModel.RegistrationTypeId)).FirstOrDefault();
            if (_eventUser != null)
            {
                var eventUser = MapperHelper.Map<EventUserDto>(_eventUser);
                return eventUser;
            }

            return null;
        }

        public async Task<EventDto> GetEventId(int eventId)
        {
            var _event = (await this.eventRepository.QueryAsync(x => x.Id == eventId && x.DeletedOn == null)).FirstOrDefault();
            if (_event != null)
            {
                var eventDto = MapperHelper.Map<EventDto>(_event);
                eventDto.Agenda = eventDto.Agenda.OrderBy(a => a.StartDate).ThenBy(ag => ag.FromTime).ToList();
                eventDto.Sessions = new List<AgendaSessionDto>();
                // The below logic is to fill agenda without sessions
                if (eventDto.Agenda.Where(a => a.SessionId == null).Count() > 0)
                {
                    eventDto.Agenda.Where(a => a.SessionId == null).ToList().ForEach(x =>
                  {
                      if (eventDto.Sessions.FirstOrDefault(a => a.Date == x.StartDate) == null)
                      {
                          AgendaSessionDto sessionObj = new AgendaSessionDto
                          {
                              Date = x.StartDate,
                              OrderNumber = eventDto.Sessions.Count + 1,
                              Id = eventDto.Sessions.Count + 1,
                              EventId = eventDto.Id,
                              Agenda = new List<AgendumDto>()
                          };
                          eventDto.Sessions.Add(sessionObj);
                      }
                      var session = eventDto.Sessions.FirstOrDefault(a => a.Date == x.StartDate);
                      session.Agenda.Add(MapperHelper.Map<AgendumDto>(x));
                  });
                }
                // The below logic is to fill agenda with Different Sessions
                if (eventDto.AgendaSessions.Count > 0)
                {
                    eventDto.AgendaSessions.ToList().ForEach(x =>
                    {
                        if (eventDto.Sessions.FirstOrDefault(a => a.Id == x.Id) == null)
                        {
                            AgendaSessionDto sessionObj = new AgendaSessionDto
                            {
                                Date = x.Date,
                                OrderNumber = x.OrderNumber,
                                NameEn = x.NameEn,
                                NameAr = x.NameAr,
                                Id = x.Id,
                                EventId = eventDto.Id,
                                Agenda = new List<AgendumDto>()
                            };
                            eventDto.Sessions.Add(sessionObj);
                        }

                        var session = eventDto.Sessions.FirstOrDefault(a => a.Date == x.Date);
                        x.Agenda.ToList().ForEach(y =>
                        {
                            session.Agenda.Add(MapperHelper.Map<AgendumDto>(y));
                        });
                    });
                }
                eventDto.speakersCount = eventDto.EventPersons.Count(a => a.PersonTypeId == (int)PersonTypeEnum.Speaker);
                eventDto.vipCount = eventDto.EventPersons.Count(a => a.PersonTypeId == (int)PersonTypeEnum.VIP);
                eventDto.sponsorsCount = eventDto.EventCompanies.Count(a => a.CompanyTypeId == (int)CompanyTypeEnum.Sponser); ;
                eventDto.exhibitorsCount = eventDto.EventCompanies.Count(a => a.CompanyTypeId == (int)CompanyTypeEnum.Exhibitor);
                eventDto.partnersCount = eventDto.EventCompanies.Count(a => a.CompanyTypeId == (int)CompanyTypeEnum.Partner);
                eventDto.registeredUsersCount = _event.EventUsers.Count;
                eventDto.sessionCount = eventDto.Agenda.Count;
                eventDto.Surveys = eventDto.Surveys.Where(x => x.DeletedOn == null).ToList();


                return eventDto;
            }
            return null;
        }

        public async Task<EventRegitrationDto> GetEventRegitration(int eventId)
        {
            var _event = (await this.eventRepository.QueryAsync(x => x.Id == eventId && x.DeletedOn == null)).FirstOrDefault();
            if (_event != null)
            {
                var eventDto = MapperHelper.Map<EventRegitrationDto>(_event);
                foreach (var agendaSessionItem in eventDto.AgendaSessions)
                {
                    agendaSessionItem.Date = agendaSessionItem.Agenda.FirstOrDefault().StartDate;

                    foreach (var agendaItem in agendaSessionItem.Agenda)
                    {
                        agendaItem.FromTime24Hr = DateTime.Parse(agendaItem.FromTime).TimeOfDay;
                    }
                }
                // eventDto.Agenda = eventDto.Agenda.OrderBy(a => a.StartDate).ThenBy(ag => ag.FromTime).ToList();
                //eventDto.Sessions = new List<AgendaSessionDto>();
                // The below logic is to fill agenda without sessions
                //if (eventDto.Agenda.Where(a => a.SessionId == null).Count() > 0)
                //{
                //    eventDto.Agenda.Where(a => a.SessionId == null).ToList().ForEach(x =>
                //    {
                //        if (eventDto.Sessions.FirstOrDefault(a => a.Date == x.StartDate) == null)
                //        {
                //            AgendaSessionDto sessionObj = new AgendaSessionDto
                //            {
                //                Date = x.StartDate,
                //                OrderNumber = eventDto.Sessions.Count + 1,
                //                Id = eventDto.Sessions.Count + 1,
                //                EventId = eventDto.Id,
                //                Agenda = new List<AgendumDto>()
                //            };
                //            eventDto.Sessions.Add(sessionObj);
                //        }
                //        var session = eventDto.Sessions.FirstOrDefault(a => a.Date == x.StartDate);
                //        session.Agenda.Add(MapperHelper.Map<AgendumDto>(x));
                //    });
                //}
                // The below logic is to fill agenda with Different Sessions
                //if (eventDto.AgendaSessions.Count > 0)
                //{
                //    eventDto.AgendaSessions.ToList().ForEach(x =>
                //    {
                //        if (eventDto.Sessions.FirstOrDefault(a => a.Id == x.Id) == null)
                //        {
                //            AgendaSessionDto sessionObj = new AgendaSessionDto
                //            {
                //                Date = x.Date,
                //                OrderNumber = x.OrderNumber,
                //                NameEn = x.NameEn,
                //                NameAr = x.NameAr,
                //                Id = x.Id,
                //                EventId = eventDto.Id,
                //                Agenda = new List<AgendumDto>()
                //            };
                //            eventDto.Sessions.Add(sessionObj);
                //        }

                //        var session = eventDto.Sessions.FirstOrDefault(a => a.Date == x.Date);
                //        x.Agenda.ToList().ForEach(y =>
                //        {
                //            session.Agenda.Add(MapperHelper.Map<AgendumDto>(y));
                //        });
                //    });
                //}
                //eventDto.speakersCount = eventDto.EventPersons.Count(a => a.PersonTypeId == (int)PersonTypeEnum.Speaker);
                //eventDto.vipCount = eventDto.EventPersons.Count(a => a.PersonTypeId == (int)PersonTypeEnum.VIP);
                //eventDto.sponsorsCount = eventDto.EventCompanies.Count(a => a.CompanyTypeId == (int)CompanyTypeEnum.Sponser); ;
                //eventDto.exhibitorsCount = eventDto.EventCompanies.Count(a => a.CompanyTypeId == (int)CompanyTypeEnum.Exhibitor);
                //eventDto.partnersCount = eventDto.EventCompanies.Count(a => a.CompanyTypeId == (int)CompanyTypeEnum.Partner);
                //eventDto.registeredUsersCount = eventDto.EventUsers.Count;
                //eventDto.sessionCount = eventDto.Agenda.Count;
                //eventDto.Surveys = eventDto.Surveys.Where(x => x.DeletedOn == null).ToList();


                return eventDto;
            }
            return null;
        }

        public async Task<string> getAdminMail()
        {
            var user = await this.userRepository.QueryAsync(x => x.Roles.Any(y => y.Id == 1));
            string email = "";
            foreach (var item in user)
            {
                email = item.Email;
            }

            return email;
        }

        public async Task<List<EventLightDto>> GetEvents()
        {
            bool isAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.Administrator) > 0;
            bool isSuperAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0;
            bool isUser = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.User) > 0;
            var userCompanies = this.user.UserInfo.UserCompanies.ToList();
            Func<Event, bool> filterExt = a => (isUser != true || userCompanies.Count(x => x.CompanyId == a.User.CompanyId) > 0);
            var events = await this.eventRepository.QueryAsync(x => x.DeletedOn == null &&
            (isSuperAdmin || (isAdmin && x.CreatedBy == this.user.UserInfo.Id) ||
            (isUser == true && x.EndDate >= DateTime.Now)));
            events = events.Where(filterExt).OrderBy(x => x.StartDate).ToList();
            var eventsDtos = MapperHelper.Map<List<EventLightDto>>(events);
            return eventsDtos;
        }

        public async Task<ReturnEventsDto> GetEventsByCompanyName(string compCode, FilterParams filterParams, int page, int pageSize)
        {
            try
            {
                List<Company> company = this._companyRepository.Query(x => x.CompanyCode == compCode).ToList();
                int createdUid = 0;
                if (company.Count <= 0)
                {
                    return new ReturnEventsDto { events = null, eventsCount = 0 };
                }
                foreach (var item in company)
                {
                    createdUid = Convert.ToInt32(item.CreatedBy);
                }

                // var compDto = MapperHelper.Map<CompanySingleDto>(company);


                bool isAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.Administrator) > 0;
                bool isSuperAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0;
                bool isUser = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.User) > 0;
                int[] eventIds = (filterParams.Events == null || filterParams.Events.Count() == 0) ? new int[0] : filterParams.Events.Select(x => x.Id).ToArray();
                bool emptyEvent = (filterParams.Events == null || filterParams.Events.Count() == 0);
                var userCompanies = this.user.UserInfo.UserCompanies.ToList();
                Func<Event, bool> filterExt = a => (emptyEvent || eventIds.Contains(a.Id))
                && (isUser != true || userCompanies.Count(x => x.CompanyId == a.User.CompanyId) > 0);
                var events = await this.eventRepository.QueryAsync(x => x.CreatedBy == createdUid && x.DeletedOn == null &&
                (isSuperAdmin == true || (isAdmin == true && x.CreatedBy == this.user.UserInfo.Id) ||
                 (isUser == true && x.EndDate >= DateTime.Now)));
                events = events.Where(filterExt).OrderBy(x => x.StartDate).ToList();
                var eventDtoLst = MapperHelper.Map<List<EventLightDto>>(events.Skip((page - 1) * pageSize)
                       .Take(pageSize).ToList());
                return new ReturnEventsDto { events = eventDtoLst, eventsCount = events.Count };
            }
            catch (Exception)
            {
                return new ReturnEventsDto { events = null, eventsCount = 0 };
            }

        }

        public async Task<ReturnEventsDto> GetEventsByCompanyId(int userId, FilterParams filterParams, int page, int pageSize)
        {
            try
            {
                var user = (await this.userRepository.QueryAsync(x => x.Id == userId)).FirstOrDefault();

                List<Company> company = this._companyRepository.Query(x => x.Id == user.CompanyId).ToList();
                int createdUid = 0;
                if (company.Count <= 0)
                {
                    return new ReturnEventsDto { events = null, eventsCount = 0 };
                }
                foreach (var item in company)
                {
                    createdUid = Convert.ToInt32(item.CreatedBy);
                }

                // var compDto = MapperHelper.Map<CompanySingleDto>(company);


                bool isAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.Administrator) > 0;
                bool isSuperAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0;
                bool isUser = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.User) > 0;
                int[] eventIds = (filterParams.Events == null || filterParams.Events.Count() == 0) ? new int[0] : filterParams.Events.Select(x => x.Id).ToArray();
                bool emptyEvent = (filterParams.Events == null || filterParams.Events.Count() == 0);
                Func<Event, bool> filterExt = a => (emptyEvent || eventIds.Contains(a.Id));
                var events = await this.eventRepository.QueryAsync(x => x.CreatedBy == createdUid && x.DeletedOn == null &&
                (isSuperAdmin == true || (isAdmin == true && x.CreatedBy == this.user.UserInfo.Id) ||
                 (isUser == true && x.EndDate >= DateTime.Now)));
                events = events.Where(filterExt).OrderBy(x => x.StartDate).ToList();
                var eventDtoLst = MapperHelper.Map<List<EventLightDto>>(events.Skip((page - 1) * pageSize)
                       .Take(pageSize).ToList());
                return new ReturnEventsDto { events = eventDtoLst, eventsCount = events.Count };
            }
            catch (Exception)
            {
                return new ReturnEventsDto { events = null, eventsCount = 0 };
            }

        }

        public async Task<ReturnEventsDto> GetAllEvents(FilterParams filterParams, int page, int pageSize)
        {
            List<Event> myEvents = new List<Event>();
            //var IsEmailSent = await imailSender.SendEmailAsync("Hifive", "noreply@hifive.ae", "Abbas Majeed", "abbas@hifive.ae", "Email Testing", "<div><p>Dear <strong>" + "Abbas" + "</strong> </p><br><p>Your registeration has been approved for Event: " + "Athelete Event" + "</p></div>");
            bool isAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.Administrator) > 0;
            bool isSuperAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0;
            bool isUser = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.User) > 0;
            int[] eventIds = (filterParams.Events == null || filterParams.Events.Count() == 0) ? new int[0] : filterParams.Events.Select(x => x.Id).ToArray();
            bool emptyEvent = (filterParams.Events == null || filterParams.Events.Count() == 0);
            var userCompanies = this.user.UserInfo.UserCompanies.ToList();
            Func<Event, bool> filterExt = a => (emptyEvent || eventIds.Contains(a.Id))
            && (isUser != true || userCompanies == null || userCompanies.Count(x => a.User != null && x.CompanyId == a.User.CompanyId) > 0);
            if (filterParams.UserRole != null)
            {
                switch (filterParams.UserRole.ToLower())
                {
                    case "speaker":
                        {
                            myEvents = await this.eventRepository.QueryAsync(x => x.DeletedOn == null && (x.EventPersons.Any(p => p.PersonId == this.user.UserInfo.PersonId))
                                &&
                                (String.IsNullOrEmpty(filterParams.EventName)
                                || x.NameEn.Contains(filterParams.EventName)
                                || x.NameAr.Contains(filterParams.EventName)));
                            break;
                        }
                    case "vip":
                        {
                            {
                                myEvents = await this.eventRepository.QueryAsync(x => x.DeletedOn == null && (x.EventPersons.Any(p => p.PersonId == this.user.UserInfo.PersonId))
                                    &&
                                    (String.IsNullOrEmpty(filterParams.EventName)
                                    || x.NameEn.Contains(filterParams.EventName)
                                    || x.NameAr.Contains(filterParams.EventName)));
                                break;
                            }
                        }
                    case "participant":
                        {
                            {
                                myEvents = await this.eventRepository.QueryAsync(x => x.DeletedOn == null && (x.EventUsers.Any(p => p.UserId == this.user.UserInfo.Id))
                                    &&
                                    (String.IsNullOrEmpty(filterParams.EventName)
                                    || x.NameEn.Contains(filterParams.EventName)
                                    || x.NameAr.Contains(filterParams.EventName)));
                                break;
                            }
                        }
                    case "creator":
                        {
                            {
                                myEvents = await this.eventRepository.QueryAsync(x => x.DeletedOn == null && (x.CreatedBy == this.user.UserInfo.Id)
                                    &&
                                    (String.IsNullOrEmpty(filterParams.EventName)
                                    || x.NameEn.Contains(filterParams.EventName)
                                    || x.NameAr.Contains(filterParams.EventName)));
                                break;
                            }
                        }
                    case "sponsor":
                        {
                            var type = Convert.ToInt32(CompanyTypeEnum.Sponser);
                            myEvents = await this.eventRepository.QueryAsync(x => x.DeletedOn == null && (x.EventCompanies.Any(c => c.CompanyId == this.user.UserInfo.CompanyId && c.CompanyTypeId.Value == type))
                                 &&
                                 (String.IsNullOrEmpty(filterParams.EventName)
                                 || x.NameEn.Contains(filterParams.EventName)
                                 || x.NameAr.Contains(filterParams.EventName)));
                            break;
                        }
                    case "exhibitor":
                        {
                            var type = Convert.ToInt32(CompanyTypeEnum.Exhibitor);
                            myEvents = await this.eventRepository.QueryAsync(x => x.DeletedOn == null && (x.EventCompanies.Any(c => c.CompanyId == this.user.UserInfo.CompanyId && c.CompanyTypeId.Value == type))
                                 &&
                                 (String.IsNullOrEmpty(filterParams.EventName)
                                 || x.NameEn.Contains(filterParams.EventName)
                                 || x.NameAr.Contains(filterParams.EventName)));
                            break;
                        }
                    case "partner":
                        {
                            var type = Convert.ToInt32(CompanyTypeEnum.Partner);
                            myEvents = await this.eventRepository.QueryAsync(x => x.DeletedOn == null && (x.EventCompanies.Any(c => c.CompanyId == this.user.UserInfo.CompanyId && c.CompanyTypeId.Value == type))
                                 &&
                                 (String.IsNullOrEmpty(filterParams.EventName)
                                 || x.NameEn.Contains(filterParams.EventName)
                                 || x.NameAr.Contains(filterParams.EventName)));
                            break;
                        }
                }
            }
            else
            {
                myEvents = await this.eventRepository.QueryAsync(x => x.DeletedOn == null &&
                (filterParams.CompanyId == 0
                || x.User.Company.Id == filterParams.CompanyId
                || x.User.Company.Id == filterParams.CompanyId) &&
                (String.IsNullOrEmpty(filterParams.createdBy)
                || x.User.FirstName.Contains(filterParams.createdBy)
                || x.User.LastName.Contains(filterParams.createdBy)
                || x.User.Email.Contains(filterParams.createdBy)) &&
                (String.IsNullOrEmpty(filterParams.EventName)
                || x.NameEn.Contains(filterParams.EventName)
                || x.NameAr.Contains(filterParams.EventName)) &&
                (isSuperAdmin == true || (isAdmin == true && x.CreatedBy == this.user.UserInfo.Id) ||
                 (isUser == true && x.EndDate >= DateTime.Today)));
            }

            myEvents = myEvents.Where(filterExt).OrderByDescending(x => x.StartDate).ToList();
            var eventDtoLst = MapperHelper.Map<List<EventLightDto>>(myEvents.Skip((page - 1) * pageSize)
                   .Take(pageSize).ToList());
            return new ReturnEventsDto { events = eventDtoLst, eventsCount = myEvents.Count };

        }

        public async Task<List<UserDto>> GetRegisterdUsers(int _eventId)
        {
            var users = await this.userRepository.QueryAsync(x => x.EventUsers.Where(y => y.EventId == _eventId).Any());
            List<UserDto> userDto = MapperHelper.Map<List<UserDto>>(users);
            return userDto;
        }

        public async Task<List<EventLightDto>> GetEventsByUserId(int userId)
        {
            var events = await this.eventRepository.QueryAsync(x => x.DeletedOn == null);
            List<EventLightDto> eventsDto = MapperHelper.Map<List<EventLightDto>>(events);
            return eventsDto;
        }

        public string DeleteEvent(int eventId)
        {
            Event eventEntity = this.eventRepository.Get(eventId);
            eventEntity.DeletedBy = this.user.UserInfo.Id;
            eventEntity.DeletedOn = DateTime.Now;
            return "200";
        }

        public async Task<EventDto> UpdateEvent(EventDto _eventDto)
        {
            Event eventEntity = await this.eventRepository.GetAsync(_eventDto.Id);
            Event _event = MapperHelper.Map<Event>(_eventDto);
            eventEntity.ModifiedBy = this.user.UserInfo.Id;
            eventEntity.LastModified = DateTime.Now;
            eventEntity.NameEn = _eventDto.NameEn;
            eventEntity.NameAr = _eventDto.NameAr;
            eventEntity.StartDate = _eventDto.StartDate;
            eventEntity.EndDate = _eventDto.EndDate;
            eventEntity.DescriptionEn = _eventDto.DescriptionEn;
            eventEntity.DescriptionAr = _eventDto.DescriptionAr;
            eventEntity.BannerPhoto = _eventDto.BannerPhoto;
            eventEntity.BadgePhoto = _eventDto.BadgePhoto;
            eventEntity.EventTypeId = _eventDto.EventTypeId;
            eventEntity.HasExhibitors = _eventDto.HasExhibitors;
            eventEntity.HasPartners = _eventDto.HasPartners;
            eventEntity.HasSpeaker = _eventDto.HasSpeaker;
            eventEntity.HasVIP = _eventDto.HasVIP;
            eventEntity.HasSponsors = _eventDto.HasSponsors;
            eventEntity.SponsorsOnlineRegister = _eventDto.SponsorsOnlineRegister;
            eventEntity.ExhibitorsOnlineRegister = _eventDto.ExhibitorsOnlineRegister;
            eventEntity.PartnersOnlineRegister = _eventDto.PartnersOnlineRegister;
            eventEntity.SpeakerOnlineRegister = _eventDto.SpeakerOnlineRegister;
            eventEntity.VIPOnlineRegister = _eventDto.VIPOnlineRegister;
            eventEntity.FacebookURL = _eventDto.FacebookURL;
            eventEntity.InstagramURL = _eventDto.InstagramURL;
            eventEntity.LinkedInURL = _eventDto.LinkedInURL;
            eventEntity.TwitterURL = _eventDto.TwitterURL;
            eventEntity.VideoURL = _eventDto.VideoURL;
            eventEntity.Video = _eventDto.Video;
            eventEntity.EventLogo = _eventDto.EventLogo;
            eventEntity.ThemeURL = _eventDto.ThemeURL;
            eventEntity.ParticipantFees = _eventDto.ParticipantFees;
            eventEntity.BgImageId = _eventDto.BgImageId;
            eventEntity.UserSubscriptionId = _eventDto.UserSubscriptionId;
            eventEntity.SelectedCurrency = _eventDto.SelectedCurrency;
            eventEntity.ParticipantsRegistrationTypeId = _eventDto.ParticipantsRegistrationTypeId;


            eventEntity.HasPayment = _eventDto.HasPayment;
            if (eventEntity.UniqueId == null)
            {
                eventEntity.UniqueId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            }
            //Approve user if participant limit has been increased
            if ((eventEntity.ParticipantsLimit != _eventDto.ParticipantsLimit) && (eventEntity.EventUsers.Count < _eventDto.ParticipantsLimit))
            {
                var users = eventEntity.EventUsers.Where(x => x.IsRegistered.HasValue && !x.IsRegistered.Value).ToList();
                if (users.Count > 0)
                {
                    foreach (var guest in users)
                    {
                        eventEntity.EventUsers.Where(x => x.Id == guest.Id).FirstOrDefault().IsRegistered = true;
                        imailSender.SendEmailAsync("Hifive", "noreply@hifive.ae", guest.User.FirstName, guest.User.Email, "Event Registration Approved", "<div><p>Dear <strong>" + guest.User.FirstName + "</strong> </p><br><p>Your registeration has been approved for Event: " + _eventDto.NameEn + "</p></div>");
                    }
                }
            }
            eventEntity.ParticipantsLimit = _eventDto.ParticipantsLimit;
            List<EventAddress> deletedEventAddresses = eventEntity.EventAddresses.Where(x => _event.EventAddresses.Where(y => y.Id == x.Id).FirstOrDefault() == null).ToList();
            foreach (EventAddress item in deletedEventAddresses)
            {
                eventEntity.EventAddresses.Remove(item);
            }

            List<EventAddress> updatedEventAddresses = eventEntity.EventAddresses.Where(x => _event.EventAddresses.Where(y => y.Id == x.Id).FirstOrDefault() != null).ToList();
            foreach (EventAddress item in updatedEventAddresses)
            {
                EventAddress updatedEvent = _event.EventAddresses.Where(y => y.Id == item.Id).FirstOrDefault();
                item.Address.CountryId = updatedEvent.Address.Country != null ? updatedEvent.Address.Country.Id : updatedEvent.Address.CountryId;
                item.Address.StateId = updatedEvent.Address.State != null ? updatedEvent.Address.State.Id : updatedEvent.Address.StateId;
                item.Address.Street = updatedEvent.Address.Street;
                item.Address.Lat = updatedEvent.Address.Lat;
                item.Address.Lng = updatedEvent.Address.Lng;
                item.Address.LocationPhoto = updatedEvent.Address.LocationPhoto;
            }

            List<EventAddress> addedEventAddress = _event.EventAddresses.Where(x => eventEntity.EventAddresses.Where(y => y.Id == x.Id).FirstOrDefault() == null).ToList();
            foreach (EventAddress item in addedEventAddress)
            {
                item.Address.CountryId = item.Address.Country != null ? item.Address.Country.Id : item.Address.CountryId;
                item.Address.StateId = item.Address.State != null ? item.Address.State.Id : item.Address.StateId;
                item.Address.Country = null;
                item.Address.State = null;
                eventEntity.EventAddresses.Add(item);
            }

            List<AgendaSession> deletedSession = eventEntity.AgendaSessions.Where(x => _event.AgendaSessions.Where(y => y.Id == x.Id).FirstOrDefault() == null).ToList();
            foreach (AgendaSession item in deletedSession)
            {
                eventEntity.AgendaSessions.Remove(item);
            }

            List<AgendaSession> updatedSession = eventEntity.AgendaSessions.Where(x => _event.AgendaSessions.Where(y => y.Id == x.Id).FirstOrDefault() != null).ToList();
            foreach (AgendaSession item in updatedSession)
            {
                AgendaSession updatedEvent = _event.AgendaSessions.Where(y => y.Id == item.Id).FirstOrDefault();
                item.Date = updatedEvent.Date;
                item.NameEn = updatedEvent.NameEn;
                item.NameAr = updatedEvent.NameAr;
            }

            List<AgendaSession> addedSession = _event.AgendaSessions.Where(x => eventEntity.AgendaSessions.Where(y => y.Id == x.Id).FirstOrDefault() == null).ToList();
            foreach (AgendaSession item in addedSession)
            {
                item.Agenda = null;
                item.Event = null;
                eventEntity.AgendaSessions.Add(item);
            }


            List<Agendum> deletedAgenda = eventEntity.Agenda.Where(x => _event.Agenda.Where(y => y.Id == x.Id).FirstOrDefault() == null).ToList();
            foreach (Agendum item in deletedAgenda)
            {
                eventEntity.Agenda.Remove(item);
            }

            List<Agendum> updatedAgenda = eventEntity.Agenda.Where(x => _event.Agenda.Where(y => y.Id == x.Id).FirstOrDefault() != null).ToList();
            foreach (Agendum item in updatedAgenda)
            {
                Agendum updatedEvent = _event.Agenda.Where(y => y.Id == item.Id).FirstOrDefault();
                item.DescriptionEn = updatedEvent.DescriptionEn;
                item.DescriptionAr = updatedEvent.DescriptionAr;
                item.TitleEn = updatedEvent.TitleEn;
                item.TitleAr = updatedEvent.TitleAr;
                item.SpeakerId = updatedEvent.SpeakerId;
                item.ParticipantsLimit = updatedEvent.ParticipantsLimit;
                item.ReservationCount = updatedEvent.ReservationCount == null ? 0 : item.ReservationCount;
                item.FromTime = updatedEvent.FromTime;
                item.StartDate = updatedEvent.StartDate;
                item.EndDate = updatedEvent.EndDate;
                item.ToTime = updatedEvent.ToTime;
                item.Location = updatedEvent.Location;
                item.SpeakerId = updatedEvent.Person != null ? updatedEvent.Person.Id : updatedEvent.SpeakerId;
                item.SessionId = updatedEvent.AgendaSession != null ? updatedEvent.AgendaSession.Id : updatedEvent.SessionId;
            }

            List<Agendum> addedAgend = _event.Agenda.Where(x => eventEntity.Agenda.Where(y => y.Id == x.Id).FirstOrDefault() == null).ToList();
            foreach (Agendum item in addedAgend)
            {
                item.SpeakerId = item.Person != null ? item.Person.Id : item.SpeakerId;
                item.SessionId = item.AgendaSession != null ? item.AgendaSession.Id : item.SessionId;
                item.Person = null;
                item.AgendaSession = null;
                item.ReservationCount = 0;
                eventEntity.Agenda.Add(item);
            }

            List<Package> deletedPackages = eventEntity.Packages.Where(x => _event.Packages.Where(y => y.Id == x.Id).FirstOrDefault() == null).ToList();
            foreach (Package item in deletedPackages)
            {
                eventEntity.Packages.Remove(item);
            }

            List<Package> updatedPackages = eventEntity.Packages.Where(x => _event.Packages.Where(y => y.Id == x.Id).FirstOrDefault() != null).ToList();
            foreach (Package item in updatedPackages)
            {
                Package updatedPackage = _event.Packages.Where(y => y.Id == item.Id).FirstOrDefault();
                item.Benefits = updatedPackage.Benefits;
                item.Cost = updatedPackage.Cost;
                item.EventId = _eventDto.Id;
                item.SponsorTypeId = updatedPackage.SponserType == null ? (int?)null : updatedPackage.SponserType.id;
            }

            List<Package> addedPackages = _event.Packages.Where(x => eventEntity.Packages.Where(y => y.Id == x.Id).FirstOrDefault() == null).ToList();
            foreach (Package item in addedPackages)
            {
                item.EventId = _eventDto.Id;
                item.Event = null;
                item.SponsorTypeId = item.SponserType == null ? (int?)null : item.SponserType.id;
                item.SponserType = null;
                eventEntity.Packages.Add(item);
            }

            List<EventCompany> updatedEventCompanies = eventEntity.EventCompanies.Where(x => _event.EventCompanies.Where(y => y.Id == x.Id).FirstOrDefault() != null).ToList();
            foreach (EventCompany item in updatedEventCompanies)
            {
                EventCompany updatedEventCompany = _event.EventCompanies.Where(y => y.Id == item.Id).FirstOrDefault();
                item.CompanyTypeId = updatedEventCompany.CompanyTypeId;
                item.EventId = _eventDto.Id;
                if (item.IsApproved != updatedEventCompany.IsApproved)
                {
                    item.IsApproved = updatedEventCompany.IsApproved;

                    List<PushNotification> collSendData = new List<PushNotification>();
                    List<Recipient> myContacts = new List<Recipient>();
                    var userObj = this.userRepository.Query(a => a.CompanyId == item.CompanyId).FirstOrDefault();
                    if (userObj != null && userObj.LoggedUserConnections != null)
                    {
                        if (userObj.LoggedUserConnections.Count > 0 && userObj.LoggedUserConnections.FirstOrDefault() != null)
                        {
                            myContacts.Add(new Recipient()
                            {
                                RecipientId = userObj.Id,
                                ReferenceConnectionId = userObj.LoggedUserConnections.FirstOrDefault().Token
                            });
                            if (item.IsApproved == true)
                            {
                                collSendData.Add(new PushNotification()
                                {
                                    Message = "You are approved as " + item.CompanyType.NameEn + " for the Event:" + _eventDto.NameEn,
                                    ReferenceId = item.Id,
                                    CreatedOn = DateTime.Now,
                                    Recipients = myContacts
                                });
                            }
                            else
                            {
                                collSendData.Add(new PushNotification()
                                {
                                    Message = "You are rejected as " + item.CompanyType.NameEn + " for the Event:" + _eventDto.NameEn,
                                    ReferenceId = item.Id,
                                    CreatedOn = DateTime.Now,
                                    Recipients = myContacts
                                });
                            }
                        }
                    }
                    if (collSendData.Count > 0)
                    {
                        foreach (var notification in collSendData)
                        {
                            NotificationConnector.PushNotifications(notification);
                        }
                    }
                }
            }

            List<EventCompany> deletedEventCompanies = eventEntity.EventCompanies.Where(x => _event.EventCompanies.Where(y => y.Id == x.Id).FirstOrDefault() == null).ToList();
            foreach (EventCompany item in deletedEventCompanies)
            {
                eventEntity.EventCompanies.Remove(item);
            }


            List<EventCompany> addedEventCompanies = _event.EventCompanies.Where(x => eventEntity.EventCompanies.Where(y => y.Id == x.Id).FirstOrDefault() == null).ToList();
            foreach (EventCompany item in addedEventCompanies)
            {
                item.CreatedOn = System.DateTime.Now;
                item.IsApproved = true;
                item.EventId = eventEntity.Id;
                item.Event = null;
                item.CompanyId = item.CompanyId;
                item.Company = null;
                eventEntity.EventCompanies.Add(item);
            }

            //List<EventUser> deletedaddedEventsUser = eventEntity.EventUsers.Where(x => _event.EventUsers.Where(y => y.Id == x.Id).FirstOrDefault() == null).ToList();
            //foreach (EventUser item in deletedaddedEventsUser)
            //{
            //    eventEntity.EventUsers.Remove(item);
            //}

            //List<EventUser> addedEventsUser = _event.EventUsers.Where(x => eventEntity.EventUsers.Where(y => y.Id == x.Id).FirstOrDefault() == null).ToList();
            //foreach (EventUser item in addedEventsUser)
            //{
            //    item.UserId = item.User.Id;
            //    item.User = null;
            //    eventEntity.EventUsers.Add(item);
            //}

            List<EventPerson> deletedEventPersons = eventEntity.EventPersons.Where(x => _event.EventPersons.Where(y => y.Id == x.Id).FirstOrDefault() == null).ToList();
            foreach (EventPerson item in deletedEventPersons)
            {
                eventEntity.EventPersons.Remove(item);
            }

            List<EventPerson> updatedEventPersons = eventEntity.EventPersons.Where(x => _event.EventPersons.Where(y => y.Id == x.Id).FirstOrDefault() != null).ToList();
            foreach (EventPerson item in updatedEventPersons)
            {
                EventPerson updatedEventPerson = _event.EventPersons.Where(y => y.Id == item.Id).FirstOrDefault();

                item.PersonTypeId = updatedEventPerson.PersonTypeId;
                item.EventId = _eventDto.Id;

                if (item.IsApproved != updatedEventPerson.IsApproved)
                {
                    item.IsApproved = updatedEventPerson.IsApproved;
                    List<PushNotification> collSendData = new List<PushNotification>();
                    List<Recipient> myContacts = new List<Recipient>();
                    var userObj = this.userRepository.Query(a => a.PersonId == item.PersonId).FirstOrDefault();
                    if (userObj != null && userObj.LoggedUserConnections != null)
                    {
                        if (userObj.LoggedUserConnections.Count > 0 && userObj.LoggedUserConnections.FirstOrDefault() != null)
                        {
                            myContacts.Add(new Recipient()
                            {
                                RecipientId = userObj.Id,
                                ReferenceConnectionId = userObj.LoggedUserConnections.FirstOrDefault().Token
                            });
                            if (item.IsApproved == true)
                            {
                                collSendData.Add(new PushNotification()
                                {
                                    Message = "You are approved as " + item.PersonType.NameEn + " for the Event:" + _eventDto.NameEn,
                                    ReferenceId = item.Id,
                                    CreatedOn = DateTime.Now,
                                    Recipients = myContacts
                                });
                            }
                            else
                            {
                                collSendData.Add(new PushNotification()
                                {
                                    Message = "You are rejected as " + item.PersonType.NameEn + " for the Event:" + _eventDto.NameEn,
                                    ReferenceId = item.Id,
                                    CreatedOn = DateTime.Now,
                                    Recipients = myContacts
                                });
                            }
                        }
                    }
                    if (collSendData.Count > 0)
                    {
                        foreach (var notification in collSendData)
                        {
                            NotificationConnector.PushNotifications(notification);
                        }
                    }
                }
            }


            List<EventPerson> addedEventPersons = _event.EventPersons.Where(x => eventEntity.EventPersons.Where(y => y.Id == x.Id).FirstOrDefault() == null).ToList();
            foreach (EventPerson item in addedEventPersons)
            {
                item.CreatedOn = System.DateTime.Now;
                item.IsApproved = true;
                item.EventId = eventEntity.Id;
                item.Event = null;
                item.PersonId = item.PersonId;
                item.Person = null;
                item.PersonTypeId = item.PersonTypeId;
                item.PersonType = null;
                eventEntity.EventPersons.Add(item);
            }

            List<Photo> addedEventPhotos = _event.Photos.Where(x => eventEntity.Photos.Where(y => y.PhotoName == x.PhotoName).FirstOrDefault() == null).ToList();
            foreach (Photo item in addedEventPhotos)
            {
                item.EventId = _event.Id;
                eventEntity.Photos.Add(item);
            }

            List<Photo> deletedEventPhotos = eventEntity.Photos.Where(x => _event.Photos.Where(y => y.PhotoName == x.PhotoName).FirstOrDefault() == null).ToList();
            foreach (Photo item in deletedEventPhotos)
            {
                eventEntity.Photos.Remove(item);
            }

            List<Presentation> deletedEventPresentations = eventEntity.Presentations.Where(x => _event.Presentations.Where(y => y.Id == x.Id).FirstOrDefault() == null).ToList();
            foreach (Presentation item in deletedEventPresentations)
            {
                eventEntity.Presentations.Remove(item);
            }

            List<Presentation> addedEventPresenations = _event.Presentations.Where(x => eventEntity.Presentations.Where(y => y.Id == x.Id).FirstOrDefault() == null).ToList();
            foreach (Presentation item in addedEventPresenations)
            {
                item.EventId = _event.Id;
                eventEntity.Presentations.Add(item);
            }

            return MapperHelper.Map<EventDto>(eventEntity);
        }

        public async Task<EventsGroupingByWeek> GetEventsGroupingByWeek(string companyCode)
        {

            //    var nextMonday = DayOfWeek.Saturday.AddDays(7);
            //var nextFriday = friday.AddDays(7);

            var startOfWeek = DateTime.Now.FirstDayOfWeek().Date;
            var endOfWeek = DateTime.Now.LastDayOfWeek().Date;
            // var events = await this.eventRepository.GetAllAsync();
            //var eventcompany = events.Where(e => e.EventCompanies.Any(x => x.Company.CompanyCode.ToLower() == companyCode));
            var currentEvents = await this.eventRepository.QueryAsync(x => x.User.Company.CompanyCode.ToLower() == companyCode.ToLower()
            &&
            x.DeletedOn == null && (x.EndDate >= startOfWeek && x.StartDate <= endOfWeek)
            );
            var nextEvents = await this.eventRepository.QueryAsync(x => x.User.Company.CompanyCode.ToLower() == companyCode.ToLower() &&
            x.DeletedOn == null && x.StartDate > endOfWeek);

            var currentEventsDtos = MapperHelper.Map<List<EventDto>>(currentEvents);
            var nextEventsDtos = MapperHelper.Map<List<EventDto>>(nextEvents);

            foreach (EventDto eventDto in currentEventsDtos)
            {
                eventDto.DescriptionEn = HtmlToPlainText(eventDto.DescriptionEn);
                eventDto.DescriptionAr = HtmlToPlainText(eventDto.DescriptionAr);
                eventDto.Agenda = eventDto.Agenda.OrderBy(a => a.StartDate).ThenBy(ag => ag.FromTime).ToList();
                eventDto.Sessions = new List<AgendaSessionDto>();
                // The below logic is to fill agenda without sessions
                if (eventDto.Agenda.Where(a => a.SessionId == null).Count() > 0)
                {
                    eventDto.Agenda.Where(a => a.SessionId == null).ToList().ForEach(x =>
                    {
                        x.DescriptionEn = HtmlToPlainText(x.DescriptionEn);
                        x.DescriptionAr = HtmlToPlainText(x.DescriptionAr);
                        x.TitleAr = HtmlToPlainText(x.TitleAr);
                        x.TitleEn = HtmlToPlainText(x.TitleEn);
                        x.DateStringFormat = x.StartDate.HasValue ? x.StartDate.Value.ToString("dd-MMMM-yy") : null;
                        if (eventDto.Sessions.FirstOrDefault(a => a.Date == x.StartDate) == null)
                        {
                            AgendaSessionDto sessionObj = new AgendaSessionDto
                            {
                                Date = x.StartDate,
                                OrderNumber = eventDto.Sessions.Count + 1,
                                Id = eventDto.Sessions.Count + 1,
                                EventId = eventDto.Id,
                                NameEn = "Day " + eventDto.Sessions.Count + 1,
                                NameAr = "Day " + eventDto.Sessions.Count + 1,
                                Agenda = new List<AgendumDto>()
                            };
                            eventDto.Sessions.Add(sessionObj);
                        }
                        var session = eventDto.Sessions.FirstOrDefault(a => a.Date == x.StartDate);
                        session.Agenda.Add(MapperHelper.Map<AgendumDto>(x));
                    });
                }
                else
                {
                    eventDto.Agenda.ToList().ForEach(x => x.DateStringFormat = x.StartDate.HasValue ? x.StartDate.Value.ToString("dd-MMMM-yy") : null);
                }

                // The below logic is to fill agenda with Different Sessions
                if (eventDto.AgendaSessions.Count > 0)
                {
                    eventDto.AgendaSessions.ToList().ForEach(x =>
                    {
                        if (eventDto.Sessions.FirstOrDefault(a => a.Id == x.Id) == null)
                        {
                            AgendaSessionDto sessionObj = new AgendaSessionDto
                            {
                                Date = x.Date,
                                OrderNumber = x.OrderNumber,
                                NameEn = x.NameEn,
                                NameAr = x.NameAr,
                                Id = x.Id,
                                EventId = eventDto.Id,
                                Agenda = new List<AgendumDto>()
                            };
                            eventDto.Sessions.Add(sessionObj);
                        }

                        var session = eventDto.Sessions.FirstOrDefault(a => a.Date == x.Date);
                        x.Agenda.ToList().ForEach(y =>
                        {
                            y.DescriptionEn = HtmlToPlainText(y.DescriptionEn);
                            y.DescriptionAr = HtmlToPlainText(y.DescriptionAr);
                            y.TitleAr = HtmlToPlainText(y.TitleAr);
                            y.TitleEn = HtmlToPlainText(y.TitleEn);
                            y.DateStringFormat = y.StartDate.HasValue ? y.StartDate.Value.ToString("dd-MMMM-yy") : null;
                            session.Agenda.Add(MapperHelper.Map<AgendumDto>(y));
                        });
                    });
                }
                eventDto.speakersCount = eventDto.EventPersons.Count(a => a.PersonTypeId == (int)PersonTypeEnum.Speaker);
                eventDto.vipCount = eventDto.EventPersons.Count(a => a.PersonTypeId == (int)PersonTypeEnum.VIP);
                eventDto.sponsorsCount = eventDto.EventCompanies.Count(a => a.CompanyTypeId == (int)CompanyTypeEnum.Sponser); ;
                eventDto.exhibitorsCount = eventDto.EventCompanies.Count(a => a.CompanyTypeId == (int)CompanyTypeEnum.Exhibitor);
                eventDto.partnersCount = eventDto.EventCompanies.Count(a => a.CompanyTypeId == (int)CompanyTypeEnum.Partner);
                //eventDto.registeredUsersCount = eventDto.EventUsers.Count;
                eventDto.sessionCount = eventDto.Agenda.Count;
                //item.RegisteredNormalUser = item.EventUsers.Where(x => x.RegistrationTypeId == (int)RegistrationTypeEnum.Athlete).Select(eu => new EventUserDto()
                //{
                //    Id = eu.Id,
                //    UserId = eu.UserId,
                //    EventId = eu.EventId,
                //    IsAttended = eu.IsAttended,
                //    RegistrationTypeId = eu.RegistrationTypeId,
                //    QRCode = eu.QRCode,
                //    DocumentName = HttpContext.Current.Request.Url.Authority + configurationsRepository.GetByKey("EventUserDocumentPath") + eu.DocumentName,
                //    User = eu.User,
                //    RegistrationType = eu.RegistrationType

                //}).ToList();
                //item.RegisteredVIPUsers = item.EventUsers.Where(x => x.RegistrationTypeId == (int)RegistrationTypeEnum.Athlete).ToList();
                //item.RegisteredSpeakerUsers = item.EventUsers.Where(x => x.RegistrationTypeId == (int)RegistrationTypeEnum.Athlete).ToList();
                //item.RegisteredExhibitorUsers = item.EventUsers.Where(x => x.RegistrationTypeId == (int)RegistrationTypeEnum.Exhibitor).ToList();
                //item.RegisteredSponsorUsers = item.EventUsers.Where(x => x.RegistrationTypeId == (int)RegistrationTypeEnum.Sponsor).ToList();
                //item.RegisteredPartnersUsers = item.EventUsers.Where(x => x.RegistrationTypeId == (int)RegistrationTypeEnum.Partner).ToList();
                eventDto.Sessions = eventDto.AgendaSessions;
            }


            foreach (EventDto eventDto in nextEventsDtos)
            {
                eventDto.DescriptionEn = HtmlToPlainText(eventDto.DescriptionEn);
                eventDto.DescriptionAr = HtmlToPlainText(eventDto.DescriptionAr);
                eventDto.Agenda = eventDto.Agenda.OrderBy(a => a.StartDate).ThenBy(ag => ag.FromTime).ToList();
                eventDto.Sessions = new List<AgendaSessionDto>();
                // The below logic is to fill agenda without sessions
                if (eventDto.Agenda.Where(a => a.SessionId == null).Count() > 0)
                {
                    eventDto.Agenda.Where(a => a.SessionId == null).ToList().ForEach(x =>
                    {
                        if (eventDto.Sessions.FirstOrDefault(a => a.Date == x.StartDate) == null)
                        {
                            AgendaSessionDto sessionObj = new AgendaSessionDto
                            {
                                Date = x.StartDate,
                                OrderNumber = eventDto.Sessions.Count + 1,
                                Id = eventDto.Sessions.Count + 1,
                                EventId = eventDto.Id,
                                NameEn = "Day " + eventDto.Sessions.Count + 1,
                                NameAr = "Day " + eventDto.Sessions.Count + 1,
                                Agenda = new List<AgendumDto>()
                            };
                            eventDto.Sessions.Add(sessionObj);
                        }
                        var session = eventDto.Sessions.FirstOrDefault(a => a.Date == x.StartDate);
                        x.DateStringFormat = x.StartDate.HasValue ? x.StartDate.Value.ToString("dd-MMMM-yy") : null;
                        session.Agenda.Add(MapperHelper.Map<AgendumDto>(x));
                    });
                }
                else
                {
                    eventDto.Agenda.ToList().ForEach(x => x.DateStringFormat = x.StartDate.HasValue ? x.StartDate.Value.ToString("dd-mmmm-yy") : null);
                }
                // The below logic is to fill agenda with Different Sessions
                if (eventDto.AgendaSessions.Count > 0)
                {
                    eventDto.AgendaSessions.ToList().ForEach(x =>
                    {
                        if (eventDto.Sessions.FirstOrDefault(a => a.Id == x.Id) == null)
                        {
                            AgendaSessionDto sessionObj = new AgendaSessionDto
                            {
                                Date = x.Date,
                                OrderNumber = x.OrderNumber,
                                NameEn = x.NameEn,
                                NameAr = x.NameAr,
                                Id = x.Id,
                                EventId = eventDto.Id,
                                Agenda = new List<AgendumDto>()
                            };
                            eventDto.Sessions.Add(sessionObj);
                        }

                        var session = eventDto.Sessions.FirstOrDefault(a => a.Date == x.Date);
                        x.Agenda.ToList().ForEach(y =>
                        {
                            y.DateStringFormat = y.StartDate.HasValue ? y.StartDate.Value.ToString("dd-MMMM-yy") : null;
                            session.Agenda.Add(MapperHelper.Map<AgendumDto>(y));
                        });
                    });
                }
                eventDto.speakersCount = eventDto.EventPersons.Count(a => a.PersonTypeId == (int)PersonTypeEnum.Speaker);
                eventDto.vipCount = eventDto.EventPersons.Count(a => a.PersonTypeId == (int)PersonTypeEnum.VIP);
                eventDto.sponsorsCount = eventDto.EventCompanies.Count(a => a.CompanyTypeId == (int)CompanyTypeEnum.Sponser); ;
                eventDto.exhibitorsCount = eventDto.EventCompanies.Count(a => a.CompanyTypeId == (int)CompanyTypeEnum.Exhibitor);
                eventDto.partnersCount = eventDto.EventCompanies.Count(a => a.CompanyTypeId == (int)CompanyTypeEnum.Partner);
                //eventDto.registeredUsersCount = eventDto.EventUsers.Count;
                eventDto.sessionCount = eventDto.Agenda.Count;
                //item.RegisteredNormalUser = item.EventUsers.Where(x => x.RegistrationTypeId == (int)RegistrationTypeEnum.Athlete).Select(eu => new EventUserDto()
                //{
                //    Id = eu.Id,
                //    UserId = eu.UserId,
                //    EventId = eu.EventId,
                //    IsAttended = eu.IsAttended,
                //    RegistrationTypeId = eu.RegistrationTypeId,
                //    QRCode = eu.QRCode,
                //    DocumentName = HttpContext.Current.Request.Url.Authority + configurationsRepository.GetByKey("EventUserDocumentPath") + eu.DocumentName,
                //    User = eu.User,
                //    RegistrationType = eu.RegistrationType

                //}).ToList();
                //item.RegisteredVIPUsers = item.EventUsers.Where(x => x.RegistrationTypeId == (int)RegistrationTypeEnum.Athlete).ToList();
                //item.RegisteredSpeakerUsers = item.EventUsers.Where(x => x.RegistrationTypeId == (int)RegistrationTypeEnum.Athlete).ToList();
                //item.RegisteredExhibitorUsers = item.EventUsers.Where(x => x.RegistrationTypeId == (int)RegistrationTypeEnum.Exhibitor).ToList();
                //item.RegisteredSponsorUsers = item.EventUsers.Where(x => x.RegistrationTypeId == (int)RegistrationTypeEnum.Sponsor).ToList();
                //item.RegisteredPartnersUsers = item.EventUsers.Where(x => x.RegistrationTypeId == (int)RegistrationTypeEnum.Partner).ToList();
                eventDto.Sessions = eventDto.AgendaSessions;
            }

            return new EventsGroupingByWeek { CurrentWeek = currentEventsDtos, Next = nextEventsDtos };
        }

        private static string HtmlToPlainText(string html)
        {
            if (html != null)
            {
                const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
                const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
                const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
                var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
                var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
                var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

                var text = html;
                //Decode html specific characters
                text = System.Net.WebUtility.HtmlDecode(text);
                //Remove tag whitespace/line breaks
                text = tagWhiteSpaceRegex.Replace(text, "><");
                //Replace <br /> with line breaks
                text = lineBreakRegex.Replace(text, Environment.NewLine);
                //Strip formatting
                text = stripFormattingRegex.Replace(text, string.Empty);

                return text;
            }
            return null;
        }

        public async Task<List<AttendeeQuestionDto>> GetQuestions(int eventid, int speakerid)
        {
            bool isAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.Administrator) > 0;
            bool isSuperAdmin = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.SuperAdministrator) > 0;
            bool isUser = this.user.UserInfo.Roles.Count(a => a.Id == (int)RolesEnum.User) > 0;

            var questions = await this.qstnRepository.QueryAsync(x => (eventid == 0 || x.EventId == eventid) &&
           (speakerid == 0 || x.SpeakerId == speakerid) &&
           (isSuperAdmin == true || (isAdmin == true && x.Event.CreatedBy == this.user.UserInfo.Id)
            || (isUser == true && x.Event != null && (x.Event.EventUsers.Count(a => a.UserId == this.user.UserInfo.Id) > 0
             || x.SpeakerId == this.user.UserInfo.PersonId))));
            var qstnDto = MapperHelper.Map<List<AttendeeQuestionDto>>(questions);
            return qstnDto;
        }

        public AttendeeQuestionDto Addquestion(AttendeeQuestionDto _questions)
        {
            AttendeeQuestion qstnMap;
            if (_questions.Id != 0)
            {
                qstnMap = this.qstnRepository.Query(x => x.Id == _questions.Id).FirstOrDefault();
                //Question Update if already inserted in db
                if (qstnMap != null)
                {
                    qstnMap.QuestionAr = _questions.QuestionAr;
                    qstnMap.QuestionEn = _questions.QuestionEn;
                    qstnMap.EventId = _questions.EventId;
                    qstnMap.SpeakerId = _questions.SpeakerId;
                    return _questions;
                }
            }
            qstnMap = MapperHelper.Map<AttendeeQuestion>(_questions);
            this.qstnRepository.Insert(qstnMap);
            AttendeeQuestionDto _qstnDto = MapperHelper.Map<AttendeeQuestionDto>(_questions);
            return _qstnDto;
        }

        public async Task<AttendeeQuestionDto> UpdateAnswer(AttendeeQuestionDto _qstnDto)
        {
            AttendeeQuestion qstnEntity = await this.qstnRepository.GetAsync(_qstnDto.Id);
            AttendeeQuestion _question = MapperHelper.Map<AttendeeQuestion>(_qstnDto);

            qstnEntity.Answer = _qstnDto.Answer;
            qstnEntity.Answered = true;
            return MapperHelper.Map<AttendeeQuestionDto>(qstnEntity);
        }

        public async Task<List<UserSubscriptionSingleDto>> GetPaidPackages(int userId)
        {
            var userSubscription = await this._userSubscriptionRepository.QueryAsync(x => x.UserId == userId && x.Events.Count == 0 && x.PaymentStatusId == (int)PaymentStatusEnum.Paid);
            List<UserSubscriptionSingleDto> userDto = MapperHelper.Map<List<UserSubscriptionSingleDto>>(userSubscription);
            return userDto;
        }

        public int checkSubsription(int userId, int subscriptionId, int eventId)
        {
            var data = this._userSubscriptionRepository.Query(x => x.UserId == userId && x.Events.Any(y => y.UserSubscriptionId == subscriptionId && y.Id != eventId));
            return data.Count();
        }
    }
}

public static partial class DateTimeExtensions
{
    public static DateTime FirstDayOfWeek(this DateTime dt)
    {
        var culture = new CultureInfo("ar-AE");//System.Threading.Thread.CurrentThread.CurrentCulture;
        var diff = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
        if (diff < 0)
            diff += 7;
        return dt.AddDays(-diff).Date;
    }

    public static DateTime LastDayOfWeek(this DateTime dt)
    {
        return dt.FirstDayOfWeek().AddDays(6);
    }

    public static DateTime FirstDayOfMonth(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, 1);
    }

    public static DateTime LastDayOfMonth(this DateTime dt)
    {
        return dt.FirstDayOfMonth().AddMonths(1).AddDays(-1);
    }

    public static DateTime FirstDayOfNextMonth(this DateTime dt)
    {
        return dt.FirstDayOfMonth().AddMonths(1);
    }
}