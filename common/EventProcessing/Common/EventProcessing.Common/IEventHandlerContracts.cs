namespace NetSteps.EventProcessing.Common
{
	using System.Diagnostics.Contracts;

	internal abstract class IEventHandlerContracts : IEventHandler
	{
		public bool Execute(int eventID)
		{
			Contract.Requires(eventID > 0);
			return default(bool);
		}
	}
}
