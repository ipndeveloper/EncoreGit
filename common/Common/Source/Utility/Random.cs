using NetSteps.Common.Extensions;

namespace NetSteps.Common
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Wrapper class for the Random class to extend it's functionality. 
	/// Created: 02-05-2009
	/// </summary>
	public static class Random
	{
		private static System.Random random = new System.Random((int)System.DateTime.Now.Ticks);

		public static int Next()
		{
			return random.Next();
		}

		public static int Next(int maxValue)
		{
			return random.Next(maxValue);
		}

		public static int Next(int minValue, int maxValue)
		{
			return random.Next(minValue, maxValue);
		}

		public static bool GetBoolean()
		{
			return random.Next(2).ToBool();
		}

		public static System.DateTime GetBirthday()
		{
			return GetDateTime(System.DateTime.Now.ApplicationNow().AddYears(-80), System.DateTime.Now.ApplicationNow().AddYears(-10));
		}

        public static string GetString(int length)
        {
            return System.Guid.NewGuid().ToString("N").Substring(0, length > 32 ? 32 : length);
        }
		/// <summary>
		/// This function is not perfect but works ok for now - JHE
		/// </summary>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static System.DateTime GetDateTime(System.DateTime minValue, System.DateTime maxValue)
		{
			var timeSpan = maxValue - minValue;
			return minValue.AddDays((double)random.Next(1, (int)timeSpan.TotalDays - 1)).AddSeconds(random.Next(1, 86400));
		}
	}
}
