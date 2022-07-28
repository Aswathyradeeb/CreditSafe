using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs.Athlete
{
    public class ClaimedVoucherDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ClaimedFoodVoucher { get; set; }
        public Nullable<int> ClaimedDrinkVoucher { get; set; }

        public UserInfo User { get; set; }
        public UserInfoDto User1 { get; set; }
        public EventBasicInfoDto Event { get; set; }
    }
}
