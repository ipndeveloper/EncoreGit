using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Modules.Downline.Common;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Modules.Downline.Tests
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class DownlineSearchTests
	{
		public DownlineSearchTests()
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

		private IEnumerable<IDownlineAccount> CreateTestResult()
		{
			List<IDownlineAccount> testList = new List<IDownlineAccount>();
			for (int i = 0; i < 11; i++)
			{
				testList.Add(Create.Mutation(Create.New<IDownlineAccount>(), a =>
							{
								a.AccountID = i;
								a.FirstName = "test" + i.ToString();
								a.LastName = "test" + i.ToString();
							}));
				
			}
			return (testList as IEnumerable<IDownlineAccount>).AsQueryable<IDownlineAccount>();
		}
		private IEnumerable<IDownlineAccount> CreateTestResult(int accountID)
		{
			List<IDownlineAccount> testList = new List<IDownlineAccount>();
			
			testList.Add(Create.Mutation(Create.New<IDownlineAccount>(), a =>
			{
				a.AccountID = accountID;
				a.FirstName = "test" + accountID.ToString();
				a.LastName = "test" + accountID.ToString();
			}));

			return (testList as IEnumerable<IDownlineAccount>).AsQueryable<IDownlineAccount>();
		}

		[TestMethod]
		public void DownlineSearch_Search_SponsorAndAccountID_ReturnsNewIDownlineSearchResult()
		{
			var mock = new Mock<IDownlineRepositoryAdapter>();
			mock.Setup<IEnumerable<IDownlineAccount>>(x => x.Search(It.IsAny<ISearchDownlineModel>())).Returns(CreateTestResult(1));

			var search = new DefaultDownlineSearch(mock.Object);
			var model = Create.New<ISearchDownlineModel>();
			model.SponsorID = 1;
			model.AccountID = 1;

			var result = search.Search(model);

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(IDownlineSearchResult));
			Assert.IsTrue(result.Success);
			Assert.AreEqual(1, result.DownlineAccounts.Count());
			Assert.AreEqual(result.DownlineAccounts.FirstOrDefault().AccountID, 1);
			Assert.AreEqual(result.DownlineAccounts.FirstOrDefault().FirstName, "test1");
			Assert.AreEqual(result.DownlineAccounts.FirstOrDefault().LastName, "test1");
		}

		[TestMethod]
		public void DownlineSearch_Search_SponsorAndName_ReturnsNewIDownlineSearchResult()
		{
			var mock = new Mock<IDownlineRepositoryAdapter>();
			mock.Setup<IEnumerable<IDownlineAccount>>(x => x.Search(It.IsAny<ISearchDownlineModel>())).Returns(CreateTestResult());

			var search = new DefaultDownlineSearch(mock.Object);
			var model = Create.New<ISearchDownlineModel>();
			model.SponsorID = 1;
			model.Query = "test test";

			var result = search.Search(model);

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(IDownlineSearchResult));
			Assert.IsTrue(result.Success);
			Assert.AreEqual(11, result.DownlineAccounts.Count());
			Assert.AreEqual(result.DownlineAccounts.FirstOrDefault().AccountID, 0);
			Assert.AreEqual(result.DownlineAccounts.FirstOrDefault().FirstName, "test0");
			Assert.AreEqual(result.DownlineAccounts.FirstOrDefault().LastName, "test0");
		}
	}
}
