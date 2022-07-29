using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Microsoft.Owin;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Routing;
using EventsApp.Framework;
using eventsapp.WebAPI.Models;
using EventsApp.Framework.EmailsSender;
using EventsApp.Domain.DTOs;

namespace eventsapp.WebAPI
{
    public class WindsorCompositionRoot : IHttpControllerActivator
    {

        public WindsorCompositionRoot()
        {
        }


        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            var controller = (IHttpController)IoC.Instance.Resolve(controllerType);

            request.RegisterForDispose(new Release(() => IoC.Instance.Release(controller)));

            return controller;
        }

        private class Release : IDisposable
        {
            private readonly Action release;

            public Release(Action release)
            {
                this.release = release;
            }

            public void Dispose()
            {
                this.release();
            }
        }
    }

    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyNamed("eventsapp.WebAPI")
                                .BasedOn<System.Web.Http.ApiController>()
                                .LifestyleTransient());
        }
    }

    public class ManagerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IUnitOfWork>()
               .ImplementedBy<EntityFrameworkUnitOfWork>()
               .LifestylePerWebRequest());

          
            container.Register(Component.For<System.Data.Entity.DbContext>()
               .ImplementedBy<EventsApp.Domain.Entities.eventsappEntities>()
               .LifestylePerWebRequest());


            container.Register(Component.For<ApplicationDbContext>()
                .ImplementedBy<ApplicationDbContext>()
                .LifestylePerWebRequest());
            container.Register(Component.For(typeof(IKeyedRepository<,>))
          .ImplementedBy(typeof(RepositoryBase<,>))
          .LifestylePerWebRequest());

            container.Register(Component.For<PersistenceManager>().LifestylePerWebRequest());

            container.Register(
            Classes.FromAssemblyNamed("Eventsapp.Repositories")
            .Where(type => type.Name.EndsWith("Repository"))
           .WithServiceDefaultInterfaces()
           .LifestylePerWebRequest()
           .Configure(c => c.LifestylePerWebRequest()));

            container.Register(Classes.FromAssemblyNamed("Eventsapp.Services")
                .Where(type => type.Name.EndsWith("Service"))
                .WithServiceDefaultInterfaces()
                .LifestylePerWebRequest()
                .Configure(c => c.LifestylePerWebRequest()
                    .Interceptors<PersistenceManager>()
                    ));
        }
    }

    public class MvcInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //  container.Kernel.Register(Component.For<IEvaluationCriteriaRepository>().ImplementedBy<EvaluationCriteriaRepository>().LifestylePerWebRequest());
            container.Kernel.Register(Component.For<RouteCollection>().Instance(RouteTable.Routes));
            container.Kernel.Register(Component.For<HttpSessionStateBase>().UsingFactoryMethod(() => new HttpSessionStateWrapper(HttpContext.Current.Session)));
            container.Kernel.Register(Component.For<HttpContextBase>().UsingFactoryMethod(() => new HttpContextWrapper(HttpContext.Current)));
            container.Kernel.Register(Component.For<HttpServerUtilityBase>().UsingFactoryMethod(() => new HttpServerUtilityWrapper(HttpContext.Current.Server)));

            container.Kernel.Register(Component.For<IOwinContext>().UsingFactoryMethod(() => System.Web.HttpContext.Current.Request.GetOwinContext()).LifestylePerWebRequest());

            container.Kernel.Register(Component.For<RequestContext>().UsingFactoryMethod(() => HttpContext.Current.Request.RequestContext).LifestylePerWebRequest());

            container.Kernel.Register(Component.For<System.Web.Http.Routing.UrlHelper>().UsingFactoryMethod(() =>
            {
                return new System.Web.Http.Routing.UrlHelper();
            }));
        }
    }
}