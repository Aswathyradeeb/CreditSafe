using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{

    [DataContract]
    public class FilterParams
    {
        [DataMember]
        public ICollection<City> Cities { get; set; }
     
        [DataMember]
        public int CountryId { get; set; }
        public string Searchtext { get; set; }
    }
}