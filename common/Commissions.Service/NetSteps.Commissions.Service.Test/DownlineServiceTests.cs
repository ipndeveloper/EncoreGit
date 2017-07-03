using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetSteps.Commissions.Service.Test
{
	[TestClass]
	public class DownlineServiceTests
	{
		[TestMethod]
		public void DownlineService_should_get_downline()
		{
			var service = new DownlineService();

			var downline = service.GetDownline(201406);
			Assert.IsNotNull(downline);
		}
	}
}
