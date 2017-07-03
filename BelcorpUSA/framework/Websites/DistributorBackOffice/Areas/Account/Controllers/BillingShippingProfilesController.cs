using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DistributorBackOffice.Areas.Account.Models.BillingShippingProfiles;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Web.Mvc.Controls.Configuration;

namespace DistributorBackOffice.Areas.Account.Controllers
{
    public class BillingShippingProfilesController : BaseAccountsController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult LookupZip(int countryId, string zip)
        {
            try
            {
                var postalCodeLookup = AddressConfiguration.GetISO(SmallCollectionCache.Instance.Countries.GetById(countryId).CountryCode).PostalCodeLookup;

                if (zip.Length == Convert.ToInt32(postalCodeLookup.Size) || zip.Length == Convert.ToInt32(postalCodeLookup.SizeSearch))
                {
                    var results = TaxCacheExtensions.PostalLookUpByAccountID(countryId, zip, (CurrentAccount == null ? 0 : CurrentAccount.AccountID))
                                    .Select(r => new
                                    {
                                        city = r.City,
                                        county = r.County,
                                        stateId = r.StateID,
                                        state = r.StateAbbreviation,
                                        street = r.Street,
                                        EditaCounty = AddressConfiguration.GetISO(SmallCollectionCache.Instance.Countries.GetById(countryId).CountryCode).Tags.Where(t => t.Field == "Street").FirstOrDefault() == null ? false : r.EditaCounty,
                                        EditaStreet = r.EditaStreet
                                    }).Distinct();
                    return Json(results);
                }
                return Json(new List<NetSteps.Common.Globalization.PostalCodeData>());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetAddressControl(int? countryId, int? addressId, string prefix, List<string> excludeFields, bool showCountrySelect = true)
        {
            try
            {
                Address address = addressId.HasValue && addressId.Value > 0 ? Address.Load(addressId.Value) : new Address();
                AddressModel model = new AddressModel()
                {
                    Address = address,
                    LanguageID = CoreContext.CurrentLanguageID,
                    ShowCountrySelect = showCountrySelect,
                    ChangeCountryURL = "~/Accounts/BillingShippingProfiles/GetAddressControl",
                    ExcludeFields = excludeFields,
                    Prefix = prefix
                };

                if (countryId.HasValue && countryId.Value > 0)
                    model.Country = SmallCollectionCache.Instance.Countries.First(c => c.CountryID == countryId);
                else if (addressId.HasValue && address.CountryID > 0)
                    model.Country = SmallCollectionCache.Instance.Countries.First(c => c.CountryID == address.CountryID);

                return PartialView("Address", model);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        #region Shipping Addresses
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult AddressModal(int? addressId)
        {
            try
            {
                ViewData["Countries"] = SmallCollectionCache.Instance.Countries;
                return PartialView(addressId.HasValue ? CurrentAccount.Addresses.GetByAddressID(addressId.Value) : new Address());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult AddressModalCustom(int? addressId)
        {
            try
            {
                ViewData["Countries"] = SmallCollectionCache.Instance.Countries;
                var address = addressId.HasValue ? CurrentAccount.Addresses.GetByAddressID(addressId.Value) : new Address();
                return PartialView("AddressModalCustom", address);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetAddresses()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;
                foreach (Address address in CurrentAccount.Addresses.Where(a => a.AddressTypeID == NetSteps.Data.Entities.Constants.AddressType.Shipping.ToInt()))
                {
                    builder.Append("<div class=\"Profile").Append(count % 2 == 1 ? " Alt" : "").Append("\"><span class=\"FR\"><a href=\"javascript:void(0);\" onclick=\"setDefaultAddress(")
                        .Append(address.AddressID).Append(")\"").Append(address.IsDefault ? "style=\"display:none;\"" : "").Append(">Set As Default Address</a><span style=\"margin-left: 5px; margin-right: 5px;\">")
                        .Append(address.IsDefault ? "" : "|").Append("</span><a href=\"javascript:void(0);\" class=\"deletePaymentMethod\" onclick=\"deleteAddress(")
                        .Append(address.AddressID).Append(")\">Delete Address</a></span><div><b><a title=\"Edit\" style=\"cursor: pointer;\" onclick=\"editAddress(")
                        .Append(address.AddressID).Append(");\">").Append(string.IsNullOrEmpty(address.ProfileName) ? "Edit" : address.ProfileName).Append("</a></b><span class=\"isDefault\">")
                        .Append(address.IsDefault ? " (default)" : "").Append("</span><br />").Append(address.ToString().ToHtmlBreaks())
                        .Append("</div></div>");
                    ++count;
                }
                return Content(builder.ToString());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult SetDefaultAddress(int addressId)
        {
            try
            {
                NetSteps.Data.Entities.Account account = CurrentAccount;
                account.StartEntityTracking();
                Address address = account.Addresses.FirstOrDefault(a => a.AddressID == addressId);
                if (address == null)
                    throw EntityExceptionHelper.GetAndLogNetStepsException("That address is not associated with the current account.");
                if (!address.IsDefault)
                {
                    address.IsDefault = true;
                }
                // Set the in memory objects, since we won't reload them after this operation
                foreach (Address add in account.Addresses.GetAllByTypeID((NetSteps.Data.Entities.Constants.AddressType)address.AddressTypeID).Where(a => a.IsDefault && a.AddressID != addressId))
                {
                    add.IsDefault = false;
                }
                account.Save();
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public Address obtainAddress(NetSteps.Data.Entities.Account account, int? addressId)
        {
            Address address = null;
            account.StartEntityTracking();
            if (addressId.HasValue && addressId > 0)
            {
                address = account.Addresses.GetByAddressID(addressId.Value);
            }
            else
            {
                address = new Address();
                account.Addresses.Add(address);
            }
            address.StartEntityTracking();
            return address;
        }

        public void updateAddress(Address address, AddressViewModel model)
        {
            address.AttachAddressChangedCheck();
            address.ProfileName = model.ProfileName.ToCleanString();
            address.Attention = model.Attention.ToCleanString();
            address.Address1 = model.Address1.ToCleanString();
            address.Address2 = model.Address2.ToCleanString();
            address.Address3 = model.Address3.ToCleanString();
            address.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Shipping.ToShort();
            address.City = model.City.ToCleanString();
            //if (model.State.IsValidInt())
            //{
            //    int spid = model.State.ToInt();
            //    address.StateProvinceID = (spid > 0) ? spid : (int?)null;
            //    address.State = (spid > 0) ? SmallCollectionCache.Instance.StateProvinces.GetById(spid).StateAbbreviation : model.State.ToCleanString();
            //}
            //else if (!String.IsNullOrWhiteSpace(model.State))
            //{
            //    string st = model.State.ToCleanString();
            //    var stprov = SmallCollectionCache.Instance.StateProvinces.Where(sp => sp.StateAbbreviation.Equals(st, StringComparison.InvariantCultureIgnoreCase) && sp.CountryID == model.CountryId).FirstOrDefault();
            //    if (stprov != null)
            //    {
            //        address.StateProvinceID = stprov.StateProvinceID;
            //        address.State = stprov.StateAbbreviation;
            //    }
            //}
            //address.State = (model.State.IsValidInt() && model.State.ToInt() > 0) ? SmallCollectionCache.Instance.StateProvinces.GetById(model.State.ToInt()).StateAbbreviation : model.State.ToCleanString();
            address.SetState(model.State, model.CountryId);
            address.County = model.County.ToCleanString();
            address.PostalCode = model.PostalCode.ToCleanString();
            address.CountryID = model.CountryId;
            address.PhoneNumber = model.Phone.ToCleanString();
            address.LookUpAndSetGeoCode();
            address.SetEmailAddress(model.Email);
        }

        public virtual ActionResult SaveAddress(AddressViewModel model)
        {
            try
            {
                //NetSteps.Data.Entities.Account account = CurrentAccount;
                NetSteps.Data.Entities.Account account = NetSteps.Data.Entities.Account.LoadForSession(CurrentAccount.AccountID);

                Address address = obtainAddress(account, model.AddressId);
                updateAddress(address, model);
                address.Validate();
                if (!address.IsValid)
                {
                    ReloadCurrentAccount();
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

                UpdateAddressStreet(account, model.Street, address.AddressID);


                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private void UpdateAddressStreet(NetSteps.Data.Entities.Account account, string street, int addressID)
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

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult DeleteAddress(int addressId)
        {
            try
            {
                var account = CurrentAccount;
                if (account.Addresses.Select(a => a.AddressID).Contains(addressId))
                {
                    account.StartEntityTracking();
                    var address = account.Addresses.First(a => a.AddressID == addressId);
                    if (address.ChangeTracker.State != ObjectState.Added)
                        address.MarkAsDeleted();
                    account.Addresses.Remove(address);
                    account.Save();
                    return Json(new { result = true });
                }
                else
                    return Json(new { result = false, message = Translation.GetTerm("AddressNotFoundOnCurrentAccount", "Address not found on current Account.") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        #region Payment Methods
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult PaymentMethodModal(int? paymentMethodId)
        {
            try
            {
                ViewData["Countries"] = SmallCollectionCache.Instance.Countries;

                AccountPaymentMethod accountPaymentMethod = null;
                if (paymentMethodId.HasValue && paymentMethodId.Value > 0)
                    accountPaymentMethod = CurrentAccount.AccountPaymentMethods.GetByAccountPaymentMethodID(paymentMethodId.Value);
                else
                    accountPaymentMethod = new AccountPaymentMethod() { BillingAddress = new Address() };

                if (accountPaymentMethod.BillingAddress == null)
                    accountPaymentMethod.BillingAddress = new Address()
                    {
                        AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Billing.ToShort()
                    };

                return PartialView(accountPaymentMethod);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult PaymentMethodModalCustom(int? paymentMethodId)
        {
            try
            {
                ViewData["Countries"] = SmallCollectionCache.Instance.Countries;

                AccountPaymentMethod accountPaymentMethod = null;
                if (paymentMethodId.HasValue && paymentMethodId.Value > 0)
                    accountPaymentMethod = CurrentAccount.AccountPaymentMethods.GetByAccountPaymentMethodID(paymentMethodId.Value);
                else
                    accountPaymentMethod = new AccountPaymentMethod() { BillingAddress = new Address() };

                if (accountPaymentMethod.BillingAddress == null)
                    accountPaymentMethod.BillingAddress = new Address()
                    {
                        AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Billing.ToShort()
                    };

                return PartialView(accountPaymentMethod);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetPaymentMethods()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;
                foreach (AccountPaymentMethod paymentMethod in CurrentAccount.AccountPaymentMethods)
                {
                    builder.Append("<div class=\"Profile").Append(count % 2 == 1 ? " Alt" : "").Append("\"><span class=\"FR\"><a href=\"javascript:void(0);\" class=\"defaultPaymentMethod\" onclick=\"setDefaultPaymentMethod(")
                        .Append(paymentMethod.AccountPaymentMethodID).Append(")\"").Append(paymentMethod.IsDefault ? "style=\"display:none;\"" : "").Append(">Set As Default Payment Method</a><span style=\"margin-left: 5px; margin-right: 5px;\">")
                        .Append(paymentMethod.IsDefault ? "" : "|").Append("</span><a href=\"javascript:void(0);\" class=\"deletePaymentMethod\" onclick=\"deletePaymentMethod(")
                        .Append(paymentMethod.AccountPaymentMethodID).Append(")\">Delete Payment Method</a></span><div><b><a title=\"Edit\" style=\"cursor: pointer;\" onclick=\"editPaymentMethod(")
                        .Append(paymentMethod.AccountPaymentMethodID).Append(");\">").Append(string.IsNullOrEmpty(paymentMethod.ProfileName) ? "Edit" : paymentMethod.ProfileName).Append("</a></b><span class=\"isDefault\">")
                        .Append(paymentMethod.IsDefault ? " (default)" : "").Append("</span><br />").Append(paymentMethod.DecryptedAccountNumber).Append("<br />").Append(paymentMethod.FormatedExpirationDate)
                        .Append("<br /></div></div>");
                    ++count;
                }
                return Content(builder.ToString());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        //[OutputCache(CacheProfile = "DontCache")]
        //public virtual ActionResult GetAddress(int? addressId)
        //{
        //    try
        //    {
        //        ViewData["Countries"] = SmallCollectionCache.Instance.Countries;
        //        return PartialView("Address", addressId.HasValue ? CurrentAccount.Addresses.GetByAddressID(addressId.Value) : new Address());
        //    }
        //    catch (Exception ex)
        //    {
        //        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
        //        throw exception;
        //    }
        //}

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult SetDefaultPaymentMethod(int paymentMethodId)
        {
            try
            {
                var account = CurrentAccount;
                account.StartEntityTracking();

                AccountPaymentMethod paymentMethod = account.AccountPaymentMethods.Cast<AccountPaymentMethod>().FirstOrDefault(pm => pm.AccountPaymentMethodID == paymentMethodId);
                if (paymentMethod == null)
                    throw EntityExceptionHelper.GetAndLogNetStepsException("That payment method is not associated with the current account.");
                if (!paymentMethod.IsDefault)
                    paymentMethod.IsDefault = true;
                foreach (AccountPaymentMethod pm in account.AccountPaymentMethods)
                {
                    if (pm.IsDefault && pm.AccountPaymentMethodID != paymentMethodId)
                        pm.IsDefault = false;
                }

                account.Save();

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        protected internal virtual AccountPaymentMethod obtainPaymentMethod(NetSteps.Data.Entities.Account account, int? paymentMethodId)
        {

            if (paymentMethodId.HasValue && paymentMethodId.Value > 0)
            {
                return account.AccountPaymentMethods.First(a => a.AccountPaymentMethodID == paymentMethodId.Value);
            }
            else
            {
                AccountPaymentMethod paymentMethod = new AccountPaymentMethod();
                account.AccountPaymentMethods.Add(paymentMethod);
                return paymentMethod;
            }
        }

        protected internal virtual void updatePaymentMethod(AccountPaymentMethod paymentMethod, Constants.PaymentType paymentType,
            string decryptedAccountNumber, string profileName, string nameOnAccount, string routingNumber, short bankAccountType)
        {
            paymentMethod.PaymentTypeID = (int)paymentType;
            paymentMethod.DecryptedAccountNumber = decryptedAccountNumber;
            paymentMethod.ProfileName = profileName;
            paymentMethod.NameOnCard = nameOnAccount;
            paymentMethod.RoutingNumber = routingNumber;
            paymentMethod.BankAccountTypeID = bankAccountType;
        }

        //This is used for saving bank accounts as a payment method.
        [HttpPost]
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult SavePaymentMethodEFT(int? paymentMethodId, string nameOnAccount, string bankName, string routingInput, string accountInput,
            string accountType, string profileName, string attention, string address1, string address2, string address3, string zip, string city,
            string county, string state, string street, int countryId, string phone, int? addressId)
        {
            try
            {
                //NetSteps.Data.Entities.Account account = CurrentAccount;
                NetSteps.Data.Entities.Account account = NetSteps.Data.Entities.Account.LoadForSession(CurrentAccount.AccountID);
                account.StartEntityTracking();

                #region Obtain PaymentMethod
                AccountPaymentMethod paymentMethod = obtainPaymentMethod(account, paymentMethodId);

                paymentMethod.StartEntityTracking();
                paymentMethod.BankName = bankName;
                string decryptedAccountNumber = String.Empty;
                if (!accountInput.Contains("*"))
                    decryptedAccountNumber = accountInput.RemoveNonNumericCharacters();

                updatePaymentMethod(paymentMethod,
                                    Constants.PaymentType.EFT,
                                    decryptedAccountNumber,
                                    profileName,
                                    nameOnAccount.ToCleanString(),
                                    routingInput.ToCleanString(),
                                    short.Parse(accountType)
                                    );
                #endregion

                #region Update Payment Address
                //TODO: update address code here to use separated calls (see SaveAddress) to facilitate unit testing.

                Address billingAddress = null;
                if (addressId.HasValue && addressId.Value > 0)
                {
                    billingAddress = account.Addresses.GetByAddressID(addressId.Value);
                }
                else
                {
                    billingAddress = new Address();
                    account.Addresses.Add(billingAddress);

                    if (paymentMethod.BillingAddress != null && paymentMethod.BillingAddress.AddressID == 0)
                        paymentMethod.BillingAddress = null;
                }

                billingAddress.AttachAddressChangedCheck();
                billingAddress.ProfileName = profileName.ToCleanString();
                billingAddress.Attention = attention.ToCleanString();
                billingAddress.Address1 = address1.ToCleanString();
                billingAddress.Address2 = address2.ToCleanString();
                billingAddress.Address3 = address3.ToCleanString();
                billingAddress.City = city.ToCleanString();
                billingAddress.County = county.ToCleanString();
                billingAddress.SetState(state, countryId);
                //billingAddress.StateProvinceID = (state.IsValidInt() && state.ToInt() > 0) ? state.ToInt() : (int?)null;
                //billingAddress.State = (state.IsValidInt() && state.ToInt() > 0) ? SmallCollectionCache.Instance.StateProvinces.GetById(state.ToInt()).StateAbbreviation : state.ToCleanString();
                billingAddress.PostalCode = zip.ToCleanString();
                billingAddress.CountryID = countryId;
                billingAddress.PhoneNumber = phone.ToCleanString();
                billingAddress.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Billing.ToShort();
                billingAddress.Street = street;
                billingAddress.LookUpAndSetGeoCode();
                

                #endregion

                account.Save();

                paymentMethod.BillingAddress = billingAddress;
                paymentMethod.BillingAddressID = billingAddress.AddressID;

                account.Save();

                UpdateAddressStreet(account, street, billingAddress.AddressID);

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [HttpPost]
        public virtual ActionResult SavePaymentMethod(int? paymentMethodId, /*string accountName, */string nameOnCard, string accountNumber,
            DateTime expDate, string profileName, string attention, string address1, string address2, string address3, string zip,
            string city, string county, string state, string street, int countryId, string phone, int? addressId)
        {
            try
            {
                NetSteps.Data.Entities.Account account = NetSteps.Data.Entities.Account.LoadForSession(CurrentAccount.AccountID); ;
                account.StartEntityTracking();
                AccountPaymentMethod paymentMethod = null;
                if (paymentMethodId.HasValue && paymentMethodId.Value > 0)
                {
                    paymentMethod = account.AccountPaymentMethods.First(a => a.AccountPaymentMethodID == paymentMethodId.Value);
                }
                else
                {
                    paymentMethod = new AccountPaymentMethod();
                    account.AccountPaymentMethods.Add(paymentMethod);
                }
                paymentMethod.StartEntityTracking();

                paymentMethod.ProfileName = profileName.ToCleanString();
                paymentMethod.PaymentTypeID = (int)NetSteps.Data.Entities.Constants.PaymentType.CreditCard;
                if (!accountNumber.Contains("*"))
                    paymentMethod.DecryptedAccountNumber = accountNumber.RemoveNonNumericCharacters();
                paymentMethod.NameOnCard = nameOnCard.ToCleanString();
                paymentMethod.ExpirationDateUTC = expDate.LastDayOfMonth();

                Address billingAddress = null;
                if (addressId.HasValue && addressId.Value > 0)
                {
                    billingAddress = account.Addresses.GetByAddressID(addressId.Value);
                }
                else
                {
                    billingAddress = new Address();
                    account.Addresses.Add(billingAddress);

                    if (paymentMethod.BillingAddress != null && paymentMethod.BillingAddress.AddressID == 0)
                        paymentMethod.BillingAddress = null;
                }

                billingAddress.AttachAddressChangedCheck();
                billingAddress.ProfileName = profileName.ToCleanString();
                billingAddress.Attention = attention.ToCleanString();
                billingAddress.Address1 = address1.ToCleanString();
                billingAddress.Address2 = address2.ToCleanString();
                billingAddress.Address3 = address3.ToCleanString();
                billingAddress.City = city.ToCleanString();
                billingAddress.County = county.ToCleanString();
                billingAddress.SetState(state, countryId);
                //billingAddress.StateProvinceID = (state.IsValidInt() && state.ToInt() > 0) ? state.ToInt() : (int?)null;
                //billingAddress.State = (state.IsValidInt() && state.ToInt() > 0) ? SmallCollectionCache.Instance.StateProvinces.GetById(state.ToInt()).StateAbbreviation : state.ToCleanString();
                billingAddress.PostalCode = zip.ToCleanString();
                billingAddress.CountryID = countryId;
                billingAddress.PhoneNumber = phone.ToCleanString();
                billingAddress.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Billing.ToShort();
                billingAddress.Street = street;
                billingAddress.LookUpAndSetGeoCode();
                
                account.Save();

                paymentMethod.BillingAddressID = billingAddress.AddressID;

                account.Save();

                UpdateAddressStreet(account, street, billingAddress.AddressID);

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult DeletePaymentMethod(int paymentMethodId)
        {
            try
            {
                var account = CurrentAccount;
                if (account.AccountPaymentMethods.Select(a => a.AccountPaymentMethodID).Contains(paymentMethodId))
                {
                    account.DeleteAccountPaymentMethod(paymentMethodId);

                    return Json(new { result = true });
                }
                else
                    return Json(new { result = false, message = Translation.GetTerm("PaymentmethodNotFoundOnCurrentAccount", "PaymentMethod not found on current Account.") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        public virtual ActionResult EditAddress(int? addressId)
        {
            try
            {
                ViewData["Countries"] = SmallCollectionCache.Instance.Countries;
                var address = addressId.HasValue && addressId.Value > 0 ? CurrentAccount.Addresses.GetByAddressID(addressId.Value) : new Address();
                return View(address);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public virtual ActionResult EditPayment(int? paymentMethodId)
        {
            try
            {
                ViewData["Countries"] = SmallCollectionCache.Instance.Countries;

                AccountPaymentMethod accountPaymentMethod = null;
                if (paymentMethodId.HasValue && paymentMethodId.Value > 0)
                    accountPaymentMethod = CurrentAccount.AccountPaymentMethods.GetByAccountPaymentMethodID(paymentMethodId.Value);
                else
                    accountPaymentMethod = new AccountPaymentMethod() { BillingAddress = new Address() };

                if (accountPaymentMethod.BillingAddress == null)
                    accountPaymentMethod.BillingAddress = new Address()
                    {
                        AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Billing.ToShort()
                    };

                return View(accountPaymentMethod);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }
    }
}