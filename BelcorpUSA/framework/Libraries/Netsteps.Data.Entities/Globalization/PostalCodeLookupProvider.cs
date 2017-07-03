using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Interfaces;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Tax;
using NetSteps.Encore.Core.Dto;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Globalization
{
	[DTO]
	public interface IPostalCodeLookupKey
	{
		int CountryID { get; set; }
		string PostalCode { get; set; }
	}

	[Serializable]
	[ContainerRegister(typeof(IPostalCodeLookupProvider), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class PostalCodeLookupProvider : IPostalCodeLookupProvider, IDefaultImplementation
	{
		ICache<IPostalCodeLookupKey, List<PostalCodeData>> _cache;

		class PostalCodeLookupResolver : DemuxCacheItemResolver<IPostalCodeLookupKey, List<PostalCodeData>>
		{
			PostalCodeLookupProvider _owner;
			public PostalCodeLookupResolver(PostalCodeLookupProvider owner)
			{
				_owner = owner;
			}
			protected override bool DemultiplexedTryResolve(IPostalCodeLookupKey key, out List<PostalCodeData> value)
			{
				value = _owner.PerformLookupPostalCode(key.CountryID, key.PostalCode);
				return value != null;
			}
		}

		public PostalCodeLookupProvider()
		{
			_cache = new ActiveMruLocalMemoryCache<IPostalCodeLookupKey, List<PostalCodeData>>("PostalCode", new PostalCodeLookupResolver(this));
		} 

		public List<PostalCodeData> LookupPostalCode(int countryID, string postalCode)
		{
			var key = Create.New<IPostalCodeLookupKey>();
			key.CountryID = countryID;
			key.PostalCode = CleanPostalCode(countryID, postalCode);

			if (!IsValidPostalCode(countryID, key.PostalCode))
				throw new InvalidOperationException("Invalid Postal Code: " + key.PostalCode);

			List<PostalCodeData> result;
            result = PostalCodeExtesions.GetPostaCode(key.PostalCode, countryID);
			//_cache.TryGet(key, out result); 
			return result;
		}

        public List<PostalCodeData> LookupPostalCodeByAccount(int countryID, string postalCode, int accountID, int? addressID)
        {
            var key = Create.New<IPostalCodeLookupKey>();
            key.CountryID = countryID;
            key.PostalCode = CleanPostalCode(countryID, postalCode);

            if (!IsValidPostalCode(countryID, key.PostalCode))
                throw new InvalidOperationException("Invalid Postal Code: " + key.PostalCode);

            List<PostalCodeData> result;
            result = PostalCodeExtesions.GetPostaCodeByAccountID(key.PostalCode, countryID, accountID, Convert.ToInt32(addressID));
            //_cache.TryGet(key, out result); 
            return result;
        }

		private List<PostalCodeData> PerformLookupPostalCode(int countryId, string postalCode)
		{
			var returnResults = LookupPostalCodeFromDatabase(countryId, postalCode);
			if (returnResults.Count == 0)
			{
				bool hasResults = LookupPostalCodeFromTaxProviderWebServer(countryId, postalCode);
				if (hasResults)
					returnResults = LookupPostalCodeFromDatabase(countryId, postalCode);
			}

			return returnResults;
		}
		private List<PostalCodeData> LookupPostalCodeFromDatabase(int countryId, string postalCode)
		{
			var lookup = new PostalCodeLookup();
			var data = lookup.GetPostalData(countryId, postalCode);

			//If they mistyped the plus four, or put it in by mistake, we'll get the normal postal code results - DES
			if (countryId == 1 && postalCode.Length > 5 && !data.Any())
			{
				data.AddRange(lookup.GetPostalData(countryId, postalCode.Substring(0, 5)));
			}

			return data;
		}

		public bool LookupPostalCodeFromTaxProviderWebServer(int countryId, string postalCode)
		{
			if (countryId == Constants.Country.UnitedStates.ToInt())
			{
				// TODO: Lookup address with Simpova if no results were found - JHE
				SimpovaTaxRateProvider simpovaTaxRateProvider = new SimpovaTaxRateProvider();
				try
				{
					var rates = simpovaTaxRateProvider.GetTaxInfo(postalCode);
					return rates.Any();
				}
				catch //if there was a problem with the simpova service, just swallow the exception. 
				{
					return false;
				}
			}
			else
				return false;
		}

		public string CleanPostalCode(int countryId, string postalCode)
		{
			//Take out everything except alphanumeric characters to make it easier to clean up - DES
			postalCode = Regex.Replace(postalCode, @"[^\w]", string.Empty);

			Country country = SmallCollectionCache.Instance.Countries.GetById(countryId);
			if (country != null)
			{
				RegionInfo region = new RegionInfo(country.CountryCode);
				switch (region.ThreeLetterISORegionName.ToUpper())
				{
                    //case "USA":
                    //    postalCode = Regex.Replace(postalCode, @"\D", "");
                    //    if (postalCode.Length > 5)
                    //    {
                    //        if (postalCode.Length < 9)
                    //            postalCode = postalCode.Substring(0, 5);
                    //        else
                    //            postalCode = postalCode.Substring(0, 5) + "-" + postalCode.Substring(5, 4);
                    //    }
                    //    break;
					case "CAN":
						postalCode = postalCode.Substring(0, 3) + " " + postalCode.Substring(3, 3);
						break;
                    case "BRA":
                    case "USA":
                        postalCode = postalCode;
                        break;
				}
			}

			return postalCode;
		}

		public bool IsValidPostalCode(int countryId, string postalCode)
		{
			Country country = SmallCollectionCache.Instance.Countries.GetById(countryId);
			if (country != null)
			{
				RegionInfo region = new RegionInfo(country.CountryCode);
				switch (region.ThreeLetterISORegionName.ToUpper())
				{
					case "USA":
						return Regex.IsMatch(postalCode, @"^(\d{5}|\d{5}-?\d{4})$");
					case "CAN":
						return Regex.IsMatch(postalCode, @"^[A-Z]\d[A-Z]\s?\d[A-Z]\d$");
                    case "BRA":
                        return Regex.IsMatch(postalCode, @"^(\d{8})$");
				}
			}
			return false;
		}

		/// <summary>
		/// Validates and then cleans the passed postal code
		/// </summary>
		/// <param name="countryId">CountryId</param>
		/// <param name="postalCode">PostalCode to validate and clean</param>
		/// <param name="cleanedPostalCode">Cleaned PostalCode, or the given PostalCode if validation fails.</param>
		/// <returns>True if valid for the given country</returns>
		public bool ValidateAndCleanPostalCode(int countryId, ref string postalCode)
		{
			Contract.Requires<ArgumentException>(countryId > 0, "countryId not valid");
			Contract.Requires<ArgumentNullException>(postalCode != null);

			var postalCode_upper = postalCode.ToCleanString().ToUpper();
			if (!IsValidPostalCode(countryId, postalCode_upper))
			{
				return false;
			}
			postalCode = CleanPostalCode(countryId, postalCode_upper);
			return true;
		}


       
    }
}
