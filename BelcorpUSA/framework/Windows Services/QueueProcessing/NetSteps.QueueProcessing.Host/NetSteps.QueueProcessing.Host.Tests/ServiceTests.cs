using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueueProcessingService = NetSteps.QueueProcessing.Host.QueueProcessingHost;

namespace NetSteps.QueueProcessing.Host.Tests
{
	[TestClass]
	public class ServiceTests
	{
		[TestMethod]
		public void Main_ShouldGetThroughWireups()
		{
			string[] args = new string[1] { "/unitTest" };
			new QueueProcessingService().OnStart(args);
		}
	}
}
