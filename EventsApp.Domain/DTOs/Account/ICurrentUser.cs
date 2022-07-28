using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs
{
    public interface ICurrentUser
    {
        UserInfo UserInfo { get; }

        Task<UserInfo> GetUserInfoAsync();
        void SendEmail(string subject, string body);
        void SendSMS(string PhoneNumber, string SMS);

    }
}
