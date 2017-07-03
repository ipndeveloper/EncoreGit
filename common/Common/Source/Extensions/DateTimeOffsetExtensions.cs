using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: DateTimeOffset Extensions
    /// Created: 03-08-2010
    /// </summary>
    public static class DateTimeOffsetExtensions
    {
        public static List<TimeZoneInfo> GetPossibleTimeZones(this DateTimeOffset offsetTime)
        {
            TimeSpan offset = offsetTime.Offset;
            ReadOnlyCollection<TimeZoneInfo> timeZones;

            // Get all time zones defined on local system
            timeZones = TimeZoneInfo.GetSystemTimeZones();
            // Iterate time zones 
            List<TimeZoneInfo> possibleTimeZones = new List<TimeZoneInfo>();
            foreach (TimeZoneInfo timeZone in timeZones)
            {
                // Compare offset with offset for that date in that time zone
                if (timeZone.GetUtcOffset(offsetTime.DateTime).Equals(offset))
                    possibleTimeZones.Add(timeZone);
            }

            return possibleTimeZones;
        }
    }
}
