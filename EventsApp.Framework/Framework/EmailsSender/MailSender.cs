using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Framework.EmailsSender
{
    public class MailSender : IMailSender
    {
        private static string host = string.Empty;
        private static string smtpPort = string.Empty;
        private static string enableSSL = string.Empty;

        public MailSender()
        {

        }

        #region Send
        public async Task<bool> SendEmailAsync(string fromName, string fromEmail, string toName, string toEmail, string subject, string body, bool isBodyHtml = true, string bcc = "", string[] attachmentFilesPaths = null, string[] attachmentFilesNames = null)
        {
            //Throw exception if the recipient email is null or empty
            if (string.IsNullOrEmpty(toEmail))
                throw new ArgumentException();

            MailAddressCollection recipients = new MailAddressCollection();
            recipients.Add(new MailAddress(toEmail, (string.IsNullOrEmpty(toName) ? toEmail : toName)));

            return await SendEmailAsync(fromName, fromEmail, recipients, subject, body, isBodyHtml, bcc, attachmentFilesPaths, attachmentFilesNames);
        }

        public async Task<bool> SendEmailAsync(string fromName, string fromEmail, Dictionary<string, string> toRecipients, string subject, string body, bool isBodyHtml = true, string bcc = "", string[] attachmentFilesPaths = null, string[] attachmentFilesNames = null)
        {
            MailAddressCollection recipients = new MailAddressCollection();
            foreach (var item in toRecipients)
            {
                //Throw exception if the recipient email is null or empty
                if (string.IsNullOrEmpty(item.Key))
                    throw new ArgumentException();

                recipients.Add(new MailAddress(item.Key, (String.IsNullOrEmpty(item.Value) ? item.Key : item.Value)));
            }

            return await SendEmailAsync(fromName, fromEmail, recipients, subject, body, isBodyHtml, bcc, attachmentFilesPaths, attachmentFilesNames);
        }

        private async Task<bool> SendEmailAsync(string fromName, string fromEmail, MailAddressCollection recipients, string subject,
                               string body, bool isBodyHtml, string bcc, string[] attachmentFilesPaths = null, string[] attachmentFilesNames = null)
        {
            bool result = true;
            try
            {
                string SMTPServer = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"];
                int SMTPPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTPPort"]);
                string SMTPSSL = System.Configuration.ConfigurationManager.AppSettings["SMTPSSL"];
                string EmailAddress = System.Configuration.ConfigurationManager.AppSettings["EmailAddress"];
                string EmailPassword = System.Configuration.ConfigurationManager.AppSettings["EmailPassword"];

                if (string.IsNullOrEmpty(subject))
                    throw new ArgumentException();

                MailMessage mailMessage = new MailMessage();
                foreach (var item in recipients)
                {
                    mailMessage.To.Add(item);
                }
                mailMessage.SubjectEncoding = Encoding.UTF8;
                mailMessage.Subject = subject;
                mailMessage.BodyEncoding = Encoding.UTF8;
                //mailMessage.Body = CreateEmailBody(body);
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.From = new MailAddress(fromEmail, fromName);
                //mailMessage.Bcc.Add("abbas@hifive.ae");
                if (!string.IsNullOrWhiteSpace(bcc))
                {
                    MailAddress addressBcc = new MailAddress(bcc);
                    mailMessage.Bcc.Add(addressBcc);
                }
                int i = 0;
                if (attachmentFilesPaths != null)
                {
                    foreach (string attachmentFilePath in attachmentFilesPaths)
                    {
                        Attachment attachment = new Attachment(attachmentFilePath);
                        attachment.Name = attachmentFilesNames[i++];
                        mailMessage.Attachments.Add(attachment);
                    }
                }
                //queue email 
                SmtpClient smtp = new SmtpClient(SMTPServer, SMTPPort);
                //SmtpClient smtp = new SmtpClient("54.253.104.244", 25);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = Convert.ToBoolean(SMTPSSL);
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential(EmailAddress, EmailPassword);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Send(mailMessage);
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        #endregion


        public string CreateEmailBody(string content)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            var emailPath = System.Web.Hosting.HostingEnvironment.MapPath("/") + "/EmailTemplate.html";
            using (StreamReader reader = new StreamReader(emailPath))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{{Content}}", content);
            return body;
        }
    }
}