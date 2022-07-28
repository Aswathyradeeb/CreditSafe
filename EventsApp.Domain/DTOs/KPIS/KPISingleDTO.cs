using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EventsApp.Domain.DTOs;

namespace EventsApp.Domain.DTOs
{
    public class KPISingleDTO
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string Source { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public int ChartTypeId { get; set; }
        public Nullable<int> FrequencyUpdateId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<bool> IsPublic { get; set; }
        public Nullable<bool> IsJobSetup { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> AccessRightLevelId { get; set; }
    }
}
