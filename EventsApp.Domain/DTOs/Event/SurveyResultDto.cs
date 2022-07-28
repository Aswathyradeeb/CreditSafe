using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{ 
    public class SurveyResultDto
    {
        public int Id { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int SurveyOptionId { get; set; }
        public int UserId { get; set; }
        public int SurveyId { get; set; }
        public int EventId { get; set; }
        public int AgendaId { get; set; }
        public UserDto User { get; set; }
        public   SurveyOptionDto SurveyOption { get; set; }
        public EventDto Events { get; set; }
        public SurveyDto Survey { get; set; }
        public virtual AgendumDto Agendum { get; set; }
    }
    public class SurveyResultSingleDto
    {
        public int Id { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int SurveyOptionId { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; } 
    }
    public class SurveyResultDataTable
    {
    
        [DataMember]
        public System.DateTime CreatedOn { get; set; }
        [DataMember]
        public int SurveyOptionId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int SurveyId { get; set; }
        [DataMember]
        public Nullable<int> AgendaId { get; set; }
        [DataMember]
        public int EventId { get; set; }
    }
    public class SurveyResponse
    {         
        public int SurveyOptionId { get; set; }
        public int UserId { get; set; }
        public int SurveyId { get; set; }
        public int EventId { get; set; }
        public Nullable<int> AgendaId { get; set; }
    }
}