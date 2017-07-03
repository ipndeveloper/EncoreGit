using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class AccountTypeRepository
    {
        #region Members
        protected override Func<NetStepsEntities, IQueryable<AccountType>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<AccountType>>(context => context.AccountTypes
                    .Include("Roles")
                    .Include("Roles.Functions")
					.Include("AutoshipSchedules")
                );
            }
        }
        #endregion

        public override SqlUpdatableList<AccountType> LoadAllFullWithSqlDependency()
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var accountTypes = context.AccountTypes
                        .Include("Roles")
                        .Include("Roles.Functions")
                        .ToList();

                    SqlUpdatableList<AccountType> list = new SqlUpdatableList<AccountType>();

                    list.AddRange(accountTypes);

                    return list;
                }
            });
        }

        public List<Role> GetRolesByAccountType(short accountTypeID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return
                        context.AccountTypes.Where(at => at.AccountTypeID == accountTypeID).SelectMany(at => at.Roles).
                            ToList();
                }
            });
        }
    }
}
