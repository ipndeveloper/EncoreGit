using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NetSteps.Modules.Enrollments.Common
{
	/// <summary>
	/// Generall Extension Methods
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Check to see if a string is null or empty
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty(this string value)
		{
			string val = (value == null) ? string.Empty : value.Trim();
			return string.IsNullOrEmpty(val);
		}

		/// <summary>
		/// Clean a string
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToCleanString(this string value)
		{
			return (value == null) ? string.Empty : value.Trim();
		}

		/// <summary>
		/// Remove any non-numeric characters from a string
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string RemoveNonNumericCharacters(this string text)
		{
			if (text.IsNullOrEmpty())
				return string.Empty;

			return Regex.Replace(text.ToCleanString(), @"\D", string.Empty);
		}

		/// <summary>
		/// Used to check against weak passwords. 
		/// For input: 'secret' it returns '123456' - JHE
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string GetIncrementalNumberStringEquivalent(this string text)
		{
			string returnValue = string.Empty;
			for (int i = 1; i <= text.Length; i++)
				returnValue += i.ToString();
			return returnValue;
		}

		/// <summary>
		/// Get the last day of the month for a given date.
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static DateTime LastDayOfMonth(this DateTime dateTime)
		{
			return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
		}

		/// <summary>
		/// Add a range of items to a list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="itemsToAdd"></param>
		public static void AddRange<T>(this IList<T> list, IEnumerable<T> itemsToAdd)
		{
			if (itemsToAdd != null)
			{
				foreach (T item in itemsToAdd)
					list.Add(item);
			}
		}
	}
}
