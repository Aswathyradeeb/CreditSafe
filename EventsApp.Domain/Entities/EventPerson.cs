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
    
    public partial class EventPerson
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int EventId { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public Nullable<int> PersonTypeId { get; set; }
    
        public virtual Person Person { get; set; }
        public virtual PersonType PersonType { get; set; }
        public virtual Event Event { get; set; }
    }
}