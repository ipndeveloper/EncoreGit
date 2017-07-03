using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Communication.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Communication.Services.Context.Mocks;
using System.Diagnostics.Contracts;
using System;

namespace NetSteps.Communication.Services.Tests.AccountAlerts
{
    [TestClass]
    public class MessageAccountAlertServiceTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            WireupCoordinator.SelfConfigure();
        }

        [TestMethod]
        public void Wireup_Test()
        {
            var service = Create.New<IMessageAccountAlertService>();
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
            var original = context.MessageAccountAlerts.Find(test.AccountAlertId);
            Assert.IsNotNull(original);
            Assert.IsNull(original.AccountAlert.DismissedDateUtc);

            service.Dismiss(test.AccountAlertId, test.AccountId);

            var updated = context.MessageAccountAlerts.Find(test.AccountAlertId);
            Assert.IsNotNull(updated);
            Assert.IsNotNull(updated.AccountAlert.DismissedDateUtc);
        }

        private static MockCommunicationDatabase CreateMockDatabase()
        {
            return new MockCommunicationDatabase()
                .InitializeData()
                // Put data for tests here
                .AddMessageAccountAlert(accountAlertId: 1, accountId: 1)
            ;
        }

        private static IMessageAccountAlertService CreateService()
        {
            var database = CreateMockDatabase();
            return CreateService(() => new MockCommunicationContext(database));
        }

        private static IMessageAccountAlertService CreateService(Func<ICommunicationContext> contextFactory)
        {
            Contract.Requires<ArgumentNullException>(contextFactory != null);

            return new MessageAccountAlertService(
                contextFactory,
                new MessageAccountAlertRepository()
            );
        }
    }
}
