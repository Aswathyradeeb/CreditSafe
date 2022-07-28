using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class QuestionOptionDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string OptionEn { get; set; }
        public string OptionAr { get; set; }
    }
}