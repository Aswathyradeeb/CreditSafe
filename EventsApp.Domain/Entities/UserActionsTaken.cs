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
    
    public partial class UserActionsTaken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public System.DateTime ActionDate { get; set; }
        public int ActionBy { get; set; }
        public string Note { get; set; }
        public short UserActionId { get; set; }
    
        public virtual UserAction UserAction { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
