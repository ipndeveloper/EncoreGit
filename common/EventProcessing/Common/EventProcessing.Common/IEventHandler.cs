namespace NetSteps.EventProcessing.Common
{
	public interface IEventHandler
	{
		bool Execute(int eventID);
	}
}