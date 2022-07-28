using EventsApp.Framework;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;

namespace eventsapp.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //var cors = new EnableCorsAttribute("http://192.168.1.114:16250", "*", "*");
            //config.EnableCors(cors);


            IoC.Instance.RegisterInstallers(Assembly.GetExecutingAssembly());
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorCompositionRoot());

            config.Services.Replace(typeof(IHttpControllerActivator), new WindsorCompositionRoot());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            
            //return the response in json format
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
