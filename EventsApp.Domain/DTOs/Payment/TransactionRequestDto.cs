using EventsApp.Domain.DTOs.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs.Payment
{
    public class TransactionDto
    { 
        public int Id { get; set; }
        public string cartId { get; set; }
        public string test { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string reference { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string paymentref { get; set; }
        public string email { get; set; }
        public string forenames { get; set; }
        public string surname { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string line1 { get; set; }
        public string authorisedURL { get; set; }
        public string statusCode { get; set; }
        public string statusText { get; set; }
        public Nullable<int> UserSubscriptionId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
         
    }

    public class TransactionReverseDto
    {
        public int Id { get; set; }
        public string cartId { get; set; }
        public string test { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string reference { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string paymentref { get; set; }
        public string email { get; set; }
        public string forenames { get; set; }
        public string surname { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string line1 { get; set; }
        public string authorisedURL { get; set; }
        public string statusCode { get; set; }
        public string statusText { get; set; }
        public Nullable<int> UserSubscriptionId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public   UserSubscriptionSingleDto UserSubcription { get; set; }

    }
    public class TransactionRequestDto
    {
        public string method { get; set; }
        public string store { get; set; }
        public string authkey { get; set; }
        public OrderDto order { get; set; }
        public CustomerDto customer { get; set; }
        public ReturnDto returnType { get; set; }
    }


    public class TransactionResponseDto
    {
        public string method { get; set; }
        public string store { get; set; }
        public string authkey { get; set; }
        public OrderResponseDto order { get; set; } 
    }
    public class TransactionValidatorDto
    {
        public string method { get; set; }
        public string store { get; set; }
        public string authkey { get; set; }
        public OrderResponseDto order { get; set; }
    }
    public class StatusDto
    {
        public string code { get; set; }
        public string text { get; set; }

    }
    public class OrderResponseDto
    { 
        public string reference { get; set; }
        public string url { get; set; }
        public string paymentref { get; set; } 
    }
    public class OrderDto
    {
        public string cartid { get; set; }
        public string test { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string reference { get; set; }
        public string url { get; set; }
        public string paymentref { get; set; }
        public StatusDto status { get; set; }
    }
    public class CustomerDto
    { 
        public string email { get; set; }
        public NameDto name { get; set; }
        public AddressDto address { get; set; }
        public string phone { get; set; } 
    }
    public class NameDto
    {
        public string forenames { get; set; }
        public string surname { get; set; }
    }
    public class AddressDto
    {
        public string line1 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
    }
    public class ReturnDto
    {
        public string authorised { get; set; }
        public string declined { get; set; }
        public string cancelled { get; set; } 

    }
} 