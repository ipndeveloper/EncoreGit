using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using NetSteps.Validation.BatchProcess.Common;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;
using System.Collections.Concurrent;

namespace NetSteps.Validation.BatchProcess.Service
{
    public class RecordCache
    {
        private ConcurrentStack<IRecord> _recordStack;
        private IRecordRepository _repository;
        private IEnumerable<object> _keys;
        private IRecordQuery _query;

        public RecordCache(IRecordRepository repository, IRecordQuery query)
        {
            _recordStack = new ConcurrentStack<IRecord>();
            _repository = repository;
            _query = query;
            _keys = new object[0];
        }

        public IEnumerable<object> KeyCollection { get; private set; }
        public bool HasRecordsToProcess
        {
            get { return _recordStack.Count > 0; }
        }

        public int TotalRecords { get; private set; }

        public bool CacheCompleted
        {
            get
            {
                return !_keys.Any() && 
                    !LoadingInProcess;
            }
        }

        public bool CacheEmpty
        {
            get
            {
                if (!CacheCompleted)
                {
                    return false;
                }
                return !_recordStack.Any();
            }
        }

        public bool GetNextRecord(out IRecord record)
        {
            return _recordStack.TryPop(out record);
        }

        public bool LoadingInProcess { get; private set; }

        internal void BeginLoading()
        {
            LoadingInProcess = true;
            _keys = _repository.RetrieveRecordKeys(_query);
            KeyCollection = _keys.ToList();
            TotalRecords = _keys.Count();
            Console.WriteLine(String.Format("Total records: {0}", _keys.Count()));
        }

        internal void RunCacheLoaderThread()
        {
            while (_keys.Any())
            {
                LoadBuffer();
            }
            LoadingInProcess = false;
        }

        internal void InitialLoad()
        {
            if (_keys.Any())
            {
                LoadBuffer();
            }
        }

        private void LoadBuffer()
        {
            try
            {
                
                var timer = new Stopwatch();
                timer.Start();

                LoadingInProcess = true;
                IEnumerable<IRecord> records;
                if (_keys.Count() < _query.MaximumBufferRecords)
                {
                    records = _repository.RetrieveRecords(_keys);
                    _keys = new object[0];
                }
                else
                {
                    var bufferSet = _keys.Take(_query.MaximumBufferRecords);
                    _keys = _keys.Skip(_query.MaximumBufferRecords);
                    records = _repository.RetrieveRecords(bufferSet);
                }
                foreach (var record in records)
                {
                    _recordStack.Push(record);
                }
                LoadingInProcess = false;
                timer.Stop();
                Console.WriteLine(String.Format("Buffer Load Elapsed Time: {0} seconds. Remaining records: {1}", timer.Elapsed.Seconds, _keys.Count()));
            }
            catch (Exception e)
            {
                Exception effectiveException;
                if (e.InnerException != null)
                {
                    effectiveException = e.InnerException;
                }
                else
                {
                    effectiveException = e;
                }
                Console.WriteLine(effectiveException.Message);
            }
        }
    }
}
