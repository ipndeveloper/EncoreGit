using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Threading;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System.Data.Entity;
using NetSteps.Data.Entities.EntityModels;
using System.Dynamic;
using System.Collections.Specialized;
using NetSteps.Data.Entities.Business;
using System.Data.Entity.Infrastructure;
namespace NetSteps.Data.Entities
{
    public static class DataAccess
    {
        #region Members
        private static string _defaultConnectionStringName = "Sites";
        public const string STRING = "";
        public const int INT = 0;
        public static List<string> SqlDependencyOpenConnectionStrings = new List<string>();

        /// <summary>
        /// DbContext que obtiene la cadena de conexión
        /// </summary>
        static DbContext dbContext = new DbContext("Core");

        #endregion

        #region Properties

        public static string DefaultConnectionStringName
        {
            get { return _defaultConnectionStringName; }
        }

        /// <summary>
        /// This will return the Default Connection String for "Sites"
        /// </summary>
        internal static string ConnectionString
        {
            get
            {
                return GetConnectionString(DefaultConnectionStringName);
            }
        }

        internal static string GetConnectionString(string connectionStringName)
        {
            return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString+";Connection Timeout = 3600";
            //switch (connectionStringName.ToLower())
            //{
            //    case "maildb":
            //        return ConfigurationManager.GetConnectionString(connectionStringName); // CustomConfigurationHandler.Config.Database.MailDB;
            //    case "commissions":
            //        return CustomConfigurationHandler.Config.Database.Commissions;
            //    case "reporting":
            //        return CustomConfigurationHandler.Config.Database.ReportsServer;
            //    case "mail":
            //        return CustomConfigurationHandler.Config.Database.Mail;
            //    default:
            //        return CustomConfigurationHandler.Config.Database.Sites;
            //}
        }

        #endregion

        #region Constructors
        static DataAccess()
        {

        }
        #endregion

        #region Methods

        //Developed by IPN - CSTI
        public static SqlDataReader queryDatabase(string storeProcedureName, SqlConnection connection, SqlParameter[] parameters = null)
        {
            SqlCommand cmd = new SqlCommand(storeProcedureName, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            foreach (SqlParameter p in parameters)
            {
                cmd.Parameters.Add(p);
            }
            return cmd.ExecuteReader(CommandBehavior.SingleResult);
        }

        /// <summary>
        /// This is the Default Connection using the "Sites" Connection String Name
        /// </summary>
        /// <param name="commandValue"></param>
        /// <returns></returns>
        public static IDbCommand SetCommand(string commandValue)
        {
            return SetCommand(commandValue, DefaultConnectionStringName);
        }

        /// <summary>
        /// Use This Method to override the Default Connection String Name
        /// </summary>
        /// <param name="commandValue"></param>
        /// <param name="connectionStringName"></param>
        /// <returns></returns>
        public static IDbCommand SetCommand(string commandValue, string connectionStringName = null, string connectionString = null)
        {
            SqlConnection sqlConnection = new SqlConnection();

            //if (!connectionStringName.IsNullOrEmpty())
            //    sqlConnection.ConnectionString = GetConnectionString(connectionStringName);
            if (!connectionString.IsNullOrEmpty())
                sqlConnection.ConnectionString = connectionString;
            else
                throw new ArgumentException("Please provide either a 'connectionStringName' or 'connectionString'.");

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = commandValue;
            DbCommand dbCommand = new ProfiledDbCommand(sqlCommand, sqlConnection, MiniProfiler.Current)
            {
                CommandType = CommandType.StoredProcedure
            };
            // set command timeout equal to connection if it was changed via config to be larger
            if (sqlConnection.ConnectionTimeout > dbCommand.CommandTimeout)
                dbCommand.CommandTimeout = sqlConnection.ConnectionTimeout;
            return dbCommand;
        }

        //Developed by Wesley Campos S. - CSTI
        public static IDbCommand GetCommand(string storeProcedureName, Dictionary<string, object> parameters, string connectionStringName)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storeProcedureName;
            foreach (var parameter in parameters) cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
            return cmd;
        }

        //Developed by Wesley Campos S. - CSTI
        public static IDbCommand GetCommand(string storeProcedureName, Dictionary<string, object> parameters, SqlConnection connection, SqlTransaction tr)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storeProcedureName;
            cmd.Transaction = tr;
            foreach (var parameter in parameters) cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
            return cmd;
        }

        //Developed by BAL - CSTI - AINI
        public static SqlDataReader GetDataReader(string storeProcedureName, Dictionary<string, object> parameters, string connectionStringName)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
            

                SqlCommand cmd = new SqlCommand();
                
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storeProcedureName;
                    cmd.Connection.Open();

                    if (parameters != null)
                    {
                        foreach (var parameter in parameters) cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                     SqlDataReader reader = cmd.ExecuteReader() ;

                     return reader;

        }

        public static SqlDataReader GetDataReaderF(string function, Dictionary<string, object> parameters, string connectionStringName)
        {

             SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
            

                SqlCommand cmd = new SqlCommand();
                
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = function;
                    cmd.Connection.Open();

                    if (parameters != null)
                    {
                        foreach (var parameter in parameters) cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }

                    SqlDataReader reader = cmd.ExecuteReader();
                  
                        return reader;    
                    
                 
                
             
        }
        //Developed by BAL - CSTI - AFIN

        #region Add Input/Output Methods
        public static void AddInputParameter(string parameterName, object parameterValue, IDbCommand dbCommand)
        {
            dbCommand.Parameters.Add(new SqlParameter("@" + parameterName, parameterValue));
        }

        internal static void AddInputParameterCheckNull(string parameterName, object parameterValue, IDbCommand dbCommand)
        {
            if (parameterValue is string)
            {
                if (string.IsNullOrEmpty(parameterValue as string))
                    DataAccess.AddInputParameter(parameterName, DBNull.Value, dbCommand);
                else
                    DataAccess.AddInputParameter(parameterName, parameterValue as string, dbCommand);
            }
            else if (parameterValue is DateTime?)
            {
                if ((parameterValue as DateTime?) == null || (parameterValue as DateTime?) == DateTime.MinValue)
                    DataAccess.AddInputParameter(parameterName, DBNull.Value, dbCommand);
                else
                    DataAccess.AddInputParameter(parameterName, parameterValue as DateTime?, dbCommand);
            }
            else if (parameterValue is int)
            {
                if (Convert.ToInt32(parameterValue) == 0)
                    DataAccess.AddInputParameter(parameterName, DBNull.Value, dbCommand);
                else
                    DataAccess.AddInputParameter(parameterName, parameterValue, dbCommand);
            }
            else if (parameterValue is short)
            {
                if (Convert.ToInt16(parameterValue) == 0)
                    DataAccess.AddInputParameter(parameterName, DBNull.Value, dbCommand);
                else
                    DataAccess.AddInputParameter(parameterName, parameterValue, dbCommand);
            }
            else if (parameterValue == null)
            {
                DataAccess.AddInputParameter(parameterName, DBNull.Value, dbCommand);
            }
        }

        public static void AddInputParameterStructured(string parameterName, string typeName, object parameterValue, IDbCommand dbCommand)
        {
            dbCommand.Parameters.Add(new SqlParameter("@" + parameterName, parameterValue)
                {
                    SqlDbType = SqlDbType.Structured,
                    TypeName = typeName
                });
        }

        internal static void AddStringOutputParameter(string parameterName, IDbCommand dbCommand)
        {
            dbCommand.Parameters.Add(new SqlParameter("@" + parameterName, SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output });
        }

        internal static void AddOutputParameter(string parameterName, IDbCommand dbCommand)
        {
            dbCommand.Parameters.Add(new SqlParameter("@" + parameterName, SqlDbType.Int, 4) { Direction = ParameterDirection.Output });
        }

        internal static void AddMoneyOutputParameter(string parameterName, IDbCommand dbCommand)
        {
            dbCommand.Parameters.Add(new SqlParameter("@" + parameterName, SqlDbType.Money) { Direction = ParameterDirection.Output });
        }

        internal static void AddInputOutputParameter(string parameterName, object parameterValue, IDbCommand dbCommand)
        {
            dbCommand.Parameters.Add(new SqlParameter("@" + parameterName, parameterValue) { Direction = ParameterDirection.InputOutput });
        }

        internal static void AddReturnParameter(string parameterName, IDbCommand dbCommand)
        {
            dbCommand.Parameters.Add(new SqlParameter("@" + parameterName, SqlDbType.Int, 4) { Direction = ParameterDirection.ReturnValue });
        }
        #endregion

        #region Get Values Methods
        internal static int GetInt32ReturnValue(string parameterName, IDbCommand dbCommand)
        {
            if (((SqlParameter)dbCommand.Parameters["@" + parameterName]).Value.ToString().Length == 0)
            {
                return 0;
            }
            return Int32.Parse(((SqlParameter)dbCommand.Parameters["@" + parameterName]).Value.ToString(), CultureInfo.InvariantCulture);
        }

        internal static Int64 GetInt64ReturnValue(string parameterName, IDbCommand dbCommand)
        {
            if (((SqlParameter)dbCommand.Parameters["@" + parameterName]).Value.ToString().Length == 0)
            {
                return 0;
            }
            return Int64.Parse(((SqlParameter)dbCommand.Parameters["@" + parameterName]).Value.ToString(), CultureInfo.InvariantCulture);
        }

        internal static Decimal GetDecimalReturnValue(string parameterName, IDbCommand dbCommand)
        {
            if (((SqlParameter)dbCommand.Parameters["@" + parameterName]).Value.ToString().Length == 0)
            {
                return 0;
            }
            return Decimal.Parse(((SqlParameter)dbCommand.Parameters["@" + parameterName]).Value.ToString(), CultureInfo.InvariantCulture);
        }

        internal static string GetStringReturnValue(string parameterName, IDbCommand dbCommand)
        {
            if (((SqlParameter)dbCommand.Parameters["@" + parameterName]).Value.ToString().Length == 0)
            {
                return string.Empty;
            }
            return ((SqlParameter)dbCommand.Parameters["@" + parameterName]).Value.ToString();
        }


        internal static String GetString(String column, IDataRecord reader)
        {
            return GetStringNoTrim(column, reader).Trim();
        }

        public static String GetStringNoTrim(String column, IDataRecord reader)
        {
            int index = GetColumnIndex(column, reader);
            return GetStringNoTrim(index, reader);
        }

        public static String GetStringNoTrim(int ordinal, IDataRecord reader)
        {
            return reader.IsDBNull(ordinal) ? STRING : Convert.ToString(reader[ordinal]);
        }

        public static int GetColumnIndex(string column, IDataRecord reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(column, StringComparison.InvariantCultureIgnoreCase))
                    return i;
            }
            var ex = new ColumnNotInReaderException("The column \"" + column + "\" is not part of the reader's data record.");
            throw ex;
        }

        public static int GetInt32(String column, IDataRecord reader)
        {
            int index = GetColumnIndex(column, reader);
            return GetInt32(index, reader);
        }

        public static int GetInt32(int ordinal, IDataRecord reader)
        {
            if (reader.IsDBNull(ordinal))
                return INT;

            if (reader[ordinal] is byte[])
                return BitConverter.ToInt32((byte[])reader[ordinal], 0);

            return Convert.ToInt32(reader[ordinal]);
        }

        internal static byte GetByteReturnValue(string parameterName, IDbCommand dbCommand)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                return 0;
            }
            return Byte.Parse(((SqlParameter)dbCommand.Parameters["@" + parameterName]).Value.ToString(), CultureInfo.InvariantCulture);
        }

        public static bool GetBooleanReturnValue(string parameterName, IDbCommand dbCommand)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                return false;
            }
            string result = ((SqlParameter)dbCommand.Parameters["@" + parameterName]).Value.ToString();
            if (result == "0" || result == "1")
                result = result == "0" ? "false" : "true";
            return Convert.ToBoolean(result);
        }

        internal static object GetScalarValue(IDbCommand dbCommand)
        {
            try
            {
                using (dbCommand)
                {
                    if (dbCommand.Connection.State == ConnectionState.Closed)
                    {
                        dbCommand.Connection.Open();
                    }
                    return dbCommand.ExecuteScalar();
                }
            }
            finally
            {
                Close(dbCommand);
            }
        }

        public static string GetValue(string sql)
        {
            string value = string.Empty;
            DataSet ds = GetDataSet(sql);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                value = row[0].ToString();
                break;
            }
            return value;
        }

        internal static string GetValue(IDbCommand dbCommand)
        {
            string value = "";
            IDataReader dr = ExecuteReader(dbCommand);
            if (dr.Read())
                value = dr[0].ToString();
            Close(dr);
            Close(dbCommand);
            return value;
        }
        #endregion

        public static IDataReader ExecuteReader(IDbCommand dbCommand)
        {
            try
            {
                using (dbCommand)
                {
                    if (dbCommand.Connection.State != ConnectionState.Open)
                        dbCommand.Connection.Open();
                    return dbCommand.ExecuteReader();
                }
            }
            catch
            {
                Close(dbCommand);
                throw;
            }
        }

        public static int ExecuteNonQuery(IDbCommand dbCommand)
        {
            try
            {
                if (dbCommand.Connection.State != ConnectionState.Open)
                    dbCommand.Connection.Open();
                return dbCommand.ExecuteNonQuery();
            }
            catch
            {
                Close(dbCommand);
                throw;
            }
        }

        public static object ExecuteScalar(IDbCommand dbCommand)
        {
            try
            {
                if (dbCommand.Connection.State != ConnectionState.Open)
                    dbCommand.Connection.Open();
                return dbCommand.ExecuteScalar();
            }
            catch
            {
                Close(dbCommand);
                throw;
            }
        }

        internal static int ExecuteNonQuery(string inSQL, string connectionStringName)
        {
            SqlConnection sqlConnection = new SqlConnection(GetConnectionString(connectionStringName));

            DbCommand dbCommand = new ProfiledDbCommand(new SqlCommand(inSQL), sqlConnection, MiniProfiler.Current)
            {
                CommandType = CommandType.Text
            };

            try
            {
                return DataAccess.ExecuteNonQuery(dbCommand);
            }
            finally
            {
                DataAccess.Close(dbCommand);
            }
        }

        public static int ExecuteNonQuery(string inSQL)
        {
            return ExecuteNonQuery(inSQL, DefaultConnectionStringName);
        }

        internal static DataTable GetDataTable(IDbCommand dbCommand)
        {
            DataSet dataSet = GetDataSet(dbCommand);
            return dataSet.Tables.Count > 0 ? dataSet.Tables[0] : null;
        }

        public static DataSet GetDataSet(IDbCommand dbCommand)
        {
            try
            {
                DataSet dataSet = new DataSet();

                SqlCommand sqlCommand = null;
                if (dbCommand is SqlCommand)
                    sqlCommand = dbCommand as SqlCommand;
                else if (dbCommand is ProfiledDbCommand && ((ProfiledDbCommand)dbCommand).InternalCommand is SqlCommand)
                    sqlCommand = ((ProfiledDbCommand)dbCommand).InternalCommand as SqlCommand;
                else
                    throw new ArgumentException("Please provide either a SqlCommand or a ProfiledDbCommand wrapping a SqlCommand");

                sqlCommand.CommandTimeout = 3600;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                ProfiledDbDataAdapter dbDataAdapter = new ProfiledDbDataAdapter(sqlDataAdapter);
                dbDataAdapter.Fill(dataSet);
                dbDataAdapter.Dispose();

                return dataSet;
            }
            finally
            {
                Close(dbCommand);
            }
        }

        internal static DataSet GetDataSet(IDbCommand dbCommand, string connectionStringName)
        {
            SqlConnection sqlConnection = new SqlConnection(GetConnectionString(connectionStringName));
            dbCommand.Connection = sqlConnection;

            return DataAccess.GetDataSet(dbCommand);
        }

        #region ADDED BY CJH FROM 1.1 OBJECT LAYER EXAMPLE ********************
        public static DataSet GetDataSet(string inSQL, string connectionStringName, string connectionString = null, int commandTimeout = 30)
        {
            if (connectionString == null)
                connectionString = GetConnectionString(connectionStringName);
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            IDbCommand dbCommand = new SqlCommand(inSQL, sqlConnection)
            {
                CommandType = CommandType.Text,
                Connection = sqlConnection,
                CommandTimeout = commandTimeout
            };

            return GetDataSet(dbCommand);
        }

        public static DataSet GetDataSet(string inSQL)
        {
            return GetDataSet(inSQL, DefaultConnectionStringName);
        }
        #endregion ADDED BY CJH FROM 1.1 OBJECT LAYER EXAMPLE ********************

        public static bool HasData(string sqlString)
        {
            IDbCommand dbCommand = new SqlCommand();
            IDataReader reader = null;
            bool result = false;
            try
            {
                dbCommand = SetCommand(sqlString);
                dbCommand.CommandType = CommandType.Text;
                reader = ExecuteReader(dbCommand);

                if (reader.Read())
                    result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Close(reader);
                Close(dbCommand);
            }
            return result;
        }

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

        public static void Close(IDbCommand dbCommand)
        {
            if (dbCommand != null)
            {
                dbCommand.Connection.Close();
                dbCommand.Dispose();
            }
        }

        public static void CloseAllSqlDependencies()
        {
            try
            {
                //ParallelOptions options = new ParallelOptions();
                //Parallel.ForEach(DataAccess.SqlDependencyOpenConnectionStrings, options, connectionString =>
                //{
                //});

                foreach (var connectionString in DataAccess.SqlDependencyOpenConnectionStrings)
                {
                    ThreadHelpers.RunWithTimeout(() => SqlDependency.Stop(connectionString), TimeSpan.FromSeconds(4));
                }
            }
            catch (TimeoutException)
            {
                // Swallowing this for now - JHE
            }
        }

        #region Add FHP CSTI.

        /// <summary>
        /// Permite obtener el listado de entidades segun el tipado via store procedure
        /// </summary>
        /// <param name="query">nombre de Base de Datos</param>
        /// <param name="query">nombre de store procedure</param>
        /// <param name="parameters">parametros de entrada del store procedure</param>
        /// <returns>Lista de entidades</returns>
        public static IEnumerable<T> ExecWithStoreProcedure<T>(string dataBase, string query, params object[] parameters)
        {
            query = GenerateQueryString(query, parameters);
            return new DbContext(dataBase).Database.SqlQuery<T>(query, parameters);
        }

        /// <summary>
        /// Permite ejecutar un store procedure de actualizacion
        /// </summary>
        /// <param name="query">nombre de Base de Datos</param>
        /// <param name="query">nombre de Store Procedure</param>
        /// <param name="parameters">parametros de entrada del store procedure</param>
        /// <returns>Numero de registros afectados</returns>
        public static int ExecWithStoreProcedureSave(string dataBase, string query, params object[] parameters)
        {
            /*CS.13JUL2016.Inicio.Time Out 1Hora*/
            DbContext miDbContext = new DbContext(GetConnectionString(dataBase));
            var objectContext = (miDbContext as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = 3600;
            query = GenerateQueryString(query, parameters);
            /*CS.13JUL2016.Fin.Time Out 1Hora*/

            return miDbContext.Database.ExecuteSqlCommand(query, parameters);
            //return new DbContext(dataBase).Database.ExecuteSqlCommand(query, parameters);
        }
         
        /// <summary>
        /// Permite ejecutar un store procedure que devuelve una valor unico entero
        /// </summary>
        /// <param name="query">nombre de Base de Datos</param>
        /// <param name="query">nombre de store procedure</param>
        /// <param name="parameters">parametros de entrada del store procedure</param>
        /// <returns>Numero generado</returns>
        public static int ExecWithStoreProcedureScalar(string dataBase, string query, params object[] parameters)
        {
            query = GenerateQueryString(query, parameters);
            return new DbContext(dataBase).Database.SqlQuery<int>(query, parameters).FirstOrDefault(); 
        }

        //INI - GR_Encore-07
        /// <summary>
        /// Permite ejecutar una funcion que devuelve una valor unico entero
        /// </summary>
        /// <param name="query">nombre de Base de Datos</param>
        /// <param name="query">nombre de funcion</param>
        /// <param name="parameters">parametros de entrada del store procedure</param>
        /// <returns>Numero generado</returns>
        public static int ExecWithFunctionScalar(string dataBase, string query, params object[] parameters)
        {
            query = GenerateQueryFunctionString(query, parameters);
            return new DbContext(dataBase).Database.SqlQuery<int>(query, parameters).FirstOrDefault();
        }
        //INI - GR_Encore-07


        /// <summary>
        /// Permite ejecutar un store procedure que devuelve una valor unico tipado
        /// </summary>
        /// <param name="query">nombre de store procedure</param>
        /// <param name="parameters">parametros de entrada del store procedure</param>
        /// <returns>valor tipado</returns>
        public static T ExecWithStoreProcedureScalarType<T>(string dataBase, string query, params object[] parameters)
        {
            query = GenerateQueryString(query, parameters);
            return new DbContext(dataBase).Database.SqlQuery<T>(query, parameters).FirstOrDefault();
        }

        /// <summary>
        /// Permite ejecutar un query de forma dinamica que devuelve la cantidad de filas afectadas
        /// </summary>
        /// <param name="dataBase">Nombre de la base de Datos</param>
        /// <param name="query">Query que se va ejecutar</param>
        /// <returns>Número de filas que fueron afectadas</returns>
        public static int ExecQueryDinamico(string dataBase, string query)
        {
            int result = 0;
            SqlConnection sqlConnection = new SqlConnection();
            try
            { 
                sqlConnection = (SqlConnection)new DbContext(dataBase).Database.Connection; 
                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.CommandTimeout = sqlConnection.ConnectionTimeout;

                    sqlConnection.Open();
                    result = cmd.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
            return result;
        }

        /// <summary>
        /// Permite obtener el listado de entidades segun el tipado via store procedure
        /// </summary>
        /// <param name="query">nombre de store procedure</param>
        /// <param name="parameters">parametros de entrada del store procedure</param>
        /// <returns>Lista de entidades</returns>
        //public static IEnumerable<T> ExecWithStoreProcedure<T>(string dataBase, string query, params object[] parameters)
        //{
        //    query = GenerateQueryString(query, parameters);
        //    return new DbContext(dataBase).Database.SqlQuery<T>(query, parameters);
        //}

        public static IEnumerable<T> ExecWithStoreProcedureListParam<T>(string dataBase, string query, params object[] parameters)
        {
            query = GenerateQueryString(query, parameters);
            return new DbContext(dataBase).Database.SqlQuery<T>(query, parameters);
        }

        public static IEnumerable<T> ExecWithStoreProcedureListParam<T>(string dataBase, string query, out object outData, params object[] parameters)
        {
            query = GenerateQueryStringWithOutParam(query, parameters);
            var result = new ExtendedTimeoutContext(dataBase).Database.SqlQuery<T>(query, parameters).ToList();
            outData = ((System.Data.SqlClient.SqlParameter)(parameters[parameters.Length-1])).Value;
            return result;
        }

        //public static Dictionary<string, object> ExecQueryEntidadDictionarys(string dataBase, string query)
        //{ 
        //    Dictionary<string, object> result = null;
        //    SqlConnection sqlConnection = (SqlConnection)new DbContext(dataBase).Database.Connection;
        //    try
        //    {
        //        using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
        //        {
        //            sqlConnection.Open();
        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                result = ReadQueryDictionarys(reader);
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        sqlConnection.Close();
        //    }
        //    return result;
        //}

        //private static Dictionary<string, object> ReadQueryDictionarys(System.Data.Common.DbDataReader reader)
        //{
        //    Dictionary<string, object> registro = new Dictionary<string, object>();
        //    while (reader.Read())
        //    {
        //        for (int i = 0; i < reader.FieldCount; i++)
        //        {
        //            registro.Add(reader.GetName(i), reader.GetValue(i));
        //        }
        //    }
        //    return registro;
        //}


       


        public static Dictionary<string, string> ExecQueryEntidadDictionary(string dataBase, string query)
        {
            SqlConnection sqlConnection = (SqlConnection)new DbContext(dataBase).Database.Connection;
            Dictionary<string, string> result = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    sqlConnection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        result = ReadQueryDictionary(reader);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
            return result;
        }

        public static Dictionary<string, string> ReadQueryDictionary(System.Data.Common.DbDataReader reader)
        {
            Dictionary<string, string> registro = new Dictionary<string, string>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    registro.Add(Convert.ToString(reader.GetInt32(0)), reader.GetString(1));

                }
            }
            return registro;
        }

        public static IEnumerable<T> ExecWithStoreProcedureLists<T>(string dataBase, string query)
        {
            return new DbContext(dataBase).Database.SqlQuery<T>(query);
        }

        //public static int ExecWithStoreProcedureSave(string dataBase, string query, params object[] parameters)
        //{
        //    query = GenerateQueryString(query, parameters);
        //    if (parameters == null)
        //    {
        //        return new DbContext(dataBase).Database.ExecuteSqlCommand(query);
        //    }
        //    return new DbContext(dataBase).Database.ExecuteSqlCommand(query, parameters);
        //} 

        public static int ExecWithStoreProcedureSaveIdentity(string dataBase, string query, params object[] parameters)
        {
            //query = GenerateQueryString(query, parameters);
            //return new DbContext(dataBase).Database.SqlQuery<int>(query, parameters).FirstOrDefault();
            /*CS.13JUL2016.Inicio.Time Out 1Hora*/
            DbContext miDbContext = new DbContext(GetConnectionString(dataBase));
            var objectContext = (miDbContext as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = 3600;
            query = GenerateQueryString(query, parameters);
            return miDbContext.Database.SqlQuery<int>(query, parameters).FirstOrDefault();
            /*CS.13JUL2016.Fin.Time Out 1Hora*/
        }

        public static decimal ExecWithStoreProcedureDecimal(string dataBase, string query, params object[] parameters)
        {
            query = GenerateQueryString(query, parameters);
            return new DbContext(dataBase).Database.SqlQuery<decimal>(query, parameters).FirstOrDefault();
        }
        public static bool ExecWithStoreProcedureBool(string dataBase, string query, params object[] parameters)
        {
            query = GenerateQueryString(query, parameters);
            return new DbContext(dataBase).Database.SqlQuery<bool>(query, parameters).FirstOrDefault();
        }

        private static string GenerateQueryString(string query, params object[] parameters)
        {
            if (!query.Contains("@"))
            {
                if (parameters != null)
                {
                    for (var i = 0; i < parameters.Length; i++)
                    {
                        if (i == 0)
                        {
                            query += " @" + ((System.Data.SqlClient.SqlParameter)(parameters[i])).ParameterName;
                        }
                        else
                        {
                            query += ", @" + ((System.Data.SqlClient.SqlParameter)(parameters[i])).ParameterName;
                        }
                    }
                }
            }
            return query;
        }

        private static string GenerateQueryStringWithOutParam(string query, params object[] parameters)
        {
            if (!query.Contains("@"))
            {
                if (parameters != null)
                {
                    for (var i = 0; i < parameters.Length; i++)
                    {
                        if (i == 0)
                        {
                            query += " @" + ((System.Data.SqlClient.SqlParameter)(parameters[i])).ParameterName;
                        }
                        else
                        {
                            query += ", @" + ((System.Data.SqlClient.SqlParameter)(parameters[i])).ParameterName;
                        }

                        if (((System.Data.SqlClient.SqlParameter)(parameters[i])).Direction == ParameterDirection.Output)
                            query += " out";
                    }
                }
            }
            return query;
        }

        //INI - GR_Encore-07
        private static string GenerateQueryFunctionString(string query, params object[] parameters)
        {
            query = "SELECT " + query;
            if (!query.Contains("@"))
            {
                if (parameters != null)
                {
                    for (var i = 0; i < parameters.Length; i++)
                    {
                        if (i == 0)
                        {
                            query += "(@" + ((System.Data.SqlClient.SqlParameter)(parameters[i])).ParameterName;
                        }
                        else
                        {
                            query += ", @" + ((System.Data.SqlClient.SqlParameter)(parameters[i])).ParameterName;
                        }
                    }
                }
            }
            return query + ")";
        }
        //FIN - GR_Encore-07
        #endregion

        #endregion

    }

    public static class ObjectExtensions
    {
        /// <summary>
        /// Extension method for adding in a bunch of parameters
        /// </summary>
        public static void AddParams(this DbCommand cmd, params object[] args)
        {
            foreach (var item in args)
            {
                AddParam(cmd, item);
            }
        }
        /// <summary>
        /// Extension for adding single parameter
        /// </summary>
        public static void AddParam(this DbCommand cmd, object item)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = string.Format("@{0}", cmd.Parameters.Count);
            if (item == null)
            {
                p.Value = DBNull.Value;
            }
            else
            {
                if (item.GetType() == typeof(Guid))
                {
                    p.Value = item.ToString();
                    p.DbType = DbType.String;
                    p.Size = 4000;
                }
                else if (item.GetType() == typeof(ExpandoObject))
                {
                    var d = (IDictionary<string, object>)item;
                    p.Value = d.Values.FirstOrDefault();
                }
                else
                {
                    p.Value = item;
                }
                if (item.GetType() == typeof(string))
                    p.Size = ((string)item).Length > 4000 ? -1 : 4000;
            }
            cmd.Parameters.Add(p);
        }
        /// <summary>
        /// Turns an IDataReader to a Dynamic list of things
        /// </summary>
        public static List<dynamic> ToExpandoList(this IDataReader rdr)
        {
            var result = new List<dynamic>();
            while (rdr.Read())
            {
                result.Add(rdr.RecordToExpando());
            }
            return result;
        }
        public static dynamic RecordToExpando(this IDataReader rdr)
        {
            dynamic e = new ExpandoObject();
            var d = e as IDictionary<string, object>;
            object[] values = new object[rdr.FieldCount];
            rdr.GetValues(values);
            for (int i = 0; i < values.Length; i++)
            {
                var v = values[i];
                d.Add(rdr.GetName(i), DBNull.Value.Equals(v) ? null : v);
            }
            return e;
        }
        /// <summary>
        /// Turns the object into an ExpandoObject
        /// </summary>
        public static dynamic ToExpando(this object o)
        {
            if (o.GetType() == typeof(ExpandoObject)) return o; //shouldn't have to... but just in case
            var result = new ExpandoObject();
            var d = result as IDictionary<string, object>; //work with the Expando as a Dictionary
            if (o.GetType() == typeof(NameValueCollection) || o.GetType().IsSubclassOf(typeof(NameValueCollection)))
            {
                var nv = (NameValueCollection)o;
                nv.Cast<string>().Select(key => new KeyValuePair<string, object>(key, nv[key])).ToList().ForEach(i => d.Add(i));
            }
            else
            {
                var props = o.GetType().GetProperties();
                foreach (var item in props)
                {
                    d.Add(item.Name, item.GetValue(o, null));
                }
            }
            return result;
        }

        /// <summary>
        /// Turns the object into a Dictionary
        /// </summary>
        //public static IDictionary<string, object> ToDictionary(this object thingy)
        //{
        //    return (IDictionary<string, object>)thingy.ToExpando();
        //}
    }

    /// <summary>
    /// Convenience class for opening/executing data
    /// </summary>
    public static class DB
    {
        public static DynamicModel Current
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings.Count > 1)
                {
                    return new DynamicModel(ConfigurationManager.ConnectionStrings[1].Name);
                }
                throw new InvalidOperationException("Need a connection string name - can't determine what it is");
            }
        }
    }

    /// <summary>
    /// A class that wraps your database table in Dynamic Funtime
    /// </summary>
    public class DynamicModel : DynamicObject
    {
        DbProviderFactory _factory;
        string ConnectionString;
        public static DynamicModel Open(string connectionStringName)
        {
            dynamic dm = new DynamicModel(connectionStringName);
            return dm;
        }
        public DynamicModel(string connectionStringName, string tableName = "",
            string primaryKeyField = "", string descriptorField = "")
        {
            TableName = tableName == "" ? this.GetType().Name : tableName;
            PrimaryKeyField = string.IsNullOrEmpty(primaryKeyField) ? "ID" : primaryKeyField;
            DescriptorField = descriptorField;
            var _providerName = "System.Data.SqlClient";

            if (!string.IsNullOrWhiteSpace(ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName))
                _providerName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;

            _factory = DbProviderFactories.GetFactory(_providerName);
            ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        /// <summary>
        /// Creates a new Expando from a Form POST - white listed against the columns in the DB
        /// </summary>
        public dynamic CreateFrom(NameValueCollection coll)
        {
            dynamic result = new ExpandoObject();
            var dc = (IDictionary<string, object>)result;
            var schema = Schema;
            //loop the collection, setting only what's in the Schema
            foreach (var item in coll.Keys)
            {
                var exists = schema.Any(x => x.COLUMN_NAME.ToLower() == item.ToString().ToLower());
                if (exists)
                {
                    var key = item.ToString();
                    var val = coll[key];
                    dc.Add(key, val);
                }
            }
            return result;
        }
        /// <summary>
        /// Gets a default value for the column
        /// </summary>
        public dynamic DefaultValue(dynamic column)
        {
            dynamic result = null;
            string def = column.COLUMN_DEFAULT;
            if (String.IsNullOrEmpty(def))
            {
                result = null;
            }
            else if (def == "getdate()" || def == "(getdate())")
            {
                result = DateTime.Now.ToShortDateString();
            }
            else if (def == "newid()")
            {
                result = Guid.NewGuid().ToString();
            }
            else
            {
                result = def.Replace("(", "").Replace(")", "");
            }
            return result;
        }
        /// <summary>
        /// Creates an empty Expando set with defaults from the DB
        /// </summary>
        public dynamic Prototype
        {
            get
            {
                dynamic result = new ExpandoObject();
                var schema = Schema;
                foreach (dynamic column in schema)
                {
                    var dc = (IDictionary<string, object>)result;
                    dc.Add(column.COLUMN_NAME, DefaultValue(column));
                }
                result._Table = this;
                return result;
            }
        }
        public string DescriptorField { get; protected set; }
        /// <summary>
        /// List out all the schema bits for use with ... whatever
        /// </summary>
        IEnumerable<dynamic> _schema;
        public IEnumerable<dynamic> Schema
        {
            get
            {
                if (_schema == null)
                    _schema = Query("SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @0", TableName);
                return _schema;
            }
        }

        /// <summary>
        /// Enumerates the reader yielding the result - thanks to Jeroen Haegebaert
        /// </summary>
        public virtual IEnumerable<dynamic> Query(string sql, params object[] args)
        {
            using (var conn = OpenConnection())
            {
                var rdr = CreateCommand(sql, conn, args).ExecuteReader();
                while (rdr.Read())
                {
                    yield return rdr.RecordToExpando(); ;
                }
            }
        }
        public virtual IEnumerable<dynamic> Query(string sql, DbConnection connection, params object[] args)
        {
            using (var rdr = CreateCommand(sql, connection, args).ExecuteReader())
            {
                while (rdr.Read())
                {
                    yield return rdr.RecordToExpando(); ;
                }
            }
        }
        /// <summary>
        /// Returns a single result
        /// </summary>
        public virtual object Scalar(string sql, params object[] args)
        {
            object result = null;
            using (var conn = OpenConnection())
            {
                result = CreateCommand(sql, conn, args).ExecuteScalar();
            }
            return result;
        }
        /// <summary>
        /// Creates a DBCommand that you can use for loving your database.
        /// </summary>
        DbCommand CreateCommand(string sql, DbConnection conn, params object[] args)
        {
            var result = _factory.CreateCommand();
            result.Connection = conn;
            result.CommandText = sql;
            if (args.Length > 0)
                result.AddParams(args);
            return result;
        }
        /// <summary>
        /// Returns and OpenConnection
        /// </summary>
        public virtual DbConnection OpenConnection()
        {
            var result = _factory.CreateConnection();
            result.ConnectionString = ConnectionString;
            result.Open();
            return result;
        }
        /// <summary>
        /// Builds a set of Insert and Update commands based on the passed-on objects.
        /// These objects can be POCOs, Anonymous, NameValueCollections, or Expandos. Objects
        /// With a PK property (whatever PrimaryKeyField is set to) will be created at UPDATEs
        /// </summary>
        public virtual List<DbCommand> BuildCommands(params object[] things)
        {
            var commands = new List<DbCommand>();
            foreach (var item in things)
            {
                if (HasPrimaryKey(item))
                {
                    commands.Add(CreateUpdateCommand(item.ToExpando(), GetPrimaryKey(item)));
                }
                else
                {
                    commands.Add(CreateInsertCommand(item.ToExpando()));
                }
            }
            return commands;
        }


        public virtual int Execute(DbCommand command)
        {
            return Execute(new DbCommand[] { command });
        }

        public virtual int Execute(string sql, params object[] args)
        {
            return Execute(CreateCommand(sql, null, args));
        }
        /// <summary>
        /// Executes a series of DBCommands in a transaction
        /// </summary>
        public virtual int Execute(IEnumerable<DbCommand> commands)
        {
            var result = 0;
            using (var conn = OpenConnection())
            {
                using (var tx = conn.BeginTransaction())
                {
                    foreach (var cmd in commands)
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = tx;
                        result += cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                }
            }
            return result;
        }
        public virtual string PrimaryKeyField { get; set; }
        /// <summary>
        /// Conventionally introspects the object passed in for a field that 
        /// looks like a PK. If you've named your PrimaryKeyField, this becomes easy
        /// </summary>
        public virtual bool HasPrimaryKey(object o)
        {
            return o.ToDictionary().ContainsKey(PrimaryKeyField);
        }
        /// <summary>
        /// If the object passed in has a property with the same name as your PrimaryKeyField
        /// it is returned here.
        /// </summary>
        public virtual object GetPrimaryKey(object o)
        {
            object result = null;
            o.ToDictionary().TryGetValue(PrimaryKeyField, out result);
            return result;
        }
        public virtual string TableName { get; set; }
        /// <summary>
        /// Returns all records complying with the passed-in WHERE clause and arguments, 
        /// ordered as specified, limited (TOP) by limit.
        /// </summary>
        public virtual IEnumerable<dynamic> All(string where = "", string orderBy = "", int limit = 0, string columns = "*", params object[] args)
        {
            string sql = BuildSelect(where, orderBy, limit);
            return Query(string.Format(sql, columns, TableName), args);
        }
        private static string BuildSelect(string where, string orderBy, int limit)
        {
            string sql = limit > 0 ? "SELECT TOP " + limit + " {0} FROM {1} " : "SELECT {0} FROM {1} ";
            if (!string.IsNullOrEmpty(where))
                sql += where.Trim().StartsWith("where", StringComparison.OrdinalIgnoreCase) ? where : " WHERE " + where;
            if (!String.IsNullOrEmpty(orderBy))
                sql += orderBy.Trim().StartsWith("order by", StringComparison.OrdinalIgnoreCase) ? orderBy : " ORDER BY " + orderBy;
            return sql;
        }

        /// <summary>
        /// Returns a dynamic PagedResult. Result properties are Items, TotalPages, and TotalRecords.
        /// </summary>
        public virtual dynamic Paged(string where = "", string orderBy = "", string columns = "*", int pageSize = 20, int currentPage = 1, params object[] args)
        {
            return BuildPagedResult(where: where, orderBy: orderBy, columns: columns, pageSize: pageSize, currentPage: currentPage, args: args);
        }

        public virtual dynamic Paged(string sql, string primaryKey, string where = "", string orderBy = "", string columns = "*", int pageSize = 20, int currentPage = 1, params object[] args)
        {
            return BuildPagedResult(sql, primaryKey, where, orderBy, columns, pageSize, currentPage, args);
        }

        private dynamic BuildPagedResult(string sql = "", string primaryKeyField = "", string where = "", string orderBy = "", string columns = "*", int pageSize = 20, int currentPage = 1, params object[] args)
        {
            dynamic result = new ExpandoObject();
            var countSQL = "";
            if (!string.IsNullOrEmpty(sql))
                countSQL = string.Format("SELECT COUNT({0}) FROM ({1}) AS PagedTable", primaryKeyField, sql);
            else
                countSQL = string.Format("SELECT COUNT({0}) FROM {1}", PrimaryKeyField, TableName);

            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = string.IsNullOrEmpty(primaryKeyField) ? PrimaryKeyField : primaryKeyField;
            }

            if (!string.IsNullOrEmpty(where))
            {
                if (!where.Trim().StartsWith("where", StringComparison.CurrentCultureIgnoreCase))
                {
                    where = " WHERE " + where;
                }
            }

            var query = "";
            if (!string.IsNullOrEmpty(sql))
                query = string.Format("SELECT {0} FROM (SELECT ROW_NUMBER() OVER (ORDER BY {1}) AS Row, {0} FROM ({2}) AS PagedTable {3}) AS Paged ", columns, orderBy, sql, where);
            else
                query = string.Format("SELECT {0} FROM (SELECT ROW_NUMBER() OVER (ORDER BY {1}) AS Row, {0} FROM {2} {3}) AS Paged ", columns, orderBy, TableName, where);

            var pageStart = (currentPage - 1) * pageSize;
            query += string.Format(" WHERE Row > {0} AND Row <={1}", pageStart, (pageStart + pageSize));
            countSQL += where;
            result.TotalRecords = Scalar(countSQL, args);
            result.TotalPages = result.TotalRecords / pageSize;
            if (result.TotalRecords % pageSize > 0)
                result.TotalPages += 1;
            result.Items = Query(string.Format(query, columns, TableName), args);
            return result;
        }
        /// <summary>
        /// Returns a single row from the database
        /// </summary>
        public virtual dynamic Single(string where, params object[] args)
        {
            var sql = string.Format("SELECT * FROM {0} WHERE {1}", TableName, where);
            return Query(sql, args).FirstOrDefault();
        }
        /// <summary>
        /// Returns a single row from the database
        /// </summary>
        public virtual dynamic Single(object key, string columns = "*")
        {
            var sql = string.Format("SELECT {0} FROM {1} WHERE {2} = @0", columns, TableName, PrimaryKeyField);
            return Query(sql, key).FirstOrDefault();
        }
        /// <summary>
        /// This will return a string/object dictionary for dropdowns etc
        /// </summary>
        public virtual IDictionary<string, object> KeyValues(string orderBy = "")
        {
            if (String.IsNullOrEmpty(DescriptorField))
                throw new InvalidOperationException("There's no DescriptorField set - do this in your constructor to describe the text value you want to see");
            var sql = string.Format("SELECT {0},{1} FROM {2} ", PrimaryKeyField, DescriptorField, TableName);
            if (!String.IsNullOrEmpty(orderBy))
                sql += "ORDER BY " + orderBy;

            var results = Query(sql).ToList().Cast<IDictionary<string, object>>();
            return results.ToDictionary(key => key[PrimaryKeyField].ToString(), value => value[DescriptorField]);
        }

        /// <summary>
        /// This will return an Expando as a Dictionary
        /// </summary>
        public virtual IDictionary<string, object> ItemAsDictionary(ExpandoObject item)
        {
            return (IDictionary<string, object>)item;
        }
        //Checks to see if a key is present based on the passed-in value
        public virtual bool ItemContainsKey(string key, ExpandoObject item)
        {
            var dc = ItemAsDictionary(item);
            return dc.ContainsKey(key);
        }
        /// <summary>
        /// Executes a set of objects as Insert or Update commands based on their property settings, within a transaction.
        /// These objects can be POCOs, Anonymous, NameValueCollections, or Expandos. Objects
        /// With a PK property (whatever PrimaryKeyField is set to) will be created at UPDATEs
        /// </summary>
        public virtual int Save(params object[] things)
        {
            foreach (var item in things)
            {
                if (!IsValid(item))
                {
                    throw new InvalidOperationException("Can't save this item: " + String.Join("; ", Errors.ToArray()));
                }
            }
            var commands = BuildCommands(things);
            return Execute(commands);
        }
        public virtual DbCommand CreateInsertCommand(dynamic expando)
        {
            DbCommand result = null;
            var settings = (IDictionary<string, object>)expando;
            var sbKeys = new StringBuilder();
            var sbVals = new StringBuilder();
            var stub = "INSERT INTO {0} ({1}) \r\n VALUES ({2})";
            result = CreateCommand(stub, null);
            int counter = 0;
            foreach (var item in settings)
            {
                sbKeys.AppendFormat("{0},", item.Key);
                sbVals.AppendFormat("@{0},", counter.ToString());
                result.AddParam(item.Value);
                counter++;
            }
            if (counter > 0)
            {
                var keys = sbKeys.ToString().Substring(0, sbKeys.Length - 1);
                var vals = sbVals.ToString().Substring(0, sbVals.Length - 1);
                var sql = string.Format(stub, TableName, keys, vals);
                result.CommandText = sql;
            }
            else throw new InvalidOperationException("Can't parse this object to the database - there are no properties set");
            return result;
        }
        /// <summary>
        /// Creates a command for use with transactions - internal stuff mostly, but here for you to play with
        /// </summary>
        public virtual DbCommand CreateUpdateCommand(dynamic expando, object key)
        {
            var settings = (IDictionary<string, object>)expando;
            var sbKeys = new StringBuilder();
            var stub = "UPDATE {0} SET {1} WHERE {2} = @{3}";
            var args = new List<object>();
            var result = CreateCommand(stub, null);
            int counter = 0;
            foreach (var item in settings)
            {
                var val = item.Value;
                if (!item.Key.Equals(PrimaryKeyField, StringComparison.OrdinalIgnoreCase) && item.Value != null)
                {
                    result.AddParam(val);
                    sbKeys.AppendFormat("{0} = @{1}, \r\n", item.Key, counter.ToString());
                    counter++;
                }
            }
            if (counter > 0)
            {
                //add the key
                result.AddParam(key);
                //strip the last commas
                var keys = sbKeys.ToString().Substring(0, sbKeys.Length - 4);
                result.CommandText = string.Format(stub, TableName, keys, PrimaryKeyField, counter);
            }
            else throw new InvalidOperationException("No parsable object was sent in - could not divine any name/value pairs");
            return result;
        }

        /// <summary>
        /// Creates a command for use with transactions - internal stuff mostly, but here for you to play with
        /// </summary>
        public virtual DbCommand CreateUpdateWhereCommand(dynamic expando, string where = "", params object[] args)
        {
            var settings = (IDictionary<string, object>)expando;
            var sbKeys = new StringBuilder();
            string stub;

            if (!string.IsNullOrEmpty(where))
            {
                stub = where.Trim().StartsWith("where", StringComparison.OrdinalIgnoreCase) ? "UPDATE {0} SET {1} " : "UPDATE {0} SET {1} WHERE ";
                stub += where;
            }
            else
            {
                stub = "UPDATE {0} SET {1}";
            }


            var result = CreateCommand(stub, null, args);
            // not sure if we should do regex over where to count params... @ followed by number
            int counter = args.Length > 0 ? args.Length : 0;

            foreach (var item in settings)
            {
                var val = item.Value;
                if (!item.Key.Equals(PrimaryKeyField, StringComparison.OrdinalIgnoreCase) && item.Value != null)
                {
                    result.AddParam(val);
                    sbKeys.AppendFormat("{0} = @{1}, \r\n", item.Key, counter.ToString());
                    counter++;
                }
            }

            if (counter > 0)
            {
                //strip the last commas
                var keys = sbKeys.ToString().Substring(0, sbKeys.Length - 4);
                result.CommandText = string.Format(stub, TableName, keys);
            }
            else throw new InvalidOperationException("No parsable object was sent in - could not divine any name/value pairs");
            return result;
        }

        /// <summary>
        /// Removes one or more records from the DB according to the passed-in WHERE
        /// </summary>
        public virtual DbCommand CreateDeleteCommand(string where = "", object key = null, params object[] args)
        {
            var sql = string.Format("DELETE FROM {0} ", TableName);
            if (key != null)
            {
                sql += string.Format("WHERE {0}=@0", PrimaryKeyField);
                args = new object[] { key };
            }
            else if (!string.IsNullOrEmpty(where))
            {
                sql += where.Trim().StartsWith("where", StringComparison.OrdinalIgnoreCase) ? where : "WHERE " + where;
            }
            return CreateCommand(sql, null, args);
        }

        public bool IsValid(dynamic item)
        {
            Errors.Clear();
            Validate(item);
            return Errors.Count == 0;
        }

        //Temporary holder for error messages
        public IList<string> Errors = new List<string>();
        /// <summary>
        /// Adds a record to the database. You can pass in an Anonymous object, an ExpandoObject,
        /// A regular old POCO, or a NameValueColletion from a Request.Form or Request.QueryString
        /// </summary>
        public virtual dynamic Insert(object o)
        {
            var ex = o.ToExpando();
            if (!IsValid(ex))
            {
                throw new InvalidOperationException("Can't insert: " + String.Join("; ", Errors.ToArray()));
            }
            if (BeforeSave(ex))
            {
                using (dynamic conn = OpenConnection())
                {
                    var cmd = CreateInsertCommand(ex);
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "SELECT @@IDENTITY as newID";
                    ex.ID = cmd.ExecuteScalar();
                    Inserted(ex);
                }
                return ex;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Updates a record in the database. You can pass in an Anonymous object, an ExpandoObject,
        /// A regular old POCO, or a NameValueCollection from a Request.Form or Request.QueryString
        /// </summary>
        public virtual int Update(object o, object key)
        {
            var ex = o.ToExpando();
            if (!IsValid(ex))
            {
                throw new InvalidOperationException("Can't Update: " + String.Join("; ", Errors.ToArray()));
            }
            var result = 0;
            if (BeforeSave(ex))
            {
                result = Execute(CreateUpdateCommand(ex, key));
                Updated(ex);
            }
            return result;
        }
        /// <summary>
        /// Updates a all records in the database that match where clause. You can pass in an Anonymous object, an ExpandoObject,
        /// A regular old POCO, or a NameValueCollection from a Request.Form or Request.QueryString. Where works same same as
        /// in All().
        /// </summary>
        /// <returns>
        /// 0 - if no records updated or if you did fall into this method accenditently by passing null or "" to Update(object, object) method.
        /// n - number of records updated
        /// </returns>
        public virtual int Update(object o, string where = "1=1", params object[] args)
        {
            if (string.IsNullOrWhiteSpace(where))
            {
                return 0;
            }

            var ex = o.ToExpando();
            if (!IsValid(ex))
            {
                throw new InvalidOperationException("Can't Update: " + String.Join("; ", Errors.ToArray()));
            }
            var result = 0;
            if (BeforeSave(ex))
            {
                result = Execute(CreateUpdateWhereCommand(ex, where, args));
                Updated(ex);
            }
            return result;
        }
        /// <summary>
        /// Removes one or more records from the DB according to the passed-in WHERE
        /// </summary>
        public int Delete(object key = null, string where = "", params object[] args)
        {
            var deleted = this.Single(key);
            var result = 0;
            if (BeforeDelete(deleted))
            {
                result = Execute(CreateDeleteCommand(where: where, key: key, args: args));
                Deleted(deleted);
            }
            return result;
        }

        public void DefaultTo(string key, object value, dynamic item)
        {
            if (!ItemContainsKey(key, item))
            {
                var dc = (IDictionary<string, object>)item;
                dc[key] = value;
            }
        }

        //Hooks
        public virtual void Validate(dynamic item) { }
        public virtual void Inserted(dynamic item) { }
        public virtual void Updated(dynamic item) { }
        public virtual void Deleted(dynamic item) { }
        public virtual bool BeforeDelete(dynamic item) { return true; }
        public virtual bool BeforeSave(dynamic item) { return true; }

        //validation methods
        public virtual void ValidatesPresenceOf(object value, string message = "Required")
        {
            if (value == null)
                Errors.Add(message);
            if (String.IsNullOrEmpty(value.ToString()))
                Errors.Add(message);
        }
        //fun methods
        public virtual void ValidatesNumericalityOf(object value, string message = "Should be a number")
        {
            var type = value.GetType().Name;
            var numerics = new string[] { "Int32", "Int16", "Int64", "Decimal", "Double", "Single", "Float" };
            if (!numerics.Contains(type))
            {
                Errors.Add(message);
            }
        }
        public virtual void ValidateIsCurrency(object value, string message = "Should be money")
        {
            if (value == null)
                Errors.Add(message);
            decimal val = decimal.MinValue;
            decimal.TryParse(value.ToString(), out val);
            if (val == decimal.MinValue)
                Errors.Add(message);


        }
        public int Count()
        {
            return Count(TableName);
        }
        public int Count(string tableName, string where = "", params object[] args)
        {
            return (int)Scalar("SELECT COUNT(*) FROM " + tableName + " " + where, args);
        }

        /// <summary>
        /// A helpful query tool
        /// </summary>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            //parse the method
            var constraints = new List<string>();
            var counter = 0;
            var info = binder.CallInfo;
            // accepting named args only... SKEET!
            if (info.ArgumentNames.Count != args.Length)
            {
                throw new InvalidOperationException("Please use named arguments for this type of query - the column name, orderby, columns, etc");
            }
            //first should be "FindBy, Last, Single, First"
            var op = binder.Name;
            var columns = " * ";
            string orderBy = string.Format(" ORDER BY {0}", PrimaryKeyField);
            string sql = "";
            string where = "";
            var whereArgs = new List<object>();

            //loop the named args - see if we have order, columns and constraints
            if (info.ArgumentNames.Count > 0)
            {

                for (int i = 0; i < args.Length; i++)
                {
                    var name = info.ArgumentNames[i].ToLower();
                    switch (name)
                    {
                        case "orderby":
                            orderBy = " ORDER BY " + args[i];
                            break;
                        case "columns":
                            columns = args[i].ToString();
                            break;
                        default:
                            constraints.Add(string.Format(" {0} = @{1}", name, counter));
                            whereArgs.Add(args[i]);
                            counter++;
                            break;
                    }
                }
            }

            //Build the WHERE bits
            if (constraints.Count > 0)
            {
                where = " WHERE " + string.Join(" AND ", constraints.ToArray());
            }
            //probably a bit much here but... yeah this whole thing needs to be refactored...
            if (op.ToLower() == "count")
            {
                result = Scalar("SELECT COUNT(*) FROM " + TableName + where, whereArgs.ToArray());
            }
            else if (op.ToLower() == "sum")
            {
                result = Scalar("SELECT SUM(" + columns + ") FROM " + TableName + where, whereArgs.ToArray());
            }
            else if (op.ToLower() == "max")
            {
                result = Scalar("SELECT MAX(" + columns + ") FROM " + TableName + where, whereArgs.ToArray());
            }
            else if (op.ToLower() == "min")
            {
                result = Scalar("SELECT MIN(" + columns + ") FROM " + TableName + where, whereArgs.ToArray());
            }
            else if (op.ToLower() == "avg")
            {
                result = Scalar("SELECT AVG(" + columns + ") FROM " + TableName + where, whereArgs.ToArray());
            }
            else
            {

                //build the SQL
                sql = "SELECT TOP 1 " + columns + " FROM " + TableName + where;
                var justOne = op.StartsWith("First") || op.StartsWith("Last") || op.StartsWith("Get") || op.StartsWith("Single");

                //Be sure to sort by DESC on the PK (PK Sort is the default)
                if (op.StartsWith("Last"))
                {
                    orderBy = orderBy + " DESC ";
                }
                else
                {
                    //default to multiple
                    sql = "SELECT " + columns + " FROM " + TableName + where;
                }

                if (justOne)
                {
                    //return a single record
                    result = Query(sql + orderBy, whereArgs.ToArray()).FirstOrDefault();
                }
                else
                {
                    //return lots
                    result = Query(sql + orderBy, whereArgs.ToArray());
                }
            }
            return true;
        }
    }

    #region Entity Framework Code First

    /// <summary>
    /// Clase creada para trabajar con Entity Framework Code first
    /// </summary>
    public class EntityDBContext : DbContext
    {
        /// <summary>
        /// Constructor donde le pasamos como parametro a DbContext el nombre de la cadena de conexión
        /// </summary>
        public EntityDBContext() : base("name=Core") { }

        /// <summary>
        /// Constructor donde le pasamos como parametro a DbContext el nombre de la cadena de conexión
        /// </summary>
        public EntityDBContext(string dataBase) : base(dataBase) { }

        /// <summary>
        /// Coleccion que representa la tabla Holiday en la DB Core
        /// </summary>
        public DbSet<Holiday> Holidays { get; set; }

       /// <summary>
        /// @01
        /// Coleccion que representa la tabla BankPayments en la DB Core
       /// </summary>
        public DbSet<BankPayments> BankPayments { get; set; }
        /// <summary>
        /// @02
        /// Coleccion que representa la tabla LogErrorBankPayments en la DB Core
        /// </summary>
        public DbSet<LogErrorBankPayments> LogErrorBankPayments { get; set; }

        /// <summary>
        /// @03 Colección que representa la tabla ArrearsAndDefaultsRules en la BD BelcorpBRACore
        /// </summary>
        public DbSet<ArrearAndDefaultRule> ArrearsAndDefaultsRules { get; set; }

        /// <summary>
        /// @03 Colección que representa la tabla LoadingErrors en la BD BelcorpBRACore
        /// </summary>
        //public DbSet<LoadingError> LoadingErrors { get; set; }

        /// <summary>
        /// @03 Colección que representa la tabla OverduePayments en la BD BelcorpBRACore
        /// </summary>
        //public DbSet<OverduePayment> OverduePayments { get; set; }

        /// <summary>
        /// @03 Colección que representa la tabla ReportedOverduePayments en la BD BelcorpBRACore
        /// </summary>
        public DbSet<ReportedOverduePayment> ReportedOverduePayments { get; set; }
        
        /// <summary>
        /// <summary> 
        /// Coleccion que representa la tabla BonusRequirements
        /// </summary>
        public DbSet<BonusRequirementsTable> BonusRequirements { get; set; }

        ///// <summary>
        ///// Coleccion que representa la tabla BonusTypes
        ///// </summary>
        public DbSet<BonusTypesTable> BonusTypes { get; set; }

        ///// <summary>
        ///// Coleccion que representa la tabla Plans
        ///// </summary>
        public DbSet<PlansTable> Plans { get; set; }

        /// <summary>
        /// Coleccion que representa la tabla AccountConsistencyStatus
        /// </summary>
        public DbSet<AccountConsistencyStatus> AccountConsistencyStatuses { get; set; }

        /// <summary>
        /// Coleccion que representa la tabla ActivityStatusesTable
        /// </summary>
        public DbSet<ActivityStatus> ActivityStatuses { get; set; }


        /// <summary>
        /// Coleccion que representa la tabla LogCommissions
        /// </summary>
        public DbSet<LogCommissionTable> LogCommissions { get; set; }

        /// <summary>
        /// Coleccion que representa la tabla Titles
        /// </summary>
        public DbSet<TitleTable> Titles { get; set; }

        /// <summary>
        /// Coleccion que representa la tabla Titles
        /// </summary>
        public DbSet<OrderTable> Orders { get; set; }
        
        /// <summary>
        /// Coleccion que representa la tabla Titles
        /// </summary>
        public DbSet<OrderCustomerTable> OrderCustomers { get; set; }
        
        /// <summary>
        /// Coleccion que representa la tabla Titles
        /// </summary>
        public DbSet<OrderItemTable> OrderItems { get; set; }

        /// <summary>
        /// Coleccion que representa la tabla Titles
        /// </summary>
        public DbSet<OrderItemPriceTable> OrderItemPrices { get; set; }

        public DbSet<BonusValueTable> BonusValues { get; set; }

        /// <summary>
        /// Collection that represents Promo.PrmotionTypes table.
        /// </summary>
        public DbSet<PromoPromotionTypeTable> PromoPromotionTypes { get; set; }

        /// <summary>
        /// Collection that represents Promo.Promotions table.
        /// </summary>
        public DbSet<PromoPromotionTable> PromoPromotions { get; set; }

        /// <summary>
        /// Collection that represents Promo.Promotions table.
        /// </summary>
        public DbSet<PromoPromotionRewardTable> PromoPromotionRewards { get; set; }

        /// <summary>
        /// Collection that represents Promo.Promotions table.
        /// </summary>
        public DbSet<PromoPromotionRewardEffectTable> PromoPromotionRewardEffects { get; set; }

        /// <summary>
        /// Collection that represents Promo.Promotions table.
        /// </summary>
        public DbSet<PromoPromotionQualificationTable> PromoPromotionQualifications { get; set; }

        /// <summary>
        /// Collection that represents Promo.PromotionStatusTypes Table
        /// </summary>
        public DbSet<PromoPromotionStatusTypeTable> PromoPromotionStatusTypes { get; set; }

        /// <summary>
        /// Collection that represents Promo.PromotionTypeConfigurations Table
        /// </summary>
        public DbSet<PromoPromotionTypeConfigurationTable> PromoPromotionTypeConfigurations { get; set; }

        /// <summary>
        /// Collection that represents Promo.PromotionTypeConfigurationsPerOrderTable Table
        /// </summary>
        public DbSet<PromoPromotionTypeConfigurationsPerOrderTable> PromoPromotionTypeConfigurationsPerOrders { get; set; }

        /// <summary>
        /// Collection that represents Promo.PromotionConfigurationControl Table
        /// </summary>
        public DbSet<PromoPromotionConfigurationControlTable> PromoPromotionConfigurationControls { get; set; }

        /// <summary>
        /// Collection that represents Promo.PromotionTypeConfigurationPerPromotion Table
        /// </summary>
        public DbSet<PromoPromotionTypeConfigurationPerPromotionTable> PromoPromotionTypeConfigurationPerPromotions { get; set; }

        /// <summary>
        /// Collection that represents Promo.PromotionRewardEffectApplyOrderItemPropertyValues Table
        /// </summary>
        public DbSet<PromoPromotionRewardEffectApplyOrderItemPropertyValueTable> PromoPromotionRewardEffectApplyOrderItemPropertyValues { get; set; }

        /// <summary>
        /// Collection that represents Promo.PromotionRewardEffectApplyOrderItemPropertyValues Table
        /// </summary>
        public DbSet<PromoPromotionRewardEffectReduceOrderItemPropertyValuesMarketValueTable> PromoPromotionRewardEffectReduceOrderItemPropertyValuesMarketValues { get; set; }


        /// <summary>
        /// Collection that represents Promo.PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmount Table
        /// </summary>
        public DbSet<PromoPromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountTable> PromoPromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmounts { get; set; }

        /// <summary>
        /// Collection that represents Promo.PromoPromotionQualificationCustomerPriceTypeTotalRange Table
        /// </summary>
        public DbSet<PromoPromotionQualificationCustomerPriceTypeTotalRangeTable> PromoPromotionQualificationCustomerPriceTypeTotalRanges { get; set; }
        
        /// <summary>
        /// Collection that represents Promo.PromoPromotionQualificationCustomerPriceTypeTotalRange Table
        /// </summary>
        public DbSet<PromotionQualificationCustomerSubtotalRangeCurrencyAmountTable> PromotionQualificationCustomerSubtotalRangeCurrencyAmounts { get; set; }
        
            /// <summary>
        /// Collection that represents ProductPrices Table
        /// </summary>
        public DbSet<ProductPriceTable> ProductPrices { get; set; }

        /// <summary>
        /// Coleccion que representa la tabla Activities
        /// </summary>
        public DbSet<ActivityTable> Activities { get; set; }

        /// <summary>
        /// Coleccion que representa la tabla ProductPriceTypes
        /// </summary>
        public DbSet<ProductPriceTypeTable> ProductPriceTypes { get; set; }

        /// <summary>
        /// Coleccion que representa la tabla ScopeLevels
        /// </summary>
        public DbSet<ScopeLevelTable> ScopeLevels { get; set; }

        /// <summary>
        /// Modificar de ser necesario para inicializar datos al momento de persistir con la DB
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

    /// <summary>
    /// Connection keys
    /// </summary>
    public class ConnectionStrings
    {
        /// <summary>
        /// Commission connection key name
        /// </summary>
        public const string BelcorpCommission = "Commissions";

        /// <summary>
        /// Core connection key name
        /// </summary>
        public const string BelcorpCore = "Core";

        /// <summary>
        /// Mail connection key name
        /// </summary>
        public const string BelcorpMail = "Mail";
    }
    #endregion

    internal sealed class ConfiguracionDAC
    {
        static SqlConnection conexion;
        static string cadenaConexion;

        static ConfiguracionDAC()
        {
            cadenaConexion = ConfigurationManager.ConnectionStrings["Core"].ConnectionString;
            conexion = new SqlConnection(cadenaConexion);
        }


        public static SqlConnection obtenerConexion()
        {
            if (conexion.State == ConnectionState.Closed)
                conexion.Open();
            return conexion;
        }


        public static SqlCommand obtenerComando(string nombreProcedimientoAlmacenado)
        {
            SqlCommand comando = new SqlCommand(nombreProcedimientoAlmacenado, obtenerConexion());
            comando.CommandType = CommandType.StoredProcedure;
            return comando;
        }
        //fsv

    }

    public class ExtendedTimeoutContext : DbContext
    {
        public ExtendedTimeoutContext(string connectionString)
            : base(connectionString)
        {
            // Get the ObjectContext related to this DbContext
            var objectContext = (this as IObjectContextAdapter).ObjectContext;

            // Sets the command timeout for all the commands
            // to 2 min instead of the default 30 sec
            objectContext.CommandTimeout = 120;
        }
    }
}
