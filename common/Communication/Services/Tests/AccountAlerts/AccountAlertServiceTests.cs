using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Communication.Common;
using NetSteps.Communication.Services.Context.Mocks;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;

namespace NetSteps.Communication.Services.Tests.AccountAlerts
{
    [TestClass]
    public class AccountAlertServiceTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            WireupCoordinator.SelfConfigure();
        }

        [TestMethod]
        public void Wireup_Test()
        {
            var service = Create.New<IAccountAlertService>();
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void GetStub_ReturnsStub()
        {
            var test = new
            {
                AccountAlertId = 1
            };

            var service = CreateService();
            var accountAlertStub = service.GetStub(test.AccountAlertId);
            Assert.AreEqual(test.AccountAlertId, accountAlertStub.AccountAlertId);

        }

        [TestMethod]
        public void GetAll_Test()
        {
            var service = CreateService();
            var accountAlerts = service.GetAll();
            Assert.IsTrue(accountAlerts.Any());
        }

        private static MockCommunicationDatabase CreateMockDatabase()
        {
            return new MockCommunicationDatabase()
                .InitializeData()
                // Put data for tests here
                .AddPromotionAccountAlert(accountAlertId: 1)
            ;
        }

        private static IAccountAlertService CreateService()
        {
            var database = CreateMockDatabase();
            return CreateService(() => new MockCommunicationContext(database));
        }

        private static IAccountAlertService CreateService(Func<ICommunicationContext> contextFactory)
        {
            Contract.Requires<ArgumentNullException>(contextFactory != null);

            return new AccountAlertService(
                new AccountAlertProviderCollection(new[]
                {
                    new KeyValuePair<Guid, Lazy<IAccountAlertProvider>>(
                        CommunicationConstants.AccountAlertProviderKey.Promotion,
                        new Lazy<IAccountAlertProvider>(() => new PromotionAccountAlertProvider(
                            new PromotionAccountAlertService(
                                contextFactory,
                                new PromotionAccountAlertRepository()
                            )
                        ))
                    )
                }),
                contextFactory
            );
        }
    }
}
