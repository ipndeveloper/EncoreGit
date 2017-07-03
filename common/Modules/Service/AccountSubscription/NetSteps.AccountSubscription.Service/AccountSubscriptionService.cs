using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Content.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.AccountSubscription.Common;

namespace NetSteps.AccountSubscription.Service
{

    /// <summary>
    /// Default Implementation of IAccountSubscriptionService
    /// </summary>
    [ContainerRegister(typeof(IAccountSubscriptionService), RegistrationBehaviors.IfNoneOther, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class AccountSubscriptionService : IAccountSubscriptionService
    {

        #region Declarations

        private IAccountSubscriptionRepositoryAdapter _accountSubscriptionRepository;

        private ITermResolver _termTranslation;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AccountSubscriptionService() : this(Create.New<IAccountSubscriptionRepositoryAdapter>(), Create.New<ITermResolver>()) { }

        /// <summary>
        /// Constructor with Dependency Injection
        /// </summary>
        /// <param name="accountRenewalRepository"></param>
        /// <param name="termTranslation"></param>
        public AccountSubscriptionService(IAccountSubscriptionRepositoryAdapter accountSubscriptionRepository, ITermResolver termTranslation)
        {
            _accountSubscriptionRepository = accountSubscriptionRepository ?? Create.New<IAccountSubscriptionRepositoryAdapter>();
            _termTranslation = termTranslation ?? Create.New<ITermResolver>();
        }

        #endregion

        #region Helper Methods

        private IAccountSubscriptionResult CreateNewAccountSubscriptionResult()
        {
            var result = Create.New<IAccountSubscriptionResult>();
            result.ErrorMessages = new List<string>();
            result.Success = false;

            return result;
        }

        private IAccountSubscriptionResult GetErrorMessages(IAccountSubscriptionResult subscriptionAccount)
        {
            var result = CreateNewAccountSubscriptionResult();

            string term = string.Empty;

            if (subscriptionAccount == null)
            {
                term = _termTranslation.Term("Renewal Account", "Account cannot be null.");
                result.ErrorMessages.Add(term);
            }

            if (subscriptionAccount != null && subscriptionAccount.AccountID == 0)
            {
                term = _termTranslation.Term("Renewal_Invalid_AccountID", "Invalid AccountID:");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, subscriptionAccount.AccountID));
            }

            if (subscriptionAccount != null && subscriptionAccount.AccountTypeID == 0)
            {
                term = _termTranslation.Term("Renewal_Invalid_AccountTypeID", "Invalid AccountTypeID:");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, subscriptionAccount.AccountTypeID));
            }

            return result;
        }

        private bool HasRenewalProduct(IAccountSubscription account, ref int yearsToAdd)
        {
            bool result = false;

            IList<IProduct> renewalProducts = _accountSubscriptionRepository.LoadAccountSubscriptionProducts(1);
            IList<IProduct> orderProducts = _accountSubscriptionRepository.LoadOrderProducts(account);

            var product = renewalProducts.FirstOrDefault(x => orderProducts.Any(o => o.ProductID == x.ProductID));
            if (product != null)
            {
                result = true;
                yearsToAdd = product.IntervalCount;
            }

            return result;
        }       

        private IAccountSubscriptionResult SaveDistributorRenewalDate(IAccountSubscription account, IAccountSubscriptionResult accountSubscription)
        {
            int yearsToAdd = 1;
            var result = CreateNewAccountSubscriptionResult();

            IAccountSubscriptionValues values = _accountSubscriptionRepository.LoadSubscriptionValues();           

            // Save new account subscription
            if (accountSubscription.RenewalEndDate == null)
            {
                account.RenewalStartDate = accountSubscription.EnrollmentDateUTC.HasValue
                    ? accountSubscription.EnrollmentDateUTC.Value 
                    : DateTime.Today;

                account.RenewalEndDate = accountSubscription.EnrollmentDateUTC.HasValue
                    ? accountSubscription.EnrollmentDateUTC.Value.AddYears(yearsToAdd)
                    : DateTime.Today.AddYears(yearsToAdd);

                result = _accountSubscriptionRepository.SaveAccountSubscription(values.SubscriptionKindID ,account);
                result.ErrorMessages.AddRange(accountSubscription.ErrorMessages);
                result.Success = accountSubscription.Success;                
            }
            // Update the renewal date if a particular product is purchased
            else if (HasRenewalProduct(account, ref yearsToAdd))
            {
                account.RenewalEndDate = accountSubscription.RenewalEndDate.HasValue
                    ? accountSubscription.RenewalEndDate.Value.AddYears(yearsToAdd)
                    : DateTime.Today.AddYears(yearsToAdd);

                result = _accountSubscriptionRepository.UpdateAccountSubscription(account);
                result.ErrorMessages.AddRange(accountSubscription.ErrorMessages);
                result.Success = accountSubscription.Success;                
            }
            // Update the renewal date if manually changed
            else if (account.RenewalEndDate.HasValue && accountSubscription.RenewalEndDate.HasValue && account.RenewalEndDate.Value != accountSubscription.RenewalEndDate.Value)
            {
                result = _accountSubscriptionRepository.UpdateAccountSubscription(account);
                result.ErrorMessages.AddRange(accountSubscription.ErrorMessages);
                result.Success = accountSubscription.Success;                
            }
            else
            {
                TranslateSubscriptionAccountResult(accountSubscription, result);
            }

            return result;
        }

        private IAccountSubscriptionResult SaveCustomerRenewalDate(IAccountSubscription account, IAccountSubscriptionResult accountSubscription)
        {
            var result = CreateNewAccountSubscriptionResult();

            DateTime maxDate = new DateTime(9999, 12, 31); // DateTime.Max differs in the ticks thereby forcing updates for each evaluation.

            if (accountSubscription.RenewalEndDate.HasValue && accountSubscription.RenewalEndDate.Value != maxDate)
            {
                account.AccountID = accountSubscription.AccountID;
                account.OrderID = accountSubscription.OrderID;
                account.RenewalStartDate = DateTime.Today;
                account.RenewalEndDate = maxDate;                
                
                result = _accountSubscriptionRepository.UpdateAccountSubscription(account);                
            }            

            return result;
        }

        private IAccountSubscriptionResult SaveAccountSubscriptionRenewalDate(IAccountSubscription account)
        {
            var result = CreateNewAccountSubscriptionResult();

            IAccountTypes accountTypes = _accountSubscriptionRepository.LoadAccountTypes();
            IAccountSubscriptionResult accountSubscription = LoadAccountSubscription(account.AccountID);

            if (accountSubscription.AccountTypeID == accountTypes.DistributorAccountTypeID)
            {
                result = SaveDistributorRenewalDate(account, accountSubscription);
            }
            //else if (accountSubscription.AccountTypeID == accountTypes.LoyalCustomerAccountTypeID || accountSubscription.AccountTypeID == accountTypes.RetailCustomerAccountTypeID)
            //{
            //    result = SaveCustomerRenewalDate(account, accountSubscription);
            //}
            else
            {
                result = TranslateSubscriptionAccountResult(accountSubscription, result);
            }
            
            return result;
        }

        private IAccountSubscriptionResult TranslateSubscriptionAccountResult(IAccountSubscriptionResult subscriptionAccount, IAccountSubscriptionResult result)
        {
            result.AccountID = subscriptionAccount.AccountID;
            result.AccountTypeID = subscriptionAccount.AccountTypeID;
            result.AccountStatusID = subscriptionAccount.AccountStatusID;
            result.ErrorMessages.AddRange(subscriptionAccount.ErrorMessages);
            result.RenewalEndDate = subscriptionAccount.RenewalEndDate;           
            result.OrderID = subscriptionAccount.OrderID;
            result.Success = subscriptionAccount.Success;

            return result;
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// Load a renewal account.
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public IAccountSubscriptionResult LoadAccountSubscription(int accountID)
        {
            var account = Create.New<IAccountSubscription>();
            account.AccountID = accountID;

            return _accountSubscriptionRepository.LoadAccountSubscription(account);
        }
               
        /// <summary>
        /// load all accounts that are less than a particular date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public IList<IAccountSubscriptionResult> LoadAccountSubscriptionsByDate(DateTime date)
        {
            return _accountSubscriptionRepository.LoadAccountSubscriptionsByDate(date);
        }

        public IList<IAccountSubscriptionResult> LoadAccountSubscriptionsBeforeOrOnDate(DateTime date)
        {
            return _accountSubscriptionRepository.LoadAccountSubscriptionsBeforeOrOnDate(date);
        }

        /// <summary>
        /// Update the Account Status for an account.
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="accountStatusID"></param>
        /// <returns></returns>
        public IAccountSubscriptionResult UpdateAccountStatus(int accountID, short accountStatusID)
        {
            return _accountSubscriptionRepository.UpdateAccountStatus(accountID, accountStatusID);
        }
        
        /// <summary>
        /// Update the Renewal Date for an account.
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public IAccountSubscriptionResult UpdateAccountSubscriptionRenewalDate(int accountID, int orderID)
        {
            var result = CreateNewAccountSubscriptionResult();

            var account = Create.New<IAccountSubscription>();
            account.AccountID = accountID;
            account.OrderID = orderID;

            result = SaveAccountSubscriptionRenewalDate(account);            

            return result;
        }

        /// <summary>
        /// Update the Renewal Date for an account.
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="renewalDate"></param>
        /// <returns></returns>
        public IAccountSubscriptionResult UpdateAccountSubscriptionRenewalDate(int accountID, DateTime renewalDate)
        {
            var result = CreateNewAccountSubscriptionResult();

            var account = Create.New<IAccountSubscription>();
            account.AccountID = accountID;
            account.RenewalEndDate = renewalDate;

            result = SaveAccountSubscriptionRenewalDate(account);

            return result;
        }

        #endregion

    }
}
