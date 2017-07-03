using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class CatalogItemRepository
    {
        public PaginatedList<CatalogItem> Search(FilterPaginatedListParameters<CatalogItem> searchParams)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    PaginatedList<CatalogItem> results = new PaginatedList<CatalogItem>(searchParams);

                    IQueryable<CatalogItem> catalogItems = context.CatalogItems;

                    if (searchParams.WhereClause != null)
                        catalogItems = catalogItems.Where(searchParams.WhereClause);

                    catalogItems = catalogItems.ApplyOrderByFilter(searchParams, context);

                    // Set total count before applying pagination
                    results.TotalCount = catalogItems.Count();

                    catalogItems = catalogItems.ApplyPagination(searchParams);

                    results.AddRange(catalogItems);

                    return results;
                }
            });
        }


    }
}
