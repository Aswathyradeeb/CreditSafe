using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Eventsapp.Repositories;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using System;
using System.Web;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using FirebaseNet.Messaging;

namespace Eventsapp.Services
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly IKeyedRepository<PushNotification, int> _pushNotificationRepository;
        public void sendNotification(NotifyIOSDto notIOS, string[] tokens)
        {
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", "Basic YzMxMGY3NWYtMjNmOS00NGM0LWFhMjEtN2Q2ZmU0MjI4NDM4");

            var serializer = new JavaScriptSerializer();
            var obj = new
            {
                app_id = "04a37fd2-1083-472b-b715-53dda31767b2",
                contents = new { en = "IT Expo Event will Start now" },
                data = notIOS,
                //included_segments = new string[] { "All" }
                include_ios_tokens = tokens
                //new string[] { "39c83431c518c20f3156241af7705a2277037c9513a084c4ca1abd9a2c52bbad" }
            };
            var param = serializer.Serialize(obj);

            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }

            System.Diagnostics.Debug.WriteLine(responseContent);
        }

        public void sendNotificationAndroid(NotifyIOSDto notIOS)
        {

            try
            {

                string applicationID = "AAAAr0JQz80:APA91bExGvsJoSZsYFKM2lKsWXnXKtKk4985s89VaU40CjESt90CM7laCFw2_ZkPwN7duUZhTghNMdveD7avg5DwiuIaxqdkgvdiiM4reD9JcE0aIL1ZTxoKQ9YWz-wVg9NnsQ3b6-1W";

                string senderId = "752731869133";

                string deviceId = "e-xyzL3xO4U:APA91bGaY3P8nEqy_voG-XRUaGSWnLPSHporjcKrxTDopt5_t32qjG_tyTWUNOtFWbBQeDry2Qklekb5LluqAK_5g4MQb_pLpahlyq9SKBtf-9oSqJdeDmKLU-wUG21B2LViiDJMUr8Y";

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body = "Osama",
                        title = "AlBaami",
                        sound = "Enabled"

                    }
                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }

        
    }
}
