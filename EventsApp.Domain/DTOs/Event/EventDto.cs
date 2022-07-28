
using EventsApp.Domain.DTOs.Subscription;
using System;
using System.Collections.Generic;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> EventTypeId { get; set; }
        public string BadgePhoto { get; set; }
        public Nullable<int> BgImageId { get; set; }
        public string BadgePhotoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + BadgePhoto;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(BadgePhoto))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public string BannerPhoto { get; set; }
        public string BannerPhotoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + BannerPhoto;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(BannerPhoto))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public bool HasSponsors { get; set; }
        public bool HasExhibitors { get; set; }
        public bool HasSpeaker { get; set; }
        public bool HasPartners { get; set; }
        public bool HasVIP { get; set; }
        public bool SponsorsOnlineRegister { get; set; }
        public bool ExhibitorsOnlineRegister { get; set; }
        public bool SpeakerOnlineRegister { get; set; }
        public bool PartnersOnlineRegister { get; set; }
        public bool VIPOnlineRegister { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string EventLogo { get; set; }
        public string EventLogoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + EventLogo;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(EventLogo))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public string VideoURL { get; set; }
        public string Video { get; set; }
        public string SelectedCurrency { get; set; }
        public string VideoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + Video;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(Video))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public string InstagramURL { get; set; }
        public string FacebookURL { get; set; }
        public string TwitterURL { get; set; }
        public string LinkedInURL { get; set; }
        public string ThemeURL { get; set; }
        public string UniqueId { get; set; }
        public int speakersCount { get; set; }
        public int vipCount { get; set; }
        public int registeredUsersCount { get; set; }
        public int sponsorsCount { get; set; }
        public int partnersCount { get; set; }
        public int exhibitorsCount { get; set; }
        public int sessionCount { get; set; }
        public Nullable<int> UserSubscriptionId { get; set; }
        public int ParticipantsLimit { get; set; }
        public Nullable<decimal> ParticipantFees { get; set; }
        public Nullable<bool> HasPayment { get; set; }
        public Nullable<int> ParticipantsRegistrationTypeId { get; set; }
        public virtual UserDto User { get; set; }
        public virtual UserDto User1 { get; set; }
        public virtual UserDto User2 { get; set; }
        public virtual ICollection<AgendumDto> Agenda { get; set; }
        public virtual ICollection<AgendaSessionDto> Sessions { get; set; }
        public virtual ICollection<AgendaSessionDto> AgendaSessions { get; set; }
        public virtual ICollection<AttendeeQuestionDto> AttendeeQuestions { get; set; }
        public virtual ICollection<EventAddressDto> EventAddresses { get; set; }
        public virtual ICollection<EventCompanyDto> EventCompanies { get; set; }
        public virtual ICollection<EventNewsDto> EventNews { get; set; }
        public virtual ICollection<EventPersonDto> EventPersons { get; set; }
        //public virtual ICollection<SpeakerRatingDto> SpeakerRatings { get; set; }
        public virtual EventTypeDto EventType { get; set; }
        //public virtual ICollection<EventUserLightDto> EventUsers { get; set; } // Breaking for large number of Attendees
        public virtual ICollection<NotificationDto> Notifications { get; set; }
        public virtual ICollection<PackageDto> Packages { get; set; }
        public virtual ICollection<PhotoDto> Photos { get; set; }
        public virtual ICollection<PresentationDto> Presentations { get; set; }
        public virtual ICollection<SurveyDto> Surveys { get; set; }
        public virtual UserSubscriptionSingleDto UserSubscription { get; set; }

        //public ICollection<EventUserDto> RegisteredNormalUser { get; set; }
        //public ICollection<EventUserDto> RegisteredVIPUsers { get; set; }
        //public ICollection<EventUserDto> RegisteredSpeakerUsers { get; set; }
        //public ICollection<EventUserDto> RegisteredExhibitorUsers { get; set; }
        //public ICollection<EventUserDto> RegisteredSponsorUsers { get; set; }
        //public ICollection<EventUserDto> RegisteredPartnersUsers { get; set; }
        public ParticipantsRegistrationTypeDto ParticipantsRegistrationType { get; set; }
    }

    public class EventLightDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> EventTypeId { get; set; }
        public string BadgePhoto { get; set; }
        public string BadgePhotoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + BadgePhoto;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(BadgePhoto))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public Nullable<int> UserSubscriptionId { get; set; }
        public string BannerPhoto { get; set; }
        public Nullable<int> BgImageId { get; set; }
        public string BannerPhotoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + BannerPhoto;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(BannerPhoto))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public Nullable<bool> HasSponsors { get; set; }
        public Nullable<bool> HasExhibitors { get; set; }
        public Nullable<bool> HasSpeaker { get; set; }
        public Nullable<bool> HasPartners { get; set; }
        public Nullable<bool> HasVIP { get; set; }
        public Nullable<bool> SponsorsOnlineRegister { get; set; }
        public Nullable<bool> ExhibitorsOnlineRegister { get; set; }
        public Nullable<bool> SpeakerOnlineRegister { get; set; }
        public Nullable<bool> PartnersOnlineRegister { get; set; }
        public Nullable<bool> VIPOnlineRegister { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string SelectedCurrency { get; set; }
        public string EventLogo { get; set; }
        public string EventLogoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + EventLogo;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(EventLogo))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public string VideoURL { get; set; }
        public string Video { get; set; }

        public string VideoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + Video;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(Video))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public string InstagramURL { get; set; }
        public string FacebookURL { get; set; }
        public string TwitterURL { get; set; }
        public string LinkedInURL { get; set; }
        public string ThemeURL { get; set; }
        public string UniqueId { get; set; }
        public virtual UserDto User { get; set; }
        public virtual EventTypeDto EventType { get; set; }
        public virtual ICollection<EventCompanyDto> EventCompanies { get; set; }
        public virtual ICollection<EventPersonDto> EventPersons { get; set; }
        public virtual UserSubscriptionSingleDto UserSubscription { get; set; }
        public Nullable<int> ParticipantsLimit { get; set; }
        public Nullable<decimal> ParticipantFees { get; set; }
        public Nullable<bool> HasPayment { get; set; }
        public Nullable<int> ParticipantsRegistrationTypeId { get; set; }
        //public ParticipantsRegistrationTypeDto ParticipantsRegistrationType { get; set; }
    }

    public class EventSingleDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> EventTypeId { get; set; }
        public string BadgePhoto { get; set; }
        public string BadgePhotoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + BadgePhoto;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(BadgePhoto))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public Nullable<int> BgImageId { get; set; }
        public string BannerPhoto { get; set; }
        public string BannerPhotoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + BannerPhoto;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(BannerPhoto))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public Nullable<bool> HasSponsors { get; set; }
        public Nullable<bool> HasExhibitors { get; set; }
        public Nullable<bool> HasSpeaker { get; set; }
        public Nullable<bool> HasPartners { get; set; }
        public Nullable<bool> HasVIP { get; set; }
        public Nullable<bool> SponsorsOnlineRegister { get; set; }
        public Nullable<bool> ExhibitorsOnlineRegister { get; set; }
        public Nullable<bool> SpeakerOnlineRegister { get; set; }
        public Nullable<bool> PartnersOnlineRegister { get; set; }
        public Nullable<bool> VIPOnlineRegister { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string SelectedCurrency { get; set; }
        public string EventLogo { get; set; }
        public string EventLogoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + EventLogo;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(EventLogo))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public string VideoURL { get; set; }
        public string Video { get; set; }

        public string VideoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + Video;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(Video))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public string InstagramURL { get; set; }
        public string FacebookURL { get; set; }
        public string TwitterURL { get; set; }
        public string LinkedInURL { get; set; }
        public Nullable<int> UserSubscriptionId { get; set; }

        public virtual ICollection<AgendumDto> Agenda { get; set; }
        public virtual ICollection<AgendaSessionDto> AgendaSessions { get; set; }
        public virtual ICollection<EventAddressDto> EventAddresses { get; set; }
        public Nullable<int> ParticipantsLimit { get; set; }
        public Nullable<decimal> ParticipantFees { get; set; }
        public Nullable<bool> HasPayment { get; set; }
        public Nullable<int> ParticipantsRegistrationTypeId { get; set; }
        //public ParticipantsRegistrationTypeDto ParticipantsRegistrationType { get; set; }
    }

    public class EventNameDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
    }
    public class EventsGroupingByWeek
    {
        public List<EventDto> CurrentWeek { get; set; }
        public List<EventDto> Next { get; set; }
    }

    public class EventRegitrationDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> EventTypeId { get; set; }
        public string BadgePhoto { get; set; }
        public Nullable<int> BgImageId { get; set; }
        public string BadgePhotoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + BadgePhoto;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(BadgePhoto))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public string BannerPhoto { get; set; }
        public string BannerPhotoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + BannerPhoto;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(BannerPhoto))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public string EventLogo { get; set; }
        public string EventLogoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + EventLogo;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(EventLogo))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public Nullable<int> UserSubscriptionId { get; set; }
        public UserDto User { get; set; }
        public ICollection<AgendaSessionLightDto> AgendaSessions { get; set; }
        public int ParticipantsLimit { get; set; }
        public Nullable<int> ParticipantsRegistrationTypeId { get; set; }
    }

    public class EventBasicInfoDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> ParticipantsLimit { get; set; }
        public Nullable<decimal> ParticipantFees { get; set; }
        public Nullable<bool> HasPayment { get; set; }
        public Nullable<int> ParticipantsRegistrationTypeId { get; set; }
    }

}