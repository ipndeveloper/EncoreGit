using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Modules.AccountRenewal.Common.Models;
using NetSteps.Modules.AccountRenewal.Common.Results;

namespace NetSteps.Modules.AccountRenewal.Common
{
    /// <summary>
    /// Account Renewal Repository Adapter
    /// </summary>
    public interface IAccountRenewalRepositoryAdapter
    {
        /// <summary>
        /// Load the account types.
        /// </summary>
        /// <returns></returns>
        IRenewalAccountTypes LoadAccountTypes();

        /// <summary>
        /// Load the account.
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        IRenewalAccountResult LoadAccount(IRenewalAccount account);

        /// <summary>
        /// load all accounts that are less than a particular date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        IList<IRenewalAccountResult> LoadRenewalDateAccounts(DateTime date);

        /// <summary>
        /// Load the renewal products.
        /// </summary>
        /// <returns></returns>
        IList<IProduct> LoadRenewalProducts();

        /// <summary>
        /// Load the order products.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        IList<IProduct> LoadOrderProducts(IRenewalAccount account);

        /// <summary>
        /// Save the renewal date for an account.
        /// </summary>
        /// <param name="renewalAccount"></param>
        /// <returns></returns>
        IRenewalAccountResult SaveRenewalDate(IRenewalAccount account);

        /// <summary>
        /// Update the Account Status for an account.
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="accountStatusID"></param>
        /// <returns></returns>
        IRenewalAccountResult UpdateAccountStatus(int accountID, short accountStatusID);
    }
}
