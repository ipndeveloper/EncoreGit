using System;
using System.Globalization;
using NetSteps.Common.Globalization;

namespace NetSteps.Common.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: DateTime Extensions
	/// Created: 11-01-2008
	/// </summary>
	public static class DateTimeExtensions
	{
		#region Validation Methods
		/// <summary>
		/// Returns whether the DateTime is on a Weekend.
		/// </summary>
		/// <param name="dateTime">Required. The DateTime to evaluate.</param>
		/// <returns>Returns whether the DateTime is on a Weekend.</returns>
		public static bool IsWeekend(this System.DateTime dateTime)
		{
			return (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday);
		}

		/// <summary>
		/// Returns whether the DateTime is on a Week Day.
		/// </summary>
		/// <param name="dateTime">Required. The DateTime to evaluate.</param>
		/// <returns>Returns whether the DateTime is on a Week Day.</returns>
		public static bool IsWeekDay(this System.DateTime dateTime)
		{
			return !dateTime.IsWeekend();
		}

		public static bool IsNullOrEmpty(this System.DateTime dateTime)
		{
			return (dateTime == DateTime.MinValue || dateTime == new DateTime(1900, 1, 1));
		}

		public static bool IsNullOrEmpty(this System.DateTime? dateTime)
		{
			return (dateTime == null || dateTime == DateTime.MinValue || dateTime == new DateTime(1900, 1, 1));
		}

		public static bool IsEqualUpToSecond(this System.DateTime? value1, System.DateTime? value2)
		{
			if (value1 == null && value2 == null)
				return true;
			else if (value1 != null && value2 != null)
			{
				DateTime dt1 = (DateTime)value1;
				DateTime dt2 = (DateTime)value2;
				return (DateTime.Compare(new DateTime(dt1.Year, dt1.Month, dt1.Day, dt1.Hour, dt1.Minute, dt1.Second), new DateTime(dt2.Year, dt2.Month, dt2.Day, dt2.Hour, dt2.Minute, dt2.Second)) == 0);
			}
			else
				return false;
		}

		public static bool IsEqualUpToSecond(this System.DateTime dt1, System.DateTime dt2)
		{
			return (DateTime.Compare(new DateTime(dt1.Year, dt1.Month, dt1.Day, dt1.Hour, dt1.Minute, dt1.Second), new DateTime(dt2.Year, dt2.Month, dt2.Day, dt2.Hour, dt2.Minute, dt2.Second)) == 0);
		}
		#endregion

		#region Conversion Methods
		public static string ToShortDateString(this System.DateTime? value)
		{
			return value.ToDateTime().ToShortDateString();
		}
		public static string ToShortDateString(this System.DateTime? value, IFormatProvider culture)
		{
			return value.ToDateTime().ToString("d", culture);
		}
		/// <summary>
		/// Will display "N/A" if the date is null or empty. - JHE
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToShortDateStringDisplay(this System.DateTime? value)
		{
			if (value.IsNullOrEmpty())
				return Translation.GetTerm("N/A");
			else
				return value.ToDateTime().ToShortDateString();
		}
		public static string ToShortDateStringDisplay(this System.DateTime? value, IFormatProvider culture)
		{
			if (value.IsNullOrEmpty())
				return Translation.GetTerm("N/A");
			else
				return value.ToDateTime().ToString("d", culture);
		}
		public static string ToShortDateStringDisplay(this System.DateTime value, IFormatProvider culture)
		{
			if (value.IsNullOrEmpty())
				return Translation.GetTerm("N/A");
			else
				return value.ToString("d", culture);
		}

		public static string ToStringDisplay(this DateTime? value)
		{
			return value.HasValue ? value.ToString() : Translation.GetTerm("N/A");
		}
		public static string ToStringDisplay(this DateTime? value, IFormatProvider culture)
		{
			return value.HasValue ? value.ToDateTime().ToString("F", culture) : "N/A";
		}

		public static string ToMonthYearString(this System.DateTime? value)
		{
			return (value == null) ? string.Empty : string.Format("{0}/{1}", value.ToDateTime().Month.PadWithZeros(2), value.ToDateTime().Year);
		}

		public static string ToFileNameString(this System.DateTime? value)
		{
			return (value == null) ? string.Empty : String.Format("{0:yyyy-MM-dd hh.mm.ss tt}", value);
		}

		public static System.DateTime ToDateTime(this System.DateTime? dateTime)
		{
			return (dateTime == null) ? System.DateTime.MinValue : Convert.ToDateTime(dateTime);
		}

		public static System.DateTime? ToDateTimeNullable(this System.DateTime dateTime)
		{
			return (dateTime == System.DateTime.MinValue) ? null : (System.DateTime?)dateTime;
		}

		public static string ToMonthString(this DateTime dateTime)
		{
			return dateTime.ToMonthString(ApplicationContextCommon.Instance.ApplicationDefaultCultureInfo);
		}
		public static string ToMonthString(this DateTime dateTime, CultureInfo cultureInfo)
		{
			return dateTime.ToString("MMMM", cultureInfo);
		}

		public static string ToExpirationString(this DateTime dateTime)
		{
			return dateTime.ToString("MM/yyyy", ApplicationContextCommon.Instance.ApplicationDefaultCultureInfo);
		}

		public static string ToExpirationStringDisplay(this DateTime? dateTime, IFormatProvider culture)
		{
			if (dateTime == null)
				return Translation.GetTerm("N/A");
			else
				return dateTime.ToDateTime().ToExpirationStringDisplay(culture);
		}
		public static string ToExpirationStringDisplay(this DateTime dateTime, IFormatProvider culture)
		{
			if (culture is CultureInfo && (culture as CultureInfo).IsUnitedStates())
				return dateTime.ToString("MM/yyyy", culture);
			else
				return dateTime.ToString("y", culture);
		}


		public static string ToShortTimeString(this System.DateTime? value)
		{
			if (value == null)
				return string.Empty;
			else
				return value.ToDateTime().ToShortTimeString();
		}
		public static string ToShortTimeStringDisplay(this System.DateTime? value)
		{
			if (value == null)
				return Translation.GetTerm("N/A");
			else
				return value.ToDateTime().ToShortTimeString();
		}
		#endregion

		/// <summary>
		/// Use to set the time on a DateTime object without modifying the date. - JHE
		/// </summary>
		/// <param name="value"></param>
		/// <param name="time"></param>
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

		public static DateTime AddTime(this DateTime date, DateTime time)
		{
			return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second, time.Millisecond);
		}

		public static DateTime AddTime(this DateTime date, DateTime? time)
		{
			if (time.HasValue)
				return date.AddTime(time.Value);
			else
				return date;
		}

		public static DateTime? AddTime(this DateTime? date, DateTime? time, bool returnNullIfNoDate = true)
		{
			if (date.HasValue)
			{
				return date.Value.AddTime(time);
			}
			else if (time.HasValue && !returnNullIfNoDate)
				return time;
			return null;
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

		public static DateTime JustDate(this System.DateTime dateTime)
		{
			return dateTime.Date;
		}
		public static TimeSpan JustTime(this System.DateTime dateTime)
		{
			return new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second);
		}

		// Not tested yet - JHE
		public static DateTime Noon(this DateTime dateTime)
		{
			return dateTime.AddTime(new DateTime(1900, 1, 1, 12, 0, 0));
		}

		/// <summary>
		/// Gets a DateTime representing midnight on the current date
		/// </summary>
		/// <param name="dateTime">The current date</param>
		public static DateTime Midnight(this System.DateTime dateTime)
		{
			return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
		}
		public static DateTime? Midnight(this System.DateTime? dateTime)
		{
			if (dateTime.HasValue)
			{
				DateTime castDateTime = dateTime.Value;
				return new DateTime(castDateTime.Year, castDateTime.Month, castDateTime.Day);
			}
			return null;
		}

		public static string MonthName(this DateTime dateTime)
		{
			return dateTime.ToString("MMMM");
		}
		public static string MonthName(this DateTime dateTime, IFormatProvider culture)
		{
			return dateTime.ToString("MMMM", culture);
		}

		public static string ShortMonthName(this DateTime dateTime)
		{
			return dateTime.ToString("MMM");
		}
		public static string ShortMonthName(this DateTime dateTime, IFormatProvider culture)
		{
			return dateTime.ToString("MMM", culture);
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
			return time.Substring(time.IndexOf(' '));
		}

		public static int GetHour(this DateTime dateTime)
		{
			if (dateTime.Hour == 0)
				return 12;
			else
				return (dateTime.Hour > 12) ? (dateTime.Hour - 12) : dateTime.Hour;
		}


		/// <summary>
		/// This is to fix the Leap year problem when setting a date to 2/29/**** 
		/// Date time currently throws an exception - JHE
		/// </summary>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <param name="day"></param>
		/// <returns></returns>
		public static DateTime GetSafeDate(int year, int month, int day)
		{
			if (month == 2 && day > 28)
				day = 28;

			return new DateTime(year, month, day);
		}

		public static DateTime? AddHours(this System.DateTime? dateTime, double value)
		{
			if (dateTime != null)
			{
				DateTime castDateTime = dateTime.ToDateTime();
				return castDateTime.AddHours(value);
			}
			return null;
		}

		/// <summary>
		/// Gets a DateTime representing midnight on the first day of the current date's month
		/// </summary>
		/// <param name="dateTime">The current date</param>
		/// <returns></returns>
		public static DateTime StartOfMonth(this System.DateTime dateTime)
		{
			return dateTime.FirstDayOfMonth();
		}

		public static DateTime StartOfDay(this System.DateTime value)
		{
			return value.Midnight();
		}
		public static DateTime? StartOfDay(this System.DateTime? value)
		{
			return value.Midnight();
		}

		public static DateTime EndOfDay(this System.DateTime value)
		{
			return value.Midnight().AddHours(24).AddSeconds(-1);
		}
		public static DateTime? EndOfDay(this System.DateTime? value)
		{
			if (value == null)
				return null;
			else
				return value.ToDateTime().Midnight().AddHours(24).AddSeconds(-1);
		}

		public static DateTime? LocalToUTC(this DateTime? localDateTime)
		{
			if (localDateTime == null)
				return null;

			DateTime.SpecifyKind(localDateTime.ToDateTime(), DateTimeKind.Local);

			return (DateTime?)localDateTime.ToDateTime().ToUniversalTime();

		}
		public static DateTime LocalToUTC(this DateTime localDateTime)
		{
			if (localDateTime == DateTime.MinValue)
				return DateTime.MinValue;

			DateTime.SpecifyKind(localDateTime, DateTimeKind.Local);

			return localDateTime.ToUniversalTime();
		}

		public static DateTime? LocalToUTC(this DateTime? localDateTime, TimeZone timeZone)
		{
			if (localDateTime == null)
				return null;

			if (localDateTime == DateTime.MinValue)
				return DateTime.MinValue;

			if (localDateTime.HasValue)
				return timeZone.ToUniversalTime(localDateTime.Value);
			else
				return null;
		}
		public static DateTime LocalToUTC(this DateTime localDateTime, TimeZone timeZone)
		{
			if (localDateTime == DateTime.MinValue)
				return DateTime.MinValue;

			return timeZone.ToUniversalTime(localDateTime);
		}


		public static DateTime? LocalToUTC(this DateTime? localDateTime, TimeZoneInfo timeZone)
		{
			if (localDateTime == null)
				return null;

			if (localDateTime == DateTime.MinValue)
				return DateTime.MinValue;

			if (localDateTime.HasValue)
			{
				DateTime dateTimeValue = DateTime.SpecifyKind(localDateTime.Value, DateTimeKind.Unspecified);
				return TimeZoneInfo.ConvertTimeToUtc(dateTimeValue, timeZone);
			}
			else
				return null;
		}
		public static DateTime LocalToUTC(this DateTime localDateTime, TimeZoneInfo timeZone)
		{
			if (localDateTime == DateTime.MinValue)
				return DateTime.MinValue;

			if (localDateTime.Kind == DateTimeKind.Local)
				return TimeZoneInfo.ConvertTimeToUtc(localDateTime, TimeZoneInfo.Local);

			return TimeZoneInfo.ConvertTimeToUtc(localDateTime, timeZone);
		}


		public static DateTime UTCToLocal(this DateTime? universalTime)
		{
			return universalTime.ToDateTime().UTCToLocal();
		}
		public static DateTime UTCToLocal(this DateTime universalTime)
		{
			if (universalTime == DateTime.MinValue)
				return DateTime.MinValue;

			DateTime.SpecifyKind(universalTime, DateTimeKind.Utc);

			return universalTime.ToLocalTime();
		}

		public static DateTime? UTCToLocal(this DateTime? universalTime, TimeZone timeZone)
		{
			if (universalTime == null)
				return null;

			if (universalTime == DateTime.MinValue)
				return DateTime.MinValue;

			return timeZone.ToLocalTime(universalTime.ToDateTime());
		}
		public static DateTime UTCToLocal(this DateTime universalTime, TimeZone timeZone)
		{
			if (universalTime == DateTime.MinValue)
				return DateTime.MinValue;

			return timeZone.ToLocalTime(universalTime);
		}

		public static DateTime? UTCToLocal(this DateTime? universalTime, TimeZoneInfo timeZone)
		{
			if (universalTime == null)
				return null;

			if (universalTime == DateTime.MinValue)
				return DateTime.MinValue;

			return TimeZoneInfo.ConvertTimeFromUtc(universalTime.Value, timeZone);
		}
		public static DateTime UTCToLocal(this DateTime universalTime, TimeZoneInfo timeZone)
		{
			if (universalTime == DateTime.MinValue)
				return DateTime.MinValue;

			return TimeZoneInfo.ConvertTimeFromUtc(universalTime, timeZone);
		}




		public static bool IsOlderThan(this DateTime dateTime, TimeSpan timeSpan)
		{
			return (dateTime.Add(timeSpan) < DateTime.Now.ApplicationNow());
		}

		public static DateTime FirstDayOfYear(this DateTime dateTime)
		{
			return new DateTime(dateTime.Year, 1, 1);
		}

		public static DateTime LastDayOfYear(this DateTime dateTime)
		{
			return new DateTime(dateTime.Year, 12, 31);
		}

		public static DateTime FirstDayOfMonth(this DateTime dateTime)
		{
			return new DateTime(dateTime.Year, dateTime.Month, 1);
		}

		public static DateTime LastDayOfMonth(this DateTime dateTime)
		{
			return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
		}

		public static DateTime FirstDayOfWeek(this DateTime dateTime)
		{
			return dateTime.Date.AddDays(-(dateTime.DayOfWeek - DayOfWeek.Sunday));
		}

		public static DateTime LastDayOfWeek(this DateTime dateTime)
		{
			return dateTime.Date.AddDays(DayOfWeek.Saturday - dateTime.DayOfWeek);
		}

		public static bool IsBetween(this DateTime dateTime, DateTime startDate, DateTime endDate)
		{
			return dateTime >= startDate && dateTime <= endDate;
		}

		public static bool IsBetween(this DateTime dateTime, DateTime? startDate, DateTime endDate)
		{
			return (!startDate.HasValue || dateTime >= startDate) && dateTime <= endDate;
		}

		public static bool IsBetween(this DateTime dateTime, DateTime startDate, DateTime? endDate)
		{
			return dateTime >= startDate && (!endDate.HasValue || dateTime <= endDate);
		}

		public static bool IsBetween(this DateTime dateTime, DateTime? startDate, DateTime? endDate)
		{
			return (!startDate.HasValue || dateTime >= startDate) && (!endDate.HasValue || dateTime <= endDate);
		}

		public static bool IsBetweenExclusive(this DateTime dateTime, DateTime startDate, DateTime endDate)
		{
			return dateTime > startDate && dateTime < endDate;
		}

		public static bool IsBetweenExclusive(this DateTime dateTime, DateTime? startDate, DateTime endDate)
		{
			return (!startDate.HasValue || dateTime > startDate) && dateTime < endDate;
		}

		public static bool IsBetweenExclusive(this DateTime dateTime, DateTime startDate, DateTime? endDate)
		{
			return dateTime > startDate && (!endDate.HasValue || dateTime < endDate);
		}

		public static bool IsBetweenExclusive(this DateTime dateTime, DateTime? startDate, DateTime? endDate)
		{
			return (!startDate.HasValue || dateTime > startDate) && (!endDate.HasValue || dateTime < endDate);
		}

		public static DateTime ApplicationNow(this DateTime dateTime)
		{
			if (NetSteps.Common.ApplicationContextCommon.Instance.IsDateTimeNowSet())
			{
				return NetSteps.Common.ApplicationContextCommon.Instance.DateTimeNow;
			}
			else
			{
				return dateTime;
			}
		}

		/// <summary>
		/// Returns the total number of months since 0/0/0000.
		/// </summary>
		public static int TotalMonths(this DateTime date)
		{
			return (date.Year * 12) + date.Month;
		}
	}
}
