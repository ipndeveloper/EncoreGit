using System.Collections.Generic;
using System.Globalization;
using NetSteps.Addresses.PickupPoints.Common.Models;
using NetSteps.Addresses.PickupPoints.Common.Services;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Addresses.PickupPoints.Services
{
	[ContainerRegister(typeof(IPickupPointService), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class PickupPointService : IPickupPointService
	{
		protected IPickupPointRepository pickupPointRepository;
		public PickupPointService()
			: this(null)
		{
		}

		public PickupPointService(IPickupPointRepository repository)
		{
			pickupPointRepository = repository ?? Create.New<IPickupPointRepository>();
		}

		public virtual IEnumerable<IPickupPointModel> GetPickupPoints(CultureInfo culture, string postalCode, string city)
		{
			var returnValue = new List<IPickupPointModel>();

			var addresses = pickupPointRepository.GetAddresses(culture, postalCode, city);

			foreach (var address in addresses)
			{
				IPickupPoint pickupPoint = pickupPointRepository.GetPickupPoint(address.AddressID);
				var modelToAdd = Create.New<IPickupPointModel>();
				modelToAdd.PickupPointCode = pickupPoint.Code;
				modelToAdd.PickupPointID = pickupPoint.PickupPointID;
				modelToAdd.PickupPointAddress = Create.New<IPickupPointAddress>();
				modelToAdd.PickupPointAddress.Address1 = address.Address1;
				modelToAdd.PickupPointAddress.Address2 = address.Address2;
				modelToAdd.PickupPointAddress.Address3 = address.Address3;
				modelToAdd.PickupPointAddress.City = address.City;
				modelToAdd.PickupPointAddress.State = address.State;
				modelToAdd.PickupPointAddress.Country = address.Country;
			
				returnValue.Add(modelToAdd);
			}

			return returnValue;
		}

		public virtual void SavePickupPoint(IPickupPointModel pickupPointModel)
		{
			pickupPointRepository.Save(pickupPointModel);			
		}
	}
}
