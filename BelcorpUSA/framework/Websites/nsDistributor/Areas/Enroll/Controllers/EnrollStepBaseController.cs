using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Enrollment.Common.Provider;
using NetSteps.Orders.Common.Models;
using NetSteps.Payments.Common.Models;
using NetSteps.Sites.Common.Models;
using NetSteps.Web.Mvc.Controls.Attributes;
using nsDistributor.Areas.Enroll.Models.Shared;
using nsDistributor.Areas.Enroll.Models.Products;
using NetSteps.Web.Mvc.Controls.Models.Enrollment;

using NetSteps.Common.Comparer;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Common;
using NetSteps.Data.Entities.Business.Logic;

namespace nsDistributor.Areas.Enroll.Controllers
{
    [EnrollmentStep("~/Enroll")]
    public abstract class EnrollStepBaseController : EnrollBaseController
    {
        #region Context
        protected IEnrollmentContext<EnrollmentKitConfig> _enrollmentContext { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Make sure we have session
            if (filterContext.HttpContext == null
                || filterContext.HttpContext.Session == null)
            {
                return;
            }

            // Load _enrollmentContext from session
            _enrollmentContext = Create.New<IEnrollmentContextProvider>().GetEnrollmentContext() as IEnrollmentContext<EnrollmentKitConfig>;

            // Check for completed enrollment
            if (_enrollmentContext.EnrollmentComplete
                && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName != "Receipt")
            {
                _enrollmentContext.Clear();
                filterContext.Result = RedirectToAction("Index", "Landing");
            }

            // Check for ReturnUrl and place it in context
            if (!string.IsNullOrWhiteSpace(Request["ReturnUrl"]))
            {
                _enrollmentContext.ReturnUrl = Request["ReturnUrl"];
            }
        }
        #endregion

        #region Master Helpers
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                if (filterContext.Result is ViewResult
                    && _enrollmentContext != null)
                {
                    LoadViewData(filterContext);
                }

                base.OnActionExecuted(filterContext);
            }
            catch (Exception excp)
            {
                excp.Log();
                throw;
            }
        }
        public virtual void SetLanguageAccount(int languageID)
        {

            _enrollmentContext.EnrollingAccount.DefaultLanguageID = languageID;
            _enrollmentContext.LanguageID = languageID;
        }

        protected virtual void LoadViewData(ActionExecutedContext filterContext)
        {
            LoadTitle(filterContext);
            LoadHeaderTotals(filterContext);
            LoadHeaderSteps(filterContext);
            LoadStepCounter(filterContext);
        }

        protected virtual void LoadTitle(ActionExecutedContext filterContext)
        {
            ViewBag.Title = GetLocalizedStepName(_enrollmentContext.EnrollmentConfig.Steps.CurrentItem);
        }

        protected virtual void LoadHeaderTotals(ActionExecutedContext filterContext)
        {
            // Disable header totals on receipt page
            if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Receipt")
            {
                return;
            }

            var headerTotals = HeaderTotals_GetModel();

            // Hide totals if zero to avoid confusion when the enrollee had items in their shopping cart.
            if (headerTotals.Count > 0)
            {
                ViewBag.HeaderTotals = headerTotals;
            }
        }

        protected virtual void LoadHeaderSteps(ActionExecutedContext filterContext)
        {
            ViewBag.HeaderSteps = new HeaderStepsModel
            {
                HeaderStepItems = from stepConfig in _enrollmentContext.EnrollmentConfig.Steps
                                  select new HeaderStepsModel.HeaderStepItemModel
                                  {
                                      Text = GetLocalizedStepName(stepConfig),
                                      Url = GetStepUrl(stepConfig),
                                      IsCurrentStep = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName
                                        .Equals(stepConfig.Controller, StringComparison.OrdinalIgnoreCase),
                                      EnableHyperlink = false
                                  }
            };
        }

        protected virtual void LoadStepCounter(ActionExecutedContext filterContext)
        {
            ViewBag.StepCounter = Translation.GetTerm(string.Format("Enrollment_StepCounter_{0}", _enrollmentContext.StepCounter), _enrollmentContext.StepCounter.ToString());
        }

        protected virtual HeaderTotalsModel HeaderTotals_GetModel()
        {
            var initialOrder = _enrollmentContext.InitialOrder;
            int itemCount = 0;
            decimal subtotal = 0;

            if (initialOrder != null && initialOrder.OrderCustomers.Any())
            {
                itemCount += initialOrder.OrderCustomers.First().OrderItems.Sum(x => x.Quantity);
                subtotal += initialOrder.Subtotal ?? 0;
            }

            var model = new HeaderTotalsModel()
                .LoadResources(itemCount, subtotal, CoreContext.CurrentCultureInfo);

            return model;
        }
        #endregion

        #region Account Helpers

        private void CancelOrder(string orderNumber, int AccountTypeID)
        {
            try
            {
                string message = null;
                bool result = false;
                Order order = Order.LoadByOrderNumberFull(orderNumber);
                int OrderStatusID = order.OrderStatusID;
                int ppt = order.OrderCustomers[0].ProductPriceTypeID;
                int RowDispatchControlDel = ShippingCalculatorExtensions.DispatchItemControlsDel(order.OrderCustomers[0].AccountID, order.OrderID);
                int AccountID = order.OrderCustomers[0].AccountID;
                var data = OrderExtensions.GetWarehousePreOrderFromOrderByAccountID(AccountID).First();

                foreach (var item in ShippingCalculatorExtensions.GetProductQuantity(orderNumber))
                {
                    Order.GenerateAllocation(item.ProductID,
                                             item.Quantity,
                                             order.OrderID,
                                             data.Key,
                                             EntitiesEnums.MaintenanceMode.Delete,
                                             data.Value,
                                             AccountTypeID, false);
                }
                if (order != null) result = OrderService.TryCancel(order, out message);

                if (OrderContext.Order != null && result)
                {
                    if (OrderContext.Order.OrderID == order.OrderID) OrderContext.Clear();
                }
                OrderExtensions.TerminateOrder(AccountID);
            }
            catch (Exception ex)
            {
                int orderId;
                if (!Int32.TryParse(orderNumber, out orderId)) orderId = 0;
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: orderId);
            }
        }

        public virtual Account GetEnrollingAccount(bool crearCuenta = false)
        {
            Account account = null;
            if (crearCuenta)
            {
                if (_enrollmentContext.EnrollingAccount != null)
                {
                    account = (Account)(_enrollmentContext.EnrollingAccount);

                    string resultado = string.Empty;
                    resultado = ActivityBusinessLogic.Instance.DeleteActivitiesByAccountID(account.AccountID);
                    if (resultado.Trim().Length != 0) return account;

                    AccountSuppliedIDsBusinessLogic metodoAccountSuppliedIDs = new AccountSuppliedIDsBusinessLogic();
                    resultado = metodoAccountSuppliedIDs.DeleteAccountSuppliedIDsByAccountID(account.AccountID);
                    if (resultado.Trim().Length != 0) return account;

                    CreditRequirementsBusinessLogic metodoCreditRequirements = new CreditRequirementsBusinessLogic();
                    resultado = metodoCreditRequirements.DeleteCreditRequirementsByAccountID(account.AccountID);
                    if (resultado.Trim().Length != 0) return account;

                    AccountPropertiesBusinessLogic metodoAccountProperties = new AccountPropertiesBusinessLogic();
                    resultado = metodoAccountProperties.DeleteAccountPropertiesByAccountID(account.AccountID);
                    if (resultado.Trim().Length != 0) return account;

                    AccountPoliciesBusinessLogic metodoAccountPolicies = new AccountPoliciesBusinessLogic();
                    resultado = metodoAccountPolicies.DeleteAccountPoliciesByAccountID(account.AccountID);
                    if (resultado.Trim().Length != 0) return account;

                    var cache = Create.New<IOrderSearchCache>();
                    var orders = cache.Search(new NetSteps.Data.Entities.Business.OrderSearchParameters()
                    {
                        ConsultantOrCustomerAccountID = account.AccountID
                    });
                    if (orders.Count > 0) CancelOrder(orders[0].OrderNumber, account.AccountTypeID);

                    account.Delete();
                }
                account = CreateEnrollingAccount();
            }
            else
            {
                account = (Account)(_enrollmentContext.EnrollingAccount = _enrollmentContext.EnrollingAccount ?? CreateEnrollingAccount());
            }
            // BasicInfo clears the first/last name, so we have to reset it for temp accounts every time.
            if (account.IsTempAccount)
            {
                account.FirstName = "TempName";
                account.LastName = "TempName";
            }
            return account;
        }

        public virtual Account CreateEnrollingAccount()
        {
            Func<int> marketID = () =>
            {
                int mID = 0;

                if (_enrollmentContext != null)
                {
                    mID = _enrollmentContext.MarketID;

                    if (mID == 0 && _enrollmentContext.Sponsor != null)
                        mID = _enrollmentContext.Sponsor.MarketID;
                }

                try
                {
                    if (mID == 0 && System.Web.HttpContext.Current.Session["CurrentMarketID"] != null)
                        mID = (int)System.Web.HttpContext.Current.Session["CurrentMarketID"];
                }
                catch (NullReferenceException)
                {
                    //Swallowing an NullReferenceException generated by an HTTPContext dependancy that doesn't exist durring testing... 
                    //throw;
                }

                if (mID == 0)
                    mID = 1;//Default to...

                return mID;
            };

            var account = new Account
            {
                AccountStatusID = (short)NetSteps.Data.Entities.Constants.AccountStatus.BegunEnrollment,
                AccountTypeID = (short)_enrollmentContext.AccountTypeID,
                DefaultLanguageID = _enrollmentContext.LanguageID,
                FirstName = "TempName",
                LastName = "TempName",
                IsTempAccount = true,
                MarketID = marketID(),
                SponsorID = _enrollmentContext.SponsorID,
                EnrollerID = _enrollmentContext.SponsorID
            };

            // Account must be saved once to generate an AccountID and AccountNumber
            account.Save();

            return account;
        }

        /// <summary>
        /// Replaces the Enrolling Account in the enrollment context.
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns>The new Enrolling Account</returns>
        public virtual Account SetEnrollingAccount(int accountID)
        {
            var account = Account.LoadFull(accountID);

            if (account == null)
                throw new Exception("Account was not found during Set Enrolling Account");

            _enrollmentContext.EnrollingAccount = account;

            return account;
        }

        public virtual IEnumerable<int> GetPaymentTypeIDs()
        {
            return GetPaymentTypes()
                .Select(x => x.PaymentTypeID);
        }

        public virtual IEnumerable<IPaymentType> GetPaymentTypes()
        {
            // TODO: Get this from config
            return SmallCollectionCache.Instance.PaymentTypes
                .Where(x => x.PaymentTypeID == (int)ConstantsGenerated.PaymentType.CreditCard);
        }

        public virtual AccountPaymentMethod GetAccountPaymentMethod(Account account, IEnumerable<int> paymentTypeIDs = null)
        {
            var accountPaymentMethods = account.AccountPaymentMethods.AsEnumerable();

            // Filter
            if (paymentTypeIDs != null)
            {
                accountPaymentMethods = accountPaymentMethods
                    .Where(x => paymentTypeIDs.Contains(x.PaymentTypeID));
            }

            return accountPaymentMethods
                .OrderByDescending(x => x.IsDefault)
                .FirstOrDefault();
        }

        public virtual void UpdateAccountPaymentMethodAddress(Account account)
        {
            var accountPaymentMethod = GetAccountPaymentMethod(account);
            var billingAddress = account.Addresses.GetDefaultByTypeID(ConstantsGenerated.AddressType.Billing);

            if (accountPaymentMethod == null || billingAddress == null)
            {
                return;
            }

            accountPaymentMethod.BillingAddress = billingAddress;
        }

        public virtual string GetPWSDomain()
        {
            var baseSite = Site.LoadBaseSiteForNewPWS(_enrollmentContext.MarketID);

            if (baseSite == null)
            {
                return string.Empty;
            }

            return baseSite.GetDomains().FirstOrDefault() ?? string.Empty;
        }

        protected virtual string GetPWSDomainFromDB()
        {
            int marketID = SmallCollectionCache.Instance.Countries.GetById((int)_enrollmentContext.CountryID).MarketID;
            Site baseSite = Site.LoadBaseSiteForNewPWS(marketID);
            var domains = baseSite != null ? baseSite.GetDomains() : new List<string>();

            return domains[0];
        }

        protected virtual string FormatPWSUrlFromDB(string subdomain)
        {
            if (string.IsNullOrWhiteSpace(subdomain))
            {
                return string.Empty;
            }

            string pwsDomain = GetPWSDomainFromDB();
            if (string.IsNullOrWhiteSpace(pwsDomain))
            {
                return string.Empty;
            }

            return string.Format("http://{0}.{1}", subdomain, pwsDomain);
        }

        public virtual string FormatPWSUrl(string subdomain)
        {
            if (string.IsNullOrWhiteSpace(subdomain))
            {
                return string.Empty;
            }

            string pwsDomain = GetPWSDomain();
            if (string.IsNullOrWhiteSpace(pwsDomain))
            {
                return string.Empty;
            }

            return string.Format("http://{0}.{1}", subdomain, pwsDomain);
        }

        public virtual string FormatEmailAddress(string mailbox)
        {
            string domain = MailDomain.LoadDefaultForInternal().DomainName.ToLower();
            string emailAddress = string.Format("{0}@{1}", mailbox, domain);
            return emailAddress;
        }

        public virtual bool IsSiteUrlAvailable(Account account, string subdomain)
        {
            //ISite site = null;
            //if (account != null && account.AccountID > 0)
            //{
            //    site = Site.LoadByUrl(subdomain);
            //}


            //bool isSiteUrlAvailable = site == null
            //    ? SiteUrl.IsAvailable(FormatPWSUrl(subdomain))
            //    : SiteUrl.IsAvailable(site.SiteID, FormatPWSUrl(subdomain));

            //string emailAddress = FormatEmailAddress(subdomain);
            //bool isEmailAddressAvailable = MailAccount.IsAvailable(emailAddress, account == null ? 0 : account.AccountID);
            bool isEmailAddressAvailables = true;
            var urls = FormatPWSUrl(subdomain);
            var exist = SiteUrl.Repository.Where(x => x.Url == urls).FirstOrDefault();
            if (exist != null)
            {
                isEmailAddressAvailables = false;
            }

            #region Grabar WebSite
            //SaveSiteSubscriptions(_enrollmentContext.SiteID, account.FullName, null, 1, _enrollmentContext.LanguageID, null);
            //SaveSiteUrls(account, subdomain);
            #endregion

            return isEmailAddressAvailables;
        }

        public void SaveSiteUrls(Account account, string subdomain)
        {
            ISite site = null;
            if (account != null && account.AccountID > 0)
            {
                site = Site.LoadByAccountID(account.AccountID).FirstOrDefault();
            }
            SiteUrl entidadSiteUrl = new SiteUrl();
            entidadSiteUrl.SiteID = site.SiteID;
            entidadSiteUrl.Url = FormatPWSUrl(subdomain);
            entidadSiteUrl.IsPrimaryUrl = true;
            entidadSiteUrl.LanguageID = _enrollmentContext.LanguageID;
            SiteUrl.Repository.Save(entidadSiteUrl);
        }

        public void SaveSiteSubscriptions(int baseSiteId, string siteName, string siteDescription, short? siteStatusId, int? siteDefaultLanguageId, List<SiteUrl> urls)
        {
            try
            {
                Site site = Site.LoadByAccountID(CoreContext.CurrentAccount.AccountID).FirstOrDefault();

                ////Grab the current account's main or shipping address
                Account coreContextAccount = CoreContext.CurrentAccount;

                if (coreContextAccount.Addresses == null)
                    Account.LoadAddresses(coreContextAccount);

                Address tempAddress = coreContextAccount.Addresses.FirstOrDefault(ad => ad.IsDefault
                                                                                && (ad.AddressTypeID == (int)Constants.AddressType.Main
                                                                                    || ad.AddressTypeID == (int)Constants.AddressType.Shipping)
                                                                          );

                ////Load the appropriate country to obtain a marketID. If there is no address, load the base site's MarketID (US)
                int marketID = 0;
                if (tempAddress != null)
                {
                    var country = tempAddress.GetCountryFromCache();
                    if (country != null)
                    {
                        marketID = country.MarketID;
                    }
                }
                if (marketID == 0)
                {
                    marketID = Site.Load(baseSiteId).MarketID;
                }

                if (site == null)
                {
                    var languages = Language.Repository.LoadAll();
                    site = new Site()
                    {
                        AccountID = CoreContext.CurrentAccount.AccountID,
                        AccountNumber = CoreContext.CurrentAccount.AccountNumber,
                        CreatedByUserID = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUserID,
                        AutoshipOrderID = null,
                        BaseSiteID = baseSiteId,
                        MarketID = marketID,
                        IsBase = false,
                        DateCreated = DateTime.Now,
                        DateSignedUp = DateTime.Now,
                        SiteTypeID = (int)Constants.SiteType.Replicated
                    };

                    foreach (var language in languages)
                    {
                        site.Languages.Add(language);
                    }
                }

                site.Name = siteName;
                site.Description = siteDescription;
                site.DefaultLanguageID = siteDefaultLanguageId.ToInt();
                site.SiteStatusID = siteStatusId.Value;

                if (urls != null && urls.Count > 0)
                {
                    urls[0].IsPrimaryUrl = true;

                    site.SiteUrls.SyncTo(urls, new LambdaComparer<SiteUrl>((su1, su2) => su1.SiteUrlID == su2.SiteUrlID), (su1, su2) => su2.Url = su1.Url);
                }
                else
                    site.SiteUrls.RemoveAllAndMarkAsDeleted();

                site.Save();

                //return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                //return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private void SaveOrder(Order order)
        {
            order.Save();
        }

        #endregion

        #region Order Helpers
        public virtual Order GetInitialOrder(bool createIfNull = true)
        {
            //if (_enrollmentContext.InitialOrder == null
            //    && createIfNull)
            //{
            //    _enrollmentContext.InitialOrder = CreateInitialOrder(GetEnrollingAccount());
            //}
            if (createIfNull)
                _enrollmentContext.InitialOrder = CreateInitialOrder(GetEnrollingAccount());

            //Ensure the order type and pending state are correct in case we are using an online order.
            _enrollmentContext.InitialOrder.OrderTypeID = (short)Constants.OrderType.EnrollmentOrder;
            _enrollmentContext.InitialOrder.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;


            //int orderID = _enrollmentContext.InitialOrder.ConsultantID;
            //if (orderID > 0)
            //{
            //    Order order = CoreContext.CurrentOrder;
            //    return order;
            //}
            //else
            //    return (Order)_enrollmentContext.InitialOrder;
            return (Order)_enrollmentContext.InitialOrder;
        }

        public virtual AutoshipOrder GetAutoshipOrder(bool createIfNull = true)
        {
            if (_enrollmentContext.AutoshipOrder == null
                && createIfNull)
            {
                _enrollmentContext.AutoshipOrder = CreateAutoshipOrder(GetEnrollingAccount());
            }

            return (AutoshipOrder)_enrollmentContext.AutoshipOrder;
        }

        protected virtual AutoshipOrder GetSubscriptionOrder(
            bool createIfNull = true)
        {
            if (_enrollmentContext.SiteSubscriptionAutoshipOrder == null
                && createIfNull)
            {
                _enrollmentContext.SiteSubscriptionAutoshipOrder = CreateSubscriptionAutoshipOrder(GetEnrollingAccount());
            }

            return (AutoshipOrder)_enrollmentContext.SiteSubscriptionAutoshipOrder;
        }

        public virtual AutoshipOrder CreateAutoshipOrder(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            // Try getting autoshipScheduleID from config
            var autoshipScheduleID = _enrollmentContext.EnrollmentConfig.Autoship.AutoshipScheduleID;

            if (autoshipScheduleID == null)
            {
                throw new Exception("AutoshipScheduleID is not defined in the 'Autoship' enrollment configuration.");
            }

            return CreateAutoshipOrder(account, autoshipScheduleID.Value);
        }

        protected virtual AutoshipOrder CreateSubscriptionAutoshipOrder(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            // Try getting autoshipScheduleID from config
            var autoshipScheduleID = _enrollmentContext.EnrollmentConfig.Subscription.AutoshipScheduleID;

            if (autoshipScheduleID == null)
            {
                throw new Exception("AutoshipScheduleID is not defined in the 'Subscription' enrollment configuration.");
            }

            return CreateAutoshipOrder(account, autoshipScheduleID.Value);
        }

        protected virtual AutoshipOrder CreateAutoshipOrder(Account account, int autoshipScheduleID)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }
            if (account.AccountID <= 0)
            {
                throw new ArgumentException("AccountID cannot be zero. Account must be saved before order is created.", "account");
            }

            var autoshipSchedule = AutoshipSchedule.LoadFull(autoshipScheduleID);

            var autoshipOrder = new AutoshipOrder
            {
                AccountID = account.AccountID,
                AutoshipScheduleID = autoshipScheduleID,
                StartDate = DateTime.Today,
                Order = CreateBaseOrder(autoshipSchedule.OrderTypeID, account)
            };

            autoshipOrder.NextRunDate = autoshipOrder.CalculateFirstRunDate();
            autoshipOrder.Day = autoshipOrder.NextRunDate.Value.Day;

            foreach (var autoshipScheduleProduct in autoshipSchedule.AutoshipScheduleProducts)
            {
                autoshipOrder.Order.AddItem(autoshipScheduleProduct.ProductID, autoshipScheduleProduct.Quantity);
            }

            return autoshipOrder;
        }

        protected virtual Order CreateBaseOrder(short orderTypeID, Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }
            if (account.AccountID <= 0)
            {
                throw new ArgumentException("AccountID cannot be zero. Account must be saved before order is created.", "account");
            }

            // Be careful to keep order and account separate in the "graph".
            var order = new Order
            {
                OrderStatusID = (short)Constants.OrderStatus.Pending,
                SiteID = CurrentSite.SiteID,
                CurrencyID = _enrollmentContext.CurrencyID,
                OrderTypeID = orderTypeID,
                OrderPendingState = Constants.OrderPendingStates.Quote
            };

            order.StartEntityTracking();

            order.SetConsultantID(account);

            order.OrderCustomers.Add(new OrderCustomer
            {
                OrderCustomerTypeID = (short)Constants.OrderCustomerType.Normal,
                AccountID = account.AccountID,
                EffectiveOrderAccountTypeID = (short)_enrollmentContext.AccountTypeID
            });
            order.OrderCustomers[0].StartEntityTracking();

            return order;
        }

        /// <summary>
        /// Sets the AccountID on all enrollment & autoship orders.
        /// This is not pretty, but it is used when switching the enrolling account to upgrade a prospect.
        /// </summary>
        /// <param name="accountID">The new AccountID</param>
        public virtual void SetAccountIDOnOrders(int accountID)
        {
            if (_enrollmentContext.InitialOrder != null)
            {
                SetAccountIDOnOrder((Order)_enrollmentContext.InitialOrder, accountID);

                _enrollmentContext.InitialOrder.Save();
            }

            if (_enrollmentContext.AutoshipOrder != null)
            {
                ((AutoshipOrder)_enrollmentContext.AutoshipOrder).AccountID = accountID;

                if (((AutoshipOrder)_enrollmentContext.AutoshipOrder).Order != null)
                {
                    SetAccountIDOnOrder(((AutoshipOrder)_enrollmentContext.AutoshipOrder).Order, accountID);
                }

                ((AutoshipOrder)_enrollmentContext.AutoshipOrder).Save();
            }

            if (_enrollmentContext.SiteSubscriptionAutoshipOrder != null)
            {
                ((AutoshipOrder)_enrollmentContext.SiteSubscriptionAutoshipOrder).AccountID = accountID;

                if (((AutoshipOrder)_enrollmentContext.SiteSubscriptionAutoshipOrder).Order != null)
                {
                    SetAccountIDOnOrder(((AutoshipOrder)_enrollmentContext.SiteSubscriptionAutoshipOrder).Order, accountID);
                }

                ((AutoshipOrder)_enrollmentContext.AutoshipOrder).Save();
            }
        }

        /// <summary>
        /// Sets the AccountID on an order.
        /// This is not pretty, but it is used when switching the enrolling account to upgrade a prospect.
        /// </summary>
        /// <param name="accountID">The new AccountID</param>
        public virtual void SetAccountIDOnOrder(Order order, int accountID)
        {
            // This is just to allow the order to save, the "real" ConsultantID is set when the order is submitted.
            order.ConsultantID = accountID;
            foreach (var orderCustomer in order.OrderCustomers)
            {
                orderCustomer.AccountID = accountID;
            }
        }

        public virtual void UpdateAllOrderShipmentAddresses(Address shippingAddress)
        {
            if (_enrollmentContext.InitialOrder != null)
            {
                UpdateOrderShipmentAddress((Order)_enrollmentContext.InitialOrder, shippingAddress);
                var orderContext = Create.New<IOrderContext>();
                orderContext.Order = _enrollmentContext.InitialOrder.AsOrder();
                OrderService.UpdateOrder(orderContext);
            }

            if (_enrollmentContext.AutoshipOrder != null && ((AutoshipOrder)_enrollmentContext.AutoshipOrder).Order != null)
            {
                UpdateOrderShipmentAddress(((AutoshipOrder)_enrollmentContext.AutoshipOrder).Order, shippingAddress);
                var orderContext = Create.New<IOrderContext>();
                orderContext.Order = ((AutoshipOrder)_enrollmentContext.AutoshipOrder).Order;
                OrderService.UpdateOrder(orderContext);
            }

            if (_enrollmentContext.SiteSubscriptionAutoshipOrder != null && ((AutoshipOrder)_enrollmentContext.SiteSubscriptionAutoshipOrder).Order != null)
            {
                UpdateOrderShipmentAddress(((AutoshipOrder)_enrollmentContext.SiteSubscriptionAutoshipOrder).Order, shippingAddress);
                var orderContext = Create.New<IOrderContext>();
                orderContext.Order = ((AutoshipOrder)_enrollmentContext.SiteSubscriptionAutoshipOrder).Order;
                OrderService.UpdateOrder(orderContext);
            }
        }

        public virtual void UpdateOrderShipmentAddress(Order order, Account account)
        {
            var shippingAddress = account.Addresses.GetDefaultByTypeID(NetSteps.Data.Entities.Constants.AddressType.Shipping);
            if (shippingAddress != null)
            {
                UpdateOrderShipmentAddress(order, shippingAddress);
            }
        }

        public virtual void UpdateOrderShipmentAddress(Order order, IAddress shippingAddress)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order");
            }
            if (shippingAddress == null)
            {
                throw new ArgumentNullException("shippingAddress");
            }

            order.UpdateOrderShipmentAddress(order.GetDefaultShipment(), shippingAddress);
        }

        public virtual AutoshipSchedule GetAutoshipSchedule(AutoshipOrder autoshipOrder)
        {
            return SmallCollectionCache.Instance.AutoshipSchedules.GetById(autoshipOrder.AutoshipScheduleID);
        }

        public virtual AutoshipOrder CreateAutoshipOrderFromInitialOrder(Order initialOrder, Account account)
        {
            if (initialOrder == null)
            {
                throw new ArgumentNullException("order");
            }
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            var autoshipOrder = CreateAutoshipOrder(account);
            //This is to make sure the autoship does not have duplicate orderitems
            if (!((AutoshipOrder)autoshipOrder).Order.OrderCustomers[0].OrderItems.Any())
            {
                CopyOrderItems(autoshipOrder.Order, initialOrder);
            }
            CopyOrderShipment(autoshipOrder.Order, initialOrder);
            autoshipOrder.Order = this.TotalOrder(autoshipOrder.Order);

            return autoshipOrder;
        }

        protected virtual Order CreateInitialOrderFromAutoshipOrder(AutoshipOrder autoshipOrder, Account account)
        {
            if (autoshipOrder == null)
            {
                throw new ArgumentNullException("order");
            }
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            var initialOrder = CreateInitialOrder(account);
            CopyOrderItems(initialOrder, autoshipOrder.Order);
            CopyOrderShipment(initialOrder, autoshipOrder.Order);

            return initialOrder;
        }

        protected virtual void ImportShoppingOrder(Order targetOrder)
        {
            var shoppingOrder = GetShoppingOrder();
            if (shoppingOrder != null)
            {
                CopyOrderItems(targetOrder, shoppingOrder);

                // Clear the shopping order since it has been duplicated.
                // This will also prevent the items from being imported multiple times.
                ResetShoppingOrder();
            }
        }

        protected virtual Order GetShoppingOrder()
        {
            return OrderContext.Order.AsOrder();
        }

        protected virtual void ResetShoppingOrder()
        {
            OrderContext.Clear();
            CoreContext.CurrentOrder = base.CreateNewOrder();
            OrderContext.Order = CoreContext.CurrentOrder;
        }

        public virtual void CopyOrderItems(IOrder targetOrder, IOrder sourceOrder)
        {
            Order _targetOrder = (Order)targetOrder;
            if (_targetOrder == null
                || _targetOrder.OrderCustomers.Count != 1
                || sourceOrder == null
                || _targetOrder.OrderCustomers.Count != 1)
            {
                return;
            }

            foreach (var orderItem in sourceOrder.OrderCustomers.First().OrderItems)
            {
                if (orderItem.ProductID != null)
                {
                    ((Order)targetOrder).AddItem(orderItem.ProductID.Value, orderItem.Quantity);
                }
            }
        }

        public virtual void CopyOrderShipment(Order targetOrder, Order sourceOrder)
        {
            var sourceShipment = ((Order)sourceOrder).GetDefaultShipmentNoDefault();
            if (sourceShipment == null)
            {
                throw new ArgumentException("The source order does not have a shipment.", "sourceOrder");
            }

            UpdateOrderShipmentAddress(targetOrder, sourceShipment);

            var targetShipment = ((Order)targetOrder).GetDefaultShipment();
            targetShipment.ShippingMethodID = sourceShipment.ShippingMethodID;
        }

        public virtual void DeleteAutoshipOrder()
        {
            if (_enrollmentContext.AutoshipOrder == null)
            {
                return;
            }

            var autoshipOrder = (AutoshipOrder)_enrollmentContext.AutoshipOrder;
            if (autoshipOrder.AutoshipOrderID != 0)
            {
                autoshipOrder.MarkAsDeleted();
                autoshipOrder.Save();
            }

            _enrollmentContext.AutoshipOrder = null;
        }

        public virtual Order CreateInitialOrder(Account account)
        {
            OrderContext.Order = CreateBaseOrder((short)Constants.OrderType.EnrollmentOrder, account);
            return OrderContext.Order.AsOrder();
        }

        private void SaveAutoshipOrder(AutoshipOrder autoshipOrder)
        {
            ((AutoshipOrder)autoshipOrder).Save();
        }
        #endregion

        #region Other Helpers

        protected virtual Order TotalOrder(Order order)
        {
            var orderContext = Create.New<IOrderContext>();
            orderContext.Order = order;
            OrderService.UpdateOrder(orderContext);

            return orderContext.Order.AsOrder();
        }

        protected virtual ActionResult StepCompleted()
        {
            if (!string.IsNullOrWhiteSpace(_enrollmentContext.ReturnUrl))
            {
                string returnUrl = _enrollmentContext.ReturnUrl;
                _enrollmentContext.ReturnUrl = null;
                return Redirect(returnUrl);
            }

            return RedirectToStep(_enrollmentContext.EnrollmentConfig.Steps.NextItem);
        }

        /*CSTI(CS)-05/03/2016-Inicio*/
        protected virtual ActionResult StepCompleted(bool MVCAutomation = true)
        {
            if (!string.IsNullOrWhiteSpace(_enrollmentContext.ReturnUrl))
            {
                string returnUrl = _enrollmentContext.ReturnUrl;
                _enrollmentContext.ReturnUrl = null;
                if (MVCAutomation)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return Json(new { TypeRedirect = 0, Url = returnUrl });
                }

            }

            return RedirectToStep(_enrollmentContext.EnrollmentConfig.Steps.NextItem, MVCAutomation);
        }
        /*CSTI(CS)-05/03/2016-Fin*/

        /// <summary>
        /// Temporary constructor until we wireup DI.
        /// </summary>
        public EnrollStepBaseController() { }

        /// <summary>
        /// Testing constructor.
        /// </summary>
        public EnrollStepBaseController(IEnrollmentContext<EnrollmentKitConfig> enrollmentContext) { _enrollmentContext = enrollmentContext; }
        #endregion
    }
}
