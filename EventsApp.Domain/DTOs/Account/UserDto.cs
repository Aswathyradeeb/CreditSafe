using EventsApp.Domain.DTOs.Athlete;
using EventsApp.Domain.DTOs.Lookups;
using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Photo { get; set; }
        public string PhotoFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + Photo;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(Photo))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public string QrImage { get; set; }
        public string QRFullPath
        {
            get
            {
                string virtualFilePath = "";
                string absulotePath = "";

                virtualFilePath = "/WebApi/Uploads/Attachment/" + QrImage;
                absulotePath = VirtualPathUtility.ToAbsolute(virtualFilePath);
                if (!String.IsNullOrEmpty(QrImage))
                {
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + absulotePath;
                }
                else
                {
                    return null;
                }
            }
        }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> EventId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> PersonId { get; set; }
        public Nullable<int> RegistrationTypeId { get; set; }
        public RegistrationTypeDto RegistrationType { get; set; }
        public bool LimitForEventsReached { get; set; }
        public EventNameDto Event { get; set; }
        public ICollection<UserActionsTakenDto> UserActionsTakens { get; set; }
        public ICollection<UserActionDto> UserActions { get; set; }
        public List<RoleInfo> Roles { get; set; }
        public CompanyDto Company { get; set; }
        public ICollection<UserCompanyDto> UserCompanies { get; set; }
        public ICollection<PreferredLanguageDto> PreferredLanguages { get; set; }
        public  ICollection<AthleteVoucherDto> AthleteVouchers { get; set; }
        public ICollection<VoucherDto> Vouchers { get; set; }
        public Nullable<int> AssignedFoodVouchers { get; set; }
        public Nullable<int> AssignedDrinkVouchers { get; set; }
        public Nullable<int> UsedFoodVouchers { get; set; }
        public Nullable<int> UsedDrinkVouchers { get; set; }
        public Nullable<int> GuestOf { get; set; }
        public Nullable<int> GuestCount { get; set; }
        public string Relation { get; set; }
    }

    public class UserCompanySingleDto
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> CompanyId { get; set; }
    }
    public   class UserCompanyDto
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> CompanyId { get; set; }

        public virtual CompanySingleDto Company { get; set; }
    }
    public class UserEventDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Nullable<int> EventId { get; set; }
        public virtual EventNameDto Event { get; set; }
    }
    public class UserDataTable
    {
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        [DataMember]
        public System.DateTime CreatedOn { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public bool EmailConfirmed { get; set; }
        [DataMember]
        public string PasswordHash { get; set; }
        [DataMember]
        public string SecurityStamp { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public bool PhoneNumberConfirmed { get; set; }
        [DataMember]
        public bool TwoFactorEnabled { get; set; }
        [DataMember]
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        [DataMember]
        public bool LockoutEnabled { get; set; }
        [DataMember]
        public int AccessFailedCount { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Photo { get; set; }
        [DataMember]
        public Nullable<bool> IsActive { get; set; }
        [DataMember]
        public Nullable<int> EventId { get; set; } 
        [DataMember]
        public Nullable<int> CompanyId { get; set; }
        [DataMember]
        public Nullable<int> PersonId { get; set; }
        [DataMember]
        public Nullable<int> RegistrationTypeId { get; set; }
       
    }
    public class AttendeeHistory
    {
        public string EventNameEn { get; set; }
        public string EventNameAr { get; set; }
        public string AttendedAs { get; set; }
        public int UserId { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
    }

    public class UserInfoDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}