using EventsApp.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Framework.ExceptionHandling
{
    public class ExceptionHandlingManager
    {
       public static ILogger logger = LogManager.GetLogger("log");

        public static bool HandleException(Exception exceptionToHandle, ExceptionHandlingPolicies policy)
        {
            //TODO:
            // Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
            //return Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicy.HandleException(exceptionToHandle,policy);
            logger.Error(exceptionToHandle.Message, exceptionToHandle);
            return true;
        }
    }
}
