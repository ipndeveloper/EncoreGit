using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Plugins.Qualifications;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Tests.QualificationTests;
using NetSteps.Promotions.Common.Model;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Data.Common.Services;

namespace NetSteps.Promotions.Plugins.Test.QualificationTests
{
    [TestClass]
    public class AccountTypeQualificationHandlerTest : BaseQualificationHandlerTestProxy<AccountTypeQualificationHandler>
    {
		/// <summary>
		/// Mocks the wireup.
		/// </summary>
		/// <param name="current">The current.</param>
		private void MockWireup(IContainer current)
		{
			
		}

        [TestMethod]
        public void AccountTypeQualificationHandler_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
				MockWireup(container);

                // test the IOC registration to verify that we've got it correct
                var registry = Create.New<IDataObjectExtensionProviderRegistry>();
                var handler = registry.RetrieveExtensionProvider(QualificationExtensionProviderKeys.AccountTypeProviderKey);
                Assert.IsNotNull(handler);
                Assert.IsInstanceOfType(handler, typeof(AccountTypeQualificationHandler));
            }
        }

        [TestMethod]
        public void AccountTypeQualificationHandler_should_match_iforderContext_account_type_is_contained_in_account_type_list()
        {
            using (var container = Create.NewContainer())
            {
				MockWireup(container);

                var orderContext = GetMockOrderContext(1);

                AccountTypeQualificationHandler handler = new AccountTypeQualificationHandler();
                IAccountTypeQualificationExtension extension = Create.New<IAccountTypeQualificationExtension>();
                extension.AccountTypes.Add(orderContext.Order.OrderCustomers[0].AccountTypeID);

                Assert.IsTrue(handler.Matches(Create.New<IPromotion>(), extension, orderContext));
            }
        }

        [TestMethod]
        public void AccountTypeQualificationHandler_should_not_match_iforderContext_account_type_is_not_contained_in_account_type_list()
        {
            using (var container = Create.NewContainer())
            {
				MockWireup(container);

                var orderContext = GetMockOrderContext(1);

                AccountTypeQualificationHandler handler = new AccountTypeQualificationHandler();
                IAccountTypeQualificationExtension extension = Create.New<IAccountTypeQualificationExtension>();
                short randomAccountType = 0;
                while (randomAccountType == 0 && randomAccountType == orderContext.Order.OrderCustomers[0].AccountTypeID)
                {
                    randomAccountType = (short)new Random().Next();
                }
                extension.AccountTypes.Add(randomAccountType);

                Assert.IsTrue(!handler.Matches(Create.New<IPromotion>(), extension, orderContext));
            }
        }
    }
}
