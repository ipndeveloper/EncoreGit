using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace TestMasterHelpProvider
{
	public static class DataAccess
	{
		private static string _defaultConnectionStringName = "TestConnection";
		public static string DefaultConnectionStringName
		{
			get { return _defaultConnectionStringName; }
		}

		#region --- Constructors ---

		//static DataAccess()
		//{
		//    if (AutoUpdateSchema.Mode == AutoUpdateSchemaMode.On) { AutoUpdateSchema.UpdateSchema(); }
		//}

		#endregion

		internal static string GetConnectionString(string connectionStringName)
		{
			return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
		}

		/// <summary>
		/// This will return the Default Connection String for "Sites"
		/// </summary>
		internal static string ConnectionString
		{
			get { return GetConnectionString(DefaultConnectionStringName); }
		}

		internal static string ConnectionADString
		{
			get { return ConfigurationManager.AppSettings["connectionActiveDirectory"]; }
		}

		/// <summary>
		/// This is the Default Connection using the "Sites" Connection String Name
		/// </summary>
		/// <param name="commandValue"></param>
		/// <returns></returns>
		public static SqlCommand SetCommand(string commandValue)
		{
			return SetCommand(commandValue, DefaultConnectionStringName);
		}

		/// <summary>
		/// Use This Method to override the Default Connection String Name
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="commandValue"></param>
		/// <returns></returns>
		public static SqlCommand SetCommand(string commandValue, string connectionStringName)
		{
			SqlConnection sqlConnection = new SqlConnection(GetConnectionString(connectionStringName));
			SqlCommand sqlCommand = new SqlCommand(commandValue, sqlConnection)
			{
				CommandType = CommandType.StoredProcedure,
				Connection = sqlConnection
			};
			return sqlCommand;
		}

		internal static void AddInputParameter(string parameterName, object parameterValue, SqlCommand sqlCommand)
		{
			sqlCommand.Parameters.AddWithValue("@" + parameterName, parameterValue);
		}

		internal static void AddInputParameterCheckNull(string parameterName, object parameterValue, SqlCommand sqlCommand)
		{
			if (parameterValue is string)
			{
				if (string.IsNullOrEmpty(parameterValue as string))
					DataAccess.AddInputParameter(parameterName, DBNull.Value, sqlCommand);
				else
					DataAccess.AddInputParameter(parameterName, parameterValue as string, sqlCommand);
			}
			else if (parameterValue is DateTime?)
			{
				if ((parameterValue as DateTime?) == null || (parameterValue as DateTime?) == DateTime.MinValue)
					DataAccess.AddInputParameter(parameterName, DBNull.Value, sqlCommand);
				else
					DataAccess.AddInputParameter(parameterName, parameterValue as DateTime?, sqlCommand);
			}
			else if (parameterValue is int)
			{
				if (Convert.ToInt32(parameterValue) == 0)
					DataAccess.AddInputParameter(parameterName, DBNull.Value, sqlCommand);
				else
					DataAccess.AddInputParameter(parameterName, parameterValue, sqlCommand);
			}
			else if (parameterValue == null)
			{
				DataAccess.AddInputParameter(parameterName, DBNull.Value, sqlCommand);
			}
		}

		internal static void AddStringOutputParameter(string parameterName, SqlCommand sqlCommand)
		{
			sqlCommand.Parameters.Add("@" + parameterName, SqlDbType.NVarChar, 500);
			sqlCommand.Parameters["@" + parameterName].Direction = ParameterDirection.Output;
		}

		internal static void AddOutputParameter(string parameterName, SqlCommand sqlCommand)
		{
			sqlCommand.Parameters.Add("@" + parameterName, SqlDbType.Int, 4);
			sqlCommand.Parameters["@" + parameterName].Direction = ParameterDirection.Output;
		}

		internal static void AddMoneyOutputParameter(string parameterName, SqlCommand sqlCommand)
		{
			sqlCommand.Parameters.Add("@" + parameterName, SqlDbType.Money);
			sqlCommand.Parameters["@" + parameterName].Direction = ParameterDirection.Output;
		}

		internal static void AddInputOutputParameter(string parameterName, object parameterValue, SqlCommand sqlCommand)
		{
			sqlCommand.Parameters.AddWithValue("@" + parameterName, parameterValue);
			sqlCommand.Parameters["@" + parameterName].Direction = ParameterDirection.InputOutput;
		}

		internal static void AddReturnParameter(string parameterName, SqlCommand sqlCommand)
		{
			sqlCommand.Parameters.Add("@" + parameterName, SqlDbType.Int, 4);
			sqlCommand.Parameters["@" + parameterName].Direction = ParameterDirection.ReturnValue;
		}

		internal static int GetInt32ReturnValue(string parameterName, SqlCommand sqlCommand)
		{
			if (sqlCommand.Parameters["@" + parameterName].Value.ToString().Length == 0)
			{
				return 0;
			}
			return Int32.Parse(sqlCommand.Parameters["@" + parameterName].Value.ToString(), CultureInfo.InvariantCulture);
		}

		internal static Int64 GetInt64ReturnValue(string parameterName, SqlCommand sqlCommand)
		{
			if (sqlCommand.Parameters["@" + parameterName].Value.ToString().Length == 0)
			{
				return 0;
			}
			return Int64.Parse(sqlCommand.Parameters["@" + parameterName].Value.ToString(), CultureInfo.InvariantCulture);
		}

		internal static Decimal GetDecimalReturnValue(string parameterName, SqlCommand sqlCommand)
		{
			if (sqlCommand.Parameters["@" + parameterName].Value.ToString().Length == 0)
			{
				return 0;
			}
			return Decimal.Parse(sqlCommand.Parameters["@" + parameterName].Value.ToString(), CultureInfo.InvariantCulture);
		}

		internal static string GetStringReturnValue(string parameterName, SqlCommand sqlCommand)
		{
			if (sqlCommand.Parameters["@" + parameterName].Value.ToString().Length == 0)
			{
				return string.Empty;
			}
			return sqlCommand.Parameters["@" + parameterName].Value.ToString();
		}

		internal static byte GetByteReturnValue(string parameterName, SqlCommand sqlCommand)
		{
			if (string.IsNullOrEmpty(parameterName))
			{
				return 0;
			}
			return Byte.Parse(sqlCommand.Parameters["@" + parameterName].Value.ToString(), CultureInfo.InvariantCulture);
		}

		public static IDataReader ExecuteReader(SqlCommand sqlCommand)
		{
			try
			{
				using (sqlCommand)
				{
					sqlCommand.Connection.Open();
					return sqlCommand.ExecuteReader();
				}
			}
			catch (Exception ex)
			{
				Close(sqlCommand);
				throw; //TODO: Custom Error
			}
		}

		public static int ExecuteNonQuery(SqlCommand sqlCommand)
		{
			try
			{
				using (sqlCommand)
				{
					sqlCommand.Connection.Open();
					return sqlCommand.ExecuteNonQuery();
				}
			}
			catch
			{
				Close(sqlCommand);
				throw; //TODO: Custom Error
			}
		}

		internal static int ExecuteNonQuery(string inSQL, string connectionStringName)
		{
			SqlConnection sqlConnection = new SqlConnection(GetConnectionString(connectionStringName));
			SqlCommand sqlCommand = new SqlCommand(inSQL, sqlConnection)
			{
				CommandType = CommandType.Text,
				Connection = sqlConnection
			};
			return DataAccess.ExecuteNonQuery(sqlCommand);
		}

		public static int ExecuteNonQuery(string inSQL)
		{
			return ExecuteNonQuery(inSQL, DefaultConnectionStringName);
		}

		internal static DataTable GetDataTable(SqlCommand sqlCommand)
		{
			try
			{
				using (sqlCommand)
				{
					DataSet dataSet = new DataSet();
					SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

					adapter.Fill(dataSet);
					adapter.Dispose();
					return dataSet.Tables.Count > 0 ? dataSet.Tables[0] : null;
				}
			}
			finally
			{
				Close(sqlCommand);
			}
		}

		internal static DataSet GetDataSet(SqlCommand sqlCommand)
		{
			try
			{
				using (sqlCommand)
				{
					DataSet dataSet = new DataSet();
					SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

					adapter.Fill(dataSet);
					adapter.Dispose();
					return dataSet;
				}
			}
			finally
			{
				Close(sqlCommand);
			}
		}

		internal static DataSet GetDataSet(SqlCommand sqlCommand, string connectionStringName)
		{
			SqlConnection sqlConnection = new SqlConnection(GetConnectionString(connectionStringName));
			sqlCommand.Connection = sqlConnection;

			return DataAccess.GetDataSet(sqlCommand);
		}

		internal static object GetScalarValue(SqlCommand sqlCommand)
		{
			try
			{
				using (sqlCommand)
				{
					if (sqlCommand.Connection.State == ConnectionState.Closed)
					{
						sqlCommand.Connection.Open();
					}
					return sqlCommand.ExecuteScalar();
				}
			}
			finally
			{
				Close(sqlCommand);
			}
		}

		public static string GetValue(string inSQL)
		{
			string returnVal = string.Empty;
			DataSet ds = GetDataSet(inSQL);

			foreach (DataRow row in ds.Tables[0].Rows)
			{
				returnVal = row[0].ToString();
				break;
			}
			return returnVal;
		}

		internal static string GetValue(SqlCommand sqlCommand)
		{
			string sValue = String.Empty;
			IDataReader dr = ExecuteReader(sqlCommand);
			if (dr.Read())
				sValue = dr[0].ToString();
			Close(dr);
			Close(sqlCommand);
			return sValue;
		}

		#region ADDED BY CJH FROM 1.1 OBJECT LAYER EXAMPLE ********************
		public static DataSet GetDataSet(string inSQL, string connectionStringName)
		{
			SqlConnection sqlConnection = new SqlConnection(GetConnectionString(connectionStringName));
			SqlCommand sqlCommand = new SqlCommand(inSQL, sqlConnection)
			{
				CommandType = CommandType.Text,
				Connection = sqlConnection
			};


			return GetDataSet(sqlCommand);
		}

		public static DataSet GetDataSet(string inSQL)
		{
			return GetDataSet(inSQL, DefaultConnectionStringName);
		}

		//private static System.Data.OleDb.OleDbConnection getConnection()
		//{
		//    System.Data.OleDb.OleDbConnection objConn = new System.Data.OleDb.OleDbConnection(ConnectionString);
		//    return objConn;
		//}
		#endregion ADDED BY CJH FROM 1.1 OBJECT LAYER EXAMPLE ********************

		public static object GetScalarValue(string sqlString, string connectionStringName)
		{
			using (SqlConnection conn = new SqlConnection(GetConnectionString(connectionStringName)))
			{
				SqlCommand cmd = new SqlCommand(sqlString, conn);
				return GetScalarValue(cmd);
			}
		}

		internal static object GetScalarValue(string sqlString)
		{
			return GetScalarValue(sqlString, DefaultConnectionStringName);
		}

		//public static bool HasData(string sqlString)
		//{
		//    SqlCommand sqlCommand = new SqlCommand();
		//    IDataReader reader = null;
		//    bool result = false;
		//    try
		//    {
		//        sqlCommand = NetSteps.Common.DataAccess.DataAccess.SetCommand(sqlString);
		//        sqlCommand.CommandType = CommandType.Text;
		//        reader = NetSteps.Common.DataAccess.DataAccess.ExecuteReader(sqlCommand);

		//        if (reader.Read())
		//            result = true;
		//    }
		//    catch (Exception ex)
		//    {
		//        NetSteps.Common.Exceptions.ExceptionHandler.HandleException(ex, true);
		//        throw new NetSteps.Common.Exceptions.LoadDataException(ex.Message, ex);
		//    }
		//    finally
		//    {
		//        NetSteps.Common.DataAccess.DataAccess.Close(reader);
		//        NetSteps.Common.DataAccess.DataAccess.Close(sqlCommand);
		//    }
		//    return result;
		//}

		public static bool ReaderHasData(IDataReader reader)
		{
			bool result = false;
			try
			{
				if (reader.Read())
				{
					result = true;
				}
			}
			catch
			{
				Close(reader);
				throw;
			}
			return result;
		}

		public static void Close(IDataReader reader)
		{
			if (reader != null)
			{
				reader.Close();
				reader.Dispose();
			}
		}

		public static void Close(SqlCommand sqlCommand)
		{
			sqlCommand.Connection.Close();
			sqlCommand.Dispose();
		}

		// #endregion

		//#region Object Readers

		////TODO: Provide overload for index parameter for speed
		//public static Int64 GetInt64(String column, IDataRecord reader)
		//{
		//    Int64 defaultValue = 0;
		//    int index = GetColumnIndex(column, reader);
		//    return (reader.IsDBNull(index)) ? defaultValue : (Int64)reader[column];
		//}

		//public static int GetInt32(String column, IDataRecord reader)
		//{
		//    int defaultValue = 0;
		//    int index = GetColumnIndex(column, reader);
		//    return (reader.IsDBNull(index)) ? defaultValue : (int)reader[column];
		//}

		//public static short GetInt16(String column, IDataRecord reader)
		//{
		//    short defaultValue = 0;
		//    int index = GetColumnIndex(column, reader);
		//    return (reader.IsDBNull(index)) ? defaultValue : Convert.ToInt16(reader[column]);
		//}

		//public static byte[] GetBytes(String column, IDataRecord reader)
		//{
		//    //System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
		//    //return encoding.GetBytes((string)reader[column]);

		//    byte[] defaultValue = null;
		//    int index = GetColumnIndex(column, reader);
		//    return (reader.IsDBNull(index)) ? defaultValue : (byte[])reader.GetValue(reader.GetOrdinal(column));
		//}

		//public static float GetFloat(String column, IDataRecord reader)
		//{
		//    float defaultValue = 0;
		//    int index = GetColumnIndex(column, reader);
		//    return (reader.IsDBNull(index)) ? defaultValue : float.Parse(reader[column].ToString(), CultureInfo.InvariantCulture);
		//}

		//public static double GetDouble(String column, IDataRecord reader)
		//{
		//    double defaultValue = 0;
		//    int index = GetColumnIndex(column, reader);
		//    return (reader.IsDBNull(index)) ? defaultValue : double.Parse(reader[column].ToString(), CultureInfo.InvariantCulture);
		//}

		//public static decimal GetDecimal(String column, IDataRecord reader)
		//{
		//    decimal defaultValue = 0;
		//    int index = GetColumnIndex(column, reader);
		//    return (reader.IsDBNull(index)) ? defaultValue : decimal.Parse(reader[column].ToString(), CultureInfo.InvariantCulture);
		//}

		//public static bool GetBoolean(String column, IDataRecord reader)
		//{
		//    bool defaultValue = false;
		//    int index = GetColumnIndex(column, reader);
		//    return (reader.IsDBNull(index)) ? defaultValue : Convert.ToBoolean(reader[column]);
		//}

		//public static String GetString(String column, IDataRecord reader)
		//{
		//    string defaultValue = string.Empty;
		//    int index = GetColumnIndex(column, reader);
		//    return (reader.IsDBNull(index)) ? defaultValue : reader[column].ToString().Trim();
		//}

		//public static Char GetChar(String column, IDataRecord reader)
		//{
		//    char defaultValue = ' ';
		//    int index = GetColumnIndex(column, reader);
		//    return (reader.IsDBNull(index)) ? defaultValue : char.Parse(reader[column].ToString());
		//}

		//public static DateTime GetDateTime(String column, IDataRecord reader)
		//{
		//    DateTime defaultValue = new DateTime(1900, 1, 1);// DateTime.MinValue;
		//    int index = GetColumnIndex(column, reader);
		//    return ((reader.IsDBNull(index)) || string.IsNullOrEmpty(reader[column].ToString())) ? defaultValue : (DateTime)reader[column];
		//}

		//public static T GetEnum<T>(String column, IDataRecord reader)
		//{
		//    T defaultValue = (T)Enum.GetValues(typeof(T)).GetValue(0); // This sets the default to the first item of the enum...
		//    int index = GetColumnIndex(column, reader);
		//    return (reader.IsDBNull(index)) ? defaultValue : (T)reader[column];
		//}

		//public static Object GetObject(String column, IDataRecord reader)
		//{
		//    Object defaultValue = null;
		//    int index = GetColumnIndex(column, reader);
		//    return (reader.IsDBNull(index)) ? defaultValue : reader[column];
		//}

		//public static bool GetBooleanReturnValue(string parameterName, SqlCommand sqlCommand)
		//{
		//    if (string.IsNullOrEmpty(parameterName))
		//    {
		//        return false;
		//    }
		//    string result = sqlCommand.Parameters["@" + parameterName].Value.ToString();
		//    if (result == "0" || result == "1")
		//        result = result == "0" ? "false" : "true";
		//    return Convert.ToBoolean(result);
		//}

		////public static int GetColumnIndex(string column, IDataRecord reader)
		////{
		////    int index = 0;
		////    try
		////    {
		////        index = reader.GetOrdinal(column);
		////    }
		////    catch (IndexOutOfRangeException ex)
		////    {
		////        index = -1;     // IndexOutOfRangeException -- named field is not found
		////        throw new ColumnNotInReaderException("The column \"" + column + "\" is not part of the reader's data record.", ex);
		////    }
		////    return index;
		////}
		//#endregion
	}

	public class DataAccessAutoUpdaterException : Exception
	{
		private string _message;
		public override string Message
		{
			get
			{
				return _message;
			}
		}
		// Summary:
		//     Initializes a new instance of the NetSteps.Objects.Data.DataAccessAutoUpdaterException class with a specified
		//     error message and a reference to the inner exception that is the cause of
		//     this exception.
		//
		// Parameters:
		//   message:
		//     The error message that explains the reason for the exception.
		//
		//   innerException:
		//     The exception that is the cause of the current exception, or a null reference
		//     (Nothing in Visual Basic) if no inner exception is specified.
		public DataAccessAutoUpdaterException(SqlConnection connection, string resourceName, Exception innerException)
			: base(string.Empty, innerException)
		{
			_message = string.Format("Error in {0}, on {1}", resourceName, connection.ConnectionString);
		}

	}
}
