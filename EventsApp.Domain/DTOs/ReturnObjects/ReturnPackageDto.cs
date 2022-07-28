using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs.ReturnObjects
{
    public class ReturnPackageDto
    {
        public List<PackageDto> Packages { get; set; }
        public int PackagesCount { get; set; }
    }
}
