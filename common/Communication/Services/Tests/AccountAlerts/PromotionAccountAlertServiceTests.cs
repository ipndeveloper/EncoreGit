using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Communication.Common;
using NetSteps.Communication.Services;
using NetSteps.Communication.Services.Context.Mocks;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using M = NetSteps.Communication.Services.Models;

namespace NetSteps.Communication.Services.Tests
{
    [TestClass]
    public class PromotionAccountAlertServiceTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            WireupCoordinator.SelfConfigure();
        }

        [TestMethod]
        public void Wireup_Test()
        {
            var service = Create.New<IPromotionAccountAlertService>();
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void Dismiss_ValidAccountId_SetsDismissedDateUtc()
        {
            var test = new
            {
                AccountAlertId = 1,
                AccountId = 1
            };

            var context = new MockCommunicationContext(CreateMockDatabase());
            var service = CreateService(() => context);
            var original = context.PromotionAccountAlerts.Find(test.AccountAlertId);
            Assert.IsNotNull(original);
            Assert.IsNull(original.AccountAlert.DismissedDateUtc);

            service.Dismiss(test.AccountAlertId, test.AccountId);

            var updated = context.PromotionAccountAlerts.Find(test.AccountAlertId);
            Assert.IsNotNull(updated);
            Assert.IsNotNull(updated.AccountAlert.DismissedDateUtc);
        }

        [TestMethod]
        public void Dismiss_InvalidAccountId_ThrowsException()
        {
            var test = new
            {
                AccountAlertId = 1,
                AccountId = 2
            };

            var context = new MockCommunicationContext(CreateMockDatabase());
            var service = CreateService(() => context);
            var original = context.PromotionAccountAlerts.Find(test.AccountAlertId);
            Assert.IsNotNull(original);
            Assert.IsNull(original.AccountAlert.DismissedDateUtc);

            Exception exception = null;
            try
            {
                service.Dismiss(test.AccountAlertId, test.AccountId);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);
        }

        private static MockCommunicationDatabase CreateMockDatabase()
        {
            return new MockCommunicationDatabase()
                .InitializeData()
                // Put data for tests here
                .AddPromotionAccountAlert(accountAlertId: 1, accountId: 1)
            ;
        }

        private static IPromotionAccountAlertService CreateService()
        {
            var database = CreateMockDatabase();
            return CreateService(() => new MockCommunicationContext(database));
        }

        private static IPromotionAccountAlertService CreateService(Func<ICommunicationContext> contextFactory)
        {
            Contract.Requires<ArgumentNullException>(contextFactory != null);

            return new PromotionAccountAlertService(
                contextFactory,
                new PromotionAccountAlertRepository()
            );
        }
    }
}
