using System;
using System.Data;
using System.Globalization;
using NetSteps.Common.Exceptions;

namespace NetSteps.Common.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: IDataRecord Extensions
	/// Created: 04-21-2010
	/// </summary>
	public static class IDataRecordExtensions
	{
		// TODO: Provide overload for index parameter for speed
		public static Int64 GetInt64(this IDataRecord reader, string column)
		{
			Int64 defaultValue = 0;
			int index = reader.GetColumnIndex(column);
			return (reader.IsDBNull(index)) ? defaultValue : Convert.ToInt64(reader[column]);
		}

		public static int GetInt32(this IDataRecord reader, string column)
		{
			int defaultValue = 0;
			int index = reader.GetColumnIndex(column);
			return (reader.IsDBNull(index)) ? defaultValue : Convert.ToInt32(reader[column]);
		}

		public static int? GetNullableInt32(this IDataRecord reader, string column)
		{
			int i;
			if (reader.IsDBNull(GetColumnIndex(reader, column)) || !int.TryParse(reader[column].ToString(), out i))
				return (int?)null;
			return i;
		}

		public static short GetInt16(this IDataRecord reader, string column)
		{
			short defaultValue = 0;
			int index = reader.GetColumnIndex(column);
			return (reader.IsDBNull(index)) ? defaultValue : Convert.ToInt16(reader[column]);
		}

		public static byte[] GetBytes(this IDataRecord reader, string column)
		{
			//System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
			//return encoding.GetBytes((string)reader[column]);

			byte[] defaultValue = null;
			int index = reader.GetColumnIndex(column);
			return (reader.IsDBNull(index)) ? defaultValue : (byte[])reader.GetValue(reader.GetOrdinal(column));
		}

		public static float GetFloat(this IDataRecord reader, string column)
		{
			float defaultValue = 0;
			int index = reader.GetColumnIndex(column);
			return (reader.IsDBNull(index)) ? defaultValue : float.Parse(reader[column].ToString(), CultureInfo.InvariantCulture);
		}

		public static double GetDouble(this IDataRecord reader, string column)
		{
			double defaultValue = 0;
			int index = reader.GetColumnIndex(column);
			return (reader.IsDBNull(index)) ? defaultValue : double.Parse(reader[column].ToString(), CultureInfo.InvariantCulture);
		}

		public static decimal GetDecimal(this IDataRecord reader, string column)
		{
			decimal defaultValue = 0;
			int index = reader.GetColumnIndex(column);
			return (reader.IsDBNull(index)) ? defaultValue : decimal.Parse(reader[column].ToString(), CultureInfo.InvariantCulture);
		}

		public static bool GetBoolean(this IDataRecord reader, string column)
		{
			bool defaultValue = false;
			int index = reader.GetColumnIndex(column);
			return (reader.IsDBNull(index)) ? defaultValue : Convert.ToBoolean(reader[column]);
		}

		public static String GetString(this IDataRecord reader, string column)
		{
			string defaultValue = string.Empty;
			int index = reader.GetColumnIndex(column);
			return (reader.IsDBNull(index)) ? defaultValue : reader[column].ToString().Trim();
		}

		public static Char GetChar(this IDataRecord reader, string column)
		{
			char defaultValue = ' ';
			int index = reader.GetColumnIndex(column);
			return (reader.IsDBNull(index)) ? defaultValue : char.Parse(reader[column].ToString());
		}

		public static DateTime GetDateTime(this IDataRecord reader, string column)
		{
			DateTime defaultValue = DateTime.MinValue;
			int index = reader.GetColumnIndex(column);
			return ((reader.IsDBNull(index)) || string.IsNullOrEmpty(reader[column].ToString())) ? defaultValue : DateTime.Parse(reader[column].ToString());
		}

		public static DateTime? GetDateTimeNullable(this IDataRecord reader, string column)
		{
			DateTime defaultValue = DateTime.MinValue;
			int index = reader.GetColumnIndex(column);
			DateTime? returnValue = ((reader.IsDBNull(index)) || string.IsNullOrEmpty(reader[column].ToString())) ? defaultValue : (DateTime)reader[column];

			if (returnValue == DateTime.MinValue)
				returnValue = null;

			return returnValue;
		}

		public static T GetEnum<T>(this IDataRecord reader, string column)
		{
			T defaultValue = (T)Enum.GetValues(typeof(T)).GetValue(0); // This sets the default to the first item of the enum...
			int index = reader.GetColumnIndex(column);
			return (reader.IsDBNull(index)) ? defaultValue : (T)reader[column];
		}

		public static Object GetObject(this IDataRecord reader, string column)
		{
			Object defaultValue = null;
			int index = reader.GetColumnIndex(column);
			return (reader.IsDBNull(index)) ? defaultValue : reader[column];
		}

		public static bool ColumnExists(this IDataRecord reader, string column)
		{
			try
			{
				int index = reader.GetColumnIndex(column);
			}
			catch (ColumnNotInReaderException)
			{
				return false;
			}

			return true;
		}

		public static int GetColumnIndex(this IDataRecord reader, string column)
		{
			for (int i = 0; i < reader.FieldCount; i++)
			{
				if (reader.GetName(i).Equals(column, StringComparison.InvariantCultureIgnoreCase))
					return i;
			}
			throw new ColumnNotInReaderException("The column \"" + column + "\" is not part of the reader's data record.");
			//return -1;
		}
	}
}
