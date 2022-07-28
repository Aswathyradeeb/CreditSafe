using System.Collections.Generic;

namespace EventsApp.Domain.DTOs.ReturnObjects
{
    public class ReturnEventsDto
    {
        public List<EventLightDto> events { get; set; }
        public int eventsCount { get; set; }
    }
}
