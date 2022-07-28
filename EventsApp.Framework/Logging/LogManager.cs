using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Framework.Logging
{
    /// <summary>
    /// A factory that creates <code>ILogger</code> implementations
    /// </summary>
    public static class LogManager
    {
        #region Data Members

        private static readonly Dictionary<string, ILogger> Loggers = new Dictionary<string, ILogger>();

        #endregion

        /// <summary>
        /// Creates logging object
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>    
        public static ILogger GetLogger(string name)
        {
            ILogger logger = null;
            Loggers.TryGetValue(name, out logger);
            if (logger != null)
                return logger;

            // creates a log4net log based on the configuration supplied in the app.config
            log4net.Config.XmlConfigurator.Configure();
            log4net.ILog log4NetLogger = log4net.LogManager.GetLogger(name);

            logger = new Logger(log4NetLogger);
            Loggers.Add(name, logger);

            return logger;
        }
    }
}
