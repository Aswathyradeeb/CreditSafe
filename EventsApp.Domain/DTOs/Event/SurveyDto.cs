using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{ 
    public class SurveyDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int EventId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<int> AgendaId { get; set; }
        public virtual AgendumLightDto Agendum { get; set; }
        public virtual EventLightDto Event { get; set; }
        public virtual ICollection<SurveyOptionDto> SurveyOptions { get; set; }
        public Nullable<bool> FromSpeaker { get; set; }
        public Nullable<int> ResponseAlotTime { get; set; }
    }

    public class SpeakerQuestions
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int AgendaId { get; set; }
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public int EventId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }

        public Nullable<int> ResponseAlotTime { get; set; }

        public virtual ICollection<SurveyOptionDto> SurveyOptions { get; set; }
    }

    public class AddSpeakerQuestionDTO
    {
      
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int AgendaId { get; set; }

        public int EventId { get; set; }
        public Nullable<int> ResponseAlotTime { get; set; }

        public virtual ICollection<SurveyOptionDto> SurveyOptions { get; set; }
    }

}