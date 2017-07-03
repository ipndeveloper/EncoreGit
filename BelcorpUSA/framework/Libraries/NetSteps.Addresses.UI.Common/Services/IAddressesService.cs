using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using AddressValidator.Common;
using NetSteps.Addresses.Common.Models;
using NetSteps.Addresses.UI.Common.Models;
using NetSteps.Data.Entities;


namespace NetSteps.Addresses.UI.Common.Services
{

	[ContractClass( typeof( Contracts.AddressServiceContract ) )]
	public interface IAddressesService
	{
		IAddressCountrySettingsRegistry CountrySettingsRegistry { get; }
		IAddressModelRegistry AddressModelRegistry { get; }

		IEnumerable<Country> GetAvailableCountries( int marketID );
		IAddressCountrySettingsModel GetCountrySettings( string countryCode );
		IAddressUIModel GetMarketAddressModel( int marketID );
		IAddressUIModel GetCountryAddressModel( string countryCode );
		void RegisterCountrySettings( IAddressCountrySettingsModel countrySettings );
		void RegisterAddressModelType<T>( string countryCode ) where T : IAddressUIModel;

		IAddressValidationResult GetScrubbedAddressSuggestions( IAddressUIModel fromAddress );
		IAddressValidationResult2 GetScrubbedAddressSuggestions2( IAddressUIModel fromAddress );

		IAddressUIModel GetUIModelFromAddressEntity( Address entity );
		IAddressUIModel GetUIModelFromAddressEntity( IAddress entity );
		Address GetAddressEntityFromUIModel( IAddressUIModel model );
		void CopyAddressModelToEntity( IAddressUIModel model, Address entity );
	}

	namespace Contracts
	{
		[ContractClassFor( typeof( IAddressesService ) )]
		abstract class AddressServiceContract : IAddressesService
		{

			public IAddressCountrySettingsRegistry CountrySettingsRegistry
			{
				get
				{
					Contract.Ensures( Contract.Result<IAddressCountrySettingsRegistry>( ) != null );
					throw new NotImplementedException( );
				}
			}

			public IAddressModelRegistry AddressModelRegistry
			{
				get
				{
					Contract.Ensures( Contract.Result<IAddressModelRegistry>( ) != null );
					throw new NotImplementedException( );
				}
			}

			public IEnumerable<Country> GetAvailableCountries( int marketID )
			{
				Contract.Requires( marketID > 0 );

				throw new NotImplementedException( );
			}

			public IAddressCountrySettingsModel GetCountrySettings( string countryCode )
			{
				Contract.Requires( !String.IsNullOrWhiteSpace( countryCode ) );
				Contract.Ensures( Contract.Result<IAddressCountrySettingsModel>( ) != null );

				throw new NotImplementedException( );
			}

			public IAddressUIModel GetMarketAddressModel( int marketID )
			{
				Contract.Requires( marketID > 0 );
				Contract.Ensures( Contract.Result<IAddressUIModel>( ) != null );
				throw new NotImplementedException( );
			}

			public IAddressUIModel GetCountryAddressModel( string countryCode )
			{
				Contract.Requires( !String.IsNullOrWhiteSpace( countryCode ) );
				Contract.Ensures( Contract.Result<IAddressUIModel>( ) != null );

				throw new NotImplementedException( );
			}

			public void RegisterCountrySettings( IAddressCountrySettingsModel countrySettings )
			{
				Contract.Requires( countrySettings != null );

				throw new NotImplementedException( );
			}


			public IAddressValidationResult GetScrubbedAddressSuggestions( IAddressUIModel fromAddress )
			{
				Contract.Requires( fromAddress != null );

				throw new NotImplementedException( );
			}

			public void RegisterAddressModelType<T>( string countryCode ) where T : IAddressUIModel
			{
				Contract.Requires( !String.IsNullOrWhiteSpace( countryCode ) );

				throw new NotImplementedException( );
			}

			public IAddressUIModel GetUIModelFromAddressEntity( Address entity )
			{
				Contract.Requires( entity != null );
				Contract.Requires( entity.CountryID > 0 );
				Contract.Result<IAddressUIModel>( );

				throw new NotImplementedException( );
			}

			public IAddressUIModel GetUIModelFromAddressEntity( IAddress entity )
			{
				Contract.Requires( entity != null );
				Contract.Requires( entity.CountryID > 0 );
				Contract.Result<IAddressUIModel>( );

				throw new NotImplementedException( );
			}

			public Address GetAddressEntityFromUIModel( IAddressUIModel model )
			{
				Contract.Requires( model != null );
				Contract.Requires( !String.IsNullOrEmpty( model.CountryCode ) );
				Contract.Result<Address>( );

				throw new NotImplementedException( );
			}

			public void CopyAddressModelToEntity( IAddressUIModel model, Address entity )
			{
				Contract.Requires( entity != null );
				Contract.Requires( model != null );
				throw new NotImplementedException( );
			}


			public IAddressValidationResult2 GetScrubbedAddressSuggestions2( IAddressUIModel fromAddress )
			{
				Contract.Requires( fromAddress != null );
				throw new NotImplementedException( );
			}
		}
	}
}
