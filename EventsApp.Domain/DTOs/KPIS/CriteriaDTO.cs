using System.Collections.Generic;

namespace EventsApp.Domain.DTOs
{
    public class CriteriaDTO
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int KPIId { get; set; }
        public string AxisType { get; set; }
        // public KPIDTO KPIs { get; set; }
        public List<LabelSingleDTO> Labels { get; set; }
        public List<SeriesDTO> Series { get; set; }
    }
}
