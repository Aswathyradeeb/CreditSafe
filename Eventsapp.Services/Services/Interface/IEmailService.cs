using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.ReturnObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public interface IEmailService
    {
        Task<bool> SendContactUsEmail(ContactUsDto ContactUsModel);
        Task<bool> SendContactUsSuccesResponseEmail(ContactUsDto ContactUsModel);
    }
}
