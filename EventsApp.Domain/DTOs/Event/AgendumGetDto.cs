using System.Runtime.Serialization;

namespace EventsApp.Domain.DTOs
{
    public class AgendumGetDto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string TitleEn { get; set; }
        [DataMember]
        public string TitleAr { get; set; }
        [DataMember]
        public string DescriptionEn { get; set; }
        [DataMember]
        public string DescriptionAr { get; set; }
        [DataMember]
        public string FromTime { get; set; }
        [DataMember]
        public string ToTime { get; set; }
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public int EventId { get; set; }
        [DataMember]
        public int SpeakerId { get; set; }
        [DataMember]
        public PersonDto Speaker { get; set; }
        [DataMember]
        public int? SessionId { get; set; }
        [DataMember]
        public AgendaSessionDto AgendaSession { get; set; }
    }
}