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
    
    public partial class EventNew
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    
        public virtual Event Event { get; set; }
    }
}
