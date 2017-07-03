using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Commissions;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class PeriodRepository
	{
		#region Members
		#endregion

		public List<int> GetPeriodIds(int accountID)
		{
            return new List<int>();
            //TODO: Commissions Refactor - GetPeriods by AccountID
            //return ExceptionHandledDataAction.Run<List<int>>(new ExecutionContext(this), () =>
            //{
            //    using (CommissionsEntities context = new CommissionsEntities())
            //    {
            //        return context.uspGetPeriodsByAccountID(accountID)
            //            .Select(x => x.PeriodID).ToList();
            //    }
            //});
		}

        //TODO: Commissions Refactor - GetLastClosedPeriod
        //public Period GetLastClosedPeriod(int planID)
        //{
        //    return new Pe
        //    //return ExceptionHandledDataAction.Run<Period>(new ExecutionContext(this), () =>
        //    //{
        //    //    using (CommissionsEntities context = new CommissionsEntities())
        //    //    {
        //    //        return (from p in context.Periods
        //    //                         where p.PlanID==planID
        //    //                         orderby p.ClosedDate descending
        //    //                         select p).ToList().Where(p=>p.IsClosed()).FirstOrDefault();
                    
        //    //    }
        //    //});
        //}
	}
}
