using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Framework.Utilities
{
    public class Environment
    {
        public static DateTime GetServerTime()
        {
            return DateTime.Now;
        }
        public static DateTime GetServerDate()
        {
            return GetServerTime().Date;
        }
        public static DateTime GetServerDateHours()
        {
            var serverTime = GetServerTime();
            return new DateTime(serverTime.Year, serverTime.Month, serverTime.Day, serverTime.Hour, 0, 0);
        }
    }
}
