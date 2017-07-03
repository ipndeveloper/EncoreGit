using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NetSteps.Addresses.PickupPoints.Common.Models;
using NetSteps.Addresses.PickupPoints.Common.Services;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Addresses.PickupPoints.Services
{
  public class PickupPointRepositoryMock : IPickupPointRepository
  {
	List<IPickupPoint> pickup = new List<IPickupPoint>();
	List<IPickupPointAddress> pickupPointAddresses = new List<IPickupPointAddress>();

	public PickupPointRepositoryMock()
	{
	  for (int i = 0; i < 5; i++)
	  {
		var pickupPoint = Create.New<IPickupPoint>();
		pickupPoint.PickupPointID = i + 1;
		pickupPoint.Code = "XYZ" + i;
		pickupPoint.AddressID = i + 1;

		var address = Create.New<IPickupPointAddress>();
		address.Address1 = "XYZ" + i;
		address.City = "Vouhe";
		address.PostalCode = "79310";
		address.Country = "US";
		address.AddressID = i + 1;

		pickup.Add(pickupPoint);
		pickupPointAddresses.Add(address);
	  }
	}

	public IPickupPoint GetPickupPoint(int addressID)
	{
	  return CopyPickupPoint(pickup.FirstOrDefault(x => x.AddressID == addressID));
	}

	public IEnumerable<IPickupPointAddress> GetAddresses(CultureInfo culture, string postalCode, string city)
	{
	  return pickupPointAddresses.Where(x =>
		x.PostalCode.Equals(postalCode, StringComparison.InvariantCultureIgnoreCase) &&
		x.City.Equals(city, StringComparison.InvariantCultureIgnoreCase))
		.Select(x => CopyPickupPointAddress(x));
	}

	public void Save(IPickupPointModel pickupPointModel)
	{
	  throw new NotImplementedException();
	}

	public void Save(IPickupPointAddress pickupPointAddress)
	{
	  throw new NotImplementedException();
	}

	#region Mock Helpers
	private IPickupPoint CopyPickupPoint(IPickupPoint point)
	{
	  var pickupPointReturn = Create.New<IPickupPoint>();

	  pickupPointReturn.AddressID = point.AddressID;
	  pickupPointReturn.Code = point.Code;
	  pickupPointReturn.PickupPointID = point.PickupPointID;

	  return pickupPointReturn;
	}

	private IPickupPointAddress CopyPickupPointAddress(IPickupPointAddress point)
	{
	  var pickupPointAddressReturn = Create.New<IPickupPointAddress>();

	  pickupPointAddressReturn.AddressID = point.AddressID;
	  pickupPointAddressReturn.Address1 = point.Address1;
	  pickupPointAddressReturn.Address2 = point.Address2;
	  pickupPointAddressReturn.Address3 = point.Address3;
	  pickupPointAddressReturn.City = point.City;
	  pickupPointAddressReturn.Country = point.Country;
	  pickupPointAddressReturn.PostalCode = point.PostalCode;
	  pickupPointAddressReturn.State = point.State;

	  return pickupPointAddressReturn;
	}
	#endregion
  }
}
