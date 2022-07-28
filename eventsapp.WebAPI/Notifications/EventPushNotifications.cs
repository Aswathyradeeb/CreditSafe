using eventsapp.WebAPI.PushNotificationFireBase;
using EventsApp.Domain.Entities;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eventsapp.WebAPI.Notifications
{
    public class EventJob : IJob
    {
        eventsappEntities entity = new eventsappEntities();
        public void Execute(IJobExecutionContext context)
        {
            //var result = NotificationConnector.InitiateFireBase("fX7pBGVjJz4:APA91bG5NTpePzFpm0AjKXSd6bNS97u1WHGI03OHyEx8zWahoL6MBSxKUKl01szifXXFeehz8D4zMIBNKGCtOJYdclEjNVIMoe91CIKxGEDOWq6ZKcRJ1GgtCk84lZMx540h78DWJpy6", "Event Schedular", "Event Schedule Now");
            DateTime dt = DateTime.Now;
            List<PushNotification> collSendData = new List<PushNotification>();
            List<Recipient> myContacts = new List<Recipient>();
            var events = entity.Events.ToList();
            foreach (var item in events)
            {
                //Event Reminder
                if (item.StartDate.Value.Subtract(dt).TotalDays <= 1)
                {
                    var myEventUsers = item.EventUsers.Where(x => x.EventId == item.Id);
                    if (myEventUsers.Count() > 0)
                    {
                        foreach (var user in myEventUsers.Distinct())
                        {
                            if (user.User.LoggedUserConnections != null)
                                if (user.User.LoggedUserConnections.Count > 0 && user.User.LoggedUserConnections.FirstOrDefault() != null)
                                {
                                    myContacts.Add(new Recipient()
                                    {
                                        RecipientId = user.UserId,
                                        ReferenceConnectionId = user.User.LoggedUserConnections.FirstOrDefault().Token
                                    });
                                }

                        }
                        collSendData.Add(new PushNotification()
                        {
                            Message = "The Event:" + item.NameEn + "will be starting on " + item.StartDate.Value.ToString("dd-MMM-yyyy"),
                            ReferenceId = item.Id,
                            CreatedOn = DateTime.Now,
                            Recipients = myContacts
                        });
                    }
                }
            }
            if (collSendData.Count > 0)
            {
                foreach (var notification in collSendData)
                {
                    NotificationConnector.PushNotifications(notification);
                }
            }
        }
    }
    public class AgendaJob : IJob
    {
        eventsappEntities entity = new eventsappEntities();
        public void Execute(IJobExecutionContext context)
        {
            //var result = NotificationConnector.InitiateFireBase("fX7pBGVjJz4:APA91bG5NTpePzFpm0AjKXSd6bNS97u1WHGI03OHyEx8zWahoL6MBSxKUKl01szifXXFeehz8D4zMIBNKGCtOJYdclEjNVIMoe91CIKxGEDOWq6ZKcRJ1GgtCk84lZMx540h78DWJpy6", "Event Schedular", "Event Schedule Now");
            DateTime dt = DateTime.Now;
            string currentTimeString = dt.ToString("HH:mm");
            List<PushNotification> collSendData = new List<PushNotification>();
            List<Recipient> myContacts = new List<Recipient>();
            var events = entity.Events.ToList();
            foreach (var item in events)
            {
                //Agenda Reminder
                foreach (var agenda in item.Agenda)
                {
                    if ((string.Compare(currentTimeString, agenda.FromTime) == 1)  && (string.Compare(agenda.ToTime, currentTimeString) == 1))
                    {
                        if (string.Compare(agenda.ToTime, currentTimeString) == 1)
                        {
                            var myEventUsers = item.EventUsers.Where(x => x.EventId == agenda.EventId);
                            if (myEventUsers.Count() > 0)
                            {
                                foreach (var user in myEventUsers.Distinct())
                                {
                                    if (user.User.LoggedUserConnections != null)
                                        if (user.User.LoggedUserConnections.Count > 0 && user.User.LoggedUserConnections.FirstOrDefault() != null)
                                        {
                                            myContacts.Add(new Recipient()
                                            {
                                                RecipientId = user.UserId,
                                                ReferenceConnectionId = user.User.LoggedUserConnections.FirstOrDefault().Token
                                            });
                                        }

                                }
                                collSendData.Add(new PushNotification()
                                {
                                    Message = "The Session: " + agenda.TitleEn + " for Event:" + item.NameEn + "will be starting on " + agenda.ToTime,
                                    ReferenceId = agenda.SessionId,
                                    CreatedOn = DateTime.Now,
                                    Recipients = myContacts
                                });
                            }
                        }
                    }
                }
            }
            if (collSendData.Count > 0)
            {
                foreach (var notification in collSendData)
                {
                    NotificationConnector.PushNotifications(notification);
                }
            }
        }
    }
}