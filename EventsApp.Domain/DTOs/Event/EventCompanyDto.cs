using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{ 
    public class EventCompanyDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int EventId { get; set; }
        public Nullable<int> PackageId { get; set; }
        public string StandLocation { get; set; }
        public string StandNumber { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public Nullable<int> CompanyTypeId { get; set; }
        public CompanyTypeDto CompanyType { get; set; }
        public virtual PackageDto Package { get; set; }
        public virtual CompanyDto Company { get; set; }
    }
}