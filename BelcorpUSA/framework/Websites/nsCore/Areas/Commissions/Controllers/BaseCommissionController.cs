using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities.Cache;
using nsCore.Controllers;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;

namespace nsCore.Areas.Commissions.Controllers
{

	public abstract class BaseCommissionController : BaseController
	{
		protected readonly ICommissionJobService _commissionJobService = Create.New<ICommissionJobService>();

		private readonly ICommissionsService _commissionsService = Create.New<ICommissionsService>();

		public BaseCommissionController()
		{
			var plans = _commissionsService.GetCommissionPlans().OrderByDescending(x => x.IsDefault).ToList();
			var plansWithJobs = new List<ICommissionPlan>();
			foreach (var plan in plans)
			{
				if (_commissionJobService.GetCommissionJobs(plan.DisbursementFrequency).Count() > 0)
				{
					plansWithJobs.Add(plan);
				}
			}
		}
	}
}
