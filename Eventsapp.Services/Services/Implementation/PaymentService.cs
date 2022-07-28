using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Eventsapp.Repositories;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using System.Web;
using System;
using Eventsapp.Services;
using System.Net;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using EventsApp.Domain.DTOs.Payment;
using EventsApp.Domain.Enums;

namespace Eventsapp.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IKeyedRepository<Transaction, int> _TransactionRepository;
        private readonly IKeyedRepository<UserSubscription, int> _UserSubscriptionRepository;
        private readonly ICurrentUser user;
        public PaymentService(IKeyedRepository<Transaction, int> _TransactionRepository,
            IKeyedRepository<UserSubscription, int> _UserSubscriptionRepository, ICurrentUser user)
        {
            this._TransactionRepository = _TransactionRepository;
            this._UserSubscriptionRepository = _UserSubscriptionRepository;
            this.user = user;
        }
        public async Task<string> MakePayment(int subscriptionId)
        {
            var userSubscription = this._UserSubscriptionRepository.Query(x => x.Id == subscriptionId).FirstOrDefault();
            string paymentGatewayResponse = string.Empty;
            string reference = (new Random()).Next(100000000, 999999999).ToString("000000000");
            TransactionRequestDto transactionRequest = new TransactionRequestDto
            {
                authkey = "KWbrs#QZsQ-5kzd7",
                store = "20164",
                customer = new CustomerDto
                {
                    address = userSubscription.User.Company != null ? new EventsApp.Domain.DTOs.Payment.AddressDto
                    {
                        city = userSubscription.User.Company.Address.State.NameEn,
                        country = userSubscription.User.Company.Address.Country.NameEn,
                        line1 = userSubscription.User.Company.Address.Street
                    } : new EventsApp.Domain.DTOs.Payment.AddressDto(),
                    //address =  new EventsApp.Domain.DTOs.Payment.AddressDto
                    //{
                    //    city ="Dubai",
                    //    country = "UAE",
                    //    line1 = "Line 13"
                    //} ,
                    email = userSubscription.User.Email,
                    name = new NameDto
                    {
                        forenames = userSubscription.User.FirstName,
                        surname = userSubscription.User.LastName,
                    },
                    phone = userSubscription.User.PhoneNumber
                },
                method = "create",
                order = new OrderDto
                {
                    amount = userSubscription.EventPackage != null ? userSubscription.EventPackage.Price.ToString() : userSubscription.Fees.ToString(),
                    cartid = reference,
                    currency = "AED",
                    description = "this is a test payment for payment reference : " + reference,
                    test = "1"
                },
                returnType = new ReturnDto
                {
                    authorised = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/WebApi/api/Payment/ReceivePayment?paymentRef=" + reference,
                    cancelled = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/WebApi/api/Payment/ReceivePayment?paymentRef=" + reference,
                    declined = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/WebApi/api/Payment/ReceivePayment?paymentRef=" + reference,
                }
            };

            Transaction transaction = new Transaction
            {
                city = transactionRequest.customer.address.city,
                amount = transactionRequest.order.amount,
                authorisedURL = transactionRequest.returnType.authorised,
                cartId = transactionRequest.order.cartid,
                country = transactionRequest.customer.address.country,
                currency = transactionRequest.order.currency,
                description = transactionRequest.order.description,
                email = transactionRequest.customer.email,
                forenames = transactionRequest.customer.name.forenames,
                line1 = transactionRequest.customer.address.line1,
                paymentref = transactionRequest.order.paymentref,
                reference = transactionRequest.order.cartid,
                surname = transactionRequest.customer.name.surname,
                test = transactionRequest.order.test,
                url = "https://secure.telr.com/gateway/order.json",
                UserSubscriptionId = subscriptionId,
                CreatedOn = DateTime.Now,
                CreatedBy = this.user.UserInfo.Id
            };


            var json = new JavaScriptSerializer().Serialize(transactionRequest);
            json = json.Replace("returnType", "return");
            byte[] requestBytes = Encoding.UTF8.GetBytes(json);


            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://secure.telr.com/gateway/order.json");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.ContentLength = requestBytes.Length;

            // write paramters in request stream 
            using (Stream requestStream = httpWebRequest.GetRequestStream())
            {
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
            }
            using (Stream receiveStream = httpWebRequest.GetResponse().GetResponseStream())
            {
                using (StreamReader streamIn = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    paymentGatewayResponse = streamIn.ReadToEnd();
                    streamIn.Close();
                }
            }
            var returnObj = new JavaScriptSerializer().Deserialize<TransactionResponseDto>(paymentGatewayResponse);
            dynamic dynamicListObj = new JavaScriptSerializer().Deserialize<object>(paymentGatewayResponse);
            foreach (var item in dynamicListObj)
            {
                if (item.Key == "order")
                {
                    foreach (var childItem in item.Value)
                    {
                        if (childItem.Key == "ref")
                        {
                            returnObj.order.paymentref = childItem.Value;
                            transaction.paymentref = childItem.Value;
                        }
                    }
                }

            }
            userSubscription.PaymentStatusId = (int)PaymentStatusEnum.Paid;
            this._TransactionRepository.Insert(transaction);
            return returnObj.order.url;
        }
        public async Task<int> ReceivePayment(string paymentRef)
        {
            string paymentGatewayResponse = string.Empty;
            var transactionObj = this._TransactionRepository.Query(a => a.reference == paymentRef).FirstOrDefault();

            TransactionValidatorDto transactionRequest = new TransactionValidatorDto
            {
                authkey = "KWbrs#QZsQ-5kzd7",
                store = "20164",
                method = "check",
                order = new OrderResponseDto
                {
                    paymentref = transactionObj.paymentref
                },
            };
            var json = new JavaScriptSerializer().Serialize(transactionRequest);
            json = json.Replace("returnType", "return");
            json = json.Replace("paymentref", "ref");
            byte[] requestBytes = Encoding.UTF8.GetBytes(json);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://secure.telr.com/gateway/order.json");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.ContentLength = requestBytes.Length;
            // write paramters in request stream 
            using (Stream requestStream = httpWebRequest.GetRequestStream())
            {
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
            }
            using (Stream receiveStream = httpWebRequest.GetResponse().GetResponseStream())
            {
                using (StreamReader streamIn = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    paymentGatewayResponse = streamIn.ReadToEnd();
                    streamIn.Close();
                }
            }
            var returnObj = new JavaScriptSerializer().Deserialize<TransactionRequestDto>(paymentGatewayResponse);
            transactionObj.statusCode = returnObj.order.status.code;
            transactionObj.statusText = returnObj.order.status.text;
            if (transactionObj.statusCode == "3" && transactionObj.statusText == "Paid")
            {
                transactionObj.UserSubscription.PaymentStatusId = (int)PaymentStatusEnum.Paid;
                transactionObj.UserSubscription.TransactionId = transactionObj.Id;
                if (transactionObj.UserSubscription.EventUsers.FirstOrDefault() != null)
                {
                    transactionObj.UserSubscription.EventUsers.FirstOrDefault().IsApproved = true;
                }
            }
            else
            {
                transactionObj.UserSubscription.PaymentStatusId = (int)PaymentStatusEnum.Pending;
                transactionObj.UserSubscription.TransactionId = null;
            }
            return transactionObj.Id;
        }

        public async Task<TransactionReverseDto> GetTransaction(int transactionId)
        {
            string paymentGatewayResponse = string.Empty;
            var transactionObj = this._TransactionRepository.Query(a => a.Id == transactionId).FirstOrDefault();
            var returnValue = MapperHelper.Map<TransactionReverseDto>(transactionObj);
            return returnValue;
        }
    }
}
