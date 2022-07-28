using EventsApp.Domain.DTOs.Lookups;
using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class PreferredLanguageDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LanguageId { get; set; }

        public LanguageDto Language { get; set; }
    }
}