using System;
using System.Linq;
using NetSteps.Accounts.Common.Models;
using NetSteps.Common.Dynamic;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Models.Config;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Locale.Common.Models;
using System.Collections.Generic;

namespace NetSteps.Web.Mvc.Controls.Models.Enrollment
{
	[Serializable]
	[ContainerRegister(typeof(IEnrollmentContext), RegistrationBehaviors.Default)]
	[ContainerRegister(typeof(IEnrollmentContext<EnrollmentKitConfig>), RegistrationBehaviors.Default)]
	public class EnrollmentContext : IEnrollmentContext<EnrollmentKitConfig>
	{
		#region Properties
		// Private
		[NonSerialized]
		private IAccount _sponsor;
		private IAccount _enroller;
		private IAccount _placement;
		[NonSerialized]
		private ICountry _country;

		// Public
		// Note: Before adding anything here, ask yourself if you should just use .Data instead.
		public bool IsEntity { get; set; }
		public int CountryID { get; set; }
		public int LanguageID { get; set; }
		public int SiteID { get; set; }
		public int? SponsorID { get; set; }
		public int? EnrollerID { get; set; }
		public int? PlacementID { get; set; }
		public int? ReferralSponsorID { get; set; }
		public int? SiteSubscriptionAutoshipScheduleID { get; set; }
		public string SiteSubscriptionUrl { get; set; }
		public IAutoshipOrder SiteSubscriptionAutoshipOrder { get; set; }
		public bool GenerateUsername { get; set; }
		public bool ForcePasswordChange { get; set; }
		public IAccount EnrollingAccount { get; set; }
		public bool IsSameShippingAddress { get; set; }
		public int BillingAddressSourceTypeID { get; set; }
		public IOrder InitialOrder { get; set; }
		public IAutoshipOrder AutoshipOrder { get; set; }
		public bool EnrollmentComplete { get; set; }
		public bool IsUpgrade { get; set; }
		public bool AddAutoshipToInitialOrder { get; set; }
		public bool AddSubscriptionToInitialOrder { get; set; }
		public bool SkipAutoshipOrder { get; set; }

		public List<EnrollmentKitConfig> EnrollmentKitConfigs { get; set; }
		public Dictionary<int, int> CustomOrderItems { get; set; }
		public string ReturnUrl { get; set; }

		public int AccountTypeID { get; private set; }
		public IEnrollmentConfig EnrollmentConfig { get; private set; }

		public int StepCounter
		{
			get
			{
				if (EnrollmentConfig == null || EnrollmentConfig.Steps == null || !EnrollmentConfig.Steps.Any())
				{
					return 1;
				}

				return EnrollmentConfig.Steps.CurrentItemIndex + 1;
			}
		}

		public ICountry Country
		{
			get
			{
				if (_country == null || _country.CountryID != CountryID)
				{
					_country = SmallCollectionCache.Instance.Countries.GetById(CountryID);
				}
				return _country;
			}
		}

		public int CurrencyID
		{
			get
			{
				return Country == null
					? SmallCollectionCache.Instance.Countries.GetById(ApplicationContext.Instance.CurrentCountryID).CurrencyID
					: Country.CurrencyID;
			}
		}

		public IAccount Sponsor
		{
			get
			{
				if (SponsorID == null)
				{
					return null;
				}
				if (_sponsor == null || _sponsor.AccountID != SponsorID)
				{
					_sponsor = Account.Load(SponsorID.Value);
				}
				return _sponsor;
			}
		}

		public IAccount Enroller
		{
			get
			{
				if (EnrollerID == null)
				{
					return null;
				}
				if (_enroller == null || _enroller.AccountID != EnrollerID)
				{
					_enroller = Account.Load(EnrollerID.Value);
				}
				return _enroller;
			}
		}

		public IAccount Placement
		{
			get
			{
				if (PlacementID == null)
				{
					return null;
				}
				if (_placement == null || _placement.AccountID != PlacementID)
				{
					_placement = Account.Load(PlacementID.Value);
				}
				return _placement;
			}
		}

		public int MarketID
		{
			get
			{
				if (Country != null)
				{
					return Country.MarketID;
				}
				else
				{
					return 0;
				}
			}
		}

		public IMarket Market
		{
			get
			{
				return SmallCollectionCache.Instance.Markets.GetById(MarketID);
			}
		}

		public int StoreFrontID
		{
			get
			{
				if (MarketID != 0)
				{
					var marketStoreFront = SmallCollectionCache.Instance.MarketStoreFronts.FirstOrDefault(x =>
						x.SiteTypeID == ApplicationContext.Instance.SiteTypeID
						&& x.MarketID == MarketID);
					if (marketStoreFront != null)
					{
						return marketStoreFront.StoreFrontID;
					}
				}
				return ApplicationContext.Instance.StoreFrontID;
			}
		}
		#endregion

		#region Infrastructure
		public EnrollmentContext()
		{
			Clear();
		}

		/// <summary>
		/// Resets the <see cref="EnrollmentContext"/> object to default values.
		/// </summary>
		public void Clear()
		{
			AccountTypeID = 0;
			EnrollmentConfig = null;
			_sponsor = null;
			_enroller = null;
			_placement = null;
			_country = null;

			IsEntity = false;
			CountryID = 0;
			LanguageID = 0;
			SponsorID = null;
			EnrollerID = null;
			PlacementID = null;
			SiteSubscriptionAutoshipScheduleID = null;
			SiteSubscriptionUrl = null;
			SiteSubscriptionAutoshipOrder = null;
			GenerateUsername = false;
			ForcePasswordChange = false;
			EnrollingAccount = null;
			IsSameShippingAddress = true;
			BillingAddressSourceTypeID = (int)Constants.AddressType.Main;
			InitialOrder = null;
			AutoshipOrder = null;
			EnrollmentComplete = false;
			IsUpgrade = false;

			EnrollmentKitConfigs = null;
			CustomOrderItems = null;
			ReturnUrl = null;

			AddAutoshipToInitialOrder = false;
			AddSubscriptionToInitialOrder = false;
			SkipAutoshipOrder = false;
		}

		public void Initialize(IEnrollmentConfig enrollmentConfig, int countryID, int languageID, int siteID)
		{
			Clear();
			LanguageID = languageID;
			CountryID = countryID;
			SiteID = siteID;
			SetEnrollmentConfig(enrollmentConfig);
		}

		/// <summary>
		/// Sets the <see cref="EnrollmentConfig"/> values.
		/// </summary>
		public void SetEnrollmentConfig(IEnrollmentConfig enrollmentConfig)
		{
			EnrollmentConfig = enrollmentConfig;
			AccountTypeID = enrollmentConfig.AccountTypeID;
		}

		#endregion
	}
}
