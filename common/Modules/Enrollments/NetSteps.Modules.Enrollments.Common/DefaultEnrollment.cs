using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;
using NetSteps.Content.Common;
using NetSteps.Modules.AvailabilityLookup.Common;
using NetSteps.Modules.Enrollments.Common.Models;
using NetSteps.Modules.Enrollments.Common.Results;
using NetSteps.Modules.Order.Common;
using NetSteps.Modules.Order.Common.Results;

namespace NetSteps.Modules.Enrollments.Common
{
    /// <summary>
    /// Default Implementation of IEnrollment
    /// </summary>
    [ContainerRegister(typeof(IEnrollment), RegistrationBehaviors.IfNoneOther, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class DefaultEnrollment : IEnrollment
    {

        #region Declarations

        private IEnrollmentAutoshipRepositoryAdapter autoshipRepository;
        private IEnrollmentRepositoryAdapter enrollmentRepository;
		private ISiteOrder orderModule;
        private IPaymentRepositoryAdapter paymentRepository;
        private IAvailabilityLookup pwsAvailabilityLookup;
        private ITermResolver termTranslation;

        #endregion

        #region Constructor(s)

		/// <summary>
		/// Default Constructor
		/// </summary>
		public DefaultEnrollment()
			: this(
				Create.New<IEnrollmentAutoshipRepositoryAdapter>(),
				Create.New<IEnrollmentRepositoryAdapter>(),
				Create.New<IPaymentRepositoryAdapter>(),
				Create.New<IAvailabilityLookup>(),
				Create.New<ISiteOrder>(),
				Create.New<ITermResolver>())
		{
		}

		/// <summary>
		/// Consturctor with Dependency Injection
		/// </summary>
		/// <param name="autoRepository">Autoship Repositroy</param>
		/// <param name="enrollRepository">Enrollment Repository</param>        
		/// <param name="payRepository">Payment Repository</param>
		/// <param name="lookup">Availability Lookup Module</param>
		/// <param name="siteOrder">Order Module</param>
		/// <param name="termTrans">Term Resolver</param>
		public DefaultEnrollment(
			IEnrollmentAutoshipRepositoryAdapter autoRepository,
			IEnrollmentRepositoryAdapter enrollRepository,
			IPaymentRepositoryAdapter payRepository,
			IAvailabilityLookup lookup,
			ISiteOrder siteOrder,
			ITermResolver termTrans)
		{
			autoshipRepository = autoRepository ?? Create.New<IEnrollmentAutoshipRepositoryAdapter>();
			enrollmentRepository = enrollRepository ?? Create.New<IEnrollmentRepositoryAdapter>();
			paymentRepository = payRepository ?? Create.New<IPaymentRepositoryAdapter>();
			pwsAvailabilityLookup = lookup ?? Create.New<IAvailabilityLookup>();
			orderModule = siteOrder ?? Create.New<ISiteOrder>();
			termTranslation = termTrans ?? Create.New<ITermResolver>();
		}

        #endregion

        #region Methods

        private IEnrollmentAddress CleanAddress(IEnrollmentAddress address)
        {
            address.AddressLine1 = address.AddressLine1 == null ? "" : address.AddressLine1.ToCleanString();
            address.AddressLine2 = address.AddressLine2 == null ? "" : address.AddressLine2.ToCleanString();
            address.AddressLine3 = address.AddressLine3 == null ? "" : address.AddressLine3.ToCleanString();
            address.Attention = address.Attention == null ? "" : address.Attention.ToCleanString();
            address.City = address.City == null ? "" : address.City.ToCleanString();
            address.County = address.County == null ? "" : address.County.ToCleanString();
            address.PostalCode = address.PostalCode == null ? "" : address.PostalCode.ToCleanString().Replace("-", "");
            address.State = address.State == null ? "" : address.State.ToCleanString();

            return address;
        }

        private IEnrollmentBillingProfile CleanBillingInfo(IEnrollmentBillingProfile profile)
        {
            if (profile != null)
            {
                profile.NameOnCard = profile.NameOnCard.ToCleanString();
                profile.ExpirationDate = profile.ExpirationDate.LastDayOfMonth();
                profile.CCNumber = profile.CCNumber.RemoveNonNumericCharacters();
                profile.CVV = profile.CVV.RemoveNonNumericCharacters().ToCleanString();
                profile.BillingAddress = CleanAddress(profile.BillingAddress);
            }

            return profile;
        }

		private IEnrollmentAccountResult ValidateAccountInfo(IEnrollingAccount account)
        {
            var result = Create.New<IEnrollmentAccountResult>();
            result.ErrorMessages = new List<string>();

            if (account.FirstName.IsNullOrEmpty() || account.LastName.IsNullOrEmpty())
            {
                result.ErrorMessages.Add(termTranslation.Term("NameIsRequired", "First and Last Name are Required"));
                result.Success = false;
            }

            if (!enrollmentRepository.ValidateEmailAddressAvailibility(account.Email))
            {
                result.ErrorMessages.Add(termTranslation.Term("EmailAccountAlreadyExists", "An account with this e-mail already exists."));
                result.Success = false;
            }

            if (!enrollmentRepository.IsTaxNumberAvailable(account.TaxNumber, account.AccountID))
            {
                result.ErrorMessages.Add(termTranslation.Term("TaxNumberInUse", string.Format("The Tax Number ({0}) is already in use by another account.", account.TaxNumber.ToCleanString())));
                result.Success = false;
            }

            var addressRes = ValidateAddress(account.MainAddress);
            if (!addressRes.Success)
            {
                result.ErrorMessages.AddRange(addressRes.ErrorMessages);
                result.Success = false;
            }

            addressRes = ValidateAddress(account.ShippingAddress);
            if (!addressRes.Success)
            {
                result.ErrorMessages.AddRange(addressRes.ErrorMessages);
                result.Success = false;
            }

            result.ErrorMessages.AddRange(ValidateBillingInfo(account.BillingProfile).ErrorMessages);

            return result;
        }

		private IEnrollmentOrderResult TranslateOrderCreateResult(IOrderCreateResult orderCreateResult)
		{
			var result = Create.New<IEnrollmentOrderResult>();
			result.ErrorMessages = new List<string>();
			result.ErrorMessages.AddRange(orderCreateResult.ErrorMessages);
			result.OrderID = orderCreateResult.OrderID;
			result.Success = orderCreateResult.Success;

			return result;
		}

        private IResult ValidateAddress(IEnrollmentAddress address)
        {
            var result = Create.New<IResult>();
            result.ErrorMessages = new List<string>();
            result.Success = true;

            if (address != null)
            {
                if (address.AddressLine1.IsNullOrEmpty())
                {
                    result.ErrorMessages.Add(termTranslation.Term("AddressLIne1IsRequired", "Address Line 1 is required"));
                    result.Success = false;
                }
                if (address.City.IsNullOrEmpty())
                {
                    result.ErrorMessages.Add(termTranslation.Term("CityIsRequired", "City is required"));
                    result.Success = false;
                }
                if (address.CountryID <= 0)
                {
                    result.ErrorMessages.Add(termTranslation.Term("CountryIDIsRequired", "CountryID is required"));
                    result.Success = false;
                }

                if (address.State.IsNullOrEmpty())
                {
                    result.ErrorMessages.Add(termTranslation.Term("StateIsRequired", "State is required"));
                    result.Success = false;
                }
                if (address.PostalCode.IsNullOrEmpty())
                {
                    result.ErrorMessages.Add(termTranslation.Term("PostalCodeIsRequired", "Postal Code is required"));
                    result.Success = false;
                }

            }

            return result;
        }

        private IResult ValidateBillingInfo(IEnrollmentBillingProfile profile)
        {
            var result = Create.New<IResult>();
            result.ErrorMessages = new List<string>();

            result.Success = true;

            if (profile.CCNumber.IsNullOrEmpty() || profile.CCNumber.Length > 16)
            {
                result.ErrorMessages.Add(termTranslation.Term("InvalidCCNumber", "Invalid Credit Card Number"));
                result.Success = false;
            }
            if (profile.NameOnCard.IsNullOrEmpty())
            {
                result.ErrorMessages.Add(termTranslation.Term("NameOnCardIsRequired", "Name on Card must not be blank."));
                result.Success = false;
            }
            if (profile.ExpirationDate < DateTime.Now)
            {
                result.ErrorMessages.Add(termTranslation.Term("InvalidExperationDate", "Expiration Date is invalid."));
                result.Success = false;
            }

            result.ErrorMessages.AddRange(ValidateAddress(profile.BillingAddress).ErrorMessages);

            return result;
        }

        private IResult ValidatePassword(string password)
        {
            var result = Create.New<IResult>();
            result.ErrorMessages = new List<string>();
            result.Success = true;

            if (string.IsNullOrWhiteSpace(password))
            {
                result.ErrorMessages.Add(termTranslation.Term("Password_Password_Required", "Please enter a password."));
                result.Success = false;
                return result;
            }
            if (password.Length < 6)
            {
                result.ErrorMessages.Add(termTranslation.Term("Password_Password_Character_Length", "Password must be 6 characters or longer."));
                result.Success = false;
                return result;
            }
            if (password == password.GetIncrementalNumberStringEquivalent())
            {
                result.ErrorMessages.Add(termTranslation.Term("Password_Invalid_Password", "Invalid password. Please choose a different password."));
                result.Success = false;
                return result;
            }
            return result;
        }

        /// <summary>
        /// Create a new Account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public virtual IEnrollmentAccountResult CreateAccount(IEnrollingAccount account)
        {
			account.MainAddress = CleanAddress(account.MainAddress);
			account.ShippingAddress = CleanAddress(account.ShippingAddress);
			account.BillingProfile = CleanBillingInfo(account.BillingProfile);

			var result = ValidateAccountInfo(account);

			if (!result.Success)
			{
				return result;
			}

            var existingAccount = enrollmentRepository.GetProspectAccountForUpgradeIfExists(account.Email, account.SponsorID);

            if (existingAccount != null && existingAccount.AccountID > 0)
            {
                account = existingAccount;
            }

            account.TaxNumber = !string.IsNullOrEmpty(account.TaxNumber) ? account.TaxNumber.Replace("-", string.Empty).ToCleanString() : string.Empty;

            var accountResult = enrollmentRepository.CreateAccount(account);

            if (!accountResult.Success)
            {
                result.ErrorMessages.AddRange(accountResult.ErrorMessages);
                result.Success = false;
            }
            else
            {
                result.AccountID = accountResult.AccountID;
                enrollmentRepository.CreateMailAccountForDistributors(account.AccountTypeID, account.AccountID);
                result.Success = true;
            }

            return result;
        }

        /// <summary>
        /// Create a new PWS site subscription
        /// </summary>
        /// <param name="subscriptionOrder"></param>
        /// <returns></returns>
        public IEnrollmentOrderResult CreateSiteSubscription(IEnrollmentSubscriptionOrder subscriptionOrder)
        {
            var result = Create.New<IEnrollmentOrderResult>();
            result.ErrorMessages = new List<string>();

            var lookupRes = pwsAvailabilityLookup.Lookup(subscriptionOrder.Url);

            if (!lookupRes.Success)
            {
                var autoshipRes = CreateAutoshipOrder(subscriptionOrder);

                if (autoshipRes.Success)
                {
                    result.OrderID = autoshipRes.AutoshipOrderID;

                    var createSiteRes = enrollmentRepository.CreateSite(
                        subscriptionOrder.AccountID,
                        autoshipRes.AutoshipOrderID,
                        subscriptionOrder.Url,
                        subscriptionOrder.MarketID);

                    if (createSiteRes.Success)
                    {
                        result.Success = true;
                        return result;
                    }

                    result.ErrorMessages.AddRange(createSiteRes.ErrorMessages);
                    return result;
                }

                result.ErrorMessages.AddRange(autoshipRes.ErrorMessages);
                return result;
            }

            result.ErrorMessages.Add(termTranslation.Term("Subscription_Site_Unavailable", "Site Unavailable"));
            return result;
        }

        /// <summary>
        /// Create a new enrollment order
        /// </summary>
        /// <param name="enrollmentOrder"></param>
        /// <returns></returns>
		public IEnrollmentOrderResult CreateEnrollmentOrder(IOrderCreate enrollmentOrder)
        {
            IOrderCreateResult orderCreateResult = orderModule.CreateOrder(enrollmentOrder);
            IEnrollmentOrderResult result = TranslateOrderCreateResult(orderCreateResult);           

            return result;
        }

        /// <summary>
        /// Create a new User
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="password"></param>
        /// <param name="languageID"></param>
        /// <returns></returns>
        public IEnrollingUserResult CreateUser(int accountID, string password, int languageID)
        {
            var result = Create.New<IEnrollingUserResult>();
            result.ErrorMessages = new List<string>();
            result.Success = true;

            var passwordRes = ValidatePassword(password);

            if (!passwordRes.Success)
            {
                result.Success = false;
                result.ErrorMessages.AddRange(passwordRes.ErrorMessages);
                return result;
            }

            result = enrollmentRepository.CreateUser(accountID, password, languageID);

            return result;
        }

        /// <summary>
        /// Create a new Account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public IEnrollmentResult EnrollAccount(IEnrollingAccount account)
        {
            var result = Create.New<IEnrollmentResult>();

            account.MainAddress = CleanAddress(account.MainAddress);
            account.ShippingAddress = CleanAddress(account.ShippingAddress);
            account.BillingProfile = CleanBillingInfo(account.BillingProfile);

            var createAccountRes = ValidateAccountInfo(account);
            if (createAccountRes.Success)
            {
                createAccountRes = CreateAccount(account);

                if (!createAccountRes.Success)
                {
                    result.ErrorMessages.AddRange(createAccountRes.ErrorMessages);
                    return result;
                }
            }
            else
            {
                result.ErrorMessages = createAccountRes.ErrorMessages;
                return result;
            }

            return result;
        }

        /// <summary>
        /// Submit Order Payments
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public virtual IPaymentResponseResult SubmitPayments(int orderID)
        {
            var result = Create.New<IPaymentResponseResult>();

            var response = this.paymentRepository.SubmitPayments(orderID);

            if (response != null)
            {
                result.FailureCount = response.FailureCount;
                result.Message = response.Message;
                result.Success = response.Success;
            }

            return result;
        }

        /// <summary>
        /// Validate an Autoship Order
        /// </summary>
        /// <param name="enrollmentOrder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
		private IEnrollmentAutoshipOrderResult ValidateAutoshipOrder(IOrderCreate enrollmentOrder, IEnrollmentAutoshipOrderResult result)
        {
            foreach (IProduct product in enrollmentOrder.Products)
            {
				if (product.ProductID == 0)
                {
                    result.Success = false;
                    result.ErrorMessages.Add(termTranslation.Term("CreateOrder_Missing_ProductID", "Missing ProductID."));
                }

                if (product.Quantity == 0)
                {
                    result.Success = false;
                    result.ErrorMessages.Add(termTranslation.Term("CreateOrder_Missing_Quantity", "Missing Quantity."));
                }
            }

            return result;
        }

        /// <summary>
        /// Create a new Autoship Order and Template.
        /// </summary>
        /// <param name="subscriptionOrder"></param>
        /// <returns></returns>
        public virtual IEnrollmentAutoshipOrderResult CreateAutoshipOrder(IEnrollmentSubscriptionOrder subscriptionOrder)
        {
            var result = Create.New<IEnrollmentAutoshipOrderResult>();
            result.ErrorMessages = new List<string>();
            result.Success = true;

			var autoshipValidation = ValidateAutoshipOrder(subscriptionOrder, result);

            if (autoshipValidation.Success)
            {
                IEnrollmentAutoshipOrderResult createAutoshipResult = autoshipRepository.CreateAutoshipScheduleAndOrder(subscriptionOrder);

                result.ErrorMessages.AddRange(createAutoshipResult.ErrorMessages);
                result.AutoshipOrderID = createAutoshipResult.AutoshipOrderID;
                result.TemplateOrderID = createAutoshipResult.TemplateOrderID;
                result.Success = createAutoshipResult.Success;
            }

            return result;
        }

        /// <summary>
        /// Create a new Autoship Order, Template, and Initial Order.
        /// </summary>
        /// <param name="subscriptionOrder"></param>
        /// <returns></returns>
        public virtual IEnrollmentAutoshipOrderResult CreateProductSubscriptionOrder(IEnrollmentSubscriptionOrder subscriptionOrder)
        {
            IEnrollmentAutoshipOrderResult result = CreateAutoshipOrder(subscriptionOrder);

            return result;
        }

		/// <summary>
		/// Activate an Account
		/// </summary>
		/// <param name="accountID"></param>
		public void ActivateAccount(int accountID)
		{
			enrollmentRepository.ActivateAccount(accountID);
		}

		#endregion
	}
}
