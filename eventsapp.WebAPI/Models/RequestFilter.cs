using EventsApp.Domain.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace eventsapp.WebAPI.Models
{
    [DataContract]
    public class RequestFilter
    {
        [DataMember]
        public FilterParams FilterParams { get; set; }
        [DataMember]
        public string Searchtext { get; set; }
        [DataMember]
        public int Page { get; set; }
        [DataMember]
        public int PageSize { get; set; }
        [DataMember]
        public string SortBy { get; set; }
        [DataMember]
        public string SortDirection { get; set; }
    }

}