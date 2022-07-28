using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eventsapp.WebAPI.Models
{
    public class EventRegisterModel
    {
        public int userid { get; set; }
        public int eventid { get; set; }
        public int registrationTypeId { get; set; }
        public int? packageId { get; set; }
        public string StandLocation { get; set; }
        public string StandNumber { get; set; }
    }
}