using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    [DataContract]
    public class SponserTypeDto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string NameEn { get; set; }
        [DataMember]
        public string NameAr { get; set; }

    }
}