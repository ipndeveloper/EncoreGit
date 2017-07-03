using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Common.Extensions;

namespace NetSteps.Common.Tests.Extensions
{
	[TestClass]
	public class DateTimeExtensionsTests
	{
		[TestMethod()]
		public void TotalMonthsTest1()
		{
			DateTime date = new DateTime(2000, 1, 1);
			int expected = 24001;
			int actual;
			actual = DateTimeExtensions.TotalMonths(date);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void TotalMonthsTest2()
		{
			DateTime date = new DateTime(1999, 12, 31);
			int expected = 24000;
			int actual;
			actual = DateTimeExtensions.TotalMonths(date);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void TotalMonthsTest3()
		{
			DateTime date = new DateTime(2000, 2, 1);
			int expected = 24002;
			int actual;
			actual = DateTimeExtensions.TotalMonths(date);
			Assert.AreEqual(expected, actual);
		}

		void ApplicationNowTestHelper(DateTime date)
		{
			var result = date.ApplicationNow();

			Assert.AreEqual(date, result);
		}

		[TestMethod]
		public void ApplicationNowShouldReturnDateTimePassedInWhenOverrideNotSet()
		{
			var date = new DateTime(2012, 11, 15);
			ApplicationNowTestHelper(date);

			var nowUtc = DateTime.UtcNow;
			ApplicationNowTestHelper(nowUtc);
		}
	}
}
