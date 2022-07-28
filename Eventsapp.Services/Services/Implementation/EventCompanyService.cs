using Eventsapp.Repositories;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public class EventCompanyService : IEventCompanyService
    {
        private readonly IEventCompanyRepository _EventCompanyRepository;
        private readonly ICompanyService companyService;
        public EventCompanyService(IEventCompanyRepository eventCompanyRepository, ICompanyService companyService)
        {
            this._EventCompanyRepository = eventCompanyRepository;
            this.companyService = companyService;
        }

        public async Task<EventCompanyDto> CreateEventCompany(EventCompanyDto eventCompany, string connString)
        { 
            EventCompany eventCompanyObj = MapperHelper.Map<EventCompany>(eventCompany); 
            eventCompanyObj.CreatedOn = System.DateTime.Now;
            eventCompanyObj.IsApproved = true;
            eventCompanyObj.CompanyType = null;
            eventCompanyObj.Event = null;
            eventCompanyObj.Package = null; 
            var CompanyObj = await companyService.CreateCompany(eventCompany.Company, connString);
            if (CompanyObj == null)
            {
                throw new Exception("Internal Error");
            }
            eventCompanyObj.Company = null;
            eventCompanyObj.CompanyId = CompanyObj.Id;  
            this._EventCompanyRepository.Insert(eventCompanyObj);
            this._EventCompanyRepository.Commit();
            return MapperHelper.Map<EventCompanyDto>(eventCompanyObj);
        }
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public async Task<EventCompanyDto> UpdateEventCompany(EventCompanyDto eventCompanyDto)
        {
            EventCompany eventCompany = MapperHelper.Map<EventCompany>(eventCompanyDto);
            var eventCompanyEntity = await this._EventCompanyRepository.GetAsync(eventCompanyDto.Id);

            eventCompanyEntity.CompanyTypeId = eventCompany.CompanyTypeId;
            eventCompanyEntity.IsApproved = eventCompany.IsApproved;
            eventCompanyEntity.LastModified = System.DateTime.Now;
            eventCompanyEntity.PackageId = eventCompany.PackageId;
            eventCompanyEntity.StandLocation = eventCompany.StandLocation;
            eventCompanyEntity.StandNumber = eventCompany.StandNumber;
            eventCompanyEntity.Company.Email = eventCompany.Company.Email;
            eventCompanyEntity.Company.ModifiedOn = eventCompany.Company.ModifiedOn;
            eventCompanyEntity.Company.NameAr = eventCompany.Company.NameAr;
            eventCompanyEntity.Company.NameEn = eventCompany.Company.NameEn;
            eventCompanyEntity.Company.Phone = eventCompany.Company.Phone;
            eventCompanyEntity.Company.Photo = eventCompany.Company.Photo;

            if (eventCompany.Company.Address != null)
            {
                if (eventCompanyEntity.Company.Address == null)
                    eventCompanyEntity.Company.Address = new Address();

                eventCompanyEntity.Company.Address.CountryId = eventCompany.Company.Address.Country != null ? eventCompany.Company.Address.Country.Id : eventCompany.Company.Address.CountryId;
                eventCompanyEntity.Company.Address.Lat = eventCompany.Company.Address.Lat;
                eventCompanyEntity.Company.Address.Lng = eventCompany.Company.Address.Lng;
                eventCompanyEntity.Company.Address.LocationPhoto = eventCompany.Company.Address.LocationPhoto;
                eventCompanyEntity.Company.Address.StateId = eventCompany.Company.Address.State != null ? eventCompany.Company.Address.State.Id : eventCompany.Company.Address.StateId;
                eventCompanyEntity.Company.Address.Street = eventCompany.Company.Address.Street;
            }
            return MapperHelper.Map<EventCompanyDto>(eventCompanyEntity);
        }
    }
}
