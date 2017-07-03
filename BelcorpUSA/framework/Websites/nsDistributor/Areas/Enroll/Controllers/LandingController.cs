using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Web.Mvc.Controls.Infrastructure;
using NetSteps.Web.Mvc.Controls.Models.Enrollment;
using nsDistributor.Areas.Enroll.Models.Landing;
using nsDistributor.Areas.Enroll.Models.Shared;
using NetSteps.Enrollment.Common.Configuration;
using NetSteps.Encore.Core.IoC;

namespace nsDistributor.Areas.Enroll.Controllers
{
	public class LandingController : EnrollBaseController
	{
		#region Actions
		/// <summary>
		/// This method is used for both GET and POST.
		/// </summary>
		/// <param name="accountTypeId">The requested enrollment Account Type (from either query string or form POST).</param>
		/// <param name="resume">Determines whether to resume enrollment on an imported account.</param>
		/// <param name="upgrade">Determines whether to upgrade the logged-in account.</param>
		/// <param name="countryID">The enrollee's country.</param>
		/// <param name="languageID">The enrollee's language.</param>
		/// <param name="enrollmentContext">Retrieves the <see cref="EnrollmentContext"/> from session.</param>
		/// <returns>The Index view if more info is needed, or redirects to the first enrollment step.</returns>
		public virtual ActionResult Index(
			short? accountTypeId,
			bool? resume,
			bool? upgrade,
			int? countryID,
			int? languageID,
			IEnrollmentContext enrollmentContext)
		{
			if (enrollmentContext == null)
			{
				throw new ArgumentNullException("enrollmentContext");
			}

			Session.Remove("ReturnUrl");

			// Get enabled enrollment types from config
			var enrollableAccountTypeIDs = EnrollmentConfigurationProvider.EnrollableAccountTypeIDs;

			// Scrub inputs
			if (accountTypeId != null
				&& !enrollableAccountTypeIDs.Contains(accountTypeId.Value))
			{
				accountTypeId = null;
			}
			if (countryID != null
				&& !SmallCollectionCache.Instance.Countries.Any(x => x.CountryID == countryID.Value && x.Active))
			{
				countryID = null;
			}
			if (languageID != null
				&& !SmallCollectionCache.Instance.Languages.Any(x => x.LanguageID == languageID.Value && x.Active))
			{
				languageID = null;
			}

			// Start with a nice, clean context
			enrollmentContext.Clear();

			// Get logged-in account (if any)
			var currentAccount = CoreContext.CurrentAccount;

			// Check if this is a valid "resume" enrollment
			bool resumeEnrollment =
				resume == true
				&& currentAccount != null
				&& enrollableAccountTypeIDs.Contains(currentAccount.AccountTypeID)
				&& IsAccountStatusResumable(currentAccount);

			// Check if this is a valid "upgrade" enrollment
			bool upgradeEnrollment =
				upgrade == true
				&& (resume == null || !resume.Value)
				&& accountTypeId != null
				&& enrollableAccountTypeIDs.Contains(accountTypeId.Value)
				&& currentAccount != null
				&& _accountTypeUpgrades.ContainsKey(accountTypeId.Value)
				&& _accountTypeUpgrades[accountTypeId.Value].Contains(currentAccount.AccountTypeID);

			// Check if this is a valid "new" enrollment
			bool newEnrollment =
				(resume == null || !resume.Value)
				&& (upgrade == null || !upgrade.Value)
				&& accountTypeId != null
				&& countryID != null
				&& languageID != null;

			var siteTypeID = ApplicationContext.Instance.SiteTypeID;
			var siteID = CurrentSite.SiteID;

			// Resume enrollment using the logged-in account
			if (resumeEnrollment)
			{
				// If we know the language & country we can start enrollment
				var mainAddress = currentAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Main);
				if (countryID != null
					|| mainAddress != null)
				{
					var enrollmentConfig = EnrollmentConfigurationProvider.GetEnrollmentConfig(currentAccount.AccountTypeID, siteTypeID);

					// Context was already reset above, so just set the enrollment config
					enrollmentContext.Initialize(enrollmentConfig, countryID ?? getCountryID(currentAccount), languageID ?? currentAccount.DefaultLanguageID, siteID);

					// Populate context from account
					LoadEnrollmentContext(
						enrollmentContext,
						currentAccount.AccountID
					);

					return StartEnrollment(enrollmentContext);
				}
			}

			if (upgradeEnrollment)
			{
				var mainAddress = getBestAddress(currentAccount);

				// Context was already reset above, so just set the enrollment config
				var enrollmentConfig = EnrollmentConfigurationProvider.GetEnrollmentConfig(accountTypeId.Value, siteTypeID);
				enrollmentContext.Initialize(enrollmentConfig,
											 countryID ?? getCountryID(currentAccount),
											 currentAccount.DefaultLanguageID,
											 siteID);

				// Populate context from account
				LoadEnrollmentContext(
					enrollmentContext,
					currentAccount.AccountID
				);

				enrollmentContext.IsUpgrade = true;

				return StartEnrollment(enrollmentContext);
			}

			if (newEnrollment)
			{
				// Context was already reset above, so just set the enrollment config
				var enrollmentConfig = EnrollmentConfigurationProvider.GetEnrollmentConfig(accountTypeId.Value, siteID);

				enrollmentContext.Initialize(enrollmentConfig, countryID ?? getCountryID(currentAccount), languageID.Value, siteID);

				return StartEnrollment(enrollmentContext);
			}

			if (accountTypeId != null)
			{
				enrollableAccountTypeIDs = enrollableAccountTypeIDs.Where(x => x == accountTypeId.Value).ToArray();
			}

			if (currentAccount != null)
			{
				enrollableAccountTypeIDs = enrollableAccountTypeIDs.Where(x => x != currentAccount.AccountTypeID).ToArray();
			}

			var model = new IndexModel();

			Index_LoadResources(
				model,
				enrollmentContext,
				enrollableAccountTypeIDs,
				resumeEnrollment,
				countryID ?? getCountryID(currentAccount),
				languageID ?? ApplicationContext.Instance.CurrentLanguageID,
				currentAccount
			);

			return View(model);
		}
		#endregion

		#region Helpers
		/// <summary>
		/// Checks if an account has a status that allows them to resume enrollment.
		/// </summary>
		protected virtual bool IsAccountStatusResumable(Account account)
		{
			if (account == null)
			{
				throw new ArgumentNullException("account");
			}

			return account.AccountStatusID == (short)Constants.AccountStatus.BegunEnrollment
				|| account.AccountStatusID == (short)Constants.AccountStatus.Imported;
		}

		/// <summary>
		/// Indicates if an account with a status of "Imported" should be forced to change their password.
		/// Base defaults to True.
		/// </summary>
		protected virtual bool ForcePasswordChangeOnImportedAccounts { get { return true; } }

		/// <summary>
		/// Indicates if an account with a status of "Imported" should be allowed to change their sponsor.
		/// Base defaults to False.
		/// </summary>
		protected virtual bool AllowSponsorChangeOnImportedAccounts { get { return false; } }

		protected virtual void LoadEnrollmentContext(
			IEnrollmentContext enrollmentContext,
			int accountID)
		{
			var account = Account.LoadFull(accountID);

			enrollmentContext.SponsorID = account.SponsorID;

			// We don't do custom placement yet
			enrollmentContext.PlacementID = account.SponsorID;

			var site = Site.LoadByAccountID(account.AccountID).FirstOrDefault();
			if (site != null && site.SiteUrls.Any())
			{
				// Currently, we are only placing the subdomain in context. This will probably change.
				enrollmentContext.SiteSubscriptionUrl = site.SiteUrls
					.OrderByDescending(x => x.IsPrimaryUrl)
					.FirstOrDefault()
					.GetSubDomain();
			}

			enrollmentContext.EnrollingAccount = account;

			if (account.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping) != null)
			{
				enrollmentContext.IsSameShippingAddress = false;
			}

			if (account.Addresses.GetDefaultByTypeID(Constants.AddressType.Billing) != null)
			{
				enrollmentContext.BillingAddressSourceTypeID = (int)Constants.AddressType.Billing;
			}

			if (enrollmentContext.EnrollmentConfig.Autoship.AutoshipScheduleID != null)
			{
				// Try loading existing autoship
				enrollmentContext.AutoshipOrder = AutoshipOrder.LoadFullByAccountIDAndAutoshipScheduleID(
					account.AccountID,
					enrollmentContext.EnrollmentConfig.Autoship.AutoshipScheduleID.Value
				);
			}

			if (enrollmentContext.EnrollmentConfig.Subscription.AutoshipScheduleID != null)
			{
				// Try loading existing subscription
				enrollmentContext.SiteSubscriptionAutoshipOrder = AutoshipOrder.LoadFullByAccountIDAndAutoshipScheduleID(
					account.AccountID,
					enrollmentContext.EnrollmentConfig.Subscription.AutoshipScheduleID.Value
				);
			}

			// Apply imported account rules
			if (account.AccountStatusID == (short)Constants.AccountStatus.Imported)
			{
				enrollmentContext.ForcePasswordChange = ForcePasswordChangeOnImportedAccounts;

				// Kind of funky logic here
				if (!enrollmentContext.EnrollmentConfig.Sponsor.DenySponsorChange
					&& !AllowSponsorChangeOnImportedAccounts)
				{
					enrollmentContext.EnrollmentConfig.Sponsor.DenySponsorChange = true;
				}
			}
		}

		protected virtual void Index_LoadResources(
			IndexModel model,
			IEnrollmentContext enrollmentContext,
			IEnumerable<short> accountTypeIDs,
			bool resumeEnrollment,
			int countryID,
			int languageID,
			Account currentAccount = null)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model");
			}
			if (enrollmentContext == null)
			{
				throw new ArgumentNullException("enrollmentContext");
			}
			if (accountTypeIDs == null)
			{
				throw new ArgumentNullException("accountTypeIDs");
			}

			LoadPageHtmlContent();

			string fullName = currentAccount != null
				? currentAccount.FullName
				: string.Empty;

			var enrollmentTypes = accountTypeIDs.Select(x => new EnrollmentTypeModel()
				.LoadResources(
					x,
					resumeEnrollment,
					GetFormTitle(x, resumeEnrollment, fullName),
					resumeEnrollment ? false : GetShowLoginLink(currentAccount, x),
					GetLoginText(x),
					resumeEnrollment ? false : GetShowUpgradeLink(currentAccount, x),
					GetUpgradeText(x),
					GetUpgradeUrl(x),
					countryID,
					SmallCollectionCache.Instance.Countries.Where(c => c.Active),
					languageID,
					SmallCollectionCache.Instance.Languages.Where(l => l.Active)
				)
			).ToArray();

			model.LoadResources(enrollmentTypes);
		}

		protected virtual ActionResult StartEnrollment(
			IEnrollmentContext enrollmentContext,
			int countryID,
			int languageID)
		{
			enrollmentContext.CountryID = countryID;
			enrollmentContext.LanguageID = languageID;

			// Default sponsor, if null
			enrollmentContext.SponsorID = enrollmentContext.SponsorID ?? GetDefaultSponsorID();
			enrollmentContext.PlacementID = enrollmentContext.PlacementID ?? GetDefaultSponsorID();

			ApplyRules(enrollmentContext);

			return RedirectToStep(enrollmentContext.EnrollmentConfig.Steps.CurrentItem);
		}

		protected virtual ActionResult StartEnrollment(
			IEnrollmentContext enrollmentContext)
		{
			// Default sponsor, if null
			enrollmentContext.SponsorID = enrollmentContext.SponsorID ?? GetDefaultSponsorID();
			enrollmentContext.PlacementID = enrollmentContext.PlacementID ?? GetDefaultSponsorID();

			ApplyRules(enrollmentContext);

			return RedirectToStep(enrollmentContext.EnrollmentConfig.Steps.CurrentItem);
		}

		protected virtual string GetFormTitle(short accountTypeID, bool resumeEnrollment, string fullName = "")
		{
			switch (accountTypeID)
			{
				case (short)Constants.AccountType.Distributor:
					return resumeEnrollment
						? string.Format(_resumeDistributorString, fullName)
						: _enrollDistributorString;
				case (short)Constants.AccountType.PreferredCustomer:
					return resumeEnrollment
						? string.Format(_resumePreferredCustomerString, fullName)
						: _enrollPreferredCustomerString;
				default:
					return resumeEnrollment
						? string.Format(_resumeOtherString, fullName)
						: _enrollOtherString;
			}
		}

		protected virtual bool GetShowLoginLink(Account account, short accountTypeID)
		{
			if (account != null)
			{
				return false;
			}

			return _accountTypeUpgrades.ContainsKey(accountTypeID);
		}

		protected virtual string GetLoginText(short accountTypeID)
		{
			switch (accountTypeID)
			{
				case (short)Constants.AccountType.Distributor:
					return _loginToUpgradeDistributorString;
				case (short)Constants.AccountType.PreferredCustomer:
					return _loginToUpgradePreferredCustomerString;
				default:
					return _loginToUpgradeOtherString;
			}
		}

		protected virtual bool GetShowUpgradeLink(Account account, short accountTypeID)
		{
			if (account == null)
			{
				return false;
			}

			return _accountTypeUpgrades.ContainsKey(accountTypeID)
				&& _accountTypeUpgrades[accountTypeID].Contains(account.AccountTypeID);
		}

		protected virtual string GetUpgradeText(short accountTypeID)
		{
			switch (accountTypeID)
			{
				case (short)Constants.AccountType.Distributor:
					return _upgradeDistributorString;
				case (short)Constants.AccountType.PreferredCustomer:
					return _upgradePreferredCustomerString;
				default:
					return _upgradeOtherString;
			}
		}

		protected virtual string GetUpgradeUrl(short accountTypeID)
		{
			return Url.Action("Index", new { upgrade = true, accountTypeID = accountTypeID });
		}

		protected virtual void ApplyRules(IEnrollmentContext enrollmentContext)
		{
			if (enrollmentContext.AccountTypeID == (short)Constants.AccountType.PreferredCustomer)
			{
				ApplyPreferredCustomerRules(enrollmentContext);
			}
		}

		/// <summary>
		/// Virtual stub for custom rules
		/// </summary>
		/// <param name="enrollmentContext"></param>
		protected virtual void ApplyPreferredCustomerRules(IEnrollmentContext enrollmentContext)
		{
			// Nothing to do here
		}

		protected Dictionary<short, IEnumerable<short>> _accountTypeUpgradesField;
		/// <summary>
		/// Defines which account types an account can upgrade to/from.
		/// The key is the target account type. The values are the source account types.
		/// </summary>
		protected virtual Dictionary<short, IEnumerable<short>> _accountTypeUpgrades
		{
			get
			{
				if (_accountTypeUpgradesField == null)
				{
					_accountTypeUpgradesField = new Dictionary<short, IEnumerable<short>>
					{
						// RC and PC can upgrade to Distributor.
						{(short)Constants.AccountType.Distributor, new[]
							{
								(short)Constants.AccountType.RetailCustomer,
								(short)Constants.AccountType.PreferredCustomer
							}
						},
						// RC can upgrade to PC.
						{(short)Constants.AccountType.PreferredCustomer, new[]
							{
								(short)Constants.AccountType.RetailCustomer
							}
						}
					};
				}
				return _accountTypeUpgradesField;
			}
		}

		private int getCountryID(Account loggedInAccount)
		{
			if (loggedInAccount != null)
			{
				var loggedInCountry = getCountryByMarketIDIfOnlyOneCountry(loggedInAccount.MarketID);
				if (loggedInCountry != null)
				{
					return loggedInCountry.CountryID;
				}

				var mainAddress = getBestAddress(loggedInAccount);
				if (mainAddress != null)
				{
					return mainAddress.CountryID;
				}
			}

			var siteOwner = SiteOwner;
			if (siteOwner != null)
			{
				var ownerCountry = getCountryByMarketIDIfOnlyOneCountry(siteOwner.MarketID);
				if (ownerCountry != null)
				{
					return ownerCountry.CountryID;
				}

				var ownerMainAddress = getBestAddress(siteOwner);
				if (ownerMainAddress != null)
				{
					return ownerMainAddress.CountryID;
				}
			}

			return ApplicationContext.Instance.CurrentCountryID;
		}

		private Address getBestAddress(Account account)
		{
			var address =
					account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main)
					?? account.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping)
					?? account.Addresses.FirstOrDefault();
			return address;
		}

		private Country getCountryByMarketIDIfOnlyOneCountry(int marketID)
		{
			var countries = Country.GetCountriesByMarketID(marketID);
			if (countries.Count == 1)
			{
				return countries[0];
			}
			return null;
		}
		#endregion

		#region Strings
		protected virtual string _enrollDistributorString { get { return Translation.GetTerm("Enrollment_Landing_FormTitle_EnrollDistributor", "Enroll as a Distributor"); } }
		protected virtual string _resumeDistributorString { get { return Translation.GetTerm("Enrollment_Landing_FormTitle_CompleteDistributor", "Complete your Distributor enrollment: {0}"); } }
		protected virtual string _loginToUpgradeDistributorString { get { return Translation.GetTerm("Enrollment_Landing_LoginToUpgrade_Distributor", "Already have an account? Login here to upgrade"); } }
		protected virtual string _upgradeDistributorString { get { return Translation.GetTerm("Enrollment_Landing_Upgrade_Distributor", "Click here to upgrade your account to a Distributor"); } }

		protected virtual string _enrollPreferredCustomerString { get { return Translation.GetTerm("Enrollment_Landing_FormTitle_EnrollPreferredCustomer", "Enroll as a Preferred Customer"); } }
		protected virtual string _resumePreferredCustomerString { get { return Translation.GetTerm("Enrollment_Landing_FormTitle_CompletePreferredCustomer", "Complete your Preferred Customer enrollment: {0}"); } }
		protected virtual string _loginToUpgradePreferredCustomerString { get { return Translation.GetTerm("Enrollment_Landing_LoginToUpgrade_PreferredCustomer", "Already have an account? Login here to upgrade"); } }
		protected virtual string _upgradePreferredCustomerString { get { return Translation.GetTerm("Enrollment_Landing_Upgrade_PreferredCustomer", "Click here to upgrade your account to a Preferred Customer"); } }

		protected virtual string _enrollOtherString { get { return Translation.GetTerm("Enrollment_Landing_FormTitle_EnrollOther", "Begin your enrollment"); } }
		protected virtual string _resumeOtherString { get { return Translation.GetTerm("Enrollment_Landing_FormTitle_CompleteOther", "Complete your enrollment: {0}"); } }
		protected virtual string _loginToUpgradeOtherString { get { return Translation.GetTerm("Enrollment_Landing_LoginToUpgrade_Other", "Already have an account? Login here to upgrade"); } }
		protected virtual string _upgradeOtherString { get { return Translation.GetTerm("Enrollment_Landing_Upgrade_Other", "Click here to upgrade your account"); } }

		protected virtual string _errorEnrollmentSessionTimedOut { get { return Translation.GetTerm("ErrorEnrollmentSessionTimedOut", "We're sorry, but your session timed out. Please try again."); } }
		#endregion
	}
}
