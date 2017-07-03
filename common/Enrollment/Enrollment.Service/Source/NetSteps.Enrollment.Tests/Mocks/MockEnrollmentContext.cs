using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Accounts.Common.Models;
using NetSteps.Data.Common.Entities;
using NetSteps.Enrollment.Common.Models.Config;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Locale.Common.Models;

namespace NetSteps.Enrollment.Service.Tests.Mocks
{
	public class MockEnrollmentContext : IEnrollmentContext
	{
		public void Clear()
		{
			throw new NotImplementedException();
		}

		public void Initialize(IEnrollmentConfig enrollmentConfig, int countryID, int languageID, int siteID)
		{
			throw new NotImplementedException();
		}

		public bool IsEntity
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public int CountryID
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public int LanguageID
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public int SiteID
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public int? SponsorID { get; set; }

		public int? EnrollerID { get; set; }

		public int? PlacementID { get; set; }

		public int? ReferralSponsorID
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public int? SiteSubscriptionAutoshipScheduleID
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public string SiteSubscriptionUrl
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public IAutoshipOrder SiteSubscriptionAutoshipOrder
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public bool GenerateUsername
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public bool ForcePasswordChange
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public IAccount EnrollingAccount { get; set; }

		public bool IsSameShippingAddress
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public int BillingAddressSourceTypeID
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public IOrder InitialOrder { get; set; }

		public IAutoshipOrder AutoshipOrder
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public bool EnrollmentComplete
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public bool IsUpgrade
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public bool AddAutoshipToInitialOrder
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public bool AddSubscriptionToInitialOrder
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public bool SkipAutoshipOrder
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public int AccountTypeID { get; set; }

		public IEnrollmentConfig EnrollmentConfig
		{
			get { throw new NotImplementedException(); }
		}

		public int StepCounter
		{
			get { throw new NotImplementedException(); }
		}

		public ICountry Country
		{
			get { throw new NotImplementedException(); }
		}

		public int CurrencyID
		{
			get { throw new NotImplementedException(); }
		}

		public IAccount Sponsor
		{
			get { throw new NotImplementedException(); }
		}

		public IAccount Enroller
		{
			get { throw new NotImplementedException(); }
		}

		public IAccount Placement
		{
			get { throw new NotImplementedException(); }
		}

		public int MarketID
		{
			get { throw new NotImplementedException(); }
		}

		public IMarket Market
		{
			get { throw new NotImplementedException(); }
		}

		public int StoreFrontID
		{
			get { throw new NotImplementedException(); }
		}


		public Dictionary<int, int> CustomOrderItems
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public string ReturnUrl
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
	}
}
