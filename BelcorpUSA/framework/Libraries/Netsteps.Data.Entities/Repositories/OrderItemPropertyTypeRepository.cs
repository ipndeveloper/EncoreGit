using System;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class OrderItemPropertyTypeRepository
    {
        protected override Func<NetStepsEntities, IQueryable<OrderItemPropertyType>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<OrderItemPropertyType>>(
                 (context) => context.OrderItemPropertyTypes.Include("OrderItemPropertyValues"));
            }
        }

        public OrderItemPropertyType GetByName(string name)
        {
            NetStepsEntities context = CreateContext();
            IQueryable<OrderItemPropertyType> propertyTypes = context.OrderItemPropertyTypes.Include("OrderItemPropertyValues");
            return propertyTypes.FirstOrDefault(pt => pt.Name.Equals(name));
        }

        public PaginatedList<OrderItemPropertyType> Search(FilterPaginatedListParameters<OrderItemPropertyType> searchParams)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    PaginatedList<OrderItemPropertyType> results = new PaginatedList<OrderItemPropertyType>(searchParams);

                    IQueryable<OrderItemPropertyType> propertyTypes = context.OrderItemPropertyTypes.Include("OrderItemPropertyValues");

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
                    string sql = @"DELETE FROM OrderItemProperties WHERE OrderItemPropertyTypeID = @p0;
						DELETE FROM OrderItemPropertyValues WHERE OrderItemPropertyTypeID = @p0;
						DELETE FROM OrderItemTypeProperties WHERE OrderItemPropertyTypeID = @p0;
						DELETE FROM OrderItemPropertyTypes WHERE OrderItemPropertyTypeID = @p0;";

                    context.ExecuteStoreCommand(sql, primaryKey);
                }
            });
        }

        public override SqlUpdatableList<OrderItemPropertyType> LoadAllFullWithSqlDependency()
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = new NetStepsEntities())
                {
                    var orderItemPropertyTypes = context.OrderItemPropertyTypes
                        .Include("OrderItemPropertyValues")
                        .ToList();

                    SqlUpdatableList<OrderItemPropertyType> list = new SqlUpdatableList<OrderItemPropertyType>();

                    list.AddRange(orderItemPropertyTypes);

                    return list;
                }
            });
        }

    }
}
