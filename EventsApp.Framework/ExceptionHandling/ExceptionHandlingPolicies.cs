using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Framework.ExceptionHandling
{
    public enum ExceptionHandlingPolicies
    {
        Framework = 1,
        ServiceLayer = 2,
        UI = 3,
        WindowsServices = 4,
        Threading = 5
    }
}
