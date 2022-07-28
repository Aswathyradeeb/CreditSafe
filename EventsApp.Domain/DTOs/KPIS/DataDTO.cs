using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs
{
    public class DataDTO
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public int? SeriesId { get; set; }
        public int? LabelId { get; set; }
        public LabelDTO Labels { get; set; }
        public SeriesDTO Series { get; set; }
    }
}
