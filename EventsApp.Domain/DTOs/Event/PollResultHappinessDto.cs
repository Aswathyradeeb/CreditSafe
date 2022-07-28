using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class PollResultHappinessDto
    {
        public int Data { get; set; }
        public string LabelEn { get; set; }
        public string LabelAr { get; set; }
    }
}