using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using AddressValidator.Common;
using NetSteps.Addresses.Common.Models;
using NetSteps.Addresses.UI.Common.Models;
using NetSteps.Addresses.UI.Common.Services;
using NetSteps.Addresses.UI.Models;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
// using NetSteps.Data.Common.Entities;


namespace NetSteps.Addresses.UI.Services
{
	[ContainerRegister( typeof( IAddressesService ), RegistrationBehaviors.Default)]
	public class AddressesService : IAddressesService
	{
		#region Fields

		protected bool _initialized = false;

		#endregion

		#region Properties

		public IAddressCountrySettingsRegistry CountrySettingsRegistry { get; private set; }
		public IAddressModelRegistry AddressModelRegistry { get; private set; }

		#endregion

		#region Constructor

		public AddressesService( )
			: this( Create.New<IAddressCountrySettingsRegistry>( ), Create.New<IAddressModelRegistry>( ) )
		{
		}

		public AddressesService( IAddressCountrySettingsRegistry countrySettingsRegistry, IAddressModelRegistry addressModelRegistry )
		{
			Contract.Requires<ArgumentNullException>( countrySettingsRegistry != null );
			Contract.Requires<ArgumentNullException>( addressModelRegistry != null );

			CountrySettingsRegistry = countrySettingsRegistry;
			AddressModelRegistry = addressModelRegistry;

			Initialize( );
		}

		#endregion

		#region Methods

		protected void Initialize( )
		{
			if ( !_initialized )
			{
				var countrySettings = Create.New<IAddressCountrySettingsModel>( );
				countrySettings.ForCountryCode = "AU";
				RegisterAddressModelType<AddressUIModel_AU>( countrySettings.ForCountryCode );
				RegisterCountrySettings( countrySettings );

				countrySettings = Create.New<IAddressCountrySettingsModel>( );
				countrySettings.ForCountryCode = "BE";
				RegisterAddressModelType<AddressUIModel_BE>( countrySettings.ForCountryCode );
				RegisterCountrySettings( countrySettings );

				countrySettings = Create.New<IAddressCountrySettingsModel>( );
				countrySettings.ForCountryCode = "CA";
				RegisterAddressModelType<AddressUIModel_CA>( countrySettings.ForCountryCode );
				RegisterCountrySettings( countrySettings );

				countrySettings = Create.New<IAddressCountrySettingsModel>( );
				countrySettings.ForCountryCode = "DE";
				RegisterAddressModelType<AddressUIModel_DE>( countrySettings.ForCountryCode );
				RegisterCountrySettings( countrySettings );

				countrySettings = Create.New<IAddressCountrySettingsModel>( );
				countrySettings.ForCountryCode = "FR";
				RegisterAddressModelType<AddressUIModel_FR>( countrySettings.ForCountryCode );
				RegisterCountrySettings( countrySettings );

				countrySettings = Create.New<IAddressCountrySettingsModel>( );
				countrySettings.ForCountryCode = "GB";
				RegisterAddressModelType<AddressUIModel_GB>( countrySettings.ForCountryCode );
				RegisterCountrySettings( countrySettings );

				countrySettings = Create.New<IAddressCountrySettingsModel>( );
				countrySettings.ForCountryCode = "IE";
				RegisterAddressModelType<AddressUIModel_IE>( countrySettings.ForCountryCode );
				RegisterCountrySettings( countrySettings );

				countrySettings = Create.New<IAddressCountrySettingsModel>( );
				countrySettings.ForCountryCode = "LU";
				RegisterAddressModelType<AddressUIModel_LU>( countrySettings.ForCountryCode );
				RegisterCountrySettings( countrySettings );

				countrySettings = Create.New<IAddressCountrySettingsModel>( );
				countrySettings.ForCountryCode = "NL";
				RegisterAddressModelType<AddressUIModel_NL>( countrySettings.ForCountryCode );
				RegisterCountrySettings( countrySettings );

				countrySettings = Create.New<IAddressCountrySettingsModel>( );
				countrySettings.ForCountryCode = "SE";
				RegisterAddressModelType<AddressUIModel_SE>( countrySettings.ForCountryCode );
				RegisterCountrySettings( countrySettings );

				countrySettings = Create.New<IAddressCountrySettingsModel>( );
				countrySettings.ForCountryCode = "US";
				countrySettings.PostalCodeLookupEnabled = true;
				countrySettings.RequiresScrubbing = true;
				RegisterAddressModelType<AddressUIModel_US>( countrySettings.ForCountryCode );
				RegisterCountrySettings( countrySettings );

				_initialized = true;
			}
		}

		public IEnumerable<Country> GetAvailableCountries( int marketId )
		{
			IEnumerable<Country> result = null;
			ICountryRepository countryRepo = Create.New<ICountryRepository>( );

			if ( countryRepo != null )
				result = countryRepo.Where( c => c.MarketID == marketId && c.Active ).ToList( );

			return result;
		}

		public IAddressCountrySettingsModel GetCountrySettings( string countryCode )
		{
			IAddressCountrySettingsModel result = null;

			if ( !CountrySettingsRegistry.TryGetCountrySettings( countryCode, out result ) )
				result = Create.New<IAddressCountrySettingsModel>( );

			return result;
		}

		public IAddressUIModel GetMarketAddressModel( int marketID )
		{
			var countries = GetAvailableCountries( marketID );
			return GetCountryAddressModel( countries.First( ).CountryCode );
		}

		public IAddressUIModel GetCountryAddressModel( string countryCode )
		{
			IAddressUIModel result = null;

			if ( !this.AddressModelRegistry.TryAddressModelInstance( countryCode, out result ) )
				result = Create.New<IAddressUIModel>( );
			result.CountryCode = countryCode;
			return result;
		}

		public void RegisterCountrySettings( IAddressCountrySettingsModel countrySettings )
		{
			CountrySettingsRegistry.RegisterCountrySettings( countrySettings );
		}

		public void RegisterAddressModelType<T>( string countryCode ) where T : IAddressUIModel
		{
			AddressModelRegistry.RegisterAddressModelTypeForCountry<T>( countryCode );
		}

		public IAddressValidationResult GetScrubbedAddressSuggestions( IAddressUIModel fromAddress )
		{
			IValidationAddress addressToValidate = Create.New<IValidationAddress>( );
			addressToValidate.Address1 = fromAddress.Address1;
			addressToValidate.Address2 = fromAddress.Address2;
			addressToValidate.Address3 = fromAddress.Address3;
			addressToValidate.SubDivision = fromAddress.City;
			addressToValidate.MainDivision = fromAddress.StateProvince;
			addressToValidate.PostalCode = fromAddress.PostalCode;
			addressToValidate.Country = fromAddress.CountryCode;

			IAddressValidator validator = Create.New<IAddressValidator>( );
			IAddressValidationResult result = validator.ValidateAddress( addressToValidate );

			return result;
		}

		public IAddressValidationResult2 GetScrubbedAddressSuggestions2( IAddressUIModel fromAddress )
		{
			var prelimResults = GetScrubbedAddressSuggestions( fromAddress );

			IAddressValidationResult2 result = Create.New<IAddressValidationResult2>( );
			result.Message = prelimResults.Message;
			result.Status = prelimResults.Status;
			List<IAddressUIModel> suggestions = new List<IAddressUIModel>( );
			result.ValidAddresses = suggestions;

			foreach ( var cleansedAddress in prelimResults.ValidAddresses )
			{
				var model = GetCountryAddressModel( fromAddress.CountryCode );
				model.CountryCode = cleansedAddress.Country;
				model.Address1 = cleansedAddress.Address1;
				model.Address2 = cleansedAddress.Address2;
				model.Address3 = cleansedAddress.Address3;
				model.StateProvince = cleansedAddress.MainDivision;

				var states = SmallCollectionCache.Instance.StateProvinces.Where(
					i =>
						i.Name.ToLower( ) == model.StateProvince.ToLower( )
						|| i.StateAbbreviation.ToLower( ) == model.StateProvince.ToLower( )
				).ToList( );

				if ( states.Count > 0 )
				{
					StateProvince state = states[ 0 ];
					model.StateProvince = state.StateAbbreviation;
					model.StateProvinceID = state.StateProvinceID;
				}

				model.City = cleansedAddress.SubDivision;
				model.PostalCode = cleansedAddress.PostalCode;

				// Populate non-cleanse fields
				model.Attention = fromAddress.Attention;
				model.FirstName = fromAddress.FirstName;
				model.LastName = fromAddress.LastName;
				model.PhoneNumber = fromAddress.PhoneNumber;
				model.PhoneTypeID = fromAddress.PhoneTypeID;

				suggestions.Add( model );
			}

			return result;
		}

		public IAddressUIModel GetUIModelFromAddressEntity( Address entity )
		{
			var countryCode = SmallCollectionCache.Instance.Countries.First( cntry => cntry.CountryID == entity.CountryID ).CountryCode;
			var model = GetCountryAddressModel( countryCode );
			CopyEntityToAddressModel( entity, model );

			if ( entity.PhoneTypeID.HasValue )
			{
				model.PhoneTypeID = entity.PhoneTypeID;
			}

			return model;
		}

		public IAddressUIModel GetUIModelFromAddressEntity( IAddress entity )
		{
			var countryCode = SmallCollectionCache.Instance.Countries.First( cntry => cntry.CountryID == entity.CountryID ).CountryCode;
			var model = GetCountryAddressModel( countryCode );
			CopyEntityToAddressModel( entity, model );
			return model;
		}

		public Address GetAddressEntityFromUIModel( IAddressUIModel model )
		{
			Address address = new Address( );
			CopyAddressModelToEntity( model, address );
			return address;
		}

		public void CopyAddressModelToEntity( IAddressUIModel model, Address entity )
		{
			entity.Attention = model.Attention;
			entity.Address1 = model.Address1;
			entity.Address2 = model.Address2;
			entity.Address3 = model.Address3;
			entity.PostalCode = model.PostalCode;
			entity.County = model.County;
			entity.City = model.City;
			entity.FirstName = model.FirstName;
			entity.LastName = model.LastName;
			entity.PhoneNumber = model.PhoneNumber;
			entity.State = model.StateProvince;

			if ( model.PhoneTypeID.HasValue )
			{
				PhoneType phoneType = SmallCollectionCache.Instance.PhoneTypes.First( pt => pt.PhoneTypeID == model.PhoneTypeID.Value );
				entity.PhoneType = phoneType;
			}

			if ( model.StateProvinceID.HasValue )
			{
				StateProvince state = SmallCollectionCache.Instance.StateProvinces.First( st => st.StateProvinceID == model.StateProvinceID.Value );
				entity.StateProvince = state;
			}

			Country country = SmallCollectionCache.Instance.Countries.First( c => c.CountryCode == model.CountryCode );
			entity.Country = country;
		}

		protected virtual void CopyEntityToAddressModel( IAddress entity, IAddressUIModel model )
		{
			model.Attention = entity.Attention;
			model.Address1 = entity.Address1;
			model.Address2 = entity.Address2;
			model.Address3 = entity.Address3;
			model.PostalCode = entity.PostalCode;
			model.County = entity.County;
			model.City = entity.City;
			model.FirstName = entity.FirstName;
			model.LastName = entity.LastName;
			model.PhoneNumber = entity.PhoneNumber;
			model.StateProvince = entity.State;

			if ( entity.StateProvinceID.HasValue )
			{
				model.StateProvinceID = entity.StateProvinceID;
			}

			Country country = SmallCollectionCache.Instance.Countries.First( c => c.CountryID == entity.CountryID );
			model.CountryCode = country.CountryCode;
		}

		#endregion
	}
}