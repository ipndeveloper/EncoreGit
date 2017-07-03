using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using NetSteps.Validation.Common.Model;
using System.Data;
using System.Data.SqlTypes;
using NetSteps.Validation.BatchProcess.TempTableWriters.Implementation.Script;
using NetSteps.Validation.Common;
using System.Collections.Concurrent;
using System.Threading;
using NetSteps.Validation.BatchProcess.TempTableWriters.Implementation.Model;
using System.Diagnostics;

namespace NetSteps.Validation.BatchProcess.TempTableWriters.Implementation
{
    public class TableWriter
    {
        private readonly string _databaseConnectionString;
        private readonly int _maxRowsBeforeCommit;
        private readonly TempTableType _tableType;
        private readonly string _validationSchemaName;
        private readonly ConcurrentStack<IEnumerable<RecordProperty>> _records;

        private bool _hasData;
        private IRecord _definitionRecord;
        private object _definitionRecordLock = new object();

        private Thread _commitThread;
        private bool _closing;
        private EventWaitHandle _resetEvent = new EventWaitHandle(false, EventResetMode.ManualReset);

        public TableWriter(string databaseConnectionString, int maxRowsBeforeCommit, TempTableType tableType, string validationSchemaName)
        {
            _databaseConnectionString = databaseConnectionString;
            _maxRowsBeforeCommit = maxRowsBeforeCommit;
            _tableType = tableType;
            _validationSchemaName = validationSchemaName;
            _hasData = false;
            _records = new ConcurrentStack<IEnumerable<RecordProperty>>();
            _commitThread = new Thread(StartCommit);
            _commitThread.Priority = ThreadPriority.Highest;
            _closing = false;
        }

        public void Write(IRecord record)
        {
            if (!record.Properties.Any(x => x.Value.ResultKind == ValidationResultKind.IsIncorrect || x.Value.ResultKind == ValidationResultKind.IsNew))
            {
                return;
            }
            _records.Push(Convert(record));
            if (_definitionRecord == null)
            {
                lock (_definitionRecordLock)
                {
                    if (_definitionRecord == null)
                    {
                        _definitionRecord = record;
                        _commitThread.Start();
                    }
                }
            }
            _resetEvent.Set();
        }

        private IEnumerable<RecordProperty> Convert(IRecord record)
        {
            var condensedProperties = new List<RecordProperty>();
            foreach (var property in record.Properties.Values)
            {
                switch (property.PropertyRole)
                {
                    case RecordPropertyRole.ForeignKey:
                        if (_tableType == TempTableType.Insert)
                        {
                            condensedProperties.Add(new RecordProperty(true, property.Name, property.OriginalValue, property.PropertyType));
                        }
                        break;
                    case RecordPropertyRole.PrimaryKey:
                        if (_tableType == TempTableType.Update)
                        {
                            condensedProperties.Add(new RecordProperty(true, property.Name, property.OriginalValue, property.PropertyType));
                        }
                        break;
                    case RecordPropertyRole.ValidatedField:
                        condensedProperties.Add(new RecordProperty(true, String.Format("{0}_expected", property.Name), property.ExpectedValue, property.PropertyType));
                        condensedProperties.Add(new RecordProperty(true, String.Format("{0}_original", property.Name), property.OriginalValue, property.PropertyType));
                        break;
                    case RecordPropertyRole.Fact:
                        if (_tableType == TempTableType.Insert)
                        {
                            condensedProperties.Add(new RecordProperty(false, property.Name, property.OriginalValue, property.PropertyType));
                        }
                        break;
                    default:
                        throw new Exception(String.Format("Unhandled property role {0}.", property.PropertyRole));
                }
            }
            return condensedProperties;
        }

        public DataTable DefineTable(IRecord record)
        {
            var data = new DataTable(String.Format("tmp_{0}_{1}", _tableType, record.Source.TableName));
            foreach (var property in record.Properties.Values)
            {
                Type effectiveType;
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    effectiveType = property.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    effectiveType = property.PropertyType;
                }
                switch (property.PropertyRole)
                {
                    case RecordPropertyRole.ForeignKey:
                        if (_tableType == TempTableType.Insert)
                        {
                            data.Columns.Add(String.Format("{0}", property.Name), effectiveType);
                        }
                        break;
                    case RecordPropertyRole.PrimaryKey:
                        if (_tableType == TempTableType.Update)
                        {
                            data.Columns.Add(String.Format("{0}", property.Name), effectiveType);
                        }
                        break;
                    case RecordPropertyRole.ValidatedField:
                        data.Columns.Add(String.Format("{0}_expected", property.Name), effectiveType);
                        data.Columns.Add(String.Format("{0}_original", property.Name), effectiveType);
                        break;
                    case RecordPropertyRole.Fact:
                        if (_tableType == TempTableType.Insert)
                        {
                            data.Columns.Add(String.Format("{0}", property.Name), effectiveType);
                        }
                        break;
                    default:
                        throw new Exception(String.Format("Unhandled property role {0}.", property.PropertyRole));
                }
            }
            return data;
        }

        public DataRow Convert(IEnumerable<RecordProperty> record, DataRow newRow)
        {
            foreach (var property in record)
            {
                newRow.SetField(String.Format("{0}", property.Name), property.Value);
            }
            return newRow;
        }

        internal void Close()
        {
            if (_hasData)
            {
                _closing = true;
                _commitThread.Join();
            }
        }

        internal string GetCommitScript()
        {
            if (_hasData)
            {
                var scriptContainer = TableScriptWriter.GetScriptContainer(_tableType, _definitionRecord, _validationSchemaName);
                return scriptContainer.CommitScript;
            }
            else
            {
                return string.Empty;
            }
        }

        private DataTable DefineStructure(IRecord record)
        {
            // generate data structure
            var data = DefineTable(record);
            if (data.Columns.Count > 0)
            {
                _hasData = true;
                var scriptContainer = TableScriptWriter.GetScriptContainer(_tableType, record, _validationSchemaName);

                // generate temp table
                using (var connection = new SqlConnection(_databaseConnectionString))
                {
                    SqlCommand command = new SqlCommand(scriptContainer.CreateScript, connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            return data;
        }

        private void StartCommit()
        {
            IEnumerable<RecordProperty> record;
            var data = DefineStructure(_definitionRecord);
            if (data.Columns.Count == 0)
            {
                return;
            }
            while (!_closing)
            {
                if (_records.TryPop(out record))
                {
                    data.Rows.Add(Convert(record, data.NewRow()));
                }
                else
                {
                    _resetEvent.WaitOne();
                }
                if (data.Rows.Count == _maxRowsBeforeCommit)
                {
                    CommitUpdates(data);
                    data.Dispose();
                    data = DefineStructure(_definitionRecord);
                }
            }
            while (_records.TryPop(out record))
            {
                data.Rows.Add(Convert(record, data.NewRow()));
            }
            if (data.Rows.Count > 0)
            {
                CommitUpdates(data);
            }
        }

        private void CommitUpdates(DataTable data)
        {
            using (var connection = new SqlConnection(_databaseConnectionString))
            {
                try
                {
                    connection.Open();
                    var bulkCopy = new SqlBulkCopy(connection);
                    bulkCopy.DestinationTableName = String.Format("[{0}].[{1}]", _validationSchemaName, data.TableName);
                    bulkCopy.BatchSize = data.Rows.Count;
                    bulkCopy.WriteToServer(data);

                    connection.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
    }
}
