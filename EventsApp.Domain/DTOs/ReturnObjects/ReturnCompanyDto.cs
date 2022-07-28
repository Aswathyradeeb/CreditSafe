using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs.ReturnObjects
{
    public class ReturnCompanyDto
    {
        public List<CompanyDto> companies { get; set; }
        public int companiesCount { get; set; }
    }
}
