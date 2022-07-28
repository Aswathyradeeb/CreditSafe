using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs.Core
{
    [DataContract]
    class SMSMessageDto
    {
        [DataMember()]
        public string Destination { get; set; }
        [DataMember()]
        public string Subject { get; set; }
        [DataMember()]
        public string Body { get; set; }
    }
}
