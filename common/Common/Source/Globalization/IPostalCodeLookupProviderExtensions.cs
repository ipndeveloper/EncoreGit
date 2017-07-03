using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetSteps.Common.Globalization
{
	public static class IPostalCodeLookupProviderExtensions
	{
		/// <summary>
		/// Validates and then cleans the passed postal code
		/// </summary>
		/// <param name="countryId">CountryId</param>
		/// <param name="postalCode">PostalCode to validate and clean</param>
		/// <param name="cleanedPostalCode">Cleaned PostalCode, or the given PostalCode if validation fails.</param>
		/// <returns>True if valid for the given country</returns>
		public static bool ValidateAndCleanPostalCode(this IPostalCodeLookupProvider postalCodeLookupProvider, int countryId, string postalCode, out string cleanedPostalCode)
		{
			Contract.Requires<ArgumentException>(countryId > 0, "countryId not valid");
			Contract.Requires<ArgumentNullException>(postalCode != null);

			var postalCode_upper = postalCode.ToUpper();
			if (!postalCodeLookupProvider.IsValidPostalCode(countryId, postalCode_upper))
			{
				cleanedPostalCode = postalCode;
				return false;
			}
			cleanedPostalCode = postalCodeLookupProvider.CleanPostalCode(countryId, postalCode_upper);
			return true;
		}
	}
}
