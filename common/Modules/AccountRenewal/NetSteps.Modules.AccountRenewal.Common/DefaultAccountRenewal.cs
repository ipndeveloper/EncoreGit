using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Content.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.AccountRenewal.Common.Models;
using NetSteps.Modules.AccountRenewal.Common.Results;

namespace NetSteps.Modules.AccountRenewal.Common
{
    /// <summary>
    /// Default Implementation of IAccountRenewal
    /// </summary>
    [ContainerRegister(typeof(IAccountRenewal), RegistrationBehaviors.IfNoneOther, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class DefaultAccountRenewal : IAccountRenewal
    {

        #region Declarations

        private IAccountRenewalRepositoryAdapter _accountRenewalRepository;

        private ITermResolver _termTranslation;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DefaultAccountRenewal() : this(Create.New<IAccountRenewalRepositoryAdapter>(), Create.New<ITermResolver>()) { }

        /// <summary>
        /// Constructor with Dependency Injection
        /// </summary>
        /// <param name="accountRenewalRepository"></param>
        /// <param name="termTranslation"></param>
        public DefaultAccountRenewal(IAccountRenewalRepositoryAdapter accountRenewalRepository, ITermResolver termTranslation)
        {
            _accountRenewalRepository = accountRenewalRepository ?? Create.New<IAccountRenewalRepositoryAdapter>();
            _termTranslation = termTranslation ?? Create.New<ITermResolver>();
        }

        #endregion

        #region Helper Methods

        private IRenewalAccountResult CreateNewRenewalAccountResult()
        {
            var result = Create.New<IRenewalAccountResult>();
            result.ErrorMessages = new List<string>();
            result.Success = false;

            return result;
        }

        private IRenewalAccountResult GetErrorMessages(IRenewalAccountResult renewalAccount)
        {
            var result = CreateNewRenewalAccountResult();

            string term = string.Empty;

            if (renewalAccount == null)
            {
                term = _termTranslation.Term("Renewal Account", "Account cannot be null.");
                result.ErrorMessages.Add(term);
            }

            if (renewalAccount != null && renewalAccount.AccountID == 0)
            {
                term = _termTranslation.Term("Renewal_Invalid_AccountID", "Invalid AccountID:");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, renewalAccount.AccountID));
            }

            if (renewalAccount != null && renewalAccount.AccountTypeID == 0)
            {
                term = _termTranslation.Term("Renewal_Invalid_AccountTypeID", "Invalid AccountTypeID:");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, renewalAccount.AccountTypeID));
            }

            return result;
        }

        private bool HasRenewalProduct(IRenewalAccount account)
        {
            bool result = false;

            IList<IProduct> renewalProducts = _accountRenewalRepository.LoadRenewalProducts();
            IList<IProduct> orderProducts = _accountRenewalRepository.LoadOrderProducts(account);

            var product = renewalProducts.Where(x => orderProducts.Any(o => o.ProductID == x.ProductID)).ToList();
            if (product != null && product.Count > 0)
            {
                result = true;
            }

            return result;
        }

        private IRenewalAccountResult SaveDistributorRenewalDate(IRenewalAccount account, IRenewalAccountResult renewalAccount)
        {
            var result = CreateNewRenewalAccountResult();

            int yearsToAdd = 1;

            if (renewalAccount.NextRenewalDateUTC == null)
            {
                account.RenewalDate = renewalAccount.EnrollmentDateUTC.HasValue
                    ? renewalAccount.EnrollmentDateUTC.Value.AddYears(yearsToAdd)
                    : DateTime.Today.AddYears(yearsToAdd);

                result = _accountRenewalRepository.SaveRenewalDate(account);
                result.ErrorMessages.AddRange(renewalAccount.ErrorMessages);
            }
            else if (HasRenewalProduct(account))
            {
                account.RenewalDate = DateTime.Today.AddYears(yearsToAdd);

                result = _accountRenewalRepository.SaveRenewalDate(account);
                result.ErrorMessages.AddRange(renewalAccount.ErrorMessages);
            }
            else
            {
                TranslateRenewalAccountResult(renewalAccount, result);
            }

            return result;
        }

        private IRenewalAccountResult SaveCustomerRenewalDate(IRenewalAccount account, IRenewalAccountResult renewalAccount)
        {
            var result = CreateNewRenewalAccountResult();

            DateTime maxDate = new DateTime(9999, 12, 31); // DateTime.Max differs in the ticks thereby forcing updates for each evaluation.

            if (renewalAccount.NextRenewalDateUTC.HasValue && renewalAccount.NextRenewalDateUTC.Value != maxDate)
            {
                account.RenewalDate = maxDate;
                result = _accountRenewalRepository.SaveRenewalDate(account);
                result.ErrorMessages.AddRange(renewalAccount.ErrorMessages);
            }
            else
            {
                result = TranslateRenewalAccountResult(renewalAccount, result);
            }

            return result;
        }

        private IRenewalAccountResult SaveRenewalDate(IRenewalAccount account, IRenewalAccountResult renewalAccount, IRenewalAccountTypes types)
        {
            if (renewalAccount.AccountTypeID == types.DistributorAccountTypeID)
            {
                return SaveDistributorRenewalDate(account, renewalAccount);
            }
            else if (renewalAccount.AccountTypeID == types.LoyalCustomerAccountTypeID || renewalAccount.AccountTypeID == types.RetailCustomerAccountTypeID)
            {
                return SaveCustomerRenewalDate(account, renewalAccount);
            }

            return renewalAccount;
        }

        private IRenewalAccountResult TranslateRenewalAccountResult(IRenewalAccountResult renewalAccount, IRenewalAccountResult result)
        {
            result.AccountID = renewalAccount.AccountID;
            result.AccountTypeID = renewalAccount.AccountTypeID;
            result.AccountStatusID = renewalAccount.AccountStatusID;
            result.ErrorMessages.AddRange(renewalAccount.ErrorMessages);
            result.LastRenewalDateUTC = renewalAccount.LastRenewalDateUTC;
            result.NextRenewalDateUTC = renewalAccount.NextRenewalDateUTC;
            result.OrderID = renewalAccount.OrderID;
            result.Success = renewalAccount.Success;

            return result;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load a renewal account.
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public IRenewalAccountResult LoadRenewalAccount(int accountID)
        {
            var account = Create.New<IRenewalAccount>();
            account.AccountID = accountID;

            return _accountRenewalRepository.LoadAccount(account);
        }
               
        /// <summary>
        /// load all accounts that are less than a particular date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public IList<IRenewalAccountResult> LoadRenewalDateAccounts(DateTime date)
        {
            return _accountRenewalRepository.LoadRenewalDateAccounts(date);
        }

        /// <summary>
        /// Update the Account Status for an account.
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="accountStatusID"></param>
        /// <returns></returns>
        public IRenewalAccountResult UpdateAccountStatus(int accountID, short accountStatusID)
        {
            return _accountRenewalRepository.UpdateAccountStatus(accountID, accountStatusID);
        }
        
        /// <summary>
        /// Update the Renewal Date for an account.
        /// </summary>
        /// <param name="renewalAccount"></param>
        /// <returns></returns>
        public IRenewalAccountResult UpdateRenewalDate(int accountID, int orderID)
        {
            var result = CreateNewRenewalAccountResult();

            var account = Create.New<IRenewalAccount>();
            account.AccountID = accountID;
            account.OrderID = orderID;

            IRenewalAccountResult renewalAccount = _accountRenewalRepository.LoadAccount(account);

            if (renewalAccount != null && renewalAccount.AccountID != 0 && renewalAccount.AccountTypeID != 0)
            {
                IRenewalAccountTypes accountTypes = _accountRenewalRepository.LoadAccountTypes();

                result = SaveRenewalDate(account, renewalAccount, accountTypes);
            }
            else
            {
                result = GetErrorMessages(renewalAccount);
            }

            return result;
        }

        #endregion

    }
}
