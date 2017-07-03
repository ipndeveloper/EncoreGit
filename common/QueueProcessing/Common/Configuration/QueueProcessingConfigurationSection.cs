using System.Configuration;

namespace NetSteps.QueueProcessing.Common.Configuration
{
	public class QueueProcessingConfigurationSection : ConfigurationSection
	{
		private const string PropertyName_processors = "processors";

		[ConfigurationProperty("processors", IsDefaultCollection = true)]
		public QueueProcessorConfigElementCollection Processors
		{
			get
			{
				return (QueueProcessorConfigElementCollection)base["processors"];
			}
			set
			{
				base["processors"] = value;
			}
		}

		public QueueProcessingConfigurationSection()
		{
		}
	}
}
