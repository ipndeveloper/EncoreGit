using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using DistributorBackOffice.Areas.Account.Models.Edit;

using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Business;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Repositories;
using System.Text.RegularExpressions;

namespace DistributorBackOffice.Areas.Account.Controllers
{
	public class EditController : BaseAccountsController
	{
		[HttpGet]
		[FunctionFilter("Accounts", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult Index()
		{
			Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
			var viewModel = new AccountModel();
			viewModel.Account = CurrentAccount;
            GetCreditRequirementsByAccount(viewModel);
			return View(viewModel);
		}

        private void GetCreditRequirementsByAccount(AccountModel viewModel)
        {            
            AccountPropertiesBusinessLogic busines = new AccountPropertiesBusinessLogic();
            var result = busines.GetCreditRequirementsByAccount(CurrentAccount.AccountID);            
            if (result.IssueDate != null)
                viewModel.IssueDate = Convert.ToDateTime(result.IssueDate).ToString("dd/MM/yyyy");

            viewModel.OrgExp = result.ExpeditionEntity ?? string.Empty;

            viewModel.accountSuplieds  = new List<AccountSuppliedIDsParameters>();
            viewModel.accountSuplieds.Add(new AccountSuppliedIDsParameters
            {
                AccountSuppliedID = result.AccountSuppliedID_RG,
                AccountSuppliedIDValue = result.RG
            });
            viewModel.accountSuplieds.Add(new AccountSuppliedIDsParameters
            {
                AccountSuppliedID = result.AccountSuppliedID_PIS,
                AccountSuppliedIDValue = result.PIS
            });
            viewModel.accountSuplieds.Add(new AccountSuppliedIDsParameters
            {
                AccountSuppliedID = result.AccountSuppliedID_CPF,
                AccountSuppliedIDValue = result.CPF,
               
            });
            viewModel.accountProperties = new List<AccountPropertiesParameters>();
            viewModel.accountProperties.Add(new AccountPropertiesParameters
            {
                AccountPropertyID = result.AccountPropertyID_Nationality,
                AccountPropertyTypeID = result.AccountPropertyTypeID_Nationality,
                AccountPropertyValueID = result.Nationality
            });
            viewModel.accountProperties.Add(new AccountPropertiesParameters
            {
                AccountPropertyID = result.AccountPropertyID_MaritalStatus,
                AccountPropertyTypeID = result.AccountPropertyTypeID_MaritalStatus,
                AccountPropertyValueID = result.MaritalStatus
            });
            viewModel.accountProperties.Add(new AccountPropertiesParameters
            {
                AccountPropertyID = result.AccountPropertyID_Occupation,
                AccountPropertyTypeID = result.AccountPropertyTypeID_Occupation,
                AccountPropertyValueID = result.Occupation
            });

        }

        private AccountModel LoadNewCampos(AccountModel viewModel)
        {
            //viewModel.PIS = 0;
            //viewModel.CPF = 0;
            //viewModel.RG = 0;            
            viewModel.OrgExp = "";
            //viewModel.IssueDate ="";
            // viewModel.Occupation =0;
            // viewModel.Nationality =0;
            // viewModel.MaritalStatus =0;
             viewModel.AutorizationNet =true;
             viewModel.AutorizationEmails =true;
             viewModel.AutorizationLocalization =true;
             viewModel.SpeakWith ="";
            return viewModel;
        }

		[FunctionFilter("Accounts-Security Settings", "~/Account", Constants.SiteType.BackOffice)]
		public virtual ActionResult SecuritySettings()
		{
			return View();
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts-Create and Edit Account", "~/Account", Constants.SiteType.BackOffice)]
		public virtual ActionResult Save(            
            int accountId, int accountType, int defaultLanguageId, string sponsorAccountNumber, bool applicationOnFile, string username,
			string password, string confirmPassword, bool userChangingPassword, bool generatedPassword, string profileName, string attention, string address1,
			string address2, string address3, string zip, string city, string county, string state, int countryId, string phone, string firstName,
			string lastName, /*string homePhone,*/ string email, bool isTaxExempt, string ssn, DateTime? dob, short? gender, List<AccountPhone> phones)
		{
			try
			{
				NetSteps.Data.Entities.Account account = accountId > 0 ? NetSteps.Data.Entities.Account.LoadFull(accountId) : new NetSteps.Data.Entities.Account();
				account.StartEntityTracking();
				if (!string.IsNullOrEmpty(sponsorAccountNumber) && (account.SponsorInfo == null || account.SponsorInfo.AccountNumber != sponsorAccountNumber))
				{
					var sponsor = NetSteps.Data.Entities.Account.LoadSlimByAccountNumber(sponsorAccountNumber);
					account.SponsorID = sponsor.AccountID;
					account.MarketID = sponsor.MarketID;
					account.Save();
				}
				account.ReceivedApplication = applicationOnFile;
				account.DefaultLanguageID = defaultLanguageId;

				username = username.ToCleanString();
				if (account.User == null && !string.IsNullOrEmpty(username))
				{
					account.User = new NetSteps.Data.Entities.User();
					account.User.StartEntityTracking();
					account.User.UserStatusID = NetSteps.Data.Entities.Constants.UserStatus.Active.ToShort();
					account.User.UserTypeID = NetSteps.Data.Entities.Constants.UserType.Distributor.ToShort();
					account.User.DefaultLanguageID = account.DefaultLanguageID;

					account.User.Roles.Add(SmallCollectionCache.Instance.Roles.GetById(NetSteps.Data.Entities.Constants.Role.LimitedUser.ToInt()));
				}

				// Make sure the Username entered is not taken by someone else - JHE
				if (!string.IsNullOrEmpty(username) && !account.User.IsUsernameAvailable(username.ToCleanString()))
					return Json(new { result = false, message = Translation.GetTerm("UserNameisNotAvailablePleaseEnteraDifferentUsername", "User name is not available. Please enter a different Username.") });

				if (account.User != null)
				{
					account.User.Username = username.ToCleanString();
					if (account.User.UserStatusID == 0)
						account.User.UserStatusID = NetSteps.Data.Entities.Constants.UserStatus.Active.ToShort();
					if (account.User.UserTypeID == 0)
						account.User.UserTypeID = NetSteps.Data.Entities.Constants.UserType.Distributor.ToShort();
					account.User.DefaultLanguageID = account.DefaultLanguageID;
				}

				if (account.User != null && userChangingPassword)
				{
					var result = NetSteps.Data.Entities.User.NewPasswordIsValid(password, confirmPassword);
					if (result.Success)
						account.User.Password = password.ToCleanString();
					else
						return Json(new { result = false, message = result.Message });
				}

				account.FirstName = firstName.ToCleanString();
				account.LastName = lastName.ToCleanString();
				account.EmailAddress = email.ToCleanString();
				account.IsTaxExempt = isTaxExempt;
				if (!ssn.Contains("*"))
					account.DecryptedTaxNumber = ssn.ToCleanString();
				account.Birthday = dob;
				account.GenderID = gender;
				//account.MainPhone = homePhone.ToCleanString();

				if (phones != null && phones.Count > 0)
				{
					account.AccountPhones.SyncTo(phones, new NetSteps.Common.Comparer.LambdaComparer<AccountPhone>((ap1, ap2) => ap1.AccountPhoneID == ap2.AccountPhoneID), (ap1, ap2) =>
					{
						ap1.PhoneTypeID = ap2.PhoneTypeID;
						ap1.PhoneNumber = ap2.PhoneNumber;
					},
					(list, ap) =>
					{
						list.RemoveAndMarkAsDeleted(ap);
					});

					account.AccountPhones.RemoveWhere(p => p.AccountPhoneID == 0 && p.PhoneNumber.IsNullOrEmpty()); // Don't save empty phones - JHE
				}
				else
					account.AccountPhones.RemoveAllAndMarkAsDeleted();

				//account.Save();

				if (generatedPassword)
					NetSteps.Data.Entities.Account.SendGeneratedPasswordEmail(account);

				Address address = account.Addresses.GetDefaultByTypeID(NetSteps.Data.Entities.Generated.ConstantsGenerated.AddressType.Main);
				if (address == null)
				{
					address = new Address();
					address.StartEntityTracking();
				}
				address.AttachAddressChangedCheck();
				address.ProfileName = profileName.ToCleanString();
				address.Attention = attention.ToCleanString();
				address.Address1 = address1.ToCleanString();
				address.Address2 = address2.ToCleanString();
				address.Address3 = address3.ToCleanString();
				address.City = city.ToCleanString();
				address.StateProvinceID = (state.IsValidInt() && state.ToInt() > 0) ? state.ToInt() : (int?)null;
				address.State = (state.IsValidInt() && state.ToInt() > 0) ? SmallCollectionCache.Instance.StateProvinces.GetById(state.ToInt()).StateAbbreviation : state.ToCleanString();
				address.County = county.ToCleanString();
				address.PostalCode = zip.ToCleanString();
				address.CountryID = countryId;
				address.PhoneNumber = phone.ToCleanString();
				address.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Main.ToShort();
				address.IsDefault = true;
				address.LookUpAndSetGeoCode();

				// Don't save address if it is empty - JHE
				if (!address.IsEmpty(true))
					account.Addresses.Add(address);

				if (account.MarketID < 1 && CoreContext.CurrentMarketId > 0)
					account.MarketID = CoreContext.CurrentMarketId;

				account.Save();

               
            
				// We should not allow this, but I don't have time to fix it right now. - Lundy
				CoreContext.SetCurrentAccountId(account.AccountID);
				CoreContext.CurrentLanguageID = CurrentAccount.DefaultLanguageID;
				return Json(new { result = true  });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}


        private void Save_ReferencesPropertiesSuppliedIDs(List<AccountPropertiesParameters> AccountProperties,                                                         
                                                          List<AccountSuppliedIDsParameters> AccountSuppliedIDs)
        {

            try
            {               
                int accountID = CurrentAccount.AccountID;             

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
            catch (Exception ex)
            {
                //var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                //return Json(new { result = false, message = exception.PublicMessage });
            }

        }
		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts-Security Settings", "~/Account", Constants.SiteType.BackOffice)]
		public virtual ActionResult ChangePassword(string password, string confirmPassword)
		{
			try
			{
				NetSteps.Data.Entities.Account account = CurrentAccount;
				account.StartEntityTracking();

				if (account.User != null)
				{
					var result = NetSteps.Data.Entities.User.NewPasswordIsValid(password, confirmPassword);
					if (result.Success)
						account.User.Password = password.ToCleanString();
					else
						return Json(new { result = false, message = result.Message });
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

		[HttpPost]
		[FunctionFilter("Accounts-Create and Edit Account", "~/Account", Constants.SiteType.BackOffice)]
		public virtual ActionResult SaveAccountInfo(
            List<AccountPropertiesParameters> accountProperties, List<AccountSuppliedIDsParameters> accountSuplieds,
            string firstName, string lastName, string email, DateTime? dob, short? gender, int defaultLanguageId, List<AccountPhone> phones,
			string profileName, string attention, string address1, string address2, string address3,
            string zip, string city, string county, string state, int countryId, string phone, Dictionary<int, bool> newsletterValues)
		{
			try
			{
				var account = CurrentAccount;

				// If the email address hasn't changed, don't let duplicate addresses prevent saving the account.
				var emailHasChanged = string.Compare(email, account.EmailAddress) != 0;

				if (emailHasChanged && NetSteps.Data.Entities.Account.NonProspectExists(email, account.AccountID))
				{
					return Json(new { result = false, message = Translation.GetTerm("EmailAccountAlreadyExists", "An account with this e-mail already exists.") });
				}

				account.StartEntityTracking();

                account.FirstName = firstName;
                account.LastName = lastName;
				account.DefaultLanguageID = defaultLanguageId;
				account.EmailAddress = email.ToCleanString();
				account.Birthday = dob;
				account.GenderID = gender;

				if (phones != null && phones.Count > 0)
				{
					account.AccountPhones.SyncTo(phones.Where(p => !p.PhoneNumber.IsNullOrEmpty()), new NetSteps.Common.Comparer.LambdaComparer<AccountPhone>((ap1, ap2) => ap1.AccountPhoneID == ap2.AccountPhoneID, ap => (ap.AccountPhoneID > 0) ? ap.AccountPhoneID : ap.PhoneNumber.GetHashCode()), (ap1, ap2) =>
					{
						ap1.PhoneTypeID = ap2.PhoneTypeID;
						ap1.PhoneNumber = ap2.PhoneNumber;
					},
					(list, ap) =>
					{
						list.RemoveAndMarkAsDeleted(ap);
					});
				}
				else
					account.AccountPhones.RemoveAllAndMarkAsDeleted();

				Address address = account.Addresses.GetDefaultByTypeID(NetSteps.Data.Entities.Generated.ConstantsGenerated.AddressType.Main);
				if (address == null)
				{
					address = new Address();
					address.StartEntityTracking();
				}
				address.AttachAddressChangedCheck();
				address.ProfileName = profileName.ToCleanString();
				address.Attention = attention.ToCleanString();
				address.Address1 = address1.ToCleanString();
				address.Address2 = address2.ToCleanString();
				address.Address3 = address3.ToCleanString();
				address.City = city.ToCleanString();
				address.StateProvinceID = (state.IsValidInt() && state.ToInt() > 0) ? state.ToInt() : (int?)null;
				address.State = (state.IsValidInt() && state.ToInt() > 0) ? SmallCollectionCache.Instance.StateProvinces.GetById(state.ToInt()).StateAbbreviation : state.ToCleanString();
				address.County = county.ToCleanString();
				address.PostalCode = zip.ToCleanString();
				address.CountryID = countryId;
				address.PhoneNumber = phone.ToCleanString();
				address.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Main.ToShort();
				address.IsDefault = true;
				address.LookUpAndSetGeoCode();

				// Don't save address if it is empty - JHE
				if (!address.IsEmpty(true))
					account.Addresses.Add(address);

                NewsLetterSubscription(newsletterValues, account);
				account.Save();
				CoreContext.ReloadCurrentAccount();
				CoreContext.CurrentLanguageID = CurrentAccount.DefaultLanguageID;
				var mainAddress = account.Addresses.GetDefaultByTypeID(ConstantsGenerated.AddressType.Main);

                Save_ReferencesPropertiesSuppliedIDs(accountProperties, accountSuplieds);

                if (account.AccountTypeID == (short)Constants.AccountType.Distributor)
                    AccountExtensions.UpdateAccountsCommission(account.AccountID);

				TempData["ReturnSuccessMessage"] = "Saved Successfully";

				return Json(new { result = true, mainAddresHtml = mainAddress == null ? Translation.GetTerm("N/A", "N/A") : mainAddress.ToDisplay() 
                                    ,
                                  accountProperties = accountProperties.Select(apt => new
                                  {
                                      accountPropertyID = apt.AccountPropertyID
                                  }),
                                  accountSuppliedIDs = accountSuplieds.Select(apt => new
                                  {
                                      accountSuppliedID = apt.AccountSuppliedID
                                  })
                });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

        //@01 20150724 BR-COM-002 G&S LIB: Se crea los métodos para obtener y registrar newsletters

        private List<Campaign> _newsletters;
        protected virtual List<Campaign> GetActiveNewsletters()
        {
            return _newsletters ?? (_newsletters = Create.New<ICampaignRepository>().Where(c => c.Active && c.CampaignTypeID == (int)Constants.CampaignType.Newsletters).ToList());
        }


        public virtual void NewsLetterSubscription(Dictionary<int, bool> newValues, NetSteps.Data.Entities.Account account)
        {
            if (newValues == null)
                return;

            foreach (var campaign in GetActiveNewsletters())
            {
                var campaignSubscribed = account.CampaignSubscribers.FirstOrDefault(x => x.CampaignID == campaign.CampaignID);
                if (campaignSubscribed != null)
                {
                    bool newsletterExists = account.CampaignSubscribers.Any(x => x.CampaignID == campaign.CampaignID);

                    if (newValues.ContainsKey(campaign.CampaignID) && !newValues[campaign.CampaignID])
                    {
                        // User unchecked a previously checked value. Delete from CampaingSubscribers table
                        if (newsletterExists)
                        {
                            CampaignSubscriber.Delete(campaignSubscribed.CampaignSubscriberID);
                        }
                    }
                }
                else
                {
                    if (newValues.ContainsKey(campaign.CampaignID) && newValues[campaign.CampaignID])
                    {
                        var newSubscription = new CampaignSubscriber
                        {
                            CampaignID = campaign.CampaignID,
                            AccountID = account.AccountID,
                            AddedByAccountID = CurrentAccount.AccountID,
                            DateAddedUTC = DateTime.UtcNow
                        };
                        account.CampaignSubscribers.Add(newSubscription);
                    }
                }
            }
        }
	
        #region [Validations]

        public ActionResult DocumentValidation(int DocumentType, string DocumentValue)
        {
            bool result = false;
            string message = string.Empty;

            try
            {
                switch (DocumentType)
                {
                    case 8: //CPF
                        switch (swValidarCPF(DocumentValue))
                        {
                            case 1: message = Translation.GetTerm("CPFCOIsRegistered", "Provided CPF already in use."); break;
                            case 3: message = Translation.GetTerm("CPFCOisInvalid", "Incorrect CPF value."); break;
                            default: result = true; break;
                        }
                        break;
                    case 9: //PIS
                        switch (swValidarPIS(DocumentValue))
                        {
                            case 1: message = Translation.GetTerm("PisIsRegistered", "Provided PIS already in use."); break;
                            case 3: message = Translation.GetTerm("PISisInvalid", "Incorrect PIS value."); break;
                            default: result = true; break;
                        }
                        break;
                    case 4: //RG
                        switch (swValidarRG(DocumentValue))
                        {
                            case 1: message = Translation.GetTerm("RGReq", "RG is required."); break;
                            case 2: message = Translation.GetTerm("RGIsRegistered", "Provided RG already in use."); break;
                            default: result = true; break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                message = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException).PublicMessage;
            }

            return Json(new { result = result, message = message });
        }

        public ActionResult EmailValidation(string email)
        {
            bool result = false;
            string message = string.Empty;

            try
            {
                Regex RxEmail = new Regex(@"^$|^([\w\-\.]+)@((\[([0-9]{1,3}\.){3}[0-9]{1,3}\])|(([\w\-]+\.)+)([a-zA-Z]{2,4}))$");

                if (!RxEmail.IsMatch(email))
                {
                    message = Translation.GetTerm("EmailAccountInvalid", "Provided e-mail is invalid.");
                }
                else
                {
                    result = MailAccount.IsAvailable(email);

                    if (!result)
                        message = Translation.GetTerm("EmailAccountAlreadyExists", "An account with this e-mail already exists.");
                }
            }
            catch (Exception ex)
            {
                message = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException).PublicMessage;
            }

            return Json(new { result = result, message = message });
        }

        #endregion

        #region [Document Validations]

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

            if (string.IsNullOrEmpty(TextoRG))
            {
                returnInt = 1;
            }
            else
            {

                Dictionary<string, string> dcResultado = AccountExtensions.ValidarExistenciaRG(TextoRG);

                if (dcResultado.Count > 1)
                {
                    returnInt = 2;
                }
                else
                {
                    returnInt = 3;
                }
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
