using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NetSteps.Common.Comparer;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Enrollment.Common.Models.Config;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Web.Mvc.Controls.Models.DisbursementProfiles;
using NetSteps.Commissions.Common.Models;
using System.Linq;
using NetSteps.Encore.Core.IoC;
using NetSteps.Commissions.Common;
using NetSteps.Data.Entities.Business.HelperObjects;
using NetSteps.Enrollment.Common;

namespace NetSteps.Web.Mvc.Controls.Controllers.Enrollment
{
	public class AccountInfoStep : BaseEnrollmentStep
	{
        private readonly Lazy<IEnrollmentService> _enrollmentServiceFactory = new Lazy<IEnrollmentService>(Create.New<IEnrollmentService>);
        protected virtual IEnrollmentService EnrollmentService
        {
            get { return _enrollmentServiceFactory.Value; }
        }

		[OutputCache(CacheProfile = "DontCache")]
		public ActionResult Index()
		{
			_controller.ViewData["StepCounter"] = _enrollmentContext.StepCounter;
			_controller.ViewData["IsEntity"] = _enrollmentContext.IsEntity;
			_controller.ViewData["Sponsor"] = _enrollmentContext.Sponsor;

			AccountPropertiesModel accountPropertiesData = new AccountPropertiesModel();
			InitializeAccountPropertiesData(accountPropertiesData, _enrollmentContext);

			_controller.ViewData["AccountPropertiesData"] = accountPropertiesData;

			Constants.AccountType accountType;
			if (!Enum.TryParse(_enrollmentContext.AccountTypeID.ToString(), out accountType))
			{
				throw new Exception("Account Type doesn't exist");
			}

			return PartialView(accountType.ToString() + "Info");
		}

		private Account GetProspectAccountForUpgradeIfExists(string email, int sponsorID)
		{
			return Account.GetAccountByEmailAndSponsorID(email, sponsorID);
		}

		[OutputCache(CacheProfile = "DontCache")]
		public ActionResult SubmitStep(int? sponsorId, int? placementId, bool? applicationOnFile,
			string firstName, string middleName, string lastName, string entityName, string email,
			int languageId, Constants.Gender? gender, DateTime? dateOfBirth, bool taxExempt,
			string taxNumber, bool generateUsername, string coApplicant, string username, bool generatePassword, string password,
			string nameOnCard, string accountNumber, DateTime? expirationDate,
            List<AccountSuppliedIDsParameters> AccountSuppliedIDs,List<AccountSocialNetworksParameters> AccountSocialNetworks,
            AccountPropertiesParameters AccountReferences,List<AccountPropertiesParameters> AccountProperties, List<EFTAccount> DisbursementInformation,
            Address mainAddress, Address shippingAddress, CoApplicantSearchParameters CoApplicantObj = null, Address billingAddress = null, List<AccountPhone> phones = null, List<AccountProperty> properties = null
            )
		{
			try
			{
				Constants.AccountType accountType;
                if (mainAddress.City == null) mainAddress.City = ""; if (mainAddress.County == null) mainAddress.County = "";
                if (shippingAddress.City == null) shippingAddress.City = ""; if (shippingAddress.County == null) shippingAddress.County = "";
                if (billingAddress.City == null) billingAddress.City = ""; if (billingAddress.County == null) billingAddress.County = "";

				if (!Enum.TryParse(_enrollmentContext.AccountTypeID.ToString(), out accountType))
				{
					throw new Exception("Account Type doesn't exist");
				}

                int defaultSponsor = EnrollmentService.GetCorporateSponsorID();

                if (!sponsorId.HasValue)
                {
                    sponsorId = defaultSponsor;
                }

                if (!placementId.HasValue)
                {
                    placementId = defaultSponsor;
                }

                Tuple<bool, string> tplResultadoValidacion = AccountSponsorBusinessLogic.Instance.ValidateSponsorShipRules(placementId.ToInt(), Periods.GetOpenPeriodID());

                if (!tplResultadoValidacion.Item1)
                {
                    return JsonError(Translation.GetTerm("NotValidatedPlacement", "Selected Placement did not pass the Sponsorship Rules validation."));
                }

                //Validating UserName
                if (!generateUsername && !new User().IsUsernameAvailable(username))
                {
                    return JsonError(Translation.GetTerm("UserNameisNotAvailablePleaseEnteraDifferentUsername", "User name is not available. Please enter a different Username."));
                }

                if (accountType != Constants.AccountType.RetailCustomer) //*Customer no aplica estas validaciones*
                {
                    //Validating CPF, PIS, RG
                    string CPFValue = AccountSuppliedIDs.ToList().Where(x => x.IDTypeID == 8).Select(a => a.AccountSuppliedIDValue).First().ToString();

                    switch (swValidarCPF(CPFValue))
                    {
                        case 1: return JsonError(Translation.GetTerm("CPFIsRegistered", "Provided CPF already in use."));
                        case 3: return JsonError(Translation.GetTerm("CpFisInvalid", "Incorrect CPF value."));
                    }


                    var PISValue = string.Empty;
                    var PISObj = AccountSuppliedIDs.ToList().Where(x => x.IDTypeID == 9).FirstOrDefault();

                    if (PISObj.AccountSuppliedIDValue != null)
                        PISValue = PISObj.AccountSuppliedIDValue.ToString();

                    switch (swValidarPIS(PISValue))
                    {
                        case 1: return JsonError(Translation.GetTerm("PisIsRegistered", "Provided PIS already in use."));
                        case 3: return JsonError(Translation.GetTerm("PISisInvalid", "Incorrect PIS value."));
                    }

                    string RGValue = AccountSuppliedIDs.ToList().Where(x => x.IDTypeID == 4).Select(a => a.AccountSuppliedIDValue).First().ToString();

                    switch (swValidarRG(RGValue))
                    {
                        case 1: return JsonError(Translation.GetTerm("RGReq", "RG is required."));
                        case 2: return JsonError(Translation.GetTerm("RGIsRegistered", "Provided RG already in use."));
                    }

                    if (CoApplicantObj != null)
                    {
                        if (CoApplicantObj.CPF == CPFValue)
                            return JsonError(Translation.GetTerm("CPFCOInUse", "Can't use the same CPF for Applicant and CoApplicant."));
                        else
                        {
                            switch (swValidarCPF(CoApplicantObj.CPF))
                            {
                                case 1: return JsonError(Translation.GetTerm("CPFCOIsRegistered", "Provided CoApplicant CPF already in use."));
                                case 3: return JsonError(Translation.GetTerm("CpFCOisInvalid", "Incorrect CoApplicant CPF value."));
                            }
                        }

                        if (CoApplicantObj.PIS == PISValue && CoApplicantObj.PIS != string.Empty)
                            return JsonError(Translation.GetTerm("PISCOInUse", "Can't use the same PIS for Applicant and CoApplicant."));
                        else
                        {
                            switch (swValidarPIS(CoApplicantObj.PIS))
                            {
                                case 1: return JsonError(Translation.GetTerm("PisCOIsRegistered", "Provided CoApplicant PIS already in use."));
                                case 3: return JsonError(Translation.GetTerm("PISCOisInvalid", "Incorrect CoApplicant PIS value."));
                            }
                        }

                        if (CoApplicantObj.RG == RGValue)
                            return JsonError(Translation.GetTerm("RGCOInUse", "Can't use the same RG for Applicant and CoApplicant."));
                        else
                        {
                            switch (swValidarRG(CoApplicantObj.RG))
                            {
                                case 1: return JsonError(Translation.GetTerm("RGCOReq", "CoApplicant RG is required."));
                                case 2: return JsonError(Translation.GetTerm("RGCOIsRegistered", "Provided CoApplicant RG already in use."));
                            }
                        }
                    }
                }
				// Only allowing account upgrade for prospects only.
				if (Account.NonProspectExists(email))
				{
					return JsonError(Translation.GetTerm("EmailAccountAlreadyExists", "An account with this e-mail already exists."));
				}

				// Only supports Prospect upgrade. 
				var existingAccount = GetProspectAccountForUpgradeIfExists(email, sponsorId.Value);

				if (existingAccount != null && !existingAccount.EnrollmentDate.HasValue)
				{
					existingAccount.EnrollmentDate = DateTime.Now;
				}

				Account newAccount = existingAccount ?? new Account();
				newAccount.StartEntityTracking();
				newAccount.AccountTypeID = (short)accountType;
				newAccount.AccountStatusID = (int)Constants.AccountStatus.BegunEnrollment;

				SetEnrollerAndSponsorID(newAccount, sponsorId.Value, placementId.Value);

				SetAccountInfo(newAccount, firstName, middleName, lastName, entityName, email, languageId, gender, dateOfBirth, taxExempt, coApplicant, applicationOnFile, taxNumber);
				newAccount.IsEntity = _enrollmentContext != null && _enrollmentContext.IsEntity;

				taxNumber = taxNumber.Replace("-", string.Empty).ToCleanString();

				//This step is added for clients that use email address as username - Cade               
				if (!ValidateEmailAddressAvailibility(newAccount.EmailAddress))
				{
					return JsonError(Translation.GetTerm("EmailAccountAlreadyExists", "An account with this e-mail already exists."));
				}
				if (newAccount.EnforceUniqueTaxNumber())
				{
					if (newAccount.IsTaxNumberAvailable(taxNumber))
						newAccount.DecryptedTaxNumber = taxNumber;
					else
					{
						return JsonError(string.Format("The Tax Number ({0}) is already in use by another account.", taxNumber.ToCleanString()));
					}
				}
				else
					newAccount.DecryptedTaxNumber = taxNumber;

				newAccount.ReceivedApplication = applicationOnFile.GetValueOrDefault();
				newAccount.EnrollmentDate = DateTime.Now;

				if (phones != null)
					newAccount.AccountPhones.AddRange(phones);


				if (properties != null)
				{
					UpdateAccountProperties(newAccount, properties, _enrollmentContext);
				}
				else
				{
					newAccount.AccountProperties.RemoveAllAndMarkAsDeleted();
				}

				// Check if mainAddress already exists.
				var existingMainAddress = newAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Main);

				if (existingMainAddress != null)
				{
					var temp = SetAddressValue(mainAddress, "Main", (int)Constants.AddressType.Main);
					_enrollmentContext.CountryID = existingMainAddress.UpdateAddress(temp).CountryID;
				}
				else
				{
					if (mainAddress != null && !string.IsNullOrEmpty(mainAddress.Address1))
					{
						_enrollmentContext.CountryID = AddNewAddressValue(newAccount, mainAddress, "Main", (int)Constants.AddressType.Main).CountryID;
					}
					else if (accountType != NetSteps.Data.Entities.Generated.ConstantsGenerated.AccountType.RetailCustomer)
						throw new NetStepsApplicationException("Main Address must be provided for Preferred Customers and Distributors")
						{
							PublicMessage = Translation.GetTerm("MainAddressMustBeProvidedForPreferredCustomersAndDistributors", "Main Address must be provided for Preferred Customers and Distributors")
						};
				}

				if (_enrollmentContext.Country != null)
					newAccount.MarketID = _enrollmentContext.Country.MarketID;
				// Check if shippingAddress already exists
				var existingShippingAddress = newAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping);

				if (existingShippingAddress != null)
				{
					Address temp;

                    if (shippingAddress != null && !string.IsNullOrEmpty(shippingAddress.Address1))
                        temp = SetAddressValue(shippingAddress, "Default Shipping", (int)Constants.AddressType.Shipping);
                    else
                    {                        
                        temp = SetAddressValue(shippingAddress, "Default Shipping", (int)Constants.AddressType.Shipping);
                    }

					existingShippingAddress.UpdateAddress(temp);
				}
				else
				{
					if (shippingAddress != null && !string.IsNullOrEmpty(shippingAddress.Address1))
					{
						AddNewAddressValue(newAccount, shippingAddress, "Default Shipping", (int)Constants.AddressType.Shipping);
					}
					else if (accountType != Constants.AccountType.RetailCustomer)
					{
						shippingAddress = new Address();                        
                        AddNewAddressValue(newAccount, shippingAddress, Translation.GetTerm("DefaultShipping", "Default Shipping"), (int)Constants.AddressType.Shipping, true, mainAddress);
					}
				}


				// Check if Billing address exists
				var existingBillingAddress = newAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Billing);
				if (existingBillingAddress != null)
				{
					Address temp;
					// If billing address exist use that - else use main address as billing address
                    if (billingAddress != null && !string.IsNullOrEmpty(billingAddress.Address1))
                        temp = SetAddressValue(billingAddress, Translation.GetTerm("DefaultBilling", "Default Billing"), (int)Constants.AddressType.Billing);
                    else
                    {                        
                        temp = SetAddressValue(billingAddress, Translation.GetTerm("DefaultBilling", "Default Billing"), (int)Constants.AddressType.Billing);
                    }

					existingBillingAddress.UpdateAddress(temp);
					billingAddress = existingBillingAddress;
				}
				else
				{
					if (billingAddress != null && !string.IsNullOrEmpty(billingAddress.Address1))
					{
						AddNewAddressValue(newAccount, billingAddress, Translation.GetTerm("DefaultBilling", "Default Billing"), (int)Constants.AddressType.Billing);

					}
					else if (accountType != Constants.AccountType.RetailCustomer)
					{
						billingAddress = new Address();                        
                        AddNewAddressValue(newAccount, billingAddress, Translation.GetTerm("DefaultBilling", "Default Billing"), (int)Constants.AddressType.Billing, true, mainAddress);
					}
				}

				if (billingAddress != null && !billingAddress.Address1.IsNullOrEmpty())
				{
					newAccount.AccountPaymentMethods.RemoveAllAndMarkAsDeleted();

					newAccount.AccountPaymentMethods.Add(new AccountPaymentMethod()
					{
						NameOnCard = nameOnCard,
						DecryptedAccountNumber = accountNumber.RemoveNonNumericCharacters(),
						ExpirationDate = expirationDate != null ? (DateTime?)expirationDate.Value.LastDayOfMonth() : null,
						BillingAddress = billingAddress,
						PaymentTypeID = (int)Constants.PaymentType.CreditCard,
						ProfileName = Translation.GetTerm("DefaultBilling", "Default Billing"),
						IsDefault = true
					});
				}

				if (generatePassword)
					password = User.GeneratePassword();

				if (generateUsername || username.IsNullOrEmpty())
					username = GenerateTempUserNameUntilAccountSaved();

				SetUser(newAccount, username, password, languageId);

				// Now that the user has been set, we need to assign the roles to the user. :D
				Account.AssignRolesByAccountType(newAccount);

				if (_enrollmentContext != null)
				{
					newAccount.MarketID = _enrollmentContext.MarketID;
				}

				newAccount.Save();
                UpdateAddressStreet(newAccount.Addresses);

                if (placementId.HasValue && placementId.Value != sponsorId)
				{
					// We need to add a record into the AccountSponsor table - DES
                    AccountSponsorSearchParameters sponsor = new AccountSponsorSearchParameters()
					{
                        AccountID = newAccount.AccountID,
                        SponsorID = placementId.Value,
						AccountSponsorTypeID = (int)Constants.AccountSponsorType.Sponsor,
						Position = 1,
                        ModifiedByUserID = newAccount.CreatedByUserID.ToInt()
					};

                    AccountSponsorBusinessLogic.Instance.InsertAccountSponsor(sponsor);
				}

                AccountExtensions.UpdateAccountsCommission(newAccount.AccountID);

				CreateMailAccountForDistributors(accountType, newAccount);

				if (generateUsername)
				{
					SetAccountNumberAsUserName(newAccount);
				}

				EnrollingAccount = newAccount;
                if (accountType != Constants.AccountType.RetailCustomer)//*Customer no aplica estas validaciones*
                {
                    InsertAccountSupplieds(AccountSuppliedIDs, newAccount.AccountID, newAccount.CreatedByUserID.ToInt());

                    InsertAccountSocialNetWork(AccountSocialNetworks, newAccount.AccountID);

                    InsertReferencesProperties(AccountProperties, AccountReferences, newAccount.AccountID);
                }
                if (DisbursementInformation == null)
                {
                    DisbursementInformation = new List<EFTAccount>();
                    DisbursementInformation.Add(new EFTAccount()
                    {
                        DisbursementTypeID = DisbursementMethodKind.Check.ToInt()
                    });

                }

                foreach (EFTAccount account in DisbursementInformation){
                    account.AccountID = newAccount.AccountID;
                    DisbursementProfileBusinessLogic.Instance.SaveCheckDisbursementProfile(account);
                }
                

                //Co Applicant
                if (CoApplicantObj != null && accountType != Constants.AccountType.RetailCustomer)//*Customer no aplica estas validaciones*
                {
                    CoApplicantObj.AccountID = newAccount.AccountID;
                    int AccountAdditionalTitularID = new AccountBusinessLogic().InsertAccountAdditionalTitulars(CoApplicantObj);

                    AccountBusinessLogic accountBusinessLogic = new AccountBusinessLogic();

                    AccountAdditionalTitularSuppliedIDsParameters CPF = new AccountAdditionalTitularSuppliedIDsParameters(){
                        AccountAdditionalTitularID = AccountAdditionalTitularID,
                        IDTypeID = 8,
                        AccountSuppliedValue = CoApplicantObj.CPF,
                        IsPrimaryID = true
                    };

                    accountBusinessLogic.InsertAccountAdditionalTitularSuppliedIDs(CPF);

                    AccountAdditionalTitularSuppliedIDsParameters RG = new AccountAdditionalTitularSuppliedIDsParameters()
                    {
                        AccountAdditionalTitularID = AccountAdditionalTitularID,
                        IDTypeID = 4,
                        AccountSuppliedValue = CoApplicantObj.RG,
                        IsPrimaryID = false,
                        IDExpeditionDate = CoApplicantObj.IssueDate,
                        ExpeditionEntity = CoApplicantObj.OrgExp
                    };

                    accountBusinessLogic.InsertAccountAdditionalTitularSuppliedIDs(RG);

                    if (string.IsNullOrEmpty(CoApplicantObj.PIS))
                        CoApplicantObj.PIS = string.Empty;

                    AccountAdditionalTitularSuppliedIDsParameters PIS = new AccountAdditionalTitularSuppliedIDsParameters()
                    {
                        AccountAdditionalTitularID = AccountAdditionalTitularID,
                        IDTypeID = 9,
                        AccountSuppliedValue = CoApplicantObj.PIS,
                        IsPrimaryID = false
                    };

                    accountBusinessLogic.InsertAccountAdditionalTitularSuppliedIDs(PIS);

                    if (CoApplicantObj.Phones != null){
                        foreach (AccountAdditionalPhonesParameters additionalPhone in CoApplicantObj.Phones)
                        {
                            additionalPhone.AccountAdditionalTitularID = AccountAdditionalTitularID;
                            additionalPhone.ModifiedByUserID = newAccount.CreatedByUserID.ToInt();

                            accountBusinessLogic.InsertAccountAdditionalPhones(additionalPhone);
                        }
                    }
                    
                }

				return NextStep();
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
				return JsonError(exception.PublicMessage);
			}
		}

        private void UpdateAddressStreet(TrackableCollection<Address> Addresses)
        {
            AddressBusinessLogic business = new AddressBusinessLogic();

            foreach (Address address in Addresses)
            {
                business.UpdateAddressStreet(address);
            }
        }

        private void InsertAccountSupplieds(List<AccountSuppliedIDsParameters> AccountSuppliedIDs, int accountID, int createdBy)
        {
            AccountSuppliedIDsBusinessLogic busines = new AccountSuppliedIDsBusinessLogic();
            CreditRequirementsBusinessLogic businesCredit = new CreditRequirementsBusinessLogic();

            foreach (AccountSuppliedIDsParameters accountSupplied in AccountSuppliedIDs)
            {                
                accountSupplied.AccountID = accountID;
                accountSupplied.AccountSuppliedIDValue = accountSupplied.AccountSuppliedIDValue == null ? string.Empty : accountSupplied.AccountSuppliedIDValue;
                busines = new AccountSuppliedIDsBusinessLogic();
                busines.InsertAccountSuppliedIDs(accountSupplied);

                /* Account Supplied IDs Types  = Credit Requirement Types         
                    4	= 2  RG      
                    8	= 17 CPF
                    9	= 24 PIS
                */

                CreditRequirementSearchData creditRequirement = new CreditRequirementSearchData();
                creditRequirement.AccountID = accountID;
                switch (accountSupplied.IDTypeID)
                {
                    case 4: creditRequirement.RequirementTypeID = 2; break;
                    case 8: creditRequirement.RequirementTypeID = 17; break;
                    case 9: creditRequirement.RequirementTypeID = 24; break;
                }

                if (string.IsNullOrEmpty(accountSupplied.AccountSuppliedIDValue))
                    creditRequirement.RequirementStatusID = 2;
                else
                    creditRequirement.RequirementStatusID = 1;

                creditRequirement.CreationDate = DateTime.Now;
                creditRequirement.LastModifiedDate = DateTime.Now;
                creditRequirement.UserCreatedID = createdBy;
                creditRequirement.LastUserModifiedID = createdBy;
                creditRequirement.Observations = string.Empty;
                businesCredit = new CreditRequirementsBusinessLogic();
                businesCredit.Insert(creditRequirement);
            }
        }

        private void InsertAccountProperties(List<AccountPropertiesParameters> AccountProperties, int accountID)
        {
            AccountPropertiesBusinessLogic busines = new AccountPropertiesBusinessLogic();
            foreach (AccountPropertiesParameters creditRequirement in AccountProperties)
            {                
                creditRequirement.AccountID = accountID;
                creditRequirement.Active = true;                
                busines = new AccountPropertiesBusinessLogic();
                var res = busines.Insert(creditRequirement);
                creditRequirement.AccountPropertyID = res.ID;                
            }
        }

        private void InsertAccountSocialNetWork(List<AccountSocialNetworksParameters> AccountSocialNetworks, int accountID)
        {
            AccountSocialNetworksBusinessLogic businesSocial = new AccountSocialNetworksBusinessLogic();

            foreach (AccountSocialNetworksParameters accountSocialNetwork in AccountSocialNetworks)
            {
                if (accountSocialNetwork.Value != null && accountSocialNetwork.Value != "")
                {
                    accountSocialNetwork.AccountID = accountID;
                    businesSocial = new AccountSocialNetworksBusinessLogic();
                    businesSocial.Insert(accountSocialNetwork);
                }
            }
        }


        private void InsertReferencesProperties(List<AccountPropertiesParameters> AccountProperties, AccountPropertiesParameters AccountReferences, int accountID)
        {

            if (AccountReferences != null)
            {
                AccountReferences.AccountID = accountID;
                new AccountReferencesBusinessLogic().Insert(AccountReferences);
            }
            
            if (AccountProperties != null)
            {
                AccountPropertiesBusinessLogic busines = new AccountPropertiesBusinessLogic();

                foreach (AccountPropertiesParameters creditRequirement in AccountProperties)
                {
                    if (creditRequirement.PropertyValue != "0" || creditRequirement.AccountPropertyValueID != 0)
                    {
                        creditRequirement.AccountID = accountID;
                        creditRequirement.Active = true;
                        busines = new AccountPropertiesBusinessLogic();
                        busines.Insert(creditRequirement);
                    }
                }
            }
        }

        protected virtual void SetAccountInfo(Account newAccount, string firstName, string middleName, string lastName, string entityName,
			 string email, int languageId, Constants.Gender? gender, DateTime? dateOfBirth, bool taxExempt, string coApplicant, bool?
			 applicationOnFile, string taxNumber)
		{
			newAccount.FirstName = firstName;
			newAccount.MiddleName = middleName;
			newAccount.LastName = lastName;
			newAccount.EntityName = entityName;
			newAccount.EmailAddress = email;
			newAccount.DefaultLanguageID = languageId;
			newAccount.GenderID = !gender.HasValue || gender.ToShort() == 0 ? null : (short?)gender;
			newAccount.Birthday = dateOfBirth;
			newAccount.IsTaxExempt = taxExempt;
			newAccount.CoApplicant = coApplicant;
			newAccount.IsEntity = _enrollmentContext.IsEntity;
			newAccount.ReceivedApplication = applicationOnFile.ToBool();
            newAccount.TaxNumberCustom = taxNumber;
		}

		protected void SetEnrollerAndSponsorID(Account newAccount, int sponsorID, int? placementID)
		{
			// SponsorID is set to placementID and it can be changed by the enrollee
			newAccount.EnrollerID = sponsorID;
			newAccount.SponsorID = placementID ?? sponsorID;
		}

		protected void SetUser(Account newAccount, string username, string password, int languageId)
		{
			if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
			{
				if (newAccount.User != null)
				{
					newAccount.User.Password = password;
					newAccount.User.Username = username;
				}
				else
				{
					newAccount.User = NewUser(username, password, languageId);
				}
			}
		}

		protected void SetAccountNumberAsUserName(Account newAccount)
		{
			newAccount.User.Username = GetUsernameDefault(newAccount);
			newAccount.Save();
		}

		protected void CreateMailAccountForDistributors(Constants.AccountType accountType, Account newAccount)
		{
			if (accountType == ConstantsGenerated.AccountType.Distributor)
			{
				// Check to see if the mail account exists
				var mailAccount = MailAccount.LoadByAccountID(newAccount.AccountID);

				if (mailAccount == null)
				{
					//generate internal email for distributors
					mailAccount = new MailAccount
					{
						AccountID = newAccount.AccountID,
						EmailAddress = GetMailAccountMailAddress(newAccount),
						Active = true
					};
					mailAccount.Save();
				}
			}
		}

		public virtual string GetMailAccountMailAddress(Account account)
		{
			string domain = MailDomain.LoadDefaultForInternal().DomainName.ToLower();
			var emailAddress = string.Format("{0}@{1}", account.AccountNumber, domain);
			return emailAddress;
		}

		protected User NewUser(string username, string password, int languageID)
		{
			return new User()
			{
				Username = username,
				Password = password,
				UserTypeID = (int)Constants.UserType.Distributor,
				UserStatusID = (int)Constants.UserStatus.Active,
				DefaultLanguageID = languageID
			};
		}

		protected virtual string GenerateTempUserNameUntilAccountSaved()
		{
			return Guid.NewGuid().ToString();
		}

		protected virtual string GetUsernameDefault(Account account)
		{
			var username = account.AccountNumber;
			var useEmailAsUsername = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UseEmailAsUsername, false);
			if (useEmailAsUsername)
				username = account.EmailAddress;
			return username;
		}

		protected virtual bool ValidateEmailAddressAvailibility(string emailAddress)
		{
			var useEmailAsUsername = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UseEmailAsUsername, false);
			if (useEmailAsUsername)
			{
				if (Account.NonProspectExists(emailAddress))
					return false;

				if (!User.IsUsernameAvailable(0, emailAddress))
				{
					var existingUser = User.GetByUsername(emailAddress);
					existingUser.StartEntityTracking();
					existingUser.Username += "_%_" + Guid.NewGuid().ToString();
					existingUser.Save();
				}

				return true;
			}
			else
			{
				//Set to true if client uses account numbers
				return true;
			}
		}

		/// <summary>
		/// Initializes the account properties model object.
		/// </summary>
		/// <param name="accountPropertiesData">The account properties model.</param>
		/// <param name="enrollmentContext">The enrollment context.</param>
		protected virtual void InitializeAccountPropertiesData(AccountPropertiesModel accountPropertiesData, IEnrollmentContext enrollmentContext)
		{
			accountPropertiesData.AccountPropertyTypes = AccountPropertyType.LoadAllFull();
			accountPropertiesData.AccountProperties = new List<AccountProperty>();
			accountPropertiesData.IsAvataxEnabled = NetSteps.Data.Entities.AvataxAPI.Util.IsAvataxEnabled();
			accountPropertiesData.AvataxPropertyTypeName = NetSteps.Data.Entities.AvataxAPI.Constants.AVATAX_ACCOUNTPROPERTY_TYPE_NAME;
		}

		/// <summary>
		/// Synchronizes the account properties.
		/// </summary>
		/// <param name="newAccount">The account being enrolled.</param>
		/// <param name="properties">The properties.</param>
		/// <param name="enrollmentContext">The enrollment context.</param>
		protected virtual void UpdateAccountProperties(Account newAccount, List<AccountProperty> properties, IEnrollmentContext enrollmentContext)
		{
			newAccount.AccountProperties.SyncTo(properties, new LambdaComparer<AccountProperty>((p1, p2) => p1.AccountPropertyID == p2.AccountPropertyID, ap => (ap.AccountPropertyID > 0) ? ap.AccountPropertyID : ap.AccountPropertyTypeID.GetHashCode()), (p1, p2) =>
			{
				p1.PropertyValue = p2.PropertyValue.ToCleanString();
				p1.AccountPropertyValueID = p2.AccountPropertyValueID;
			});
		}

		#region Helpers
		[NonAction]
		private Address AddNewAddressValue(Account account, Address address, string profileName, short addressType, bool isBillingOrShipping = false, Address mainAddress = null)
		{
			address.AttachAddressChangedCheck();

			if (isBillingOrShipping && mainAddress != null)
			{
				Address.CopyPropertiesTo(mainAddress, address);                
			}
			else
			{
				address.SetState(address.State, address.CountryID);
			}

            if (isBillingOrShipping && mainAddress != null && !string.IsNullOrEmpty(address.Address1))
            {
                address.Street = mainAddress.Street;
            }

			address.ProfileName = profileName;
			address.AddressTypeID = addressType;
			address.IsDefault = true;
			account.Addresses.Add(address);
			address.LookUpAndSetGeoCode();            
			return address;
		}

		[NonAction]
		private Address SetAddressValue(Address address, string profileName, short addressType)
		{
			address.ProfileName = profileName;
			address.AddressTypeID = addressType;
			address.IsDefault = true;

			address.SetState(address.State, address.CountryID);

			return address;
		}
		#endregion

		public AccountInfoStep(IEnrollmentStepConfig stepConfig, Controller controller, IEnrollmentContext enrollmentContext)
			: base(stepConfig, controller, enrollmentContext) { }

        #region [Validations]

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
                dcResultado = AccountExtensions.ValidarExistenciaCPF(NuevePrimerosDigitos + PrimerDigitoValidar + SegundoDigitoValidar);
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

            if (string.IsNullOrEmpty(TextoInputPIS))
                return 2;

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
        public int swValidarRG(string TextoRG)
        {
            int returnInt = 0;
            TextoRG = TextoRG == null ? "" : TextoRG.Trim();
            Dictionary<string, string> dcResultado = AccountExtensions.ValidarExistenciaRG(TextoRG);
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

        #endregion
    }
}
