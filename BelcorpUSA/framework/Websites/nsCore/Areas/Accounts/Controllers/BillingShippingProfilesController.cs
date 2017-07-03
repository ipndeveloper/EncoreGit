using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Accounts.Models.BillingShippingProfiles;
using NetSteps.Web.Mvc.Controls.Configuration;
using NetSteps.Data.Entities.Business.Logic;
using System.Text.RegularExpressions;

namespace nsCore.Areas.Accounts.Controllers
{
    public class BillingShippingProfilesController : BaseAccountsController
    {
        [FunctionFilter("Accounts-Billing and Shipping Profiles", "~/Accounts/Overview")]
        public virtual ActionResult Index(string id)
        {
            var account = CurrentAccount;
            this.AccountNum = id ??
                                    (account != null ? account.AccountNumber : string.Empty);
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult LookupZip(int countryId, string zip, string addressId) //GR-4602 se agrega el valor addressId para que tome valor correcto de dirección
        {
            try
            {
                var postalCodeLookup = AddressConfiguration.GetISO(SmallCollectionCache.Instance.Countries.GetById(countryId).CountryCode).PostalCodeLookup;

                if (zip.Length == Convert.ToInt32(postalCodeLookup.Size) || zip.Length == Convert.ToInt32(postalCodeLookup.SizeSearch))
                {
                    Account account = CoreContext.CurrentAccount;
                    var results = TaxCacheExtensions.PostalLookUpByAccountID(countryId, zip, (account == null ? 0 : account.AccountID), addressId.ToInt())
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
        public virtual ActionResult GetAddressControl(int? countryId, int? addressId, string prefix, List<string> excludeFields,
            bool showCountrySelect = true)
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

                if (countryId != null)
                    model.Country = SmallCollectionCache.Instance.Countries.First(c => c.CountryID == countryId);
                else if (addressId != null)
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
                //GR-4602 Se agrega para el manejo de caracteres especiales
                Address addr = addressId.HasValue ? CoreContext.CurrentAccount.Addresses.GetByAddressID(addressId.Value) : new Address();
                if (addr.County != null)
                {
                    addr.County = Regex.Replace(addr.County, @"\'?[^\w\.@-]", " ");
                }

                return PartialView(addr);
                //return PartialView(addressId.HasValue ? CoreContext.CurrentAccount.Addresses.GetByAddressID(addressId.Value) : new Address());

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
                foreach (Address address in CoreContext.CurrentAccount.Addresses.Where(a => a.AddressTypeID == NetSteps.Data.Entities.Constants.AddressType.Shipping.ToInt()))
                {
                    builder.Append("<div class=\"Profile").Append(address.IsDefault ? " DefaultProfile" : "").Append("\"><div class=\"FR ProfileControls\"><a href=\"javascript:void(0);\" class=\"SetDefault\" title=\"").Append(Translation.GetTerm("MakeDefault", "Make Default")).Append("\" onclick=\"setDefaultAddress(")
                        .Append(address.AddressID).Append(")\"").Append(address.IsDefault ? "style=\"display:none;\"" : "").Append("><span>").Append(Translation.GetTerm("MakeDefault", "Make Default")).Append("</span></a>")
                        .Append("<a href=\"javascript:void(0);\" class=\"DTL Remove\" onclick=\"deleteAddress(")
                        .Append(address.AddressID).Append(")\"><span>").Append(Translation.GetTerm("Delete")).Append("</span></a></div><div><b><a title=\"").Append(Translation.GetTerm("Edit")).Append("\" class=\"DTL Edit\" onclick=\"editAddress(")
                        .Append(address.AddressID).Append(");\">").Append(string.IsNullOrEmpty(address.ProfileName) ? Translation.GetTerm("Unnamed") : address.ProfileName).Append("</a></b><br />").Append(address.ToString().ToHtmlBreaks())
                        .Append("</div></div>");
                    ++count;
                }
                return Content(builder.ToString());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Accounts-Billing and Shipping Profiles", "~/Accounts/Overview")]
        public virtual ActionResult SetDefaultAddress(int addressId)
        {
            try
            {
                //Account account = CoreContext.CurrentAccount;
                Account account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
                account.StartEntityTracking();
                Address address = account.Addresses.FirstOrDefault(a => a.AddressID == addressId);
                if (address == null)
                    throw EntityExceptionHelper.GetAndLogNetStepsException("That address is not associated with the current account.");
                if (!address.IsDefault)
                {
                    //address.StartEntityTracking();
                    address.IsDefault = true;
                    //address.Save();
                }
                // Set the in memory objects, since we won't reload them after this operation
                foreach (Address add in account.Addresses.GetAllByTypeID((NetSteps.Data.Entities.Constants.AddressType)address.AddressTypeID).Where(a => a.IsDefault && a.AddressID != addressId))
                {
                    add.IsDefault = false;
                }
                account.Save();
                CoreContext.CurrentAccount = account;
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public Address obtainAddress(Account account, int? addressId)
        {
            Address address = null;
            account.StartEntityTracking();
            if (addressId.HasValue && addressId > 0)
            {
                //account = Account.LoadFull(CoreContext.CurrentAccount.AccountID);
                address = account.Addresses.GetByAddressID(addressId.Value);
            }
            else
            {
                address = new Address();
                //account = Account.LoadFull(CoreContext.CurrentAccount.AccountID);
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
            address.StateProvinceID = (model.State.IsValidInt() && model.State.ToInt() > 0) ? model.State.ToInt() : (int?)null;
            address.Street = (model.Street == null ? string.Empty : model.Street.ToCleanString());

            if (model.State.IsValidInt())
            {
                int spid = model.State.ToInt();
                address.StateProvinceID = (spid > 0) ? spid : (int?)null;
                address.State = (spid > 0) ? SmallCollectionCache.Instance.StateProvinces.GetById(spid).StateAbbreviation : model.State.ToCleanString();
            }
            else if (!String.IsNullOrWhiteSpace(model.State))
            {
                string st = model.State.ToCleanString();
                var stprov = SmallCollectionCache.Instance.StateProvinces.Where(sp => sp.StateAbbreviation.Equals(st, StringComparison.InvariantCultureIgnoreCase) && sp.CountryID == model.CountryId).FirstOrDefault();
                if (stprov != null)
                {
                    address.StateProvinceID = stprov.StateProvinceID;
                    address.State = stprov.StateAbbreviation;
                }
            }
            address.County = model.County.ToCleanString();
            address.PostalCode = model.PostalCode.ToCleanString();
            address.CountryID = model.CountryId;
            address.PhoneNumber = model.Phone.ToCleanString();
            address.LookUpAndSetGeoCode();
            address.SetEmailAddress(model.Email);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [FunctionFilter("Accounts-Billing and Shipping Profiles", "~/Accounts/Overview")]
        public virtual ActionResult SaveAddress(AddressViewModel model)
        {
            try
            {
                //NetSteps.Data.Entities.Account account = CoreContext.CurrentAccount;
                Account account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
                var originalAccount = account.Clone();

                Address address = obtainAddress(account, model.AddressId);
                updateAddress(address, model);
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

                account.Save();

                UpdateAddressStreet(account, model.Street, address.AddressID);

                new AddressBusinessLogic().UpdateAddressStreet(address);

                CoreContext.CurrentAccount = account;

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Billing and Shipping Profiles", "~/Accounts/Overview")]
        public virtual ActionResult DeleteAddress(int addressId)
        {
            try
            {
                var account = CoreContext.CurrentAccount;
                if (account.Addresses.Select(a => a.AddressID).Contains(addressId))
                {
                    bool IsDefault = account.Addresses.ToList().Where(x => x.IsDefault).Select(a => a.AddressID).Contains(addressId);
                    int cantAccShipp = account.Addresses.ToList().Where(x => x.AddressTypeID == Constants.AddressType.Shipping.ToInt()).ToList().Count();

                    if (IsDefault)
                    {
                        return Json(new { result = false, message = Translation.GetTerm("AddressIsDefaultCurrentAccount", "Address is default on current Account.") });
                    }
                    else if (cantAccShipp == 1)
                    {
                        return Json(new { result = false, message = Translation.GetTerm("AddressOnlyCurrentAccount", "Address is only on current Account.") });
                    }
                    else
                    {
                        account.DeleteAccountAddress(addressId);
                        CoreContext.CurrentAccount = account;
                        return Json(new { result = true });
                    }
                }
                else
                    return Json(new { result = false, message = Translation.GetTerm("AddressNotFoundOnCurrentAccount", "Address not found on current Account.") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
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
                    accountPaymentMethod = CoreContext.CurrentAccount.AccountPaymentMethods.GetByAccountPaymentMethodID(paymentMethodId.Value);
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
                foreach (AccountPaymentMethod paymentMethod in CoreContext.CurrentAccount.AccountPaymentMethods)
                {
                    builder.Append("<div class=\"Profile").Append(paymentMethod.IsDefault ? " DefaultProfile" : "").Append("\"><div class=\"FR ProfileControls\"><a href=\"javascript:void(0);\" class=\"DTL SetDefault\" title=\"")
                        .Append(Translation.GetTerm("MakeDefault", "Make Default")).Append("\" onclick=\"setDefaultPaymentMethod(").Append(paymentMethod.AccountPaymentMethodID)
                        .Append(");\"").Append(paymentMethod.IsDefault ? "style=\"display:none;\"" : "").Append("><span>").Append(Translation.GetTerm("MakeDefault", "Make Default"))
                        .Append("</span></a><a href=\"javascript:void(0);\" class=\"DTL Remove\" onclick=\"deletePaymentMethod(")
                        .Append(paymentMethod.AccountPaymentMethodID).Append(")\"><span>" + Translation.GetTerm("Delete") + "</span></a></div><div id=\"PaymentMethod").Append(paymentMethod.AccountPaymentMethodID).Append("\"><b><a title=\"").Append(Translation.GetTerm("Edit")).Append("\" class=\"DTL Edit\" onclick=\"editPaymentMethod(")
                        .Append(paymentMethod.AccountPaymentMethodID).Append(");\">").Append(string.IsNullOrEmpty(paymentMethod.ProfileName) ? Translation.GetTerm("Unnamed") : paymentMethod.ProfileName).Append("</a></b><br />").Append(paymentMethod.DecryptedAccountNumber.MaskString(4)).Append("<br />").Append(paymentMethod.ExpirationDate.ToExpirationStringDisplay(CoreContext.CurrentCultureInfo))
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
        //        return PartialView("Address", addressId.HasValue ? CoreContext.CurrentAccount.Addresses.GetByAddressID(addressId.Value) : new Address());
        //    }
        //    catch (Exception ex)
        //    {
        //        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
        //        throw exception;
        //    }
        //}

        [FunctionFilter("Accounts-Billing and Shipping Profiles", "~/Accounts/Overview")]
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult SetDefaultPaymentMethod(int paymentMethodId)
        {
            try
            {
                var account = CoreContext.CurrentAccount;
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
                CoreContext.CurrentAccount = account;

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        protected internal virtual AccountPaymentMethod obtainPaymentMethod(Account account, int? paymentMethodId)
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
            string decryptedAccountNumber, string profileName, string nameOnAccount, string routingNumber, short bankAccountTypeID)
        {
            paymentMethod.PaymentTypeID = (int)paymentType;
            paymentMethod.DecryptedAccountNumber = decryptedAccountNumber;
            paymentMethod.ProfileName = profileName;
            paymentMethod.NameOnCard = nameOnAccount;
            paymentMethod.RoutingNumber = routingNumber;
            paymentMethod.BankAccountTypeID = bankAccountTypeID;
        }

        //This is used for saving bank accounts as a payment method.
        [HttpPost]
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Billing and Shipping Profiles", "~/Accounts/Overview")]
        public virtual ActionResult SavePaymentMethodEFT(int? paymentMethodId, string nameOnAccount, string bankName, string routingInput, string accountInput,
            short bankAccountTypeID, string profileName, string attention, string address1, string address2, string address3, string zip, string city,
            string street, string state, int countryId, string phone, int? addressId)
        {
            try
            {
                //Account account = CoreContext.CurrentAccount;
                Account account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);

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
                                    profileName.ToCleanString(),
                                    nameOnAccount.ToCleanString(),
                                    routingInput.ToCleanString(),
                                    bankAccountTypeID
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
                billingAddress.City = (city == null ? "" : city.ToCleanString());
                //billingAddress.StateProvinceID = (state.IsValidInt() && state.ToInt() > 0) ? state.ToInt() : (int?)null;
                //billingAddress.State = (state.IsValidInt() && state.ToInt() > 0) ? SmallCollectionCache.Instance.StateProvinces.GetById(state.ToInt()).StateAbbreviation : state.ToCleanString();
                billingAddress.SetState(state, countryId);
                billingAddress.PostalCode = zip.ToCleanString();
                billingAddress.CountryID = countryId;
                billingAddress.PhoneNumber = phone.ToCleanString();
                billingAddress.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Billing.ToShort();
                billingAddress.LookUpAndSetGeoCode();

                #endregion

                account.Save();

                paymentMethod.BillingAddress = billingAddress;
                paymentMethod.BillingAddressID = billingAddress.AddressID;

                account.Save();

                UpdateAddressStreet(account, street, Convert.ToInt32(paymentMethod.BillingAddressID));

                CoreContext.CurrentAccount = account;

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        [HttpPost]
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Billing and Shipping Profiles", "~/Accounts/Overview")]
        public virtual ActionResult SavePaymentMethod(int? paymentMethodId, /*string accountName, */string nameOnCard, string accountNumber,
            DateTime expDate, string profileName, string attention, string address1, string address2, string address3, string zip,
            string city, string county, string street, string state, int countryId, string phone, int? addressId)
        {
            try
            {

                //Account account = CoreContext.CurrentAccount;
                Account account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);/*CS*/
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
                    //remove any addresses from previous save attempts that were not successful
                    account.Addresses.RemoveAll(a => a.AddressID == 0);
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
                billingAddress.City = (city == null ? "" : city.ToCleanString());
                billingAddress.County = (county == null ? "" : county.ToCleanString());
                billingAddress.SetState(state, countryId);
                //if (state.IsValidInt())
                //{
                //    int spid = state.ToInt();
                //    billingAddress.StateProvinceID = (spid > 0) ? spid : (int?)null;
                //    billingAddress.State = (spid > 0) ? SmallCollectionCache.Instance.StateProvinces.GetById(spid).StateAbbreviation : state.ToCleanString();
                //}
                //else if (!String.IsNullOrWhiteSpace(state))
                //{
                //    string st = state.ToCleanString();
                //    var stprov = SmallCollectionCache.Instance.StateProvinces.Where(sp => sp.StateAbbreviation.Equals(st, StringComparison.InvariantCultureIgnoreCase) && sp.CountryID == countryId).FirstOrDefault();
                //    if (stprov != null)
                //    {
                //        billingAddress.StateProvinceID = stprov.StateProvinceID;
                //        billingAddress.State = stprov.StateAbbreviation;
                //    }
                //}
                billingAddress.PostalCode = zip.ToCleanString();
                billingAddress.CountryID = countryId;
                billingAddress.PhoneNumber = phone.ToCleanString();
                billingAddress.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Billing.ToShort();
                billingAddress.LookUpAndSetGeoCode();

                account.Save();

                paymentMethod.BillingAddressID = billingAddress.AddressID;

                account.Save();

                CoreContext.CurrentAccount = account;

                UpdateAddressStreet(account, street, billingAddress.AddressID);

                CoreContext.CurrentAccount = account;

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
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

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Billing and Shipping Profiles", "~/Accounts/Overview")]
        public virtual ActionResult DeletePaymentMethod(int paymentMethodId)
        {
            try
            {
                var account = Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
                if (account.AccountPaymentMethods.Select(a => a.AccountPaymentMethodID).Contains(paymentMethodId))
                {
                    account.DeleteAccountPaymentMethod(paymentMethodId);
                    CoreContext.CurrentAccount = account;

                    return Json(new { result = true });
                }
                else
                    return Json(new { result = false, message = Translation.GetTerm("PaymentmethodNotFoundOnCurrentAccount", "PaymentMethod not found on current Account.") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        #region validaciones de texto

        [HttpPost]
        public ActionResult swValidarCPF(string CPFTextoInput)
        {
            CPFTextoInput = (CPFTextoInput == null ? "" : CPFTextoInput);

            Boolean Resulado = true;
            if (CPFTextoInput.Length < 11 || CPFTextoInput.Length < 9)
                Resulado = false;


            Dictionary<string, string> dcResultado = new Dictionary<string, string>();
            string NuevePrimerosDigitos = CPFTextoInput.Substring(0, 9);

            string PrimerDigito = string.Empty;

            string SegundoDigito = string.Empty;
            if (CPFTextoInput.Length > 9)
            {
                SegundoDigito = CPFTextoInput.Substring(10, 1);
                PrimerDigito = CPFTextoInput.Substring(9, 1);
            }

            int PrimerDigitoValidar = ValidarPrimerDigito(NuevePrimerosDigitos);
            int SegundoDigitoValidar = ValidarSegundoDigito(NuevePrimerosDigitos + PrimerDigitoValidar.ToString());

            if (CPFTextoInput.Length > 9)
            {
                Resulado = (Convert.ToByte(PrimerDigito) == PrimerDigitoValidar & Convert.ToByte(SegundoDigito) == SegundoDigitoValidar);


            }
            if (Resulado)
            {
                dcResultado = AccountExtensions.ValidarExistenciaCPF(NuevePrimerosDigitos + PrimerDigitoValidar + SegundoDigitoValidar);
                if (dcResultado.Count > 0)
                {
                    return Json(
                        new
                        {
                            mensaje = Translation.GetTerm("CPFIsRegistered", "CPF entered already registered"),
                            Estado = false
                        });
                }
                else
                {
                    return Json(
                        new
                        {
                            mensaje = "Ok",
                            Estado = true
                        });
                }

            }
            else
            {
                return Json(
                           new
                           {
                               mensaje = Translation.GetTerm("CpFisInvalid", "the value of CPF is incorrect"),
                               Estado = false
                           });
            }

        }
        [HttpPost]
        public ActionResult swValidarPIS(string TextoInputPIS)
        {
            Boolean Resultado = false;

            TextoInputPIS = TextoInputPIS == null ? "" : TextoInputPIS.Trim();

            if (TextoInputPIS.Length < 11 || TextoInputPIS.Length < 9)
                Resultado = false;


            string NuevePrimerosDigitos = TextoInputPIS.Substring(0, 9);
            string PrimerDigito = TextoInputPIS.Substring(9, 1);
            string SegundoDigito = TextoInputPIS.Substring(10, 1);
            int SegundoDigitoValidar = ValidarSegundoDigitoPIS(NuevePrimerosDigitos + PrimerDigito.ToString());

            Resultado = (Convert.ToByte(SegundoDigito) == SegundoDigitoValidar);
            if (Resultado)
            {
                Dictionary<string, string> dcResultado = AccountExtensions.ValidarExistenciaPIS(NuevePrimerosDigitos + PrimerDigito.ToString() + SegundoDigitoValidar.ToString());

                if (dcResultado.Count > 0)
                {
                    return Json(
                          new
                          {
                              mensaje = Translation.GetTerm("PisIsRegistered", "PIS entered already registered"),
                              Estado = false
                          });
                }
                else
                {
                    return Json(
                        new
                        {
                            mensaje = "Ok",
                            Estado = true
                        });
                }
            }
            else
            {
                return Json(
                           new
                           {
                               mensaje = Translation.GetTerm("PISisInvalid", "the value of PIS is incorrect"),
                               Estado = false
                           });
            }
        }
        [HttpPost]
        public ActionResult swValidarRG(string TextoRG)
        {
            TextoRG = TextoRG == null ? "" : TextoRG.Trim();
            Dictionary<string, string> dcResultado = AccountExtensions.ValidarExistenciaRG(TextoRG);
            if ((TextoRG == null ? "" : TextoRG).Length == 0)
            {
                return Json(
                              new
                              {
                                  mensaje = Translation.GetTerm("PISisInvalid", "the value of RG is required"),
                                  Estado = false
                              });
            }
            if (dcResultado.Count > 1)
            {
                return Json(
                            new
                            {
                                mensaje = Translation.GetTerm("RGIsRegistered", "RG entered already registered"),
                                Estado = false
                            });
            }
            else
            {
                return Json(
                        new
                        {
                            mensaje = "Ok",
                            Estado = true
                        });
            }
        }
        [HttpPost]
        public ActionResult swValidarEdad(string fechaNacimiento)
        {
            DateTime fechaNac = DateTime.UtcNow;
            Boolean isValid = false;
            if (DateTime.TryParse(fechaNacimiento, out fechaNac))
            {
                isValid = true;
                fechaNac = DateTime.Parse(fechaNacimiento, CoreContext.CurrentCultureInfo);
            }

            if (isValid)
            {
                DateTime _birthday = fechaNac;

                DateTime now = DateTime.Today;
                int age = now.Year - _birthday.Year;
                if (now < _birthday.AddYears(age)) age--;

                if (age < 18)// validar que sea mayor de edad 
                {
                    return Json(
                           new
                           {
                               mensaje = Translation.GetTerm("18Years", "You must be 18 years of age"),
                               Estado = false
                           });

                }
                else
                {
                    return Json(
                              new
                              {
                                  mensaje = "Ok",
                                  Estado = true
                              });
                }

            }
            else
            {
                return Json(
                               new
                               {
                                   mensaje = Translation.GetTerm("DateError", "The date entered is invalid"),
                                   Estado = false
                               });
            }
        }
        #region validaciones CPF
        static bool ValidarCPF(string TextoInput)
        {
            if (TextoInput.Length < 11 || TextoInput == "")
                return false;

            string NuevePrimerosDigitos = TextoInput.Substring(0, 9);
            string PrimerDigito = TextoInput.Substring(9, 1);
            string SegundoDigito = TextoInput.Substring(10, 1);
            int PrimerDigitoValidar = ValidarPrimerDigito(NuevePrimerosDigitos);
            int SegundoDigitoValidar = ValidarSegundoDigito(NuevePrimerosDigitos + PrimerDigitoValidar.ToString());
            return (Convert.ToByte(PrimerDigito) == PrimerDigitoValidar & Convert.ToByte(SegundoDigito) == SegundoDigitoValidar);
        }
        static int ValidarPrimerDigito(string TextoValidar)
        {
            int[] Multiplicadores = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] Resultados = new int[9];
            int indice = 0;
            int total = 0;
            for (int index = 0; index < TextoValidar.Length; index++)
            {
                Resultados[indice] = Convert.ToByte(TextoValidar.Substring(index, 1)) * Multiplicadores[indice];
                indice += 1;
            }
            total = Resultados.Sum();
            var residuo = total % 11;
            if (residuo < 2)
            {
                return 0;
            }
            return 11 - residuo;
        }
        static int ValidarSegundoDigito(string TextoValidar)
        {
            int[] Multiplicadores = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] Resultados = new int[10];
            int indice = 0;
            int total = 0;
            for (int index = 0; index < TextoValidar.Length; index++)
            {
                Resultados[indice] = Convert.ToByte(TextoValidar.Substring(index, 1)) * Multiplicadores[indice];
                indice += 1;
            }
            total = Resultados.Sum();
            var residuo = total % 11;
            if (residuo < 2)
            {
                return 0;
            }
            return 11 - residuo;
        }
        #endregion

        #region validaciones PIS
        static bool ValidarPIS(string TextoInput)
        {
            if (TextoInput.Length < 11 || TextoInput == "")
                return false;

            string NuevePrimerosDigitos = TextoInput.Substring(0, 9);
            string PrimerDigito = TextoInput.Substring(9, 1);
            string SegundoDigito = TextoInput.Substring(10, 1);
            int SegundoDigitoValidar = ValidarSegundoDigitoPIS(NuevePrimerosDigitos + PrimerDigito.ToString());

            return (Convert.ToByte(SegundoDigito) == SegundoDigitoValidar);
        }

        static int ValidarSegundoDigitoPIS(string TextoValidar)
        {
            int[] Multiplicadores = { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] Resultados = new int[10];
            int indice = 0;
            int total = 0;
            for (int index = 0; index < TextoValidar.Length; index++)
            {
                Resultados[indice] = Convert.ToByte(TextoValidar.Substring(index, 1)) * Multiplicadores[indice];
                indice += 1;
            }
            total = Resultados.Sum();
            var residuo = total % 11;
            if (residuo < 2)
            {
                return 0;
            }
            return 11 - residuo;
        }
        #endregion
        #endregion

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult BanksSearch(string query)
        {
            try
            {
                var dcBank = PaymentTicktesBussinessLogic.BankSearchAuto(query).Select((dc) => new { id = dc.Key, text = dc.Value });
                return Json(dcBank);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

    }
}
