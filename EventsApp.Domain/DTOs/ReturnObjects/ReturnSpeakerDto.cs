using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs.ReturnObjects
{
    public class ReturnSpeakerDto
    {
        public List<PersonDto> speakers { get; set; }
        public int speakersCount { get; set; }
    }
}
