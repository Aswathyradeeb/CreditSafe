using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs.Athlete
{
    public class VoucherDto
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescAr { get; set; }
        public string DescEn { get; set; }
        public System.DateTime StartFrom { get; set; }
        public System.DateTime StartEnd { get; set; }
        public double Price { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
    }
}
