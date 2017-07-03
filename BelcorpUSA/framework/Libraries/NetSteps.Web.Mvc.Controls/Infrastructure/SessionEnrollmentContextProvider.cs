using System.Web;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Enrollment.Common.Provider;

#region Referencias
using NetSteps.Data.Entities;
using NetSteps.Enrollment.Common.Configuration;
using System;
using NetSteps.Web.Mvc.Controls.Models.Enrollment;
using NetSteps.Data.Entities.Extensions;
using System.Linq;
#endregion

namespace NetSteps.Web.Mvc.Controls.Infrastructure
{
	[ContainerRegister(typeof(IEnrollmentContextProvider), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class SessionEnrollmentContextProvider : IEnrollmentContextProvider
	{
		public IEnrollmentContext GetEnrollmentContext()
		{
            var session = HttpContext.Current.Session;

            _enrollmentContext = session[_enrollmentContextSessionKey] as IEnrollmentContext;
            if (_enrollmentContext == null)
            {
                _enrollmentContext = Create.New<IEnrollmentContext>();
                session[_enrollmentContextSessionKey] = _enrollmentContext;
            }
            if (_enrollmentContext.AccountTypeID != 0)
                return _enrollmentContext;
            else
                Incializar();
            return _enrollmentContext;
		}

        #region Cambio

        private IEnrollmentContext _enrollmentContext;
        private readonly Lazy<IEnrollmentConfigurationProvider> _enrollmentConfigurationProviderFactory = new Lazy<IEnrollmentConfigurationProvider>(Create.New<IEnrollmentConfigurationProvider>);

        protected virtual IEnrollmentConfigurationProvider EnrollmentConfigurationProvider
        {
            get { return _enrollmentConfigurationProviderFactory.Value; }
        }

        private void Incializar()
        {
            
            var currentAccount = HttpContext.Current.Session["CurrentAccount"] as NetSteps.Data.Entities.Account;
            if (currentAccount == null)
            {
                var session = HttpContext.Current.Session;
                _enrollmentContext = session[_enrollmentContextSessionKey] as IEnrollmentContext;
                if (_enrollmentContext == null)
                {
                    _enrollmentContext = Create.New<IEnrollmentContext>();
                    session[_enrollmentContextSessionKey] = _enrollmentContext;
                }
                return;
            }
            var siteTypeID = ApplicationContext.Instance.SiteTypeID;
            var enrollmentConfig = EnrollmentConfigurationProvider.GetEnrollmentConfig(currentAccount.AccountTypeID, siteTypeID);

            Site site = new Site();
            var siteID = site.SiteID;//currentAccount.SiteID;

            if (_enrollmentContext == null)
            {
                var session = HttpContext.Current.Session;
                _enrollmentContext = Create.New<IEnrollmentContext>();
                session[_enrollmentContextSessionKey] = _enrollmentContext;
            }

            if (_enrollmentContext != null)
            {
                _enrollmentContext.Initialize(enrollmentConfig, getCountryID(currentAccount), currentAccount.DefaultLanguageID, siteID);
                LoadEnrollmentContext(_enrollmentContext, currentAccount.AccountID);

                _enrollmentContext.IsUpgrade = true;
            }
        }

        protected virtual void LoadEnrollmentContext(IEnrollmentContext enrollmentContext,int accountID)
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

        protected virtual bool ForcePasswordChangeOnImportedAccounts { get { return true; } }
        protected virtual bool AllowSponsorChangeOnImportedAccounts { get { return false; } }

        private Address getBestAddress(Account account)
        {
            var address =
                    account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main)
                    ?? account.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping)
                    ?? account.Addresses.FirstOrDefault();
            return address;
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

            var siteOwner = loggedInAccount;
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


		/// <summary>
		/// The key name of the <see cref="EnrollmentContext"/> object stored in session.
		/// </summary>
		private const string _enrollmentContextSessionKey = "_enrollmentContext";

        //public IEnrollmentContext enrollmentContext { get; set; }
    }
}
