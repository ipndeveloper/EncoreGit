using System;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class ShippingRegionRepository
	{
		protected override Func<NetStepsEntities, IQueryable<ShippingRegion>> loadAllFullQuery
		{
			get
			{
				return CompiledQuery.Compile<NetStepsEntities, IQueryable<ShippingRegion>>(
				 (context) => context.ShippingRegions.Include("StateProvinces")
                     .Include("Warehouse")
                     .Include("Warehouse.ShippingRegions"));
			}
		}

		public ShippingRegion LoadByName(string shippingRegionName)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (var context = new NetStepsEntities())
				{
					var result = context.ShippingRegions.FirstOrDefault(p => p.Name.Equals(shippingRegionName, StringComparison.OrdinalIgnoreCase));

					if (result != null)
					{
						result.StartEntityTracking();
					}

					return result;
				}
			});
		}
	}
}
