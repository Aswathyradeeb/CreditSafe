using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    [DataContract]
    public class IOSDeviceDto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string DeviceToken { get; set; }
        [DataMember]
        public string DeviceId { get; set; }

    }
}