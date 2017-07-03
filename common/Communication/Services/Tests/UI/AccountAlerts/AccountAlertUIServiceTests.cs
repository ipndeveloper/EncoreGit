using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Common.Models;
using NetSteps.Communication.Common;
using NetSteps.Communication.Services.Context.Mocks;
using NetSteps.Communication.UI.Common;
using NetSteps.Communication.UI.Services;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;

namespace NetSteps.Communication.Services.Tests.UI.AccountAlerts
{
    [TestClass]
    public class AccountAlertUIServiceTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            WireupCoordinator.SelfConfigure();
        }

        [TestMethod]
        public void Wireup_Test()
        {
            var service = Create.New<IAccountAlertUIService>();
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void GetMessages_FiltersByAccountIdAndDisplayKind()
        {
            var test = new
            {
                AccountId = 1,
                ExpectedCount = 2
            };

            var service = CreateService();
            var searchParameters = Create.New<IAccountAlertUISearchParameters>();
            searchParameters.AccountId = test.AccountId;

            var models = service.GetMessages(searchParameters, CreateLocalizationInfo());
            Assert.AreEqual(test.ExpectedCount, models.Count);
        }

        [TestMethod]
        public void GetModals_FiltersByAccountIdAndDisplayKind()
        {
            var test = new
            {
                AccountId = 1,
                ExpectedCount = 1
            };

            var service = CreateService();
            var searchParameters = Create.New<IAccountAlertUISearchParameters>();
            searchParameters.AccountId = test.AccountId;

            var models = service.GetModals(searchParameters, CreateLocalizationInfo());
            Assert.AreEqual(test.ExpectedCount, models.Count);
        }

        [TestMethod]
        public void GetMessages_ReturnsOnlyActiveAlerts()
        {
            var test = new
            {
                AccountId = 2,
                ExpectedCount = 3
            };

            var service = CreateService();
            var searchParameters = Create.New<IAccountAlertUISearchParameters>();
            searchParameters.AccountId = test.AccountId;

            var models = service.GetMessages(searchParameters, CreateLocalizationInfo());
            Assert.AreEqual(test.ExpectedCount, models.Count);
        }

        [TestMethod]
        public void GetMessages_SortsByCreatedDate()
        {
            var test = new
            {
                AccountId = 2,
                ExpectedAccountAlertIds = new[] { 2, 1, 3 }
            };

            var service = CreateService();
            var searchParameters = Create.New<IAccountAlertUISearchParameters>();
            searchParameters.AccountId = test.AccountId;

            var models = service.GetMessages(searchParameters, CreateLocalizationInfo());
            Assert.AreEqual(test.ExpectedAccountAlertIds.Count(), models.Count);
            for (int i = 0; i < test.ExpectedAccountAlertIds.Count(); i++)
            {
                Assert.AreEqual(test.ExpectedAccountAlertIds[i], models[i].AccountAlertId);
            }
        }

        private static MockCommunicationDatabase CreateMockDatabase()
        {
            return new MockCommunicationDatabase()
                //.InitializeData()
                .AddMessageAccountAlert(accountId: 1, accountAlertDisplayKindId: (int)CommunicationConstants.AccountAlertDisplayKind.Message)
                .AddMessageAccountAlert(accountId: 1, accountAlertDisplayKindId: (int)CommunicationConstants.AccountAlertDisplayKind.Message)
                .AddMessageAccountAlert(accountId: 1, accountAlertDisplayKindId: (int)CommunicationConstants.AccountAlertDisplayKind.Modal)
                .AddMessageAccountAlert(accountAlertId: 1, accountId: 2, accountAlertDisplayKindId: (int)CommunicationConstants.AccountAlertDisplayKind.Message, createdDateUtc: new DateTime(2010, 6, 1))
                .AddMessageAccountAlert(accountAlertId: 2, accountId: 2, accountAlertDisplayKindId: (int)CommunicationConstants.AccountAlertDisplayKind.Message, createdDateUtc: new DateTime(2010, 2, 1))
                .AddMessageAccountAlert(accountAlertId: 3, accountId: 2, accountAlertDisplayKindId: (int)CommunicationConstants.AccountAlertDisplayKind.Message, createdDateUtc: new DateTime(2012, 1, 1))
                .AddMessageAccountAlert(accountAlertId: 4, accountId: 2, accountAlertDisplayKindId: (int)CommunicationConstants.AccountAlertDisplayKind.Message, createdDateUtc: new DateTime(2010, 1, 1), expirationDateUtc: new DateTime(2010, 2, 1))
                .AddMessageAccountAlert(accountAlertId: 5, accountId: 2, accountAlertDisplayKindId: (int)CommunicationConstants.AccountAlertDisplayKind.Message, createdDateUtc: new DateTime(2010, 12, 31), dismissedDateUtc: new DateTime(2011, 1, 5))
            ;
        }

        private static IAccountAlertUIService CreateService()
        {
            var database = CreateMockDatabase();
            return CreateService(() => new MockCommunicationContext(database));
        }

        private static IAccountAlertUIService CreateService(Func<ICommunicationContext> contextFactory)
        {
            Contract.Requires<ArgumentNullException>(contextFactory != null);

            return new AccountAlertUIService(
                new AccountAlertUIProviderCollection(new[]
                {
                    new KeyValuePair<Guid, Lazy<IAccountAlertUIProvider>>(
                        CommunicationConstants.AccountAlertProviderKey.Message,
                        new Lazy<IAccountAlertUIProvider>(() => new MessageAccountAlertUIProvider(
                            new MessageAccountAlertService(
                                contextFactory,
                                new MessageAccountAlertRepository()
                            )
                        ))
                    )
                }),
                new AccountAlertService(
                    new AccountAlertProviderCollection(new[]
                    {
                        new KeyValuePair<Guid, Lazy<IAccountAlertProvider>>(
                            CommunicationConstants.AccountAlertProviderKey.Message,
                            new Lazy<IAccountAlertProvider>(() => new MessageAccountAlertProvider(
                                new MessageAccountAlertService(
                                    contextFactory,
                                    new MessageAccountAlertRepository()
                                )
                            ))
                        )
                    }),
                    contextFactory
                )
            );
        }

        private static ILocalizationInfo CreateLocalizationInfo(
            string cultureName = null,
            int? languageId = null)
        {
            var cultureInfo = Create.New<ILocalizationInfo>();
            cultureInfo.CultureName = cultureName ?? "en-US";
            cultureInfo.LanguageId = languageId ?? 1;
            return cultureInfo;
        }
    }
}
