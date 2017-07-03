using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using NetSteps.Addresses.PickupPoints.Common.Models;
using NetSteps.Addresses.PickupPoints.Common.Services.Contracts;


namespace NetSteps.Addresses.PickupPoints.Common.Services
{
  [ContractClass(typeof(PickupPointServiceContracts))]
  public interface IPickupPointService
  {
	IEnumerable<IPickupPointModel> GetPickupPoints(CultureInfo culture, string postalCode, string city);
	void SavePickupPoint(IPickupPointModel pickupPointModel);
  }

  namespace Contracts
  {
	using System;
	using System.Diagnostics.Contracts;
	using System.Globalization;

	[ContractClassFor(typeof(IPickupPointService))]
	internal abstract class PickupPointServiceContracts : IPickupPointService
	{
	  public IEnumerable<IPickupPointModel> GetPickupPoints(CultureInfo culture, string postalCode, string city)
	  {
		Contract.Requires<ArgumentException>(culture != null, "Culture parameter is required");
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(culture.Name));
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(postalCode), "Postal code parameter is required");
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(city), "City parameter is required");
		throw new NotImplementedException();
	  }

	  public void SavePickupPoint(IPickupPointModel pickupPointModel)
	  {
		Contract.Requires<ArgumentException>(pickupPointModel != null);
		Contract.Requires<ArgumentException>(pickupPointModel.PickupPointAddress != null);
		throw new NotImplementedException();
	  }
	}
  }
}

