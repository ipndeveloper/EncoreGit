namespace AddressValidator.Qas.Helpers
{
	using System.Collections.Generic;
	using AddressValidation.Common;
	using com.qas.proweb;
	using Common;
	using Config;
	using Exceptions;
	using NetSteps.Encore.Core.IoC;

	public interface IQasHelper
	{
		SearchResult QasVerifyAddress(IValidationAddress address);

		IAddressValidationResult ProcessResponse(IAddressValidationResult addressValidationResult, SearchResult searchResult);
	}

	public class QasHelper : IQasHelper
	{
		#region Members

		private readonly IQuickAddress _quickAddress;

		private const int CityIndex = 3;
		private const int StateIndex = 4;
		private const int ZipIndex = 5;

		#endregion

		public string CurrentLayout { get; set; }

		public QasHelper(IQuickAddress quickAddress)
		{
			_quickAddress = quickAddress;
		}

		public QasHelper(QasAddressValidatorConfig config)
		{
			_quickAddress = new QuickAddress(config.EndpointUrl, config.UserName, config.Password);
		}

		public IEnumerable<IValidationAddress> TranslateFromQasAddress(SearchResult searchResult)
		{
			var validAddresses = new List<IValidationAddress>();

			if (searchResult.VerifyLevel == SearchResult.VerificationLevels.Verified
							&& searchResult.Address != null)
			{
				IValidationAddress address = AssignAddressValue(searchResult.Address.AddressLines);

				validAddresses.Add(address);
			}
			else if (searchResult.Picklist != null && searchResult.Picklist.Length > 0)
			{
				foreach (var picklistItem in searchResult.Picklist.Items)
				{
					var formatedAddress = _quickAddress.GetFormattedAddress(picklistItem.Moniker, CurrentLayout);  // LAYOUT_NAME;

					IValidationAddress address = AssignAddressValue(formatedAddress.AddressLines);
					validAddresses.Add(address);
				}
			}

			return validAddresses;
		}

		public string TranslateToQasAddress(IValidationAddress localAddress)
		{
			var qaAddressFormat = string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
																					(localAddress.Address1 ?? string.Empty).Replace('|', ' '),
																					(localAddress.Address2 ?? string.Empty).Replace('|', ' '),
																					(localAddress.Address3 ?? string.Empty).Replace('|', ' '),
																					(localAddress.SubDivision ?? string.Empty).Replace('|', ' '),
																					(localAddress.MainDivision ?? string.Empty).Replace('|', ' '),
																					(localAddress.PostalCode ?? string.Empty).Replace('|', ' '));
			return qaAddressFormat;
		}

		private void SetLayoutPerCountry(string country)
		{
			switch (country.ToUpperInvariant())
			{
				case "USA":
					CurrentLayout = "MSDCRM2011";//"USA2 AptLine2 Caps NoRetention";
					break;
				case "CAN":
					CurrentLayout = "MSDCRM2011";
					break;
				case "AUS":
					CurrentLayout = "MSDCRM2011";
					break;
				default:
					CurrentLayout = "QADefault";
					break;
			}
		}

		/// <summary>
		/// Call service to verify address
		/// </summary>
		/// <param name="address">Address to verify</param>
		/// <returns>The search result returned from QAS On Demand service</returns>
		public SearchResult QasVerifyAddress(IValidationAddress address)
		{
			string qasAddress = TranslateToQasAddress(address);

			string country = this.ConvertAlpha2ToAlpha3(address.Country);

			SetLayoutPerCountry(country);

			var searchResult = Search(country, qasAddress);

			return searchResult;
		}

		private string ConvertAlpha2ToAlpha3(string alpha2)
		{
			switch (alpha2.ToUpperInvariant())
			{
				case "US":
					return "USA";
				case "CA":
					return "CAN";
				case "AU":
					return "AUS";
				default:
					throw new InvalidCountryCodeException(alpha2);
			}
		}


		public SearchResult Search(string country, string sSearch)
		{
			SearchResult searchResult = _quickAddress.Search(country, sSearch, PromptSet.Types.Default, CurrentLayout);

			return searchResult;
		}

		/// <summary>
		/// Process the search result returned from the service.
		/// </summary>
		/// <param name="addressValidationResult">Will be populated with the addresses converted to local address values</param>
		/// <param name="searchResult">Contains QAS searched addresses</param>
		/// <returns></returns>
		public IAddressValidationResult ProcessResponse(IAddressValidationResult addressValidationResult, SearchResult searchResult)
		{
			if (searchResult == null)
				return addressValidationResult;

			addressValidationResult.Status = AddressInfoResultState.Success;
			addressValidationResult.Message = searchResult.VerifyLevel.ToString();
			addressValidationResult = BuildResultByResultType(addressValidationResult, searchResult);

			return addressValidationResult;
		}

		public IAddressValidationResult BuildResultByResultType(IAddressValidationResult addressValidationResult, SearchResult searchResult)
		{
			switch (searchResult.VerifyLevel)
			{
				case SearchResult.VerificationLevels.Verified:
				case SearchResult.VerificationLevels.StreetPartial:
				case SearchResult.VerificationLevels.InteractionRequired:
				case SearchResult.VerificationLevels.Multiple:
				case SearchResult.VerificationLevels.PremisesPartial:
				case SearchResult.VerificationLevels.VerifiedPlace:
				case SearchResult.VerificationLevels.VerifiedStreet:

					// convert addresses here ---
					addressValidationResult =
							Create.Mutation(addressValidationResult, t => { t.ValidAddresses = TranslateFromQasAddress(searchResult); });
					break;

				case SearchResult.VerificationLevels.None:
					addressValidationResult.ValidAddresses = new List<IValidationAddress>();
					break;
			}

			return addressValidationResult;
		}

		public IValidationAddress AssignAddressValue(AddressLine[] addressLines)
		{
			var address = Create.New<IValidationAddress>();

			switch (addressLines.Length)
			{
				case 1:
					address.Address1 = addressLines[0].Line;
					break;
				case 2:
					address.Address1 = addressLines[0].Line;
					address.Address2 = addressLines[1].Line;
					break;
				case 3:
					address.Address1 = addressLines[0].Line;
					address.Address2 = addressLines[1].Line;
					address.Address3 = addressLines[2].Line;
					break;
				case 4:
					address.Address1 = addressLines[0].Line;
					address.Address2 = addressLines[1].Line;
					address.Address3 = addressLines[2].Line;
					address.SubDivision = addressLines[CityIndex].Line;
					break;
				case 5:
					address.Address1 = addressLines[0].Line;
					address.Address2 = addressLines[1].Line;
					address.Address3 = addressLines[2].Line;
					address.SubDivision = addressLines[CityIndex].Line;
					address.MainDivision = addressLines[StateIndex].Line;
					break;
				case 6:
					address.Address1 = addressLines[0].Line;
					address.Address2 = addressLines[1].Line;
					address.Address3 = addressLines[2].Line;
					address.SubDivision = addressLines[CityIndex].Line;
					address.MainDivision = addressLines[StateIndex].Line;
					address.PostalCode = addressLines[ZipIndex].Line;
					break;
				case 7:
					address.Address1 = addressLines[0].Line;
					address.Address2 = addressLines[1].Line;
					address.Address3 = addressLines[2].Line;
					address.SubDivision = addressLines[3].Line;
					address.MainDivision = addressLines[4].Line;
					address.PostalCode = addressLines[5].Line;
					address.Country = addressLines[6].Line;
					break;
				case 9:
					address.Address1 = addressLines[1].Line;
					address.Address2 = addressLines[2].Line;
					address.Address3 = addressLines[3].Line;
					address.SubDivision = addressLines[4].Line;
					address.MainDivision = addressLines[6].Line;
					address.PostalCode = addressLines[7].Line;
					address.Country = addressLines[8].Line;
					break;
			}

			return address;
		}
	}
}