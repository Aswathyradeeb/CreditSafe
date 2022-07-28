using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{ 
    public class AttendeeQuestionDto
    {
        public int Id { get; set; }
        public string QuestionEn { get; set; }
        public string QuestionAr { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> EventId { get; set; }
        public Nullable<int> SpeakerId { get; set; }
        public Nullable<bool> Answered { get; set; }
        public Nullable<int> AgendaId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string Answer { get; set; } 
        public UserDto User { get; set; }   
        public PersonDto Person { get; set; }
         public EventLightDto Event { get; set; }
    }
    public class GuestQuestion
    {
        public int Id { get; set; }
        public string QuestionEn { get; set; }
        public string QuestionAr { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> EventId { get; set; }
        public Nullable<int> SpeakerId { get; set; }
        public Nullable<bool> Answered { get; set; }
        public Nullable<int> AgendaId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    }
}