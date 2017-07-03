namespace NetSteps.QueueProcessing.Common
{
	public interface IServiceHost
	{
		void OnStart(string[] args);
		void OnStop();
	}
}
