using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class EventPersonDto
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int EventId { get; set; }
        public Nullable<int> PersonTypeId { get; set; }
        public Nullable<int> Rating { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }

        public virtual PersonTypeDto PersonType { get; set; }
        public virtual PersonDto Person { get; set; } 
    }
}