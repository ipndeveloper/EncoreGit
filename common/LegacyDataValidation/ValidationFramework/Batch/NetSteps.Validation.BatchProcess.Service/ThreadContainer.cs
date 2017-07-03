using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetSteps.Validation.BatchProcess.Common;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;

namespace NetSteps.Validation.BatchProcess.Service
{
    public class ThreadContainer
    {
        private RecordCache _cache;
        private IRecordValidationService _service;
        private readonly ICollection<IRecordValidationResultManager> _logWriters;

        public ThreadContainer(ICollection<IRecordValidationResultManager> logWriters, RecordCache cache, IRecordValidationService service)
        {
            _logWriters = logWriters;
            _cache = cache;
            _service = service;
        }

        public void BeginProcessing()
        {
            IRecord record = null;
            while (_cache.GetNextRecord(out record) || (!_cache.CacheCompleted && !_cache.CacheEmpty))
            {
                try
                {
                    if (record != null)
                    {
                        var validation = _service.Validate(record);

                        foreach (var writer in _logWriters)
                        {
                            writer.AddValidation(validation);
                        }
                    }
                    else if (!_cache.CacheCompleted)
                    {
                        Thread.Sleep(100);
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    if (record != null)
                    {
                        Console.WriteLine(String.Format("Thread exited with error: '{0}' while processing record {1}:{2}; Stack Trace: {3}", ex.Message, record.RecordKind, record.RecordIdentity, ex.StackTrace));
                    }
                    else
                    {
                        Console.WriteLine(String.Format("Thread exited with error: '{0}' while processing null record.", ex.Message));
                    }
                }
            }
            Console.WriteLine("Validator thread exited normally.");
        }
    }
}
