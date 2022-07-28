using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Framework.EmailsSender
{
    public interface IMailSender
    {
        /// <summary>
        /// Send email message
        /// </summary>
        /// <param name="fromName">Sender name</param>
        /// <param name="fromEmail">Sender email</param>
        /// <param name="toName">Receiver name</param>
        /// <param name="toEmail">Receiver email</param>
        /// <param name="subject">Mail subject</param>
        /// <param name="body">Mail body</param>
        /// <param name="isBodyHtml">true if email Body is Html </param>
        /// <param name="Bcc">Email BCC address</param>
        /// <returns></returns>
        Task<bool> SendEmailAsync(string fromName, string fromEmail, string toName, string toEmail, string subject, string body, bool isBodyHtml = true, string Bcc = "", string[] attachmentFilesPaths = null, string[] attachmentFilesNames = null);

        /// <summary>
        /// Send email message
        /// </summary>
        /// <param name="fromName">Sender name</param>
        /// <param name="fromEmail">Sender email</param>
        /// <param name="recipients">Recipients dictionary of email and name</param>
        /// <param name="subject">Mail subject</param>
        /// <param name="body">Mail body</param>
        /// <param name="isBodyHtml">true if email Body is Html</param>
        /// <param name="Bcc">Email BCC address</param>
        /// <returns></returns>
        Task<bool> SendEmailAsync(string fromName, string fromEmail, Dictionary<string, string> toRecipients, string subject, string body, bool isBodyHtml = true, string Bcc = "", string[] attachmentFilesPaths = null, string[] attachmentFilesNames = null);
    }
}