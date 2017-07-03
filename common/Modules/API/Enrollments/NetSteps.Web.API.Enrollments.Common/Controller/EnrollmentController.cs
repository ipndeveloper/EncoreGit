using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NetSteps.Content.Common;
using NetSteps.Diagnostics.Logging.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.Enrollments.Common;
using NetSteps.Modules.Enrollments.Common.Models;
using NetSteps.Modules.Enrollments.Common.Results;
using NetSteps.Web.API.Base.Common;
using NetSteps.Web.API.Enrollments.Common.Models;
using NetSteps.Encore.Core;
using NetSteps.Modules.Order.Common;

namespace NetSteps.Web.API.Enrollments.Common
{
    /// <summary>
    /// Controller for EnrollmentController
    /// </summary>
    public class EnrollmentController : BaseController
    {

        #region Declarations

        private IEnrollment enrollment;

        private ILogResolver logResolver;

        private ITermResolver termResolver;

        private readonly static ICopier<ProductViewModel, IProduct> _productCopier = Create.New<ICopier<ProductViewModel, IProduct>>();

        private readonly static ICopier<EnrollmentSubscriptionOrderViewModel, IEnrollmentSubscriptionOrder> _subscriptionCopier = Create.New<ICopier<EnrollmentSubscriptionOrderViewModel, IEnrollmentSubscriptionOrder>>();

		private readonly static ICopier<EnrollmentOrderViewModel, IOrderCreate> _orderCopier = Create.New<ICopier<EnrollmentOrderViewModel, IOrderCreate>>();

        private readonly static ICopier<EnrollmentBillingProfileViewModel, IEnrollmentBillingProfile> _billingCopier = Create.New<ICopier<EnrollmentBillingProfileViewModel, IEnrollmentBillingProfile>>();

        private readonly static ICopier<EnrollmentAddressViewModel, IEnrollmentAddress> _addressCopier = Create.New<ICopier<EnrollmentAddressViewModel, IEnrollmentAddress>>();

        private readonly static ICopier<EnrollmentAccountViewModel, IEnrollingAccount> _accountCopier = Create.New<ICopier<EnrollmentAccountViewModel, IEnrollingAccount>>();

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new instance
        /// </summary>
        public EnrollmentController()
            : this(Create.New<IEnrollment>(), Create.New<ILogResolver>(), Create.New<ITermResolver>())
        {
            Contract.Ensures(enrollment != null);
            Contract.Ensures(termResolver != null);
            Contract.Ensures(logResolver != null);
        }

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="enroll"></param>
        /// <param name="lResolver"></param>
        /// <param name="tResolver"></param>
        public EnrollmentController(IEnrollment enroll, ILogResolver lResolver, ITermResolver tResolver)
        {
            this.enrollment = enroll ?? Create.New<IEnrollment>();
            this.logResolver = lResolver ?? Create.New<ILogResolver>();
            this.termResolver = tResolver ?? Create.New<ITermResolver>();
        }

        #endregion

        #region Action Methods

        /// <summary>
        /// Create an account, a user, an enrollment order, 
        /// a product subscription (optional), a site subscription (optional)
        /// 
        /// /// eg. http://yourdomain.com/account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiAccessKeyFilter]
        public ActionResult EnrollAccount(EnrollmentViewModel model)
        {
            // Copy Model
            EnrollmentDTO dto = CopyModelToEnrollmentDTO(model);

            // Validate Model
            var result = ValidateEnrollmentViewModel(dto);

            if (result.Success)
            {
                // Create Account
                if (dto.CreateUser)
                {
                    var accountResults = CreateAccount(dto.Account);
                    if (!accountResults.Success)
                    {
                        result.Success = accountResults.Success;
                        result.ErrorMessages.AddRange(accountResults.ErrorMessages);
                    }
                    else
                    {
                        dto.AccountID = dto.Account.AccountID;
                        dto.Order.AccountID = dto.Account.AccountID;
                        dto.SiteSubscriptionOrder.AccountID = dto.Account.AccountID;
                        dto.ProductSubscriptionOrder.AccountID = dto.Account.AccountID;
                    }
                }

                // Create User
                if (dto.CreateUser && result.Success)
                {
                    var userResults = CreateUser(dto.AccountID, dto.LanguageID, dto.Password);
                    if (!userResults.Success)
                    {
                        result.Success = userResults.Success;
                        result.ErrorMessages.AddRange(userResults.ErrorMessages);
                    }
                }

                // Create enrollment order
                if (dto.CreateEnrollmentOrder && result.Success)
                {
                    var enrollmentOrderResults = CreateEnrollmentOrder(dto.Order);
                    if (!enrollmentOrderResults.Success)
                    {
                        result.Success = enrollmentOrderResults.Success;
                        result.ErrorMessages.AddRange(enrollmentOrderResults.ErrorMessages);
                    }
                }

                // Create product subscription
                if (dto.CreateProductSubscription && result.Success)
                {
                    var productSubscriptionResults = CreateProductSubscription(dto.ProductSubscriptionOrder);
                    if (!productSubscriptionResults.Success)
                    {
                        result.Success = productSubscriptionResults.Success;
                        result.ErrorMessages.AddRange(productSubscriptionResults.ErrorMessages);
                    }
                }

                // Create site subscription
                if (dto.CreateSiteSubscription && result.Success)
                {
                    var siteSubscriptionResults = CreateSiteSubscription(dto.SiteSubscriptionOrder);
                    if (!siteSubscriptionResults.Success)
                    {
                        result.Success = siteSubscriptionResults.Success;
                        result.ErrorMessages.AddRange(siteSubscriptionResults.ErrorMessages);
                    }
                }

				if (result.Success)
				{
					this.enrollment.ActivateAccount(dto.AccountID);
				}
            }

            var enrollmentResult = Create.New<IEnrollmentAccountResult>();
            enrollmentResult.ErrorMessages = new List<string>();
            enrollmentResult.ErrorMessages.AddRange(result.ErrorMessages);
            enrollmentResult.AccountID = dto.Account.AccountID;
            enrollmentResult.Success = result.Success;

            // Return messages
            if (result.Success)
            {
                return this.Result_200_OK(enrollmentResult);
            }
            else
            {
                return this.Result_400_BadRequest(enrollmentResult);
            }
        }

        #endregion

        #region Private Methods

        private EnrollmentDTO CopyModelToEnrollmentDTO(EnrollmentViewModel model)
        {
            var dto = Create.New<EnrollmentDTO>();

            try
            {
                dto.Password = model.User.Password;
                dto.AccountID = model.User.AccountID;
                dto.LanguageID = model.User.LanguageID;

                dto.Account = CopyAccountToDTO(model);
                dto.Order = CopyOrderToDTO(model.Order);
                dto.ProductSubscriptionOrder = CopySubscriptionOrderToDTO(model.ProductSubscriptionOrder);
                dto.SiteSubscriptionOrder = CopySubscriptionOrderToDTO(model.SiteSubscriptionOrder);

                dto.CreateUser = HasUser(model.User);
                dto.CreateAccount = HasAccount(dto.Account);
                dto.CreateEnrollmentOrder = HasEnrollmentOrder(dto.Order);
                dto.CreateSiteSubscription = HasSiteSubscription(dto.SiteSubscriptionOrder);
                dto.CreateProductSubscription = HasProductSubscription(dto.ProductSubscriptionOrder);
            }
            catch (Exception ex)
            {
                logResolver.LogException(ex, true);
            }

            return dto;
        }

        private IEnrollingAccount CopyAccountToDTO(EnrollmentViewModel model)
        {
            var dto = Create.New<IEnrollingAccount>();
            _accountCopier.CopyTo(dto, model.Account, CopyKind.Loose, Container.Current);
            dto.AccountTypeID = Convert.ToInt16(model.Account.AccountTypeID);
            dto.GenderID = Convert.ToInt16(model.Account.GenderID);

            dto.BillingProfile = CopyBillingProfileToDTO(model.BillingProfile);
            dto.BillingProfile.BillingAddress = CopyAddressToDTO(model.BillingAddress);
            dto.MainAddress = CopyAddressToDTO(model.MainAddress);
            dto.ShippingAddress = CopyAddressToDTO(model.ShippingAddress);

            return dto;
        }

        private IEnrollmentAddress CopyAddressToDTO(EnrollmentAddressViewModel address)
        {
            var dto = Create.New<IEnrollmentAddress>();
            _addressCopier.CopyTo(dto, address, CopyKind.Loose, Container.Current);

            return dto;
        }

        private IEnrollmentBillingProfile CopyBillingProfileToDTO(EnrollmentBillingProfileViewModel profile)
        {
            var dto = Create.New<IEnrollmentBillingProfile>();
            _billingCopier.CopyTo(dto, profile, CopyKind.Loose, Container.Current);

            return dto;
        }

        private IEnrollmentSubscriptionOrder CopySubscriptionOrderToDTO(EnrollmentSubscriptionOrderViewModel order)
        {
            var dto = Create.New<IEnrollmentSubscriptionOrder>();
            _subscriptionCopier.CopyTo(dto, order, CopyKind.Loose, Container.Current);
            dto.AccountTypeID = Convert.ToInt16(order.AccountTypeID);
            dto.OrderTypeID = Convert.ToInt16(order.OrderTypeID);
            dto.SiteID = order.SiteID != 0 ? (int?)order.SiteID : (int?)null;

            var products = new List<IProduct>();

            if (order.Products != null)
            {
                foreach (var product in order.Products)
                {
                    var productDTO = Create.New<IProduct>();
                    _productCopier.CopyTo(productDTO, product, CopyKind.Loose, Container.Current);

                    products.Add(productDTO);
                }
            }

            dto.Products = products;

            return dto;
        }

		private IOrderCreate CopyOrderToDTO(EnrollmentOrderViewModel order)
        {
			var dto = Create.New<IOrderCreate>();
            _orderCopier.CopyTo(dto, order, CopyKind.Loose, Container.Current);
            dto.AccountTypeID = Convert.ToInt16(order.AccountTypeID);
            dto.OrderTypeID = Convert.ToInt16(order.OrderTypeID);
            dto.SiteID = order.SiteID != 0 ? (int?)order.SiteID : (int?)null;

            var products = new List<IProduct>();

            if (order.Products != null)
            {
                foreach (var product in order.Products)
                {
                    var productDTO = Create.New<IProduct>();
                    _productCopier.CopyTo(productDTO, product, CopyKind.Loose, Container.Current);

                    products.Add(productDTO);
                }
            }

            dto.Products = products;

            return dto;
        }

        private Modules.Enrollments.Common.IResult CreateAccount(IEnrollingAccount dto)
        {
            var result = CreateNewResult();

            try
            {
                IEnrollmentAccountResult enrollmentResult = this.enrollment.CreateAccount(dto);
                dto.AccountID = enrollmentResult.AccountID;

                result.Success = enrollmentResult.Success;
                result.ErrorMessages.AddRange(enrollmentResult.ErrorMessages);
            }
            catch (Exception ex)
            {
                logResolver.LogException(ex, true);

                result.Success = false;
                result.ErrorMessages.Add(ex.Message);
            }

            return result;
        }

		private Modules.Enrollments.Common.IResult CreateEnrollmentOrder(IOrderCreate dto)
        {
            var result = CreateNewResult();

            try
            {
                IEnrollmentOrderResult enrollmentResult = this.enrollment.CreateEnrollmentOrder(dto);

                result.Success = enrollmentResult.Success;
                result.ErrorMessages.AddRange(enrollmentResult.ErrorMessages);
            }
            catch (Exception ex)
            {
                logResolver.LogException(ex, true);

                result.Success = false;
                result.ErrorMessages.Add(ex.Message);
            }

            return result;
        }

		private Modules.Enrollments.Common.IResult CreateNewResult()
        {
			var result = Create.New<Modules.Enrollments.Common.IResult>();
            result.ErrorMessages = new List<string>();
            result.Success = true;

            return result;
        }

		private Modules.Enrollments.Common.IResult CreateProductSubscription(IEnrollmentSubscriptionOrder dto)
        {
            var result = CreateNewResult();

            try
            {
                IEnrollmentAutoshipOrderResult enrollmentResult = this.enrollment.CreateProductSubscriptionOrder(dto);

                result.Success = enrollmentResult.Success;
                result.ErrorMessages.AddRange(enrollmentResult.ErrorMessages);
            }
            catch (Exception ex)
            {
                logResolver.LogException(ex, true);

                result.Success = false;
                result.ErrorMessages.Add(ex.Message);
            }

            return result;
        }

		private Modules.Enrollments.Common.IResult CreateSiteSubscription(IEnrollmentSubscriptionOrder dto)
        {
            var result = CreateNewResult();

            try
            {
                IEnrollmentOrderResult enrollmentResult = this.enrollment.CreateSiteSubscription(dto);

                result.Success = enrollmentResult.Success;
                result.ErrorMessages.AddRange(enrollmentResult.ErrorMessages);
            }
            catch (Exception ex)
            {
                logResolver.LogException(ex, true);

                result.Success = false;
                result.ErrorMessages.Add(ex.Message);
            }

            return result;
        }

		private Modules.Enrollments.Common.IResult CreateUser(int accountID, int languageID, string password)
        {
            var result = CreateNewResult();

            try
            {
                IEnrollingUserResult enrollmentResult = this.enrollment.CreateUser(accountID, password, languageID);

                result.Success = enrollmentResult.Success;
                result.ErrorMessages.AddRange(enrollmentResult.ErrorMessages);
            }
            catch (Exception ex)
            {
                logResolver.LogException(ex, true);

                result.Success = false;
                result.ErrorMessages.Add(ex.Message);
            }

            return result;
        }

        #endregion

        #region Validation Methods

        private bool HasAccount(IEnrollingAccount dto)
        {
            // Add business rules
            return true;
        }

		private bool HasEnrollmentOrder(IOrderCreate dto)
        {
            bool result = false;

            // Check for any values so validation can occur.
            if ((dto.OrderTypeID != 0) || (dto.CurrencyID != 0) || (dto.Products.Count > 0))
            {
                result = true;
            }

            return result;
        }

        private bool HasProductSubscription(IEnrollmentSubscriptionOrder dto)
        {
            bool result = false;

            // Check for any values so validation can occur.
            if ((dto.AutoshipScheduleID != 0) || (dto.MarketID != 0) || (dto.OrderTypeID != 0) || (dto.CurrencyID != 0) || (dto.Products.Count > 0))
            {
                result = true;
            }

            return result;
        }

        private bool HasSiteSubscription(IEnrollmentSubscriptionOrder dto)
        {
            bool result = false;

            // Check for any values so validation can occur.
            if ((dto.AutoshipScheduleID != 0) || (dto.MarketID != 0) || (dto.OrderTypeID != 0) || (dto.CurrencyID != 0) || (dto.Products.Count > 0) || (!string.IsNullOrEmpty(dto.Url)))
            {
                result = true;
            }

            return result;
        }

        private bool HasUser(EnrollmentUserViewModel model)
        {
            // Add business rules
            return true;
        }

		private Modules.Enrollments.Common.IResult ValidateEnrollmentAccount(IEnrollingAccount dto)
        {
            var result = CreateNewResult();

            string term = string.Empty;

            if (dto.AccountTypeID == 0)
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_Invalid_AccountType", "Invalid AccountType:");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.AccountTypeID));
            }

            if (string.IsNullOrEmpty(dto.Email))
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_Invalid_Email", "Invalid Email:");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.Email));
            }

            if (string.IsNullOrEmpty(dto.FirstName))
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_Invalid_FirstName", "Invalid FirstName:");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.FirstName));
            }

            if (dto.LanguageID == 0)
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_Invalid_LanguageID", "Invalid LangaugeID:");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.LanguageID));
            }

            if (string.IsNullOrEmpty(dto.LastName))
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_Invalid_LastName", "Invalid LastName:");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.LastName));
            }

            if (dto.Placement == 0)
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_Invalid_Placement", "Invalid Placement:");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.Placement));
            }

            if (dto.SponsorID == 0)
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_Invalid_SponsorID", "Invalid SponsorID:");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.SponsorID));
            }

            return result;
        }

		private Modules.Enrollments.Common.IResult ValidateEnrollmentAddress(IEnrollmentAddress dto, string addressType)
        {
            var result = CreateNewResult();

            string term = string.Empty;

            if (string.IsNullOrEmpty(dto.AddressLine1))
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_Invalid_Address", "Invalid Address:");
                result.ErrorMessages.Add(string.Format("{0} {1} - {2}", term, addressType, dto.AddressLine1));
            }

            if (string.IsNullOrEmpty(dto.City))
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_Invalid_City", "Invalid City:");
                result.ErrorMessages.Add(string.Format("{0} {1} - {2}", term, addressType, dto.City));
            }

            if (dto.CountryID == 0)
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_Invalid_CountryID", "Invalid CountryID:");
                result.ErrorMessages.Add(string.Format("{0} {1} - {2}", term, addressType, dto.CountryID));
            }

            if (string.IsNullOrEmpty(dto.PostalCode))
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_Invalid_PostalCode", "Invalid PostalCode:");
                result.ErrorMessages.Add(string.Format("{0} {1} - {2}", term, addressType, dto.PostalCode));
            }

            if (string.IsNullOrEmpty(dto.State))
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_Invalid_State", "Invalid State:");
                result.ErrorMessages.Add(string.Format("{0} {1} - {2}", term, addressType, dto.State));
            }

            return result;
        }

		private Modules.Enrollments.Common.IResult ValidateEnrollmentBillingProfile(IEnrollmentBillingProfile dto)
        {
            var result = CreateNewResult();

            string term = string.Empty;

            if (string.IsNullOrEmpty(dto.CCNumber))
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_Invalid_CCNumber", "Invalid CCNumber");
                result.ErrorMessages.Add(term);
            }

            if (string.IsNullOrEmpty(dto.NameOnCard))
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_Invalid_NameOnCard", "Invalid Name On Card:");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.NameOnCard));
            }

            if (dto.ExpirationDate == DateTime.MinValue)
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_ExpirationDate_Required", "ExpirationDate is required.");
                result.ErrorMessages.Add(term);
            }

            if (dto.ExpirationDate <= DateTime.Today)
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_ExpirationDate_Expired", "ExpirationDate has expired.");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.ExpirationDate));
            }

            if (dto.PaymentTypeID == 0)
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_Invalid_PaymentTypeID", "Invalid PaymentTypeID:");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.PaymentTypeID));
            }

            return result;
        }

		private Modules.Enrollments.Common.IResult ValidateEnrollmentOrder(IOrderCreate dto, bool hasOrder)
        {
            var result = CreateNewResult();

            string term = string.Empty;

            if (hasOrder)
            {
                if (dto.CurrencyID == 0)
                {
                    result.Success = false;
                    term = termResolver.Term("Enrollment_Invalid_CurrencyID", "Invalid CurrencyID:");
                    result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.CurrencyID));
                }

                if (dto.OrderTypeID == 0)
                {
                    result.Success = false;
                    term = termResolver.Term("Enrollment_Invalid_OrderTypeID", "Invalid OrderTypeID:");
                    result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.CurrencyID));
                }

                if (dto.Products.Count == 0)
                {
                    result.Success = false;
                    term = termResolver.Term("Enrollment_No_Products", "There are no products on this order.");
                    result.ErrorMessages.Add(term);
                }
                else
                {
                    result = ValidateOrderProducts(dto.Products, result);
                }

            }

            return result;
        }

		private Modules.Enrollments.Common.IResult ValidateOrderProducts(IList<IProduct> products, Modules.Enrollments.Common.IResult result)
        {
            string term = string.Empty;

            foreach (IProduct product in products)
            {
                if (product.ProductID <= 0)
                {
                    result.Success = false;
                    term = termResolver.Term("Enrollment_Invalid_ProductID", "Invalid ProductID:");
                    result.ErrorMessages.Add(string.Format("{0} {1}", term, product.ProductID));
                }

                if (product.Quantity <= 0)
                {
                    result.Success = false;
                    term = termResolver.Term("Enrollment_Invalid_Quantity", "Invalid Quantity:");
                    result.ErrorMessages.Add(string.Format("{0} {1}", term, product.Quantity));
                }
            }

            return result;
        }

		private Modules.Enrollments.Common.IResult ValidateProductSubscription(IEnrollmentSubscriptionOrder dto, bool hasOrder)
        {
            var result = CreateNewResult();

            string term = string.Empty;

            if (hasOrder)
            {
                if (dto.CurrencyID == 0)
                {
                    result.Success = false;
                    term = termResolver.Term("Enrollment_Invalid_CurrencyID", "Invalid CurrencyID:");
                    result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.CurrencyID));
                }

                if (dto.OrderTypeID == 0)
                {
                    result.Success = false;
                    term = termResolver.Term("Enrollment_Invalid_OrderTypeID", "Invalid OrderTypeID:");
                    result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.CurrencyID));
                }

                if (dto.AutoshipScheduleID == 0)
                {
                    result.Success = false;
                    term = termResolver.Term("Enrollment_Invalid_AutoshipScheduleID", "Invalid AutoshipScheduleID:");
                    result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.AutoshipScheduleID));
                }

                if (dto.Products.Count == 0)
                {
                    result.Success = false;
                    term = termResolver.Term("Enrollment_No_Products", "There are no products on this order.");
                    result.ErrorMessages.Add(term);
                }
                else
                {
                    result = ValidateOrderProducts(dto.Products, result);
                }
            }

            return result;
        }

		private Modules.Enrollments.Common.IResult ValidateSiteSubscription(IEnrollmentSubscriptionOrder dto, bool hasOrder)
        {
            var result = CreateNewResult();

            string term = string.Empty;

            if (hasOrder)
            {
                if (dto.CurrencyID == 0)
                {
                    result.Success = false;
                    term = termResolver.Term("Enrollment_Invalid_CurrencyID", "Invalid CurrencyID:");
                    result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.CurrencyID));
                }

                if (dto.OrderTypeID == 0)
                {
                    result.Success = false;
                    term = termResolver.Term("Enrollment_Invalid_OrderTypeID", "Invalid OrderTypeID:");
                    result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.CurrencyID));
                }

                if (dto.AutoshipScheduleID == 0)
                {
                    result.Success = false;
                    term = termResolver.Term("Enrollment_Invalid_AutoshipScheduleID", "Invalid AutoshipScheduleID:");
                    result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.AutoshipScheduleID));
                }

                if (string.IsNullOrEmpty(dto.Url))
                {
                    result.Success = false;
                    term = termResolver.Term("Enrollment_Invalid_Url", "Invalid Url:");
                    result.ErrorMessages.Add(string.Format("{0} {1}", term, dto.Url));
                }
            }

            return result;
        }

		private Modules.Enrollments.Common.IResult ValidateEnrollmentUser(int languageID, string password)
        {
            var result = CreateNewResult();

            string term = string.Empty;

            if (languageID == 0)
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_Invalid_LanguageID", "Invalid LangaugeID:");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, languageID));
            }

            if (string.IsNullOrEmpty(password))
            {
                result.Success = false;
                term = termResolver.Term("Enrollment_Invalid_Password", "Invalid Password");
                result.ErrorMessages.Add(term);
            }

            return result;
        }

		private Modules.Enrollments.Common.IResult ValidateEnrollmentViewModel(EnrollmentDTO dto)
        {
            var result = CreateNewResult();

            try
            {
                var accountResults = ValidateEnrollmentAccount(dto.Account);
                var mainAddressResults = ValidateEnrollmentAddress(dto.Account.MainAddress, "Main");
                var billAddressResults = ValidateEnrollmentAddress(dto.Account.BillingProfile.BillingAddress, "Billing");
                var shipAddressResults = ValidateEnrollmentAddress(dto.Account.ShippingAddress, "Shipping");
                var billProfileResults = ValidateEnrollmentBillingProfile(dto.Account.BillingProfile);
                var userResults = ValidateEnrollmentUser(dto.LanguageID, dto.Password);
                var enrollmentOrderResults = ValidateEnrollmentOrder(dto.Order, dto.CreateEnrollmentOrder);
                var productSubscriptionResults = ValidateProductSubscription(dto.ProductSubscriptionOrder, dto.CreateProductSubscription);
                var siteSubscriptionResults = ValidateSiteSubscription(dto.SiteSubscriptionOrder, dto.CreateSiteSubscription);

                // Create Account Validation Results
                if (!accountResults.Success || !mainAddressResults.Success || !shipAddressResults.Success || !billProfileResults.Success)
                {
                    result.Success = false;
                    result.ErrorMessages.AddRange(accountResults.ErrorMessages);
                    result.ErrorMessages.AddRange(mainAddressResults.ErrorMessages);
                    result.ErrorMessages.AddRange(shipAddressResults.ErrorMessages);
                    result.ErrorMessages.AddRange(billProfileResults.ErrorMessages);
                }

                // Create User Validation Results
                if (!userResults.Success)
                {
                    result.Success = false;
                    result.ErrorMessages.AddRange(userResults.ErrorMessages);
                }

                // Optional
                // Create Enrollment Order
                if (!enrollmentOrderResults.Success)
                {
                    result.Success = false;
                    result.ErrorMessages.AddRange(enrollmentOrderResults.ErrorMessages);
                }

                // Optional
                // Product Subscription Order
                if (!productSubscriptionResults.Success)
                {
                    result.Success = false;
                    result.ErrorMessages.AddRange(productSubscriptionResults.ErrorMessages);
                }

                // Optional
                // Site Subscription Order
                if (!siteSubscriptionResults.Success)
                {
                    result.Success = false;
                    result.ErrorMessages.AddRange(siteSubscriptionResults.ErrorMessages);
                }
            }
            catch (Exception ex)
            {
                logResolver.LogException(ex, true);

                result.Success = false;
                result.ErrorMessages.Add(ex.Message);
            }

            return result;
        }

        #endregion

    }
}
