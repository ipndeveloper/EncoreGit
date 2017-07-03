using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class AutoshipScheduleRepository
	{
		protected override Func<NetStepsEntities, IQueryable<AutoshipSchedule>> loadAllFullQuery
		{
			get
			{
				return CompiledQuery.Compile<NetStepsEntities, IQueryable<AutoshipSchedule>>((context) => context.AutoshipSchedules
					 .Include("AutoshipScheduleDays")
					 .Include("AccountTypes")
					 .Include("AutoshipScheduleProducts")
				);
			}
		}

		public List<AutoshipSchedule> LoadByAccountTypeID(int accountTypeID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.AutoshipSchedules.Where(s => s.AccountTypes.Any(at => at.AccountTypeID == accountTypeID)).ToList();
				}
			});
		}

		public List<AutoshipSchedule> LoadFullByAccountTypeID(int accountTypeID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return loadAllFullQuery(context).Where(s => s.AccountTypes.Any(at => at.AccountTypeID == accountTypeID)).ToList();
				}
			});
		}
	}
}
