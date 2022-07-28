using System;
using System.Runtime.Serialization;

namespace EventsApp.Domain.DTOs
{ 
    public class PackageDto
    {
        public int Id { get; set; } 
        public string Benefits { get; set; }
        public Nullable<int> Cost { get; set; }
        public Nullable<int> SponsorTypeId { get; set; }
        public Nullable<int> EventId { get; set; }
         
        public virtual SponserTypeDto SponserType { get; set; }
    }
}