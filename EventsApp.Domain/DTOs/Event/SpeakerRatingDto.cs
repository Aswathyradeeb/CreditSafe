using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs
{
    public class SpeakerRatingDto
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int EventId { get; set; }
        public Nullable<int> Rating { get; set; }

        //public virtual PersonDto Person { get; set; }
    }
}
