using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Addresses.PickupPoints.Common.Services;
using NetSteps.Addresses.PickupPoints.Common.Models;
using NetSteps.Diagnostics.Utilities;
using NetSteps.Encore.Core.IoC;

namespace BelcorpUSA.Addresses.PickupPoints
{
	[ContainerRegister(typeof(IPickupPointService), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class PickupPointService : IPickupPointService
	{
		public IEnumerable<IPickupPointModel> GetPickupPoints(System.Globalization.CultureInfo culture, string postalCode, string city)
		{
			using (var getPickupPointsTrace = this.TraceActivity(string.Format("BelcorpUSA.Addresses.PickupPoints::GetPickupPoints: culture {0}", culture.Name)))
			{
				this.TraceEvent("empty implementation, returning empty list");
				return new List<IPickupPointModel>();
			}
		}

		public void SavePickupPoint(IPickupPointModel pickupPointModel)
		{
			using (var savePickupPointsTrace = this.TraceActivity("BelcorpUSA.Addresses.PickupPoints::SavePickupPoints"))
			{
				this.TraceEvent("empty implementation, did not save");
			}
		}
	}
}
