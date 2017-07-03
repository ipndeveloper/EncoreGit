using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.AccountSubscription.Common
{
    /// <summary>
    /// Account Subscription Functions.
    /// </summary>
    [ContractClass(typeof(Contracts.AccountSubscriptionContract))]
    public interface IAccountSubscriptionService
    {
        /// <summary>
        /// Load an account subscription.
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        IAccountSubscriptionResult LoadAccountSubscription(int accountID);

        /// <summary>
        /// Load all subscription accounts that are on a particular date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        IList<IAccountSubscriptionResult> LoadAccountSubscriptionsByDate(DateTime date);

        /// <summary>
        /// Load all subscription accounts that are on and before a particular date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        IList<IAccountSubscriptionResult> LoadAccountSubscriptionsBeforeOrOnDate(DateTime date);        

        /// <summary>
        /// Update the Account Status for an account.
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="accountStatusID"></param>
        /// <returns></returns>
        IAccountSubscriptionResult UpdateAccountStatus(int accountID, short accountStatusID);

        /// <summary>
        /// Update the account subscription renewal date for an account by accountID and orderID.
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="orderID"></param>
        /// <returns></returns>
        IAccountSubscriptionResult UpdateAccountSubscriptionRenewalDate(int accountID, int orderID);

        /// <summary>
        /// Update the account subscription renewal date for an account by accountID and renewalDate.
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="renewalDate"></param>
        /// <returns></returns>
        IAccountSubscriptionResult UpdateAccountSubscriptionRenewalDate(int accountID, DateTime renewalDate);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IAccountSubscriptionService))]
        internal abstract class AccountSubscriptionContract : IAccountSubscriptionService
        {
            public IAccountSubscriptionResult LoadAccountSubscription(int accountID)
            {
                Contract.Requires<ArgumentNullException>(accountID != 0);
                Contract.Ensures(Contract.Result<IAccountSubscriptionResult>() != null);

                throw new NotImplementedException();
            }

            public IList<IAccountSubscriptionResult> LoadAccountSubscriptionsByDate(DateTime date)
            {
                Contract.Requires<ArgumentNullException>(date != null);
                Contract.Ensures(Contract.Result<IList<IAccountSubscriptionResult>>() != null);

                throw new NotImplementedException();
            }

            public IList<IAccountSubscriptionResult> LoadAccountSubscriptionsBeforeOrOnDate(DateTime date)
            {
                Contract.Requires<ArgumentNullException>(date != null);
                Contract.Ensures(Contract.Result<IList<IAccountSubscriptionResult>>() != null);

                throw new NotImplementedException();
            }

            public IAccountSubscriptionResult UpdateAccountStatus(int accountID, short accountStatusID)
            {
                Contract.Requires<ArgumentNullException>(accountID != 0);
                Contract.Requires<ArgumentNullException>(accountStatusID != 0);

                Contract.Ensures(Contract.Result<IAccountSubscriptionResult>() != null);

                throw new NotImplementedException();
            }

            public IAccountSubscriptionResult UpdateAccountSubscriptionRenewalDate(int accountID, int orderID)
            {
                Contract.Requires<ArgumentNullException>(accountID != 0);
                Contract.Requires<ArgumentNullException>(orderID != 0);

                Contract.Ensures(Contract.Result<IAccountSubscriptionResult>() != null);

                throw new NotImplementedException();
            }

            public IAccountSubscriptionResult UpdateAccountSubscriptionRenewalDate(int accountID, DateTime renewalDate)
            {
                Contract.Requires<ArgumentNullException>(accountID != 0);
                Contract.Requires<ArgumentNullException>(renewalDate != null);

                Contract.Ensures(Contract.Result<IAccountSubscriptionResult>() != null);

                throw new NotImplementedException();
            }
        }
    }
}
