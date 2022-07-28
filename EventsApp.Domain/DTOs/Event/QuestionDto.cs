using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string QuestionEn { get; set; }
        public string QuestionAr { get; set; }
        public ICollection<QuestionOptionDto> QuestionOptions { get; set; }
    }
}