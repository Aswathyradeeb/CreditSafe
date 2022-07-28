using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs
{
    public class IndicatorDTO
    {
        public int id { get; set; }
        public int KPIId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
    }
}
