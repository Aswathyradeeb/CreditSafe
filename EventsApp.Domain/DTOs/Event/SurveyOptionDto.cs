using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{ 
    public class SurveyOptionDto
    { 
        public int Id { get; set; } 
        public string NameEn { get; set; } 
        public string NameAr { get; set; }
        public Nullable<int> OrderNumber { get; set; }
        public decimal Progress { get; set; } 
        public int SurveyId { get; set; } 
        public int selectedId { get; set; }
    }
    public class SpeakerQuestionsOption
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public Nullable<int> OrderNumber { get; set; }
      
    }
}