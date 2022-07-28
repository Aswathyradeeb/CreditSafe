using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    [DataContract]
    public class PollDto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string NameEn { get; set; }
        [DataMember]
        public string NameAr { get; set; }
        [DataMember]
        public int? EventId { get; set; }
        [DataMember]
        public virtual ICollection<PollOptionDto> PollOptions { get; set; }
    }
}