using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Windsor.Installer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Framework
{
    /// <summary>
    /// This class is an interface to the underlying inversion of control container.
    /// </summary>
    [CLSCompliant(false)]
    public sealed class IoC : IDisposable
    {
        #region Fields

        private readonly IWindsorContainer _container;
        private bool _disposed;

        #endregion

        #region Singleton Implementation

        public static void Init()
        {
            //preinit logic

            //dummay call just to init the singleton object
            IoC.Instance.GetType();

            //postinit logic
        }

        private static readonly IoC _instance = new IoC();

        /// <summary>
        /// This class cannot be instantiated from external code. IoC is singleton object.
        /// </summary>
        private IoC()
        {
            //if (string.IsNullOrEmpty(Settings.Default.IoCFileName))
            //    throw new ArgumentException("Invalid IoC Configuration file name");

            _container = new WindsorContainer(new XmlInterpreter("IoC.xml"));

        }


        /// <summary>
        /// Single IoC Instance accessor property. It provides access to the framework IoC service.
        /// </summary>
        public static IoC Instance
        {
            get { return _instance; }
        }

        #endregion

        /// <summary>
        /// Returns a component/object of the passed generic argument type.
        /// </summary>
        /// <typeparam name="T">The type of the component/object to return</typeparam>
        /// <returns>component/object of the given generic argument type</returns>
        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
        /// <summary>
        /// Returns a component/object of the passed generic argument type.
        /// </summary>
        /// <typeparam name="T">The type of the component/object to return</typeparam>
        /// <param name="key">Id of the component/object to return</param>
        /// <returns>component/object of the given generic argument type</returns>
        public T Resolve<T>(string key)
        {
            return _container.Resolve<T>(key);
        }


        /// <summary>
        /// Returns a component/object of the passed generic argument type.
        /// </summary>
        /// <typeparam name="T">The type of the component/object to return</typeparam>
        /// <param name="arguments">Argument to pass the component/object constructor</param>
        /// <returns>component/object of the given generic argument type</returns>
        public T Resolve<T>(IDictionary arguments)
        {
            return _container.Resolve<T>(arguments);
        }

        /// <summary>
        /// Returns a component/object of the passed generic argument type.
        /// </summary>
        /// <typeparam name="T">The type of the component/object to return</typeparam>
        /// <param name="argumentAsAnonymousType">An anonymous object with properties mapped to the component's constructor parameters.</param>
        /// <returns>component/object of the given generic argument type</returns>
        public T Resolve<T>(object argumentAsAnonymousType)
        {
            return _container.Resolve<T>(argumentAsAnonymousType);
        }
        /// <summary>
        /// Returns a component/object of the passed Service type.
        /// </summary>
        /// <param name="service">Service type</param>
        /// <returns>component/object of the given Service type</returns>
        public object Resolve(Type service)
        {
            return _container.Resolve(service);
        }
        /// <summary>
        /// Returns a component/object of the passed Service type.
        /// </summary>
        /// <param name="key">Id of the component/object to return</param>
        /// <param name="service">Service type</param>
        /// <returns>component/object of the given Service type</returns>
        public object Resolve(string key, Type service)
        {
            return _container.Resolve(key, service);
        }


        /// <summary>
        ///  Resolve all valid components that match this type.
        /// </summary>
        /// <typeparam name="T">The type of the component/object to return</typeparam>
        /// <returns>components/objects of the given generic argument type</returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            return _container.ResolveAll<T>();
        }


        //
        // Summary:
        //     Resolve all valid components that match this type.  The service type Arguments
        //     to resolve the service
        //
        // Parameters:
        //   arguments:
        //     Arguments to resolve the service
        //
        // Type parameters:
        //   T:
        //     The service type
        IEnumerable<T> ResolveAll<T>(IDictionary arguments)
        {
            return _container.ResolveAll<T>(arguments);
        }
        //
        // Summary:
        //     Resolve all valid components that match this type.  The service type Arguments
        //     to resolve the service
        //
        // Parameters:
        //   argumentsAsAnonymousType:
        //     Arguments to resolve the service
        //
        // Type parameters:
        //   T:
        //     The service type
        IEnumerable<T> ResolveAll<T>(object argumentsAsAnonymousType)
        {
            return _container.ResolveAll<T>(argumentsAsAnonymousType);
        }
        /// <summary>
        /// Releases the component instance
        /// </summary>
        /// <param name="component">Component to release</param>
        public void Release(object component)
        {
            _container.Release(component);
        }

        /// <summary>
        /// return true if the specified service is registered
        /// </summary>
        /// <param name="service">the service that will be checked</param>
        /// <returns></returns>
        public bool IsRegistered(Type service)
        {
            return _container.Kernel.HasComponent(service);
        }

        public void RegisterInstallers(IList<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                RegisterInstallers(assembly);
            }
        }
        public void RegisterInstallers(Assembly assembly)
        {
            _container.Install(FromAssembly.Instance(assembly));
        }
        #region IDispose Methods

        /// <summary>
        /// Implement IDisposable.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // take this object off the finalization queue and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizer method.  If Dispose() is called correctly, there is nothing
        /// for this to do.
        /// </summary>
        ~IoC()
        {
            if (!_disposed)
            {
                Dispose();
            }
        }

        /// <summary>
        /// Performs the actual dispose of the object
        /// </summary>
        /// <param name="disposing">If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources can be disposed.
        /// If disposing equals false, the method has been called by the
        /// runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.</param>
        private void Dispose(bool disposing)
        {
            try
            {
                // Check to see if Dispose has already been called.
                if (!_disposed)
                {
                    // If disposing equals true, dispose all managed
                    // and unmanaged resources.
                    if (disposing)
                    {
                        // Dispose managed resources.
                        _container.Dispose();
                    }

                    // Call the appropriate methods to clean up
                    // unmanaged resources here.
                }
            }
            catch (Exception ex)
            {
                //TODO:Call Logger to log this exception
            }
            finally
            {
                // Note disposing has been done.
                _disposed = true;
            }
        }

        #endregion


        public void Register(string key, Type service, Type serviceImplementation)
        {
            throw new NotImplementedException();
        }
    }
}
