using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Periods;

namespace NetSteps.Commissions.Service.Test.Periods
{
	[TestClass]
	public class PeriodServiceTest
	{
		[TestMethod]
		public void PeriodService_should_return_a_period()
		{
			var periodService = new PeriodService(new PeriodProvider(new PeriodRepository()));
			var period = periodService.GetCurrentPeriod(DisbursementFrequencyKind.Monthly);
			Assert.IsNotNull(period);
			Assert.AreNotEqual(0, period.PeriodId);
		}

		[TestMethod]
		public void PeriodService_should_return_a_period_for_Account()
		{
			var periodService = new PeriodService(new PeriodProvider(new PeriodRepository()));
			var period = periodService.GetPeriodsForAccount(3031);
			Assert.IsNotNull(period);
			Assert.AreNotEqual(0, period.First().PeriodId);
		}
	}
}