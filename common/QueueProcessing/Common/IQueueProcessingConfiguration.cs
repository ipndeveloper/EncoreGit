using System.Collections.Generic;

namespace NetSteps.QueueProcessing.Common
{
	public interface IQueueProcessingConfiguration
	{
		IEnumerable<IQueueProcessor> ProcessorList();
		IQueueProcessorConfiguration ProcessorConfiguration(string processorName);
	}
}
