using System;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class ProductPropertyTypeRepository
    {
        protected override Func<NetStepsEntities, IQueryable<ProductPropertyType>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<ProductPropertyType>>(
                 (context) => context.ProductPropertyTypes.Include("ProductPropertyValues"));
            }
        }

        public PaginatedList<ProductPropertyType> Search(FilterPaginatedListParameters<ProductPropertyType> searchParams)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    PaginatedList<ProductPropertyType> results = new PaginatedList<ProductPropertyType>(searchParams);

                    IQueryable<ProductPropertyType> propertyTypes = context.ProductPropertyTypes.Include("ProductPropertyValues");

                    if (searchParams.WhereClause != null)
                        propertyTypes = propertyTypes.Where(searchParams.WhereClause);

                    results.TotalCount = propertyTypes.Count();

                    propertyTypes = propertyTypes.ApplyOrderByFilter(searchParams, context);

                    propertyTypes = propertyTypes.ApplyPagination(searchParams);

                    results.AddRange(propertyTypes.ToList());

                    return results;
                }
            });
        }

        public override void Delete(int primaryKey)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    string sql = @"DELETE FROM ProductProperties WHERE ProductPropertyTypeID = @p0;
						DELETE FROM ProductPropertyValues WHERE ProductPropertyTypeID = @p0;
						DELETE FROM ProductTypeProperties WHERE ProductPropertyTypeID = @p0;
                        DELETE FROM ProductBaseProperties WHERE ProductPropertyTypeID = @p0;
						DELETE FROM ProductPropertyTypes WHERE ProductPropertyTypeID = @p0;";

                    context.ExecuteStoreCommand(sql, primaryKey);
                }
            });
        }
    }
}
