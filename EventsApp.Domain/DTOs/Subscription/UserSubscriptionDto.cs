using EventsApp.Domain.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs.Subscription
{
   public  class UserSubscriptionDto
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> EventPackageId { get; set; }
        public Nullable<int> TransactionId { get; set; }
        public Nullable<int> PaymentStatusId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }

        public Nullable<decimal> Fees { get; set; }
        public virtual UserDto User { get; set; }
        public virtual EventPackageDto EventPackage { get; set; }
        public virtual LookupDto PaymentStatus { get; set; }
        public virtual ICollection<TransactionDto> Transactions { get; set; } 
    }
    public class UserSubscriptionSingleDto
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> EventPackageId { get; set; }
        public Nullable<int> TransactionId { get; set; }
        public Nullable<int> PaymentStatusId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }

        public virtual UserDto User { get; set; }
        public virtual EventPackageDto EventPackage { get; set; }
        public virtual LookupDto PaymentStatus { get; set; } 
    }
}
