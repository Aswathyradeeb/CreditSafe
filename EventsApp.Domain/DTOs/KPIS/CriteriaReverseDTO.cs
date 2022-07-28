using System.Collections.Generic;

namespace EventsApp.Domain.DTOs
{
    public class CriteriaReverseDTO
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int KPIId { get; set; }
        public string AxisType { get; set; }
        public virtual KPISingleDTO KPIs { get; set; }
    }
}
