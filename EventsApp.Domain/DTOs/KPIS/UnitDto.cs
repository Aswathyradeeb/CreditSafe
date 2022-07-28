using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs
{
    public class UnitDto
    {
        public int Id { get; set; }
        public string SymbolEn { get; set; }
        public string SymbolAr { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}
