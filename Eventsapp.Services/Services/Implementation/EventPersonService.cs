using Eventsapp.Repositories;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public class EventPersonService : IEventPersonService
    {
        private readonly IEventPersonRepository _EventPersonRepository;
        private readonly ISpeakerService speakerService;
        public EventPersonService(IEventPersonRepository eventPersonRepository, ISpeakerService speakerService)
        {
            this._EventPersonRepository = eventPersonRepository;
            this.speakerService = speakerService;
        }

        public async Task<EventPersonDto> CreateEventPerson(EventPersonDto eventPerson, string connString)
        {
            EventPerson eventPersonObj = MapperHelper.Map<EventPerson>(eventPerson); 
            eventPersonObj.CreatedOn = System.DateTime.Now;
            eventPersonObj.IsApproved = true;
            eventPersonObj.Event = null;
            eventPersonObj.PersonType = null;
            var PersonObj = await speakerService.CreateSpeaker(eventPerson.Person, connString);
            if(PersonObj == null)
            {
                throw new Exception("Internal Error");
            }
            eventPersonObj.Person = null;
            eventPersonObj.PersonId = PersonObj.Id;
            this._EventPersonRepository.Insert(eventPersonObj);
            this._EventPersonRepository.Commit();
            return MapperHelper.Map<EventPersonDto>(eventPersonObj);
        }

        public async Task<EventPersonDto> UpdateEventPerson(EventPersonDto eventPersonDto)
        {
            EventPerson eventPerson = MapperHelper.Map<EventPerson>(eventPersonDto);
            var eventPersonEntity = await this._EventPersonRepository.GetAsync(eventPersonDto.Id);

            eventPersonEntity.PersonTypeId = eventPerson.PersonTypeId;
            eventPersonEntity.IsApproved = eventPerson.IsApproved;
            eventPersonEntity.LastModified = System.DateTime.Now;
            eventPersonEntity.PersonType = null;
            if (eventPersonEntity.Person != null)
            {
                eventPersonEntity.Person.DateOfBirth = eventPerson.Person.DateOfBirth;
                eventPersonEntity.Person.DescriptionAr = eventPerson.Person.DescriptionAr;
                eventPersonEntity.Person.DescriptionEn = eventPerson.Person.DescriptionEn;
                eventPersonEntity.Person.Email = eventPerson.Person.Email;
                eventPersonEntity.Person.EmiratesID = eventPerson.Person.EmiratesID;
                eventPersonEntity.Person.Gender = eventPerson.Person.Gender;
                eventPersonEntity.Person.IsResidentOfUAE = eventPerson.Person.IsResidentOfUAE;
                eventPersonEntity.Person.ModifiedOn = eventPerson.Person.ModifiedOn;
                eventPersonEntity.Person.NameAr = eventPerson.Person.NameAr;
                eventPersonEntity.Person.NameEn = eventPerson.Person.NameEn;
                eventPersonEntity.Person.PassportNumber = eventPerson.Person.PassportNumber;
                eventPersonEntity.Person.Phone = eventPerson.Person.Phone;
                eventPersonEntity.Person.Photo = eventPerson.Person.Photo;
                eventPersonEntity.Person.Resume = eventPerson.Person.Resume;
                eventPersonEntity.Person.SpeakerRatings = eventPerson.Person.SpeakerRatings;
                eventPersonEntity.Person.TitleAr = eventPerson.Person.TitleAr;
                eventPersonEntity.Person.TitleEn = eventPerson.Person.TitleEn;
            }

            return MapperHelper.Map<EventPersonDto>(eventPersonEntity);
        }
    }
}
