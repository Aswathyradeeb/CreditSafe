using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using EventsApp.Domain.Entities;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace eventsapp.WebAPI.Notifications
{
    [HubName("PushNotification")]
    public class PushNotificationHub : Hub
    {
    
        public void Hello()
        {
            Clients.All.hello();
        }
        public void Send(string message)
        {
            Clients.All.broadcastMessage(message);
        }
        private readonly static ConnectionMapping<string> _connections =
           new ConnectionMapping<string>();
     

        //Logged Use Call
        public void GetNotification(PushNotification notification)
        {
            try
            {

                string loggedUser = Context.User.Identity.Name;
                //Get TotalNotification
                string totalNotif = LoadNotifData(loggedUser);

                //Send To
                //UserHubModels receiver;
                ////Get TotalNotification
                //string connectionToSendMessage;
                //Connections.TryGetValue(notification.SentTo, out connectionToSendMessage);

                //if (!string.IsNullOrWhiteSpace(connectionToSendMessage))
                //{
                //    //var cid = receiver.ConnectionIds.FirstOrDefault();
                //    var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                //    context.Clients.Client(connectionToSendMessage).broadcaastNotif(notification);
                //    //context.Clients.All.broadcaastNotif(totalNotif);
                //}
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        //Specific User Call
        //public void SendNotification(PushNotification notification)
        //{
        //    try
        //    {
                
        //        var context = GlobalHost.ConnectionManager.GetHubContext<PushNotificationHub>();
        //        foreach (var connectionId in _connections.GetConnections(notification.RecipientConnectionId))
        //        {                    
        //            context.Clients.Client(connectionId).broadcaastNotif(notification);
        //        }                 
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}

        private string LoadNotifData(string userId)
        {
            string total = userId;// Convert.ToInt32(userId);
            //using (SGSEntities context = new SGSEntities())
            //{
            //    var query = (from t in context.Users.Notifications
            //                 where t.SentTo == userId
            //                 select t)
            //.ToList();
            //    total = query.Count;
            //    return (total++).ToString();
            //}
            return total;

        }
        public void Unsubscribe(string customerId)
        {
            Groups.Remove(Context.ConnectionId, customerId);
        }
        public override Task OnConnected()
        {
            //string userName = Context.User.Identity.Name;
            //if (!Connections.ContainsKey(userName))
            //{
            //    Connections.TryAdd(userName, Context.ConnectionId);
            //}
            //else
            //{
            //    Connections[userName] = Context.ConnectionId;
            //}
            string name = Context.User.Identity.Name;

            _connections.Add(name, Context.ConnectionId);

            return base.OnConnected();

            //string userName = Context.User.Identity.Name;
            //string connectionId = Context.ConnectionId;

            //var user = Users.GetOrAdd(userName, _ => new UserHubModels
            //{
            //    UserName = userName,
            //    ConnectionIds = new HashSet<string>()
            //});

            //lock (user.ConnectionIds)
            //{
            //    user.ConnectionIds.Add(connectionId);
            //    if (user.ConnectionIds.Count == 1)
            //    {
            //        Clients.Others.userConnected(userName);
            //    }
            //}

            //return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            _connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);

        }
    }
    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<string>> _connections =
            new Dictionary<T, HashSet<string>>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }
    }
}