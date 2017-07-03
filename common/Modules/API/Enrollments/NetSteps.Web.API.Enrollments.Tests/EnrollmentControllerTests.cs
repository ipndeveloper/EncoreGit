using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Content.Common;
using NetSteps.Diagnostics.Logging.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.Enrollments.Common;
using NetSteps.Modules.Enrollments.Common.Models;
using NetSteps.Modules.Enrollments.Common.Results;
using NetSteps.Web.API.Enrollments.Common;
using NetSteps.Web.API.Enrollments.Common.Models;

namespace NetSteps.Web.API.Enrollments.Tests
{
    [TestClass]
    public class EnrollmentControllerTests
    {

        #region Helper Methods

        private EnrollmentViewModel CreateEnrollmentViewModel(int accountID)
        {
            var result = Create.New<EnrollmentViewModel>();

            result.Account = CreateEnrollingAccount(accountID);
            result.MainAddress = CreateEnrollmentAddressViewModel();
            result.BillingAddress = CreateEnrollmentAddressViewModel();
            result.ShippingAddress = CreateEnrollmentAddressViewModel();
            result.BillingProfile = CreateEnrollmentBillingProfileViewModel();

            return result;
        }

        private EnrollmentUserViewModel CreateEnrollmentUserViewModel()
        {
            var result = Create.New<EnrollmentUserViewModel>();
            result.AccountID = 1000;
            result.LanguageID = 1;
            result.Password = "PASS";

            return result;
        }

        private EnrollmentAccountViewModel CreateEnrollingAccount(int accountID)
        {
            var result = Create.New<EnrollmentAccountViewModel>();
            result.AccountID = accountID;
            result.AccountTypeID = 1;            
            result.DateOfBirth = new DateTime(1955, 1, 1);
            result.Email = "Test@Test.com";
            result.FirstName = "Test";
            result.GenderID = 1;
            result.IsEntity = true;
            result.LanguageID = 1;
            result.LastName = "Name";            
            result.MiddleName = "T";
            result.PhoneNumber = "555-5555";
            result.Placement = 1;            
            result.SponsorID = 1;
            result.TaxExempt = true;
            result.TaxNumber = "5555";
         
            return result;
        }

        private EnrollmentAddressViewModel CreateEnrollmentAddressViewModel()
        {
            var address = Create.New<EnrollmentAddressViewModel>();
            address.AddressLine1 = "line 1";
            address.Attention = "test";
            address.City = "Sandy";
            address.CountryID = 1;
            address.County = "Salt Lake";
            address.PostalCode = "84070";
            address.State = "UT";

            return address;
        }

        private EnrollmentBillingProfileViewModel CreateEnrollmentBillingProfileViewModel()
        {
            var profile = Create.New<EnrollmentBillingProfileViewModel>();
            profile.CCNumber = "4111111111111111111111";
            profile.CVV = "123";
            profile.ExpirationDate = DateTime.Now;
            profile.NameOnCard = "test";
            profile.PaymentTypeID = 1;

            return profile;
        }

        private EnrollmentOrderViewModel CreateEnrollmentOrder(int accountID)
        {
            var result = Create.New<EnrollmentOrderViewModel>();
            result.AccountID = accountID;
            result.AccountTypeID = 1;
            result.CurrencyID = 1;
            result.OrderTypeID = 1;
            result.Products.Add(CreateProductViewModel());
            result.SiteID = 1;

            return result;
        }

        private EnrollmentSubscriptionOrderViewModel CreateEnrollmentSubscriptionOrder(int accountID)
        {
            var result = Create.New<EnrollmentSubscriptionOrderViewModel>();
            result.AccountID = accountID;
            result.AccountTypeID = 1;
            result.AutoshipScheduleID = 1;
            result.CurrencyID = 1;
            result.MarketID = 1;
            result.OrderTypeID = 1;
            result.Products.Add(CreateProductViewModel());
            result.SiteID = 1;
            result.Url = "URL";

            return result;
        }

        private ProductViewModel CreateProductViewModel()
        {
            var result = Create.New<ProductViewModel>();
            result.ProductID = 135;
            result.Quantity = 1;
            result.Sku = "111";

            return result;
        }

        private IEnrollmentAccountResult CreateEnrollmentAccountResult(int accountID)
        {
            var result = Create.New<IEnrollmentAccountResult>();
            result.AccountID = accountID;
            result.ErrorMessages = new List<string>();
            result.Success = true;

            return result;
        }
        
        private IEnrollmentAutoshipOrderResult CreateEnrollmentAutoshipOrderResult()
        {
            var result = Create.New<IEnrollmentAutoshipOrderResult>();
            result.AutoshipOrderID = 10000;            
            result.ErrorMessages = new List<string>();
            result.Success = true;
            result.TemplateOrderID = 5000;

            return result;
        }
        
        private IEnrollmentOrderResult CreateEnrollmentOrderResult(int orderID)
        {
            var result = Create.New<IEnrollmentOrderResult>();
            result.ErrorMessages = new List<string>();
            result.OrderID = orderID;
            result.Success = true;

            return result;
        }

        private IEnrollingUserResult CreateEnrollingUserResult()
        {
            var result = Create.New<IEnrollingUserResult>();
            result.ErrorMessages = new List<string>();
            result.Success = true;
            result.UserName = "UserName";

            return result;
        }

        #endregion

        //[TestMethod]
        //public void CreateAccount_Returns_New_EnrollmentResultViewModel_With_Success_Message()
        //{
        //    // Arrange
        //    int accountID = 1000;

        //    var enroll = new Mock<IEnrollment>();
        //    var term = new Mock<ITermResolver>();
        //    var log = new Mock<ILogResolver>();

        //    var enrollment = CreateEnrollmentViewModel(accountID);            
        //    var enrollResult = CreateEnrollmentAccountResult(accountID);

        //    enroll.Setup<IEnrollmentAccountResult>(x => x.CreateAccount(It.IsAny<IEnrollingAccount>())).Returns(enrollResult);

        //    var controller = new EnrollmentController(enroll.Object, log.Object, term.Object);

        //    // Act
        //    var enrollmentResult = controller.CreateAccount(enrollment);
                        
        //    // Assert
        //    Assert.IsNotNull(enrollmentResult);
        //    Assert.IsInstanceOfType(enrollmentResult, typeof(EnrollmentResultViewModel));
        //    Assert.AreEqual(true, enrollmentResult.Success);
        //}

        //[TestMethod]
        //public void CreateProductSubscription_Returns_New_EnrollmentResultViewModel_With_Success_Message()
        //{
        //    // Arrange
        //    int accountID = 1000;

        //    var enroll = new Mock<IEnrollment>();
        //    var term = new Mock<ITermResolver>();
        //    var log = new Mock<ILogResolver>();

        //    var order = CreateEnrollmentSubscriptionOrder(accountID);           
        //    var autoResult = CreateEnrollmentAutoshipOrderResult();

        //    enroll.Setup<IEnrollmentAutoshipOrderResult>(x => x.CreateAutoshipOrder(It.IsAny<IEnrollmentSubscriptionOrder>())).Returns(autoResult);

        //    var controller = new EnrollmentController(enroll.Object, log.Object, term.Object);

        //    // Act
        //    var enrollmentResult = controller.CreateProductSubscription(order);

        //    // Assert
        //    Assert.IsNotNull(enrollmentResult);
        //    Assert.IsInstanceOfType(enrollmentResult, typeof(EnrollmentResultViewModel));
        //    Assert.AreEqual(true, enrollmentResult.Success);
        //}

        //[TestMethod]
        //public void CreateEnrollmentOrder_Returns_New_EnrollmentResultViewModel_With_Success_Message()
        //{
        //    // Arrange
        //    int accountID = 1000;
        //    int orderID = 10000;

        //    var enroll = new Mock<IEnrollment>();
        //    var term = new Mock<ITermResolver>();
        //    var log = new Mock<ILogResolver>();

        //    var order = CreateEnrollmentOrder(accountID);
        //    var orderResult = CreateEnrollmentOrderResult(orderID);
            
        //    enroll.Setup<IEnrollmentOrderResult>(x => x.CreateEnrollmentOrder(It.IsAny<IEnrollmentOrder>())).Returns(orderResult);

        //    var controller = new EnrollmentController(enroll.Object, log.Object, term.Object);

        //    // Act
        //    var enrollmentResult = controller.CreateEnrollmentOrder(order);

        //    // Assert
        //    Assert.IsNotNull(enrollmentResult);
        //    Assert.IsInstanceOfType(enrollmentResult, typeof(EnrollmentResultViewModel));
        //    Assert.AreEqual(true, enrollmentResult.Success);
        //}

        //[TestMethod]
        //public void CreateSiteSubscription_Returns_New_EnrollmentResultViewModel_With_Success_Message()
        //{
        //    // Arrange
        //    int accountID = 1000;
        //    int orderID = 10000;

        //    var enroll = new Mock<IEnrollment>();
        //    var term = new Mock<ITermResolver>();
        //    var log = new Mock<ILogResolver>();

        //    var order = CreateEnrollmentSubscriptionOrder(accountID);
        //    var enrollResult = CreateEnrollmentOrderResult(orderID);
            
        //    enroll.Setup<IEnrollmentOrderResult>(x => x.CreateSiteSubscription(It.IsAny<IEnrollmentSubscriptionOrder>())).Returns(enrollResult);

        //    var controller = new EnrollmentController(enroll.Object, log.Object, term.Object);

        //    // Act
        //    var enrollmentResult = controller.CreateSiteSubscription(order);

        //    // Assert
        //    Assert.IsNotNull(enrollmentResult);
        //    Assert.IsInstanceOfType(enrollmentResult, typeof(EnrollmentResultViewModel));
        //    Assert.AreEqual(true, enrollmentResult.Success);
        //}

        //[TestMethod]
        //public void CreateUser_Returns_New_EnrollmentResultViewModel_With_Success_Message()
        //{
        //    // Arrange
        //    var enroll = new Mock<IEnrollment>();
        //    var term = new Mock<ITermResolver>();
        //    var log = new Mock<ILogResolver>();

        //    var enrollResult = CreateEnrollingUserResult();
        //    var enrollmentUser = CreateEnrollmentUserViewModel();
            
        //    enroll.Setup<IEnrollingUserResult>(x => x.CreateUser(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Returns(enrollResult);

        //    var controller = new EnrollmentController(enroll.Object, log.Object, term.Object);

        //    // Act
        //    var enrollmentResult = controller.CreateUser(enrollmentUser);

        //    // Assert
        //    Assert.IsNotNull(enrollmentResult);
        //    Assert.IsInstanceOfType(enrollmentResult, typeof(EnrollmentResultViewModel));
        //    Assert.AreEqual(true, enrollmentResult.Success);
        //}

        [TestMethod]
        public void EnrollAccount_Returns_New_EnrollmentResultViewModel_With_Success_Message()
        {
            // Arrange
            int accountID = 1000;
            int orderID = 10000;

            var enroll = new Mock<IEnrollment>();
            var term = new Mock<ITermResolver>();
            var log = new Mock<ILogResolver>();

            var model = CreateEnrollmentViewModel(accountID);
            model.User = CreateEnrollmentUserViewModel();
            model.Order = CreateEnrollmentOrder(accountID);
            model.ProductSubscriptionOrder = CreateEnrollmentSubscriptionOrder(accountID);
            model.SiteSubscriptionOrder = CreateEnrollmentSubscriptionOrder(accountID);

            var enrollmentUserResult = CreateEnrollingUserResult();
            var enrollmentAccountResult = CreateEnrollmentAccountResult(accountID);
            var enrollmentOrderResult = CreateEnrollmentOrderResult(orderID);
            var enrollmentProductResult = CreateEnrollmentAutoshipOrderResult();
            var enrollmentSubscriptionResult = CreateEnrollmentOrderResult(orderID);

            enroll.Setup<IEnrollmentAccountResult>(x => x.CreateAccount(It.IsAny<IEnrollingAccount>())).Returns(enrollmentAccountResult);
            enroll.Setup<IEnrollingUserResult>(x => x.CreateUser(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Returns(enrollmentUserResult);
            enroll.Setup<IEnrollmentOrderResult>(x => x.CreateEnrollmentOrder(It.IsAny<IEnrollmentOrder>())).Returns(enrollmentOrderResult);
            enroll.Setup<IEnrollmentAutoshipOrderResult>(x => x.CreateAutoshipOrder(It.IsAny<IEnrollmentSubscriptionOrder>())).Returns(enrollmentProductResult);
            enroll.Setup<IEnrollmentOrderResult>(x => x.CreateSiteSubscription(It.IsAny<IEnrollmentSubscriptionOrder>())).Returns(enrollmentSubscriptionResult);

            var controller = new EnrollmentController(enroll.Object, log.Object, term.Object);

            // Act
            var enrollmentResult = controller.EnrollAccount(model);

            // Assert
            Assert.IsNotNull(enrollmentResult);
            Assert.IsInstanceOfType(enrollmentResult, typeof(NetSteps.Web.API.Base.Common.JsonSuccess));            
        }

    }
}
