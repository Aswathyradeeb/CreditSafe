using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class LanguageDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string Code { get; set; }
        public Nullable<bool> IsRTL { get; set; }
    }
}