using System;
using System.Data.Objects;
using System.Linq;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class ProductTypeRepository
	{
		protected override Func<NetStepsEntities, IQueryable<ProductType>> loadAllFullQuery
		{
			get
			{
				return CompiledQuery.Compile<NetStepsEntities, IQueryable<ProductType>>(
				 (context) => context.ProductTypes.Include("ProductPropertyTypes").Include("ProductPropertyTypes.ProductPropertyValues"));
			}
		}
	}
}
