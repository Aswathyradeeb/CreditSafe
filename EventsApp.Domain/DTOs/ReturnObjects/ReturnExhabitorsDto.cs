using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs.ReturnObjects
{
    public class ReturnExhabitorsDto
    {
        public List<CompanyDto> exhabitors { get; set; }
        public int exhabitorsCount { get; set; }
    }
}
