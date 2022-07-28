using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs.ReturnObjects
{
    public class ReturnSponsersDto
    {
        public List<CompanyDto> sponsers { get; set; }
        public int sponsersCount { get; set; }
    }
}
