using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class PickupPointRepository : IPickupPointRepository
	{
		public PickupPoint GetPickupPoint(int addressID)
		{

			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.PickupPoints.FirstOrDefault(x => x.AddressID == addressID);
				}
			});

		}
	}
}
