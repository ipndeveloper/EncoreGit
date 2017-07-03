using System.Linq;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Web.Mvc.Business.Helpers
{
	public static class AddressHelper
	{
		private static Country getCountryByMarketIDIfOnlyOneCountry(int marketID)
		{
			var countries = Country.GetCountriesByMarketID(marketID);
			if (countries.Count == 1)
			{
				return countries[0];
			}
			return null;
		}

		public static Address GetBestAddress(Account account)
		{
			var address =
					account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main)
					?? account.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping)
					?? account.Addresses.FirstOrDefault();
			return address;
		}

		public static int GetCountryID(Account loggedInAccount, Account siteOwner)
		{
			if (loggedInAccount != null)
			{
				var loggedInCountry = getCountryByMarketIDIfOnlyOneCountry(loggedInAccount.MarketID);
				if (loggedInCountry != null)
				{
					return loggedInCountry.CountryID;
				}

				var mainAddress = GetBestAddress(loggedInAccount);
				if (mainAddress != null)
				{
					return mainAddress.CountryID;
				}
			}

			if (siteOwner != null)
			{
				var ownerCountry = getCountryByMarketIDIfOnlyOneCountry(siteOwner.MarketID);
				if (ownerCountry != null)
				{
					return ownerCountry.CountryID;
				}

				var ownerMainAddress = GetBestAddress(siteOwner);
				if (ownerMainAddress != null)
				{
					return ownerMainAddress.CountryID;
				}
			}

			return ApplicationContext.Instance.CurrentCountryID;
		}
	}
}
