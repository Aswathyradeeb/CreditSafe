using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs.Lookups
{
    public   class UserActionDto
    { 
        public short Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int RoleId { get; set; }

        public   RoleInfo Role { get; set; } 
    }
}
