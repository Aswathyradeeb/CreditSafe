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
    
    public partial class UserAction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserAction()
        {
            this.UserActionsTakens = new HashSet<UserActionsTaken>();
        }
    
        public short Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int RoleId { get; set; }
    
        public virtual Role Role { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserActionsTaken> UserActionsTakens { get; set; }
    }
}