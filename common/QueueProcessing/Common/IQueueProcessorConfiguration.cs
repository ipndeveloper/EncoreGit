namespace NetSteps.QueueProcessing.Common
{
	public interface IQueueProcessorConfiguration
	{
		int WorkerThreads { get; }
		int PollingIntervalMs { get; }
		int MaxNumberToPoll { get; }
	}
}
