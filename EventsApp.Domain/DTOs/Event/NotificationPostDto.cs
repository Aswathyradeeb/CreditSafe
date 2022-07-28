using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class NotificationPostDto
    {
        [DataMember]
        public int NotificationId { get; set; }
    }
}