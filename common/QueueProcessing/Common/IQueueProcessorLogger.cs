namespace NetSteps.QueueProcessing.Common
{
	public interface IQueueProcessorLogger
	{
		void Debug(string msg);
		void Debug(string format, params object[] args);
		void Info(string msg);
		void Info(string format, params object[] args);
		void Error(string msg);
		void Error(string format, params object[] args);
	}
}
