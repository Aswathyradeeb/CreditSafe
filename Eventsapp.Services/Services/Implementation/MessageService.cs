using Eventsapp.Services.MessengerService;
using System;
using System.Threading.Tasks;

namespace Eventsapp.Services
{
    public class MessageService : IMessageService
    {

        string SMSGateWayURL = System.Configuration.ConfigurationManager.AppSettings["SMSGateWayURL"];
        int SMSGateWayCustomerID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMSGateWayCustomerID"].ToString());
        string SMSGateWayUsername = System.Configuration.ConfigurationManager.AppSettings["SMSGateWayUsername"];
        string SMSGateWayPassword = System.Configuration.ConfigurationManager.AppSettings["SMSGateWayPassword"];
        MessengerSoapClient messenger;

        public MessageService()
        {
        }

        public AuthResult Authenticate()
        {
            messenger = new MessengerSoapClient(SMSGateWayURL);
            SoapUser user = new SoapUser();
            user.CustomerID = SMSGateWayCustomerID;
            user.Name = SMSGateWayUsername;
            user.Language = "en";
            user.Password = SMSGateWayPassword;
            AuthResult authData = messenger.Authenticate(user);
            return authData;
        }

        public async Task<SendResult> SendMessage(string msg, string lang, string phone)
        {
            SoapUser user = new SoapUser();
            user.CustomerID = SMSGateWayCustomerID;
            user.Name = SMSGateWayUsername;
            user.Language = lang;
            user.Password = SMSGateWayPassword;
            messenger = new MessengerSoapClient(SMSGateWayURL);
            AuthResult authResult = messenger.Authenticate(user);
            if (authResult.Result == "OK")
            {
                string originator = authResult.Originators[0];
                string defDate = DateTime.UtcNow.ToString("yyyyMMddhhmmss");
                string smsData = msg;
                SendResult result = messenger.SendSms(user, originator, smsData, phone, lang == "en" ? MessageType.Latin : MessageType.ArabicWithLatinNumbers, defDate, false, false, false);
                return result;
            }
            else
            {
                return new SendResult();
            }
        }
    }
}
