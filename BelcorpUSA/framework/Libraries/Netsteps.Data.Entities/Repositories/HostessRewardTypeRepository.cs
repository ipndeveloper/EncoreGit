using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Expressions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class HostessRewardTypeRepository
	{
		public List<int> GetAvailableCatalogs(IEnumerable<int> rewardTypes)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var where = ExpressionHelper.MakeWhereInExpression<HostessRewardType, int>(r => r.HostessRewardTypeID, rewardTypes);
					return context.HostessRewardTypes.Where(where).SelectMany(r => r.Catalogs).Select(c => c.CatalogID).ToList();
				}
			});
		}
	}
}
