using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{ 
    public class InterestedAgendaDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AgendaId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }     
        public Nullable<int> SpeakerId { get; set; }
        public Nullable<int> EventId { get; set; }
        public virtual UserDto User { get; set; }
        public virtual PersonDto Person { get; set; }
        public virtual AgendumDto Agendum { get; set; }
        public virtual EventDto Event { get; set; }
        
    }
    public class FavoriteEvent
    {
        public int EventId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> SpeakerId { get; set; }
        public int AgendaId { get; set; }

    }
    public class FavoriteAgenda
    {
        public int FavoriteAgendaId { get; set; }
        public int UserId { get; set; }
        public int? AgendaId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> SpeakerId { get; set; }
        public Nullable<int> EventId { get; set; }

    }
}