using NetSteps.Encore.Core.IoC;
using NetSteps.QueueProcessing.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace NetSteps.QueueProcessing.Service
{
	[ContainerRegister(typeof(IQueueProcessingService), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class QueueProcessingService : IQueueProcessingService
	{
		private readonly IQueueProcessorLogger _logger;
		private IList<IQueueProcessor> _processors;

		public QueueProcessingService()
			: base()
		{
			var logger = Create.New<IQueueProcessorLogger>();
			if(logger == null)
			{
				throw new Exception("There is no registered implementation for IQueueProcessorLogger");
			}
			
			_processors = new List<IQueueProcessor>();
			_logger = logger;
		}

		public void StartQueues()
		{
			try
			{
				_logger.Info("QueueProcessingService Starting Processors");
				var queueProcessingConfiguration = Create.New<IQueueProcessingConfiguration>();
				foreach (var pollingIntervalMs in queueProcessingConfiguration.ProcessorList())
				{
					_logger.Info(string.Format("Starting {0}", pollingIntervalMs.Name));
					IQueueProcessorConfiguration queueProcessorConfiguration = queueProcessingConfiguration.ProcessorConfiguration(pollingIntervalMs.Name);
					if (queueProcessorConfiguration != null)
					{
						pollingIntervalMs.PollingIntervalMs = queueProcessorConfiguration.PollingIntervalMs;
						pollingIntervalMs.WorkerThreads = queueProcessorConfiguration.WorkerThreads;
						pollingIntervalMs.MaxNumberToPoll = queueProcessorConfiguration.MaxNumberToPoll;
					}
					pollingIntervalMs.Start();
					_processors.Add(pollingIntervalMs);
					_logger.Info(string.Format("Started {0}", pollingIntervalMs.Name));
				}
				_logger.Info("QueueProcessingService Finished Starting Processors");
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				_logger.Info(string.Concat(exception.Message, " ", exception.StackTrace));
				throw exception;
			}
		}

		public void StopQueues()
		{
			_logger.Info("QueueProcessingService Stopping Processors");
			foreach (IQueueProcessor _processor in this._processors)
			{
				_logger.Info(string.Format("Stopping {0}", _processor.Name));
				_processor.Stop();
				_logger.Info(string.Format("Stopped {0}", _processor.Name));
			}
			_processors.Clear();
			_logger.Info("QueueProcessingService Finished Stopping Processors");
		}
	}
}
