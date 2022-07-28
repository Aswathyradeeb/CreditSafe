using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class PollResultDto
    {
        public int Id { get; set; }
        public System.DateTime VotedOn { get; set; }
        public int PollOptionId { get; set; }
        public string UserId { get; set; }
        public int EventId { get; set; }
        public string PollRemarks { get; set; }
        public string Suggestion { get; set; }

        public UserDto User { get; set; }
        public EventDto Event { get; set; }
        public PollOptionDto PollOption { get; set; }
        public ICollection<QuestionAnswerDto> QuestionAnswers { get; set; }
    }
}