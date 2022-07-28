using EventsApp.Domain.DTOs.Athlete;
using EventsApp.Domain.DTOs.Lookups;
using EventsApp.Domain.DTOs.Payment;
using EventsApp.Domain.DTOs.Subscription;
using EventsApp.Domain.Entities;

namespace EventsApp.Domain.DTOs
{
    public class MapperHelper
    {
        public static void MapInitialize()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Company, CompanyDto>().ReverseMap();
                cfg.CreateMap<Company, CompanySingleDto>().ReverseMap();
                cfg.CreateMap<Event, EventDto>().ReverseMap();
                cfg.CreateMap<Event, EventRegitrationDto>().ReverseMap();
                cfg.CreateMap<Event, EventBasicInfoDto>().ReverseMap();
                cfg.CreateMap<Event, EventSingleDto>().ReverseMap();
                cfg.CreateMap<Event, EventLightDto>().ReverseMap();
                cfg.CreateMap<Event, EventNameDto>().ReverseMap();
                cfg.CreateMap<Address, AddressDto>().ReverseMap();
                cfg.CreateMap<Person, PersonDto>().ReverseMap();
                cfg.CreateMap<PersonType, PersonTypeDto>().ReverseMap();
                cfg.CreateMap<User, UserDto>().ReverseMap();
                cfg.CreateMap<User, UserInfo>().ReverseMap();
                cfg.CreateMap<User, UserInfoDto>().ReverseMap();
                cfg.CreateMap<Role, RoleInfo>().ReverseMap();
                cfg.CreateMap<EventPerson, EventPersonDto>().ReverseMap();
                cfg.CreateMap<EventAddress, EventAddressDto>().ReverseMap();
                cfg.CreateMap<EventCompany, EventCompanyDto>().ReverseMap();
                cfg.CreateMap<EventNew, EventNewsDto>().ReverseMap();
                cfg.CreateMap<EventType, EventTypeDto>().ReverseMap();
                cfg.CreateMap<CompanyType, CompanyTypeDto>().ReverseMap();
                cfg.CreateMap<Photo, PhotoDto>().ReverseMap();
                cfg.CreateMap<Photo, PhotoReverseDto>().ReverseMap();
                cfg.CreateMap<SponserType, SponserTypeDto>().ReverseMap();
                cfg.CreateMap<Agendum, AgendumDto>().ReverseMap();
                cfg.CreateMap<Agendum, AgendumSingleDto>().ReverseMap();
                cfg.CreateMap<Agendum, AgendumLightDto>().ReverseMap();
                cfg.CreateMap<Agendum, AgendumListDto>().ReverseMap();
                cfg.CreateMap<AgendaSession, AgendaSessionDto>().ReverseMap();
                cfg.CreateMap<AgendaSession, AgendaSessionLightDto>().ReverseMap();
                cfg.CreateMap<InterestedAgenda, InterestedAgendaDto>().ReverseMap();
                cfg.CreateMap<EventUser, EventUserDto>().ReverseMap();
                cfg.CreateMap<EventUser, EventUserLightDto>().ReverseMap();
                cfg.CreateMap<EventUser, EventAttendeesDto>().ReverseMap();
                cfg.CreateMap<IOSDevice, IOSDeviceDto>().ReverseMap(); 
                cfg.CreateMap<AttendeeQuestion, AttendeeQuestionDto>().ReverseMap();
                cfg.CreateMap<Survey, SurveyDto>().ReverseMap();
                cfg.CreateMap<SurveyOption, SurveyOptionDto>().ReverseMap();
                cfg.CreateMap<SurveyResult, SurveyResultDto>().ReverseMap();
                cfg.CreateMap<Presentation, PresentationDto>().ReverseMap();  
                cfg.CreateMap<Country, CountryDto>().ReverseMap();
                cfg.CreateMap<State, StateDto>().ReverseMap();
                cfg.CreateMap<Package, PackageDto>().ReverseMap();
                cfg.CreateMap<RegistrationType, RegistrationTypeDto>().ReverseMap();
                cfg.CreateMap<Notification, NotificationDto>().ReverseMap();
                cfg.CreateMap<SpeakerRating, SpeakerRatingDto>().ReverseMap();
                cfg.CreateMap<UserAction, UserActionDto>().ReverseMap();
                cfg.CreateMap<UserActionsTaken, UserActionsTakenDto>().ReverseMap();
                cfg.CreateMap<PaymentStatus, LookupDto>().ReverseMap();
                cfg.CreateMap<EventPackage, EventPackageDto>().ReverseMap();
                cfg.CreateMap<UserSubscription, UserSubscriptionDto>().ReverseMap();
                cfg.CreateMap<UserSubscription, UserSubscriptionSingleDto>().ReverseMap();
                cfg.CreateMap<Transaction, TransactionDto>().ReverseMap();
                cfg.CreateMap<Transaction, TransactionReverseDto>().ReverseMap();
                cfg.CreateMap<UserCompany, UserCompanyDto>().ReverseMap();
                cfg.CreateMap<UserCompany, UserCompanySingleDto>().ReverseMap();
                cfg.CreateMap<Country, CountryWithStateDto>().ReverseMap();
                cfg.CreateMap<BackgroundTheme, BackgroundThemeDTO>().ReverseMap();
                cfg.CreateMap<Language, LanguageDto>().ReverseMap();
                cfg.CreateMap<PreferredLanguage, PreferredLanguageDto>().ReverseMap();
                cfg.CreateMap<ParticipantsRegistrationType, ParticipantsRegistrationTypeDto>().ReverseMap();
                cfg.CreateMap<Voucher, VoucherDto>().ReverseMap();
                cfg.CreateMap<AthleteVoucher, AthleteVoucherDto>().ReverseMap();
                cfg.CreateMap<ClaimedVoucher, ClaimedVoucherDto>().ReverseMap();
            });
        }
        public static T Map<T>(object source)
        {
            return (T)AutoMapper.Mapper.Map(source, source.GetType(), typeof(T));
        }
        public static T Map<T>(object source, object destination)
        {
            AutoMapper.Mapper.Map(source, destination);
            return (T)destination;
        }
    }
}