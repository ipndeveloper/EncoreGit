using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Data.Common.Services;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Enrollment.Service;
using NetSteps.Enrollment.Service.Tests.Mocks;

namespace NetSteps.Enrollment.Tests
{
	[TestClass]
	public class EnrollmentServiceTests : EnrollmentService
	{
		/// <summary>
		/// This override is to remove any config files from our test
		/// </summary>
		/// <returns></returns>
		public override int GetCorporateSponsorID()
		{
			return 1;
		}

		[TestInitialize]
		public void Initialize()
		{
			using (var root = Container.Root)
			{
				root.Registry
					.ForType<IEnrollmentRepository>()
					.Register<MockEnrollmentRepository>()
					.End();

				root.Registry
					.ForType<IOrderService>()
					.Register<MockOrderService>()
					.End();
			}
		}

		[TestMethod]
		public void IsAccountTypeSignUpEnabled_ValueInRange_ReturnsTrue()
		{
			short accountType = 1;

			bool actual = IsAccountTypeSignUpEnabled(accountType);

			Assert.IsTrue(actual);
		}

		[TestMethod]
		public void IsAccountTypeSignUpEnabled_ValueOutsideOfRange_ReturnsFalse()
		{
			short accountType = 4;

			bool actual = IsAccountTypeSignUpEnabled(accountType);

			Assert.IsFalse(actual);
		}

		[TestMethod]
		public void IsAccountTypeOptOutable_Distributor_ReturnsFalse()
		{
			int accountType = EnrollmentRepository.DistributorAccountID;

			bool actual = IsAccountTypeOptOutable((short)accountType);

			Assert.IsFalse(actual);
		}

		[TestMethod]
		public void IsAccountTypeOptOutable_NonDistributor_ReturnsTrue()
		{
			int accountType = EnrollmentRepository.DistributorAccountID + 1;

			bool actual = IsAccountTypeOptOutable((short)accountType);

			Assert.IsTrue(actual);
		}

		[TestMethod]
		public void OptOut_DistributorWhichIsInvalid_ContextRemainsUnchanged()
		{
			var enrollmentContext = createContext(1, 5, true, 1000);

			OptOut(enrollmentContext);

			Assert.IsTrue(enrollmentContext.SponsorID == 5);
			Assert.IsTrue(enrollmentContext.EnrollerID == 5);
			Assert.IsTrue(enrollmentContext.EnrollingAccount.SponsorID == 5);
			Assert.IsTrue(enrollmentContext.EnrollingAccount.EnrollerID == 5);
			Assert.IsTrue(enrollmentContext.PlacementID == 5);
			Assert.IsTrue(enrollmentContext.InitialOrder.ConsultantID == 5);
			Assert.IsTrue(enrollmentContext.InitialOrder.ParentOrderID == 1000);
			Assert.IsTrue(!enrollmentContext.EnrollingAccount.IsOptedOut);
		}

		[TestMethod]
		public void OptOut_NoOrder_OptsUserOut()
		{
			var enrollmentContext = createContext(2, 5, false);

			OptOut(enrollmentContext);

			int corporateID = GetCorporateSponsorID();
			Assert.IsTrue(enrollmentContext.SponsorID == corporateID);
			Assert.IsTrue(enrollmentContext.EnrollerID == corporateID);
			Assert.IsTrue(enrollmentContext.PlacementID == corporateID);
			Assert.IsTrue(enrollmentContext.EnrollingAccount.SponsorID == corporateID);
			Assert.IsTrue(enrollmentContext.EnrollingAccount.EnrollerID == corporateID);
			Assert.IsTrue(enrollmentContext.InitialOrder == null);
			Assert.IsTrue(enrollmentContext.EnrollingAccount.IsOptedOut);
		}

		[TestMethod]
		public void OptOut_HasOrder_OptsUserOut()
		{
			var enrollmentContext = createContext(2, 5, true, 1000);

			OptOut(enrollmentContext);

			int corporateID = GetCorporateSponsorID();
			Assert.IsTrue(enrollmentContext.SponsorID == corporateID);
			Assert.IsTrue(enrollmentContext.EnrollerID == corporateID);
			Assert.IsTrue(enrollmentContext.PlacementID == corporateID);
			Assert.IsTrue(enrollmentContext.EnrollingAccount.SponsorID == corporateID);
			Assert.IsTrue(enrollmentContext.EnrollingAccount.EnrollerID == corporateID);
			Assert.IsTrue(enrollmentContext.InitialOrder.ConsultantID == corporateID);
			Assert.IsTrue(enrollmentContext.InitialOrder.ParentOrderID == null);
			Assert.IsTrue(enrollmentContext.EnrollingAccount.IsOptedOut);
		}

		private IEnrollmentContext createContext(int accountTypeID, int sponsorID, bool haveOrder, int? parentOrderID = null)
		{
			var enrollmentContext = new MockEnrollmentContext();
			enrollmentContext.SponsorID = sponsorID;
			enrollmentContext.EnrollerID = sponsorID;
			enrollmentContext.PlacementID = sponsorID;
			if (haveOrder)
			{
				enrollmentContext.InitialOrder = new MockOrder();
				enrollmentContext.InitialOrder.ConsultantID = sponsorID;
				enrollmentContext.InitialOrder.ParentOrderID = parentOrderID;
			}
			enrollmentContext.EnrollingAccount = new MockAccount();
			enrollmentContext.EnrollingAccount.IsOptedOut = false;
			enrollmentContext.EnrollingAccount.AccountTypeID = (short)accountTypeID;
			enrollmentContext.EnrollingAccount.EnrollerID = sponsorID;
			enrollmentContext.EnrollingAccount.SponsorID = sponsorID;
			enrollmentContext.AccountTypeID = accountTypeID;

			return enrollmentContext;
		}
	}
}
