using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Commissions.Models;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;
using nsCore.Models;
using nsCore.Areas.Commissions.CommissionRunners;

namespace nsCore.Areas.Commissions.Controllers
{
	public class PublishController : BaseCommissionController
	{
        private readonly ICommissionsService _commissionsService = Create.New<ICommissionsService>();

		[FunctionFilter("Commissions", "~/Accounts")]
		public virtual ActionResult Index()
		{
			var commissionPublish = new CommissionPublish();

			var systemEventSetting = commissionPublish.GetSystemEventSettings();
			bool commissionRunInProgress =
				commissionPublish.IsCommissionRunInProgress(systemEventSetting.PrepSystemEventApplicationId) ||
				commissionPublish.IsCommissionRunInProgress(systemEventSetting.PublishSystemEventApplicationId);

			var commissionRunPlans = commissionPublish.LoadByCommissionRunType(CommissionRunKind.Publish);
			var defaultCommissionRunPlan = commissionPublish.GetDefaultCommissionRunPlan(commissionRunPlans);

            var openPeriods = _commissionsService.GetOpenPeriods(defaultCommissionRunPlan);

			var model = new PublishModel(commissionRunPlans, openPeriods, commissionRunInProgress);

			return View(model);
		}

		[HttpPost]
		[FunctionFilter("Commissions-Run Commissions", "~/Commissions")]
		public virtual ActionResult ExecuteCommissionPublish(int planID, int periodID)
		{
			var commissionPublish = new CommissionPublish();
			var commissionRun = commissionPublish.ExecuteCommissionPublish(planID, periodID);
            commissionRun.OpenPeriods = _commissionsService.GetOpenPeriods((DisbursementFrequencyKind)commissionRun.PlanId);

			if (commissionRun.CommissionRunCanBePublished)
			{
				CoreContext.CurrentCommissionRun = commissionRun;
			}
			else
			{
				commissionRun.SystemEventLog.EventMessage = Translation.GetTerm("CommissionsPublishFailed", "Commissions Publish Failed: Prep Commissions Run must be successfully completed first");
				commissionRun.SystemEventLog.CreatedDate = DateTime.Now;
			}

			return Json(commissionRun.ToJson());
		}

		[HttpPost]
		[FunctionFilter("Commissions-Run Commissions", "~/Commissions")]
		public virtual ActionResult ExecuteCommissionPublishStep()
		{
			var commissionPublish = new CommissionPublish();
			var commissionRun = commissionPublish.ExecuteCommissionPublishStep(CoreContext.CurrentCommissionRun);

            commissionRun.OpenPeriods = _commissionsService.GetOpenPeriods((DisbursementFrequencyKind)commissionRun.PlanId);

			if (commissionRun.InProgress)
			{
				CoreContext.CurrentCommissionRun = commissionRun;
			}
			else
			{
				CoreContext.CurrentCommissionRun = null;
			}

			return Json(commissionRun.ToJson());
		}
	}
}
