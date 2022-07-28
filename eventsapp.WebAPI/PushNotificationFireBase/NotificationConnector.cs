using EventsApp.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace eventsapp.WebAPI.PushNotificationFireBase
{
    //Class for Event Schedular
    public static class NotificationConnector
    {
        private static string FirebaseServerKey = "AAAASFvQaXk:APA91bGrCPi6eFbhkYHx9x7yPoNpH9jfmCZ5aiwyCDfxxyED_-X4kMATV31opS4hsENtx9LrVVu2T0apbQxIjxj3knz3Xs8m6s9_GbMDtG68nB3rHBJwWkRHBYqjRGPrNEWn4XXPq6Cv";
        private static string FirebaseSenderId = "310778030457";
        private static string FirebaseURL = "https://fcm.googleapis.com/fcm/send";
        public static eventsappEntities entity = new eventsappEntities();          
     
        public static void PushNotifications(PushNotification pushNotification)
        {
            entity.PushNotifications.Add(pushNotification);
            entity.SaveChanges();
            //push notification to user firebase(call client function)
            //var notificationtopush = entity.PushNotifications.Where(x => x.CreatedOn == DateTime.Now).ToList();
            foreach (var recipient in pushNotification.Recipients)
            {
                if (!recipient.IsSent.Value || !recipient.IsSent.HasValue)
                {
                    var result=NotificationConnector.InitiateFireBase(recipient.ReferenceConnectionId, pushNotification.Title, pushNotification.Message);
                    if (result.ToLower().Contains("success"))
                    {
                       var myRecipient =  entity.Recipients.Where(x => x.Id == recipient.Id);
                        if(myRecipient.Count()>0 && myRecipient!=null)
                        {
                            myRecipient.FirstOrDefault().IsSent = true;
                            entity.SaveChanges();
                        }
                    }
                }
                //later we will push after notifying
            }
        } 
        public static string InitiateFireBase(string deviceId,string title,string message)
        {
            String sResponseFromServer="";
            WebRequest tRequest = WebRequest.Create(FirebaseURL);
            tRequest.Method = "post";
            //serverKey - Key from Firebase cloud messaging server  
            tRequest.Headers.Add(string.Format("Authorization: key={0}", FirebaseServerKey));
            //Sender Id - From firebase project setting  
            tRequest.Headers.Add(string.Format("Sender: id={0}", FirebaseSenderId));
            tRequest.ContentType = "application/json";
            var payload = new
            {
                to = deviceId,
                priority = "high",
                content_available = true,
                notification = new
                {
                    body = message,
                    title = title,
                    badge = 1
                },
            };

            string postbody = JsonConvert.SerializeObject(payload).ToString();
            Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
            tRequest.ContentLength = byteArray.Length;
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                 sResponseFromServer = tReader.ReadToEnd();                                         
                            }
                    }
                }
            }
            return sResponseFromServer;
        }
    }
}