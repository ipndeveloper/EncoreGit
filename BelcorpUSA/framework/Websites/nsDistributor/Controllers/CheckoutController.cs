using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Addresses.PickupPoints.Common.Services;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes;
using NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes.Interfaces;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Globalization;
using NetSteps.Web.Mvc.Controls.Configuration;
using NetSteps.Encore.Core.IoC;
using NetSteps.Events.Common.Services;
using NetSteps.GiftCards.Common;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Diagnostics.Utilities;
using nsDistributor.Models.Checkout;
using NetSteps.Commissions.Common;
using NetSteps.Data.Entities.Business.Logic;

using NetSteps.Web.Mvc.Controls.Models.Enrollment;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Enrollment.Common.Provider;
using NetSteps.Web.Mvc.Controls.Configuration;

namespace nsDistributor.Controllers
{
    public class CheckoutController : BaseShoppingController
    {
        private ICommissionsService _commissionsService = Create.New<ICommissionsService>();
        private IProductCreditLedgerService _productCreditLedgerService = Create.New<IProductCreditLedgerService>();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if ((!IsLoggedIn || OrderContext.Order == null) && filterContext.ActionDescriptor.ActionName != "LookupZip")
            {
                if (Request.IsAjaxRequest())
                    filterContext.Result = Json(new { result = false, message = Translation.GetTerm("YourSessionHasTimedOutPleaseRefreshthePage", "Your session has timed out.  Please refresh the page.") });
                else
                {
                    if (OrderContext.Order == null)
                    {
                        TempData["SessionTimedOut"] = true;
                        filterContext.Result = RedirectToAction("Index", "Shop");
                    }
                    else
                        filterContext.Result = RedirectToAction("Login", "Security", new { returnUrl = Request.Url.AbsolutePath });
                }
                return;
            }
        }

        #region Shipping
        [FunctionFilter("Orders", "~/")]
        public virtual ActionResult Shipping()
        {
            // Developed by BAL - CSTI - AINI
            string strSKUs = "";
            foreach (var item in OrderContext.Order.OrderCustomers[0].OrderItems)
            {
                if (ProductQuotasRepository.ProductIsRestricted(item.ProductID.Value, item.Quantity, Account.AccountID, Account.AccountTypeID))
                    strSKUs += " *" + Inventory.GetProduct(item.ProductID.Value).SKU;
            }

            if (!strSKUs.IsEmpty())
                return RedirectToAction("Index", "Cart");
            // Developed by BAL - CSTI - AFIN

            var model = new ShippingModel();
            model.Addresses = GetDistinctAddresses().Select(x =>
            {
                var shippingAddressModel = new nsDistributor.Models.Checkout.ShippingAddressModel(x);
                shippingAddressModel.ShipToEmailAddress = x.GetEmailAddress();

                return shippingAddressModel;
            });

            model.ShowShipToEmail = ConfigurationManager.GetAppSetting("ShowShipToEmail", false);

            try
            {
                model.PickupPointsEnabled = Market.Load(CoreContext.CurrentAccount.MarketID).PickupPointsEnabled;
                if (model.PickupPointsEnabled)
                {
                    var pupService = Create.New<IPickupPointService>();
                    model.PickupPoints = pupService.GetPickupPoints(CurrentCulture, "79310", "Vouhe").Select(x => new ShippingPickupPointModel(x));
                }
            }
            catch (Exception excp)
            {
                model.PickupPointsEnabled = false;

                excp.TraceException(excp);
                EntityExceptionHelper.GetAndLogNetStepsException(excp, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

            model.AddressModel = GetAddressModel();

            return View(model);
        }

        public virtual AddressModel GetAddressModel()
        {
            return new AddressModel()
            {
                Address = null,
                ShowCountrySelect = true,
                ChangeCountryURL = "~/Checkout/GetAddressControl",
                Country = BaseController.DefaultCountry,
                ExcludeFields = new List<string>() { "PhoneNumber" },
                HonorRequired = true,
                LanguageID = BaseController.CurrentLanguageID,
                Prefix = "Checkout"
            };
        }

        protected List<Address> GetDistinctAddresses()
        {
            var account = CoreContext.CurrentAccount;
            var shippingAddresses = account.Addresses.Where<Address>(a => a.AddressTypeID == (int)Constants.AddressType.Shipping).ToList();
            var otherAddresses = account == null ? new List<Address>() : account.Addresses.Where<Address>(a => a.AddressTypeID == (int)Constants.AddressType.Main).ToList();
            var distinctAddresses = new List<Address>();

            distinctAddresses.AddRange(AddDistinctAddresses(distinctAddresses, shippingAddresses));
            distinctAddresses.AddRange(AddDistinctAddresses(distinctAddresses, otherAddresses));
            return distinctAddresses;
        }

        [NonAction]
        public List<Address> AddDistinctAddresses(List<Address> distinctAddressList, List<Address> inputAddressList)
        {
            var returnList = new List<Address>();

            foreach (var address in inputAddressList)
            {
                address.Trim();
                if (!distinctAddressList.Any(addy => addy.Attention == address.Attention &&
                        addy.FirstName == address.FirstName &&
                        addy.LastName == address.LastName &&
                        addy.Name == address.Name &&
                        addy.Address1 == address.Address1 &&
                        addy.Address2 == address.Address2 &&
                        addy.Address3 == address.Address3 &&
                        addy.City == address.City &&
                        addy.County == address.County &&
                        (addy.StateProvinceID == address.StateProvinceID || addy.State == address.State) &&
                        addy.CountryID == address.CountryID))
                    returnList.Add(address);
            }

            return returnList;
        }

        [FunctionFilter("Orders", "~/")]
        public virtual ActionResult ChooseShippingProfile(int addressId, string pickupPointCode = null)
        {
            try
            {
                /* 2011-11-04, JWL, Check to see if given address id is a shipping type
                 * if it isn't, then we must create a shipping address record and use that instead
                 */
                var inputAddress = Address.LoadFull(addressId);
                Address shippingAddress = null;
                if (inputAddress.AddressTypeID != (int)Constants.AddressType.Shipping)
                {
                    var account = CoreContext.CurrentAccount;
                    shippingAddress = new Address();
                    Address.CopyPropertiesTo(inputAddress, shippingAddress);
                    shippingAddress.AddressTypeID = (int)Constants.AddressType.Shipping;
                    shippingAddress.Save();

                    addressId = shippingAddress.AddressID;

                    shippingAddress.Accounts.Add(account);
                    account.Addresses.Add(shippingAddress);
                }

                var shippingMethods = shippingAddress.IsNotNull()
                    ? OrderContext.Order.AsOrder().UpdateOrderShipmentAddressAndDefaultShipping(shippingAddress).ToList()
                    : OrderContext.Order.AsOrder().UpdateOrderShipmentAddressAndDefaultShipping(addressId).ToList();

                if (shippingMethods.Count == 0)
                {
                    OrderContext.Order.AsOrder().SetShippingMethod(null);
                }

                if (!String.IsNullOrWhiteSpace(pickupPointCode))
                    OrderContext.Order.AsOrder().GetDefaultShipment().PickupPointCode = pickupPointCode;

                OrderService.UpdateOrder(OrderContext);

                return Json(new { result = true, chooseShippingMethod = (shippingMethods.Count >= 0) });
            }
            catch (ProductShippingExcludedShippingException ex)
            {
                var excludedProducts = ex.ProductsThatHaveExcludedShipping.Select(prod => prod.Name);
                return Json(new { result = true, message = ex.PublicMessage, products = excludedProducts });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Orders", "~/")]
        public virtual ActionResult NewShippingProfile(int country, string attention, string address1, string address2, string address3, string postalCode, string city, string state, string county, string emailId)
        {
            try
            {
                if (address1.IsNullOrWhiteSpace())
                {
                    return Json(new { result = false, message = Translation.GetTerm("ShippingProfile_Address1Required", "Address1 is required") });
                }
                Address address = new Address();
                Account account = CoreContext.CurrentAccount;
                var originalAccount = CoreContext.CurrentAccount.Clone();
                address.AttachAddressChangedCheck();
                address.ProfileName = string.IsNullOrEmpty(attention) ? account.FullName : attention;
                address.Attention = attention.ToCleanString();
                address.Address1 = address1.ToCleanString();
                address.Address2 = address2.ToCleanString();
                address.Address3 = address3.ToCleanString();
                address.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Shipping.ToShort();
                address.City = city.ToCleanString();

                if (!String.IsNullOrWhiteSpace(state))
                {
                    address.SetState(state, country);
                }

                if (!String.IsNullOrWhiteSpace(emailId))
                {
                    address.SetEmailAddress(emailId);
                }

                address.County = county.ToCleanString();
                string cleanedPostalCode;
                if (!Create.New<IPostalCodeLookupProvider>().ValidateAndCleanPostalCode(country, postalCode, out cleanedPostalCode))
                {
                    return Json(new { result = false, message = Translation.GetTerm("PWS_Checkout_InvalidPostal", "Invalid postal code: ") + postalCode });
                }

                address.PostalCode = cleanedPostalCode;
                address.CountryID = country;
                if (account.Addresses.GetAllByTypeID(Constants.AddressType.Shipping).Count == 0)
                    address.IsDefault = true;
                address.LookUpAndSetGeoCode();
                address.Validate();
                if (!address.IsValid)
                {
                    CoreContext.CurrentAccount = originalAccount;
                    return Json(new
                    {
                        result = false,
                        message = address.GetValidationErrorMessage()
                    });
                }
                var result = address.ValidateAddressAccuracy();
                if (!result.Success)
                    return Json(new { result = false, message = result.Message });

                account.Addresses.Add(address);

                //retail customers needs to have a main address added for the commissions engine
                if (account.AccountTypeID == (int)NetSteps.Data.Entities.Constants.AccountType.RetailCustomer && account.Addresses.GetAllByTypeID(Constants.AddressType.Main).Count == 0)
                {
                    Address mainAddress = new Address();
                    Address.CopyPropertiesTo(address, mainAddress);
                    mainAddress.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Main.ToShort();
                    account.Addresses.Add(mainAddress);
                    //No need to validate because Shipping address was already validated.
                }
                // To remove OrderCustomers from the Account before save - JHE
                //Order.UpdateOrderAccountInstanceWithModifiedAccount(Order, account);

                account.Save();

                CoreContext.CurrentAccount = account;

                var shippingMethods = ((Order)OrderContext.Order).UpdateOrderShipmentAddressAndDefaultShipping(address);

                //Order = Order.UpdateOrderAccountInstanceWithModifiedAccount(Order, account);

                return Json(new { result = true, chooseShippingMethod = shippingMethods.Count() > 1 });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Orders", "~/")]
        public virtual ActionResult SaveShippingProfile(int country, string attention, string address1, string address2, string address3, string postalCode,
            string city, string state, string county, string profileName, int? addressID, string emailId, string street)
        {
            try
            {
                if (address1.IsNullOrWhiteSpace())
                {
                    return Json(new { result = false, message = Translation.GetTerm("ShippingProfile_Address1Required", "Address1 is required") });
                }

                var account = CoreContext.CurrentAccount = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
                Address address = null;
                if (addressID != null)
                {
                    address = account.Addresses.GetByAddressID(addressID.Value);
                }

                if (address == null)
                {
                    address = new Address();
                    address.StartEntityTracking();
                    address.IsDefault = account.Addresses.GetAllByTypeID(Constants.AddressType.Shipping).Count == 0;
                    address.AddressTypeID = Constants.AddressType.Shipping.ToShort();
                    account.Addresses.Add(address);
                }

                address.AttachAddressChangedCheck();
                address.ProfileName = string.IsNullOrEmpty(profileName) ? account.FullName : profileName;
                address.Attention = attention.ToCleanString();
                address.Address1 = address1.ToCleanString();
                address.Address2 = address2.ToCleanString();
                address.Address3 = address3.ToCleanString();
                address.City = city.ToCleanString();
                address.County = county.ToCleanString();
                address.Street = street;

                if (!String.IsNullOrWhiteSpace(state))
                {
                    address.SetState(state, country);
                }

                if (!String.IsNullOrWhiteSpace(emailId))
                {
                    address.SetEmailAddress(emailId);
                }

                string cleanedPostalCode;
                if (!Create.New<IPostalCodeLookupProvider>().ValidateAndCleanPostalCode(country, postalCode, out cleanedPostalCode))
                {
                    return Json(new { result = false, message = Translation.GetTerm("PWS_Checkout_InvalidPostal", "Invalid postal code: ") + postalCode });
                }

                address.PostalCode = cleanedPostalCode;
                address.CountryID = country;
                address.LookUpAndSetGeoCode();
                address.Validate();
                if (!address.IsValid)
                {
                    return Json(new
                    {
                        result = false,
                        message = address.GetValidationErrorMessage()
                    });
                }
                var result = address.ValidateAddressAccuracy();
                if (!result.Success)
                    return Json(new { result = false, message = result.Message });

                account.Save();

                UpdateAddressStreet(account, street, address.AddressID);

                CoreContext.CurrentAccount = account;

                //If this is saving the first profile for this address or setting a default shipping profile
                //check to see if there are pickup points enabled
                //If pickup points are not enabled, send the user to the shipping methods page
                if (address.IsDefault && !Market.Load(CoreContext.CurrentAccount.MarketID).PickupPointsEnabled)
                {
                    OrderContext.Order.AsOrder().UpdateOrderShipmentAddressAndDefaultShipping(address);

                    return Json(new { result = true, location = "ShippingMethod" });
                }

                return this.ChooseShippingProfile(address.AddressID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private void UpdateAddressStreet(Account account, string street, int addressID)
        {
            Address direccion = new Address();
            direccion = account.Addresses.Where(donde => donde.AddressID == addressID).FirstOrDefault();
            if (direccion != null)
            {
                direccion.Street = street;
                AddressBusinessLogic business = new AddressBusinessLogic();
                business.UpdateAddressStreet(direccion);
            }
        }

        [FunctionFilter("Orders", "~/")]
        public virtual ActionResult LoadShippingProfileForEdit(int? addressID)
        {
            try
            {
                var account = CoreContext.CurrentAccount;
                string shipToEmailAddress = null;
                Address address = null;
                address = account.Addresses.GetByAddressID(addressID.Value);
                if (!addressID.HasValue || (addressID.Value != 0 && address == null))
                {
                    return Json(new { result = false, message = Translation.GetTerm("AddressProfileNotFound", "Address profile not found") });
                }
                if (address != null)
                {
                    shipToEmailAddress = address.GetEmailAddress();
                }
                var addressModel = new AddressModel()
                                       {
                                           Address = address,
                                           ShowCountrySelect = true,
                                           ChangeCountryURL = "~/Checkout/GetAddressControl",
                                           Country = BaseController.DefaultCountry,
                                           ExcludeFields = new List<string>() { "PhoneNumber" },
                                           HonorRequired = true,
                                           LanguageID = BaseController.CurrentLanguageID,
                                           Prefix = "ShippingCheckOut"
                                       };
                return
                    Json(
                        new
                            {
                                result = true,
                                shipToEmail = shipToEmailAddress,
                                data = RenderRazorPartialViewToString("Address", addressModel)
                            });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (CoreContext.CurrentOrder != null) ? CoreContext.CurrentOrder.OrderID.ToIntNullable() : null, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        #region Shipping Method
        [FunctionFilter("Orders", "~/")]
        public virtual ActionResult ShippingMethod()
        {
            IEnumerable<ShippingMethodWithRate> shippingMethods = new Collection<ShippingMethodWithRate>();

            // So we can continue to use the logic below
            try
            {
                shippingMethods = OrderContext.Order.AsOrder().GetShippingMethods();
            }
            catch (ProductShippingExcludedShippingException ex)
            {
                ViewBag.Products = ex.ProductsThatHaveExcludedShipping;
            }

            ViewData["ShippingMethodID"] = OrderContext.Order.AsOrder().GetDefaultShippingMethodID();

            return View(shippingMethods);
        }

        [FunctionFilter("Orders", "~/")]
        public virtual ActionResult ChooseShippingMethod(int shippingMethodId)
        {
            try
            {
                OrderContext.Order.AsOrder().SetShippingMethod(shippingMethodId);
                OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;
                OrderService.UpdateOrder(OrderContext);

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        #endregion

        #region Billing
        [FunctionFilter("Orders", "~/")]
        public virtual ActionResult Billing()
        {
            SetProductCreditViewData();
            return View(OrderContext.Order.AsOrder());
        }

        [NonAction]
        public virtual void SetProductCreditViewData()
        {
            var productCreditBalance = _productCreditLedgerService.GetCurrentBalanceLessPendingPayments(CoreContext.CurrentAccount.AccountID);
            ViewBag.ProductCreditBalance = productCreditBalance;
        }

        /// <summary>
        /// Override this method to add-in client specific PaymentTypes
        /// in addition to the base Encore ones.
        /// </summary>
        [NonAction]
        public virtual IPaymentTypeProvider GetPaymentTypeProvider()
        {
            return new PaymentTypeProvider();
        }

        [NonAction]
        public IPayment GetPaymentMethodFromProvider(PaymentTypeModel paymentTypeModel)
        {
            IPaymentTypeProvider provider = GetPaymentTypeProvider();
            IPayment payment = provider.GetPaymentMethod(paymentTypeModel);

            if (IsPaymentTypeCreditCard(paymentTypeModel.PaymentType))
            {
                var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
                var shipment = customer != null && customer.OrderShipments.Count > 0
                    ? customer.OrderShipments.First()
                    : OrderContext.Order.AsOrder().GetDefaultShipment();
                payment.BillingAddress.PostalCode = paymentTypeModel.BillingZipCode.ToCleanString();
                payment.BillingAddress.CountryID = shipment.CountryID;
            }

            return payment;
        }

        private bool IsPaymentTypeEFT(int PaymentType)
        {
            return PaymentType == Constants.PaymentType.EFT.ToInt();
        }

        private bool IsPaymentTypeCreditCard(int paymentType)
        {
            return paymentType == Constants.PaymentType.CreditCard.ToInt();
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders", "~/")]
        public virtual ActionResult ApplyPayment(PaymentTypeModel paymentTypeModel, bool? saveAsNewProfile)
        {
            try
            {
                IPayment payment;
                if (paymentTypeModel.PaymentMethodID.HasValue)
                {
                    payment = CoreContext.CurrentAccount.AccountPaymentMethods.GetByAccountPaymentMethodID(paymentTypeModel.PaymentMethodID.Value);
                }
                else
                {
                    //CGI (JCT) - Adicion de nuevos campos en PWS - Billing
                    if (IsPaymentTypeCreditCard(paymentTypeModel.PaymentType) && (string.IsNullOrEmpty(paymentTypeModel.PostalCode) || string.IsNullOrEmpty(paymentTypeModel.City) || string.IsNullOrEmpty(paymentTypeModel.State)))
                    {
                        var exception = EntityExceptionHelper.GetAndLogNetStepsException(TermTranslation.LoadTermByTermNameAndLanguageID("Zipcode Required.", CurrentSite.Language.LanguageID), NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                        return Json(new { result = false, message = exception.PublicMessage });
                    }

                    payment = GetPaymentMethodFromProvider(paymentTypeModel);
                    payment.PaymentTypeID = paymentTypeModel.PaymentType;
                }

                BasicResponseItem<OrderPayment> response = OrderContext.Order.AsOrder().ApplyPaymentToCustomer(payment.PaymentTypeID, paymentTypeModel.Amount, payment.NameOnCard, payment);

                if (!response.Success)
                    return Json(new { result = false, message = response.Message });

                // Ensure that shipping and taxes are not recalculated simply because a payment is being added.
                OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Open;
                OrderService.UpdateOrder(OrderContext);
                OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;

                // save as new billing profile
                if (response.Success && !paymentTypeModel.PaymentMethodID.HasValue && IsPaymentTypeCreditCard(paymentTypeModel.PaymentType)
                    && saveAsNewProfile.HasValue && saveAsNewProfile.Value)
                {
                    Account account = CoreContext.CurrentAccount;
                    AccountPaymentMethod paymentMethod = new AccountPaymentMethod();
                    Address address = new Address();

                    try
                    {
                        paymentMethod.NameOnCard = paymentTypeModel.NameOnCard;
                        paymentMethod.DecryptedAccountNumber = paymentTypeModel.AccountNumber.RemoveNonNumericCharacters();
                        paymentMethod.ExpirationDateUTC = paymentTypeModel.ExpirationDate.Value.LastDayOfMonth();
                        paymentMethod.PaymentTypeID = (int)Constants.PaymentType.CreditCard;
                        paymentMethod.ProfileName = paymentTypeModel.ProfileName;

                        address.ProfileName = paymentTypeModel.ProfileName;
                        address.StartEntityTracking();
                        address.AttachAddressChangedCheck();
                        address.PostalCode = paymentTypeModel.PostalCode;
                        address.CountryID = payment.BillingAddress.CountryID;
                        address.AddressTypeID = Constants.AddressType.Billing.ToShort();
                        address.Address1 = paymentTypeModel.AddressLine;
                        address.State = paymentTypeModel.State;
                        address.City = paymentTypeModel.City;
                        paymentMethod.BillingAddress = address;
                        account.Addresses.Add(address);
                        account.AccountPaymentMethods.Add(paymentMethod);
                        account.Save();
                    }
                    catch (Exception exBillPro)
                    {
                        // remove payment we just added and rethrow the exception
                        this.RemovePayment(response.Item.Guid.ToString("N"));
                        account.Addresses.Remove(address);
                        account.AccountPaymentMethods.Remove(paymentMethod);
                        throw exBillPro;
                    }
                }//EFT payment
                else if (response.Success && !paymentTypeModel.PaymentMethodID.HasValue && IsPaymentTypeEFT(paymentTypeModel.PaymentType)
                    && saveAsNewProfile.HasValue && saveAsNewProfile.Value)
                {
                    Account account = CoreContext.CurrentAccount;
                    AccountPaymentMethod paymentMethod = new AccountPaymentMethod();
                    Address address = new Address();
                    try
                    {
                        paymentMethod.BankName = paymentTypeModel.BankName;
                        paymentMethod.BankAccountTypeID = paymentTypeModel.BankAccountTypeID;
                        paymentMethod.ProfileName = paymentTypeModel.ProfileName;
                        paymentMethod.NameOnCard = paymentTypeModel.NameOnAccount;
                        paymentMethod.DecryptedAccountNumber = paymentTypeModel.BankAccountNumber.RemoveNonNumericCharacters();
                        paymentMethod.RoutingNumber = paymentTypeModel.RoutingNumber;
                        paymentMethod.PaymentTypeID = (int)Constants.PaymentType.EFT;
                        address = account.Addresses.FirstOrDefault(a => a.IsDefault);
                        paymentMethod.BillingAddress = address;

                        account.AccountPaymentMethods.Add(paymentMethod);
                        account.Save();
                    }
                    catch (Exception exBillPro)
                    {
                        // remove payment we just added and rethrow the exception
                        this.RemovePayment(response.Item.Guid.ToString("N"));
                        account.Addresses.Remove(address);
                        account.AccountPaymentMethods.Remove(paymentMethod);
                        throw exBillPro;
                    }
                }

                return Json(new
                {
                    result = true,
                    totals = GetTotals(),
                    productCreditBalance = _productCreditLedgerService.GetCurrentBalanceLessPendingPayments(CoreContext.CurrentAccount.AccountID).ToString(OrderContext.Order.CurrencyID),
                    paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order),
                    billingProfilesGrid = RenderRazorPartialViewToString("BillingProfilesGrid", CoreContext.CurrentAccount)
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders", "~/")]
        public virtual ActionResult RemovePayment(string paymentId)
        {
            try
            {
                var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
                if (customer != null)
                {
                    var payment = customer.OrderPayments.FirstOrDefault(op => op.Guid.ToString("N") == paymentId);
                    if (payment != null)
                    {
                        if (payment.OrderPaymentStatusID == Constants.OrderPaymentStatus.Completed.ToInt())
                        {
                            return Json(new { result = false, message = Translation.GetTerm("ThisCreditCardHasAlreadyBeenAuthorizedAndCannotBeRemoved", "This credit card has already been authorized and cannot be removed.") });
                        }

                        customer.RemovePayment(payment);
                    }

                    // Ensure that shipping and taxes are not recalculated simply because a payment is being removed.
                    OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Open;
                    OrderService.UpdateOrder(OrderContext);
                    OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;

                    return Json(new
                    {
                        result = true,
                        totals = GetTotals(),
                        productCreditBalance = _productCreditLedgerService.GetCurrentBalanceLessPendingPayments(CoreContext.CurrentAccount.AccountID).ToString(OrderContext.Order.CurrencyID),
                        paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order)
                    });
                }
                return Json(new { result = false, message = Translation.GetTerm("CouldNotFindCustomer", "Could not find that customer.") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [HttpPost]
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders", "~/")]
        public virtual ActionResult LookupGiftCardBalance(string giftCardCode)
        {
            try
            {
                var gcService = Create.New<IGiftCardService>();

                decimal? balance = gcService.GetBalanceWithPendingPayments(giftCardCode, OrderContext.Order);
                if (!balance.HasValue)
                {
                    return Json(new { result = false, message = Translation.GetTerm("GiftCardNotFound", "Gift card not found") });
                }
                else
                {
                    return Json(new { result = true, balance = balance.Value.ToString(OrderContext.Order.CurrencyID), amountToApply = Math.Min(balance.Value, ((Order)OrderContext.Order).Balance ?? 0m) });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (CoreContext.CurrentOrder != null) ? CoreContext.CurrentOrder.OrderID.ToIntNullable() : null, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [HttpPost]
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders", "~/")]
        public virtual ActionResult LoadBillingProfileForEdit(int accountPaymentMethodID)
        {
            try
            {
                var account = CoreContext.CurrentAccount;
                if (accountPaymentMethodID > 0 && account.AccountPaymentMethods.Any(pm => pm.AccountPaymentMethodID == accountPaymentMethodID))
                {
                    var paymentMethod = account.AccountPaymentMethods.First(pm => pm.AccountPaymentMethodID == accountPaymentMethodID);

                    switch (paymentMethod.PaymentTypeID)
                    {
                        case (int)Constants.PaymentType.EFT:
                            return Json(new
                            {
                                result = true,
                                nameOnAccount = paymentMethod.NameOnCard,
                                bankNum = paymentMethod.MaskedAccountNumber,
                                routingNum = paymentMethod.RoutingNumber,
                                actType = paymentMethod.BankAccountTypeID,
                                profileName = paymentMethod.ProfileName
                            });
                        case (int)Constants.PaymentType.CreditCard:
                            return Json(new
                            {
                                result = true,
                                nameoncard = paymentMethod.NameOnCard,
                                cardnum = paymentMethod.MaskedAccountNumber,
                                expmonth = paymentMethod.ExpirationDate.HasValue ? paymentMethod.ExpirationDate.Value.Month : DateTime.Now.Month,
                                expyear = paymentMethod.ExpirationDate.HasValue ? paymentMethod.ExpirationDate.Value.Year : DateTime.Now.Year,
                                zip = paymentMethod.BillingAddress != null ? paymentMethod.BillingAddress.PostalCode ?? string.Empty : string.Empty,
                                addresLine = paymentMethod.BillingAddress.Address1 != null ? paymentMethod.BillingAddress.Address1 : string.Empty,
                                postalCode = paymentMethod.BillingAddress.PostalCode != null ? paymentMethod.BillingAddress.PostalCode : string.Empty,
                                city = paymentMethod.BillingAddress.City != null ? paymentMethod.BillingAddress.City : string.Empty,
                                state = paymentMethod.BillingAddress.State != null ? paymentMethod.BillingAddress.State : string.Empty,
                                profileName = paymentMethod.ProfileName
                            });
                        default:
                            return Json(new
                            {
                                result = false,
                                message = Translation.GetTerm("PaymentTypeOnProfileNotEdiableHere", "The payment type on this profile is not editable here.")
                            });
                    }
                }
                else
                {
                    return Json(new { result = false, message = Translation.GetTerm("BillingProfileNotFound", "Billing profile not found") });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (CoreContext.CurrentOrder != null) ? CoreContext.CurrentOrder.OrderID.ToIntNullable() : null, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [HttpPost]
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders", "~/")]
        public ActionResult SaveCCBillingProfile(PaymentTypeModel paymentTypeModel)
        {
            try
            {
                var account = CoreContext.CurrentAccount;
                if (paymentTypeModel.PaymentMethodID > 0 && account.AccountPaymentMethods.Any(pm => pm.AccountPaymentMethodID == paymentTypeModel.PaymentMethodID))
                {
                    var paymentMethod = account.AccountPaymentMethods.First(pm => pm.AccountPaymentMethodID == paymentTypeModel.PaymentMethodID);
                    if (paymentMethod.PaymentTypeID != (int)Constants.PaymentType.CreditCard)
                    {
                        return Json(new { result = false, message = Translation.GetTerm("PaymentTypeOnProfileNotEdiableHere", "The payment type on this profile is not editable here.") });
                    }

                    paymentMethod.NameOnCard = paymentTypeModel.NameOnCard;
                    paymentMethod.DecryptedAccountNumber = paymentTypeModel.AccountNumber.RemoveNonNumericCharacters();
                    paymentMethod.ExpirationDateUTC = paymentTypeModel.ExpirationDate.Value.LastDayOfMonth();
                    paymentMethod.ProfileName = paymentTypeModel.ProfileName;

                    var billingAddress = paymentMethod.BillingAddress;
                    if (billingAddress == null)
                    {
                        billingAddress = new Address()
                        {
                            AddressTypeID = Constants.AddressType.Billing.ToShort()
                        };
                        billingAddress.State = paymentTypeModel.State;
                        billingAddress.City = paymentTypeModel.City;
                        billingAddress.Address1 = paymentTypeModel.AddressLine;
                        billingAddress.PostalCode = paymentTypeModel.PostalCode;
                        billingAddress.StartEntityTracking();
                        paymentMethod.BillingAddress = billingAddress;
                        account.Addresses.Add(billingAddress);
                    }
                    else
                    {
                        billingAddress.StartEntityTracking();
                    }

                    billingAddress.AttachAddressChangedCheck();
                    billingAddress.ProfileName = paymentTypeModel.ProfileName;
                    if (billingAddress.PostalCode != paymentTypeModel.PostalCode)
                    {
                        var shipment = ((Order)OrderContext.Order).OrderCustomers.Any(oc => oc.OrderShipments.Any())
                                       ? ((Order)OrderContext.Order).OrderCustomers.First().OrderShipments.First()
                                       : ((Order)OrderContext.Order).GetDefaultShipment();
                        billingAddress.PostalCode = paymentTypeModel.PostalCode;
                        billingAddress.CountryID = shipment != null ? shipment.CountryID : DefaultCountry.CountryID;
                        billingAddress.AddressTypeID = Constants.AddressType.Billing.ToShort();
                        billingAddress.Address1 = paymentTypeModel.AddressLine;
                        billingAddress.Address2 = string.Empty;
                        billingAddress.City = paymentTypeModel.City;
                        billingAddress.State = paymentTypeModel.State;
                    }

                    account.Save();

                    return Json(new
                    {
                        result = true,
                        billingProfilesGrid = RenderRazorPartialViewToString("BillingProfilesGrid", CoreContext.CurrentAccount)
                    });
                }
                else
                {
                    return Json(new { result = false, message = Translation.GetTerm("BillingProfileNotFound", "Billing profile not found") });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (CoreContext.CurrentOrder != null) ? CoreContext.CurrentOrder.OrderID.ToIntNullable() : null, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [HttpPost]
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders", "~/")]
        public ActionResult SaveBankBillingProfile(PaymentTypeModel paymentTypeModel)
        {
            try
            {
                var account = CoreContext.CurrentAccount;
                if (paymentTypeModel.PaymentMethodID > 0 && account.AccountPaymentMethods.Any(pm => pm.AccountPaymentMethodID == paymentTypeModel.PaymentMethodID))
                {
                    var paymentMethod = account.AccountPaymentMethods.First(pm => pm.AccountPaymentMethodID == paymentTypeModel.PaymentMethodID);
                    if (paymentMethod.PaymentTypeID != (int)Constants.PaymentType.EFT)
                    {
                        return Json(new { result = false, message = Translation.GetTerm("PaymentTypeOnProfileNotEdiableHere", "The payment type on this profile is not editable here.") });
                    }

                    paymentMethod.BankName = paymentTypeModel.BankName;
                    paymentMethod.BankAccountTypeID = paymentTypeModel.BankAccountTypeID;
                    paymentMethod.ProfileName = paymentTypeModel.ProfileName;
                    paymentMethod.NameOnCard = paymentTypeModel.NameOnAccount;
                    paymentMethod.DecryptedAccountNumber = paymentTypeModel.BankAccountNumber.RemoveNonNumericCharacters();
                    paymentMethod.RoutingNumber = paymentTypeModel.RoutingNumber;
                    paymentMethod.PaymentTypeID = (int)Constants.PaymentType.EFT;

                    account.Save();

                    return Json(new
                    {
                        result = true,
                        billingProfilesGrid = RenderRazorPartialViewToString("BillingProfilesGrid", CoreContext.CurrentAccount)
                    });
                }
                else
                {
                    return Json(new { result = false, message = Translation.GetTerm("BillingProfileNotFound", "Billing profile not found") });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (CoreContext.CurrentOrder != null) ? CoreContext.CurrentOrder.OrderID.ToIntNullable() : null, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        [FunctionFilter("Orders", "~/")]
        public virtual ActionResult Confirmation()
        {
            // This update shouldn't be necessary, an update was just made when applying the payment.
            //OrderService.UpdateOrder(OrderContext);
            if (OrderContext.Order.AsOrder().Balance != 0 && OrderContext.Order.OrderCustomers.Count > 0 && OrderContext.Order.AsOrder().OrderCustomers[0].OrderPayments.Count > 0)
            {
                return RedirectToAction("Billing");
            }
            ViewBag.CartModel = GetCartModelData(OrderContext.Order.AsOrder());

            return View(OrderContext.Order.AsOrder());
        }

        [FunctionFilter("Orders", "~/")]
        public virtual ActionResult Submit()
        {
            try
            {
                //Make sure this gets reset just incase they we not logged in before the order was created or if they changed users mid ordering process.
                if (!(OrderContext.Order.OrderTypeID == (short)Constants.OrderType.OverrideOrder && OrderContext.Order.AsOrder().ConsultantID > 0))
                    OrderContext.Order.AsOrder().SetConsultantID(Account, SiteOwner);

                var response = ValidateOrderItems();

                if (response.Success)
                {
                    OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;
                    response = OrderService.SubmitOrder(OrderContext);

                    //Insertar los precios para los componentes de kits en la orden
                    OrderExtensions.InsertKitItemPrices(OrderContext.Order.AsOrder().OrderID);
                    
                    var scheduler = Create.New<IEventScheduler>();
                    scheduler.ScheduleOrderCompletionEvents(OrderContext.Order.AsOrder().OrderID);

                    var newAccount = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
                    CoreContext.CurrentAccount = newAccount;
                    var siteOwner = SiteOwner;
                    this.IncrementConsultantsLeadCount(siteOwner == null ? 0 : siteOwner.AccountID);
                }

                return Json(new { result = response.Success, message = response.Message, paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order), newOrderId = OrderContext.Order.OrderID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage, paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order) });
            }
        }

        protected virtual BasicResponse ValidateOrderItems()
        {
            var response = OrderContext.Order.AsOrder().ValidateOrderItemsByStoreFront(OrderContext.Order.AsOrder());

            return response;
        }

        [FunctionFilter("Orders", "~/")]
        public virtual ActionResult Receipt(int orderId)
        {
            CheckoutReceiptModel model = new CheckoutReceiptModel();

            model.ContinueShopping = true;
            var order = OrderService.Load(orderId);
            if (CoreContext.CurrentAccount == null || ((Order)order).OrderCustomers.Where(x => x.AccountID == CoreContext.CurrentAccount.AccountID).Count() == 0)
            {
                return RedirectToAction("Index", "Shop");
            }

            model.CartModel = GetCartModelData(order.AsOrder());
            model.Order = order.AsOrder();

            return View("Receipt", "_Checkout", model);
        }


        public virtual PartialViewResult GetBillingTotals()
        {
            return PartialView("_BillingTotals", OrderContext.Order.AsOrder());
        }

        protected override bool ShouldCreateNewOrders
        {
            get
            {
                return false;
            }
        }

        [FunctionFilter("Orders", "~/")]
        public virtual JsonResult GetPickupPoints(string postalCode, string city)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(postalCode))
                    return Json(new { result = false, message = Translation.GetTerm("PickupPointsError_Requires_PostalCode", "Postal code is required.") });
                if (string.IsNullOrWhiteSpace(city))
                    return Json(new { result = false, message = Translation.GetTerm("PickupPointsError_Requires_City", "City is required.") });

                var pupService = Create.New<IPickupPointService>();
                var pickupPoints = pupService.GetPickupPoints(CurrentCulture, postalCode, city)
                    .Select(x => new
                    {
                        id = x.PickupPointID,
                        name = x.PickupPointAddress.Address1,
                        location = string.Format("{0} {1} {2} {3}", x.PickupPointAddress.Address2, x.PickupPointAddress.Address3, x.PickupPointAddress.City, x.PickupPointAddress.PostalCode),
                        distance = Translation.GetTerm("unknown")
                    });
                return Json(new { result = true, points = pickupPoints });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        #region Utilities
        //protected IEnrollmentContext<EnrollmentKitConfig> _enrollmentContext { get; private set; }
        public virtual ActionResult LookupZip(int countryId, string zip)
        {
            try
            {
                var postalCodeLookup = AddressConfiguration.GetISO(SmallCollectionCache.Instance.Countries.GetById(countryId).CountryCode).PostalCodeLookup;

                if (zip.Length == Convert.ToInt32(postalCodeLookup.Size) || zip.Length == Convert.ToInt32(postalCodeLookup.SizeSearch))
                {
                    string zipPlusFour = zip.Substring(5);
                    zip = zip.Substring(0, 5);

                    var account = CoreContext.CurrentAccount;
                    int addressID = 0;
                    if (account == null)
                    {
                        var enrollmentContext = Create.New<IEnrollmentContextProvider>().GetEnrollmentContext();
                        if (enrollmentContext != null)
                            account = (Account)enrollmentContext.EnrollingAccount;
                    }

                    if (account != null)
                    {
                        if (account.Addresses != null)
                        {
                            if (account.Addresses.Count() > 0)
                            {
                                Address direccionPrincpial = account.Addresses.Where(donde => donde.AddressTypeID == (int)Constants.AddressType.Main).FirstOrDefault();
                                if (direccionPrincpial != null)
                                    addressID = direccionPrincpial.AddressID;
                            }
                        }
 
                    }

                    return Json(Create.New<IPostalCodeLookupProvider>()
                        .LookupPostalCodeByAccount(countryId, string.Format("{0}-{1}", zip, zipPlusFour), (account == null ? 0 : account.AccountID), addressID)
                                    .Select(r => new
                                    {
                                        city = r.City.ToTitleCase().Trim(),
                                        county = r.County.ToTitleCase().Trim(),
                                        stateId = r.StateID,
                                        state = r.StateAbbreviation.Trim(),
                                        street = r.Street,
                                        EditaCounty = AddressConfiguration.GetISO(SmallCollectionCache.Instance.Countries.GetById(countryId).CountryCode).Tags.Where(t => t.Field == "Street").FirstOrDefault() == null ? false : r.EditaCounty,
                                        EditaStreet = r.EditaStreet
                                    }).Distinct());
                }
                return Json(new List<NetSteps.Common.Globalization.PostalCodeData>());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult GetAddressControl(int countryId, int? addressId, string prefix, List<string> excludeFields)
        {
            try
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Address address = addressId.HasValue && addressId.Value > 0 ? Address.Load(addressId.Value) : new Address();
                AddressModel model = new AddressModel()
                {
                    Address = address,
                    LanguageID = CurrentLanguageID,
                    Country = SmallCollectionCache.Instance.Countries.First(c => c.CountryID == countryId),
                    Prefix = prefix,
                    ExcludeFields = excludeFields
                };

                return PartialView("Address", model);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        protected virtual object GetTotals()
        {
            return new
            {
                orderBalanceDue = OrderContext.Order.AsOrder().Balance.AsymmetricRoundedNumber().ToString(OrderContext.Order.CurrencyID)
            };
        }
        #endregion
    }
}
