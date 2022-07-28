using Eventsapp.Services.MessengerService;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface IMessageService
    {
        Task<SendResult> SendMessage(string msg, string sender, string numbers);
    }
}
