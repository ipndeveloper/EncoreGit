using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Content.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.Enrollments.Common;
using NetSteps.Modules.Enrollments.Common.Models;
using NetSteps.Modules.Enrollments.Common.Results;
using NetSteps.Modules.AvailabilityLookup.Common;
using NetSteps.Modules.Order.Common;
using NetSteps.Modules.Order.Common.Results;

namespace NetSteps.Modules.Enrollments.Tests
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class EnrollmentTests
	{
		public EnrollmentTests()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		Mock<IEnrollmentRepositoryAdapter> mockRepo = new Mock<IEnrollmentRepositoryAdapter>();
		Mock<DefaultEnrollment> mockDefault = new Mock<DefaultEnrollment>();


		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		private IEnrollingAccount CreateTestEnrollingAccount()
		{
			var productIDs = new Dictionary<int, int>();

			productIDs.Add(1, 1);

			var mainAddress = Create.Mutation(Create.New<IEnrollmentAddress>(), ma =>
			{
				ma.AddressLine1 = "test line 1";
				ma.AddressLine2 = "test line 2";
				ma.AddressLine3 = "test line 3";
				ma.Attention = "attention";
				ma.City = "Lehi";
				ma.County = "Utah";
				ma.State = "UT";
				ma.PostalCode = "84043";
				ma.CountryID = 1;
			});
			var billingAddress = Create.Mutation(Create.New<IEnrollmentAddress>(), ba =>
			{
				ba.AddressLine1 = "test line 1";
				ba.AddressLine2 = "test line 2";
				ba.AddressLine3 = "test line 3";
				ba.Attention = "attention";
				ba.City = "Lehi";
				ba.County = "Utah";
				ba.State = "UT";
				ba.PostalCode = "84043";
				ba.CountryID = 1;
			});
			var shippingAddress = Create.Mutation(Create.New<IEnrollmentAddress>(), sa =>
			{
				sa.AddressLine1 = "test line 1";
				sa.AddressLine2 = "test line 2";
				sa.AddressLine3 = "test line 3";
				sa.Attention = "attention";
				sa.City = "Lehi";
				sa.County = "Utah";
				sa.State = "UT";
				sa.PostalCode = "84043";
				sa.CountryID = 1;
			});

			var billingProfile = Create.Mutation(Create.New<IEnrollmentBillingProfile>(), cc =>
				{
					cc.CCNumber = "4111111111111111";
					cc.CVV = "123";
					cc.ExpirationDate = Convert.ToDateTime("01/01/2030");
					cc.NameOnCard = "test";
					cc.PaymentTypeID = 1;
					cc.BillingAddress = billingAddress;
				});

			var account = Create.Mutation(Create.New<IEnrollingAccount>(), a =>
				{
					a.BillingProfile = billingProfile;
					a.DateOfBirth = Convert.ToDateTime("01/01/1980");
					a.Email = "test@test.com";
					a.FirstName = "test";
					a.GenderID = 1;
					a.LanguageID = 1;
					a.LastName = "tester";
					a.MiddleName = "t";
					//a.Password = "testtest";
					a.PhoneNumber = "123-123-1234";
					//a.ProductIDs = productIDs;
					//a.PWSName = "test";
					a.ShippingAddress = shippingAddress;
					a.AccountTypeID = 1;
					a.IsEntity = false;
					a.TaxNumber = "123-12-1234";
					a.MainAddress = mainAddress;
					//a.ShippingMethod = "test";
				});

			return account;
		}

		private IEnrollmentResult CreateTestEnrollmentResult()
		{
			return Create.New<IEnrollmentResult>();
		}

		private IEnrollmentAccountResult CreateTestEnrollmentAccountResult(IEnrollingAccount a)
		{
			var result = Create.New<IEnrollmentAccountResult>();
			result.Success = true;
			result.ErrorMessages = new List<string>();
			result.AccountID = 1;

			return result;
		}

		private IEnrollingUserResult CreateTestEnrollmentUserResult()
		{
			var result = Create.New<IEnrollingUserResult>();
			result.Success = true;
			result.ErrorMessages = new List<string>();

			result.UserName = "test";

			return result;
		}

		private IEnrollmentOrderResult CreateTestOrderResult()
		{
			var result = Create.New<IEnrollmentOrderResult>();
			result.Success = true;
			result.ErrorMessages = new List<string>();

			result.OrderID = 1;

			return result;
		}

		private IPaymentResponse CreatePaymentResponse()
		{
			var result = Create.New<IPaymentResponse>();
			result.FailureCount = 0;
			result.Message = "Success";
			result.Success = true;

			return result;
		}

		private IEnrollmentOrder CreateEnrollmentOrder()
		{
			var product = Create.New<IProduct>();
			product.ProductID = 1;
			product.Quantity = 1;

			IList<IProduct> products = new List<IProduct>();
			products.Add(product);

			var result = Create.New<IEnrollmentOrder>();
			result.AccountID = 1000;
			result.AccountTypeID = 1;
			result.CurrencyID = 1;
			result.OrderTypeID = 11;
			result.SiteID = 1;
			result.Products = products;

			return result;
		}

		private IOrderCreateResult CreateEnrollmentOrderResult(int orderID)
		{
			var result = Create.New<IOrderCreateResult>();
			result.ErrorMessages = new List<string>();
			result.OrderID = orderID;
			result.Success = true;

			return result;
		}

		private IEnrollmentAutoshipOrder CreateTestAutoshipOrder()
		{
			var product = Create.New<IProduct>();
			product.ProductID = 1;
			product.Quantity = 1;
			product.Sku = "2300C";

			IList<IProduct> products = new List<IProduct>();
			products.Add(product);

			var result = Create.New<IEnrollmentAutoshipOrder>();
			result.AutoshipOrderID = 2;

			return result;
		}

		private IEnrollmentAutoshipOrderResult CreateAutoshipOrderResult(int autoshipOrderID)
		{
			var result = Create.New<IEnrollmentAutoshipOrderResult>();
			result.ErrorMessages = new List<string>();
			result.AutoshipOrderID = autoshipOrderID;
			result.Success = true;

			return result;
		}

        private IEnrollmentSubscriptionOrder CreateEnrollmentSubscriptionOrder()
        {
            var product = Create.New<IProduct>();
            product.ProductID = 1;
            product.Quantity = 1;

            IList<IProduct> products = new List<IProduct>();
            products.Add(product);

            var result = Create.New<IEnrollmentSubscriptionOrder>();
            result.AccountID = 1000;
            result.AccountTypeID = 1;
            result.CurrencyID = 1;
            result.OrderTypeID = 11;
            result.SiteID = 1;
            result.AutoshipScheduleID = 1;
            result.MarketID = 1;
            result.Url = "Test";
            result.Products = products;

            return result;
        }

        private IEnrollmentAutoshipSchedule CreateEnrollmentAutoshipSchedule()
        {
            var product = Create.New<IProduct>();
			product.ProductID = 1;
			product.Quantity = 1;

			IList<IProduct> products = new List<IProduct>();
			products.Add(product);

            var result = Create.New<IEnrollmentAutoshipSchedule>();
            result.AutoshipScheduleID = 1;
            result.AutoshipScheduleTypeId = 1;
            result.IsActive = true;
            result.IsEnrollable = true;
            result.Products = products;

            return result;
        }

		private void SetupRepoMock()
		{
			mockRepo.Setup<IEnrollmentAccountResult>(x => x.CreateAccount(It.IsAny<IEnrollingAccount>())).Returns((IEnrollingAccount a) => CreateTestEnrollmentAccountResult(a));
			mockRepo.Setup<IEnrollingUserResult>(x => x.CreateUser(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Returns(CreateTestEnrollmentUserResult());
			mockRepo.Setup<bool>(x => x.IsTaxNumberAvailable(It.IsAny<string>(), It.IsAny<int>())).Returns(true);
			mockRepo.Setup<bool>(x => x.ValidateEmailAddressAvailibility(It.IsAny<string>())).Returns(true);
			mockRepo.Setup(x => x.CreateMailAccountForDistributors(It.IsAny<int>(), It.IsAny<int>()));
			mockRepo.Setup<IEnrollingAccount>(x => x.GetProspectAccountForUpgradeIfExists(It.IsAny<string>(), It.IsAny<int>()));//.Returns(CreateTestEnrollingAccount());
			mockRepo.Setup<string>(x => x.GetTerm(It.IsAny<string>(), It.IsAny<string>())).Returns((string a, string b) => b);
		}

		[TestMethod]
		public void DefaultEnrollment_CreateUser_CreatesUser()
		{
			SetupRepoMock();

			var autoRepo = new Mock<IEnrollmentAutoshipRepositoryAdapter>();
			var ordRepo = new Mock<ISiteOrder>();
			var payRepo = new Mock<IPaymentRepositoryAdapter>();
			var lookup = new Mock<IAvailabilityLookup>();
            var term = new Mock<ITermResolver>();
			var enroll = new DefaultEnrollment(autoRepo.Object, mockRepo.Object, payRepo.Object, lookup.Object, ordRepo.Object, term.Object);

			var result = enroll.CreateUser(1, "testtest", 1);

			Assert.IsTrue(result.Success);
			Assert.AreEqual("test", result.UserName);
			Assert.AreEqual(0, result.ErrorMessages.Count);
		}

		[TestMethod]
		public void DefaultEnrollment_CreateAccount_CreatesAccount()
		{
			SetupRepoMock();

			var autoRepo = new Mock<IEnrollmentAutoshipRepositoryAdapter>();
			var ordRepo = new Mock<ISiteOrder>();
			var payRepo = new Mock<IPaymentRepositoryAdapter>();
			var lookup = new Mock<IAvailabilityLookup>();
            var term = new Mock<ITermResolver>();
			var enroll = new DefaultEnrollment(autoRepo.Object, mockRepo.Object, payRepo.Object, lookup.Object, ordRepo.Object, term.Object);

			var result = enroll.CreateUser(1, "testtest", 1);

			Assert.IsTrue(result.Success);
			Assert.AreEqual("test", result.UserName);
			Assert.AreEqual(0, result.ErrorMessages.Count);
		}

		[TestMethod]
		public void DefaultEnrollment_CreateSiteSubscription_CreatesSiteSubscription()
		{
			SetupRepoMock();

			var autoRepo = new Mock<IEnrollmentAutoshipRepositoryAdapter>();
			var ordRepo = new Mock<ISiteOrder>();
			var payRepo = new Mock<IPaymentRepositoryAdapter>();
			var lookup = new Mock<IAvailabilityLookup>();
            var term = new Mock<ITermResolver>();

            var order = CreateEnrollmentSubscriptionOrder();
			var enrollOrderResult = CreateEnrollmentOrderResult(10000);
            var enrollAutoshipResult = CreateAutoshipOrderResult(2);
            var enrollAutoshipSchedule = CreateEnrollmentAutoshipSchedule();

            autoRepo.Setup<IEnrollmentAutoshipSchedule>(x => x.GetAutoshipSchedule(It.IsAny<int>())).Returns(enrollAutoshipSchedule);
            mockRepo.Setup<IEnrollmentOrderResult>(x => x.CreateSite(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(CreateTestOrderResult());
            lookup.Setup<ILookupResult>(x => x.Lookup(It.IsAny<string>())).Returns(Create.New<ILookupResult>());
			ordRepo.Setup<IOrderCreateResult>(x => x.CreateOrder(It.IsAny<IOrderCreate>())).Returns(enrollOrderResult);

			var enroll = new DefaultEnrollment(autoRepo.Object, mockRepo.Object, payRepo.Object, lookup.Object, ordRepo.Object, term.Object);

            autoRepo.Setup<IEnrollmentAutoshipOrderResult>(x => x.CreateAutoshipScheduleAndOrder(It.IsAny<IEnrollmentSubscriptionOrder>())).Returns(enrollAutoshipResult);
            
			var result = enroll.CreateSiteSubscription(order);

			Assert.IsTrue(result.Success);
			Assert.AreEqual(2, result.OrderID);
			Assert.AreEqual(0, result.ErrorMessages.Count);
		}

		[TestMethod]
		public void DefaultEnrollment_CreateEnrollmentOrder_Returns_New_IEnrollmentOrderResult()
		{
			// Arrange
			int orderID = 10000;

			var autoRepo = new Mock<IEnrollmentAutoshipRepositoryAdapter>();
			var ordRepo = new Mock<ISiteOrder>();
			var payRepo = new Mock<IPaymentRepositoryAdapter>();
			var lookup = new Mock<IAvailabilityLookup>();
            var term = new Mock<ITermResolver>();
			var enroll = new DefaultEnrollment(autoRepo.Object, mockRepo.Object, payRepo.Object, lookup.Object, ordRepo.Object, term.Object);

			var order = CreateEnrollmentOrder();
			var enrollOrderResult = CreateEnrollmentOrderResult(orderID);

			ordRepo.Setup<IOrderCreateResult>(x => x.CreateOrder(It.IsAny<IOrderCreate>())).Returns(enrollOrderResult);

			// Act
			var result = enroll.CreateEnrollmentOrder(order);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(IEnrollmentOrderResult));
			Assert.AreEqual(orderID, result.OrderID);
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual(0, result.ErrorMessages.Count);
		}

		[TestMethod]
		public void DefaultEnrollment_SubmitPayments_Returns_New_IPaymentResponseResult()
		{
			// Arrange
			var autoRepo = new Mock<IEnrollmentAutoshipRepositoryAdapter>();
			var enrollRepo = new Mock<IEnrollmentRepositoryAdapter>();
			var ordRepo = new Mock<ISiteOrder>();
			var payRepo = new Mock<IPaymentRepositoryAdapter>();
			var lookup = new Mock<IAvailabilityLookup>();
            var term = new Mock<ITermResolver>();
			var enroll = new DefaultEnrollment(autoRepo.Object, mockRepo.Object, payRepo.Object, lookup.Object, ordRepo.Object, term.Object);

			payRepo.Setup<IPaymentResponse>(x => x.SubmitPayments(It.IsAny<int>())).Returns(CreatePaymentResponse());

			// Act
			var result = enroll.SubmitPayments(5);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(IPaymentResponseResult));
			Assert.AreEqual(true, result.Success);
		}

		[TestMethod]
		public void DefaultEnrollment_CreateAutoshipOrder_Returns_New_IEnrollmentAutoshipOrderResult()
		{
            int orderID = 1000;
			int autoshipOrderID = 2;
			var autoRepo = new Mock<IEnrollmentAutoshipRepositoryAdapter>();
            var ordRepo = new Mock<ISiteOrder>();
			var payRepo = new Mock<IPaymentRepositoryAdapter>();
			var lookup = new Mock<IAvailabilityLookup>();
            var term = new Mock<ITermResolver>();
			var enroll = new DefaultEnrollment(autoRepo.Object, mockRepo.Object, payRepo.Object, lookup.Object, ordRepo.Object, term.Object);
            var order = CreateEnrollmentSubscriptionOrder();
			var enrollAutoshipResult = CreateAutoshipOrderResult(autoshipOrderID);
            var enrollAutoshipSchedule = CreateEnrollmentAutoshipSchedule();
            var enrollOrderResult = CreateEnrollmentOrderResult(orderID);

            autoRepo.Setup<IEnrollmentAutoshipOrderResult>(x => x.CreateAutoshipScheduleAndOrder(It.IsAny<IEnrollmentSubscriptionOrder>())).Returns(enrollAutoshipResult);
            autoRepo.Setup<IEnrollmentAutoshipSchedule>(x => x.GetAutoshipSchedule(It.IsAny<int>())).Returns(enrollAutoshipSchedule);
			ordRepo.Setup<IOrderCreateResult>(x => x.CreateOrder(It.IsAny<IOrderCreate>())).Returns(enrollOrderResult);

			var result = enroll.CreateAutoshipOrder(order);

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(IEnrollmentAutoshipOrderResult));
			Assert.AreEqual(autoshipOrderID, result.AutoshipOrderID);
			Assert.AreEqual(true, result.Success);
			Assert.AreEqual(0, result.ErrorMessages.Count);
		}

	}
}
