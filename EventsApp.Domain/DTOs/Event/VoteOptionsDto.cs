using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class VoteOptionsDto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string OptionDesc { get; set; }
        [DataMember]
        public bool Value { get; set; }
        [DataMember]
        public int QuestionId { get; set; }

    }
}