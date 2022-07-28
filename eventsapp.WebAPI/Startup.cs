using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using EventsApp.Domain.DTOs;
//using eventsapp.WebAPI.Notifications;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(eventsapp.WebAPI.Startup))]

namespace eventsapp.WebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            MapperHelper.MapInitialize();
            //SchedulerConfigration.Start();
            //EmailScheduler.Start();
            app.MapSignalR();
            //TODO On it when notification are sending 
            //EventNotificationScehdular.Start();
        }
    }
}
