using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetSteps.Common.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Enum Extensions
	/// Created: 11-01-2008
	/// </summary>
	public static class EnumExtensions
	{
		#region Conversion Methods
		/// <summary>
		/// To return the integer value of the Enum as a string. - JHE
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToIntString(this Enum value)
		{
			return Convert.ToInt32(value).ToString();
		}

		/// <summary>
		/// To return the integer value of an Enum. - JHE
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static int ToInt(this Enum value)
		{
			return Convert.ToInt32(value);
		}

		public static Int16 ToInt16(this Enum value)
		{
			return Convert.ToInt16(value);
		}

		public static Int16 ToShort(this Enum value)
		{
			return Convert.ToInt16(value);
		}

		public static Int16? ToShortNullable(this Enum value)
		{
			if (Convert.ToInt16(value) == 0)
				return null;
			else
				return Convert.ToInt16(value);
		}

		public static Byte ToByte(this Enum value)
		{
			return Convert.ToByte(value);
		}
		#endregion

		/// <summary>
		/// Takes a Pascal CASED string and inserts spaces:
		/// Example: "PascalCaseString" becomes "Pascal Case String" - JHE
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string PascalToSpaced(this Enum value)
		{
			return value.ToString().PascalToSpaced();
		}

		public static IList<T> GetValues<T>(this Enum enumeration)
		{
			return GetValues<T>();
		}
		public static IList<T> GetValues<T>()
		{
			Type enumType = typeof(T);
			if (!enumType.IsEnum)
				throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");

			IList<T> values = new List<T>();

			foreach (FieldInfo field in enumType.GetFields().Where(f => f.IsLiteral))
			{
				object value = field.GetValue(enumType);
				values.Add((T)value);
			}

			return values;
		}

		public static IEnumerable<T> GetValues<T>(this Type enumType)
		{
			if (!enumType.IsEnum)
				throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
			return Enum.GetNames(enumType).Select(t => (T)Enum.Parse(enumType, t));
		}

		//public static IEnumerable<T> GetValues<T>(this Enum e)
		//{
		//    Type enumType = e.GetType();
		//    return Enum.GetNames(enumType).Select(t => (T)Enum.Parse(enumType, t));
		//}

		public static IList<EnumNameValue> GetValuesList<T>(this Enum enumeration)
		{
			return GetValuesList<EnumNameValue>();
		}
		public static IList<EnumNameValue> GetValuesList<T>()
		{
			Type enumType = typeof(T);
			if (!enumType.IsEnum)
				throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");

			IList<EnumNameValue> values = new List<EnumNameValue>();

			foreach (FieldInfo field in enumType.GetFields().Where(f => f.IsLiteral))
			{
				object value = field.GetValue(enumType);
				values.Add(new EnumNameValue() { Name = ((T)value).ToString().PascalToSpaced(), Value = ((T)value as Enum).ToInt() });
			}

			return values;
		}

		public static T GetRandom<T>(this Enum value, bool excludeFirst)
		{
			IList<T> values = value.GetValues<T>();

			if (excludeFirst)
				return values.GetRandom(1);
			else
				return values.GetRandom(0);
		}
	}

	public class EnumNameValue
	{
		public string Name { get; set; }
		public int Value { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
