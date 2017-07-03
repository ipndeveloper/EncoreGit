using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using NetSteps.Modules.Enrollments.Common.Results;
using NetSteps.Modules.Enrollments.Common.Models;
using NetSteps.Modules.Order.Common;

namespace NetSteps.Modules.Enrollments.Common
{
	/// <summary>
	/// Enrollment functions
	/// </summary>
	[ContractClass(typeof(EnrollmentContract))]
	public interface IEnrollment
	{
		/// <summary>
		/// Create a new Account
		/// </summary>
		/// <param name="account"></param>
		/// <returns></returns>
		IEnrollmentAccountResult CreateAccount(IEnrollingAccount account);
		/// <summary>
        /// Create a new enrollment order
        /// </summary>
        /// <param name="enrollmentOrder"></param>
        /// <returns></returns>
        IEnrollmentOrderResult CreateEnrollmentOrder(IOrderCreate enrollmentOrder);
       /// <summary>
		/// Create a new User
		/// </summary>
		/// <param name="accountID"></param>
		/// <param name="password"></param>
		/// <param name="languageID"></param>
		/// <returns></returns>
		IEnrollingUserResult CreateUser(int accountID, string password, int languageID);
		/// <summary>
		/// Create a new PWS site subscription
		/// </summary>
		/// <param name="subscriptionOrder"></param>
		/// <returns></returns>
        IEnrollmentOrderResult CreateSiteSubscription(IEnrollmentSubscriptionOrder subscriptionOrder);
		/// <summary>
		/// Create a new Autoship Order and Template.
		/// </summary>
		/// <param name="subscriptionOrder"></param>
		/// <returns></returns>
        IEnrollmentAutoshipOrderResult CreateAutoshipOrder(IEnrollmentSubscriptionOrder subscriptionOrder);
        /// <summary>
        /// Create a new Autoship Order, Template, and Initial Order.
        /// </summary>
        /// <param name="subscriptionOrder"></param>
        /// <returns></returns>
        IEnrollmentAutoshipOrderResult CreateProductSubscriptionOrder(IEnrollmentSubscriptionOrder subscriptionOrder);
		/// <summary>
		/// Activate an Account
		/// </summary>
		/// <param name="accountID"></param>
		void ActivateAccount(int accountID);
		/// <summary>
        /// Submit Order Payments
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
		IPaymentResponseResult SubmitPayments(int orderID);
	}

	[ContractClassFor(typeof(IEnrollment))]
	internal abstract class EnrollmentContract : IEnrollment
	{
		public IEnrollmentAccountResult CreateAccount(IEnrollingAccount account)
		{
			Contract.Requires<ArgumentNullException>(account != null);

			Contract.Ensures(Contract.Result<IEnrollmentAccountResult>() != null);

			throw new NotImplementedException();
		}

		public IEnrollmentOrderResult CreateEnrollmentOrder(IOrderCreate enrollmentOrder)
        {
            Contract.Requires<ArgumentNullException>(enrollmentOrder != null);

            Contract.Ensures(Contract.Result<IEnrollmentOrderResult>() != null);

            throw new NotImplementedException();
        }

		public IEnrollingUserResult CreateUser(int accountID, string password, int languageID)
		{
			Contract.Requires<ArgumentNullException>(password != null);
			Contract.Requires<ArgumentException>(password != "");
			Contract.Requires<ArgumentOutOfRangeException>(accountID > 0);
			Contract.Requires<ArgumentOutOfRangeException>(languageID > 0);

			Contract.Ensures(Contract.Result<IEnrollingUserResult>() != null);

			throw new NotImplementedException();
		}

        public IEnrollmentOrderResult CreateSiteSubscription(IEnrollmentSubscriptionOrder subscriptionOrder)
		{			
            Contract.Requires<ArgumentNullException>(subscriptionOrder != null);
            Contract.Requires<ArgumentException>(subscriptionOrder.Url != string.Empty);
            Contract.Requires<ArgumentOutOfRangeException>(subscriptionOrder.AccountID > 0);
            Contract.Requires<ArgumentOutOfRangeException>(subscriptionOrder.MarketID > 0);
            Contract.Requires<ArgumentOutOfRangeException>(subscriptionOrder.AutoshipScheduleID > 0);

			Contract.Ensures(Contract.Result<IEnrollmentOrderResult>() != null);

			throw new NotImplementedException();
		}

        public IEnrollmentAutoshipOrderResult CreateAutoshipOrder(IEnrollmentSubscriptionOrder subscriptionOrder)
		{
            Contract.Requires<ArgumentNullException>(subscriptionOrder != null && subscriptionOrder.AutoshipScheduleID != 0);

			Contract.Ensures(Contract.Result<IEnrollmentAutoshipOrderResult>() != null);

			throw new NotImplementedException();
		}

        public IEnrollmentAutoshipOrderResult CreateProductSubscriptionOrder(IEnrollmentSubscriptionOrder subscriptionOrder)
        {
            Contract.Requires<ArgumentNullException>(subscriptionOrder != null && subscriptionOrder.AutoshipScheduleID != 0);

            Contract.Ensures(Contract.Result<IEnrollmentAutoshipOrderResult>() != null);

            throw new NotImplementedException();
        }


		public void ActivateAccount(int accountID)
		{
			Contract.Requires<ArgumentException>(accountID > 0);

			throw new NotImplementedException();
		}


		public IPaymentResponseResult SubmitPayments(int orderID)
		{
			Contract.Requires<ArgumentException>(orderID > 0);

			Contract.Ensures(Contract.Result<IPaymentResponseResult>() != null);

			throw new NotImplementedException();
		}
	}
}
