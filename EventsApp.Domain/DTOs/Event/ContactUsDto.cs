using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EventsApp.Domain.DTOs
{
    public class ContactUsDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}