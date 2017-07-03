using System;
using System.Collections.Generic;
using System.Net;
using AddressValidation.Common;
using AddressValidator.Common;
using AddressValidator.Ups.Config;
using AddressValidator.Ups.Exceptions;
using NetSteps.Encore.Core.IoC;
using XAVWebReference;

namespace AddressValidator.Ups
{
	[ContainerRegister(typeof(IAddressValidator), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class UpsAddressValidator : AbstractAddressValidator, IAddressValidator
	{
		UpsAddressValidatorConfiguration Configuration { get; set; }

		public UpsAddressValidator()
			: this(null)
		{
		}

		public UpsAddressValidator(UpsAddressValidatorConfiguration configuration)
		{
			var localConfig = configuration ?? UpsAddressValidatorConfiguration.Current;

			if (string.IsNullOrEmpty(localConfig.UserName) || string.IsNullOrEmpty(localConfig.Password))
				throw new UpsAddressValidatorCredentialException();

			Configuration = localConfig;
		}

		IEnumerable<IValidationAddress> TranslateUpsAddressResponseToValidationAddress(IEnumerable<CandidateType> candidateTypes)
		{
			var validAddresses = new List<IValidationAddress>();

			foreach (var candidateType in candidateTypes)
			{
				var validationAddress = Create.New<IValidationAddress>();

				if (candidateType.AddressKeyFormat.AddressLine.Length > 0)
					validationAddress.Address1 = candidateType.AddressKeyFormat.AddressLine[0];

				if (candidateType.AddressKeyFormat.AddressLine.Length > 1)
					validationAddress.Address2 = candidateType.AddressKeyFormat.AddressLine[1];

				if (candidateType.AddressKeyFormat.AddressLine.Length > 2)
					validationAddress.Address3 = candidateType.AddressKeyFormat.AddressLine[2];

				validationAddress.Country = candidateType.AddressKeyFormat.CountryCode;
				validationAddress.MainDivision = candidateType.AddressKeyFormat.PoliticalDivision1;
				validationAddress.SubDivision = candidateType.AddressKeyFormat.PoliticalDivision2;
				validationAddress.PostalCode = candidateType.AddressKeyFormat.PostcodePrimaryLow;

				if (!string.IsNullOrEmpty(candidateType.AddressKeyFormat.PostcodeExtendedLow))
					validationAddress.PostalCode += "-" + candidateType.AddressKeyFormat.PostcodeExtendedLow;

				validAddresses.Add(validationAddress);
			}

			return validAddresses;
		}

		XAVResponse SendUpsRequest(XAVRequest request)
		{
			ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
			return UpsService.ProcessXAV(request);
		}

		AddressKeyFormatType TranslateToUpsAddressFormat(IValidationAddress address)
		{
			var addressKeyFormatType = new AddressKeyFormatType
										   {
											   AddressLine = new[] { address.Address1, address.Address2, address.Address3 },
											   CountryCode = address.Country,
											   PoliticalDivision1 = address.MainDivision,
											   PoliticalDivision2 = address.SubDivision,
											   PostcodePrimaryLow = address.PostalCode
										   };
			return addressKeyFormatType;

		}

		XAVRequest BuildUpsRequest(IValidationAddress address)
		{
			return new XAVRequest()
			{
				AddressKeyFormat = TranslateToUpsAddressFormat(address),
				Request = new RequestType() { RequestOption = new[] { "3" } }
			};
		}

		XAVService UpsService
		{
			get
			{
				return new XAVService(Configuration.EndpointUrl) { UPSSecurityValue = UpsSecurity };
			}
		}

		UPSSecurity UpsSecurity
		{
			get
			{
				return new UPSSecurity() { UsernameToken = UpsSecurityUsernameToken, ServiceAccessToken = UpsSecurityServiceAccessToken };
			}
		}

		UPSSecurityUsernameToken UpsSecurityUsernameToken
		{
			get
			{
				return new UPSSecurityUsernameToken() { Password = Configuration.Password, Username = Configuration.UserName };
			}
		}

		UPSSecurityServiceAccessToken UpsSecurityServiceAccessToken
		{
			get
			{
				return new UPSSecurityServiceAccessToken() { AccessLicenseNumber = Configuration.AccessLicenseNumber };
			}
		}

		IAddressValidationResult BuildResultByResultType(IAddressValidationResult addressValidationResult, XAVResponse response)
		{
			switch (response.ItemElementName)
			{
				case ItemChoiceType.ValidAddressIndicator:
				case ItemChoiceType.AmbiguousAddressIndicator:
					addressValidationResult = Create.Mutation(addressValidationResult, t => { t.ValidAddresses = TranslateUpsAddressResponseToValidationAddress(response.Candidate); });
					break;
				case ItemChoiceType.NoCandidatesIndicator:
					addressValidationResult.ValidAddresses = new List<IValidationAddress>();
					break;
			}
			return addressValidationResult;
		}

		string FormatResponseMessage(ItemChoiceType itemChoiceType, string responseStatus)
		{
			return string.Format("{0}: {1}", Enum.GetName(typeof(ItemChoiceType), itemChoiceType), responseStatus);
		}


		protected override IAddressValidationResult PerformValidateAddress(IValidationAddress address)
		{
			var addressValidationResult = Create.New<IAddressValidationResult>();

			try
			{
				var request = BuildUpsRequest(address);
				var response = SendUpsRequest(request);

				if (response.Response.ResponseStatus.Code.Equals("1", StringComparison.InvariantCultureIgnoreCase))
				{
					addressValidationResult.Status = AddressInfoResultState.Success;
					addressValidationResult.Message = FormatResponseMessage(response.ItemElementName, response.Response.ResponseStatus.Description);
					addressValidationResult = BuildResultByResultType(addressValidationResult, response);
				}
				else
				{
					addressValidationResult.Status = AddressInfoResultState.Error;
					addressValidationResult.Message = response.Response.ResponseStatus.Description;
				}
			}
			catch (Exception ex)
			{
				addressValidationResult.Status = AddressInfoResultState.Error;
				addressValidationResult.Message = ex.Message;
			}

			return addressValidationResult;
		}
	}
}