//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EventsApp.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class AttendeeQuestion
    {
        public int Id { get; set; }
        public string QuestionEn { get; set; }
        public string QuestionAr { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> EventId { get; set; }
        public Nullable<int> SpeakerId { get; set; }
        public Nullable<bool> Answered { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string Answer { get; set; }
        public Nullable<int> AgendaId { get; set; }
    
        public virtual User User { get; set; }
        public virtual Person Person { get; set; }
        public virtual Agendum Agendum { get; set; }
        public virtual Event Event { get; set; }
    }
}
