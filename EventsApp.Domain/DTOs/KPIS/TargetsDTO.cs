namespace EventsApp.Domain.DTOs
{
    using System;
    using System.Collections.Generic;
    
    public partial class TargetsDTO
    {
        public int Id { get; set; }
        public Nullable<decimal> Target { get; set; }
        public Nullable<int> KPIId { get; set; }
        public string Year { get; set; }
        public Nullable<int> SeriesId { get; set; }
        public Nullable<int> LabelId { get; set; }

        public KPISingleDTO KPIs { get; set; }
        public LabelSingleDTO Labels { get; set; }
        public SeriesSingleDTO Series { get; set; }
    }
}
