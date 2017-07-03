using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Content.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.AccountRenewal.Common;
using NetSteps.Modules.AccountRenewal.Common.Models;
using NetSteps.Modules.AccountRenewal.Common.Results;

namespace NetSteps.Modules.AccountRenewal.Tests
{
    /// <summary>
    /// Account Renewal Tests
    /// </summary>
    [TestClass]
    public class AccountRenewalTests
    {
        private IRenewalAccountResult CreateNewRenewalAccountResult(int accountID, short accountTypeID, int orderID, DateTime lastRenewalDateUTC)
        {
            var result = Create.New<IRenewalAccountResult>();
            result.ErrorMessages = new List<string>();
            result.AccountID = accountID;
            result.AccountTypeID = accountTypeID;
            result.EnrollmentDateUTC = DateTime.Now;
            result.LastRenewalDateUTC = lastRenewalDateUTC;
            result.OrderID = orderID;
            result.Success = true;

            return result;
        }

        private IRenewalAccountTypes CreateNewRenewalAccountTypes()
        {
            var result = Create.New<IRenewalAccountTypes>();
            result.DistributorAccountTypeID = 1;
            result.LoyalCustomerAccountTypeID = 2;
            result.RetailCustomerAccountTypeID = 3;

            return result;
        }

        private IRenewalAccount CreateNewRenewalAccount(int accountID, int orderID)
        {
            var result = Create.New<IRenewalAccount>();
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

            var product2 = Create.New<IProduct>();
            product2.ProductID = 586;
            product2.SKU = "AU806";

            var product3 = Create.New<IProduct>();
            product3.ProductID = 587;
            product3.SKU = "UK806";

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

        [TestMethod]
        public void UpdateRenewalDateForLoyalCustomer_Returns_New_IRenewalAccountResult()
        {
            // Arrange
            int accountID = 1000;
            int orderID = 10000;
            short accountTypeID = 2;
            DateTime lastRenewalDateUTC = DateTime.MaxValue;

            var accountRepo = new Mock<IAccountRenewalRepositoryAdapter>();
            var termRepo = new Mock<ITermResolver>();
                        
            var accountResult = CreateNewRenewalAccountResult(accountID, accountTypeID, orderID, lastRenewalDateUTC);
            var accountTypes = CreateNewRenewalAccountTypes();

            accountRepo.Setup<IRenewalAccountResult>(x => x.LoadAccount(It.IsAny<IRenewalAccount>())).Returns(accountResult);
            accountRepo.Setup<IRenewalAccountResult>(x => x.SaveRenewalDate(It.IsAny<IRenewalAccount>())).Returns(accountResult);
            accountRepo.Setup<IRenewalAccountTypes>(x => x.LoadAccountTypes()).Returns(accountTypes);

            var module = new DefaultAccountRenewal(accountRepo.Object, termRepo.Object);

            // Act
            var result = module.UpdateRenewalDate(accountID, orderID);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IRenewalAccountResult));
            Assert.AreEqual(accountID, result.AccountID);
            Assert.AreEqual(orderID, result.OrderID);
            Assert.AreEqual(lastRenewalDateUTC, result.LastRenewalDateUTC);            
        }

        [TestMethod]
        public void UpdateRenewalDateForDistributor_Returns_New_IRenewalAccountResult()
        {
            // Arrange
            int accountID = 1000;            
            int orderID = 10000;
            short accountTypeID = 1;
            DateTime lastRenewalDateUTC = DateTime.Today.AddYears(1);

            var accountRepo = new Mock<IAccountRenewalRepositoryAdapter>();
            var termRepo = new Mock<ITermResolver>();
                        
            var accountResult = CreateNewRenewalAccountResult(accountID, accountTypeID, orderID, lastRenewalDateUTC);
            var accountTypes = CreateNewRenewalAccountTypes();

            var orderProducts = CreateOrderProducts();
            var renewalProducts = CreateRenewalProducts();

            accountRepo.Setup<IRenewalAccountResult>(x => x.LoadAccount(It.IsAny<IRenewalAccount>())).Returns(accountResult);
            accountRepo.Setup<IRenewalAccountResult>(x => x.SaveRenewalDate(It.IsAny<IRenewalAccount>())).Returns(accountResult);
            accountRepo.Setup<IRenewalAccountTypes>(x => x.LoadAccountTypes()).Returns(accountTypes);
            accountRepo.Setup<IList<IProduct>>(x => x.LoadRenewalProducts()).Returns(renewalProducts);
            accountRepo.Setup<IList<IProduct>>(x => x.LoadOrderProducts(It.IsAny<IRenewalAccount>())).Returns(orderProducts);

            var module = new DefaultAccountRenewal(accountRepo.Object, termRepo.Object);

            // Act
            var result = module.UpdateRenewalDate(accountID, orderID);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IRenewalAccountResult));
            Assert.AreEqual(accountID, result.AccountID);
            Assert.AreEqual(orderID, result.OrderID);
            Assert.AreEqual(lastRenewalDateUTC, result.LastRenewalDateUTC);
        }
    }
}
