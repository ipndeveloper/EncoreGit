using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Configuration;
using NetSteps.Common.EldResolver;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;


namespace NetSteps.Data.Entities.TokenValueProviders
{
	using NetSteps.Encore.Core.IoC;

[ContainerRegister(typeof(PartyTokenValueProvider), RegistrationBehaviors.IfNoneOther, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class PartyTokenValueProvider : ITokenValueProvider
	{
		#region Tokens

		protected const string HOST_NAME = "HostName";
		protected const string HOST_EMAIL_ADDRESS = "HostEmailAddress";
		protected const string HOST_SSO = "HostSSO";
		protected const string PARTY_ID = "PartyID";
		protected const string PARTY_NAME = "PartyName";
		protected const string PARTY_DATE = "PartyDate";
		protected const string PARTY_TIME = "PartyTime";
		protected const string PARTY_ADDRESS = "PartyAddress";
		protected const string DISTRIBUTOR_NAME = "DistributorName";
		protected const string DISTRIBUTOR_PWSURL = "DistributorPWSUrl";

		#endregion

		protected Party _party;
		protected Order _order;
		protected OrderCustomer _hostess;

		#region Lazy loading properties

		protected Order Order
		{
			get
			{
				if (_order == null)
				{
					_order = _party.Order ?? Order.Load(_party.OrderID);
				}
				return _order;
			}
		}

		protected OrderCustomer Hostess
		{
			get
			{
				if (_hostess == null)
				{
					if (Order == null)
					{
						return null;
					}
					_hostess = Order.GetHostess();
				}
				return _hostess;
			}
		}

		#endregion

		public PartyTokenValueProvider(Party party)
		{
			_party = party;
		}

		public virtual IEnumerable<string> GetKnownTokens()
		{
			return new List<string>()
				{
					HOST_NAME,
					HOST_EMAIL_ADDRESS,
					HOST_SSO,
					PARTY_ID,
					PARTY_NAME,
					PARTY_DATE,
					PARTY_TIME,
					PARTY_ADDRESS,
					DISTRIBUTOR_NAME,
					DISTRIBUTOR_PWSURL
				};
		}

		public virtual string GetTokenValue(string token)
		{
			if (_party == null) return null;
			OrderCustomer hostess = null;
			switch (token)
			{
				case HOST_NAME:
					if (_party.Order != null && _party.Order.OrderCustomers != null && _party.Order.GetHostess() != null) hostess = _party.Order.GetHostess();
					return hostess == null ? null : hostess.FullName;
				case HOST_SSO:
					return Party.EncryptPartyID(_party.PartyID);
				case PARTY_ID:
					return _party.PartyID.ToString();
				case PARTY_NAME:
					return _party.Name;
				case PARTY_DATE:
					return
						_party.StartDate.ToString(
							SmallCollectionCache.Instance.Languages.GetById(ApplicationContext.Instance.CurrentLanguageID).Culture);
				case PARTY_ADDRESS:
					return _party.Address.ToDisplay(false, false, false);
				case DISTRIBUTOR_NAME:
					if (_party.Order == null) return null;
					return _party.Order.ConsultantInfo.FullName;
				case DISTRIBUTOR_PWSURL:
					if (_party.Order == null) return null;
					var sites = Site.LoadByAccountID(_party.Order.ConsultantID);
					//If the site information is not there for this account,load the site details for corporate account
					if (sites == null || sites.Count == 0)
					{
						var consultantID =
							ConfigurationManager.GetAppSetting<int>(
								NetSteps.Common.Configuration.ConfigurationManager.VariableKey.CorporateAccountID);
						sites = Site.LoadByAccountID(consultantID);
					}
					Site site = null;
					site = sites.Count > 1
					       	? sites.FirstOrDefault(
					       		s =>
					       		s.SiteStatusID == Constants.SiteStatus.Active.ToShort() && s.PrimaryUrl != null
					       		&& !s.PrimaryUrl.Url.IsNullOrEmpty())
					       	: sites.FirstOrDefault();
					return site == null ? null : site.PrimaryUrl == null ? null : site.PrimaryUrl.Url.EldEncode();
				default:
					return null;
			}
		}

		protected string GetPWSUrlFromPartyOrder()
		{
			var sites = Site.LoadByAccountID(Order.ConsultantID);
			Site site = sites.Count > 1 ? sites.FirstOrDefault(s => s.SiteStatusID == (short)Constants.SiteStatus.Active && s.PrimaryUrl != null && !s.PrimaryUrl.Url.IsNullOrEmpty()) : sites.FirstOrDefault();
			return site == null ? null : site.PrimaryUrl == null ? null : site.PrimaryUrl.Url.EldEncode();
		}

		protected string GetHostAddress()
		{
			return _party.Address.ToDisplay(false);
		}

		protected virtual string GetHostEmailAddress()
		{
			return Hostess != null && Hostess.AccountInfo != null ? Hostess.AccountInfo.EmailAddress : null;
		}

		protected string GetHostName()
		{
			return Hostess == null ? null : Hostess.FullName;
		}

		protected virtual string GetDate()
		{
			return _party.StartDate.ToString(SmallCollectionCache.Instance.Languages.GetById(ApplicationContext.Instance.CurrentLanguageID).Culture);
		}

		protected virtual string GetTime()
		{
			return
				_party.StartDate.ToString("hh:mm tt");
		}
	}

	[ContainerRegister(typeof(FakePartyTokenValueProvider), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class FakePartyTokenValueProvider : PartyTokenValueProvider
	{
		public FakePartyTokenValueProvider() : base(null) { }

		public override string GetTokenValue(string token)
		{
			switch (token)
			{
				case HOST_NAME:
					return "John Host";
				case HOST_EMAIL_ADDRESS:
					return "johnhost@inter.net";
				case HOST_SSO:
					return null;
				case PARTY_ID:
					return "0";
				case PARTY_NAME:
					return "Party";
				case PARTY_DATE:
					return DateTime.Now.ToString(SmallCollectionCache.Instance.Languages.GetById(ApplicationContext.Instance.CurrentLanguageID).Culture);
				case PARTY_TIME:
					return "12:00 pm";
				case PARTY_ADDRESS:
					return "123 Anywhere St<br />Anywhereville, AA 00000";
				case DISTRIBUTOR_NAME:
					return "John Distributor";
				case DISTRIBUTOR_PWSURL:
					return null;
				default: return null;
			}
		}
	}
}
