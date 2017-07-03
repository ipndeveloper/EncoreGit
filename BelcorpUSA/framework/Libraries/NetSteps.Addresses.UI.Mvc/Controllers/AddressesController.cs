using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Addresses.UI.Common.Models;
using NetSteps.Addresses.UI.Common.Services;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Addresses.UI.Mvc.Controllers
{
	//[ContainerRegister(typeof(Common.Controllers.IAddressesController), RegistrationBehaviors.Default)]
	public class AddressesController : Controller, Common.Controllers.IAddressesController
	{
		#region Methods

		public virtual ActionResult PostalCodeLookup( string countryCode, string postalCode )
		{
			try
			{
				var countryId = SmallCollectionCache.Instance.Countries.First(ct => ct.CountryCode.ToLower() == countryCode.ToLower()).CountryID;
				var postalLookupProvider = Create.New<IPostalCodeLookupProvider>( );
				if ( postalCode.Length == 5 )
				{
					return Json( postalLookupProvider.LookupPostalCode( countryId, postalCode ).Select( r => new { city = r.City.ToTitleCase().Trim(), county = r.County.ToTitleCase().Trim(), stateId = r.StateID, state = r.StateAbbreviation.Trim() }).Distinct(), JsonRequestBehavior.AllowGet);
				}
				else if ( postalCode.Length == 9 )
				{
					string zipPlusFour = postalCode.Substring( 5 );
					postalCode = postalCode.Substring( 0, 5 );
					postalLookupProvider.LookupPostalCode( countryId, string.Format("{0}-{1}", postalCode, zipPlusFour)).Select(r => new { city = r.City.ToTitleCase().Trim(), county = r.County.ToTitleCase().Trim(), stateId = r.StateID, state = r.StateAbbreviation.Trim() }).Distinct();
				}
				return Json( new List<NetSteps.Common.Globalization.PostalCodeData>( ), JsonRequestBehavior.AllowGet );
			}
			catch ( Exception ex )
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException( ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException );
				return Json( new { result = false, message = exception.PublicMessage }, JsonRequestBehavior.AllowGet );
			}
		}

		public virtual JsonResult ScrubAddress( IAddressUIModel fromAddress )
		{
			var result = CleanseAddress( fromAddress );
			return Json( result, JsonRequestBehavior.AllowGet );
		}

		public virtual ActionResult CountryTemplate( IAddressUIModel uiModel, string clientSideAddressObjID, string clientSideAddressHtmlID )
		{
			IAddressesService service = Create.New<IAddressesService>( );
			// var model = service.GetCountryAddressModel(uiModel.CountryCode);
			ViewData[ "clientSideAddressHtmlID" ] = clientSideAddressHtmlID;
			ViewData[ "clientSideAddressObjID" ] = clientSideAddressObjID;
			return PartialView( "Addresses/AddressEntryCountryTemplate", uiModel );
		}

		#endregion

		#region Protected Methods

		protected IAddressValidationResult2 CleanseAddress( IAddressUIModel fromAddress )
		{
			IAddressesService addrService = Create.New<IAddressesService>( );
			return addrService.GetScrubbedAddressSuggestions2( fromAddress );
		}

		#endregion

		#region Static Methods


		#endregion
	}
}