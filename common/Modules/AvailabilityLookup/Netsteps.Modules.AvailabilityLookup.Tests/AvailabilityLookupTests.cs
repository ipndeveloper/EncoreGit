using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Modules.AvailabilityLookup.Common;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Modules.AvailabilityLookup.Tests
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class AvailabilityLookupTests
	{
		public AvailabilityLookupTests()
		{
			//
			// TODO: Add constructor logic here
			//
		}

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

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		private ISiteUrlAccount CreateTestSite(string hostname, int market)
		{
			var res = Create.Mutation(Create.New<ISiteUrlAccount>(), it =>
			{
				it.AccountID = 2;
				it.SiteID = 0;
				it.Url = hostname;
				it.MarketID = market;			
			});
						
			return res;
		}

		[TestMethod]
		public void AvailabilityLookup_LookupSucessful_NoMarketID_ReturnsNewILookupResultWithAccountID()
		{
			var mock = new Mock<ISiteRepositoryAdapter>();
			mock.Setup<ISiteUrlAccount>(x => x.LoadByUrl(It.IsAny<string>())).Returns((string s) => CreateTestSite(s, 1));

			var lookup = new DefaultAvailabilityLookup(mock.Object);

			var result = lookup.Lookup("existingSiteURL");

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(ILookupResult));
			Assert.IsTrue(result.Success);
			Assert.AreEqual(result.AccountID, 2);
		}

		[TestMethod]
		public void AvailabilityLookup_LookupUnSucessful_NoMarketID_ReturnsNewILookupResult()
		{
			var mock = new Mock<ISiteRepositoryAdapter>();
			mock.Setup<ISiteUrlAccount>(x => x.LoadByUrl(It.IsAny<string>()));

			var lookup = new DefaultAvailabilityLookup(mock.Object);

			var result = lookup.Lookup("notExistingURL");

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(ILookupResult));
			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void AvailabilityLookup_LookupSucessful_MarketID_ReturnsNewILookupResultWithAccountID()
		{
			var mock = new Mock<ISiteRepositoryAdapter>();
			mock.Setup<ISiteUrlAccount>(x => x.LoadByMarketAndUrl(It.IsAny<int>(), It.IsAny<string>())).Returns((int i, string s) => CreateTestSite(s, i));

			var lookup = new DefaultAvailabilityLookup(mock.Object);

			var result = lookup.Lookup(1, "existingSiteURL");

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(ILookupResult));
			Assert.IsTrue(result.Success);
			Assert.AreEqual(result.AccountID, 2);
		}

		[TestMethod]
		public void AvailabilityLookup_LookupUnSucessful_MarketID_ReturnsNewILookupResul()
		{
			var mock = new Mock<ISiteRepositoryAdapter>();
			mock.Setup<ISiteUrlAccount>(x => x.LoadByMarketAndUrl(It.IsAny<int>(), It.IsAny<string>()));

			var lookup = new DefaultAvailabilityLookup(mock.Object);

			var result = lookup.Lookup(1, "notExistingURL");

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(ILookupResult));
			Assert.IsFalse(result.Success);
		}
	}
}
