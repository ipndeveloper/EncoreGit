using System.Configuration;

namespace NetSteps.QueueProcessing.Common.Configuration
{
	public class QueueProcessorConfigElement : ConfigurationElement
	{
		private const string PropertyName_name = "name";

		private const string PropertyName_workerThreads = "workerThreads";

		private const string PropertyName_pollingIntervalMs = "pollingIntervalMs";

		private const string PropertyName_maxNumberToPoll = "maxNumberToPoll";

		[ConfigurationProperty("maxNumberToPoll", IsKey = true, IsRequired = true)]
		public int MaxNumberToPoll
		{
			get
			{
				return (int)base["maxNumberToPoll"];
			}
			set
			{
				base["maxNumberToPoll"] = value;
			}
		}

		[ConfigurationProperty("name", IsKey = true, IsRequired = true)]
		public string Name
		{
			get
			{
				return (string)base["name"];
			}
			set
			{
				base["name"] = value;
			}
		}

		[ConfigurationProperty("pollingIntervalMs", IsKey = true, IsRequired = true)]
		public int PollingIntervalMs
		{
			get
			{
				return (int)base["pollingIntervalMs"];
			}
			set
			{
				base["pollingIntervalMs"] = value;
			}
		}

		[ConfigurationProperty("workerThreads", IsKey = true, IsRequired = true)]
		public int WorkerThreads
		{
			get
			{
				return (int)base["workerThreads"];
			}
			set
			{
				base["workerThreads"] = value;
			}
		}

		public QueueProcessorConfigElement()
		{
		}
	}
}
