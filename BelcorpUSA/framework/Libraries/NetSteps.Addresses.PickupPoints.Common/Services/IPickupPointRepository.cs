using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using NetSteps.Addresses.PickupPoints.Common.Models;
using NetSteps.Addresses.PickupPoints.Common.Services.Contracts;

namespace NetSteps.Addresses.PickupPoints.Common.Services
{
  [ContractClass(typeof(PickupPointRepositoryContracts))]
  public interface IPickupPointRepository
  {
	IPickupPoint GetPickupPoint(int addressID);
	IEnumerable<IPickupPointAddress> GetAddresses(CultureInfo culture, string postalCode, string city);
	void Save(IPickupPointModel pickupPointModel);
	void Save(IPickupPointAddress pickupPointAddress);
  }

  namespace Contracts
  {
	using System;

	[ContractClassFor(typeof(IPickupPointRepository))]
	internal abstract class PickupPointRepositoryContracts : IPickupPointRepository
	{
	  public IEnumerable<IPickupPointModel> GetPickupPoints(CultureInfo culture, string postalCode, string city)
	  {
		Contract.Requires<ArgumentException>(culture != null, "Culture parameter is required");
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(culture.Name));
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(postalCode), "Postal code parameter is required");
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(city), "City parameter is required");
		throw new NotImplementedException();
	  }

	  public IPickupPoint GetPickupPoint(int addressID)
	  {
		Contract.Requires<ArgumentException>(addressID > 0);
		throw new NotImplementedException();
	  }

	  public IEnumerable<IPickupPointAddress> GetAddresses(CultureInfo culture, string postalCode, string city)
	  {
		Contract.Requires<ArgumentException>(culture != null);
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(postalCode));
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(city));
		throw new NotImplementedException();
	  }

	  public void Save(IPickupPointModel pickupPointModel)
	  {
		Contract.Requires<ArgumentException>(pickupPointModel != null);
		Contract.Requires<ArgumentException>(pickupPointModel.PickupPointAddress != null);
		throw new NotImplementedException();
	  }

	  public void Save(IPickupPointAddress pickupPointAddress)
	  {
		Contract.Requires<ArgumentException>(pickupPointAddress != null);
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(pickupPointAddress.Country));
		throw new NotImplementedException();
	  }
	}
  }
}
