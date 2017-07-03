using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Accounts.Downline.Common.Models;
using NetSteps.Accounts.Downline.Common.Repositories;
using NetSteps.Accounts.Downline.Service.Context;
using NetSteps.Accounts.Downline.Service.Repositories;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Accounts.Downline.Service.Tests.Repositories
{
	[TestClass]
	public class DownlineRepositoryTests
	{
		private static readonly Func<DownlineDbContext> __downlineDbContextFactory = () => new DownlineDbContext("TestDownlineDatabase");

		[TestInitialize]
		public void TestInitialize()
		{
			InitializeLocalDB();
		}
		
		[TestMethod]
		public void GetDownline_TopOfTree_ReturnsEntireTree()
		{
			var test = new
			{
				GetDownlineContext = CreateGetDownlineContext(rootAccountId: 1),
				ExpectedCount = 10
			};
			
			var target = new DownlineRepository(__downlineDbContextFactory);

			var result = target.GetDownline(test.GetDownlineContext);

			Assert.IsNotNull(result);
			Assert.AreEqual(test.ExpectedCount, result.Count);
		}

		[TestMethod]
		public void GetDownline_MidTree_ReturnsPartialTree()
		{
			var test = new
			{
				GetDownlineContext = CreateGetDownlineContext(rootAccountId: 4),
				ExpectedCount = 5
			};

			var target = new DownlineRepository(__downlineDbContextFactory);

			var result = target.GetDownline(test.GetDownlineContext);

			Assert.IsNotNull(result);
			Assert.AreEqual(test.ExpectedCount, result.Count);
			Assert.AreEqual(test.GetDownlineContext.RootAccountId, result[0].AccountId);
		}

		[TestMethod]
		public void GetUplineAccountIds_ExistingAccount_ReturnsUpline()
		{
			var test = new
			{
				AccountId = 10,
				ExpectedUplineAccountIds = new[] { 1, 2, 4, 9, 10 }
			};

			var target = new DownlineRepository(__downlineDbContextFactory);

			var result = target.GetUplineAccountIds(test.AccountId);

			Assert.IsNotNull(result);
			Assert.AreEqual(test.ExpectedUplineAccountIds.Length, result.Count);
			for (int i = 0; i < test.ExpectedUplineAccountIds.Length; i++)
			{
				Assert.AreEqual(test.ExpectedUplineAccountIds[i], result[i]);
			}
		}

		[TestMethod]
		public void GetDownlineAccountInfo_ExistingAccount_ReturnsAccount()
		{
			var test = new
			{
				RootAccountId = 1,
				AccountId = 4
			};

			var target = new DownlineRepository(__downlineDbContextFactory);

			var result = target.GetDownlineAccountInfo(test.RootAccountId, test.AccountId);

			Assert.IsNotNull(result);
			Assert.AreEqual(test.AccountId, result.AccountId);
		}

		[TestMethod]
		public void Wireup_Test()
		{
			var repository = Create.New<IDownlineRepository>();
			Assert.IsNotNull(repository);
		}

		#region Helpers
		private IGetDownlineContext CreateGetDownlineContext(
			IEnumerable<short> accountStatusIds = null,
			IEnumerable<short> accountTypeIds = null,
			int? maxLevels = null,
			int rootAccountId = 0)
		{
			var mock = new Mock<IGetDownlineContext>();

			mock.Setup(x => x.AccountStatusIds).Returns(accountStatusIds);
			mock.Setup(x => x.AccountTypeIds).Returns(accountTypeIds);
			mock.Setup(x => x.MaxLevels).Returns(maxLevels);
			mock.Setup(x => x.RootAccountId).Returns(rootAccountId);

			return mock.Object;
		}

		private void InitializeLocalDB()
		{
			using (var db = __downlineDbContextFactory())
			{
				db.Database.ExecuteSqlCommand("TRUNCATE TABLE Accounts.AccountInfoCache");
				db.Database.ExecuteSqlCommand("TRUNCATE TABLE Accounts.SponsorHierarchy");
				db.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.Accounts");
				db.Database.ExecuteSqlCommand(@"
SET IDENTITY_INSERT [dbo].[Accounts] ON
INSERT INTO [dbo].[Accounts] ([AccountID], [AccountNumber], [AccountTypeID], [AccountStatusID], [FirstName], [LastName]) VALUES (1, N'1', 1, 1, N'Kevin', N'California')
INSERT INTO [dbo].[Accounts] ([AccountID], [AccountNumber], [AccountTypeID], [AccountStatusID], [FirstName], [LastName]) VALUES (2, N'2', 1, 1, N'Mike', N'LastNameUnknown')
INSERT INTO [dbo].[Accounts] ([AccountID], [AccountNumber], [AccountTypeID], [AccountStatusID], [FirstName], [LastName]) VALUES (3, N'3', 1, 1, N'Cannon', N'Fodder')
INSERT INTO [dbo].[Accounts] ([AccountID], [AccountNumber], [AccountTypeID], [AccountStatusID], [FirstName], [LastName]) VALUES (4, N'4', 1, 1, N'Jeff', N'IsCool')
INSERT INTO [dbo].[Accounts] ([AccountID], [AccountNumber], [AccountTypeID], [AccountStatusID], [FirstName], [LastName]) VALUES (5, N'5', 1, 1, N'Scott', N'CanHandleIt')
INSERT INTO [dbo].[Accounts] ([AccountID], [AccountNumber], [AccountTypeID], [AccountStatusID], [FirstName], [LastName]) VALUES (6, N'6', 1, 1, N'Sally', N'DoesntWorkHere')
INSERT INTO [dbo].[Accounts] ([AccountID], [AccountNumber], [AccountTypeID], [AccountStatusID], [FirstName], [LastName]) VALUES (7, N'7', 1, 1, N'Todd', N'Karate')
INSERT INTO [dbo].[Accounts] ([AccountID], [AccountNumber], [AccountTypeID], [AccountStatusID], [FirstName], [LastName]) VALUES (8, N'8', 1, 1, N'Michael', N'Hipster')
INSERT INTO [dbo].[Accounts] ([AccountID], [AccountNumber], [AccountTypeID], [AccountStatusID], [FirstName], [LastName]) VALUES (9, N'9', 1, 1, N'Grant', N'Mazda')
INSERT INTO [dbo].[Accounts] ([AccountID], [AccountNumber], [AccountTypeID], [AccountStatusID], [FirstName], [LastName]) VALUES (10, N'10', 1, 1, N'Jason', N'Zombie')
SET IDENTITY_INSERT [dbo].[Accounts] OFF
				");
				db.Database.ExecuteSqlCommand(@"
INSERT INTO [Accounts].[SponsorHierarchy] ([AccountId], [SponsorId], [TreeLevel], [LeftAnchor], [RightAnchor], [NodeNumber], [NodeCount], [Upline]) VALUES (1, NULL, 1, 1, 20, 1, 10, 0x00000001)
INSERT INTO [Accounts].[SponsorHierarchy] ([AccountId], [SponsorId], [TreeLevel], [LeftAnchor], [RightAnchor], [NodeNumber], [NodeCount], [Upline]) VALUES (2, 1, 2, 2, 15, 2, 7, 0x0000000100000002)
INSERT INTO [Accounts].[SponsorHierarchy] ([AccountId], [SponsorId], [TreeLevel], [LeftAnchor], [RightAnchor], [NodeNumber], [NodeCount], [Upline]) VALUES (3, 1, 2, 16, 19, 3, 2, 0x0000000100000003)
INSERT INTO [Accounts].[SponsorHierarchy] ([AccountId], [SponsorId], [TreeLevel], [LeftAnchor], [RightAnchor], [NodeNumber], [NodeCount], [Upline]) VALUES (4, 2, 3, 3, 12, 4, 5, 0x000000010000000200000004)
INSERT INTO [Accounts].[SponsorHierarchy] ([AccountId], [SponsorId], [TreeLevel], [LeftAnchor], [RightAnchor], [NodeNumber], [NodeCount], [Upline]) VALUES (5, 2, 3, 13, 14, 5, 1, 0x000000010000000200000005)
INSERT INTO [Accounts].[SponsorHierarchy] ([AccountId], [SponsorId], [TreeLevel], [LeftAnchor], [RightAnchor], [NodeNumber], [NodeCount], [Upline]) VALUES (6, 3, 3, 17, 18, 6, 1, 0x000000010000000300000006)
INSERT INTO [Accounts].[SponsorHierarchy] ([AccountId], [SponsorId], [TreeLevel], [LeftAnchor], [RightAnchor], [NodeNumber], [NodeCount], [Upline]) VALUES (7, 4, 4, 4, 5, 7, 1, 0x00000001000000020000000400000007)
INSERT INTO [Accounts].[SponsorHierarchy] ([AccountId], [SponsorId], [TreeLevel], [LeftAnchor], [RightAnchor], [NodeNumber], [NodeCount], [Upline]) VALUES (8, 4, 4, 6, 7, 8, 1, 0x00000001000000020000000400000008)
INSERT INTO [Accounts].[SponsorHierarchy] ([AccountId], [SponsorId], [TreeLevel], [LeftAnchor], [RightAnchor], [NodeNumber], [NodeCount], [Upline]) VALUES (9, 4, 4, 8, 11, 9, 2, 0x00000001000000020000000400000009)
INSERT INTO [Accounts].[SponsorHierarchy] ([AccountId], [SponsorId], [TreeLevel], [LeftAnchor], [RightAnchor], [NodeNumber], [NodeCount], [Upline]) VALUES (10, 9, 5, 9, 10, 10, 1, 0x000000010000000200000004000000090000000A)
				");
				db.Database.ExecuteSqlCommand(@"
INSERT INTO [Accounts].[AccountInfoCache] ([AccountID]) VALUES (1)
INSERT INTO [Accounts].[AccountInfoCache] ([AccountID]) VALUES (2)
INSERT INTO [Accounts].[AccountInfoCache] ([AccountID]) VALUES (3)
INSERT INTO [Accounts].[AccountInfoCache] ([AccountID]) VALUES (4)
INSERT INTO [Accounts].[AccountInfoCache] ([AccountID]) VALUES (5)
INSERT INTO [Accounts].[AccountInfoCache] ([AccountID]) VALUES (6)
INSERT INTO [Accounts].[AccountInfoCache] ([AccountID]) VALUES (7)
INSERT INTO [Accounts].[AccountInfoCache] ([AccountID]) VALUES (8)
INSERT INTO [Accounts].[AccountInfoCache] ([AccountID]) VALUES (9)
INSERT INTO [Accounts].[AccountInfoCache] ([AccountID]) VALUES (10)
				");
			}
		}
		#endregion
	}
}
