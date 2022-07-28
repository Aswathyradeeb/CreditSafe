using System;
using System.Collections.Generic;

namespace EventsApp.Domain.DTOs
{
    public class AgendaSessionDto
    {
        public int Id { get; set; }
        public Nullable<int> EventId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> OrderNumber { get; set; }
        public virtual ICollection<AgendumDto> Agenda { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }

    public class AgendaSessionLightDto
    {
        public int Id { get; set; }
        public Nullable<int> EventId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> OrderNumber { get; set; }
        public virtual ICollection<AgendumLightDto> Agenda { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}