using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs
{ 
    public class EventNewsDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }

    }
}
