using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class QuestionAnswerDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int QuestionOptionId { get; set; }
        public int PollResultId { get; set; }

        public QuestionOptionDto QuestionOption { get; set; }
        public QuestionDto Question { get; set; }
    }
}