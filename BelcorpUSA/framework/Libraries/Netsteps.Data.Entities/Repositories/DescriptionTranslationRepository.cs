using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Common.Utility;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class DescriptionTranslationRepository
	{
		#region Members
		protected override Func<NetStepsEntities, IQueryable<DescriptionTranslation>> loadAllFullQuery
		{
			get
			{
				return CompiledQuery.Compile<NetStepsEntities, IQueryable<DescriptionTranslation>>(
				   (context) => from a in context.DescriptionTranslations
								select a);
			}
		}
		#endregion

		public virtual IEnumerable<DescriptionTranslation> LoadByProductID(int productId)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return (from dt in context.DescriptionTranslations
						   where dt.Products.Select(x => x.ProductID).Contains(productId)
							select dt).ToList();
				}
			});
		}
	}
}
