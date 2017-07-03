using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using NetSteps.Encore.Core.IoC;
using NetSteps.Validation.BatchProcess.Common;
using NetSteps.Validation.Common;

namespace NetSteps.Validation.BatchProcess.Service
{
    public class BatchValidationService : IBatchValidationService
    {
        private const int _maxThreads = 5;
        
        private readonly IRecordValidationService _validationService;
        
        /// <summary>
        /// Initializes the <see cref="BatchValidationService" /> class.
        /// </summary>
        /// <param name="validationService">The validation service.</param>
        public BatchValidationService(IRecordValidationService validationService)
        {
            _validationService = validationService;
        }

        public void ProcessBatch(IRecordRepository repository, IRecordQuery query, ICollection<IRecordValidationResultManager> logWriters, Func<IEnumerable<IDependentDataService>> dependentServiceCollector)
        {
            var cache = new RecordCache(repository, query);
            cache.BeginLoading();
            Console.WriteLine("Starting initial buffer load.");
            var initialCacheLoadThread = new Thread(new ThreadStart(cache.InitialLoad));
            initialCacheLoadThread.Start();

            // initialize dependent services
            var serviceInitThreads = new List<Thread>();
            {
                var dependentServices = dependentServiceCollector();
                foreach (var service in dependentServices)
                {
                    Console.WriteLine(String.Format("Starting dependent service {0}.", service.Name));
                    service.QueryBase = query;
                    var serviceInitializingThread = new Thread(new ThreadStart(service.Initialize));
                    serviceInitThreads.Add(serviceInitializingThread);
                    serviceInitializingThread.Start();
                }
            }
            foreach (var serviceThread in serviceInitThreads)
            {
                serviceThread.Join();
            }
            initialCacheLoadThread.Join();
            Console.WriteLine("Initial buffer load complete.");

            var cacheThread = new Thread(new ThreadStart(cache.RunCacheLoaderThread));
            cacheThread.Start();

            var recordCount = cache.TotalRecords;
            var maxThreads = recordCount > _maxThreads ? _maxThreads : recordCount;
            var threads = new List<Thread>();
            
            if (cache.HasRecordsToProcess)
            {
                for (int count = 0; count < maxThreads; count++)
                {
                    var threadContainer = new ThreadContainer(logWriters, cache, _validationService);
                    var newThread = new Thread(new ThreadStart(threadContainer.BeginProcessing));
                    threads.Add(newThread);
                    newThread.Start();
                }
            }

            cacheThread.Join();
            foreach (var thread in threads)
            {
                thread.Join();
            }
            
            foreach (var logger in logWriters)
            {
                logger.NotifyValidationComplete();
                logger.OnFinished();
            }

        }
    }
}