using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.Autoship.Common;

namespace NetSteps.Modules.Autoship.Tests
{
	[TestClass]
	public class AutoshipTests
	{
		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		private IEnumerable<ISiteAutoship> CreateTestSite(int accountID, int autoshipScheduleID)
		{
			List<ISiteAutoship> results = new List<ISiteAutoship>();
			for (int i = 1; i < 11; i++)
			{
				var finish = Create.Mutation(Create.New<ISiteAutoship>(), it =>
				{
					it.AccountID = accountID;
					it.OrderID = i;
					it.OrderDate = DateTime.Today;
					it.BonusVolume = 0;
					it.OrderTotal = 300;
					it.AutoShipScheduleID = autoshipScheduleID;
				});
				results.Add(finish);
			}

			return results;
		}

		private ISiteAutoship CreateTestCancelSite(int accountID, int autoshipID)
		{
			var res = Create.Mutation(Create.New<ISiteAutoship>(), ic =>
			{
				ic.AccountID = 2;
			});
			return res;
		}

		[TestMethod]
		public void AutoshipSearch_SearchSuccessful_ReturnsNewISearchResultWithAccountIDAndAutoshipScheduleID()
		{
			var mock = new Mock<IAutoshipRepositoryAdapter>();
			mock.Setup<IEnumerable<ISiteAutoship>>(x => x.SearchByAccount(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).Returns((int i, int s, bool b) => CreateTestSite(i, s));

			var search = new DefaultAutoship(mock.Object);
			var result = search.Search(1, 1);

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(IAutoshipSearchResult));
		}

		[TestMethod]
		public void AutoshipSearch_SearchUnSuccessful_NoAutoshipScheduleID_ReturnsNewISearchResult()
		{
			var mock = new Mock<IAutoshipRepositoryAdapter>();
			mock.Setup<IEnumerable<ISiteAutoship>>(x => x.SearchByAccount(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())); 

			var search = new DefaultAutoship(mock.Object);
			var result = search.Search(2, 2);

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(IAutoshipSearchResult));
			Assert.IsFalse(result.Success);

		}

		[TestMethod]
		public void AutoshipCancel_CancelSuccessful_AccountID_ReturnsNewICancelResultWithAutoshipID()
		{
			var mock = new Mock<IAutoshipRepositoryAdapter>();
			mock.Setup<ISiteAutoship>(x => x.CancelByAccountIDAndAutoshipID(It.IsAny<int>(), It.IsAny<int>())).Returns((int i, int t) => CreateTestCancelSite(i, t));

			var cancel = new DefaultAutoship(mock.Object);
			var result = cancel.Cancel(2, 1);

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(IAutoshipCancelResult));
			Assert.IsTrue(result.Success);
			Assert.AreNotEqual(result.AccountID, result.AutoshipID);
		}

		[TestMethod]
		public void AutoshipCancel_CancelUnSuccessful_AccountID_ReturnsNewICancelResult()
		{
			var mock = new Mock<IAutoshipRepositoryAdapter>();
			mock.Setup<ISiteAutoship>(x => x.CancelByAccountIDAndAutoshipID(It.IsAny<int>(), It.IsAny<int>()));

			var cancel = new DefaultAutoship(mock.Object);
			var result = cancel.Cancel(0, 1);

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(IAutoshipCancelResult));
			Assert.IsFalse(result.Success);
		}
	}
}
