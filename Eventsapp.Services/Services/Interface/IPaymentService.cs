using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.Payment;
using EventsApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface IPaymentService
    {
        Task<string> MakePayment(int subscriptionId);
        Task<int> ReceivePayment(string paymentRef);
        Task<TransactionReverseDto> GetTransaction(int transactionId);
    }
}
