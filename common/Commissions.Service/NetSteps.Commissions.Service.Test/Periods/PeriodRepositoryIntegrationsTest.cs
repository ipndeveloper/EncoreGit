using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Commissions.Service.Periods;

namespace NetSteps.Commissions.Service.Test.Periods
{
	[TestClass]
	public class PeriodRepositoryIntegrationsTest
	{
		[TestMethod]
		public void PeriodRepository_should_retrieve_periods()
		{
			var repo = new PeriodRepository();
			var found = repo.FetchAll();
			Assert.IsNotNull(found);
			Assert.AreEqual(7, found.Count);
		}
	}
}
