using EventsApp.Domain.DTOs.Athlete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs.ReturnObjects
{
    public class ReturnAthletesDto
    {
        public List<UserDto> athletes { get; set; }
        public int athletesCount { get; set; }
    }

    public class ReturnAthleteVoucherDto
    {
        public List<VoucherDto> voucher { get; set; }
        public int voucherCount { get; set; }
    }
}
