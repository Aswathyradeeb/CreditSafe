using EventsApp.Domain.DTOs.Subscription;
using System;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public partial class EventUserDto
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public Nullable<bool> IsAttended { get; set; }
        public Nullable<int> RegistrationTypeId { get; set; }
        public string QRCode { get; set; }
        public string QRCodeFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + QRCode;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(QRCode))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public string DocumentName { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public Nullable<int> AgendaId { get; set; }
        public bool BadgePrintEnabled { get; set; }
        public bool checkInEnabled { get; set; }
        public virtual UserDto User { get; set; }
        public virtual EventLightDto Event { get; set; }

        public virtual RegistrationTypeDto RegistrationType { get; set; }
        public virtual UserSubscriptionSingleDto UserSubscription { get; set; }
        public AgendumLightDto Agendum { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<int> UserSubscriptionId { get; set; }
        public Nullable<bool> IsRegistered { get; set; }
        public Nullable<bool> IsIndividual { get; set; }
        public Nullable<int> VisitorCount { get; set; }
    }

    public partial class EventUserLightDto
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public Nullable<int> AgendaId { get; set; }
        public Nullable<bool> IsAttended { get; set; }
        public Nullable<int> RegistrationTypeId { get; set; }
        public string QRCode { get; set; }
        public string DocumentName { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }

        public bool BadgePrintEnabled { get; set; }
        public bool checkInEnabled { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<int> UserSubscriptionId { get; set; }
        public Nullable<bool> IsRegistered { get; set; }
        public Nullable<bool> IsIndividual { get; set; }
        public Nullable<int> VisitorCount { get; set; }
    }

    public partial class EventAttendeesDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public Nullable<bool> IsAttended { get; set; }
        public Nullable<int> RegistrationTypeId { get; set; }
        public string QRCode { get; set; }
        public string QRCodeFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + QRCode;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(QRCode))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public Nullable<int> AgendaId { get; set; }
        public bool BadgePrintEnabled { get; set; }
        public bool checkInEnabled { get; set; }
        public UserInfoDto User { get; set; }
        public EventBasicInfoDto Event { get; set; }

        public RegistrationTypeDto RegistrationType { get; set; }
        public AgendumLightDto Agendum { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<int> UserSubscriptionId { get; set; }
        public Nullable<bool> IsRegistered { get; set; }
        public Nullable<bool> IsIndividual { get; set; }
        public Nullable<int> VisitorCount { get; set; }
    }
}