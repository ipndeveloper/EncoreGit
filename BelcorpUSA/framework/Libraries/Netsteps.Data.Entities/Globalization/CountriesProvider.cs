using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Globalization;
using NetSteps.Common.Interfaces;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Globalization
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Default implementation of ICountriesProvider to lookup countries.
	/// Created: 03-24-2010
	/// </summary>
	[Serializable]
	[ContainerRegister(typeof(ICountriesProvider), RegistrationBehaviors.Default)]
	public class CountriesProvider : ICountriesProvider, IDefaultImplementation
	{
		public List<CountryData> GetCountries()
		{
			List<CountryData> matchingResults = new List<CountryData>();

			var allCountries = Country.GetCountries();
			var activeCountries = allCountries.Where(c => c.Active).ToList();

			foreach (var result in activeCountries)
			{
				matchingResults.Add(new CountryData
				{
					Name = result.Name,
					CountryIso2 = result.CountryCode,
					CountryID = result.CountryID,
					CultureCode = result.CultureInfo,
					IsAvailableForRegistration = result.IsAvailableForRegistration
				});
			}

			return matchingResults;
		}
	}
}
