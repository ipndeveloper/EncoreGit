using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace NetSteps.Common.Globalization
{
    public class Globalization
    {
        /// <summary>
        ///  http://www.danrigsby.com/blog/index.php/2008/08/24/timezone-vs-timezoneinfo-in-net/
        /// </summary>
        /// <returns></returns>
        public static ReadOnlyCollection<TimeZoneInfo> GetSystemTimeZones()
        {
            return TimeZoneInfo.GetSystemTimeZones();
        }

    }
}
