using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Comparer;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Accounts.Models;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Business;
using System.Globalization;
using NetSteps.Common.Configuration;
using System.Xml;
using MailBee;
using MailBee.SmtpMail;
using MailBee.Mime;
using MailBee.Security;
using NetSteps.Common.Configuration;

//@01 20150805 BR-MLM-007 CSTI JMO: Removed placement and enroller section (Save Method)

namespace nsCore.Areas.Accounts.Controllers
{
    public class EditController : BaseAccountsController
    {
        protected override void SetViewData()
        {
            base.SetViewData();
            ViewBag.EditAccount = EditAccount;
            GetCreditRequirementsByAccount();
        }


        private void GetCreditRequirementsByAccount()
        {
            AccountPropertiesBusinessLogic busines = new AccountPropertiesBusinessLogic();
            var result = busines.GetCreditRequirementsByAccount(CurrentAccount.AccountID);

            ViewBag.EditAccountAdditional = result;

        }

        protected Account EditAccount
        {
            get
            {
                var res = CurrentAccount;
                if (res == null)
                { // if there is no account, create a new one (initialized with a new user)...
                    CurrentAccount = res = new Account() { User = new User() };
                }
                return res;
            }
        }


        [OutputCache(CacheProfile = "DontCache")]
        //[FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult CanEditAccountType()
        {
            string canEdit = AccountExtensions.CanEditAccountType(CoreContext.CurrentUser.UserID);
            return Json(new { result = canEdit });
        }

        #region Anterior
        //CAMBIO ENCORE-4
        //[FunctionFilter("Accounts-Create and Edit Account", "~/Accounts/Overview")]
        //[HttpGet]
        //public virtual ActionResult Index(string id)
        //{
        //    Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

        //    //Check if Tax Exempt Category for Avatax is configured
        //    if (NetSteps.Data.Entities.AvataxAPI.Util.IsAvataxEnabled())
        //    {
        //        var account = Account.LoadAccountProperties(EditAccount.AccountID);

        //        if (!Equals(null, account))
        //        {
        //            var accountPropertiesData = new AccountPropertiesModel();
        //            accountPropertiesData.AccountPropertyTypes = AccountPropertyType.LoadAllFull().Where(x => x.TermName == NetSteps.Data.Entities.AvataxAPI.Constants.AVATAX_ACCOUNTPROPERTY_TYPE_NAME).ToList();
        //            accountPropertiesData.AccountProperties = !Equals(null, account.AccountProperties) ? account.AccountProperties.ToList() : null;
        //            ViewData["AccountPropertiesData"] = accountPropertiesData;
        //        }
        //    }

        //    var model = new EditAccountViewModel();
        //    model.DisplayUsernameField = GetAuthUIService().GetConfiguration().ShowUsernameFormFields;

        //    this.AccountNum = id;
        //    return View(model);
        //}

        //public virtual ActionResult Save(
        //    List<AccountPropertiesParameters> AccountProperties, decimal ReferenceID, string ReferenceName, string PhoneNumberMain, int RelationShip,
        //    List<AccountSuppliedIDsParameters> AccountSuppliedIDs,
        //    int accountId, int accountType, int defaultLanguageId,
        //    List<AccountSocialNetworksParameters> AccountSocialNetworks,
        //    /*@01 D1
        //    string sponsorAccountNumber, string enrollerAccountNumber, 
        //    */
        //    bool applicationOnFile, string username,
        //    string password, string confirmPassword, bool userChangingPassword, bool generatedPassword, /*string profileName,*/ string attention, string address1,
        //    string address2, string address3, string zip, string city, string county, string state, string street, int countryId, string phone, string firstName, string middleName,
        //    string lastName, /*string homePhone,*/ string email, string hostedEmail, bool isTaxExempt, bool isTaxExemptVerified, string ssn,
        //    DateTime? dob, short? gender, List<AccountPhone> phones, bool isEntity, string entityName, short userstatus, short accountstatus, string coApplicant, string propertyValuedropdown = null,

        //     bool optOut = false

        //    )
        //{
        //    try
        //    {

        //        Account account = accountId > 0 ? Account.LoadFull(accountId) : new Account();
        //        account.StartEntityTracking();

        //        bool accountStatusChanged = account.AccountStatusID != accountstatus;

        //        hostedEmail = FormatEmailAddress(hostedEmail);
        //        if (!hostedEmail.IsNullOrEmpty())
        //        {
        //            bool isEmailAddressAvailable = MailAccount.IsAvailable(hostedEmail, account == null ? 0 : account.AccountID);
        //            if (!isEmailAddressAvailable)
        //                return Json(new { result = false, message = Translation.GetTerm("SpecifiedHostedEmailAddressIsNotAvailable.", "Specified hosted email address is not available.") });
        //        }
        //        else
        //        {
        //            hostedEmail = FormatEmailAddress(account.EmailAddress.Substring(1, account.EmailAddress.IndexOf('@') - 1));
        //            bool isEmailAddressAvailable = MailAccount.IsAvailable(hostedEmail, account == null ? 0 : account.AccountID);
        //            if (!isEmailAddressAvailable)
        //                hostedEmail = "";
        //        }

        //        var validCPF = swValidarCPF(AccountSuppliedIDs.ToList().Where(x => x.IDTypeID == 8).Select(a => a.AccountSuppliedIDValue).First().ToString());
        //        if (validCPF == 1)
        //            return Json(new { result = false, campo = "CPF", valid = "CPFIsRegistered", message = Translation.GetTerm("CPFIsRegistered", "CPF entered already registered") });

        //        if (validCPF == 3)
        //            return Json(new { result = false, campo = "CPF", valid = "CPFIsRegistered", message = Translation.GetTerm("CpFisInvalid", "the value of CPF is incorrect") });

        //        string PIS = string.Empty;
        //        PIS = AccountSuppliedIDs.ToList().Where(x => x.IDTypeID == 9).Select(a => a.AccountSuppliedIDValue ?? "").First().ToString();

        //        if (PIS != string.Empty)
        //        {
        //            var validPIS = swValidarPIS(PIS);
        //            if (validPIS == 1)
        //                return Json(new { result = false, campo = "PIS", valid = "PisIsRegistered", message = Translation.GetTerm("PisIsRegistered", "PIS entered already registered") });

        //            if (validPIS == 3)
        //                return Json(new { result = false, campo = "PIS", valid = "PISisInvalid", message = Translation.GetTerm("PISisInvalid", "the value of PIS is incorrect") });
        //        }

        //        var validRG = swValidarRG(AccountSuppliedIDs.ToList().Where(x => x.IDTypeID == 4).Select(a => a.AccountSuppliedIDValue).First().ToString());
        //        if (validRG == 1)
        //            return Json(new { result = false, campo = "RG", valid = "PISisInvalid", message = Translation.GetTerm("PISisInvalid", "the value of RG is required") });

        //        if (validRG == 2)
        //            return Json(new { result = false, campo = "RG", valid = "RGIsRegistered", message = Translation.GetTerm("RGIsRegistered", "RG entered already registered") });




        //        string sponsorValidationMessage = string.Empty;
        //        /*@01 D2
        //        if (!this.SaveSponsor(account, accountId, sponsorAccountNumber, enrollerAccountNumber, ref sponsorValidationMessage))
        //        {
        //            return Json(new { result = false, message = sponsorValidationMessage });
        //        }
        //        string enrollerValidationMessage = string.Empty;
        //        var hasEnrollerFunction = CoreContext.CurrentUser.HasFunction("Accounts-Change Enroller");
        //        if (hasEnrollerFunction && !this.SaveEnroller(account, accountId, sponsorAccountNumber, enrollerAccountNumber, ref enrollerValidationMessage))
        //        {
        //            return Json(new { result = false, message = enrollerValidationMessage });
        //        }
        //        */
        //        account.ReceivedApplication = applicationOnFile;
        //        account.DefaultLanguageID = defaultLanguageId;

        //        if (account.MarketID < 1)
        //        {
        //            var countryForMarket = SmallCollectionCache.Instance.Countries.GetById(countryId);
        //            if (countryForMarket != null)
        //                account.MarketID = countryForMarket.MarketID;
        //        }

        //        account.IsEntity = isEntity;
        //        account.EntityName = entityName;

        //        SetAccountStatus(account, accountstatus);

        //        ManageOptOut(account, optOut, email);


        //        // Set the account status accordingly
        //        account.AccountStatusID = accountstatus;

        //        DisableSitesIfAccountStatusTerminated(account);
        //        SetSiteStatus(account);
        //        if (accountStatusChanged && accountstatus == (short)Constants.AccountStatus.Active)
        //            SetAutoshipOrdersStatus(account);

        //        username = username.ToCleanString();

        //        if (!string.IsNullOrEmpty(username))
        //        {
        //            // Make sure the Username entered is not taken by someone else - JHE
        //            var tmpUser = account.User ?? new User();
        //            if (!tmpUser.IsUsernameAvailable(username))
        //            {
        //                return Json(new { result = false, message = Translation.GetTerm("UserNameisNotAvailablePleaseEnteraDifferentUsername", "User name is not available. Please enter a different Username.") });
        //            }

        //            if (account.User == null)
        //            {
        //                var user = new User();
        //                user.StartEntityTracking();
        //                user.Username = username;
        //                user.UserStatusID = (short)Constants.UserStatus.Active;
        //                user.UserTypeID = (short)Constants.UserType.Distributor;
        //                user.DefaultLanguageID = account.DefaultLanguageID;
        //                user.Roles.Add(Role.Load((int)Constants.Role.LimitedUser));

        //                user.Save();
        //                account.User = user;

        //            }
        //        }

        //        if (account.User != null)
        //        {
        //            account.User.StartEntityTracking();
        //            if (account.User != null)
        //            {
        //                if (!String.IsNullOrEmpty(username))
        //                {
        //                    account.User.Username = username;
        //                }
        //                else
        //                {
        //                    account.User.Username = account.AccountNumber;
        //                }
        //            }
        //            if (account.User.UserTypeID == 0)
        //            {
        //                account.User.UserTypeID = (short)Constants.UserType.Distributor;
        //            }

        //            account.User.DefaultLanguageID = account.DefaultLanguageID;
        //            account.User.UserStatusID = userstatus;

        //            if (userChangingPassword)
        //            {
        //                var result = NetSteps.Data.Entities.User.NewPasswordIsValid(password, confirmPassword);
        //                if (result.Success)
        //                {
        //                    account.User.Password = password.ToCleanString();
        //                }
        //                else
        //                {
        //                    return Json(new { result = false, message = result.Message });
        //                }
        //            }
        //        }

        //        account.FirstName = firstName.ToCleanString();
        //        account.MiddleName = middleName.ToCleanString();
        //        account.LastName = lastName.ToCleanString();
        //        account.EmailAddress = email.ToCleanString();
        //        account.IsTaxExempt = isTaxExempt;
        //        account.IsTaxExemptVerified = isTaxExemptVerified;
        //        ssn = ssn.Replace("-", string.Empty).ToCleanString();
        //        if (!ssn.Contains("*") && account.DecryptedTaxNumber != ssn)
        //        {
        //            if (account.EnforceUniqueTaxNumber())
        //            {
        //                if (account.IsTaxNumberAvailable(ssn))
        //                    account.DecryptedTaxNumber = ssn;
        //                else
        //                    return Json(new { result = false, message = string.Format("The Tax Number ({0}) is already in use by another account.", ssn.ToCleanString()) });
        //            }
        //            else
        //                account.DecryptedTaxNumber = ssn;
        //        }
        //        IFormatProvider culture = new CultureInfo("en-US", true);
        //        string d = Convert.ToDateTime(dob).ToString("MM/dd/yyyy");

        //        //DateTime.ParseExact(str, "MM/dd/yyyy", System.Globalization.CultureInfo.CurrentCulture);
        //        var dobf = Convert.ToDateTime(d, culture);

        //        account.Birthday = dobf;
        //        account.GenderID = gender;

        //        if (phones != null && phones.Count > 0)
        //        {
        //            //account.AccountPhones.SyncTo(phones.Where(p => !p.PhoneNumber.IsNullOrEmpty()), new LambdaComparer<AccountPhone>((ap1, ap2) => ap1.AccountPhoneID == ap2.AccountPhoneID, ap => (ap.AccountPhoneID > 0) ? ap.AccountPhoneID : ap.PhoneNumber.GetHashCode()), (ap1, ap2) =>
        //            account.AccountPhones.SyncTo(
        //                phones.Where(p => !p.PhoneNumber.IsNullOrEmpty()),
        //                new LambdaEqualityComparer<AccountPhone, int>(x => (x.AccountPhoneID > 0) ? x.AccountPhoneID : new { x.PhoneNumber, x.PhoneTypeID }.GetHashCode()),
        //                (ap1, ap2) =>
        //                {
        //                    ap1.PhoneTypeID = ap2.PhoneTypeID;
        //                    ap1.PhoneNumber = ap2.PhoneNumber;
        //                },
        //                (list, ap) =>
        //                {
        //                    list.RemoveAndMarkAsDeleted(ap);
        //                }
        //                );
        //        }
        //        else
        //            account.AccountPhones.RemoveAllAndMarkAsDeleted();

        //        if (generatedPassword)
        //            Account.SendGeneratedPasswordEmail(account);

        //        Address address = account.Addresses.GetDefaultByTypeID(NetSteps.Data.Entities.Generated.ConstantsGenerated.AddressType.Main);
        //        if (address == null)
        //        {
        //            address = new Address();
        //            address.StartEntityTracking();
        //        }
        //        address.AttachAddressChangedCheck();
        //        address.Attention = attention.ToCleanString();
        //        address.Address1 = address1.ToCleanString();
        //        address.Address2 = address2.ToCleanString();
        //        address.Address3 = address3.ToCleanString();
        //        address.City = (city == null ? "" : city.ToCleanString());
        //        address.SetState(state, countryId);
        //        address.Street = (street == null ? string.Empty : street.ToCleanString());
        //        address.County = (county == null ? "" : county.ToCleanString());
        //        address.PostalCode = zip.ToCleanString();
        //        address.CountryID = countryId;
        //        address.PhoneNumber = phone.RemoveNonNumericCharacters();
        //        address.AddressTypeID = (short)Constants.AddressType.Main;
        //        address.IsDefault = true;
        //        address.LookUpAndSetGeoCode();

        //        // Don't save address if it is empty - JHE
        //        if (!address.IsEmpty(true))
        //        {
        //            account.Addresses.Add(address);
        //        }

        //        if (coApplicant != null)
        //            account.CoApplicant = coApplicant.IsNullOrWhiteSpace() ? null : coApplicant;

        //        var accountPropType = SmallCollectionCache.Instance.AccountPropertyTypes.FirstOrDefault(pt => pt.TermName == NetSteps.Data.Entities.AvataxAPI.Constants.AVATAX_ACCOUNTPROPERTY_TYPE_NAME);

        //        if (!string.IsNullOrEmpty(propertyValuedropdown))
        //        {
        //            if (accountPropType != null)
        //            {
        //                var accountProp = account.AccountProperties.FirstOrDefault(ap => ap.AccountPropertyTypeID == accountPropType.AccountPropertyTypeID);
        //                if (accountProp != null)
        //                {
        //                    //Update
        //                    accountProp.AccountPropertyValueID = GetIntFromString(propertyValuedropdown);
        //                }
        //                else
        //                {
        //                    AccountProperty accountProperty = new AccountProperty();
        //                    accountProperty.AccountID = accountId;
        //                    accountProperty.AccountPropertyTypeID = accountPropType.AccountPropertyTypeID;
        //                    accountProperty.AccountPropertyValueID = GetIntFromString(propertyValuedropdown);
        //                    account.AccountProperties.Add(accountProperty);
        //                }
        //            }
        //            else
        //            {
        //                EntityExceptionHelper.GetAndLogNetStepsException("Tax Exempt Category for Avatax has not been configured ", NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
        //                //return Json(new { result = false, message = "Tax Exempt Category for Avatax has not been configured " });
        //            }
        //        }
        //        else
        //        {
        //            if (accountPropType != null)
        //            {
        //                var accountProp = account.AccountProperties.FirstOrDefault(ap => ap.AccountPropertyTypeID == accountPropType.AccountPropertyTypeID);

        //                if (accountProp != null)
        //                {
        //                    account.AccountProperties.RemoveAndMarkAsDeleted(accountProp);
        //                }
        //            }
        //        }

        //        bool unblock = false;
        //        MailAccount mailAccount = null;
        //        if (account.MailAccounts != null && account.MailAccounts.Count > 0)
        //        {
        //            mailAccount = account.MailAccounts.FirstOrDefault();
        //        }
        //        if (mailAccount != null && !hostedEmail.IsNullOrEmpty())
        //        {
        //            if (mailAccount.IsLockedOut && hostedEmail != mailAccount.EmailAddress)
        //                unblock = true;

        //            mailAccount.EmailAddress = hostedEmail;
        //            mailAccount.Active = true;
        //        }

        //        account.Save();

        //        AddressBusinessLogic business = new AddressBusinessLogic();

        //        foreach (Address addressOBJ in account.Addresses)
        //        {
        //            if(addressOBJ.Street != null)
        //                business.UpdateAddressStreet(addressOBJ);
        //        }

        //        // Add new MailAccount if it dosent exist yet - JHE
        //        if (mailAccount == null && !hostedEmail.IsNullOrEmpty())
        //        {
        //            mailAccount = MailAccount.LoadByAccountID(account.AccountID);
        //            if (mailAccount == null)
        //            {
        //                mailAccount = new MailAccount()
        //                {
        //                    AccountID = account.AccountID
        //                };
        //            }
        //            mailAccount.EmailAddress = hostedEmail;
        //            mailAccount.Active = true;
        //            mailAccount.Save();
        //        }

        //        if (accountStatusChanged)
        //        {
        //            UpdateAccountInfos(account.AccountID);
        //        }
        //        Save_ReferencesPropertiesSuppliedIDs(AccountProperties, ReferenceID, ReferenceName, PhoneNumberMain, RelationShip, AccountSuppliedIDs, AccountSocialNetworks);

        //        if (account.AccountTypeID == (short)Constants.AccountType.Distributor)
        //            AccountExtensions.UpdateAccountsCommission(account.AccountID);

        //        //Automatic Unblock by change email -JQP
        //        if (unblock)
        //            Account.UnBlockMailAccount(account.AccountID, hostedEmail);

        //        CoreContext.CurrentAccount = Account.LoadForSession(account.AccountID); // Necessary in case Duplicate Entities were needed to be removed to save the Entity - JHE

        //        return Json(new
        //        {
        //            result = true,
        //            referenceID = ReferenceID,
        //            accountProperties = AccountProperties.Select(apt => new
        //            {
        //                accountPropertyID = apt.AccountPropertyID
        //            }),
        //            accountSuppliedIDs = AccountSuppliedIDs.Select(apt => new
        //            {
        //                accountSuppliedID = apt.AccountSuppliedID
        //            }),
        //            accountSocialNetworks = AccountSocialNetworks.Select(apt => new
        //            {
        //                accountSocialNetworkID = apt.AccountSocialNetworkID
        //            })
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
        //        return Json(new { result = false, message = exception.PublicMessage });
        //    }
        //}
        #endregion

        [FunctionFilter("Accounts-Create and Edit Account", "~/Accounts/Overview")]
        [HttpGet]
        public virtual ActionResult Index(string id)
        {
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

            //Check if Tax Exempt Category for Avatax is configured
            if (NetSteps.Data.Entities.AvataxAPI.Util.IsAvataxEnabled())
            {
                var account = Account.LoadAccountProperties(EditAccount.AccountID);

                if (!Equals(null, account))
                {
                    var accountPropertiesData = new AccountPropertiesModel();
                    accountPropertiesData.AccountPropertyTypes = AccountPropertyType.LoadAllFull().Where(x => x.TermName == NetSteps.Data.Entities.AvataxAPI.Constants.AVATAX_ACCOUNTPROPERTY_TYPE_NAME).ToList();
                    accountPropertiesData.AccountProperties = !Equals(null, account.AccountProperties) ? account.AccountProperties.ToList() : null;
                    ViewData["AccountPropertiesData"] = accountPropertiesData;
                }
            }

            var model = new EditAccountViewModel();
            model.DisplayUsernameField = GetAuthUIService().GetConfiguration().ShowUsernameFormFields;

            this.AccountNum = id;
            //ENCORE_4 Parametrizable
            int iCountry = System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry"].ToInt();
            var countryDate = SmallCollectionCache.Instance.Countries.GetById(iCountry);
            ViewData["countryID"] = countryDate.CountryCode3;
            ViewData["listField"] = getFieldsEditAccount(iCountry, "Account_Config", "IDIOMA");

            return View(model);
        }
        //ENCORE_4 Parametrizable
        #region SAVEUSA
        public virtual ActionResult SaveUSA(int accountId, int accountType, int defaultLanguageId, string sponsorAccountNumber, string enrollerAccountNumber, bool applicationOnFile, string username,
            string password, string confirmPassword, bool userChangingPassword, bool generatedPassword, /*string profileName,*/ string attention, string address1,
            string address2, string address3, string zip, string city, string county, string state, int countryId, string phone, string firstName,
            string lastName, /*string homePhone,*/ string email, string hostedEmail, bool isTaxExempt, bool isTaxExemptVerified, string ssn,
            string dob, short? gender, List<AccountPhone> phones, bool isEntity, string entityName, short userstatus, short accountstatus, string coApplicant, string propertyValuedropdown = null, bool optOut = false)
        {
            try
            {
                Account account = accountId > 0 ? Account.LoadFull(accountId) : new Account();
                account.StartEntityTracking();

                bool accountStatusChanged = account.AccountStatusID != accountstatus;

                hostedEmail = FormatEmailAddress(hostedEmail);
                if (!hostedEmail.IsNullOrEmpty())
                {
                    bool isEmailAddressAvailable = MailAccount.IsAvailable(hostedEmail, account == null ? 0 : account.AccountID);
                    if (!isEmailAddressAvailable)
                        return Json(new { result = false, message = Translation.GetTerm("SpecifiedHostedEmailAddressIsNotAvailable.", "Specified hosted email address is not available.") });

                }

                string sponsorValidationMessage = string.Empty;
                if (!this.SaveSponsor(account, accountId, sponsorAccountNumber, enrollerAccountNumber, ref sponsorValidationMessage))
                {
                    return Json(new { result = false, message = sponsorValidationMessage });
                }

                string enrollerValidationMessage = string.Empty;
                var hasEnrollerFunction = CoreContext.CurrentUser.HasFunction("Accounts-Change Enroller");
                if (hasEnrollerFunction && !this.SaveEnroller(account, accountId, sponsorAccountNumber, enrollerAccountNumber, ref enrollerValidationMessage))
                {
                    return Json(new { result = false, message = enrollerValidationMessage });
                }
                account.ReceivedApplication = applicationOnFile;
                account.DefaultLanguageID = defaultLanguageId;

                if (account.MarketID < 1)
                {
                    var countryForMarket = SmallCollectionCache.Instance.Countries.GetById(countryId);
                    if (countryForMarket != null)
                        account.MarketID = countryForMarket.MarketID;
                }

                account.IsEntity = isEntity;
                account.EntityName = entityName;

                SetAccountStatus(account, accountstatus);

                if (!email.IsNullOrEmpty())
                {
                    bool isEmailAddressAvailable = MailAccount.IsOtherAvailable(email, account == null ? 0 : account.AccountID);
                    if (!isEmailAddressAvailable)
                        return Json(new { result = false, message = Translation.GetTerm("SpecifiedEmailAddressIsNotAvailable.", "Specified personal email address is not available.") });

                }

                ManageOptOut(account, optOut, email);


                // Set the account status accordingly
                account.AccountStatusID = accountstatus;

                DisableSitesIfAccountStatusTerminated(account);
                SetSiteStatus(account);
                if (accountStatusChanged && accountstatus == (short)Constants.AccountStatus.Active)
                    SetAutoshipOrdersStatus(account);

                username = username.ToCleanString();

                if (!string.IsNullOrEmpty(username))
                {
                    // Make sure the Username entered is not taken by someone else - JHE
                    var tmpUser = account.User ?? new User();
                    if (!tmpUser.IsUsernameAvailable(username))
                    {
                        return Json(new { result = false, message = Translation.GetTerm("UserNameisNotAvailablePleaseEnteraDifferentUsername", "User name is not available. Please enter a different Username.") });
                    }

                    if (account.User == null)
                    {
                        var user = new User();
                        user.StartEntityTracking();
                        user.Username = username;
                        user.UserStatusID = (short)Constants.UserStatus.Active;
                        user.UserTypeID = (short)Constants.UserType.Distributor;
                        user.DefaultLanguageID = account.DefaultLanguageID;
                        user.Roles.Add(Role.Load((int)Constants.Role.LimitedUser));

                        user.Save();
                        account.User = user;

                    }
                }

                if (account.User != null)
                {
                    account.User.StartEntityTracking();
                    if (account.User != null)
                    {
                        if (!String.IsNullOrEmpty(username))
                        {
                            account.User.Username = username;
                        }
                        else
                        {
                            account.User.Username = account.AccountNumber;
                        }
                    }
                    if (account.User.UserTypeID == 0)
                    {
                        account.User.UserTypeID = (short)Constants.UserType.Distributor;
                    }

                    account.User.DefaultLanguageID = account.DefaultLanguageID;
                    account.User.UserStatusID = userstatus;

                    if (userChangingPassword)
                    {
                        var result = NetSteps.Data.Entities.User.NewPasswordIsValid(password, confirmPassword);
                        if (result.Success)
                        {
                            account.User.Password = password.ToCleanString();
                            if (!account.User.PasswordIsIdentical)
                            {
                                SendMailPassword(account.FirstName, password.ToCleanString(), account.EmailAddress);
                            }
                        }
                        else
                        {
                            return Json(new { result = false, message = result.Message });
                        }
                    }
                }

                account.FirstName = firstName.ToCleanString();
                account.LastName = lastName.ToCleanString();
                account.EmailAddress = email.ToCleanString();
                account.IsTaxExempt = isTaxExempt;
                account.IsTaxExemptVerified = isTaxExemptVerified;
                ssn = ssn.Replace("-", string.Empty).ToCleanString();
                if (!ssn.Contains("*") && account.DecryptedTaxNumber != ssn)
                {
                    if (account.EnforceUniqueTaxNumber())
                    {
                        if (account.IsTaxNumberAvailable(ssn))
                            account.DecryptedTaxNumber = ssn;
                        else
                            return Json(new { result = false, message = string.Format("The Tax Number ({0}) is already in use by another account.", ssn.ToCleanString()) });
                    }
                    else
                        account.DecryptedTaxNumber = ssn;
                }

                IFormatProvider culture = new CultureInfo("en-US", true);
                string d = Convert.ToDateTime(dob).ToString("MM/dd/yyyy");

                //DateTime.ParseExact(str, "MM/dd/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                var dobf = Convert.ToDateTime(d, culture);

                account.Birthday = dobf;
                account.GenderID = gender;

                if (phones != null && phones.Count > 0)
                {
                    //account.AccountPhones.SyncTo(phones.Where(p => !p.PhoneNumber.IsNullOrEmpty()), new LambdaComparer<AccountPhone>((ap1, ap2) => ap1.AccountPhoneID == ap2.AccountPhoneID, ap => (ap.AccountPhoneID > 0) ? ap.AccountPhoneID : ap.PhoneNumber.GetHashCode()), (ap1, ap2) =>
                    account.AccountPhones.SyncTo(
                        phones.Where(p => !p.PhoneNumber.IsNullOrEmpty()),
                        new LambdaEqualityComparer<AccountPhone, int>(x => (x.AccountPhoneID > 0) ? x.AccountPhoneID : new { x.PhoneNumber, x.PhoneTypeID }.GetHashCode()),
                        (ap1, ap2) =>
                        {
                            ap1.PhoneTypeID = ap2.PhoneTypeID;
                            ap1.PhoneNumber = ap2.PhoneNumber;
                        },
                        (list, ap) =>
                        {
                            list.RemoveAndMarkAsDeleted(ap);
                        }
                        );
                }else {
                        account.AccountPhones.RemoveAllAndMarkAsDeleted();
                }

                if (generatedPassword)
                    Account.SendGeneratedPasswordEmail(account);

                Address address = account.Addresses.GetDefaultByTypeID(NetSteps.Data.Entities.Generated.ConstantsGenerated.AddressType.Main);
                if (address == null)
                {
                    address = new Address();
                    address.StartEntityTracking();
                }
                address.AttachAddressChangedCheck();
                address.Attention = attention.ToCleanString();
                address.Address1 = address1.ToCleanString();
                address.Address2 = address2.ToCleanString();
                address.Address3 = address3.ToCleanString();
                address.City = city.ToCleanString();
                address.SetState(state, countryId);
                address.County = county.ToCleanString();
                address.PostalCode = zip.ToCleanString();
                address.CountryID = countryId;
                address.PhoneNumber = phone.RemoveNonNumericCharacters();
                address.AddressTypeID = (short)Constants.AddressType.Main;
                address.IsDefault = true;
                address.LookUpAndSetGeoCode();

                // Don't save address if it is empty - JHE
                if (!address.IsEmpty(true))
                {
                    account.Addresses.Add(address);
                }

                if (coApplicant != null)
                    account.CoApplicant = coApplicant.IsNullOrWhiteSpace() ? null : coApplicant;

                var accountPropType = SmallCollectionCache.Instance.AccountPropertyTypes.FirstOrDefault(pt => pt.TermName == NetSteps.Data.Entities.AvataxAPI.Constants.AVATAX_ACCOUNTPROPERTY_TYPE_NAME);

                if (!string.IsNullOrEmpty(propertyValuedropdown))
                {
                    if (accountPropType != null)
                    {
                        var accountProp = account.AccountProperties.FirstOrDefault(ap => ap.AccountPropertyTypeID == accountPropType.AccountPropertyTypeID);
                        if (accountProp != null)
                        {
                            //Update
                            accountProp.AccountPropertyValueID = GetIntFromString(propertyValuedropdown);
                        }
                        else
                        {
                            AccountProperty accountProperty = new AccountProperty();
                            accountProperty.AccountID = accountId;
                            accountProperty.AccountPropertyTypeID = accountPropType.AccountPropertyTypeID;
                            accountProperty.AccountPropertyValueID = GetIntFromString(propertyValuedropdown);
                            account.AccountProperties.Add(accountProperty);
                        }
                    }
                    else
                    {
                        EntityExceptionHelper.GetAndLogNetStepsException("Tax Exempt Category for Avatax has not been configured ", NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                        //return Json(new { result = false, message = "Tax Exempt Category for Avatax has not been configured " });
                    }
                }
                else
                {
                    if (accountPropType != null)
                    {
                        var accountProp = account.AccountProperties.FirstOrDefault(ap => ap.AccountPropertyTypeID == accountPropType.AccountPropertyTypeID);

                        if (accountProp != null)
                        {
                            account.AccountProperties.RemoveAndMarkAsDeleted(accountProp);
                        }
                    }
                }

                MailAccount mailAccount = null;
                if (account.MailAccounts != null && account.MailAccounts.Count > 0)
                {
                    mailAccount = account.MailAccounts.FirstOrDefault();
                }
                if (mailAccount != null && !hostedEmail.IsNullOrEmpty())
                {
                    mailAccount.EmailAddress = hostedEmail;
                    mailAccount.Active = true;
                }

                account.Save();

                // Add new MailAccount if it dosent exist yet - JHE
                if (mailAccount == null && !hostedEmail.IsNullOrEmpty())
                {
                    mailAccount = MailAccount.LoadByAccountID(account.AccountID);
                    if (mailAccount == null)
                    {
                        mailAccount = new MailAccount()
                        {
                            AccountID = account.AccountID
                        };
                    }
                    mailAccount.EmailAddress = hostedEmail;
                    mailAccount.Active = true;
                    mailAccount.Save();
                }

                if (accountStatusChanged)
                {
                    UpdateAccountInfos(account.AccountID);
                }

                /*R2719 - CGI(JICM) - Invocando al Método que ejecuta el SP Para actualizar Cuentas BA - INI*/
                if (account.AccountTypeID == (short)Constants.AccountType.Distributor)
                    AccountExtensions.UpdateAccountsCommission(account.AccountID);
                /*R2719 - CGI(JICM) - Invocando al Método que ejecuta el SP Para actualizar Cuentas BA - FIN*/

                CoreContext.CurrentAccount = Account.LoadForSession(account.AccountID); // Necessary in case Duplicate Entities were needed to be removed to save the Entity - JHE

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        #endregion

        //ENCORE_4 Parametrizable
        #region SAVEBRA
        public virtual ActionResult SaveBRA(
            List<AccountPropertiesParameters> AccountProperties, decimal ReferenceID, string ReferenceName, string PhoneNumberMain, int RelationShip,
            List<AccountSuppliedIDsParameters> AccountSuppliedIDs,
            int accountId, int accountType, int defaultLanguageId,
            List<AccountSocialNetworksParameters> AccountSocialNetworks,
            /*@01 D1
            string sponsorAccountNumber, string enrollerAccountNumber, 
            */
            string username,
            string password, string confirmPassword, bool userChangingPassword, bool generatedPassword, /*string profileName,*/ string attention, string address1,
            string address2, string address3, string zip, string city, string county, string state, string street, int countryId, string phone, string firstName, string middleName,
            string lastName, /*string homePhone,*/ string email, string hostedEmail, string ssn,
            DateTime? dob, List<AccountPhone> phones, short userstatus, short accountstatus, string coApplicant = null, string propertyValuedropdown = null,
            bool applicationOnFile = false, bool isTaxExempt = false, bool isTaxExemptVerified = false, bool isEntity = false, string entityName = null, short? gender = 0,
            bool optOut = false
            )
        {
            try
            {

                Account account = accountId > 0 ? Account.LoadFull(accountId) : new Account();
                account.StartEntityTracking();

                bool accountStatusChanged = account.AccountStatusID != accountstatus;

                hostedEmail = FormatEmailAddress(hostedEmail);
                if (!hostedEmail.IsNullOrEmpty())
                {
                    bool isEmailAddressAvailable = MailAccount.IsAvailable(hostedEmail, account == null ? 0 : account.AccountID);
                    if (!isEmailAddressAvailable)
                        return Json(new { result = false, message = Translation.GetTerm("SpecifiedHostedEmailAddressIsNotAvailable.", "Specified hosted email address is not available.") });
                }
                else
                {
                    hostedEmail = FormatEmailAddress(account.EmailAddress.Substring(1, account.EmailAddress.IndexOf('@') - 1));
                    bool isEmailAddressAvailable = MailAccount.IsAvailable(hostedEmail, account == null ? 0 : account.AccountID);
                    if (!isEmailAddressAvailable)
                        hostedEmail = "";
                }

                var validCPF = swValidarCPF(AccountSuppliedIDs.ToList().Where(x => x.IDTypeID == 8).Select(a => a.AccountSuppliedIDValue).First().ToString());
                if (validCPF == 1)
                    return Json(new { result = false, campo = "CPF", valid = "CPFIsRegistered", message = Translation.GetTerm("CPFIsRegistered", "CPF entered already registered") });

                if (validCPF == 3)
                    return Json(new { result = false, campo = "CPF", valid = "CPFIsRegistered", message = Translation.GetTerm("CpFisInvalid", "the value of CPF is incorrect") });

                string PIS = string.Empty;
                PIS = AccountSuppliedIDs.ToList().Where(x => x.IDTypeID == 9).Select(a => a.AccountSuppliedIDValue ?? "").First().ToString();

                if (PIS != string.Empty)
                {
                    var validPIS = swValidarPIS(PIS);
                    if (validPIS == 1)
                        return Json(new { result = false, campo = "PIS", valid = "PisIsRegistered", message = Translation.GetTerm("PisIsRegistered", "PIS entered already registered") });

                    if (validPIS == 3)
                        return Json(new { result = false, campo = "PIS", valid = "PISisInvalid", message = Translation.GetTerm("PISisInvalid", "the value of PIS is incorrect") });
                }

                var validRG = swValidarRG(AccountSuppliedIDs.ToList().Where(x => x.IDTypeID == 4).Select(a => a.AccountSuppliedIDValue).First().ToString());
                if (validRG == 1)
                    return Json(new { result = false, campo = "RG", valid = "PISisInvalid", message = Translation.GetTerm("PISisInvalid", "the value of RG is required") });

                if (validRG == 2)
                    return Json(new { result = false, campo = "RG", valid = "RGIsRegistered", message = Translation.GetTerm("RGIsRegistered", "RG entered already registered") });




                string sponsorValidationMessage = string.Empty;
                /*@01 D2
                if (!this.SaveSponsor(account, accountId, sponsorAccountNumber, enrollerAccountNumber, ref sponsorValidationMessage))
                {
                    return Json(new { result = false, message = sponsorValidationMessage });
                }
                string enrollerValidationMessage = string.Empty;
                var hasEnrollerFunction = CoreContext.CurrentUser.HasFunction("Accounts-Change Enroller");
                if (hasEnrollerFunction && !this.SaveEnroller(account, accountId, sponsorAccountNumber, enrollerAccountNumber, ref enrollerValidationMessage))
                {
                    return Json(new { result = false, message = enrollerValidationMessage });
                }
                */
                account.ReceivedApplication = applicationOnFile;
                account.DefaultLanguageID = defaultLanguageId;

                if (account.MarketID < 1)
                {
                    var countryForMarket = SmallCollectionCache.Instance.Countries.GetById(countryId);
                    if (countryForMarket != null)
                        account.MarketID = countryForMarket.MarketID;
                }

                account.IsEntity = isEntity;
                account.EntityName = entityName;

                SetAccountStatus(account, accountstatus);

                if (!email.IsNullOrEmpty())
                {
                    bool isEmailAddressAvailable = MailAccount.IsOtherAvailable(email, account == null ? 0 : account.AccountID);
                    if (!isEmailAddressAvailable)
                        return Json(new { result = false, message = Translation.GetTerm("SpecifiedEmailAddressIsNotAvailable.", "Specified personal email address is not available.") });

                }

                ManageOptOut(account, optOut, email);


                // Set the account status accordingly
                account.AccountStatusID = accountstatus;

                DisableSitesIfAccountStatusTerminated(account);
                SetSiteStatus(account);
                if (accountStatusChanged && accountstatus == (short)Constants.AccountStatus.Active)
                    SetAutoshipOrdersStatus(account);

                username = username.ToCleanString();

                if (!string.IsNullOrEmpty(username))
                {
                    // Make sure the Username entered is not taken by someone else - JHE
                    var tmpUser = account.User ?? new User();
                    if (!tmpUser.IsUsernameAvailable(username))
                    {
                        return Json(new { result = false, message = Translation.GetTerm("UserNameisNotAvailablePleaseEnteraDifferentUsername", "User name is not available. Please enter a different Username.") });
                    }

                    if (account.User == null)
                    {
                        var user = new User();
                        user.StartEntityTracking();
                        user.Username = username;
                        user.UserStatusID = (short)Constants.UserStatus.Active;
                        user.UserTypeID = (short)Constants.UserType.Distributor;
                        user.DefaultLanguageID = account.DefaultLanguageID;
                        user.Roles.Add(Role.Load((int)Constants.Role.LimitedUser));

                        user.Save();
                        account.User = user;

                    }
                }

                if (account.User != null)
                {
                    account.User.StartEntityTracking();
                    if (account.User != null)
                    {
                        if (!String.IsNullOrEmpty(username))
                        {
                            account.User.Username = username;
                        }
                        else
                        {
                            account.User.Username = account.AccountNumber;
                        }
                    }
                    if (account.User.UserTypeID == 0)
                    {
                        account.User.UserTypeID = (short)Constants.UserType.Distributor;
                    }

                    account.User.DefaultLanguageID = account.DefaultLanguageID;
                    account.User.UserStatusID = userstatus;

                    if (userChangingPassword)
                    {
                        var result = NetSteps.Data.Entities.User.NewPasswordIsValid(password, confirmPassword);
                        if (result.Success)
                        {
                            account.User.Password = password.ToCleanString();
                        }
                        else
                        {
                            return Json(new { result = false, message = result.Message });
                        }
                    }
                }

                account.FirstName = firstName.ToCleanString();
                account.MiddleName = middleName.ToCleanString();
                account.LastName = lastName.ToCleanString();
                account.EmailAddress = email.ToCleanString();
                account.IsTaxExempt = isTaxExempt;
                account.IsTaxExemptVerified = isTaxExemptVerified;
                ssn = ssn.Replace("-", string.Empty).ToCleanString();
                if (!ssn.Contains("*") && account.DecryptedTaxNumber != ssn)
                {
                    if (account.EnforceUniqueTaxNumber())
                    {
                        if (account.IsTaxNumberAvailable(ssn))
                            account.DecryptedTaxNumber = ssn;
                        else
                            return Json(new { result = false, message = string.Format("The Tax Number ({0}) is already in use by another account.", ssn.ToCleanString()) });
                    }
                    else
                        account.DecryptedTaxNumber = ssn;
                }
                IFormatProvider culture = new CultureInfo("en-US", true);
                string d = Convert.ToDateTime(dob).ToString("MM/dd/yyyy");

                //DateTime.ParseExact(str, "MM/dd/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                var dobf = Convert.ToDateTime(d, culture);

                account.Birthday = dobf;
                account.GenderID = gender;

                if (phones != null && phones.Count > 0)
                {
                    //account.AccountPhones.SyncTo(phones.Where(p => !p.PhoneNumber.IsNullOrEmpty()), new LambdaComparer<AccountPhone>((ap1, ap2) => ap1.AccountPhoneID == ap2.AccountPhoneID, ap => (ap.AccountPhoneID > 0) ? ap.AccountPhoneID : ap.PhoneNumber.GetHashCode()), (ap1, ap2) =>
                    account.AccountPhones.SyncTo(
                        phones.Where(p => !p.PhoneNumber.IsNullOrEmpty()),
                        new LambdaEqualityComparer<AccountPhone, int>(x => (x.AccountPhoneID > 0) ? x.AccountPhoneID : new { x.PhoneNumber, x.PhoneTypeID }.GetHashCode()),
                        (ap1, ap2) =>
                        {
                            ap1.PhoneTypeID = ap2.PhoneTypeID;
                            ap1.PhoneNumber = ap2.PhoneNumber;
                        },
                        (list, ap) =>
                        {
                            list.RemoveAndMarkAsDeleted(ap);
                        }
                        );
                }
                else
                    account.AccountPhones.RemoveAllAndMarkAsDeleted();

                if (generatedPassword)
                    Account.SendGeneratedPasswordEmail(account);

                Address address = account.Addresses.GetDefaultByTypeID(NetSteps.Data.Entities.Generated.ConstantsGenerated.AddressType.Main);
                if (address == null)
                {
                    address = new Address();
                    address.StartEntityTracking();
                }
                address.AttachAddressChangedCheck();
                address.Attention = attention.ToCleanString();
                address.Address1 = address1.ToCleanString();
                address.Address2 = address2.ToCleanString();
                address.Address3 = address3.ToCleanString();
                address.City = (city == null ? "" : city.ToCleanString());
                address.SetState(state, countryId);
                address.Street = (street == null ? string.Empty : street.ToCleanString());
                address.County = (county == null ? "" : county.ToCleanString());
                address.PostalCode = zip.ToCleanString();
                address.CountryID = countryId;
                address.PhoneNumber = phone.RemoveNonNumericCharacters();
                address.AddressTypeID = (short)Constants.AddressType.Main;
                address.IsDefault = true;
                address.LookUpAndSetGeoCode();

                // Don't save address if it is empty - JHE
                if (!address.IsEmpty(true))
                {
                    account.Addresses.Add(address);
                }

                if (coApplicant != null)
                    account.CoApplicant = coApplicant.IsNullOrWhiteSpace() ? null : coApplicant;

                var accountPropType = SmallCollectionCache.Instance.AccountPropertyTypes.FirstOrDefault(pt => pt.TermName == NetSteps.Data.Entities.AvataxAPI.Constants.AVATAX_ACCOUNTPROPERTY_TYPE_NAME);

                if (!string.IsNullOrEmpty(propertyValuedropdown))
                {
                    if (accountPropType != null)
                    {
                        var accountProp = account.AccountProperties.FirstOrDefault(ap => ap.AccountPropertyTypeID == accountPropType.AccountPropertyTypeID);
                        if (accountProp != null)
                        {
                            //Update
                            accountProp.AccountPropertyValueID = GetIntFromString(propertyValuedropdown);
                        }
                        else
                        {
                            AccountProperty accountProperty = new AccountProperty();
                            accountProperty.AccountID = accountId;
                            accountProperty.AccountPropertyTypeID = accountPropType.AccountPropertyTypeID;
                            accountProperty.AccountPropertyValueID = GetIntFromString(propertyValuedropdown);
                            account.AccountProperties.Add(accountProperty);
                        }
                    }
                    else
                    {
                        EntityExceptionHelper.GetAndLogNetStepsException("Tax Exempt Category for Avatax has not been configured ", NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                        //return Json(new { result = false, message = "Tax Exempt Category for Avatax has not been configured " });
                    }
                }
                else
                {
                    if (accountPropType != null)
                    {
                        var accountProp = account.AccountProperties.FirstOrDefault(ap => ap.AccountPropertyTypeID == accountPropType.AccountPropertyTypeID);

                        if (accountProp != null)
                        {
                            account.AccountProperties.RemoveAndMarkAsDeleted(accountProp);
                        }
                    }
                }

                bool unblock = false;
                MailAccount mailAccount = null;
                if (account.MailAccounts != null && account.MailAccounts.Count > 0)
                {
                    mailAccount = account.MailAccounts.FirstOrDefault();
                }
                if (mailAccount != null && !hostedEmail.IsNullOrEmpty())
                {
                    if (mailAccount.IsLockedOut && hostedEmail != mailAccount.EmailAddress)
                        unblock = true;

                    mailAccount.EmailAddress = hostedEmail;
                    mailAccount.Active = true;
                }

                account.Save();

                AddressBusinessLogic business = new AddressBusinessLogic();

                foreach (Address addressOBJ in account.Addresses)
                {
                    if (addressOBJ.Street != null)
                        business.UpdateAddressStreet(addressOBJ);
                }

                // Add new MailAccount if it dosent exist yet - JHE
                if (mailAccount == null && !hostedEmail.IsNullOrEmpty())
                {
                    mailAccount = MailAccount.LoadByAccountID(account.AccountID);
                    if (mailAccount == null)
                    {
                        mailAccount = new MailAccount()
                        {
                            AccountID = account.AccountID
                        };
                    }
                    mailAccount.EmailAddress = hostedEmail;
                    mailAccount.Active = true;
                    mailAccount.Save();
                }

                if (accountStatusChanged)
                {
                    UpdateAccountInfos(account.AccountID);
                }
                Save_ReferencesPropertiesSuppliedIDs(AccountProperties, ReferenceID, ReferenceName, PhoneNumberMain, RelationShip, AccountSuppliedIDs, AccountSocialNetworks);

                if (account.AccountTypeID == (short)Constants.AccountType.Distributor)
                    AccountExtensions.UpdateAccountsCommission(account.AccountID);

                //Automatic Unblock by change email -JQP
                if (unblock)
                    Account.UnBlockMailAccount(account.AccountID, hostedEmail);

                CoreContext.CurrentAccount = Account.LoadForSession(account.AccountID); // Necessary in case Duplicate Entities were needed to be removed to save the Entity - JHE

                return Json(new
                {
                    result = true,
                    referenceID = ReferenceID,
                    accountProperties = AccountProperties.Select(apt => new
                    {
                        accountPropertyID = apt.AccountPropertyID
                    }),
                    accountSuppliedIDs = AccountSuppliedIDs.Select(apt => new
                    {
                        accountSuppliedID = apt.AccountSuppliedID
                    }),
                    accountSocialNetworks = AccountSocialNetworks.Select(apt => new
                    {
                        accountSocialNetworkID = apt.AccountSocialNetworkID
                    })
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        #endregion

        protected virtual void SendMailPassword(string pAccountName, string pAccountPassword, string pMail)
        {
            string smtpServerName = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.SmtpServer);
            int smtpPort = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.SmtpPort, 25);
            bool useSMTPAuthentication = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UseSmtpAuthentication);
            string smtpUserName = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.SmtpUserName);
            string smtpPassword = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.SmtpPassword);

            Smtp.LicenseKey = "MN200-9F57689E57B057AB577699C15A6B-9A7C";
            var smtp = new Smtp();
            var smtpServer = useSMTPAuthentication
                    ? new SmtpServer(smtpServerName, smtpUserName, smtpPassword)
                    : new SmtpServer(smtpServerName);
            smtpServer.Port = smtpPort;

            smtp.SmtpServers.Add(smtpServer);
            MailBee.Mime.MailMessage mail = smtp.Message;
            mail.To.Add(pMail);
            mail.From.Email = "contactus@belcorpusa.com";
            mail.From.DisplayName = "Usa Belcorp";
            mail.Subject = "Recuperación de Clave Belcorp USA";
            mail.LoadBodyText(Server.MapPath("~/EmailChangePassword.html"), MessageBodyType.Html,
                null, ImportBodyOptions.ImportRelatedFiles);
            mail.BodyHtmlText = mail.BodyHtmlText.Replace("{{AccountName}}", pAccountName).Replace("{{NewPassword}}", pAccountPassword);
            smtp.Send();
        }
        //FIN CAMBIO ENCORE-4

        private void Save_ReferencesPropertiesSuppliedIDs(List<AccountPropertiesParameters> AccountProperties,
                                                          decimal ReferenceID, string ReferenceName, string PhoneNumberMain, int RelationShip,
                                                          List<AccountSuppliedIDsParameters> AccountSuppliedIDs,
                                                          List<AccountSocialNetworksParameters> AccountSocialNetworks)
        {

            try
            {
                AccountReferencesBusinessLogic referenceBusines = new AccountReferencesBusinessLogic();
                AccountPropertiesParameters referenceDat = new AccountPropertiesParameters();
                AccountSocialNetworksBusinessLogic businesSocial = new AccountSocialNetworksBusinessLogic();

                int accountID = CurrentAccount.AccountID;

                referenceDat.AccountReferencID = ReferenceID;
                referenceDat.AccountID = accountID;
                referenceDat.ReferenceName = ReferenceName;
                referenceDat.PhoneNumberMain = !string.IsNullOrEmpty(PhoneNumberMain) ? Int64.Parse(PhoneNumberMain) : 0;
                referenceDat.RelationShip = RelationShip;
                if (ReferenceID == 0)
                {
                    var res02 = referenceBusines.Insert(referenceDat);
                    ReferenceID = res02.ID;
                }
                else
                {
                    referenceBusines.Update(referenceDat);
                }


                foreach (AccountPropertiesParameters creditRequirement in AccountProperties)
                {
                    AccountPropertiesBusinessLogic busines = new AccountPropertiesBusinessLogic();
                    creditRequirement.AccountID = accountID;
                    creditRequirement.Active = true;

                    if (creditRequirement.AccountPropertyID == 0)
                    {
                        busines = new AccountPropertiesBusinessLogic();
                        var res = busines.Insert(creditRequirement);
                        creditRequirement.AccountPropertyID = res.ID;
                    }
                    else
                    {
                        busines = new AccountPropertiesBusinessLogic();
                        busines.Update(creditRequirement);
                    }


                }

                foreach (AccountSuppliedIDsParameters accountSupplied in AccountSuppliedIDs)
                {
                    AccountSuppliedIDsBusinessLogic busines = new AccountSuppliedIDsBusinessLogic();
                    accountSupplied.AccountID = accountID;

                    accountSupplied.AccountSuppliedIDValue = accountSupplied.AccountSuppliedIDValue ?? string.Empty;

                    if (accountSupplied.AccountSuppliedIDValue != null)
                    {
                        if (accountSupplied.AccountSuppliedID == 0)
                        {
                            busines = new AccountSuppliedIDsBusinessLogic();
                            var res = busines.Insert(accountSupplied);
                            accountSupplied.AccountSuppliedID = res.ID;
                        }
                        else
                        {
                            busines = new AccountSuppliedIDsBusinessLogic();
                            busines.Update(accountSupplied);
                        }
                    }
                }

                foreach (AccountSocialNetworksParameters accountSocialNetwork in AccountSocialNetworks)
                {
                    accountSocialNetwork.AccountID = CurrentAccount.AccountID;

                    if (accountSocialNetwork.AccountSocialNetworkID == 0)
                    {
                        businesSocial = new AccountSocialNetworksBusinessLogic();
                        var res = businesSocial.Insert(accountSocialNetwork);
                        accountSocialNetwork.AccountSocialNetworkID = res.ID;
                    }
                    else
                    {
                        businesSocial = new AccountSocialNetworksBusinessLogic();
                        if (accountSocialNetwork.Value==null) accountSocialNetwork.Value="";//GR4602
                        businesSocial.Update(accountSocialNetwork);
                    }
                }
            }
            catch (Exception ex)
            {
                //var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                //return Json(new { result = false, message = exception.PublicMessage });
            }

        }

        public int swValidarCPF(string CPFTextoInput)
        {
            int returnInt = 0;
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
                dcResultado = AccountExtensions.ValidarExistenciaCPF(NuevePrimerosDigitos + PrimerDigitoValidar + SegundoDigitoValidar, CurrentAccount.AccountID);
                if (dcResultado.Count > 0)
                {
                    returnInt = 1;
                }
                else
                {
                    returnInt = 2;
                }

            }
            else
            {
                returnInt = 3;
            }

            return returnInt;

        }
        public int swValidarPIS(string TextoInputPIS)
        {
            int returnInt = 0;
            Boolean Resultado = false;

            TextoInputPIS = TextoInputPIS == null ? "" : TextoInputPIS.Trim();

            if (TextoInputPIS != "")
            {
                string NuevePrimerosDigitos = "";
                string PrimerDigito = "";
                string SegundoDigito = "";
                int SegundoDigitoValidar = 0;

                if (TextoInputPIS.Length < 11)
                {
                    Resultado = false;
                }
                else
                {
                    NuevePrimerosDigitos = TextoInputPIS == "" ? "" : TextoInputPIS.Substring(0, 9);
                    PrimerDigito = TextoInputPIS == "" ? "" : TextoInputPIS.Substring(9, 1);
                    SegundoDigito = TextoInputPIS == "" ? "" : TextoInputPIS.Substring(10, 1);
                    SegundoDigitoValidar = ValidarSegundoDigitoPIS(NuevePrimerosDigitos + PrimerDigito.ToString());

                    Resultado = (Convert.ToByte(SegundoDigito) == SegundoDigitoValidar);
                }

                if (Resultado)
                {
                    Dictionary<string, string> dcResultado = AccountExtensions.ValidarExistenciaPIS(NuevePrimerosDigitos + PrimerDigito.ToString() + SegundoDigitoValidar.ToString(), CurrentAccount.AccountID);

                    if (dcResultado.Count > 0)
                    {
                        returnInt = 1;
                    }
                    else
                    {
                        returnInt = 2;
                    }
                }
                else
                {
                    returnInt = 3;
                }
            }

            return returnInt;
        }

        public int swValidarRG(string TextoRG)
        {
            int returnInt = 0;
            TextoRG = TextoRG == null ? "" : TextoRG.Trim();
            Dictionary<string, string> dcResultado = AccountExtensions.ValidarExistenciaRG(TextoRG, CurrentAccount.AccountID);
            if ((TextoRG == null ? "" : TextoRG).Length == 0)
            {
                returnInt = 1;
            }
            if (dcResultado.Count > 1)
            {
                returnInt = 2;
            }
            else
            {
                returnInt = 3;
            }

            return returnInt;
        }
        protected virtual void UpdateAccountInfos(int accountId)
        { }

        /// <summary>
        /// This method is here to be overriden for clients who need to manage opt-out information
        /// </summary>
        /// <param name="account">
        /// The account.
        /// </param>
        /// <param name="optOut">
        /// The opt Out.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        protected virtual void ManageOptOut(Account account, bool optOut, string email)
        { }

        public virtual void SetAccountStatus(Account account, short accountstatus)
        {
            // Set the account status accordingly
            account.AccountStatusID = accountstatus;

            if (accountstatus == (short)Constants.AccountStatus.Active && !account.EnrollmentDate.HasValue)
                account.EnrollmentDate = DateTime.Now;

        }

        protected string FormatEmailAddress(string mailbox)
        {
            if (!mailbox.IsNullOrEmpty())
            {
                string domain = MailDomain.LoadDefaultForInternal().DomainName.ToLower();
                string emailAddress = string.Format("{0}@{1}", mailbox, domain);
                return emailAddress;

            }
            else
                return mailbox;
        }

        public virtual ActionResult SaveStatus(short statusId, short changeReasonId)
        {
            try
            {
                var account = CoreContext.CurrentAccount;

                if (account.AccountStatusID != statusId)
                {
                    // If they are changing from Terminated, remove the termination date
                    if (account.AccountStatusID == (short)Constants.AccountStatus.Terminated)
                    {
                        account.TerminatedDateUTC = null;
                    }

                    account.AccountStatusChangeReasonID = changeReasonId;
                    SetAccountStatus(account, statusId);

                    if (statusId == (short)Constants.AccountStatus.Terminated)
                    {
                        account.User.UserStatusID = (short)Constants.UserStatus.Inactive;
                        account.TerminatedDateUTC = DateTime.UtcNow;

                    }
                    else if (statusId == (short)Constants.AccountStatus.Active)
                    {
                        if (Equals(null, account.User))
                        {
                            Account.CreateNewDefaultUser(account);
                        }
                        else
                        {
                            account.User.UserStatusID = (short)Constants.UserStatus.Active;
                        }
                        var _accountDatos = Account.LoadAccountProperties(account.AccountID);

                        if (_accountDatos.AccountStatusID == (short)Constants.AccountStatus.Terminated)
                        {
                            var _retstatusID = AccountExtensions.UpdateAccountStatusByReEntryRules(account.AccountID);
                            if (_retstatusID != 0)
                                account.AccountStatusID = (short)_retstatusID;
                        }
                    }

                    account.Save();
                    CoreContext.CurrentAccount = account;

                    DisableSitesIfAccountStatusTerminated(account);
                    SetSiteStatus(account);
                    SetAutoshipOrdersStatus(account);
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Accounts-Edit EnrollmentDate", "~/Accounts")]
        public virtual ActionResult ChangeEnrollmentDate(string accountNumber, DateTime enrollmentDate)
        {
            int? accountID = null;
            try
            {
                Account account = Account.LoadByAccountNumber(accountNumber);
                accountID = account.AccountID;
                account.EnrollmentDate = enrollmentDate;
                account.Save();

                return Json(new
                {
                    result = true
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: accountID.ToIntNullable());
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Accounts-Edit NextRenewalDate", "~/Accounts")]
        public virtual ActionResult ChangeNextRenewalDate(string accountNumber, DateTime nextRenewal)
        {
            int? accountID = null;
            try
            {
                Account account = Account.LoadByAccountNumber(accountNumber);
                accountID = account.AccountID;
                account.NextRenewal = nextRenewal;
                account.Save();

                return Json(new
                {
                    result = true
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: accountID.ToIntNullable());
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual bool SaveSponsor(Account account, int accountId, string sponsorAccountNumber, string enrollerAccountNumber, ref string message)
        {
            bool returnValue = true;

            // Validate sponsor and enroller account numbers for distributors / preferred customers.
            bool accountTypeRequiresSponsorAndEnroller = (account.AccountTypeID == (int)Constants.AccountType.Distributor || account.AccountTypeID == (int)Constants.AccountType.PreferredCustomer) ? true : false;

            if (accountTypeRequiresSponsorAndEnroller && string.IsNullOrWhiteSpace(sponsorAccountNumber) && returnValue)
            {
                returnValue = false;
                message = Translation.GetTerm("AccountRequiresSponsor", "This account requires a sponsor.");
            }

            if (accountTypeRequiresSponsorAndEnroller && string.IsNullOrWhiteSpace(enrollerAccountNumber) && returnValue)
            {
                returnValue = false;
                message = Translation.GetTerm("AccountRequiresEnroller", "This account requires an enroller.");
            }

            // If AccountType requires sponsor and Enroller, and if they are not provided, returnValue will be false, so no need to validate and save new sponsor.
            if (returnValue && (account.SponsorInfo == null || account.SponsorInfo.AccountNumber != sponsorAccountNumber))
            {
                var sponsor = Account.LoadSlimByAccountNumber(sponsorAccountNumber);

                int enrollerID = 0;

                // For account types Customer/Contact/Employee, enroller will not be provided, where as placement might be there which needs to be updated.
                // If enroller is not provided also, update the Sponsor/Placement
                if (!string.IsNullOrWhiteSpace(enrollerAccountNumber))
                {
                    enrollerID = Account.LoadSlimByAccountNumber(enrollerAccountNumber).AccountID;
                }

                if (this.ValidateNewSponsor(accountId, sponsor.AccountID, enrollerID, ref message))
                {
                    // Save the sponsor to Core and Commissions database for distributors / preferred customers.
                    account.SponsorID = sponsor.AccountID;
                    account.Save();

                    this.SaveNewSponsor(accountId, sponsor.AccountID);
                }
                else
                {
                    returnValue = false;
                }
            }

            return returnValue;
        }

        public virtual bool SaveEnroller(Account account, int accountId, string sponsorAccountNumber, string enrollerAccountNumber, ref string message)
        {
            bool returnValue = true;

            // Validate sponsor and enroller account numbers for distributors / preferred customers.
            bool accountTypeRequiresSponsorAndEnroller = (account.AccountTypeID == (int)Constants.AccountType.Distributor || account.AccountTypeID == (int)Constants.AccountType.PreferredCustomer) ? true : false;

            if (accountTypeRequiresSponsorAndEnroller && string.IsNullOrWhiteSpace(sponsorAccountNumber) && returnValue)
            {
                returnValue = false;
                message = Translation.GetTerm("AccountRequiresSponsor", "This account requires a sponsor.");
            }

            if (accountTypeRequiresSponsorAndEnroller && string.IsNullOrWhiteSpace(enrollerAccountNumber) && returnValue)
            {
                returnValue = false;
                message = Translation.GetTerm("AccountRequiresEnroller", "This account requires an enroller.");
            }
            bool enrollerNumberChanged = (account.EnrollerInfo == null && !string.IsNullOrWhiteSpace(enrollerAccountNumber))
                || (account.EnrollerInfo != null && account.EnrollerInfo.AccountNumber != enrollerAccountNumber);
            if (returnValue && enrollerNumberChanged)
            {
                var sponsor = Account.LoadSlimByAccountNumber(sponsorAccountNumber);
                var enroller = Account.LoadSlimByAccountNumber(enrollerAccountNumber);

                if (this.ValidateNewEnroller(accountId, sponsor.AccountID, enroller.AccountID, ref message))
                {
                    // Save the enroller to Core and Commissions database for distributors / preferred customers.
                    account.EnrollerID = enroller.AccountID;
                    account.Save();

                    this.SaveNewEnroller(accountId, enroller.AccountID);
                }
                else
                {
                    returnValue = false;
                }
            }

            return returnValue;
        }

        public virtual bool SaveNewSponsor(int accountID, int sponsorID)
        {
            return true;
        }

        public virtual bool ValidateNewSponsor(int accountID, int sponsorID, int enrollerID, ref string message)
        {
            // Integrity check - commissions circular reference
            if (accountID == sponsorID)
            {
                return false;
            }

            return true;
        }

        public virtual bool SaveNewEnroller(int accountID, int enrollerID)
        {
            return true;
        }

        public virtual bool ValidateNewEnroller(int accountID, int sponsorID, int enrollerID, ref string message)
        {
            // Integrity check - commissions circular reference
            if (accountID == enrollerID)
            {
                return false;
            }

            return true;
        }



        #region Helpers

        protected void DisableSitesIfAccountStatusTerminated(Account account)
        {
            if (account.AccountID > 0 && account.AccountStatusID == Constants.AccountStatus.Terminated.ToShort())
            {
                var sites = Site.LoadByAccountID(account.AccountID);

                foreach (var site in sites)
                {
                    site.SiteStatusID = Constants.SiteStatus.InActive.ToShort();



                    site.Save();
                }
            }
        }

        protected void SetSiteStatus(Account account)
        {
            if (account.AccountID > 0 && (account.AccountStatusID == Constants.AccountStatus.Terminated.ToShort() || account.AccountStatusID == Constants.AccountStatus.Active.ToShort()))
            {
                var sites = Site.LoadByAccountID(account.AccountID);

                foreach (var site in sites)
                {
                    if (account.AccountStatusID == Constants.AccountStatus.Terminated.ToShort())
                    {
                        site.SiteStatusID = Constants.SiteStatus.InActive.ToShort();
                    }
                    else if (account.AccountStatusID == Constants.AccountStatus.Active.ToShort())
                    {
                        site.SiteStatusID = Constants.SiteStatus.Active.ToShort();
                    }
                    site.Save();
                }
            }
        }

        protected void SetAutoshipOrdersStatus(Account account)
        {
            if (account.AccountID > 0 && account.AccountStatusID == Constants.AccountStatus.Active.ToShort())
            {
                List<AutoshipOrder> accountAutoshipOrders = AutoshipOrder.LoadAllFullByAccountID(account.AccountID);

                foreach (var accountAutoshipOrder in accountAutoshipOrders)
                {
                    if (accountAutoshipOrder.Order.OrderStatusID != (short)Constants.OrderStatus.Cancelled)
                    {
                        accountAutoshipOrder.Order.OrderStatusID = (short)Constants.OrderStatus.Cancelled;
                        accountAutoshipOrder.Save();

                        DomainEventQueueItem.AddAutoshipCanceledEventToQueue(accountAutoshipOrder.Order.OrderID, bccToSponsor: true);
                    }
                }
            }
        }

        protected int GetIntFromString(string inputString)
        {
            int returnValue = 0;
            int.TryParse(inputString, out returnValue);
            return returnValue;
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
        public List<string> getFieldsEditAccount(int countryID, string filename, string tag)
        {
            List<string> campos = new List<string>();

            XmlDocument xDoc = new XmlDocument();
            string filePath = System.IO.Path.Combine(Server.MapPath("~/"), filename+".XML");
            xDoc.Load(filePath);

            var countryDate = SmallCollectionCache.Instance.Countries.GetById(countryID);

            XmlNodeList idioma = xDoc.GetElementsByTagName("IDIOMA");
            XmlNodeList lista = ((XmlElement)idioma[0]).GetElementsByTagName(countryDate.CountryCode3);
            
            foreach (XmlElement nodo in lista)
            {
                string sCan = nodo.GetAttribute("can");
                int can = sCan.ToInt();

                for (int i = 1; i <= can; i++)
                {
                    XmlNodeList nCampo = nodo.GetElementsByTagName("campo" + i);
                    campos.Add(nCampo[0].InnerText);
                } 
            }

            return campos;

        }
    }
}
