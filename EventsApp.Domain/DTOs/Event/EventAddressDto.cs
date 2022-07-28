using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class EventAddressDto
    { 
        public int Id { get; set; } 
        public int AddressId { get; set; } 
        public int EventId { get; set; }
        public Nullable<System.DateTime> Date { get; set; } 
        public virtual AddressDto Address { get; set; }

    }
}