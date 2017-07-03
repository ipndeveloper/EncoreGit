using NetSteps.Validation.Common.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.BatchProcess.TempTableWriters.Implementation.Script
{
    public static class TableScriptWriter
    {
        public static TableScriptContainer GetScriptContainer(TempTableType tableType, IRecord record, string tempSchemaName)
        {
            var scriptContainer = new TableScriptContainer(
                CreateTableCreationScript(record, tableType, tempSchemaName),
                tableType == TempTableType.Update ? 
                    CreateUpdateScript(record, tempSchemaName, tableType) :
                    CreateInsertScript(record, tempSchemaName, tableType)
            );
            return scriptContainer;
        }

        private static string CreateTableCreationScript(IRecord record, TempTableType tableType, string tempSchemaName)
        {
            StringBuilder scriptBuilder = new StringBuilder();

            // drop and create
            scriptBuilder.Append("IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'");
            scriptBuilder.Append(tempSchemaName);
            scriptBuilder.Append("')");
            scriptBuilder.Append(Environment.NewLine);
            scriptBuilder.Append("BEGIN");
            scriptBuilder.Append(Environment.NewLine);
            scriptBuilder.Append("\t");
            scriptBuilder.Append("EXEC('CREATE SCHEMA ");
            scriptBuilder.Append(tempSchemaName);
            scriptBuilder.Append("')");
            scriptBuilder.Append(Environment.NewLine);
            scriptBuilder.Append("END");
            scriptBuilder.Append(Environment.NewLine);
            scriptBuilder.Append(Environment.NewLine);
            scriptBuilder.Append("IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'");
            scriptBuilder.Append(tempSchemaName);
            scriptBuilder.Append(".tmp_");
            scriptBuilder.Append(tableType);
            scriptBuilder.Append("_");
            scriptBuilder.Append(record.Source.TableName);
            scriptBuilder.Append("') AND type in (N'U'))");
            scriptBuilder.Append(Environment.NewLine);
            scriptBuilder.Append("BEGIN");
            scriptBuilder.Append(Environment.NewLine);
            scriptBuilder.Append("\tDROP TABLE [");
            scriptBuilder.Append(tempSchemaName);
            scriptBuilder.Append("].[tmp_");
            scriptBuilder.Append(tableType);
            scriptBuilder.Append("_");
            scriptBuilder.Append(record.Source.TableName);
            scriptBuilder.Append("]");
            scriptBuilder.Append(Environment.NewLine);
            scriptBuilder.Append("END");
            scriptBuilder.Append(Environment.NewLine);
            scriptBuilder.Append(Environment.NewLine);

            scriptBuilder.Append("CREATE TABLE [");
            scriptBuilder.Append(tempSchemaName);
            scriptBuilder.Append("].[tmp_");
            scriptBuilder.Append(tableType);
            scriptBuilder.Append("_");
            scriptBuilder.Append(record.Source.TableName);
            scriptBuilder.Append("]");
            scriptBuilder.Append(Environment.NewLine);
            scriptBuilder.Append("(");

            bool first = true;
            foreach (var property in record.Properties.Values)
            {
                switch (property.PropertyRole)
                {
                    case RecordPropertyRole.ForeignKey:
                        if (tableType == TempTableType.Insert)
                        {
                            if (!first)
                            {
                                scriptBuilder.Append(",");
                            }
                            scriptBuilder.Append(Environment.NewLine);
                            scriptBuilder.Append("\t");
                            scriptBuilder.Append(property.Name);
                            scriptBuilder.Append(" ");
                            scriptBuilder.Append(GetSQLTypeString(property.PropertyType));
                            first = false;
                        }
                        break;
                    case RecordPropertyRole.PrimaryKey:
                        if (tableType == TempTableType.Update)
                        {
                            if (!first)
                            {
                                scriptBuilder.Append(",");
                            }
                            scriptBuilder.Append(Environment.NewLine);
                            scriptBuilder.Append("\t");
                            scriptBuilder.Append(property.Name);
                            scriptBuilder.Append(" ");
                            scriptBuilder.Append(GetSQLTypeString(property.PropertyType));
                            first = false;
                        }
                        break;
                    case RecordPropertyRole.ValidatedField:
                        if (!first)
                        {
                            scriptBuilder.Append(",");
                        }

                        scriptBuilder.Append(Environment.NewLine);
                        scriptBuilder.Append("\t");
                        scriptBuilder.Append(property.Name);
                        scriptBuilder.Append("_original");
                        scriptBuilder.Append(" ");
                        scriptBuilder.Append(GetSQLTypeString(property.PropertyType));
                        scriptBuilder.Append(",");

                        scriptBuilder.Append(Environment.NewLine);
                        scriptBuilder.Append("\t");
                        scriptBuilder.Append(property.Name);
                        scriptBuilder.Append("_expected");
                        scriptBuilder.Append(" ");
                        scriptBuilder.Append(GetSQLTypeString(property.PropertyType));
                        first = false;
                        break;
                    case RecordPropertyRole.Fact:
                        if (tableType == TempTableType.Insert)
                        {
                            if (!first)
                            {
                                scriptBuilder.Append(",");
                            }
                            scriptBuilder.Append(Environment.NewLine);
                            scriptBuilder.Append("\t");
                            scriptBuilder.Append(property.Name);
                            scriptBuilder.Append(" ");
                            scriptBuilder.Append(GetSQLTypeString(property.PropertyType));
                            first = false;
                        }
                        break;
                    default:
                        break;
                }
            }
            scriptBuilder.Append(Environment.NewLine);
            scriptBuilder.Append(")");

            return scriptBuilder.ToString();
        }

        private static string CreateInsertScript(IRecord record, string TempSchemaName, TempTableType tableType)
        {
            var builder = new StringBuilder();
            builder.Append("-- *********************** NEW ");
            builder.Append(record.Source.TableName);
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append("INSERT INTO");
            builder.Append(Environment.NewLine);
            builder.Append("\t[");
            builder.Append(record.Source.SchemaName);
            builder.Append("].[");
            builder.Append(record.Source.TableName);
            builder.Append("]");
            builder.Append(Environment.NewLine);
            builder.Append("\t\t(");

            var props = record.Properties.Values.Where(x => x.PropertyRole == RecordPropertyRole.ForeignKey || x.PropertyRole == RecordPropertyRole.ValidatedField || x.PropertyRole == RecordPropertyRole.Fact);
            bool first = true;
            foreach (var property in props)
            {
                if (!first)
                {
                    builder.Append(",");
                }
                builder.Append(Environment.NewLine);
                builder.Append("\t\t");
                builder.Append(property.Name);
                first = false;
            }
            builder.Append(Environment.NewLine);
            builder.Append("\t\t)");
            builder.Append(Environment.NewLine);
            builder.Append("SELECT");
            first = true;
            foreach (var property in props)
            {
                if (!first)
                {
                    builder.Append(",");
                }
                builder.Append(Environment.NewLine);
                builder.Append("\t");
                builder.Append(property.Name);
                if (property.PropertyRole == RecordPropertyRole.ValidatedField)
                {
                    builder.Append("_expected");
                }
                first = false;
            }
            builder.Append(Environment.NewLine);
            builder.Append("FROM");
            builder.Append(Environment.NewLine);
            builder.Append("\t[");
            builder.Append(TempSchemaName);
            builder.Append("].[tmp_");
            builder.Append(tableType);
            builder.Append("_");
            builder.Append(record.Source.TableName);
            builder.Append("]");

            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append("GO");
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);

            return builder.ToString();

        }

        private static string CreateUpdateScript(IRecord record, string TempSchemaName, TempTableType tableType)
        {
            var builder = new StringBuilder();
            builder.Append("-- *********************** UPDATE ");
            builder.Append(record.Source.TableName);
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append("UPDATE");
            builder.Append(Environment.NewLine);
            builder.Append("\tmain");
            builder.Append(Environment.NewLine);
            builder.Append("SET");

            bool first = true;
            foreach (var property in record.Properties.Values.Where(x => x.PropertyRole == RecordPropertyRole.ValidatedField || (tableType == TempTableType.Insert && x.PropertyRole == RecordPropertyRole.Fact)))
            {
                if (!first)
                {
                    builder.Append(",");
                }
                builder.Append(Environment.NewLine);
                builder.Append("\tmain.");
                builder.Append(property.Name);
                builder.Append("=");
                builder.Append("tmp.");
                builder.Append(property.Name);
                builder.Append("_expected");
                first = false;
            }
            builder.Append(Environment.NewLine);
            builder.Append("FROM");
            builder.Append(Environment.NewLine);
            builder.Append("\t[");
            builder.Append(record.Source.SchemaName);
            builder.Append("].[");
            builder.Append(record.Source.TableName);
            builder.Append("] main JOIN");
            builder.Append(Environment.NewLine);
            builder.Append("\t[");
            builder.Append(TempSchemaName);
            builder.Append("].[tmp_");
            builder.Append(tableType);
            builder.Append("_");
            builder.Append(record.Source.TableName);
            builder.Append("] tmp ON ");

            first = true;
            foreach (var property in record.Properties.Values.Where(x => x.PropertyRole == ((tableType == TempTableType.Update) ? RecordPropertyRole.PrimaryKey : RecordPropertyRole.ForeignKey)))
            {
                builder.Append("main.[");
                builder.Append(property.Name);
                builder.Append("] = tmp.[");
                builder.Append(property.Name);
                builder.Append("]");
            }

            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append("GO");
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);

            return builder.ToString();
        }

        private static string GetSQLTypeString(Type type)
        {
            string typeString;
            string nullString;
            Type effectiveType;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                effectiveType = type.GetGenericArguments()[0];
                typeString = effectiveType.Name;
                nullString = " NULL";
            }
            else
            {
                effectiveType = type;
                typeString = type.Name;
                nullString = String.Empty;
            }

            // I'm setting some defaults here.... it seems rather silly to have calculated bytes in here anyway.
            var sqlTypeString = new TypeSwitch(effectiveType)
                                        .Case<SqlInt64, Int64>("bigint")
                                        .Case<SqlInt32, Int32>("int")
                                        .Case<SqlBytes, SqlBinary, Byte[]>("varbinary(500)")
                                        .Case<SqlBoolean, Boolean>("bit")
                                        .Case<SqlDateTime>("datetime")
                                        .Case<DateTime>("datetime2")
                                        .Case<SqlDecimal>("decimal")
                                        .Case<SqlDouble, Double>("float")
                                        .Case<SqlMoney, Decimal>("money")
                                        .Case<SqlChars, SqlString, String, Char[]>("nvarchar(255)")
                                        .Case<SqlSingle, Single>("real")
                                        .Case<SqlInt16, Int16>("smallint")
                                        .Case<SqlByte, Byte>("tinyint")
                                        .Case<SqlGuid, Guid>("uniqueidentifier")
                                        .Default("UNHANDLED");
            return sqlTypeString + nullString;
        }



    
    }
}
