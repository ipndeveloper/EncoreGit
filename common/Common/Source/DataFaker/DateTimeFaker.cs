using NetSteps.Common.Extensions;

namespace NetSteps.Common.DataFaker
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Methods to generate 'Fake' data for use in testing.
	/// Created: 03-18-2009
	/// </summary>
	public static class DateTimeFaker
	{
		public static System.DateTime DateTime(System.DateTime from, System.DateTime to)
		{
			var timeSpan = to - from;
			return from.AddDays((double)Random.Next(1, (int)timeSpan.TotalDays - 1)).AddSeconds(Random.Next(1, 86400));
		}

		public static System.DateTime DateTime()
		{
			return DateTimeBetweenDays(5);
		}

		public static System.DateTime DateTimeBetweenDays(double fromDays, double toDays)
		{
			return DateTime(System.DateTime.Now.ApplicationNow().AddDays(-1 * fromDays), System.DateTime.Now.ApplicationNow().AddDays(toDays));
		}

		public static System.DateTime DateTimeBetweenDays(double days)
		{
			return DateTime(System.DateTime.Now.ApplicationNow().AddDays(-1 * days), System.DateTime.Now.ApplicationNow().AddDays(days));
		}

		public static System.DateTime DateTimeBetweenMonths(int fromMonths, int toMonths)
		{
			return DateTime(System.DateTime.Now.ApplicationNow().AddMonths(-1 * fromMonths), System.DateTime.Now.ApplicationNow().AddMonths(toMonths));
		}

		public static System.DateTime DateTimeBetweenMonths(int months)
		{
			return DateTime(System.DateTime.Now.ApplicationNow().AddMonths(-1 * months), System.DateTime.Now.ApplicationNow().AddMonths(months));
		}

		public static System.DateTime DateTimeBetweenYears(int fromYears, int toYears)
		{
			return DateTime(System.DateTime.Now.ApplicationNow().AddYears(-1 * fromYears), System.DateTime.Now.ApplicationNow().AddYears(toYears));
		}

		public static System.DateTime DateTimeBetweenYears(int years)
		{
			return DateTime(System.DateTime.Now.ApplicationNow().AddYears(-1 * years), System.DateTime.Now.ApplicationNow().AddDays(years));
		}

		public static System.DateTime BirthDay(int minAge, int maxAge)
		{
			return DateTimeBetweenYears(maxAge, -1 * minAge);
		}

		public static System.DateTime BirthDay(int minAge)
		{
			return DateTimeBetweenYears(100, -1 * minAge);
		}

		public static System.DateTime BirthDay()
		{
			return BirthDay(18);
		}
	}
}
