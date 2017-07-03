using System;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class AutoresponderRepository
	{
		protected override Func<NetStepsEntities, IQueryable<Autoresponder>> loadAllFullQuery
		{
			get
			{
				return CompiledQuery.Compile<NetStepsEntities, IQueryable<Autoresponder>>(
				 (context) => context.Autoresponders.Include("Translations").Include("Translations.EmailTemplate"));
			}
		}

		public PaginatedList<Autoresponder> Search(AutoresponderSearchParameters searchParams)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var results = new PaginatedList<Autoresponder>();
					IQueryable<Autoresponder> autoresponders = context.Autoresponders;

					if (searchParams.IsInternal.HasValue)
						autoresponders = autoresponders.Where(a => a.IsInternal == searchParams.IsInternal.Value);
					if (searchParams.IsExternal.HasValue)
						autoresponders = autoresponders.Where(a => a.IsExternal == searchParams.IsExternal.Value);
					if (searchParams.Active.HasValue)
						autoresponders = autoresponders.Where(a => a.Active == searchParams.Active.Value);

					if (searchParams.WhereClause != null)
						autoresponders = autoresponders.Where(searchParams.WhereClause);

					autoresponders = autoresponders.ApplyOrderByFilter(searchParams, context);

					results.TotalCount = autoresponders.Count();

					autoresponders = autoresponders.ApplyPagination(searchParams);

					results.AddRange(autoresponders);

					return results;
				}
			});
		}
	}
}
