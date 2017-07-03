using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class AccountPropertyTypeRepository
    {
        protected override Func<NetStepsEntities, IQueryable<AccountPropertyType>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<AccountPropertyType>>(
                 (context) => context.AccountPropertyTypes.Include("AccountPropertyValues"));
            }
        }

        public List<AccountPropertyType> LoadAllFullAccountPropertyType()
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var result = loadAllFullQuery(context).ToList();
                    return result;
                }
            });
        }


        public PaginatedList<AccountPropertyType> Search(FilterPaginatedListParameters<AccountPropertyType> searchParams)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    PaginatedList<AccountPropertyType> results = new PaginatedList<AccountPropertyType>(searchParams);

                    IQueryable<AccountPropertyType> propertyTypes = context.AccountPropertyTypes.Include("AccountPropertyValues");

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

    }
}
