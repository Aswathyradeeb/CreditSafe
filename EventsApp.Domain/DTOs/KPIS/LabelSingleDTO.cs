using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs
{
    public class LabelSingleDTO
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int CriteriaId { get; set; }
        public string Formula { get; set; }
        public string CellName { get; set; }
        public Nullable<int> UnitId { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public UnitDto Unit { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<bool> HasFormula { get; set; }
        public Nullable<bool> HideDataEntry { get; set; }
    }
}
