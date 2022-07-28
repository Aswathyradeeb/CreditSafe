using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs
{
    public class LabelDTO
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int CriteriaId { get; set; }
        public ICollection<DataDTO> Data { get; set; }
    }
}
