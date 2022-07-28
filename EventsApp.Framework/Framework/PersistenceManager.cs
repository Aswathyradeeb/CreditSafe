using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Framework
{
    /// <summary>
    /// MainInterceptor , this Interceptor wrap the call with different Behaviors depend on the attributes on the method . 
    /// </summary>
    public class PersistenceManager : IInterceptor
    {




        #region IInterceptor Members
        public void Intercept(IInvocation invocation)
        {
            var successfully = false;
            Exception innerException = null;
            var UnitOfWork = IoC.Instance.Resolve<IUnitOfWork>();//unitOfWork;
            try
            {
                // Proceed the Call
                invocation.Proceed();
                //code to process async calls
                var returnType = invocation.Method.ReturnType;
                if (returnType != typeof(void))
                {
                    var returnValue = invocation.ReturnValue;
                    if (returnType == typeof(Task) || (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>)))
                    {
                        //Log.Debug("Returning with a task.");
                        var task = (Task)returnValue;
                        task.ContinueWith((antecedent) =>
                        {
                            try
                            {

                                if ((antecedent.IsCanceled) || (antecedent.Exception != null) || (antecedent.IsFaulted))
                                {
                                    UnitOfWork.Rollback();
                                    successfully = false;
                                    if (antecedent.Exception != null)
                                    {
                                        EventsApp.Framework.ExceptionHandling.ExceptionHandlingManager.HandleException(antecedent.Exception,
                                           EventsApp.Framework.ExceptionHandling.ExceptionHandlingPolicies.ServiceLayer);
                                    }
                                }
                                else
                                {
                                    UnitOfWork.Commit();
                                    successfully = true; 
                                }
                            }
                            catch (Exception ex)
                            {
                                innerException = ex;
                                successfully = false;
                                UnitOfWork.Rollback();
                                var reThrow = EventsApp.Framework.ExceptionHandling.ExceptionHandlingManager.HandleException(ex,
                                    EventsApp.Framework.ExceptionHandling.ExceptionHandlingPolicies.ServiceLayer);
                                if (reThrow)
                                {
                                    throw;
                                }
                            }
                            //});
                        }, TaskContinuationOptions.ExecuteSynchronously);
                        if (!successfully && innerException != null)
                        {
                            UnitOfWork.Rollback();
                            var reThrow = EventsApp.Framework.ExceptionHandling.ExceptionHandlingManager.HandleException(innerException,
                                EventsApp.Framework.ExceptionHandling.ExceptionHandlingPolicies.ServiceLayer);
                            throw innerException;
                        }
                    }
                    else
                    {
                        UnitOfWork.Commit();
                        successfully = true;
                    }
                }
                else
                {
                    UnitOfWork.Commit();
                    successfully = true;
                }
            }
            //catch the Call exception
            catch (Exception ex)
            {
                UnitOfWork.Rollback();
                var reThrow = EventsApp.Framework.ExceptionHandling.ExceptionHandlingManager.HandleException(ex,
                    EventsApp.Framework.ExceptionHandling.ExceptionHandlingPolicies.ServiceLayer);
                //throw;
                if (reThrow)
                {
                    throw;
                }
            }
        }
        #endregion
    }
}
