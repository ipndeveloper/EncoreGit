using NetSteps.Validation.BatchProcess.TempTableWriters.Common;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetSteps.Validation.BatchProcess.TempTableWriters.Implementation
{
    public class TempTableOutputWriter : ITempTableOutputWriter
    {
        public TempTableOutputWriter(string connectionString, int maxRowsBeforeCommit, string validationSchemaName, string filePath)
        {
            _tableWriters = new Dictionary<string, TableWriter>();
            _connectionString = connectionString;
            _maxRowsBeforeCommit = maxRowsBeforeCommit;
            _validationSchemaName = validationSchemaName;
            _filePath = filePath;
        }

        private readonly string _connectionString;
        private readonly int _maxRowsBeforeCommit;
        private readonly Dictionary<string, TableWriter> _tableWriters;
        private readonly string _validationSchemaName;
        private readonly string _filePath;

        public void Handle(IRecord record)
        {
            RecursiveWriteToTables(record);
        }

        private void RecursiveWriteToTables(IRecord record)
        {
            if (!record.Result.IsIn(ValidationResultKind.IsIncorrect | ValidationResultKind.IsNew | ValidationResultKind.IsWithinMarginOfError))
            {
                return;
            }
            var tableHandler = GetTableHandler(record.RecordKind, record.Result);
            tableHandler.Write(record);

            if (record.ChildRecords != null)
            {
                foreach (var child in record.ChildRecords)
                {
                    RecursiveWriteToTables(child);
                }
            }
        }

        private TableWriter GetTableHandler(string recordKind, ValidationResultKind resultType)
        {
            // determine temp table type
            TempTableType tempTableType;
            if (resultType.IsIn(ValidationResultKind.IsWithinMarginOfError | ValidationResultKind.IsIncorrect))
            {
                tempTableType = TempTableType.Update;
            }
            else if (resultType == ValidationResultKind.IsNew)
            {
                tempTableType = TempTableType.Insert;
            }
            else
            {
                throw new Exception(String.Format("Unable to produce handler for record validation result {0}", resultType));
            }

            string key = recordKind + tempTableType;
            if (_tableWriters.ContainsKey(key))
            {
                return _tableWriters[key];
            }
            else
            {
                var newWriter = new TableWriter(_connectionString, _maxRowsBeforeCommit, tempTableType, _validationSchemaName);
                _tableWriters.Add(key, newWriter);
                return newWriter;
            }
        }

        public void Close()
        {
            List<Thread> _tableWriterThreads = new List<Thread>();
            foreach (var handler in _tableWriters.Values)
            {
                Thread thread = new Thread(new ThreadStart(handler.Close));
                _tableWriterThreads.Add(thread);
                thread.Start();
            }
            foreach (var thread in _tableWriterThreads)
            {
                thread.Join();
            }
            Console.WriteLine("Completed writing temp records to database.");

            Console.WriteLine("Writing update script.");
            WriteCommitScript();

        }

        private void WriteCommitScript()
        {
            var fullFilePath = string.Format("{0}\\CommitFromTempTables.sql", _filePath);
            using (var stream = new FileStream(fullFilePath, FileMode.OpenOrCreate))
            {
                var writer = new StreamWriter(stream);

                foreach (var handler in _tableWriters.Values)
                {
                    writer.Write(handler.GetCommitScript());
                }

                writer.Flush();
                writer.Close();
            }
        }
    }
}
