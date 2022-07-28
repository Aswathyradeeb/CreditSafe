using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Eventsapp.Repositories;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using System.Web;
using System;
using EventsApp.Domain.DTOs.ReturnObjects;
using System.IO;
using System.Net.Mail;

namespace Eventsapp.Services
{
    public class EmailService : IEmailService
    {

        public EmailService()
        {
        }

        public async Task<bool> SendContactUsEmail(ContactUsDto ContactUsModel)
        {
            var result = true;
            var body = createContactUsEmailBody(ContactUsModel.Name, ContactUsModel.Email, ContactUsModel.Mobile, ContactUsModel.Subject, ContactUsModel.Message);
            string[] attachments = new string[0];
            result = await SendEmail(ContactUsModel.Subject, body, "customercare@esma.gov.ae", attachments);
            //result = await SendEmail(ContactUsModel.Subject, body, "abbas@inlogic.ae", attachments);
            return result;
        }

        public async Task<bool> SendContactUsSuccesResponseEmail(ContactUsDto ContactUsModel)
        {
            var result = true;
            var body = createContactusResponseEmailBody(ContactUsModel.Name, ContactUsModel.Email, ContactUsModel.Mobile, ContactUsModel.Subject, ContactUsModel.Message);
            string[] attachments = new string[0];
            result = await SendEmail("Email Recieved", body, ContactUsModel.Email, attachments);
            return result;
        }

        private string createContactUsEmailBody(string Name, string Email, string Mobile, string Subject, string Message)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/sendemail-contact-us.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("[Full Name]", Name);
            body = body.Replace("[Email]", Email);
            body = body.Replace("[Mobile No]", Mobile);
            body = body.Replace("[Subject]", Subject);
            body = body.Replace("[Message]", Message);
            return body;
        }

        private string createContactusResponseEmailBody(string Name, string Email, string Mobile, string Subject, string Message)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/email-contact-us.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("[FIRSTNAME]", Name);
            return body;
        }

        public async Task<bool> SendEmail(string subject, string body, string recipient, string[] attachments)
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                bool result = true;
                mailMessage.From = new MailAddress("noreply@hifive.ae");
                //mailMessage.From = new MailAddress("noreply@esma.gov.ae", "Emirates Authority for Standardization and Metrology");
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.To.Add(recipient);
                mailMessage.Bcc.Add("abbas@hifive.ae");

                SmtpClient smtp = new SmtpClient("mail.hifive.ae", 25);
                smtp.EnableSsl = Convert.ToBoolean("false");
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential("noreply@hifive.ae", "Hifive@123");
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Send(mailMessage);
                return result;
            }
        }
    }
}
