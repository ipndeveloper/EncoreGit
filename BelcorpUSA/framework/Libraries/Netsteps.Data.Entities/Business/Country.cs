using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Data.Entities
{
	public partial class Country
	{
		public CultureInfo Culture
		{
			get
			{
				return new CultureInfo(this.CultureInfo);
			}
		}

		public static Country GetCountryByCountryCode(string countryCode)
		{
			return string.IsNullOrEmpty(countryCode)
				? null
				: SmallCollectionCache.Instance.Countries.FirstOrDefault(c => c.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase));
		}

		public static List<Country> GetCountriesByMarketID(int marketID)
		{
			return SmallCollectionCache.Instance.Countries.Where(c => c.MarketID == marketID).ToList();
		}

		#region Basic Crud
		public static List<Country> GetCountries()
		{
			var list = Repository.GetCountries();
			foreach (var item in list)
			{
				item.StartTracking();
				item.IsLazyLoadingEnabled = true;
			}
			return list;
		}
		#endregion
	}
}
