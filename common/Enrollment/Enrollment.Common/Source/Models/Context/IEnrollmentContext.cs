using NetSteps.Data.Common.Entities;
using NetSteps.Accounts.Common.Models;
using NetSteps.Enrollment.Common.Models.Config;
using NetSteps.Locale.Common.Models;
using System.Collections.Generic;

namespace NetSteps.Enrollment.Common.Models.Context
{
	/// <summary>
	/// This is the generic enrollment context. Client data can reside in the CustomClientData dynamic variable
	/// </summary>
	public interface IEnrollmentContext
	{
		bool IsEntity { get; set; }
		int CountryID { get; set; }
		int LanguageID { get; set; }
		int SiteID { get; set; }
		int? SponsorID { get; set; }
		int? EnrollerID { get; set; }
		int? PlacementID { get; set; }
		int? ReferralSponsorID { get; set; }
		int? SiteSubscriptionAutoshipScheduleID { get; set; }
		string SiteSubscriptionUrl { get; set; }
		IAutoshipOrder SiteSubscriptionAutoshipOrder { get; set; }
		bool GenerateUsername { get; set; }
		bool ForcePasswordChange { get; set; }
		IAccount EnrollingAccount { get; set; }
		bool IsSameShippingAddress { get; set; }
		int BillingAddressSourceTypeID { get; set; }
		IOrder InitialOrder { get; set; }
		IAutoshipOrder AutoshipOrder { get; set; }
		bool EnrollmentComplete { get; set; }
		bool IsUpgrade { get; set; }
		bool AddAutoshipToInitialOrder { get; set; }
		bool AddSubscriptionToInitialOrder { get; set; }
		bool SkipAutoshipOrder { get; set; }
		Dictionary<int, int> CustomOrderItems { get; set; }
		string ReturnUrl { get; set; }

		int AccountTypeID { get; }
		IEnrollmentConfig EnrollmentConfig { get; }
		int StepCounter { get; }
		ICountry Country { get; }

		int CurrencyID { get; }
		IAccount Sponsor { get; }
		IAccount Enroller { get; }
		IAccount Placement { get; }

		int MarketID { get; }
		IMarket Market { get; }

		int StoreFrontID { get; }

		void Clear();
		void Initialize(IEnrollmentConfig enrollmentConfig, int countryID, int languageID, int siteID);
	}

	public interface IEnrollmentContext<T> : IEnrollmentContext
		where T : class
	{
		List<T> EnrollmentKitConfigs { get; set; }
	}
}
