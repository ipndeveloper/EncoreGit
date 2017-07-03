namespace NetSteps.QueueProcessing.Common
{
	public interface IQueueProcessor
	{
		string Name { get; set; }
		bool IsRunning { get; set; }
		int WorkerThreads { get; set; }
		int PollingIntervalMs { get; set; }
		int MaxNumberToPoll { get; set; }
		void Start();
		void Stop();
	}
}
