using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Encore.Core.IoC;
using NetSteps.AccountSubscription.Common;
using NetSteps.AccountSubscription.Service;
using NetSteps.Content.Common;

namespace NetSteps.AccountSubscription.Tests
{
    [TestClass]
    public class AccountSubscriptionServiceTests
    {
        /// <summary>
        /// Account Renewal Tests
        /// </summary>
        [TestClass]
        public class AccountRenewalTests
        {
            private IAccountSubscriptionResult CreateNewAccountSubscriptionResult(int accountID, short accountTypeID, int orderID, DateTime lastRenewalDateUTC)
            {
                var result = Create.New<IAccountSubscriptionResult>();
                result.ErrorMessages = new List<string>();
                result.AccountID = accountID;
                result.AccountTypeID = accountTypeID;
                result.EnrollmentDateUTC = DateTime.Now;
                result.RenewalEndDate = lastRenewalDateUTC;
                result.OrderID = orderID;
                result.Success = true;

                return result;
            }

            private IAccountTypes CreateNewAccountTypes()
            {
                var result = Create.New<IAccountTypes>();
                result.DistributorAccountTypeID = 1;
                result.LoyalCustomerAccountTypeID = 2;
                result.RetailCustomerAccountTypeID = 3;

                return result;
            }

            private IAccountSubscription CreateNewAccountSubscription(int accountID, int orderID)
            {
                var result = Create.New<IAccountSubscription>();
                result.AccountID = accountID;
                result.OrderID = orderID;

                return result;
            }

            private IList<IProduct> CreateRenewalProducts()
            {
                IList<IProduct> result = new List<IProduct>();

                var product1 = Create.New<IProduct>();
                product1.ProductID = 585;
                product1.SKU = "806";
                product1.IntervalCount = 1;

                var product2 = Create.New<IProduct>();
                product2.ProductID = 586;
                product2.SKU = "AU806";
                product2.IntervalCount = 1;

                var product3 = Create.New<IProduct>();
                product3.ProductID = 587;
                product3.SKU = "UK806";
                product3.IntervalCount = 1;

                result.Add(product1);
                result.Add(product2);
                result.Add(product3);

                return result;
            }

            private IList<IProduct> CreateOrderProducts()
            {
                IList<IProduct> result = new List<IProduct>();

                var product = Create.New<IProduct>();
                product.ProductID = 585;
                product.SKU = "806";

                result.Add(product);

                return result;
            }

            private IAccountSubscriptionValues CreateNewAccountSubscriptionValues()
            {
                var result = Create.New<IAccountSubscriptionValues>();
                result.SubscriptionKindID = 1;

                return result;
            }

            [TestMethod]
            public void UpdateRenewalDateForLoyalCustomer_Returns_New_IRenewalAccountResult()
            {
                // Arrange
                int accountID = 1000;
                int orderID = 10000;
                short accountTypeID = 2;
                DateTime lastRenewalDateUTC = DateTime.MaxValue;

                var accountRepo = new Mock<IAccountSubscriptionRepositoryAdapter>();
                var termRepo = new Mock<ITermResolver>();

                var accountResult = CreateNewAccountSubscriptionResult(accountID, accountTypeID, orderID, lastRenewalDateUTC);
                var accountTypes = CreateNewAccountTypes();

                accountRepo.Setup<IAccountSubscriptionResult>(x => x.LoadAccountSubscription(It.IsAny<IAccountSubscription>())).Returns(accountResult);
                accountRepo.Setup<IAccountSubscriptionResult>(x => x.UpdateAccountSubscription(It.IsAny<IAccountSubscription>())).Returns(accountResult);
                accountRepo.Setup<IAccountTypes>(x => x.LoadAccountTypes()).Returns(accountTypes);

                var module = new AccountSubscriptionService(accountRepo.Object, termRepo.Object);

                // Act
                var result = module.UpdateAccountSubscriptionRenewalDate(accountID, orderID);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(IAccountSubscriptionResult));
                Assert.AreEqual(accountID, result.AccountID);
                Assert.AreEqual(orderID, result.OrderID);
                Assert.AreEqual(lastRenewalDateUTC, result.RenewalEndDate);
            }

            [TestMethod]
            public void UpdateRenewalDateForDistributor_Returns_New_IRenewalAccountResult()
            {
                // Arrange
                int accountID = 1000;
                int orderID = 10000;
                short accountTypeID = 1;
                DateTime lastRenewalDateUTC = DateTime.Today.AddYears(1);

                var accountRepo = new Mock<IAccountSubscriptionRepositoryAdapter>();
                var termRepo = new Mock<ITermResolver>();

                var accountResult = CreateNewAccountSubscriptionResult(accountID, accountTypeID, orderID, lastRenewalDateUTC);
                var subscriptionValues = CreateNewAccountSubscriptionValues();
                var accountTypes = CreateNewAccountTypes();
                var orderProducts = CreateOrderProducts();
                var renewalProducts = CreateRenewalProducts();

                accountRepo.Setup<IAccountSubscriptionResult>(x => x.LoadAccountSubscription(It.IsAny<IAccountSubscription>())).Returns(accountResult);
                accountRepo.Setup<IAccountSubscriptionResult>(x => x.UpdateAccountSubscription(It.IsAny<IAccountSubscription>())).Returns(accountResult);
                accountRepo.Setup<IAccountTypes>(x => x.LoadAccountTypes()).Returns(accountTypes);
                accountRepo.Setup<IList<IProduct>>(x => x.LoadAccountSubscriptionProducts(It.IsAny<int>())).Returns(renewalProducts);
                accountRepo.Setup<IList<IProduct>>(x => x.LoadOrderProducts(It.IsAny<IAccountSubscription>())).Returns(orderProducts);
                accountRepo.Setup<IAccountSubscriptionValues>(x => x.LoadSubscriptionValues()).Returns(subscriptionValues);

                var module = new AccountSubscriptionService(accountRepo.Object, termRepo.Object);

                // Act
                var result = module.UpdateAccountSubscriptionRenewalDate(accountID, orderID);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(IAccountSubscriptionResult));
                Assert.AreEqual(accountID, result.AccountID);
                Assert.AreEqual(orderID, result.OrderID);
                Assert.AreEqual(lastRenewalDateUTC, result.RenewalEndDate);
            }
        }
    }
}
