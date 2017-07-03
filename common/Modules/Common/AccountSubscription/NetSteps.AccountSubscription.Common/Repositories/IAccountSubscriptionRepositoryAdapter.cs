using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.AccountSubscription.Common
{
    /// <summary>
    /// Account Subscription Repository Adapter
    /// </summary>
    public interface IAccountSubscriptionRepositoryAdapter
    {
        /// <summary>
        /// Load the account types.
        /// </summary>
        /// <returns></returns>
        IAccountTypes LoadAccountTypes();

        /// <summary>
        /// Load the account subscriptions.
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        IAccountSubscriptionResult LoadAccountSubscription(IAccountSubscription account);

        /// <summary>
        /// load all accounts subscriptions that are less than a particular date.
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
        /// Load the account subscription products.
        /// </summary>
        /// <returns></returns>
        IList<IProduct> LoadAccountSubscriptionProducts(int subscriptionKindID);

        /// <summary>
        /// Load the products on an order.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        IList<IProduct> LoadOrderProducts(IAccountSubscription account);

        /// <summary>
        /// Load the subscription values;
        /// </summary>
        /// <returns></returns>
        IAccountSubscriptionValues LoadSubscriptionValues();

        /// <summary>
        /// Save a new account subscription.
        /// </summary>
        /// <param name="renewalAccount"></param>
        /// <returns></returns>
        IAccountSubscriptionResult SaveAccountSubscription(int subscriptionKindID, IAccountSubscription account);
        
        /// <summary>
        /// Update the Account Status for an account.
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="accountStatusID"></param>
        /// <returns></returns>
        IAccountSubscriptionResult UpdateAccountStatus(int accountID, short accountStatusID);

        /// <summary>
        /// Update the account subscription.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        IAccountSubscriptionResult UpdateAccountSubscription(IAccountSubscription account);
    }
}
