using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SqlServer.Server;
using NetSteps.Sql;

/// <summary>
/// Author: John Egbert
/// Created: 3/26/2010
/// </summary>
public partial class Triggers
{
    #region Enumerations
    public enum AuditChangeType : byte
    {
        NotSet = 0,
        Insert = 1,
        Update = 2,
        Delete = 3
    }
    #endregion

    #region Helper Classes
    public class AuditContextInfo
    {
        public short AuditTableID { get; set; }
        public AuditChangeType AuditChangeTypeID { get; set; }
        public short ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public int UserID { get; set; }
        public string ChangeDateTime { get; set; }
        public string SqlUserName { get; set; }
        public int SqlUserNameID { get; set; }
        public string MachineName { get; set; }
        public int MachineNameID { get; set; }

        public int PK { get; set; }
        public string PKs { get; set; }
    }
    #endregion

    /// <summary>
    /// Author: John Egbert
    /// Description: Sql CLR trigger to log 'Audit' value changes to tables.
    /// http://social.technet.microsoft.com/Forums/de-DE/sqlnetfx/thread/60ed909b-51ac-425c-8e00-4d7978f39f70
    /// Has support for only Auditing certain columns and/or ignoring certain columns like dateType TimeStamp - JHE
    /// Created: 3/26/2010
    /// </summary>
    [Microsoft.SqlServer.Server.SqlTrigger(Name = "AuditTrigger", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void AuditTrigger()
    {
        try
        {
            SqlTriggerContext triggerContext = SqlContext.TriggerContext; // Trigger Context
            string alteredTableName = string.Empty; // Where we store the Altered Table's Name
            DataRow insertedDataRow; // DataRow to hold the inserted values
            DataRow deletedDataRow; // DataRow to how the deleted/overwritten values
            DataRow auditDataRow; // Audit DataRow to build our Audit entry with

            using (SqlConnection conn = new SqlConnection("context connection=true")) // Our Connection
            {
                conn.Open(); // Open the Connection

                AuditContextInfo auditContextInfo = new AuditContextInfo();
                auditContextInfo.ChangeDateTime = DateTime.Now.ToString();

                // Build the AuditAdapter and Matching Table
                SqlDataAdapter auditAdapter = new SqlDataAdapter("SELECT TOP 1 * FROM AuditLogs WHERE 1=0", conn);
                DataTable auditTable = new DataTable();
                auditAdapter.FillSchema(auditTable, SchemaType.Source);
                SqlCommandBuilder auditCommandBuilder = new SqlCommandBuilder(auditAdapter); // Populates the Insert command for us

                // Get the inserted values
                SqlDataAdapter loader = new SqlDataAdapter("SELECT * from INSERTED", conn);
                DataTable inserted = new DataTable();
                loader.Fill(inserted);

                // No Data updated. Returning. - JHE
                if (inserted.Columns == null || inserted.Columns.Count == 0)
                    return;

                DataTable deleted = new DataTable();
                if (triggerContext.TriggerAction != TriggerAction.Insert) // To optimize performance
                {
                    // Get the deleted and/or overwritten values
                    loader.SelectCommand.CommandText = "SELECT * from DELETED";
                    loader.Fill(deleted);
                }

                // Retrieve the Name of the Table that currently has a lock from the executing command(i.e. the one that caused this trigger to fire)
                // (This query is the most Expensive thing in this Trigger by far!!!! - JHE)
                //SqlCommand sqlCommand = new SqlCommand("SELECT object_name(resource_associated_entity_id) FROM sys.dm_tran_locks WHERE request_session_id = @@spid and resource_type = 'OBJECT'", conn);
                //alteredTableName = sqlCommand.ExecuteScalar().ToString();

                alteredTableName = inserted.Columns[0].ColumnName.Substring(0, inserted.Columns[0].ColumnName.Length - 2); // Not Plural - JHE
                alteredTableName = Helpers.Pluralize(alteredTableName);


                // Method of getting tableName by Schema querying - JHE
                //string schema = GetSchema("Inserted", conn);
                //alteredTableName = DetermineTableNameFromSchema(schema, conn);
                //alteredTableName = alteredTableName.Substring(alteredTableName.IndexOf('.') + 1).Replace("[", string.Empty).Replace("]", string.Empty);


                auditContextInfo.AuditTableID = (short)SqlHelpers.SqlGetInt(string.Format("SELECT AuditTableID FROM AuditTables WHERE Name = '{0}'", alteredTableName), conn);
                // Insert new AuditTable if auditContextInfo.AuditTableID = 0 - JHE
                if (auditContextInfo.AuditTableID == 0)
                {
                    SqlHelpers.SqlRun(string.Format("INSERT INTO AuditTables (Name) VALUES ('{0}')", alteredTableName), conn);
                    auditContextInfo.AuditTableID = (short)SqlHelpers.SqlGetInt(string.Format("SELECT AuditTableID FROM AuditTables WHERE Name = '{0}'", alteredTableName), conn);
                }

                auditContextInfo.ApplicationName = SqlHelpers.SqlGetString("Select APP_NAME() as currentApplication", conn);
                auditContextInfo.ApplicationID = (short)SqlHelpers.SqlGetInt(string.Format("SELECT ApplicationID FROM Applications WHERE Name = '{0}'", auditContextInfo.ApplicationName), conn);
                // Insert new Application if auditContextInfo.ApplicationName = 0 - JHE
                if (auditContextInfo.ApplicationID == 0)
                {
                    SqlHelpers.SqlRun(string.Format("INSERT INTO Applications (Name) VALUES ('{0}')", auditContextInfo.ApplicationName), conn);
                    auditContextInfo.ApplicationID = (short)SqlHelpers.SqlGetInt(string.Format("SELECT ApplicationID FROM Applications WHERE Name = '{0}'", auditContextInfo.ApplicationName), conn);
                }

                // Retrieve the SqlUserName of the current Database User
                auditContextInfo.SqlUserName = SqlHelpers.SqlGetString("SELECT system_user", conn);
                auditContextInfo.SqlUserNameID = (short)SqlHelpers.SqlGetInt(string.Format("SELECT AuditSqlUserNameID FROM AuditSqlUserNames WHERE Name = '{0}'", auditContextInfo.SqlUserName), conn);
                // Insert new SqlUserName if auditContextInfo.SqlUserNameID = 0 - JHE
                if (auditContextInfo.SqlUserNameID == 0)
                {
                    SqlHelpers.SqlRun(string.Format("INSERT INTO AuditSqlUserNames (Name) VALUES ('{0}')", auditContextInfo.SqlUserName), conn);
                    auditContextInfo.SqlUserNameID = (short)SqlHelpers.SqlGetInt(string.Format("SELECT AuditSqlUserNameID FROM AuditSqlUserNames WHERE Name = '{0}'", auditContextInfo.SqlUserName), conn);
                }

                auditContextInfo.MachineName = SqlHelpers.SqlGetString("Select Host_Name() as Computer_Name", conn);
                auditContextInfo.MachineNameID = (short)SqlHelpers.SqlGetInt(string.Format("SELECT AuditMachineNameID FROM AuditMachineNames WHERE Name = '{0}'", auditContextInfo.MachineName), conn);
                // Insert new SqlUserName if auditContextInfo.SqlUserNameID = 0 - JHE
                if (auditContextInfo.MachineNameID == 0)
                {
                    SqlHelpers.SqlRun(string.Format("INSERT INTO AuditMachineNames (Name) VALUES ('{0}')", auditContextInfo.MachineName), conn);
                    auditContextInfo.MachineNameID = (short)SqlHelpers.SqlGetInt(string.Format("SELECT AuditMachineNameID FROM AuditMachineNames WHERE Name = '{0}'", auditContextInfo.MachineName), conn);
                }

                Type timeStamp = typeof(System.Byte[]);
                Type dateTime = typeof(DateTime?);
                Type stringType = typeof(string);

                var primaryKeys = GetPrimaryKeys(alteredTableName, conn);
                var ignoredColumns = GetIgnoredColumns(auditContextInfo.AuditTableID, conn);

                // Create list of properties to Audit for the current table (Exclude Primary Keys, any column in the AuditTableIgnoredColumns, and DataVersion (timeStamp) columns. - JHE
                System.Collections.Generic.Dictionary<string, string> columnsToAudit = new System.Collections.Generic.Dictionary<string, string>();
                foreach (DataColumn column in inserted.Columns)
                    if (!primaryKeys.Contains(column.ColumnName) && column.DataType != timeStamp && !ignoredColumns.Contains(column.ColumnName) && !columnsToAudit.ContainsKey(column.ColumnName))
                        columnsToAudit.Add(column.ColumnName, column.ColumnName);

                switch (triggerContext.TriggerAction) // Switch on the Action occurring on the Table
                {
                    case TriggerAction.Update:
                        if (inserted.Rows != null && inserted.Rows.Count > 0)
                        {
                            insertedDataRow = inserted.Rows[0]; // Get the inserted values in row form
                            deletedDataRow = deleted.Rows[0]; // Get the overwritten values in row form
                            auditContextInfo.PK = GetSingularPrimaryKey(primaryKeys, insertedDataRow);
                            if (auditContextInfo.PK == 0)
                                auditContextInfo.PKs = PrimaryKeyStringBuilder(primaryKeys, insertedDataRow);
                            auditContextInfo.AuditChangeTypeID = AuditChangeType.Update;
                            foreach (DataColumn column in inserted.Columns) // Walk through all possible Table Columns
                            {
                                if (columnsToAudit.ContainsKey(column.ColumnName) && !IsValueEqual(insertedDataRow[column.Ordinal], deletedDataRow[column.Ordinal])) // If value changed
                                {
                                    auditDataRow = auditTable.NewRow();
                                    auditDataRow["AuditTableColumnID"] = GetAuditTableColumnID(auditContextInfo.AuditTableID, column.ColumnName, conn);
                                    auditDataRow["OldValue"] = GetTruncatedValue(deletedDataRow[column.Ordinal].ToString());
                                    auditDataRow["NewValue"] = GetTruncatedValue(insertedDataRow[column.Ordinal].ToString());

                                    SetCommonFields(auditDataRow, insertedDataRow, auditContextInfo);
                                    SetAuditFieldsOnTable(auditDataRow, insertedDataRow);

                                    auditTable.Rows.InsertAt(auditDataRow, 0); // Insert the entry
                                }
                            }
                        }
                        break;
                    case TriggerAction.Insert:
                        if (inserted.Rows != null && inserted.Rows.Count > 0)
                        {
                            insertedDataRow = inserted.Rows[0];
                            auditContextInfo.PK = GetSingularPrimaryKey(primaryKeys, insertedDataRow);
                            if (auditContextInfo.PK == 0)
                                auditContextInfo.PKs = PrimaryKeyStringBuilder(primaryKeys, insertedDataRow);
                            auditContextInfo.AuditChangeTypeID = AuditChangeType.Insert;
                            foreach (DataColumn column in inserted.Columns)
                            {
                                if (columnsToAudit.ContainsKey(column.ColumnName) && !IsValueEqual(DBNull.Value, insertedDataRow[column.Ordinal]))
                                {
                                    // Build an Audit Entry
                                    auditDataRow = auditTable.NewRow();
                                    auditDataRow["AuditTableColumnID"] = GetAuditTableColumnID(auditContextInfo.AuditTableID, column.ColumnName, conn);
                                    auditDataRow["OldValue"] = null;
                                    auditDataRow["NewValue"] = GetTruncatedValue(insertedDataRow[column.Ordinal].ToString());

                                    SetCommonFields(auditDataRow, insertedDataRow, auditContextInfo);
                                    SetAuditFieldsOnTable(auditDataRow, insertedDataRow);

                                    auditTable.Rows.InsertAt(auditDataRow, 0); // Insert the Entry
                                }
                            }
                        }
                        break;
                    case TriggerAction.Delete:
                        if (deleted.Rows != null && deleted.Rows.Count > 0)
                        {
                            deletedDataRow = deleted.Rows[0];
                            auditContextInfo.PK = GetSingularPrimaryKey(primaryKeys, deletedDataRow);
                            if (auditContextInfo.PK == 0)
                                auditContextInfo.PKs = PrimaryKeyStringBuilder(primaryKeys, deletedDataRow);
                            auditContextInfo.AuditChangeTypeID = AuditChangeType.Delete;
                            foreach (DataColumn column in inserted.Columns)
                            {
                                if (columnsToAudit.ContainsKey(column.ColumnName))
                                {
                                    // Build and Audit Entry
                                    auditDataRow = auditTable.NewRow();
                                    auditDataRow["AuditTableColumnID"] = GetAuditTableColumnID(auditContextInfo.AuditTableID, column.ColumnName, conn);
                                    auditDataRow["OldValue"] = GetTruncatedValue(deletedDataRow[column.Ordinal].ToString());
                                    auditDataRow["NewValue"] = null;

                                    SetCommonFields(auditDataRow, deletedDataRow, auditContextInfo);
                                    SetAuditFieldsOnTable(auditDataRow, deletedDataRow);

                                    auditTable.Rows.InsertAt(auditDataRow, 0); // Insert the Entry
                                }
                            }
                        }
                        break;
                    default:
                        //Do Nothing
                        break;
                }
                auditAdapter.Update(auditTable); // Write all Audit Entries back to AuditTable
                conn.Close(); // Close the Connection
            }
        }
        catch
        {
            // Swallow the exception to prevent audit exceptions from interfering with other database operations
        }
    }

    private static string GetTruncatedValue(string value)
    {
        if (value.Length > 4000) // Current DB column limit - JHE
            value = value.Substring(0, 3999);
        return value;
    }

    #region Helper Methods
    // Helper function that takes a Table of the Primary Key Column Names and the modified rows Values
    // and builds a string of the form "<PKColumn1Name=Value1>,PKColumn2Name=Value2>,......"
    public static string PrimaryKeyStringBuilder(List<string> primaryKeys, DataRow valuesDataRow)
    {
        string temp = String.Empty;
        foreach (var primaryKey in primaryKeys) // for all Primary Keys of the Table that is being changed
        {
            temp = String.Concat(temp, String.Concat("<", primaryKey, "=", valuesDataRow[primaryKey].ToString(), ">,"));
        }
        return temp;
    }

    public static string PrimaryKeyStringBuilder(DataTable primaryKeysTable, DataRow valuesDataRow)
    {
        string temp = String.Empty;
        foreach (DataRow row in primaryKeysTable.Rows) // for all Primary Keys of the Table that is being changed
        {
            temp = String.Concat(temp, String.Concat("<", row[0].ToString(), "=", valuesDataRow[row[0].ToString()].ToString(), ">,"));
        }
        return temp;
    }

    public static int GetSingularPrimaryKey(List<string> primaryKeys, DataRow valuesDataRow)
    {
        if (primaryKeys.Count == 1)
            return Convert.ToInt32(valuesDataRow[primaryKeys[0]].ToString());
        else
            return 0;
    }

    public static int GetSingularPrimaryKey(DataTable primaryKeysTable, DataRow valuesDataRow)
    {
        if (primaryKeysTable.Rows.Count == 1)
            return Convert.ToInt32(valuesDataRow[primaryKeysTable.Rows[0][0].ToString()].ToString());
        else
            return 0;
    }

    public static int GetAuditTableColumnID(int auditTableID, string columnName, SqlConnection conn)
    {
        int result = (short)SqlHelpers.SqlGetInt(string.Format("SELECT AuditTableColumnID FROM AuditTableColumns WHERE AuditTableID = '{0}' AND ColumnName = '{1}'", auditTableID, columnName), conn);
        // Insert new AuditTableColumn if result = 0 - JHE
        if (result == 0)
        {
            SqlHelpers.SqlRun(string.Format("INSERT INTO AuditTableColumns (AuditTableID, ColumnName) VALUES ('{0}', '{1}')", auditTableID, columnName), conn);
            result = (short)SqlHelpers.SqlGetInt(string.Format("SELECT AuditTableColumnID FROM AuditTableColumns WHERE AuditTableID = '{0}' AND ColumnName = '{1}'", auditTableID, columnName), conn);
        }
        return result;
    }

    public static void SetCommonFields(DataRow auditDataRow, DataRow valuesDataRow, AuditContextInfo auditContextInfo)
    {
        auditDataRow["ApplicationID"] = auditContextInfo.ApplicationID;
        auditDataRow["AuditTableID"] = auditContextInfo.AuditTableID;
        auditDataRow["DateChanged"] = auditContextInfo.ChangeDateTime;
        auditDataRow["AuditSqlUserNameID"] = auditContextInfo.SqlUserNameID;
        auditDataRow["AuditMachineNameID"] = auditContextInfo.MachineNameID;
        auditDataRow["PKs"] = auditContextInfo.PKs;
        auditDataRow["PK"] = auditContextInfo.PK;
        auditDataRow["AuditChangeTypeID"] = (short)auditContextInfo.AuditChangeTypeID;
    }

    public static void SetAuditFieldsOnTable(DataRow auditDataRow, DataRow valuesDataRow)
    {
        //if (valuesDataRow.Table.Columns.Contains("ModifiedByApplicationID"))
        //    auditDataRow["ApplicationID"] = valuesDataRow["ModifiedByApplicationID"];

        if (valuesDataRow.Table.Columns.Contains("ModifiedByUserID"))
            auditDataRow["UserID"] = valuesDataRow["ModifiedByUserID"];

        if (string.IsNullOrEmpty(auditDataRow["UserID"].ToString()) && valuesDataRow.Table.Columns.Contains("UserID"))
            auditDataRow["UserID"] = valuesDataRow["UserID"];
    }

    // Currently will mark a property as not-changed if one value is null and the other is an empty string. - JHE
    private static bool IsValueEqual(object obj1, object obj2)
    {
        // TODO: Finish deploying this and test it - JHE
        if ((IsNullOrEmptyOrDbNull(obj1) && IsNullOrEmptyOrDbNull(obj2)) || (IsNullOrEmptyOrDbNull(obj2) && IsNullOrEmptyOrDbNull(obj1.ToString())))
            return true;
        else
            return obj1.Equals(obj2);
    }

    private static bool IsNullOrEmptyOrDbNull(object obj1)
    {
        if (obj1 == DBNull.Value || obj1.ToString() == string.Empty)
            return true;
        else
            return false;
    }


    // We need to expire this cache quickly every 15/30 sec maybe. - JHE
    // Adapted the following command from a T-SQL audit trigger by Nigel Rivett
    // http://www.nigelrivett.net/AuditTrailTrigger.html
    //private readonly static object _lock = new object();
    private readonly static ExpirationInfo _pkExpirationInfo = new ExpirationInfo() { ExpirationDate = DateTime.Now, TimeToLive = TimeSpan.FromSeconds(15) };
    private readonly static Dictionary<string, List<string>> _tablePrimaryKeyInfo = new Dictionary<string, List<string>>();
    private static List<string> GetPrimaryKeys(string tableName, SqlConnection conn)
    {
        List<string> result = new List<string>();

        // SAFE Triggers won't allow the lock (_lock) - JHE
        //if (_tablePrimaryKeyInfo.Count == 0 || _expirationDate < DateTime.Now.Add(_timeToLive))
        {
            //lock (_lock)
            {
                if (_tablePrimaryKeyInfo.Count == 0 || _pkExpirationInfo.IsExpired())
                {
                    _tablePrimaryKeyInfo.Clear();
                    //SqlContext.Pipe.Send("Loading PK cache..");

                    // First time, fill the collection with schema info
                    string sql = @"SELECT pk.TABLE_NAME, c.COLUMN_NAME
                        FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
                        INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
                        WHERE CONSTRAINT_TYPE = 'PRIMARY KEY'
                        AND c.TABLE_NAME = pk.TABLE_NAME
                        AND c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME
                        ORDER BY pk.TABLE_NAME";
                    SqlDataAdapter ds = new SqlDataAdapter(sql, conn);
                    DataTable tables = new DataTable();
                    ds.Fill(tables);

                    // Because this is not thread-safe, we have to assume _tablePrimaryKeyInfo could be cleared at any time.
                    var tempDictionary = new Dictionary<string, List<string>>();
                    foreach (DataRow row in tables.Rows)
                    {
                        string eachTableName = row["TABLE_NAME"].ToString();
                        string columnName = row["COLUMN_NAME"].ToString();

                        if (!tempDictionary.ContainsKey(eachTableName))
                            tempDictionary.Add(eachTableName, new List<string>());

                        tempDictionary[eachTableName].Add(columnName);
                    }
                    foreach (var key in tempDictionary.Keys)
                    {
                        _tablePrimaryKeyInfo[key] = tempDictionary[key];
                    }

                    _pkExpirationInfo.Reset();
                }
            }
        }

        try
        {
            // Not thread-safe
            if (_tablePrimaryKeyInfo.ContainsKey(tableName))
            {
                result = _tablePrimaryKeyInfo[tableName];
            }
        }
        catch
        {
            // Swallow the exception
        }

        return result;
    }

    private readonly static ExpirationInfo _icExpirationInfo = new ExpirationInfo() { ExpirationDate = DateTime.Now, TimeToLive = TimeSpan.FromSeconds(15) };
    private readonly static Dictionary<int, List<string>> _tableIgnoredColumns = new Dictionary<int, List<string>>();
    private static List<string> GetIgnoredColumns(int auditTableID, SqlConnection conn)
    {
        List<string> result = new List<string>();

        // SAFE Triggers won't allow the lock (_lock) - JHE
        //if (_tablePrimaryKeyInfo.Count == 0 || _expirationDate < DateTime.Now.Add(_timeToLive))
        {
            //lock (_lock)
            {
                if (_tableIgnoredColumns.Count == 0 || _icExpirationInfo.IsExpired())
                {
                    _tableIgnoredColumns.Clear();
                    //SqlContext.Pipe.Send("Loading IC cache..");

                    // First time, fill the collection with schema info
                    string sql = @"SELECT ColumnName
                        FROM AuditTableIgnoredColumns
                        WHERE AuditTableID = 'PRIMARY KEY'";
                    sql = sql.Replace("'PRIMARY KEY'", auditTableID.ToString());
                    SqlDataAdapter ds = new SqlDataAdapter(sql, conn);
                    DataTable tables = new DataTable();
                    ds.Fill(tables);

                    // Because this is not thread-safe, we have to assume _tableIgnoredColumns could be cleared at any time.
                    var tempDictionary = new Dictionary<int, List<string>>();
                    foreach (DataRow row in tables.Rows)
                    {
                        string columnName = row["ColumnName"].ToString();
                        if (columnName != null)
                            columnName = columnName.Trim();

                        if (!tempDictionary.ContainsKey(auditTableID))
                            tempDictionary.Add(auditTableID, new List<string>());

                        tempDictionary[auditTableID].Add(columnName);
                    }
                    foreach (var key in tempDictionary.Keys)
                    {
                        _tableIgnoredColumns[key] = tempDictionary[key];
                    }

                    _icExpirationInfo.Reset();
                }
            }
        }

        try
        {
            // Not thread-safe
            if (_tableIgnoredColumns.ContainsKey(auditTableID))
            {
                result = _tableIgnoredColumns[auditTableID];
            }
        }
        catch
        {
            // Swallow the exception
        }

        return result;
    }
    #endregion

    #region TableLookupBySchema
    // http://msmvps.com/blogs/theproblemsolver/archive/2007/02/19/determining-the-table-updated-inside-of-a-sqltrigger.aspx
    private readonly static Dictionary<string, string> _schemas = new Dictionary<string, string>();
    private static string DetermineTableNameFromSchema(string schema, SqlConnection conn)
    {
        string result = "";

        if (_schemas.Count == 0)
        {
            // First time, fill the collection with schema info
            string sql = "SELECT * FROM INFORMATION_SCHEMA.Tables WHERE TABLE_TYPE = 'BASE TABLE'";
            SqlDataAdapter ds = new SqlDataAdapter(sql, conn);
            DataTable tables = new DataTable();
            ds.Fill(tables);

            foreach (DataRow row in tables.Rows)
            {
                string tableSchema = row["TABLE_SCHEMA"].ToString();
                string tableName = row["TABLE_NAME"].ToString();
                string fullName = string.Format("[{0}].[{1}]", tableSchema, tableName);
                string tempSchema = GetSchema(fullName, conn);
                if (!_schemas.ContainsKey(tempSchema))
                    _schemas.Add(tempSchema, fullName);
            }
        }

        _schemas.TryGetValue(schema, out result);

        return result;
    }
    private static string GetSchema(string tableName, SqlConnection conn)
    {
        string schema = "";
        string sql = string.Format("SELECT TOP 1 * FROM {0}", tableName);
        SqlDataAdapter ds = new SqlDataAdapter(sql, conn);
        DataTable dt = new DataTable();
        ds.FillSchema(dt, SchemaType.Source);

        // Rename the table as we want to match it against the inserted table
        dt.TableName = "Inserted";
        // Always remove the primary key as it won't be present on the Inserted table
        dt.PrimaryKey = null;

        StringWriter sw = new StringWriter();
        dt.WriteXmlSchema(sw, false);
        schema = sw.ToString();
        sw.Close();

        return schema;
    }
    #endregion
}