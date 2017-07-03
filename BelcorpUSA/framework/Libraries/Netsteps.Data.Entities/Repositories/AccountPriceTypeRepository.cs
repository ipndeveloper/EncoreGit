using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class AccountPriceTypeRepository
	{
		public List<AccountPriceType> LoadAllByStoreFront(int storeFrontID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.AccountPriceTypes.Where(apt => apt.StoreFrontID == storeFrontID).ToList();
				}
			});
		}
	}
}
