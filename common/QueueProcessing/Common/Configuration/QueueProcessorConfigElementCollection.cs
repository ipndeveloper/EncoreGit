using NetSteps.Encore.Core.Configuration;

namespace NetSteps.QueueProcessing.Common.Configuration
{
	public class QueueProcessorConfigElementCollection : AbstractConfigurationElementCollection<QueueProcessorConfigElement, string>
	{
		public QueueProcessorConfigElementCollection()
		{
		}

		protected override string PerformGetElementKey(QueueProcessorConfigElement element)
		{
			return element.Name;
		}
	}
}
