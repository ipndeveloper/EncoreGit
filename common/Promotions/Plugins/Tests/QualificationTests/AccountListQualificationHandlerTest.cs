using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Qualifications;
using NetSteps.Promotions.Plugins.Tests.QualificationTests;
using NetSteps.Encore.Core.Wireup;

namespace NetSteps.Promotions.Plugins.Test.QualificationTests
{
    [TestClass]
    public class AccountListQualificationHandlerTest : BaseQualificationHandlerTestProxy<AccountListQualificationHandler>
    {
        private void mockWireup()
        {
        }

        [TestMethod]
        public void AccountListQualificationHandler_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup();
                // test the IOC registration to verify that we've got it correct
                var registry = Create.New<IDataObjectExtensionProviderRegistry>();
                var handler = registry.RetrieveExtensionProvider(QualificationExtensionProviderKeys.AccountListProviderKey);
                Assert.IsNotNull(handler);
                Assert.IsInstanceOfType(handler, typeof(AccountListQualificationHandler));
            }
        }

        [TestMethod]
        public void AccountListQualificationHandler_should_match_iforderContext_account_number_is_contained_in_account_number_list()
        {
            using (var container = Create.NewContainer())
            {
                var orderContext = GetMockOrderContext(1);

                AccountListQualificationHandler handler = new AccountListQualificationHandler();
                IAccountListQualificationExtension extension = Create.New<IAccountListQualificationExtension>();
                extension.AccountNumbers.Add(orderContext.Order.OrderCustomers[0].AccountID);

                Assert.IsTrue(handler.Matches(Create.New<IPromotion>(), extension, orderContext));
            }
        }

        [TestMethod]
        public void AccountListQualificationHandler_should_not_match_iforderContext_account_number_is_not_contained_in_account_number_list()
        {
            using (var container = Create.NewContainer())
            {
                var orderContext = GetMockOrderContext(1);

                AccountListQualificationHandler handler = new AccountListQualificationHandler();
                IAccountListQualificationExtension extension = Create.New<IAccountListQualificationExtension>();
                int randomAccountNumber = 0;
                while (randomAccountNumber == 0 && randomAccountNumber == orderContext.Order.OrderCustomers[0].AccountID)
                {
                    randomAccountNumber = new Random().Next();
                }
                extension.AccountNumbers.Add(randomAccountNumber);

                Assert.IsTrue(!handler.Matches(Create.New<IPromotion>(), extension, orderContext));
            }
        }
    }
}
