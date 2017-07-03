using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class DistributionListRepository
    {
        #region Members
        protected override Func<NetStepsEntities, IQueryable<DistributionList>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<DistributionList>>(
                   (context) => from a in context.DistributionLists
                                               .Include("DistributionSubscribers")
                                select a);
            }
        }
        #endregion

        public List<DistributionList> LoadByAccountID(int accountID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.DistributionLists.Where(c => c.AccountID == accountID).ToList();
                }
            });
        }

        public List<DistributionList> LoadByAccountIDFull(int accountID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return loadAllFullQuery(context).Where(c => c.AccountID == accountID).ToList();
                }
            });
        }
    }
}
