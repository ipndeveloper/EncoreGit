using System;
using System.Globalization;

namespace NetSteps.Silverlight.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Use to set the time on a DateTime object without modifying the date. - JHE
        /// </summary>
        /// <param name="value"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static System.DateTime? SetTime(this System.DateTime? value, string time)
        {
            if (value != null && time != null)
            {
                System.DateTime newDateTime = value.ToDateTime();
                string dateTime = string.Format("{0} {1}", newDateTime.ToShortDateString(), time);
                if (System.DateTime.TryParse(dateTime, out newDateTime))
                    return newDateTime;
                else
                    return value;
            }
            else
                return value;
        }

        public static System.DateTime? SetTime(this System.DateTime? value, int hour, int minute)
        {
            if (value != null)
            {
                string time = string.Format("{0}:{1}", hour, minute);
                System.DateTime newDateTime = value.ToDateTime();
                string dateTime = string.Format("{0} {1}", newDateTime.ToShortDateString(), time);
                if (System.DateTime.TryParse(dateTime, out newDateTime))
                    return newDateTime;
                else
                    return value;
            }
            else
                return value;
        }

        /// <summary>
        /// Use to set the date on a DateTime object without modifying the time. - JHE
        /// </summary>
        /// <param name="value"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? SetDate(this System.DateTime? value, DateTime? date)
        {
            if (value != null)
            {
                System.DateTime newDateTime = value.ToDateTime();
                string dateTime = string.Format("{0} {1}", date.ToDateTime().ToShortDateString(), value.ToDateTime().ToShortTimeString());
                if (System.DateTime.TryParse(dateTime, out newDateTime))
                    return newDateTime;
                else
                    return value;
            }
            else
                return value;
        }

        public static string ToShortDateString(this System.DateTime? value)
        {
            return (value == null) ? string.Empty : value.ToDateTime().ToShortDateString();
        }

        public static string ToMonthYearString(this System.DateTime? value)
        {
            return (value == null) ? string.Empty : string.Format("{0}/{1}", value.ToDateTime().Month.PadWithZeros(2), value.ToDateTime().Year);
        }

        /// <summary>
        /// Returns whether the DateTime is on a Weekend.
        /// </summary>
        /// <param name="dt">Required. The DateTime to evaluate.</param>
        /// <returns>Returns whether the DateTime is on a Weekend.</returns>
        public static bool IsWeekend(this System.DateTime dateTime)
        {
            return (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday);
        }

        /// <summary>
        /// Returns whether the DateTime is on a Week Day.
        /// </summary>
        /// <param name="dt">Required. The DateTime to evaluate.</param>
        /// <returns>Returns whether the DateTime is on a Week Day.</returns>
        public static bool IsWeekDay(this System.DateTime dateTime)
        {
            return !dateTime.IsWeekend();
        }

        public static DateTime JustDate(this System.DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        }
        public static TimeSpan JustTime(this System.DateTime dateTime)
        {
            return new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        public static System.DateTime ToDateTime(this System.DateTime? dateTime)
        {
            return (dateTime == null) ? System.DateTime.MinValue : Convert.ToDateTime(dateTime);
        }

        public static string ToMonthString(this DateTime dateTime)
        {
            return dateTime.ToMonthString(new CultureInfo("en-US"));
        }
        public static string ToMonthString(this DateTime dateTime, CultureInfo cultureInfo)
        {
            return dateTime.ToString("MMMM", cultureInfo);
        }

        // Not tested yet - JHE
        public static DateTime Noon(this DateTime dateTime)
        {
            return (DateTime)((DateTime?)dateTime).SetTime("12:00 pm");
        }


        public static string MonthName(this DateTime dateTime)
        {
            return dateTime.ToString("MMMM");
        }

        public static DateTime NextMonth(this DateTime dateTime)
        {
            return dateTime.AddMonths(1);
        }

        public static DateTime LastMonth(this DateTime dateTime)
        {
            return dateTime.AddMonths(-1);
        }

        public static string Meridiem(this DateTime dateTime)
        {
            string time = dateTime.ToString("t").ToLower();
            if (time.Contains(" "))
                return time.Substring(time.IndexOf(' '));
            else return time;
        }

        public static int GetHour(this DateTime dateTime)
        {
            if (dateTime.Hour == 0)
                return 12;
            else
                return (dateTime.Hour > 12) ? (dateTime.Hour - 12) : dateTime.Hour;
        }

        public static DateTime LocalToUTC(this DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
                return DateTime.MinValue;

            DateTime.SpecifyKind(dateTime, DateTimeKind.Local);

            return dateTime.ToUniversalTime();
        }

        public static DateTime UTCToLocal(this DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
                return DateTime.MinValue;

            DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

            return dateTime.ToLocalTime();
        }


        public static bool IsNullOrEmpty(this System.DateTime dateTime)
        {
            return (dateTime == DateTime.MinValue || dateTime == new DateTime(1900, 1, 1));
        }

        public static bool IsNullOrEmpty(this System.DateTime? dateTime)
        {
            return (dateTime == null || dateTime == DateTime.MinValue || dateTime == new DateTime(1900, 1, 1));
        }


        public static bool IsOneDayOldOrLess(this System.DateTime dateTime)
        {
            return (dateTime >= DateTime.Now.AddDays(-1));
        }

        public static bool IsOneWeekOldOrLess(this System.DateTime dateTime)
        {
            return (dateTime >= DateTime.Now.AddDays(-7));
        }

        public static bool IsOneMonthOldOrLess(this System.DateTime dateTime)
        {
            return (dateTime >= DateTime.Now.AddMonths(-1));
        }

        public static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }
    }
}
