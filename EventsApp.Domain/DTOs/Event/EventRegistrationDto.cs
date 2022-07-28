using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EventsApp.Domain.DTOs
{
    public class EventRegistrationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public PersonDto Person { get; set; }
        public CompanyDto Company { get; set; }
        public int RegistrationTypeId { get; set; }
        public ICollection<string> Delegates { get; set; }
        public int? PackageId { get; set; }
        public Nullable<int> Rating { get; set; }

        public RegistrationTypeDto RegistrationType { get; set; }
    }
}