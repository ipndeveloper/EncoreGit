using System;
using System.Linq;
using NetSteps.Data.Entities.Exceptions;
using System.Collections.Generic;
using NetSteps.Common.Utility;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class ProductPropertyRepository : IProductPropertyRepository
	{
		public virtual IEnumerable<ProductProperty> LoadByProductID(int productId)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.ProductProperties.Where(p => p.ProductID == productId).ToList();
				}
			});
			
		}

	}
}
