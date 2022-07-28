using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EventsApp.Domain.DTOs
{
    public class AgendumListDto
    {

        [DataMember]
        public string Date;

        [DataMember]
        public List<AgendaSessionDto> Session;

       
        public string TitleEn;

        public string Id;

        public AgendumListDto()
        {
            Session = new List<AgendaSessionDto>();
        }                   
    }
}