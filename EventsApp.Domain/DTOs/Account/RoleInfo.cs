 
using System.Runtime.Serialization; 

namespace EventsApp.Domain.DTOs
{ 
    public class RoleInfo
    { 
        public int Id { get; set; } 
        public string Name { get; set; } 
        public string NameEn { get; set; } 
        public string NameAr { get; set; }
    }
}