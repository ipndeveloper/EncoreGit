using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Modules.AccountRenewal.Common.Models;
using NetSteps.Modules.AccountRenewal.Common.Results;

namespace NetSteps.Modules.AccountRenewal.Common
{
    /// <summary>
    /// Account Renewal Functions
    /// </summary>
    [ContractClass(typeof(AccountRenewalContract))]
    public interface IAccountRenewal
    {
        /// <summary>
        /// Load a renewal account.
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        IRenewalAccountResult LoadRenewalAccount(int accountID);

        /// <summary>
        /// load all accounts that are less than a particular date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        IList<IRenewalAccountResult> LoadRenewalDateAccounts(DateTime date);

        /// <summary>
        /// Update the Account Status for an account.
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="accountStatusID"></param>
        /// <returns></returns>
        IRenewalAccountResult UpdateAccountStatus(int accountID, short accountStatusID);        

        /// <summary>
        /// Update the Renewal Date for an account.
        /// </summary>
        /// <param name="renewalAccount"></param>
        /// <returns></returns>
        IRenewalAccountResult UpdateRenewalDate(int accountID, int orderID);
    }

    [ContractClassFor(typeof(IAccountRenewal))]
    internal abstract class AccountRenewalContract : IAccountRenewal
    {
        public IRenewalAccountResult LoadRenewalAccount(int accountID)
        {
            Contract.Requires<ArgumentNullException>(accountID != 0);
            Contract.Ensures(Contract.Result<IRenewalAccountResult>() != null);

            throw new NotImplementedException();
        }

        public IList<IRenewalAccountResult> LoadRenewalDateAccounts(DateTime date)
        {
            Contract.Requires<ArgumentNullException>(date != null);
            Contract.Ensures(Contract.Result<IList<IRenewalAccountResult>>() != null);

            throw new NotImplementedException();
        }

        public IRenewalAccountResult UpdateAccountStatus(int accountID, short accountStatusID)
        {
            Contract.Requires<ArgumentNullException>(accountID != 0);
            Contract.Requires<ArgumentNullException>(accountStatusID != 0);

            throw new NotImplementedException();
        }

        public IRenewalAccountResult UpdateRenewalDate(int accountID, int orderID)
        {            
            Contract.Requires<ArgumentNullException>(accountID != 0);
            Contract.Requires<ArgumentNullException>(orderID != 0);

            Contract.Ensures(Contract.Result<IRenewalAccountResult>() != null);

            throw new NotImplementedException();
        }
    }
}
