using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Agreements.Common;
using NetSteps.Agreements.Services.Agreements;
using NetSteps.Agreements.Services.Context.Mocks;
using System.Diagnostics.Contracts;

namespace NetSteps.Agreements.Services.Tests
{
	[TestClass]
	public class AgreementsServiceTest
	{
		[ClassInitialize]
		public static void ClassInitialize(TestContext context)
		{
			WireupCoordinator.SelfConfigure();
		}

		[TestMethod]
		public void Wireup_Test()
		{
			var service = Create.New<IAgreementsService>();
			Assert.IsNotNull(service);
		}

		[TestMethod]
		public void GetAgreements_Test()
		{
			var service = CreateService();
			var agreements = service.GetAgreements(24702, 1);
			Assert.IsNotNull(agreements);
		}

		[TestMethod]
		public void SaveAcceptedAgreements_Test()
		{
			var service = CreateService();
			var agreements = service.GetAgreements(1, 1);
			service.SaveAcceptedAgreements(agreements);
			agreements = service.GetAgreements(1, 1);
			Assert.IsTrue(agreements.Count() == 0);
		}

		//[TestMethod]
		public void SaveAgreements_Test()
		{
			// method to test not yet implemented
		}

		//[TestMethod]
		public void GetAllAgreements_Test()
		{
			// method to test not yet implemented
		}

		[TestMethod]
		public void GetAgreementsVersions()
		{
			var service = CreateService();
			var agreementsVersions = service.GetAgreementVersions(1);
			Assert.IsTrue(agreementsVersions.Count() > 0);
		}

		#region Utility
		private static MockAgreementsDatabase CreateMockDatabase()
		{
			return new MockAgreementsDatabase().InitializeData();
		}

		private static IAgreementsService CreateService()
		{
			return CreateRealService();
			//var database = CreateMockDatabase();
			//return CreateService(() => new MockAgreementsContext(database));
		}

		private static IAgreementsService CreateService(Func<IAgreementsContext> contextFactory)
		{
			Contract.Requires<ArgumentNullException>(contextFactory != null);

			return new AgreementsService(contextFactory, new AgreementsRepository());
		}

		private static IAgreementsService CreateRealService()
		{
			return Create.New<IAgreementsService>();
		}
		#endregion
	}
}
