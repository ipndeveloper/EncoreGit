using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NetSteps.Addresses.PickupPoints.Common.Models;
using NetSteps.Addresses.PickupPoints.Common.Services;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Generated;
using NetSteps.Encore.Core.IoC;
using NetSteps.Diagnostics.Utilities;

namespace NetSteps.Addresses.PickupPoints.Services
{
	[ContainerRegister(typeof(IPickupPointRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class PickupPointRepository : IPickupPointRepository
	{
		public IPickupPoint GetPickupPoint(int addressID)
		{
			PickupPoint pickupPoint = PickupPoint.LoadByAddressID(addressID);
			var returnValue = Create.New<IPickupPoint>();

			returnValue.AddressID = pickupPoint.AddressID;
			returnValue.Code = pickupPoint.PickupPointCode;
			returnValue.PickupPointID = pickupPoint.PickupPointID;
			return returnValue;
		}

		public IEnumerable<IPickupPointAddress> GetAddresses(CultureInfo culture, string postalCode, string city)
		{
			using (var getAddressesTrace = this.TraceActivity(string.Format("PickupPointRepository::GetAddresses for culture {0}", culture != null ? culture.Name : "null")))
			{
				try
				{
					var country = Create.New<ICountriesProvider>().GetCountries().FirstOrDefault(x => string.Equals(x.CultureCode, culture.Name, StringComparison.InvariantCultureIgnoreCase));
					if (country == null) throw new NullReferenceException(string.Format("country was not found: culture {0}", culture != null ? culture.Name : "null"));

					//var addresses = Create.New<IPostalCodeLookupProvider>().LookupPostalCode(country.CountryID, postalCode).Where(x => city.Equals(x.City, StringComparison.InvariantCultureIgnoreCase)).ToList();

					var allAddresses = Address.GetByAddressTypePostalCodeAndCity((int)ConstantsGenerated.AddressType.PickupPoint, postalCode, city);
					var returnValue = new List<IPickupPointAddress>();

					foreach (var item in allAddresses)
					{
						var toAdd = Create.New<IPickupPointAddress>();
						toAdd.AddressID = item.AddressID;
						toAdd.Address1 = item.Address1;
						toAdd.Address2 = item.Address2;
						toAdd.Address3 = item.Address3;
						toAdd.City = item.City;
						toAdd.State = item.State;
						toAdd.PostalCode = item.PostalCode;
						returnValue.Add(toAdd);
					}

					return returnValue;
				}
				catch (Exception excp)
				{
					excp.TraceException(excp);
					throw;
				}
			}
		}

		public void Save(IPickupPointModel pickupPointModel)
		{
			Save(pickupPointModel.PickupPointAddress);
			var pickupPoint = new PickupPoint
			{
				PickupPointID = pickupPointModel.PickupPointID,
				PickupPointCode = pickupPointModel.PickupPointCode,
				AddressID = pickupPointModel.PickupPointAddress.AddressID
			};

			pickupPoint.Save();
			pickupPointModel.PickupPointID = pickupPoint.PickupPointID;
		}

		public void Save(IPickupPointAddress pickupPointAddress)
		{
			var country = Create.New<ICountriesProvider>().GetCountries().FirstOrDefault(x => string.Equals(x.Name, pickupPointAddress.Country, StringComparison.InvariantCultureIgnoreCase));
			if (country == null)
				throw new ArgumentException();

			var address = new NetSteps.Data.Entities.Address();
			address.StartEntityTracking();
			address.AddressID = pickupPointAddress.AddressID;
			address.Address1 = pickupPointAddress.Address1;
			address.Address2 = pickupPointAddress.Address2;
			address.Address3 = pickupPointAddress.Address3;
			address.City = pickupPointAddress.City;
			address.State = pickupPointAddress.State;
			address.PostalCode = pickupPointAddress.PostalCode;
			address.CountryID = country.CountryID;

			address.LookUpAndSetGeoCode();
			address.Save();

			pickupPointAddress.AddressID = address.AddressID;
		}
	}
}
