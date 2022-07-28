using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs.Athlete
{
    public class AthleteVoucherDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VoucherId { get; set; }
        public Nullable<bool> IsValid { get; set; }

        public virtual VoucherDto Voucher { get; set; }
    }
}
