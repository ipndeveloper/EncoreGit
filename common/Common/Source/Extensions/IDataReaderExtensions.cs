using System;
using System.Data;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: IDataReader Extensions
    /// Created: 4-28-2009
    /// </summary>
    public static class IDataReaderExtensions
    {
        public static string GetStringSafe(this IDataReader dataReader, string name)
        {
            try
            {
                return (dataReader.IsDBNull(dataReader.GetOrdinal(name)) ? string.Empty : Convert.ToString(dataReader[name]));
            }
            catch
            {
                return string.Empty;
            }
        }

        public static bool GetBooleanSafe(this IDataReader dataReader, string columnName, bool defaultValue=default(bool))
        {
            try
            {
                return (dataReader.IsDBNull(dataReader.GetOrdinal(columnName)) ? defaultValue : dataReader.GetBoolean(columnName));
            }
            catch 
            {
                return defaultValue;
            }
        }

        public static int GetInt32Safe(this IDataReader dataReader, string columnName)
        {
            try
            {
                return (dataReader.IsDBNull(dataReader.GetOrdinal(columnName)) ? default(int) : Convert.ToInt32(dataReader[columnName]));
            }
            catch
            {
                return default(int);
            }
        }
    }
}
