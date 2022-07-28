//using eventsapp.WebAPI.Notifications;
using eventsapp.WebAPI.Schedular;
using EventsApp.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace eventsapp.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //EmailScheduler.Start();
            VoucherScheduler.Start();
            //EventNotificationScehdular.Start();
        }
    }
}
