using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs
{
    [DataContract]
    public class FilterParams
    {
        [DataMember]
        public ICollection<Event> Events { get; set; }
        [DataMember]                                                                                       
        public ICollection<Event> Speakers { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public int CompanyId { get; set; }
        [DataMember]
        public string  UserRole { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string EventName { get; set; }
        [DataMember]
        public Nullable<DateTime> FromCreatedOn { get; set; }
        [DataMember]
        public Nullable<DateTime> ToCreatedOn { get; set; }
        [DataMember]
        public ICollection<RoleInfo> Roles { get; set; }
        public string Searchtext { get; set; }
    }
}
