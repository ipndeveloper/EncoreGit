using NetSteps.Encore.Core;
using NetSteps.Encore.Core.Configuration;
using NetSteps.Encore.Core.IoC;
using NetSteps.QueueProcessing.Common;
using NetSteps.QueueProcessing.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace NetSteps.QueueProcessing.Service
{
	[ContainerRegister(typeof(IQueueProcessingConfiguration), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class IoCQueueProcessingConfiguration : IQueueProcessingConfiguration
	{
		private readonly IQueueProcessorRegistry _registry;
		private IList<IQueueProcessor> _processors;
		private readonly static string _sectionName;

		private IList<IQueueProcessor> Processors
		{
			get
			{
				return Util.NonBlockingLazyInitializeVolatile<IList<IQueueProcessor>>(ref this._processors, new Func<IList<IQueueProcessor>>(this.MakeProcessorList));
			}
		}

		static IoCQueueProcessingConfiguration()
		{
			IoCQueueProcessingConfiguration._sectionName = "netsteps.queueprocessor";
		}

		public IoCQueueProcessingConfiguration()
			: base()
		{
			var registry = Create.New<IQueueProcessorRegistry>();
			if (registry == null)
			{
				throw new Exception("There is no registered implementation for IQueueProcessingConfiguration");
			}
			this._registry = registry;
		}

		private IList<IQueueProcessor> MakeProcessorList()
		{
			List<IQueueProcessor> queueProcessors = new List<IQueueProcessor>();
			using (IContainer container = Create.SharedOrNewContainer())
			{
				foreach (Type allProcessorsType in this._registry.AllProcessorsTypes)
				{
					queueProcessors.Add(IContainerExtensions.NewImplementationOf<IQueueProcessor>(container, allProcessorsType));
				}
			}
			return Extensions.ToReadOnly<IQueueProcessor>(queueProcessors);
		}

		public IQueueProcessorConfiguration ProcessorConfiguration(string processorName)
		{
			var section = ConfigurationManager.GetSection(IoCQueueProcessingConfiguration._sectionName) as QueueProcessingConfigurationSection;
			var item = section.Processors[processorName];
			if (item == null)
			{
				return null;
			}
			return new IoCQueueProcessingConfiguration.QueueProcessorConfiguration(item.WorkerThreads, item.PollingIntervalMs, item.MaxNumberToPoll);
		}

		public IEnumerable<IQueueProcessor> ProcessorList()
		{
			return this.MakeProcessorList();
		}

		private class QueueProcessorConfiguration : IQueueProcessorConfiguration
		{
			public int MaxNumberToPoll { get; set; }
			public int PollingIntervalMs { get; set; }
			public int WorkerThreads { get; set; }

			public QueueProcessorConfiguration(int workerThreads, int pollingIntervalMs, int maxNumberToPoll)
			{
				this.WorkerThreads = workerThreads;
				this.PollingIntervalMs = pollingIntervalMs;
				this.MaxNumberToPoll = maxNumberToPoll;
			}
		}
	}
}
