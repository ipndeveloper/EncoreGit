using System;
using System.Globalization;

namespace NetSteps.Common.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Double Extensions
	/// Created: 11-01-2008
	/// </summary>
	public static class DoubleExtensions
	{
		#region Validation Methods
		public static bool IsPositive(this double value)
		{
			return (value > 0);
		}
		#endregion

		#region Conversion Methods
		public static int ToInt(this double value)
		{
			return Convert.ToInt32(value);
		}

		public static string ToMoneyString(this double value)
		{
			return value.ToMoneyString(new CultureInfo("en-US", false));
		}
		public static string ToMoneyString(this double value, CultureInfo cultureInfo)
		{
			return value.ToString("c", cultureInfo);
		}
		#endregion

		public static string TruncateDoubleInsertCommas(this double value, int decimalPoints = 0)
		{
			string returnString = string.Empty;
			int decimalStartPos = 0;

			returnString = string.Format("{0:n}", value);
			decimalStartPos = returnString.IndexOf(".");

			if (decimalStartPos > 0 && (returnString.Length > decimalStartPos + decimalPoints))
				returnString = returnString.Substring(0, decimalStartPos + decimalPoints + 1);

			if (returnString.EndsWith("."))
				returnString = returnString.Substring(0, returnString.Length - 1);

			return returnString;
		}

		/// <summary>
		///  Returns a positive value representing the difference between 2 double values. - JHE
		/// </summary>
		/// <param name="value"></param>
		/// <param name="otherValue"></param>
		/// <returns></returns>
		public static double GetDelta(this double value, double otherValue)
		{
			double delta = value - otherValue;
			return (delta < 0) ? delta * -1 : delta;
		}

		/// <summary>
		/// http://blogs.msdn.com/avip/archive/2008/09/30/trivial-but-useful-extension-method.aspx
		/// </summary>
		/// <param name="num"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static double Constrain(this double num, double min, double max)
		{
			if (num < min)
				return min;
			else if (num > max)
				return max;
			else
				return num;
		}

		public static double DegreesToRadians(this double degrees)
		{
			double radians = ((degrees / 360) * 2 * Math.PI);
			return radians;
		}

		public static decimal ToDecimal(this double value)
		{
			return (decimal)value;
		}

		public static decimal ToDecimal(this double? value)
		{
			return value.HasValue ? (decimal)value.Value : 0M;
		}
	}
}
