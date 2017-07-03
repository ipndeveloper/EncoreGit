using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Controls.Attributes;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Web.Mvc.Controls.Models.DisbursementProfiles;
using NetSteps.Web.Mvc.Extensions;
using nsDistributor.Areas.Enroll.Models.AccountInfo;

using NetSteps.Common.Extensions;
using NetSteps.Data.Common.Models;

namespace nsDistributor.Areas.Enroll.Controllers
{
    using NetSteps.Data.Entities.Cache;
    using NetSteps.Data.Entities.Generated;
    using NetSteps.Encore.Core.IoC;
    using NetSteps.Enrollment.Common.Models.Config;
    using NetSteps.Enrollment.Common.Models.Context;
    using NetSteps.Commissions.Common.Models;
    using NetSteps.Commissions.Common;
    using NetSteps.Web.Mvc.Controls.Models.Enrollment;
    using NetSteps.Data.Entities.Business.Logic;
    using NetSteps.Data.Entities.Business;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
    using nsDistributor.Models.Shared;
    using NetSteps.Security;
    //using nsDistributor.Areas.Enroll.Models.Products;

    public class AccountInfoController : EnrollStepBaseController
    {
        private readonly ICommissionsService _commissionsService = Create.New<ICommissionsService>();

        #region Actions
        [EnrollmentStepSection]
        public virtual ActionResult Index()
        {
            return RedirectToAction(GetSections().First().Action);
        }

        [EnrollmentStepSection]
        public virtual ActionResult BasicInfo()
        {
            int countryId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry_Aux"]);
            var account = GetEnrollingAccount();
            BasicInfoModel model = new BasicInfoModel();
            foreach (var item in PreOrderExtension.GetParameterCountriesByCountyIdStep(Country.Repository.Where(x => x.CountryID == countryId).FirstOrDefault().CountryID, 2))
            {
                model.ParameterCountries.Add(new ParameterCountryModel() { Id = item.Id, CountryId = item.CountryId, Controls = item.Controls, Active = item.Active, Descriptions = item.Descriptions, Step = item.Step, Sites = item.Sites });
            }

            return SectionsView(account);
        }

        [HttpPost, ValidateAntiForgeryToken, EnrollmentStepSection]
        public virtual ActionResult BasicInfo(BasicInfoModel model)
        {              
			var account = GetEnrollingAccount();              
            int countryId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry_Aux"]);
            if (countryId == (int)Constants.Country.UnitedStates)
            {
                SetLanguageAccount(model.CustomLanguageID);
                BasicInfo_ValidateTaxNumber(model, account);
                BasicInfo_ValidateEmailAvailability(model, account);

                foreach (var items in ModelState.Values)
                {
                    if (items.Errors.Count > 0)
                    {
                        string errorMessage = items.Errors[0].ErrorMessage;
                        if (errorMessage == _errorSSNAlreadyInUseString || errorMessage == _errorSSNInvalid || errorMessage == _errorEINAlreadyInUseString || 
                            errorMessage == _errorEmailAccountAlreadyExistsString || errorMessage == _errorEINInvalid ||
                            errorMessage == _errorEINInvalid ||  errorMessage == _errorDOBIsRequired)
                        {
                            if (!ModelState.IsValid)
                            {
                                foreach (var item in PreOrderExtension.GetParameterCountriesByCountyIdStep(Country.Repository.Where(x => x.CountryID == countryId).FirstOrDefault().CountryID, 2))
                                {
                                    model.ParameterCountries.Add(new ParameterCountryModel() { Id = item.Id, CountryId = item.CountryId, Controls = item.Controls, Active = item.Active, Descriptions = item.Descriptions, Step = item.Step, Sites = item.Sites });
                                } 
                                model.RGIssueDate = new Models.Shared.DateModel();
                                model.CountryID = countryId;
                                BasicInfo_LoadResources(model, account);
                                return SectionsView(account, model);
                            }
                        }  
                    }
                }
            }
            
           

            BasicInfo_ValidateEmailAvailability(model, account);
            //Removed Latitude and Longitude Validations
            ModelState.Remove("MainAddress.Latitude");
            ModelState.Remove("MainAddress.Longitude");
            if (model.Birthday == null)
            {
                model.Birthday = new Models.Shared.DateModel();
            }
            model.RGIssueDate = new Models.Shared.DateModel();
            model.CountryID = countryId;

            foreach (var item in PreOrderExtension.GetParameterCountriesByCountyIdStep(Country.Repository.Where(x => x.CountryID == countryId).FirstOrDefault().CountryID, 2))
            {
                model.ParameterCountries.Add(new ParameterCountryModel() { Id = item.Id, CountryId = item.CountryId, Controls = item.Controls, Active = item.Active, Descriptions = item.Descriptions, Step = item.Step, Sites = item.Sites });
            }
            if (countryId == (int)Constants.Country.Brazil)
            {
                if (!ModelState.IsValid)
                {
                    BasicInfo_LoadResources(model, account);
                    return SectionsView(account, model);
                }
            }
            try
            {
                BasicInfo_Complete(model, account);
            }
            catch (Exception ex)
            {
                AddErrorToViewData(ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage);
                BasicInfo_LoadResources(model, account);
                return SectionsView(account, model);
            }
            return SectionCompleted();
        }

        protected virtual void BasicInfo_Complete(BasicInfoModel model, Account account)
        {
            // Indicates whether to update the AccountID on orders after saving the account.
            var accountIDHasChanged = false;

            // Don't upgrade twice
            if (!_enrollmentContext.IsUpgrade)
            {

                // Upgrade any existing prospect
                var prospectAccount = BasicInfo_GetExistingProspect(model, account);    //move
                if (prospectAccount != null)
                {
                    // This will indicate to the ReviewController that it should set the Account Type on completion.
                    _enrollmentContext.IsUpgrade = true;

                    // This may seem redundant, but it allows SetEnrollingAccount() to control how the account is loaded from the database
                    account = SetEnrollingAccount(prospectAccount.AccountID);
                    accountIDHasChanged = true;

                    // Even though they may be enrolling as a PC/Distributor, make the prospect a retail customer for now to allow them to place an order.
                    account.AccountTypeID = (short)Constants.AccountType.RetailCustomer;
                }
            }
            
            //move to a single call to update basic info (apply basic info)
            // Apply updates
            BasicInfo_UpdateAccount(model, account);
            BasicInfo_UpdateAddress(model, account);
            BasicInfo_UpdateUser(model, account);

            // We only force password change on the first time through
            _enrollmentContext.ForcePasswordChange = false;

            account.TaxNumberCustom = NetSteps.Security.Encryption.EncryptTripleDES(model.CPF.Value);

            foreach (AccountPhone phone in account.AccountPhones)
            {
                if (phone.PhoneTypeID == 1)
                    phone.IsDefault = true;
                else
                {
                    phone.PhoneTypeID = (short)Constants.PhoneType.Other;
                }
            }

            account.Save();
            
            AccountExtensions.UpdateAccountsCommission(account.AccountID);
            AccountExtensions.AccountPropertyTypesIns(model.AuthNetworkData, model.AuthEmailSend, account.AccountID);

            Session["AccountId"] = account.AccountID;
            UpdateAddressStreet(account.Addresses);
            Save_PropertiesSuppliedIDs(account.AccountID, account.UserID.Value, model);
            // After saving account, update AccountID on orders if necessary.
            // This must happen after saving the account because orders lazy-load AccountInfo and we need to make sure the AccountInfo (i.e. AccountTypeID) is accurate.
            if (accountIDHasChanged)
            {
                // Change orders to use the new AccountID.
                SetAccountIDOnOrders(account.AccountID);
            }

            SetEnrollingAccount(account.AccountID);
        }

        private void UpdateAddressStreet(TrackableCollection<Address> Addresses)
        {
            AddressBusinessLogic business = new AddressBusinessLogic();

            foreach (Address address in Addresses)
            {
                business.UpdateAddressStreet(address);
            }
        }

        private void UpdateAddressStreet(Address Addresses)
        {
            AddressBusinessLogic business = new AddressBusinessLogic();
            business.UpdateAddressStreet(Addresses);
        }

        private void Save_PropertiesSuppliedIDs(int accountID, int userID, BasicInfoModel model)
        {

            try
            {               
                AccountSuppliedIDsBusinessLogic businesSupplied = new AccountSuppliedIDsBusinessLogic();
                AccountSuppliedIDsParameters accountSupplied = new AccountSuppliedIDsParameters();
                // PIS : 9
                accountSupplied = new AccountSuppliedIDsParameters();
                accountSupplied.AccountID = accountID;
                accountSupplied.IsPrimaryID = false;
                accountSupplied.IDTypeID = 9;
                accountSupplied.AccountSuppliedIDValue = model.PIS.Value ?? string.Empty;    
                businesSupplied = new AccountSuppliedIDsBusinessLogic();
                businesSupplied.Insert(accountSupplied);  
                // RG : 4            
                accountSupplied = new AccountSuppliedIDsParameters();
                accountSupplied.AccountID = accountID;
                accountSupplied.IsPrimaryID = false;
                accountSupplied.IDTypeID = 4;
                accountSupplied.AccountSuppliedIDValue = model.RG;
                if (model.RGIssueDate != null)
                    if (model.RGIssueDate.Date != null)
                        accountSupplied.IDExpeditionIDate = DateTime.Parse(model.RGIssueDate.Date.ToString());
                accountSupplied.ExpeditionEntity = model.OrgExp;
                businesSupplied = new AccountSuppliedIDsBusinessLogic();
                businesSupplied.Insert(accountSupplied);
                // CPF : 8          
                accountSupplied = new AccountSuppliedIDsParameters();
                accountSupplied.AccountID = accountID;
                accountSupplied.IsPrimaryID = true;
                accountSupplied.IDTypeID = 8;
                accountSupplied.AccountSuppliedIDValue = model.CPF.Value;
                businesSupplied = new AccountSuppliedIDsBusinessLogic();
                businesSupplied.Insert(accountSupplied);

                /*  Requirements
                    Account Supplied IDs Types  = Credit Requirement Types         
                        4	= 2  RG      
                        8	= 17 CPF
                        9	= 24 PIS
                */

                CreditRequirementsBusinessLogic busisnessRequirements = null;

                CreditRequirementSearchData requirementCPF = new CreditRequirementSearchData();
                requirementCPF.AccountID = accountID;
                requirementCPF.RequirementTypeID = 17;
                requirementCPF.RequirementStatusID = 1;
                requirementCPF.CreationDate = DateTime.Now;
                requirementCPF.LastModifiedDate = DateTime.Now;
                requirementCPF.UserCreatedID = userID;
                requirementCPF.LastUserModifiedID = userID;
                requirementCPF.Observations = string.Empty;
                busisnessRequirements = new CreditRequirementsBusinessLogic();
                busisnessRequirements.Insert(requirementCPF);

                CreditRequirementSearchData requirementPIS = new CreditRequirementSearchData();
                requirementPIS.AccountID = accountID;
                requirementPIS.RequirementTypeID = 24;
                requirementPIS.RequirementStatusID = 1;
                requirementPIS.CreationDate = DateTime.Now;
                requirementPIS.LastModifiedDate = DateTime.Now;
                requirementPIS.UserCreatedID = userID;
                requirementPIS.LastUserModifiedID = userID;
                requirementPIS.Observations = string.Empty;
                busisnessRequirements = new CreditRequirementsBusinessLogic();
                busisnessRequirements.Insert(requirementPIS);

                CreditRequirementSearchData requirementRG = new CreditRequirementSearchData();
                requirementRG.AccountID = accountID;
                requirementRG.RequirementTypeID = 2;
                requirementRG.RequirementStatusID = 1;
                requirementRG.CreationDate = DateTime.Now;
                requirementRG.LastModifiedDate = DateTime.Now;
                requirementRG.UserCreatedID = userID;
                requirementRG.LastUserModifiedID = userID;
                requirementRG.Observations = string.Empty;
                busisnessRequirements = new CreditRequirementsBusinessLogic();
                busisnessRequirements.Insert(requirementRG);

                AccountPropertiesBusinessLogic busines = new AccountPropertiesBusinessLogic();
                AccountPropertiesParameters property = new AccountPropertiesParameters();
                // Nationality : 1010
                property = new AccountPropertiesParameters();
                property.AccountID = accountID;
                property.AccountPropertyTypeID = 1010;
                property.AccountPropertyValueID = Convert.ToInt32(model.Nationality); 
                property.Active =true;        
                busines = new AccountPropertiesBusinessLogic();
                busines.Insert(property);
                // Marital Status : 1009
                property = new AccountPropertiesParameters();
                property.AccountID = accountID;
                property.AccountPropertyTypeID = 1009;
                property.AccountPropertyValueID = Convert.ToInt32(model.MaritalStatus);
                property.Active = true;
                busines = new AccountPropertiesBusinessLogic();
                busines.Insert(property);
                // Occupation :1011
                property = new AccountPropertiesParameters();
                property.AccountID = accountID;
                property.AccountPropertyTypeID = 1011;
                property.AccountPropertyValueID = Convert.ToInt32(model.Occupation);
                property.Active = true;
                busines = new AccountPropertiesBusinessLogic();
                busines.Insert(property);     
            }
            catch (Exception ex)
            {
                //var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                //return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        [EnrollmentStepSection]
        public virtual ActionResult AboutYou()
        {
            var account = GetEnrollingAccount();

            return SectionsView(account);
        }

        [HttpPost, ValidateAntiForgeryToken, EnrollmentStepSection]
        public virtual ActionResult AboutYou(AboutYouModel model)
        {
            var account = GetEnrollingAccount();

            if (!ModelState.IsValid)
            {
                return SectionsView(account, model);
            }

            try
            {
                // Apply updates
                foreach (AccountPropertyModel property in model.AccountPropertyList)
                {
                    // if the property doesn't already exist in the account object, add it
                    // otherwise, update the existing one
                    var selectedProperty = account.AccountProperties.FirstOrDefault(x => x.AccountPropertyTypeID == property.AccountPropertyTypeID);
                    if (selectedProperty != null)
                    {
                        selectedProperty.PropertyValue = property.PropertyValue;
                    }
                    else
                    {
                        account.AccountProperties.Add(new AccountProperty
                        {
                            PropertyValue = property.PropertyValue,
                            AccountPropertyTypeID = property.AccountPropertyTypeID,
                            AccountPropertyValueID = property.AccountPropertyValueID,
                        });
                    }
                }


            }
            catch (Exception ex)
            {
                AddErrorToViewData(ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage);
                return SectionsView(account, model);
            }

            return SectionCompleted();
        }

        [EnrollmentStepSection]
        public virtual ActionResult Shipping()
        {
            /*CS.17MAY2016.Inicio.Obtener ultimo datos de la cuenta*/
            var account = GetEnrollingAccount();
            //var account = Account.LoadFull(accountEnroll.AccountID);
            /*CS.17MAY2016.Fin*/
            return this.SectionsView(account);
        }

        [HttpPost, ValidateAntiForgeryToken, EnrollmentStepSection]
        public virtual ActionResult Shipping(ShippingModel model)
        {
            var account = GetEnrollingAccount();

            //Removed Latitude and Longitude Validations
            ModelState.Remove("ShippingAddress.Latitude");
            ModelState.Remove("ShippingAddress.Longitude");  

            if (!ModelState.IsValid)
            {
                Shipping_LoadResources(model, account);
                return SectionsView(account, model);
            }
             
            try
            {
                // Apply updates
                Shipping_UpdateAddress(model, account);
                account.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping).Save();
                
                //account.Save();

                // Update context
                Shipping_UpdateContext(model, account);
            }
            catch (Exception ex)
            {
                AddErrorToViewData(ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage);
                Shipping_LoadResources(model, account);
                return SectionsView(account, model);
            }

            _enrollmentContext.EnrollingAccount = account;
            return SectionCompleted();
        }


        [EnrollmentStepSection]
        public virtual ActionResult AdditionalInfo()
        {
            var account = GetEnrollingAccount();
            CoreContext.CurrentAccount = account;
            return SectionsView(account);
        }

        [EnrollmentStepSection]
        public virtual ActionResult ReferralInfo()
        {
            var account = GetEnrollingAccount();

            return SectionsView(account);
        }

        [HttpPost, ValidateAntiForgeryToken, EnrollmentStepSection]
        public virtual ActionResult AdditionalInfo(AdditionalInformationModel model)
        {
            var account = GetEnrollingAccount();


            if (!ModelState.IsValid)
            {

                AdditionalInfo_LoadResources(model, account);
                return SectionsView(account, model);
            }
            else { Save_References(account.AccountID, model); }

            // Update context
            //AdditionalInfo_UpdateContext(model, account);

            return SectionCompleted();
        }

        private void Save_References(int accountID, AdditionalInformationModel model)
        {

            try
            {
                //AccountPropertiesParameters referenceDat = new AccountPropertiesParameters();
                //AccountReferencesBusinessLogic referenceBusines = new AccountReferencesBusinessLogic();           
                //referenceDat.AccountID = accountID;
                //referenceDat.ReferenceName = model.Name;
                //referenceDat.PhoneNumberMain = !string.IsNullOrEmpty(model.MainPhone.Value) ? Int64.Parse(model.MainPhone.Value) : 0;
                //referenceDat.RelationShip = Convert.ToInt32(model.Relationship);               
                //referenceBusines.Insert(referenceDat);


                AccountPropertiesBusinessLogic busines = new AccountPropertiesBusinessLogic();
                AccountPropertiesParameters property = new AccountPropertiesParameters();
                // Schooling Level : 1005
                property = new AccountPropertiesParameters();
                property.AccountID = accountID;
                property.AccountPropertyTypeID = 1005;
                property.AccountPropertyValueID = Convert.ToInt32(model.SchoolineLevel);
                property.Active = true;
                busines = new AccountPropertiesBusinessLogic();
                busines.Insert(property);

                AccountPropertiesParameters AuthNetworkData = new AccountPropertiesParameters()
                {
                    AccountID = accountID,
                    AccountPropertyTypeID = 1006,
                    PropertyValue = model.AuthNetworkData.ToString(),
                    Active = true
                };

                AccountPropertiesParameters AuthEmailSend = new AccountPropertiesParameters()
                {
                    AccountID = accountID,
                    AccountPropertyTypeID = 1007,
                    PropertyValue = model.AuthEmailSend.ToString(),
                    Active = true
                };

                AccountPropertiesParameters AuthShareData = new AccountPropertiesParameters()
                {
                    AccountID = accountID,
                    AccountPropertyTypeID = 1008,
                    PropertyValue = model.AuthShareData.ToString(),
                    Active = true
                };


                busines.Insert(AuthNetworkData);
                busines.Insert(AuthEmailSend);
                busines.Insert(AuthShareData);

                // First domain event
                //try
                //{

                //    if (accountID != null)
                //    {
                //        DomainEventQueueItem.AddEnrollmentCompletedEventToQueue(accountID,(short)Constants.AccountType.Distributor);
                        
                //    }
                   
                //}
                //catch (Exception ex)
                //{
                //    // Just log and continue
                //    ex.Log(accountID: accountID  != null ? (int?)accountID : null);
                //}

                // Second domain event
                try
                {
                   
                            DomainEventQueueItem.AddDistributorJoinsDownlineEventToQueue(1, accountID);
                }
                catch (Exception ex)
                {
                    // Just log and continue
                    ex.Log(accountID: accountID != null ? (int?)accountID : null);
                }

                
            }
            catch (Exception ex)
            {
                //var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                //return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [HttpPost, ValidateAntiForgeryToken, EnrollmentStepSection]
        public virtual ActionResult ReferralInfo(ReferralInfoModel model)
        {
            var account = GetEnrollingAccount();

            if (!ModelState.IsValid)
            {
                ReferralInfo_LoadResources(model, account);
                return SectionsView(account, model);
            }

            // Update context
            ReferralInfo_UpdateContext(model, account);

            return SectionCompleted();
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult SearchSponsor(string query)
        {
            try
            {
                return Json(AccountCache.GetAccountSearchByTextAndAccountStatusResults(query, (int)ConstantsGenerated.AccountStatus.Active).ToAJAXSearchResults());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [EnrollmentStepSection]
        public virtual ActionResult DisbursementProfiles()
        {
            var account = GetEnrollingAccount();

            return SectionsView(account);
        }

        /// <summary>
        /// saves the disbursement profiles
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="preference">
        /// The preference.
        /// </param>
        /// <param name="useAddressOfRecord">
        /// The use address of record.
        /// </param>
        /// <param name="isActive">
        /// The is active.
        /// </param>
        /// <param name="profileName">
        /// The profile name.
        /// </param>
        /// <param name="payableTo">
        /// The payable to.
        /// </param>
        /// <param name="address1">
        /// The address 1.
        /// </param>
        /// <param name="address2">
        /// The address 2.
        /// </param>
        /// <param name="address3">
        /// The address 3.
        /// </param>
        /// <param name="city">
        /// The city.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="agreementOnFile">
        /// The agreement on file.
        /// </param>
        /// <param name="accounts">
        /// The accounts.
        /// </param>
        /// <returns>
        /// returns a view
        /// </returns>
        [HttpPost, EnrollmentStepSection]
        public ActionResult DisbursementProfiles(
              int id
            , DisbursementMethodKind preference
            , bool? useAddressOfRecord
            , bool? isActive
            , string profileName
            , string payableTo
            , string address1
            , string address2
            , string address3
            , string city
            , string state
            , string zip
            , bool? agreementOnFile
            , List<EFTAccountModel> accounts)
        {
            var account = GetEnrollingAccount();

            try
            {
                var stateCode = GetStateCode(state);
                int stateId;
                Int32.TryParse(state, out stateId);

                //Commissions.DisbursementProfile.DeleteByAccountID(account.AccountID);
                var service = Create.New<ICommissionsService>();
                var viewAddress = new Address
                {
                    ProfileName = profileName,
                    Attention = payableTo,
                    AddressTypeID = (int)ConstantsGenerated.AddressType.Disbursement,
                    Address1 = address1,
                    Address2 = address2,
                    Address3 = address3,
                    City = city,
                    State = stateCode,
                    StateProvinceID = stateId,
                    PostalCode = zip,
                    CountryID = (int)ConstantsGenerated.Country.Brazil
                };

                useAddressOfRecord = useAddressOfRecord ?? false;
                agreementOnFile = agreementOnFile ?? false;
                service.SaveDisbursementProfile(id, account, viewAddress,
                    preference, (bool)useAddressOfRecord, accounts.Select(x => x.Convert()), (bool)agreementOnFile);

                return SectionCompleted();
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (account != null) ? (int?)account.AccountID : null);
                return JsonError(exception.PublicMessage);
            }
        }

        /// <summary>
        /// Takes in a state value and will return the state code if it is an ID
        /// </summary>
        /// <param name="state">state value</param>
        /// <returns>state abbreviation</returns>
        private string GetStateCode(string state)
        {
            //set statecode to whatever is currently in state
            var stateCode = state;

            //If state is an int, get the ID from cache
            int stateId;
            if (Int32.TryParse(state, out stateId))
            {
                var stateCodes = SmallCollectionCache.Instance.StateProvinces;
                stateCode = stateCodes.GetById(stateId).StateAbbreviation;
            }
            return stateCode;
        }
        #endregion

        #region BasicInfo Helpers
        protected virtual BasicInfoModel BasicInfo_CreateModel()
        {
            return new BasicInfoModel();
        }

        protected virtual void BasicInfo_LoadModel(BasicInfoModel model, Account account, bool active)
        {
            if (account.IsTempAccount)
            {
                account.FirstName = null;
                account.LastName = null;
            }

            var mainAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main) ?? new Address();

            bool showTaxNumber = _enrollmentContext.AccountTypeID == (short)Constants.AccountType.Distributor;

            model.LoadValues(
                Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry"]),
                account,
                mainAddress,
                _enrollmentContext.ForcePasswordChange,
                showTaxNumber
            );

            int countryId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry"]);
            //model.Birthday = new Models.Shared.DateModel();
            model.RGIssueDate = new Models.Shared.DateModel();
            model.CountryID = countryId;
            if (active)
            {
                foreach (var item in PreOrderExtension.GetParameterCountriesByCountyIdStep(Country.Repository.Where(x => x.CountryID == countryId).FirstOrDefault().CountryID, 2))
                {
                    model.ParameterCountries.Add(new ParameterCountryModel() { Id = item.Id, CountryId = item.CountryId, Controls = item.Controls, Active = item.Active, Descriptions = item.Descriptions, Step = item.Step, Sites = item.Sites });
                }
                
                BasicInfo_LoadResources(model, account);
            }
        }

        protected virtual void BasicInfo_LoadResources(BasicInfoModel model, Account account)
        {
            model.LoadResources();
        }

        protected virtual void BasicInfo_ValidateTaxNumber(BasicInfoModel model, Account account)
        {
            if (account.EnforceUniqueTaxNumber() && !account.IsTaxNumberAvailable(model.TaxNumber))
            {
                if (model.IsEntity)
                {
                    ModelState.AddModelError("EIN", _errorEINAlreadyInUseString);
                }
                else
                {
                    ModelState.AddModelError("SSN", _errorSSNAlreadyInUseString);
                }
            }
        }

        /// <summary>
        /// Checks the database for an existing Prospect account with the same email and sponsor.
        /// </summary>
        /// <returns>The existing Prospect account</returns>
        protected virtual Account BasicInfo_GetExistingProspect(BasicInfoModel model, Account account)
        {
            /* 2011-11-03, JWL
             * If the enrolling account already exists in Accounts as a prospect, check the SponsorID of the existing record.
             * If the SponsorID is the same as the current account(?), then upgrade the account.
             * If the SponsorID is not the same, then allow a duplicate to be created.
             * If the enrolling account already exists and is not a prospect, then throw an error
             */
            if (account.SponsorID == null
                || string.IsNullOrWhiteSpace(model.Email))
            {
                return null;
            }

            Account existingAccount = Account.GetAccountByEmailAndSponsorID(model.Email, account.SponsorID.Value);

            if (existingAccount == null
                || existingAccount.AccountTypeID != (int)Constants.AccountType.Prospect)
            {
                return null;
            }

            return existingAccount;
        }

        protected virtual void BasicInfo_ValidateEmailAvailability(BasicInfoModel model, Account account)
        {
            // Don't worry about blank emails here
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                return;
            }

            // Emails must be unique for all active accounts, except Prospects
            if (Account.NonProspectNonExpressAccountExists(model.Email, account.AccountID))
                ModelState.AddModelError("Email", _errorEmailAccountAlreadyExistsString);
        }

        protected virtual void BasicInfo_UpdateAccount(BasicInfoModel model, Account account)
        {
            account.IsTempAccount = false;

            // if the account was upgraded it may not have an enroller id, so set now. 
            if (account.EnrollerID == null)
            {
                account.EnrollerID = _enrollmentContext.EnrollerID ?? _enrollmentContext.SponsorID;
            }
            // Set properties from model
            model.ApplyTo(account);
        }

        protected virtual void BasicInfo_UpdateUser(BasicInfoModel model, Account account)
        {
            // Create user
            if (account.UserID == null)
            {
                // Must save user before attaching it to account - JGL
                var user = new User
                {
                    UserTypeID = (short)Constants.UserType.Distributor,

                    // Default to "Inactive" until enrollment is complete.
                    UserStatusID = (short)Constants.UserStatus.Active,// .Inactive,
                     
                    DefaultLanguageID = _enrollmentContext.LanguageID,

                    // Username always starts out as the account number to prevent conflicts,
                    // but it can be changed once the enrollment is completed. See ReviewController.GetUsername().
                    Username = account.AccountNumber
                };
                user.Save();
                account.User = user;
            }

            // Set properties from context
            account.User.DefaultLanguageID = _enrollmentContext.LanguageID;

            // Set properties from model
            model.ApplyTo(account.User);
        }

        protected virtual void BasicInfo_UpdateAddress(BasicInfoModel model, Account account)
        {
            // Load/Create Address
            var mainAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main);
            if (mainAddress == null)
            {
                mainAddress = new Address();
                mainAddress.FirstName = account.FirstName;
                mainAddress.LastName = account.LastName;
                mainAddress.AddressTypeID = (short)Constants.AddressType.Main;
                mainAddress.IsDefault = true;
                mainAddress.ProfileName = Translation.GetTerm("Main");
                account.Addresses.Add(mainAddress);
            }

            // Set properties from model
            model.ApplyTo(mainAddress);

            // Update other addresses if necessary
            if (_enrollmentContext.IsSameShippingAddress
                || _enrollmentContext.EnrollmentConfig.BasicInfo.SetShippingAddressFromMain)
            {
                var shippingAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping);

                if (shippingAddress == null)
                {
                    shippingAddress = new Address();
                    shippingAddress.FirstName = account.FirstName;
                    shippingAddress.LastName = account.LastName;
                    shippingAddress.AddressTypeID = (short)Constants.AddressType.Shipping;
                    shippingAddress.IsDefault = true;
                    shippingAddress.ProfileName = Translation.GetTerm("DefaultShipping", "Default Shipping");
                    account.Addresses.Add(shippingAddress);
                }

                model.MainAddress.ApplyTo(shippingAddress);

                this.UpdateAllOrderShipmentAddresses(shippingAddress);
            }

            if (_enrollmentContext.BillingAddressSourceTypeID == (int)Constants.AddressType.Main
                || (_enrollmentContext.BillingAddressSourceTypeID == (int)Constants.AddressType.Shipping && _enrollmentContext.IsSameShippingAddress)
                || _enrollmentContext.EnrollmentConfig.BasicInfo.SetBillingAddressFromMain)
            {
                var billingAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Billing);

                if (billingAddress == null)
                {
                    billingAddress = new Address();
                    billingAddress.FirstName = account.FirstName;
                    billingAddress.LastName = account.LastName;
                    billingAddress.AddressTypeID = (short)Constants.AddressType.Billing;
                    billingAddress.IsDefault = true;
                    billingAddress.ProfileName = Translation.GetTerm("DefaultBilling", "Default Billing");
                    account.Addresses.Add(billingAddress);
                }

                model.MainAddress.ApplyTo(billingAddress);

                this.UpdateAccountPaymentMethodAddress(account);
                
            }
        }
        #endregion

        #region DisbursementProfile Helpers
        protected virtual DisbursementProfileModel DisbursementProfiles_CreateModel()
        {
            var disbursementProfile = new DisbursementProfileModel();
            var account = GetEnrollingAccount();
            disbursementProfile.ViewModel.CreateModel(account ?? new Account());
            disbursementProfile.ViewModel.PostalCodeLookupURL = "/Checkout/LookupZip";
            //disbursementProfile.ViewModel.EFTDisbursementViewModel.PostalCodeLookupURL = "/Checkout/LookupZip";

            //Shows or Hides Second Disbursement Profile Account
            ViewBag.DPA = OrderExtensions.GeneralParameterVal(56, "DPA");
            ViewBag.AccountName = account.FirstName + " " + account.MiddleName + " " + account.LastName;
            return disbursementProfile;

        }

        protected virtual void DisbursementProfiles_LoadModel(DisbursementProfileModel model, Account account, bool active)
        { }

        protected virtual void DisbursementProfiles_LoadResources(DisbursementProfileModel model)
        { }

        #endregion

        #region About You Helpers
        protected virtual AboutYouModel AboutYou_CreateModel()
        {
            return new AboutYouModel();
        }

        protected virtual void AboutYou_LoadModel(AboutYouModel model, Account account, bool active)
        {
            AboutYou_LoadValues(model, account);
            AboutYou_LoadResources(model, account);

            // Uncomment this for convenience while developing but don't check it in!
            //if (Request.IsLocal)
            //{
            //    foreach (AccountPropertyModel p in model.AccountPropertyList.Where(x => String.IsNullOrEmpty(x.PropertyValue)))
            //    {
            //        p.PropertyValue = String.Format("Answer_{0}", p.AccountPropertyTypeID);
            //    }
            //}
        }

        protected virtual void AboutYou_LoadValues(AboutYouModel model, Account account)
        {
            var modelToPass = new AccountPropertiesModel
            {
                AccountPropertyTypes = AccountPropertyType.LoadAll(),
                AccountProperties = account.AccountProperties.ToList()
            };

            model.LoadValues(modelToPass);
        }

        protected virtual void AboutYou_LoadResources(AboutYouModel model, Account account)
        {
            var modelToPass = new AccountPropertiesModel
            {
                AccountPropertyTypes = AccountPropertyType.LoadAll(),
                AccountProperties = account.AccountProperties.ToList()
            };

            model.LoadResources(modelToPass);
        }
        #endregion

        #region Shipping Helpers
        protected virtual ShippingModel Shipping_CreateModel()
        {
            return new ShippingModel();
        }

        protected virtual void Shipping_LoadModel(ShippingModel model, Account account, bool active)
        {
            var shippingAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping) ?? new Address();
            shippingAddress.Name = shippingAddress.Street;
            model.LoadValues(
                _enrollmentContext.IsSameShippingAddress,
                Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry"]),
                shippingAddress);

            // We only need this if the section is active
            if (active)
            {
                Shipping_LoadResources(model, account);
            }
        }

        protected virtual void Shipping_LoadResources(ShippingModel model, Account account)
        {
            var mainAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main) ?? new Address();                     
            mainAddress.Name = mainAddress.Street;
            model.LoadResources(mainAddress);
        }

        protected virtual void Shipping_UpdateAddress(ShippingModel model, Account account)
        {
            // Load/Create Address
            var shippingAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping);
            if (shippingAddress == null)
            {
                shippingAddress = new Address();
                shippingAddress.AddressTypeID = (short)Constants.AddressType.Shipping;
                shippingAddress.IsDefault = true;
                shippingAddress.ProfileName = Translation.GetTerm("DefaultShipping", "Default Shipping");
                account.Addresses.Add(shippingAddress);
            }

            // This is used if IsSameShippingAddress is true
            var mainAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main) ?? new Address();

            // Set properties from model
            model.ApplyTo(shippingAddress, mainAddress);

            // Update billing addresses if necessary
            if (_enrollmentContext.BillingAddressSourceTypeID == (int)Constants.AddressType.Shipping)
            {
                var billingAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Billing);
                if (billingAddress != null)
                {
                    model.ShippingAddress.ApplyTo(billingAddress);
                    UpdateAccountPaymentMethodAddress(account);
                }
            }

            UpdateAllOrderShipmentAddresses(shippingAddress);
            UpdateAddressStreet(shippingAddress);
            
        }

        protected virtual void Shipping_UpdateContext(ShippingModel model, Account account)
        {
            model.ApplyTo(_enrollmentContext);
        }
        #endregion


        #region AdditionalInfo Helpers
        protected virtual AdditionalInformationModel AdditionalInfo_CreateModel()
        {
            return new AdditionalInformationModel();
        }

        protected virtual void AdditionalInfo_LoadModel(AdditionalInformationModel model, Account account, bool active)
        {
            if (account.IsTempAccount)
            {
                account.FirstName = null;
                account.LastName = null;
            }

            var mainAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main) ?? new Address();

            bool showTaxNumber = _enrollmentContext.AccountTypeID == (short)Constants.AccountType.Distributor;

            model.LoadValues(
                (int)Constants.Country.Brazil,
                account,
                mainAddress,
                _enrollmentContext.ForcePasswordChange,
                showTaxNumber
            );

            if (active)
            {
                // Uncomment this for convenience while developing but don't check it in!
                //if (Request.IsLocal)
                //{
                //    model.FirstName = model.FirstName ?? "John";
                //    model.LastName = model.LastName ?? "Doe";
                //    model.Email = model.Email ?? "test" + new Random().Next(100000000, 999999999).ToString() + "@test.com";
                //    model.Password = model.Password ?? "123";
                //    model.ConfirmPassword = model.ConfirmPassword ?? "123";
                //    model.MainAddress.Address1 = model.MainAddress.Address1 ?? "123 South";
                //    model.MainAddress.PostalCode1 = model.MainAddress.PostalCode1 ?? "12345";
                //    if (model.Birthday.IsBlank)
                //        model.Birthday.Date = new DateTime(1960, 1, 1);
                //    if (model.MainPhone.IsBlank)
                //        model.MainPhone.LoadValues("5555555555", _enrollmentContext.CountryID.Value);
                //    if (model.SSN.IsBlank)
                //        model.SSN.LoadValues(new Random().Next(100000000, 999999999).ToString(), false, _enrollmentContext.CountryID.Value);
                //}

                AdditionalInfo_LoadResources(model, account);
            }
        }

        protected virtual void AdditionalInfo_LoadResources(AdditionalInformationModel model, Account account)
        {
            model.LoadResources();
        }



        protected virtual void AdditionalInfoInfo_UpdateAccount(AdditionalInformationModel model, Account account)
        {
            account.IsTempAccount = false;

            // if the account was upgraded it may not have an enroller id, so set now. 
            if (account.EnrollerID == null)
            {
                account.EnrollerID = _enrollmentContext.EnrollerID ?? _enrollmentContext.SponsorID;
            }
            // Set properties from model
            //model.ApplyTo(account);
        }

        protected virtual void AdditionalInfoInfo_UpdateUser(AdditionalInformationModel model, Account account)
        {
            // Create user
            if (account.UserID == null)
            {
                // Must save user before attaching it to account - JGL
                var user = new User
                {
                    UserTypeID = (short)Constants.UserType.Distributor,

                    // Default to "Inactive" until enrollment is complete.
                    UserStatusID = (short)Constants.UserStatus.Inactive,

                    DefaultLanguageID = _enrollmentContext.LanguageID,

                    // Username always starts out as the account number to prevent conflicts,
                    // but it can be changed once the enrollment is completed. See ReviewController.GetUsername().
                    Username = account.AccountNumber
                };
                user.Save();
                account.User = user;
            }

            // Set properties from context
            account.User.DefaultLanguageID = _enrollmentContext.LanguageID;

            // Set properties from model
            //model.ApplyTo(account.User);
        }

        #endregion

        #region Referral Information Helpers
        protected virtual ReferralInfoModel ReferralInfo_CreateModel()
        {
            return new ReferralInfoModel();
        }

        protected virtual void ReferralInfo_LoadModel(ReferralInfoModel model, Account account, bool active)
        {
        }

        protected virtual void ReferralInfo_LoadResources(ReferralInfoModel model, Account account)
        {
        }

        protected virtual void ReferralInfo_UpdateContext(ReferralInfoModel model, Account account)
        {
        }
        #endregion

        #region Other Helpers
        protected virtual ActionResult SectionsView(Account account, SectionModel currentSectionModel = null)
        {
            return this.View("Index", this.GetSectionModels(account, currentSectionModel));
        }

        protected virtual OrderedList<IEnrollmentStepSectionConfig> GetSections()
        {
            return _enrollmentContext.EnrollmentConfig.Steps.CurrentItem.Sections;
        }

        protected virtual IEnumerable<SectionModel> GetSectionModels(Account account, SectionModel currentSectionModel = null)
        {
            var sections = GetSections();
            var models = new List<SectionModel>();
            int contar = 0;
            foreach (var item in  sections)
            {
                contar++;
                if (item.Action == "AdditionalInfo")
                {
                    sections.RemoveAt(contar-1);
                    break;
                }
            }
            //
            char titleIndex = 'a';
            int count = 0;
            foreach (var section in sections)
            {
                count++;
               
                SectionModel model = null;
                bool active = section == sections.CurrentItem;

                // Only "load" models that are going to be rendered (i.e. active & completed sections).
                bool loadModel = active || section.Completed;

                // If this is a post, the current model is already loaded.
                if (active)
                {
                    model = currentSectionModel;
                }

                if (model == null)
                {
                    // Create and load the model using the appropriate section methods.
                    model = CreateSectionModel(section);
                    if (loadModel)
                    {
                        LoadSectionModel(section, model, account, active);
                    }
                }

                if (model == null)
                {
                    throw new Exception(string.Format("Error loading AccountInfo model for section '{0}'.", section.Name));
                }
                //if (count == 3)
                //{
                //    active = true;
                //}
                //if (count == 4)
                //{
                //    active = false;
                //    section.Completed = false; 
                //}
                model.LoadBaseResources(
                    active,
                    section.Action,
                    string.Format("{0}. {1}", titleIndex++, Translation.GetTerm(section.TermName, section.Name)),
                    string.Format("_{0}{1}", section.Action, active ? "Edit" : "Details"),
                    section.Completed
                );

                models.Add(model);
            }

            return models;
        }

        /// <summary>
        /// Finds and calls the appropriate 'CreateModel' method to instantiate a new section model.
        /// Every section in the enrollment config must have a corresponding 'CreateModel' method
        /// in this controller and the method must be named [Action]_CreateModel.
        /// Example: protected virtual BasicInfoModel BasicInfo_CreateModel()
        /// </summary>
        /// <param name="section">The <see cref="SectionConfig"/> object describing the section</param>
        /// <returns>A new section model for the provided section</returns>
        protected virtual SectionModel CreateSectionModel(IEnrollmentStepSectionConfig section)
        {
            if (section == null)
            {
                throw new ArgumentNullException("section");
            }

            // The controller type to search
            var type = this.GetType();

            // The method name to find
            var createModelMethodName = section.Action + "_CreateModel";

            // Find the 'CreateModel' method
            var createModelMethod = type
                .FindMembers(
                    System.Reflection.MemberTypes.Method,
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                    Type.FilterNameIgnoreCase,
                    createModelMethodName
                )
                .FirstOrDefault()
                as System.Reflection.MethodBase;

            if (createModelMethod == null)
            {
                throw new InvalidOperationException(string.Format("Method '{0}' not found on type '{1}'.",
                    createModelMethodName, type.Name));
            }

            // Invoke the 'CreateModel' method and return the result
            return createModelMethod.Invoke(this, null) as SectionModel;
        }

        /// <summary>
        /// Finds and calls the appropriate 'LoadModel' method to load a section model.
        /// Every section in the enrollment config must have a corresponding 'LoadModel' method in this
        /// controller, the method must be named [Action]_LoadModel, and must take the same three arguments.
        /// Example: protected virtual void BasicInfo_LoadModel(BasicInfoModel model, Account account, bool active)
        /// </summary>
        /// <param name="section">The <see cref="SectionConfig"/> object describing the section</param>
        /// <returns>A new section model for the provided section</returns>
        protected virtual SectionModel LoadSectionModel(IEnrollmentStepSectionConfig section, SectionModel model, Account account, bool active)
        {
            if (section == null)
            {
                throw new ArgumentNullException("section");
            }
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            // The controller type to search
            var type = this.GetType();

            // The method name to find
            var loadModelMethodName = section.Action + "_LoadModel";

            // Find the 'LoadModel' method
            var loadModelMethod = type
                .FindMembers(
                    System.Reflection.MemberTypes.Method,
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                    Type.FilterNameIgnoreCase,
                    loadModelMethodName
                )
                .FirstOrDefault()
                as System.Reflection.MethodBase;

            if (loadModelMethod == null)
            {
                throw new InvalidOperationException(string.Format("Method '{0}' not found on type '{1}'.",
                    loadModelMethodName, type.Name));
            }

            // Invoke the 'LoadModel' method
            loadModelMethod.Invoke(this, new object[] { model, account, active });

            return model;
        }

        protected virtual ActionResult SectionCompleted()
        {
            var sections = GetSections();

            sections.CurrentItem.Completed = true;

            if (!string.IsNullOrWhiteSpace(_enrollmentContext.ReturnUrl))
            {
                string returnUrl = _enrollmentContext.ReturnUrl;
                _enrollmentContext.ReturnUrl = null;
                return Redirect(returnUrl);
            }

            if (sections.HasNextItem)
            {
                return RedirectToAction(sections.NextItem.Action);
            }
            else
            {
                return StepCompleted();
            }
        }

        /// <summary>
        /// Temporary constructor until we wireup DI.
        /// </summary>
        public AccountInfoController() : base() { }

        /// <summary>
        /// Testing constructor.
        /// </summary>
        public AccountInfoController(IEnrollmentContext<EnrollmentKitConfig> enrollmentContext) : base(enrollmentContext) { }
        #endregion

        #region Strings

        protected virtual string _errorLoadingAccountInfoString { get { return Translation.GetTerm("ErrorLoadingAccountInfo", "There was an error loading your account info, please try again."); } }
        protected virtual string _errorSSNAlreadyInUseString { get { return Translation.GetTerm("ErrorSSNAlreadyInUse", "That SSN is already being used by another account."); } }
        protected virtual string _errorEINAlreadyInUseString { get { return Translation.GetTerm("ErrorEINAlreadyInUse", "That EIN is already being used by another account."); } }
        protected virtual string _errorEmailAccountAlreadyExistsString { get { return Translation.GetTerm("EmailAccountAlreadyExists", "An account with this e-mail already exists."); } }
        protected virtual string _errorSSNInvalid { get { return Translation.GetTerm("ErrorFieldInvalids", "SSN is invalid."); } }
        protected virtual string _errorEINInvalid { get { return Translation.GetTerm("EINisInvalid", "EIN is invalid."); } }
        protected virtual string _errorDOBIsRequired { get { return Translation.GetTerm("DOBIsRequired", "DOB is required."); } }

        
        
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
                int accountID = _enrollmentContext.EnrollingAccount.AccountID;
                dcResultado = AccountExtensions.ValidarExistenciaCPF(NuevePrimerosDigitos + PrimerDigitoValidar + SegundoDigitoValidar, accountID);
                if (dcResultado.Count > 0)
                {
                    return Json(
                        new
                        {
                            mensaje =  Translation.GetTerm("CPFIsRegistered", "CPF entered already registered"),
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
            else {
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
                int accountID = _enrollmentContext.EnrollingAccount.AccountID;
                Dictionary<string, string> dcResultado = AccountExtensions.ValidarExistenciaPIS(NuevePrimerosDigitos + PrimerDigito.ToString() + SegundoDigitoValidar.ToString(), accountID);

                if (dcResultado.Count>0)
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
                               mensaje =Translation.GetTerm("PISisInvalid", "the value of PIS is incorrect"),
                               Estado = false
                           });
            }
        }

        /*CS.29AB2016.Inicio*/
        [HttpPost]
        public ActionResult EditswValidarRG(string TextoRG)
        {
            TextoRG = TextoRG == null ? "" : TextoRG.Trim();
            int accountID = _enrollmentContext.EnrollingAccount.AccountID;
            Dictionary<string, string> dcResultado = AccountExtensions.ValidarExistenciaRG(TextoRG, accountID);
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
        /*CS.29AB2016.Fin*/

        [HttpPost]
        public ActionResult swValidarRG(string TextoRG)
        {
            TextoRG = TextoRG == null ? "" : TextoRG.Trim();
             Dictionary<string, string> dcResultado = AccountExtensions.ValidarExistenciaRG(TextoRG);
            if ((TextoRG == null ? "" : TextoRG).Length==0)
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
            else {
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
                else {
                    return Json(
                              new
                              {
                                  mensaje = "Ok",
                                  Estado = true
                              });
                }

            }
            else {
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

        [HttpPost]
        public ActionResult swValidarSSN(string SSN)
        { 
            var ssnEncription = Encryption.EncryptTripleDES(SSN);
            var account = GetEnrollingAccount();
            if (account.TaxNumber == ssnEncription)
            {
                  return Json(
                             new
                             {
                                 mensaje = "Ok",
                                 Estado = true
                             });
            }
            int countSSN = AccountExtensions.GetValidSSN(ssnEncription);
            if (countSSN> 0)
            {
                return Json(new { mensaje = "That SSN is already being used by another account.", Estado = false });
            }
            return Json(
                             new
                             {
                                 mensaje = "Ok",
                                 Estado = true
                             });
            
        }

        [HttpPost]
        public ActionResult swValidarINN(string SSN)
        {
            var ssnEncription = Encryption.EncryptTripleDES(SSN);
            var account = GetEnrollingAccount();
            if (account.TaxNumber == ssnEncription)
            {
                return Json(
                           new
                           {
                               mensaje = "Ok",
                               Estado = true
                           });
            }
            int countSSN = AccountExtensions.GetValidSSN(ssnEncription);
            if (countSSN > 0)
            {
                return Json(new { mensaje = _errorEINAlreadyInUseString, Estado = false });
            }
            return Json(
                             new
                             {
                                 mensaje = "Ok",
                                 Estado = true
                             });

        }

        #endregion

        /*CSTI(CS)-05/03/2016-Inicio*/
        #region Payment Methods

        [EnrollmentStepSection]
        public virtual ActionResult PaymentMethods()
        {
            //int countryId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry"]);
            
            //model.Birthday = new Models.Shared.DateModel();
            //model.RGIssueDate = new Models.Shared.DateModel();
            //model.CountryID = countryId;

            var account = GetEnrollingAccount();


            int countryId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry"]);
            

            return SectionsView(account);
        }

        [HttpPost, EnrollmentStepSection]
        public virtual ActionResult PaymentMethods(int id, DisbursementMethodKind preference, bool? useAddressOfRecord
            , bool? isActive, string profileName, string payableTo, string address1, string address2, string address3
            , string city, string county, string state, string zip, int? country, bool? agreementOnFile, List<EFTAccountModel> accounts)
        {
            var account = GetEnrollingAccount();
            var disbursementProfile = new AccountEnrollDisbursementProfile();
            try
            {
                disbursementProfile.id = id;
                disbursementProfile.preference = preference;
                disbursementProfile.useAddressOfRecord = useAddressOfRecord ?? false;
                disbursementProfile.isActive = isActive;
                disbursementProfile.agreementOnFile = agreementOnFile ?? false;
                disbursementProfile.accounts = ((accounts == null) ? new List<IEFTAccount>() : accounts.Select(x => x.Convert()));

                var stateCode = GetStateCode(state);
                int stateId;
                Int32.TryParse(state, out stateId);

                var viewAddress = new Address
                {
                    ProfileName = profileName,
                    Attention = payableTo,
                    AddressTypeID = (int)ConstantsGenerated.AddressType.Disbursement,
                    Address1 = address1,
                    Address2 = address2,
                    Address3 = address3,
                    City = city,
                    County = county,
                    State = stateCode,
                    StateProvinceID = stateId,
                    PostalCode = zip,
                    CountryID = country ?? (int)ConstantsGenerated.Country.UnitedStates
                };

                disbursementProfile.address = viewAddress;
                account.SetAccountEnrollDisbursementProfile(disbursementProfile);

                return SectionCompleted(false);
                //return Json(new { Url = "Products/EnrollmentVariantKits" });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (account != null) ? (int?)account.AccountID : null);
                return JsonError(exception.PublicMessage);
            }
            //_enrollmentContext.EnrollingAccount = account;
        }

        protected virtual ActionResult SectionCompleted(bool MVCAutomation = true)
        {
            var sections = GetSections();

            sections.CurrentItem.Completed = true;

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

            if (sections.HasNextItem)
            {
                if (MVCAutomation)
                {
                    return RedirectToAction(sections.NextItem.Action);
                }
                else
                {
                    return Json(new { TypeRedirect = 1, RouteValueDictionary = new { Action = sections.NextItem.Action, Controller = this.ControllerContext.RouteData.Values["controller"].ToString() } });
                }
            }
            else
            {
                return StepCompleted(MVCAutomation);
            }
        }

        protected virtual PaymentMethodsModel PaymentMethods_CreateModel()
        {
            var paymentMethodsModel = new PaymentMethodsModel();
            var account = GetEnrollingAccount();


            paymentMethodsModel.CreateModel(account);
            paymentMethodsModel.ChangeCountryURL = "/Accounts/BillingShippingProfiles/GetAddressControl";
            paymentMethodsModel.PostalCodeLookupURL = "/Checkout/LookupZip";

            var mainAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main) ?? new Address();
            paymentMethodsModel.MainAddressHtmlParent = mainAddress.ToDisplay(IAddressExtensions.AddressDisplayTypes.Web).ToMvcHtmlString();
            paymentMethodsModel.BasicInfoModel = new BasicInfoModel();
            paymentMethodsModel.BasicInfoModel.LoadValues(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry"]), account, mainAddress, _enrollmentContext.ForcePasswordChange, false);
            return paymentMethodsModel;
        }

        //protected virtual void ReferralInfo_LoadModel(ReferralInfoModel model, Account account, bool active)
        //protected virtual void PaymentMethods_LoadModel(PaymentMethodsModel model, bool active)
        protected virtual void PaymentMethods_LoadModel(PaymentMethodsModel model, Account account, bool active)
        {
            //Account account = GetEnrollingAccount();
            var mainAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main) ?? new Address();
            var shippingAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping) ?? new Address();

            model.LoadValues(_enrollmentContext.IsSameShippingAddress, _enrollmentContext.CountryID, mainAddress, shippingAddress);

            // We only need this if the section is active
            if (active)
            {
                PaymentMethods_LoadResources(model, account);
            }
        }

        protected virtual void PaymentMethods_LoadResources(PaymentMethodsModel model, Account account)
        {
            var mainAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main) ?? new Address();
            model.LoadResources(mainAddress);
        }

        protected virtual void PaymentMethods_UpdateCheck(PaymentMethodsModel model, Account account)
        {

        }

        protected virtual void PaymentMethods_UpdateDirectDeposit(PaymentMethodsModel model, Account account)
        {

        }

        protected virtual void PaymentMethods_UpdateContext(PaymentMethodsModel model)
        {
        }

        public virtual ActionResult Save(
            //bool paymentPreference,
            int id,
            DisbursementMethodKind preference,
            bool? useAddressOfRecord,
            bool? isActive,
            string profileName,
            string payableTo,
            string address1,
            string address2,
            string address3,
            string city,
            string county,
            string street,
            string state,
            string zip,
            int? country,
            bool? agreementOnFile,
            List<EFTAccountModel> accounts,
           IEnumerable<int> bankID)
        {
            try
            {

                //var account = CurrentAccount;
                var account = GetEnrollingAccount();
                if (account == null)
                {
                    //return Redirect("~/Accounts");
                }


                if (preference.ToString() == "EFT")
                {

                    var stateCode = GetStateCode(state);
                    int stateId;
                    Int32.TryParse(state, out stateId);
                    var viewAddress = new Address
                    {

                        ProfileName = profileName,
                        Attention = payableTo,
                        AddressTypeID = (int)ConstantsGenerated.AddressType.Disbursement,
                        Address1 = address1,
                        Address2 = address2,
                        Address3 = address3,
                        City = city,
                        County = county,
                        Street = street,
                        State = stateCode,
                        StateProvinceID = stateId,
                        PostalCode = zip,
                        CountryID = country ?? (int)ConstantsGenerated.Country.UnitedStates
                    };

                    useAddressOfRecord = useAddressOfRecord ?? false;
                    agreementOnFile = agreementOnFile ?? false;

                    var ieftAccounts = Enumerable.Empty<IEFTAccount>();


                    if (accounts != null && accounts.Any())
                    {
                        ieftAccounts = accounts.Select(x => x.Convert());
                    }

                    CommissionsService.SaveDisbursementProfile(
                                         id,
                                         account,
                                         viewAddress,
                                         preference,
                                         (bool)useAddressOfRecord,
                                         ieftAccounts,
                                         (bool)agreementOnFile);

                    List<MinimumSearchData> lis_ieftAccounts = new List<MinimumSearchData>();
                    if (ieftAccounts.Where(x => x.DisbursementProfileId == 0).Count() > 0)
                    {
                        lis_ieftAccounts = AccountExtensions.GetDisbursementProfileIDs(account.AccountID);
                        UpdateDisbursementProfiles(lis_ieftAccounts, bankID);
                    }
                    else
                    {
                        foreach (var item in ieftAccounts)
                        {
                            lis_ieftAccounts.Add(new MinimumSearchData
                            {
                                ID = item.DisbursementProfileId
                            });
                        };

                        UpdateDisbursementProfiles(lis_ieftAccounts, bankID);
                    }
                }
                else
                {

                    var stateCode = GetStateCode(state);
                    int stateId;
                    Int32.TryParse(state, out stateId);
                    var viewAddress = new Address
                    {

                        ProfileName = profileName,
                        Attention = payableTo,
                        AddressTypeID = (int)ConstantsGenerated.AddressType.Disbursement,
                        Address1 = address1,
                        Address2 = address2,
                        Address3 = address3,
                        City = city,
                        County = county,
                        Street = street,
                        State = stateCode,
                        StateProvinceID = stateId,
                        PostalCode = zip,
                        CountryID = country ?? (int)ConstantsGenerated.Country.UnitedStates
                    };

                    useAddressOfRecord = useAddressOfRecord ?? false;
                    agreementOnFile = agreementOnFile ?? false;

                    var ieftAccounts = Enumerable.Empty<IEFTAccount>();
                    if (accounts != null && accounts.Any())
                    {
                        ieftAccounts = accounts.Select(x => x.Convert());
                    }

                    CommissionsService.SaveDisbursementProfile(
                                         id,
                                         account,
                                         viewAddress,
                                         preference,
                                         (bool)useAddressOfRecord,
                                         ieftAccounts,
                                         (bool)agreementOnFile);
                }

                //return Json(new { result = true });
                /*CS:14ABR.Inicio*/
                //_enrollmentContext.EnrollmentComplete = true;
                /*CS:14ABR.Fin*/
                
                return Json(new { result = true });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult Save2(string preference)
        {
            try
            {
                var account = GetEnrollingAccount();
                if (account == null)
                {
                    //return Redirect("~/Accounts");
                }

                Address viewAddress = Account.Addresses.Where(donde => donde.AddressTypeID == (int)ConstantsGenerated.AddressType.Disbursement).FirstOrDefault();
                
                //useAddressOfRecord = useAddressOfRecord ?? false;
                //agreementOnFile = agreementOnFile ?? false;
                
                var ieftAccounts = Enumerable.Empty<IEFTAccount>();
                //if (accounts != null && accounts.Any())
                //{
                    //ieftAccounts = account.Select(x => x.Convert());
                    //}
                    int id = 0;
                    CommissionsService.SaveDisbursementProfile(
                                         id,
                                         account,
                                         viewAddress,
                                         DisbursementMethodKind.Check,
                                         false,
                                         ieftAccounts,
                                         false);

                //return Json(new { result = true });
                /*CS:14ABR.Inicio*/
                //_enrollmentContext.EnrollmentComplete = true;
                /*CS:14ABR.Fin*/

                return Json(new { result = true });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private void UpdateDisbursementProfiles(List<MinimumSearchData> ieftAccounts, IEnumerable<int> bankID)
        {

            if (ieftAccounts.First().ID != null && bankID.First() != null)
            {
                int bankID01 = Convert.ToInt32(bankID.First());
                int DisbursementProfileId01 = Convert.ToInt32(ieftAccounts.First().ID);
                AccountExtensions.UpdateDisbursementProfileBank(DisbursementProfileId01, bankID01);
            }

            if (ieftAccounts.Last().ID != null && bankID.Last() != null && bankID.Last() != bankID.First())
            {
                int DisbursementProfileId02 = Convert.ToInt32(ieftAccounts.Last().ID);
                int bankID02 = Convert.ToInt32(bankID.Last());
                AccountExtensions.UpdateDisbursementProfileBank(DisbursementProfileId02, bankID02);
            }
        }
        #endregion
        /*CSTI(CS)-05/03/2016-Fin*/

    }
}
