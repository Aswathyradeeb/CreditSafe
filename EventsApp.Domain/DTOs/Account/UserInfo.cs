
using EventsApp.Domain.DTOs.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs
{ 
    public class UserInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } 
        public string Photo { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> EventId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> PersonId { get; set; }
        public Nullable<int> RegistrationTypeId { get; set; }
        public Nullable<int> AssignedFoodVouchers { get; set; }
        public Nullable<int> AssignedDrinkVouchers { get; set; }
        public Nullable<int> UsedFoodVouchers { get; set; }
        public Nullable<int> UsedDrinkVouchers { get; set; }
        public RegistrationTypeDto RegistrationType { get; set; }
        public List<RoleInfo> Roles { get; set; }
        public ICollection<UserActionsTakenDto> UserActionsTakens { get; set; }
        public ICollection<UserActionDto> UserActions { get; set; }
        public ICollection<UserCompanyDto> UserCompanies { get; set; }
        public ICollection<PreferredLanguageDto> PreferredLanguages { get; set; }
        public string Relation { get; set; }
    }
}
