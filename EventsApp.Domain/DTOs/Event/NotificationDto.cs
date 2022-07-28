using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public Nullable<bool> IsRead { get; set; }
        public Nullable<int> EventId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }

    }
}