using EventsApp.Domain.DTOs.Lookups;
using System.Runtime.Serialization;

namespace EventsApp.Domain.DTOs
{ 
    public class UserActionsTakenDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public System.DateTime ActionDate { get; set; }
        public int ActionBy { get; set; }
        public string Note { get; set; }
        public short UserActionId { get; set; }

        public   UserActionDto UserAction { get; set; }
    }
}
